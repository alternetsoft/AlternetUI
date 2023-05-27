using Alternet.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;


namespace ControlsSample
{
    partial class WebBrowserPage : Control
    {
        private IPageSite? site;
        private readonly WebBrowserFindParams FindParams = new();

        //-------------------------------------------------
        public void DoTestIEShowPrintPreviewDialog()
        {
            if(WebBrowser1.Backend == WebBrowserBackend.IE || WebBrowser1.Backend == WebBrowserBackend.IELatest)
                WebBrowser1.DoCommand("IE.ShowPrintPreviewDialog");
        }
        //-------------------------------------------------
        private static bool PandaInMemory = false;
        public void DoTestPandaFromMemory()
        {
            void PandaToMemory()
            {
                if (PandaInMemory)
                    return;
                PandaInMemory = true;
                WebBrowser1.MemoryFS.Init("memory");
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
                    string sPath = GetAppFolder() + "Html\\SampleArchive\\" + name;

                    if (mimeType == null)
                        WebBrowser1.MemoryFS.AddOSFile(name, sPath);
                    else
                        WebBrowser1.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }
            PandaToMemory();
            WebBrowser1.LoadURL("memory:Html/page1.html");
        }
        //-------------------------------------------------
        private void Test()
        {
            DoTestPandaFromMemory();
            /*
            string schemeName = "zip";
            string archivePath = Path.Combine(GetAppFolder(), "SampleArchive.zip");
            archivePath = "c:\\SampleArchive.zip";
            string url = PrepareZipUrl(schemeName,archivePath, "topic0.htm");
            WebBrowser1.LoadURL("zip:///c:\\SampleArchive.zip;protocol=zip/topic0.htm");
            //scheme:///C:/example/docs.zip;protocol=zip/main.htm
            */
        }
        //-------------------------------------------------
        static WebBrowserPage()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            ParseCmdLine(commandLineArgs);
            if (CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);

            
            LogToFile("===================");
            LogToFile("Application started");
            LogToFile("===================");

            //WebBrowser.SetDefaultPage("google.com");

