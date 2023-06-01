using Alternet.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ControlsSample
{
    partial class WebBrowserPage : Control
    {
        private static int ScriptRunCounter = 0;
        private bool ScriptMessageHandlerAdded = false;
        private static WebBrowserBackend UseBackend = WebBrowserBackend.IELatest;
        private static bool SetLatestBackend = true;
        private static bool InitSchemesDone = false;
        private static readonly string ZipSchemeName = "zipfs";
        private static bool PandaInMemory = false;

        private IPageSite? site;
        private readonly WebBrowserFindParams FindParams = new();

        
        public void DoTestMessageHandler()
        {
            if(ScriptMessageHandlerAdded)
                DoRunScript("window.wx_msg.postMessage('This is a message body');");
        }
        
        public bool IsIEBackend()
        {
            return WebBrowser1.Backend == WebBrowserBackend.IE ||
                WebBrowser1.Backend == WebBrowserBackend.IELatest;
        }
        
        public void RestartWithEdge()
        {
            CommonUtils.RemoveFileWithExt("ie");
            CommonUtils.CreateFileWithExt("edge");
            Alternet.UI.Application.Current.Exit();
        }

        public void RestartWithIE()
        {
            CommonUtils.RemoveFileWithExt("edge");
            CommonUtils.CreateFileWithExt("ie");
            Alternet.UI.Application.Current.Exit();
        }

        public void DoTestIEShowPrintPreviewDialog()
        {
            if(IsIEBackend())
                WebBrowser1.DoCommand("IE.ShowPrintPreviewDialog");
        }
        
        private void InitSchemes(WebBrowser browser)
        {
            if (!IsIEBackend())
                return;

            if (InitSchemesDone)
                return;
            InitSchemesDone = true;
            browser.MemoryFS.Init("memory");
            browser.DoCommand("ZipScheme.Init", ZipSchemeName);
        }
        
        public void DoTestPandaFromMemory()
        {
            if (!IsIEBackend())
                return;

            void PandaToMemory()
            {
                if (PandaInMemory)
                    return;
                PandaInMemory = true;
                InitSchemes(WebBrowser1);
                WebBrowser1.MemoryFS.AddTextFile("index.html", "<html><body><b>index.html</b></body></html>");
                WebBrowser1.MemoryFS.AddTextFile("myFolder/index.html",
                    "<html><body><b>file in subfolder</b></body></html>");

                AddPandaFile("Html/page1.html");
                AddPandaFile("Html/page2.html");
                AddPandaFile("Images/panda1.jpg");
                AddPandaFile("Images/panda2.jpg");
                AddPandaFile("Html/default.js");
                AddPandaFile("Styles/default.css");

                void AddPandaFile(string name, string? mimeType = null)
                {
                    string sPath = CommonUtils.GetAppFolder() + "Html\\SampleArchive\\" + name;

                    if (mimeType == null)
                        WebBrowser1.MemoryFS.AddOSFile(name, sPath);
                    else
                        WebBrowser1.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }
            PandaToMemory();
            WebBrowser1.LoadURL("memory:Html/page1.html");
        }
        
        public void DoTestZip()
        {
            string arcSubPath = "Html\\SampleArchive.zip";
            string webPagePath = "root.html";

            InitSchemes(WebBrowser1);

            string archivePath = Path.Combine(CommonUtils.GetAppFolder(), arcSubPath);
            if (File.Exists(archivePath))
            {
                string url = CommonUtils.PrepareZipUrl(ZipSchemeName, archivePath, webPagePath);
                WebBrowser1.LoadURL(url);
            }
        }
        
        private void Test()
        {
            DoTestZip();
        }
        
        public static void SetBackendPathSmart(string folder)
        {
            var os = Environment.OSVersion;
            var platform = os.Platform;

            if (platform != PlatformID.Win32NT)
                return;

            var pa = "win-" + RuntimeInformation.ProcessArchitecture.ToString().ToLower();
            string edgePath = Path.Combine(folder, pa);

            WebBrowser.SetBackendPath(edgePath, true);
        }
        
        static WebBrowserPage()
        {

            SetBackendPathSmart("Edge");

            string[] commandLineArgs = Environment.GetCommandLineArgs();
            CommonUtils.ParseCmdLine(commandLineArgs);
            if (CommonUtils.CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);

            
            LogToFile("======================================");
            LogToFile("Application started");
            LogToFile("======================================");

            if (File.Exists(CommonUtils.GetFileWithExt("ie")))
            {
                UseBackend = WebBrowserBackend.IELatest;
                SetLatestBackend = false;
            }
            else
            {
                if (File.Exists(CommonUtils.GetFileWithExt("edge")))
                {
                    UseBackend = WebBrowserBackend.Edge;
                    SetLatestBackend = false;
                }
            }


            if (UseBackend==WebBrowserBackend.IE || UseBackend == WebBrowserBackend.IELatest
                || UseBackend == WebBrowserBackend.Edge)
            {
                if (WebBrowser.GetBackendOS() != WebBrowserBackendOS.Windows)
                    UseBackend = WebBrowserBackend.Default;
            }

            WebBrowser.SetBackend(UseBackend);
            if (SetLatestBackend)
                WebBrowser.SetLatestBackend();

        }
        
        public static void HookExceptionEvents(Alternet.UI.Application a)
        {
            if (!CommonUtils.CmdLineTest)
                return;
            a.ThreadException += Application_ThreadException;
            a.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += 
                CurrentDomain_UnhandledException;
        }
        
        private static string? HeaderText;
        public IPageSite? Site
        {
            get => site;

            set
            {
                HeaderText = HeaderLabel.Text;
                WebBrowser1.ZoomType = WebBrowserZoomType.Layout;
                ScriptMessageHandlerAdded = WebBrowser1.AddScriptMessageHandler("wx_msg");
                if(!ScriptMessageHandlerAdded)
                    Log("AddScriptMessageHandler not supported");
                FindParamsToControls();
                AddTestActions();
                if(IsIEBackend())
                    WebBrowser1.DoCommand("IE.SetScriptErrorsSuppressed","true");
                if (CommonUtils.CmdLineTest)
                {
                    ListBox1.Visible = true;
                    FindOptionsPanel.Visible = true;
                    FindClear.Visible = true;
                }

                site = value;
            }
        }
        
        private void FindParamsToControls()
        {
            FindWrapCheckBox.IsChecked = FindParams.Wrap;
            FindEntireWordCheckBox.IsChecked = FindParams.EntireWord;
            FindMatchCaseCheckBox.IsChecked = FindParams.MatchCase;
            FindHighlightResultCheckBox.IsChecked = FindParams.HighlightResult;
            FindBackwardsCheckBox.IsChecked = FindParams.Backwards;
        }
        
        private void FindParamsFromControls()
        {
            FindParams.Wrap = FindWrapCheckBox.IsChecked;
            FindParams.EntireWord = FindEntireWordCheckBox.IsChecked;
            FindParams.MatchCase = FindMatchCaseCheckBox.IsChecked;
            FindParams.HighlightResult = FindHighlightResultCheckBox.IsChecked;
            FindParams.Backwards = FindBackwardsCheckBox.IsChecked;
        }
        
        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = "";
            WebBrowser1.FindClearResult();
        }
        
        private void FindButton_Click(object sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = WebBrowser1.Find(FindTextBox.Text, FindParams);
            Log("Find Result = " + findResult.ToString());
        }
        
        private IntPtr GetNewClientData()
        {
            ScriptRunCounter++;
            IntPtr clientData = new(ScriptRunCounter);
            return clientData;
        }
        
        private void DoInvokeScript(string scriptName, params object?[] args)
        {
            var clientData = GetNewClientData();
            Log($"InvokeScript {scriptName}({WebBrowser1.ToInvokeScriptArgs(args)})");
            WebBrowser1.InvokeScriptAsync(scriptName, clientData, args);
        }
        
        private void DoRunScript(string script)
        {
            var clientData = GetNewClientData();
            Log($"RunScript {clientData} > {script}");
            WebBrowser1.RunScriptAsync(script, clientData);
        }
        
        public void DoTestInvokeScript()
        {
            DoInvokeScript("alert","hello");
        }
        
        public void DoTestInvokeScript3()
        {
            DoInvokeScript("alert", 16325.62901F);
            DoInvokeScript("alert", WebBrowser1.ToInvokeScriptArg(16325.62901F)?.ToString());
            DoInvokeScript("alert", System.DateTime.Now);
        }
        
        public void DoTestInvokeScript2()
        {
            DoInvokeScript("document.URL.toUpperCase");
        }
        
        public void DoTestRunScriptGetBrowser()
        {
            DoRunScript("get_browser()");
        }
        
        public void DoTestRunScript2()
        {
            DoRunScript("alert('hello');");
        }
        
        public static void DeleteLog()
        {
            if (File.Exists(MyLogFilePath))
                File.Delete(MyLogFilePath);
        }
        
        internal static readonly Destructor MyDestructor = new();
        public sealed class Destructor
        {
            ~Destructor()
            {
                LogToFile("===================");
                LogToFile("Application finished");
                LogToFile("===================");
            }
        }
        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = UrlTextBox.Text;

                LoadUrl(s);
            }
        }
        
        private void Log(string s)
        {
            LogToFile(s);
            site?.LogEvent(s);
        }
        
        static readonly string MyLogFilePath= Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");
        static readonly string[] StringSplitToArrayChars = { Environment.NewLine };
        public static void LogToFile(string s)
        {
            if (!CommonUtils.CmdLineTest && !CommonUtils.CmdLineLog)
                return;

            string dt = System.DateTime.Now.ToString(WebBrowser.StringFormatJs);
            string[] result = s.Split(StringSplitToArrayChars, StringSplitOptions.None);

            string contents = String.Empty;

            foreach (string s2 in result)
                contents += $"{dt} :: {s2}{Environment.NewLine}";
            File.AppendAllText(MyLogFilePath, contents);
        }
        
        public static void LogException(Exception e)
        {
            LogToFile("====== EXCEPTION:");
            LogToFile(e.ToString());
            LogToFile("======");
        }
        
        private static void HandleException(Exception e)
        {
            LogException(e);
        }
        
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }
        
        private static bool InsideUnhandledException;
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!InsideUnhandledException)
            {
                InsideUnhandledException = true;
                HandleException((e.ExceptionObject as Exception)!);
                InsideUnhandledException = false;
            }
        }
        
        private void LogWebBrowserEvent(WebBrowserEventArgs e)
        {
            Log("====== EVENT: " + e.EventType);

            if (!String.IsNullOrEmpty(e.Text))
                Log($"  Text = '{e.Text}'");
            if (!String.IsNullOrEmpty(e.Url))
                Log($"  Url = '{e.Url}'");
            if(!String.IsNullOrEmpty(e.TargetFrameName))
                Log($"  Target = '{e.TargetFrameName}'");
            if(e.NavigationAction!=0)
                Log($"  ActionFlags = {e.NavigationAction}");
            if (!String.IsNullOrEmpty(e.MessageHandler))
                Log($"  MessageHandler = '{e.MessageHandler}'");
            if(e.IsError)
                Log($"  IsError = {e.IsError}");
            if (e.NavigationError != null)
                Log($"  NavigationError = {e.NavigationError}");
            if(e.ClientData!=IntPtr.Zero)
                Log($"  ClientData = {e.ClientData}");
            Log("======");
        }
        
        private void WebBrowser1_BeforeBrowserCreate(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }
        
        private void WebBrowser1_FullScreenChanged(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }
        
        private void WebBrowser1_ScriptMessageReceived(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }
        
        private void WebBrowser1_ScriptResult(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }
        
        private void WebBrowser1_Navigated(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            UrlTextBox.Text = WebBrowser1.GetCurrentURL();
        }
        
        private static bool CanNavigate = true;
        private void WebBrowser1_Navigating(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            if (!CanNavigate)
                e.Cancel = true;
        }
        
        private static bool HistoryCleared = false;
        private static bool PandaLoaded = false;
        private void WebBrowser1_Loaded(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            UpdateHistoryButtons();

            if (!PandaLoaded)
            {
                PandaLoaded = true;
                try
                {
                    WebBrowser1.LoadURL(GetPandaUrl());
                }
                catch (Exception)
                {
                }
            }
        }
        
        private void WebBrowser1_Error(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }
        
        private void WebBrowser1_NewWindow(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            WebBrowser1.LoadURL(e.Url);
        }
        
        private void WebBrowser1_TitleChanged(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            var backendVersion = WebBrowser.GetBackendVersionString(WebBrowser1.Backend);
            if (this.IsIEBackend())
                backendVersion = "Internet Explorer";
            HeaderLabel.Text = HeaderText + " : " + e.Text + " : "+ backendVersion;
        }
        
        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }
        
        private void ShowBrowserVersion()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html") 
                + "version.html";
            WebBrowser1.LoadURL("file://" + filename.Replace('\\', '/'));
        }
        
        private void LoadUrl(string s) 
        {
            Log("==> LoadUrl: "+s);
            if (s == "i")
            {
                LogInfo("");
                return;
            }
            if (s == "l")
            {
                ListBox1.Visible = true;
                return;
            }
            if (s == "t")
            {
                Test();
                return;
            }
            if (s == "v")
            {
                ShowBrowserVersion();
                return;
            }
            if (s == "g")
            {
                WebBrowser1.LoadURL("www.google.com");
                return;
            }
            WebBrowser1.LoadURL(s);
        }
        
        void LogProp(object? obj, string propName, string? prefix=null)
        {
            var s = prefix;
            if (s != null)
                s += ".";

            PropertyInfo? propInfo = obj?.GetType().GetProperty(propName);
            if (propInfo == null)
                return;
            string? propValue = propInfo?.GetValue(obj)?.ToString();

            Log(s + propName + " = " + propValue);
        }
        
        void LogInfo(string s="")
        {
            Log("======="+s);


            LogProp(WebBrowser1, "HasSelection", "WebBrowser");
            LogProp(WebBrowser1, "SelectedText", "WebBrowser");
            LogProp(WebBrowser1, "SelectedSource", "WebBrowser");
            LogProp(WebBrowser1, "CanCut", "WebBrowser");
            LogProp(WebBrowser1, "CanCopy", "WebBrowser");
            LogProp(WebBrowser1, "CanPaste", "WebBrowser");
            LogProp(WebBrowser1, "CanUndo", "WebBrowser");
            LogProp(WebBrowser1, "CanRedo", "WebBrowser");
            LogProp(WebBrowser1, "IsBusy", "WebBrowser");
            LogProp(WebBrowser1, "PageSource", "WebBrowser");
            LogProp(WebBrowser1, "UserAgent", "WebBrowser");

            LogProp(WebBrowser1, "PageText", "WebBrowser");
            LogProp(WebBrowser1, "Zoom", "WebBrowser");
            LogProp(WebBrowser1, "Editable", "WebBrowser");
            LogProp(WebBrowser1, "ZoomType", "WebBrowser");
            LogProp(WebBrowser1, "ZoomFactor", "WebBrowser");

            Log("os = " + WebBrowser.GetBackendOS().ToString());
            Log("backend = " + WebBrowser1.Backend.ToString());
            Log("wxWidgetsVersion = " + WebBrowser.GetLibraryVersionString());
            Log("GetCurrentTitle() = " + WebBrowser1.GetCurrentTitle());
            Log("GetCurrentURL() = " + WebBrowser1.GetCurrentURL());
            Log("IE = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.IE));
            Log("Edge = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge));
            Log("WebKit = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit));

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                Log("Edge version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.Edge));
            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IE))
                Log("IE version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.IE));
            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
                Log("Webkit version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.WebKit));
            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Default))
                Log("Default browser version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.Default));

            Log("=======");
        }
        
        private void UpdateZoomButtons()
        {
            ZoomInButton.Enabled = WebBrowser1.CanZoomIn;
            ZoomOutButton.Enabled = WebBrowser1.CanZoomOut;
        }
        
        private void UpdateHistoryButtons()
        {
            if (!HistoryCleared)
            {
                HistoryCleared = true;
                WebBrowser1.ClearHistory();
            }

            BackButton.Enabled = WebBrowser1.CanGoBack;
            ForwardButton.Enabled = WebBrowser1.CanGoForward;
        }
        
        public void TestNavigateToString1()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }
        
        public void TestNavigateToString2()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 2</body></html>","www.aa.com");
        }
        
        public void TestNavigateToStream()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            WebBrowser1.NavigateToStream(stream);
        }
        
        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomIn();
            UpdateZoomButtons();
        }
        
        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomOut();
            UpdateZoomButtons();
        }
        
        private void BackButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoBack();
        }
        
        private void ForwardButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoForward();
        }
        
        public WebBrowserPage()
        {
            var myListener = new DebugTraceListener();
            Trace.Listeners.Add(myListener);
            InitializeComponent();
        }
        
        private void ListBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox)sender;

            int? index = listbox.SelectedIndex;
            if (index == null)
                return;

            string? name = listbox.Items[(int)index].ToString();

            if (!TestActions.TryGetValue(name!, out MethodCaller? action))
                return;
            Log("DoAction: " + name);
            action.DoCall();
        }
        
        private void AddTestAction()
        {
            AddTestAction(null, new MethodCaller());
        }
        
        private void AddTestAction(string? name = null, Action? action = null) 
        {
            AddTestAction(name, new MethodCaller(this, action));
        }
        
        private readonly Dictionary<string, MethodCaller> TestActions = new();
        private void AddTestAction(string? name=null, MethodCaller? action = null) 
        {
            if (name == null)
            {
                ListBox1.Items.Add("----");
                return;
            }

            TestActions.Add(name, action!);
            ListBox1.Items.Add(name);
        }
        
        private string GetPandaFileName()
        {
            return CommonUtils.GetAppFolder() + "Html\\SampleArchive\\Html\\page1.html";
        }
        
        private string GetPandaUrl()
        {
            return CommonUtils.PrepareFileUrl(GetPandaFileName());
        }
        
        private void AddTestActions()
        {
            AddTestAction("Open Panda sample", () => { 
                WebBrowser1.LoadURL(GetPandaUrl()); });
            AddTestAction("Google", () => { WebBrowser1.LoadURL(
                "https://www.google.com"); });
            AddTestAction("BrowserVersion", () => { ShowBrowserVersion(); });
            AddTestAction("RestartWithIE", () => { RestartWithIE(); });
            AddTestAction("RestartWithEdge", () => { RestartWithEdge(); });
            AddTestAction();
            AddTestAction("Info", () => { LogInfo(); });
            AddTestAction("Test", () => { Test(); });
            AddTestAction("TestErrorEvent", () => { WebBrowser1.LoadURL("memoray:myFolder/indeax.html"); });
            AddTestAction();
            AddTestAction("Stop", () => { WebBrowser1.Stop(); });
            AddTestAction("ClearHistory", () => { WebBrowser1.ClearHistory(); });
            AddTestAction("Reload", () => { WebBrowser1.Reload(); });
            AddTestAction("Reload(true)", () => { WebBrowser1.Reload(true); });
            AddTestAction("Reload(false)", () => { WebBrowser1.Reload(false); });
            AddTestAction("SelectAll", () => { WebBrowser1.SelectAll(); });
            AddTestAction("DeleteSelection", () => { WebBrowser1.DeleteSelection(); });
            AddTestAction("ClearSelection", () => { WebBrowser1.ClearSelection(); });
            AddTestAction("Cut", () => { WebBrowser1.Cut(); });
            AddTestAction("Copy", () => { WebBrowser1.Copy(); });
            AddTestAction("Paste", () => { WebBrowser1.Paste(); });
            AddTestAction("Undo", () => { WebBrowser1.Undo(); });
            AddTestAction("Redo", () => { WebBrowser1.Redo(); });
            AddTestAction("Print", () => { WebBrowser1.Print(); });
            AddTestAction("DeleteLog", () => { DeleteLog(); });
            AddTestAction("ZoomFactor+", () => { WebBrowser1.ZoomFactor += 1; });
            AddTestAction("ZoomFactor-", () => { WebBrowser1.ZoomFactor -= 1; });
            AddTestAction();
            AddTestAction("CanNavigate=false", () => { CanNavigate=false; });
            AddTestAction("CanNavigate=true", () => { CanNavigate = true; });
            AddTestAction("ContextMenuEnabled=true", () => { WebBrowser1.ContextMenuEnabled = true; });
            AddTestAction("ContextMenuEnabled=false", () => { WebBrowser1.ContextMenuEnabled = false; });
            AddTestAction("Editable=true", () => { WebBrowser1.Editable = true; });
            AddTestAction("Editable=false", () => { WebBrowser1.Editable = false; });
            AddTestAction("AccessToDevToolsEnabled=true", () => { WebBrowser1.AccessToDevToolsEnabled = true; });
            AddTestAction("AccessToDevToolsEnabled=false", () => { WebBrowser1.AccessToDevToolsEnabled = false; });
            AddTestAction("EnableHistory=true", () => { WebBrowser1.EnableHistory(true); });
            AddTestAction("EnableHistory=false", () => { WebBrowser1.EnableHistory(false); });
            AddTestAction();

            var methods = GetType().GetRuntimeMethods();
            foreach (var item in methods)
            {
                if (!item.Name.StartsWith("DoTest"))
                    continue;

                MethodCaller mc = new(this,item);

                AddTestAction(item.Name, mc);
            }

        }
        
        private class MethodCaller 
        {
            
            public MethodInfo? Method;
            public object? Parent;
            public Action? Action;
            
            public void DoCall()
            {
                if (Action != null)
                {
                    Action();
                    return;
                }

                object[] prm = new object[0];
                Method?.Invoke(Parent,prm);
            }
            
            public MethodCaller()
            {

            }
            
            public MethodCaller(object? parent, Action? action) 
            {
                Parent = parent;
                Action = action;
            }
            
            public MethodCaller(object? parent, MethodInfo? methodinfo)
            {
                Parent = parent;
                Method = methodinfo;
            }
            
        }
       
        internal void DoTestRunScriptString()
        {
            DoRunScript("function f(a){return a;}f('Hello World!');");
        }
        
        internal void DoTestRunScriptInteger()
        {
            DoRunScript("function f(a){return a;}f(123);");
        }
        
        internal void DoTestRunScriptDouble()
        {
            DoRunScript("function f(a){return a;}f(2.34);");
        }
        
        internal void DoTestRunScriptBool()
        {
            DoRunScript("function f(a){return a;}f(false);");
        }
        
        internal void DoTestRunScriptObject()
        {
            DoRunScript("function f(){var person = new Object();person.name = 'Foo'; " +
                "person.lastName = 'Bar'; return person;}f();");
        }
        
        internal void DoTestRunScriptArray()
        {
            DoRunScript("function f(){ return [\"foo\", \"bar\"]; }f();");
        }
        
        internal void DoTestRunScriptDOM()
        {
            DoRunScript("document.write(\"Hello World!\");");
        }
        
        internal void DoTestRunScriptUndefined()
        {
            DoRunScript("function f(){var person = new Object();}f();");
        }
        
        internal void DoTestRunScriptNull()
        {
            DoRunScript("function f(){return null;}f();");
        }
        
        internal void DoTestRunScriptDate()
        {
            DoRunScript("function f(){var d = new Date('10/08/2017 21:30:40');" +
                "var tzoffset = d.getTimezoneOffset() * 60000;" +
                "return new Date(d.getTime() - tzoffset);}f();");
        }

        internal void DoTestAddUserScript()
        {
            WebBrowser1.RemoveAllUserScripts();
            WebBrowser1.AddUserScript("alert('hello');");
            WebBrowser1.Reload();
        }

        internal void DoTestSerializeObject()
        {
            WeatherForecast value = new();
            value.Date = System.DateTime.Now;
            value.TemperatureCelsius = 15;
            value.Summary = "New York";

            var s = WebBrowser1.ObjectToJSON(value);

            Log(s);
        }
    }
    
    public class WeatherForecast
    {
        public System.DateTime Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
        public string? NullText { get; set; }
    }
}