            WebBrowser.SetLatestBackend();
            //WebBrowser.SetBackend(WebBrowserBackend.Edge);

        }
        //-------------------------------------------------
        public static void HookExceptionEvents(Alternet.UI.Application a)
        {
            if (!CmdLineTest)
                return;
            a.ThreadException += Application_ThreadException;
            a.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        //-------------------------------------------------
        public IPageSite? Site
        {
            get => site;

            set
            {
                WebBrowser1.ZoomType = WebBrowserZoomType.Layout;
                FindParamsToControls();
                AddTestActions();
                if (CmdLineTest)
                {
                    ListBox1.Visible = true;
                }
                site = value;
            }
        }
        //-------------------------------------------------
        private void FindParamsToControls()
        {
            FindWrapCheckBox.IsChecked = FindParams.Wrap;
            FindEntireWordCheckBox.IsChecked = FindParams.EntireWord;
            FindMatchCaseCheckBox.IsChecked = FindParams.MatchCase;
            FindHighlightResultCheckBox.IsChecked = FindParams.HighlightResult;
            FindBackwardsCheckBox.IsChecked = FindParams.Backwards;
        }
        //-------------------------------------------------
        private void FindParamsFromControls()
        {
            FindParams.Wrap = FindWrapCheckBox.IsChecked;
            FindParams.EntireWord = FindEntireWordCheckBox.IsChecked;
            FindParams.MatchCase = FindMatchCaseCheckBox.IsChecked;
            FindParams.HighlightResult = FindHighlightResultCheckBox.IsChecked;
            FindParams.Backwards = FindBackwardsCheckBox.IsChecked;
        }
        //-------------------------------------------------
        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = "";
            WebBrowser1.FindClearResultSelection();
        }
        //-------------------------------------------------
        private void FindButton_Click(object sender, EventArgs e)
        {
            FindParamsFromControls();
            long findResult = WebBrowser1.Find(FindTextBox.Text, FindParams);
            Log("Find Result = " + findResult.ToString());
        }
        //-------------------------------------------------
        /*public string PrepareZipUrl(string schemeName,string arcPath,string fileInArchivePath)
        {
            string prepareUrl(string s)
            {
                s = s.Replace('\\', '/')
                    .Replace(":", "%3A")
                    .Replace(" ", "%20");
                return s;
            }
            string url = schemeName+":///" + prepareUrl(arcPath) + ";protocol=zip/" + 
                prepareUrl(fileInArchivePath);
            return url;
        }*/
        //-------------------------------------------------
        public void TestMessageHandler()
        {
            /*
            m_webView->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, [](wxWebViewEvent& evt) {
                wxLogMessage("Script message received; value = %s, handler = %s", 
            evt.GetString(), evt.GetMessageHandler());
            }); 

             */

            if (!WebBrowser1.AddScriptMessageHandler("wx_msg"))
            {
                Log("AddScriptMessageHandler not supported");
                return;
            }
            WebBrowser1.RunScript("window.wx_msg.postMessage('This is a message body');");
            WebBrowser1.RemoveScriptMessageHandler("wx_msg");
        }
        //-------------------------------------------------
        public void DoTestInvokeScript()
        {
            WebBrowser1.InvokeScript("alert","hello");
        }
        //-------------------------------------------------
        public void DoDoTestInvokeScript3()
        {
            WebBrowser1.InvokeScript("alert", 16325.62901F);
            WebBrowser1.InvokeScript("alert", WebBrowser1.ToInvokeScriptArg(16325.62901F)?.ToString());
            WebBrowser1.InvokeScript("alert", System.DateTime.Now);
        }
        //-------------------------------------------------
        public void DoTestInvokeScript2()
        {
            Log("js result = " + WebBrowser1.InvokeScript("document.URL.toUpperCase"));
        }
        //-------------------------------------------------
        public void DoTestRunScript()
        {
            Log("js result = " + WebBrowser1.RunScript("get_browser()"));
        }
        //-------------------------------------------------
        public void DoTestRunScript2()
        {
            WebBrowser1.RunScript("alert('hello');");
        }
        //-------------------------------------------------
        public static void DeleteLog()
        {
            if (File.Exists(MyLogFilePath))
                File.Delete(MyLogFilePath);
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = UrlTextBox.Text;

                LoadUrl(s);
            }
        }
        //-------------------------------------------------
        private void Log(string s)
        {
            LogToFile(s);
            site?.LogEvent(s);
        }
        //-------------------------------------------------
        static readonly string MyLogFilePath= Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");
        static readonly string[] StringSplitToArrayChars = { Environment.NewLine };
        public static void LogToFile(string s)
        {
            if (!CmdLineLog)
                return;

            string dt = System.DateTime.Now.ToString(WebBrowser.StringFormatJs);
            string[] result = s.Split(StringSplitToArrayChars, StringSplitOptions.None);

            string contents = String.Empty;

            foreach (string s2 in result)
                contents += $"{dt} :: {s2}{Environment.NewLine}";
            File.AppendAllText(MyLogFilePath, contents);
        }
        //-------------------------------------------------
        public static void LogException(Exception e)
        {
            LogToFile("====== EXCEPTION:");
            LogToFile(e.ToString());
            LogToFile("======");
        }
        //-------------------------------------------------
        private static void HandleException(Exception e)
        {
            LogException(e);
        }
        //-------------------------------------------------
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
        private void LogWebBrowserEvent(string ev, WebBrowserEventArgs e)
        {
            Log("====== EVENT: " + ev);

            if (!String.IsNullOrEmpty(e.Text))
                Log($"  Text = '{e.Text}'");
            if (!String.IsNullOrEmpty(e.Url))
                Log($"  Url = '{e.Url}'");
            if(!String.IsNullOrEmpty(e.Target))
                Log($"  Target = '{e.Target}'");
            if(e.NavigationAction!=0)
                Log($"  ActionFlags = {e.NavigationAction}");
            if (!String.IsNullOrEmpty(e.MessageHandler))
                Log($"  MessageHandler = '{e.MessageHandler}'");
            if(e.IsError)
                Log($"  IsError = {e.IsError}");
            if (e.NavigationError != null)
                Log($"  NavigationError = {e.NavigationError}");
            Log("======");
        }
        //-------------------------------------------------
        private void WebBrowser1_FullScreenChanged(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("FullScreenChanged",e);
        }
        //-------------------------------------------------
        private void WebBrowser1_ScriptMessageReceived(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("ScriptMessageReceived",e);
        }
        //-------------------------------------------------
        private void WebBrowser1_ScriptResult(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("ScriptResult",e);
        }
        //-------------------------------------------------
        private void WebBrowser1_Navigated(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("Navigated",e);
            UrlTextBox.Text = WebBrowser1.GetCurrentURL();
        }
        //-------------------------------------------------
        private static bool CanNavigate = true;
        private void WebBrowser1_Navigating(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("Navigating",e);
            if (!CanNavigate)
                e.Cancel = true;
        }
        //-------------------------------------------------
        private static bool HistoryCleared = false;
        private static bool PandaLoaded = false;
        private void WebBrowser1_Loaded(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("Loaded",e);
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
        //-------------------------------------------------
        private void WebBrowser1_Error(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("Error",e);
        }
        //-------------------------------------------------
        private void WebBrowser1_NewWindow(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("NewWindow",e);
        }
        //-------------------------------------------------
        private void WebBrowser1_TitleChanged(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent("TitleChanged",e);
        }
        //-------------------------------------------------
        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }
        //-------------------------------------------------
        private void ShowBrowserVersion()
        {
            var filename = PathAddBackslash(GetAppFolder() + "Html") + "version.html";
            WebBrowser1.LoadURL("file://" + filename.Replace('\\', '/'));
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        private void UpdateZoomButtons()
        {
            ZoomInButton.Enabled = WebBrowser1.CanZoomIn;
            ZoomOutButton.Enabled = WebBrowser1.CanZoomOut;
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
        public void TestNavigateToString1()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }
        //-------------------------------------------------
        public void TestNavigateToString2()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 2</body></html>","www.aa.com");
        }
        //-------------------------------------------------
        public void TestNavigateToStream()
        {
            var filename = PathAddBackslash(GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            WebBrowser1.NavigateToStream(stream);
        }
        //-------------------------------------------------
        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomIn();
            UpdateZoomButtons();
        }
        //-------------------------------------------------
        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomOut();
            UpdateZoomButtons();
        }
        //-------------------------------------------------
        private void BackButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoBack();
        }
        //-------------------------------------------------
        private void ForwardButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoForward();
        }
        //-------------------------------------------------
        public WebBrowserPage()
        {
            InitializeComponent();
        }
        //-------------------------------------------------
        private void ListBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox)sender;

            int? index = listbox.SelectedIndex;
            if (index == null)
                return;

            string? name = listbox.Items[(int)index].ToString();

            if (!TestActions.TryGetValue(name!, out Action? action))
                return;
            Log("DoAction: " + name);
            action();
        }
        //-------------------------------------------------
        private readonly Dictionary<string, Action> TestActions = new();
        private void AddTestAction(string? name=null, Action? action=null) 
        {
            if (name == null)
            {
                ListBox1.Items.Add("----");
                return;
            }

            TestActions.Add(name, action!);
            ListBox1.Items.Add(name);
        }
        //-------------------------------------------------
        string PrepareUrl(string s)
        {
            s = s.Replace('\\', '/')
                .Replace(":", "%3A")
                .Replace(" ", "%20");
            return s;
        }
        //-------------------------------------------------
        public string PrepareFileUrl(string filename)
        {
            string url = "file:///" + PrepareUrl(filename);
            return url;
        }
        //-------------------------------------------------
        private string GetPandaFileName()
        {
            return GetAppFolder() + "Html\\SampleArchive\\Html\\page1.html";
        }
        //-------------------------------------------------
        private string GetPandaUrl()
        {
            return PrepareFileUrl(GetPandaFileName());
        }
        //-------------------------------------------------
        private void AddTestActions()
        {
            AddTestAction("Open Panda sample", () => { WebBrowser1.LoadURL(GetPandaUrl()); });
            AddTestAction("Google", () => { WebBrowser1.LoadURL("www.google.com"); });
            AddTestAction("BrowserVersion", () => { ShowBrowserVersion(); });
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
                
                AddTestAction(item.Name, () => { });
            }

        }
        //-------------------------------------------------
        public static string PathAddBackslash(string? path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            path = path.TrimEnd();
            if (PathEndsWithDirectorySeparator())
                return path;
            return path + GetDirectorySeparatorUsedInPath();
            
            char GetDirectorySeparatorUsedInPath()
            {
                if (path.Contains(Path.AltDirectorySeparatorChar))
                    return Path.AltDirectorySeparatorChar;
                return Path.DirectorySeparatorChar;
            }
            
            bool PathEndsWithDirectorySeparator()
            {
                if (path.Length == 0)
                    return false;
                char c = path[path.Length - 1];
                if (c != Path.DirectorySeparatorChar)
                    return c == Path.AltDirectorySeparatorChar;
                return true;
            }
        }
        //-------------------------------------------------
        public static bool CmdLineTest = false;
        public static bool CmdLineLog = false;
        public static bool CmdLineNoMfcDedug = false;
        public static string? CmdLineExecCommands = null;
        public static void ParseCmdLine(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string text = args[i];

                fn("-log",out CmdLineLog);
                fn("-nomfcdebug", out CmdLineNoMfcDedug);
                fn("-test", out CmdLineTest);

                if (text.StartsWith("-r=", StringComparison.CurrentCultureIgnoreCase))
                    CmdLineExecCommands = text.Substring("-r=".Length).Trim();
            }

            static void fn(string strFlag,out bool boolFlag)
            {
                boolFlag = (strFlag.ToLower() == strFlag);
            }
        }
        //-------------------------------------------------
        public static string GetAppFolder()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string s = Path.GetDirectoryName(location)!;
            return PathAddBackslash(s);
        }
        //-------------------------------------------------
        public static string StringFromStream(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }
        //-------------------------------------------------
        public static string StringFromFile(string filename)
        {
            using FileStream stream = File.OpenRead(filename);
            return StringFromStream(stream);
        }
        //-------------------------------------------------
        public static string ResNamePrefix = "ControlsSample.";
        public static Stream GetMyResourceStream(string resName)
        {
            Stream stream = ProcessResPrefix("rsrc://")!;
            if (stream != null)
                return stream;
            if (resName.StartsWith("file://"))
                resName = resName.Remove(0, "file://".Length);
            
            resName = Path.Combine(GetAppFolder(), resName);
            return File.OpenRead(resName);

            Stream? ProcessResPrefix(string resPrefix)
            {
                if (resName.StartsWith(resPrefix))
                {
                    resName = resName.Remove(0, resPrefix.Length);
                    resName = resName.Replace('/', '.');
                    string name = ResNamePrefix + resName;
                    return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                }
                return null;
            }
        }
        //-------------------------------------------------

    }
}