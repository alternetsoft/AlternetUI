using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class WebBrowserTestPage : Control
    {
        private static readonly string ZipSchemeName = "zipfs";
        private static readonly Destructor MyDestructor = new ();

        private static bool canNavigate = true;
        private static bool historyCleared = false;
        private static bool pandaLoaded = false;
        private static string? headerText;
        private static bool insideUnhandledException;
        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;
        private static int scriptRunCounter = 0;
        private static bool pandaInMemory = false;

        private readonly WebBrowserFindParams findParams = new ();
        private readonly Dictionary<string, MethodCaller> testActions = new ();
        private bool scriptMessageHandlerAdded = false;
        private ITestPageSite? site;

        static WebBrowserTestPage()
        {
            WebBrowser.SetDefaultFSNameMemory("memory");
            WebBrowser.SetDefaultFSNameArchive(ZipSchemeName);
            WebBrowser.SetDefaultUserAgent("Mozilla");

            SetBackendPathSmart("Edge");

            string[] commandLineArgs = Environment.GetCommandLineArgs();
            CommonTestUtils.ParseCmdLine(commandLineArgs);
            if (CommonTestUtils.CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);

            CommonTestUtils.LogToFile("======================================");
            CommonTestUtils.LogToFile("Application started");
            CommonTestUtils.LogToFile("======================================");

            if (File.Exists(CommonTestUtils.GetFileWithExt("ie")))
                useBackend = WebBrowserBackend.IELatest;
            else
                if (File.Exists(CommonTestUtils.GetFileWithExt("edge")))
                useBackend = WebBrowserBackend.Edge;

            if (useBackend == WebBrowserBackend.IE || useBackend == WebBrowserBackend.IELatest
                || useBackend == WebBrowserBackend.Edge)
            {
                if (WebBrowser.GetBackendOS() != WebBrowserBackendOS.Windows)
                    useBackend = WebBrowserBackend.Default;
            }

            WebBrowser.SetBackend(useBackend);
        }

        public WebBrowserTestPage()
        {
            var myListener = new CommonTestUtils.DebugTraceListener();
            Trace.Listeners.Add(myListener);
            InitializeComponent();
        }

        public ITestPageSite? Site
        {
            get => site;

            set
            {
                headerText = HeaderLabel.Text;
                WebBrowser1.ZoomType = WebBrowserZoomType.Layout;
                scriptMessageHandlerAdded = WebBrowser1.AddScriptMessageHandler("wx_msg");
                if (!scriptMessageHandlerAdded)
                    Log("AddScriptMessageHandler not supported");
                FindParamsToControls();
                AddTestActions();
                if (IsIEBackend())
                    WebBrowser1.DoCommand("IE.SetScriptErrorsSuppressed", "true");
                if (CommonTestUtils.CmdLineTest)
                {
                    ListBox1.Visible = true;
                    FindOptionsPanel.Visible = true;
                    FindClear.Visible = true;
                }

                site = value;
            }
        }

        public static void HookExceptionEvents(Alternet.UI.Application a)
        {
            if (!CommonTestUtils.CmdLineTest)
                return;
            a.ThreadException += Application_ThreadException;
            a.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException;
        }

        private static void HandleException(Exception e)
        {
            CommonTestUtils.LogException(e);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!insideUnhandledException)
            {
                insideUnhandledException = true;
                HandleException((e.ExceptionObject as Exception)!);
                insideUnhandledException = false;
            }
        }

        private static void SetBackendPathSmart(string folder)
        {
            var os = Environment.OSVersion;
            var platform = os.Platform;

            if (platform != PlatformID.Win32NT)
                return;

            var pa = "win-" + RuntimeInformation.ProcessArchitecture.ToString().ToLower();
            string edgePath = Path.Combine(folder, pa);

            WebBrowser.SetBackendPath(edgePath, true);
        }

        private void DoTestMessageHandler()
        {
            if (scriptMessageHandlerAdded)
                DoRunScript("window.wx_msg.postMessage('This is a message body');");
        }

        private bool IsIEBackend()
        {
            return WebBrowser1.Backend == WebBrowserBackend.IE ||
                WebBrowser1.Backend == WebBrowserBackend.IELatest;
        }

        private void RestartWithDefault()
        {
            CommonTestUtils.RemoveFileWithExt("ie");
            CommonTestUtils.RemoveFileWithExt("edge");
            Alternet.UI.Application.Current.Exit();
        }

        private void RestartWithEdge()
        {
            CommonTestUtils.RemoveFileWithExt("ie");
            CommonTestUtils.CreateFileWithExt("edge");
            Alternet.UI.Application.Current.Exit();
        }

        private void RestartWithIE()
        {
            CommonTestUtils.RemoveFileWithExt("edge");
            CommonTestUtils.CreateFileWithExt("ie");
            Alternet.UI.Application.Current.Exit();
        }

        private void DoTestIEShowPrintPreviewDialog()
        {
            if (IsIEBackend())
                WebBrowser1.DoCommand("IE.ShowPrintPreviewDialog");
        }

        private void DoTestPandaFromMemory()
        {
            if (WebBrowser1.Backend == WebBrowserBackend.Edge)
            {
                Log("Memory scheme not supported in Edge backend");
                return;
            }

            void PandaToMemory()
            {
                if (pandaInMemory)
                    return;
                pandaInMemory = true;
                WebBrowser1.MemoryFS.AddTextFile(
                    "index.html",
                    "<html><body><b>index.html</b></body></html>");
                WebBrowser1.MemoryFS.AddTextFile(
                    "myFolder/index.html",
                    "<html><body><b>file in subfolder</b></body></html>");

                AddPandaFile("Html/page1.html");
                AddPandaFile("Html/page2.html");
                AddPandaFile("Images/panda1.jpg");
                AddPandaFile("Images/panda2.jpg");
                AddPandaFile("Html/default.js");
                AddPandaFile("Styles/default.css");

                void AddPandaFile(string name, string? mimeType = null)
                {
                    string sPath = CommonTestUtils.GetAppFolder() + "Html\\SampleArchive\\" + name;

                    if (mimeType == null)
                        WebBrowser1.MemoryFS.AddOSFile(name, sPath);
                    else
                        WebBrowser1.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }

            PandaToMemory();
            WebBrowser1.LoadURL("memory:Html/page1.html");
        }

        private void DoTestZip()
        {
            if (WebBrowser1.Backend == WebBrowserBackend.Edge)
            {
                Log("Archive scheme not supported in Edge backend");
                return;
            }

            string arcSubPath = "Html\\SampleArchive.zip";
            string webPagePath = "root.html";

            string archivePath = Path.Combine(CommonTestUtils.GetAppFolder(), arcSubPath);
            if (File.Exists(archivePath))
            {
                string url = CommonTestUtils.PrepareZipUrl(ZipSchemeName, archivePath, webPagePath);
                WebBrowser1.LoadURL(url);
            }
        }

        private void Test()
        {
            DoTestZip();
        }

        private void FindParamsToControls()
        {
            FindWrapCheckBox.IsChecked = findParams.Wrap;
            FindEntireWordCheckBox.IsChecked = findParams.EntireWord;
            FindMatchCaseCheckBox.IsChecked = findParams.MatchCase;
            FindHighlightResultCheckBox.IsChecked = findParams.HighlightResult;
            FindBackwardsCheckBox.IsChecked = findParams.Backwards;
        }

        private void FindParamsFromControls()
        {
            findParams.Wrap = FindWrapCheckBox.IsChecked;
            findParams.EntireWord = FindEntireWordCheckBox.IsChecked;
            findParams.MatchCase = FindMatchCaseCheckBox.IsChecked;
            findParams.HighlightResult = FindHighlightResultCheckBox.IsChecked;
            findParams.Backwards = FindBackwardsCheckBox.IsChecked;
        }

        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = string.Empty;
            WebBrowser1.FindClearResult();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = WebBrowser1.Find(FindTextBox.Text, findParams);
            Log("Find Result = " + findResult.ToString());
        }

        private IntPtr GetNewClientData()
        {
            scriptRunCounter++;
            IntPtr clientData = new (scriptRunCounter);
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

        private void DoTestInvokeScript()
        {
            DoInvokeScript("alert", "hello");
        }

        private void DoTestInvokeScript3()
        {
            DoInvokeScript("alert", 16325.62901F);
            DoInvokeScript("alert", WebBrowser1.ToInvokeScriptArg(16325.62901F)?.ToString());
            DoInvokeScript("alert", System.DateTime.Now);
        }

        private void DoTestInvokeScript2()
        {
            DoInvokeScript("document.URL.toUpperCase");
        }

        private void DoTestRunScriptGetBrowser()
        {
            DoRunScript("get_browser()");
        }

        private void DoTestRunScript2()
        {
            DoRunScript("alert('hello');");
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
            CommonTestUtils.LogToFile(s);
            site?.LogEvent(s);
        }

        private void LogWebBrowserEvent(WebBrowserEventArgs e)
        {
            Log("====== EVENT: " + e.EventType);

            if (!string.IsNullOrEmpty(e.Text))
                Log($"  Text = '{e.Text}'");
            if (!string.IsNullOrEmpty(e.Url))
                Log($"  Url = '{e.Url}'");
            if (!string.IsNullOrEmpty(e.TargetFrameName))
                Log($"  Target = '{e.TargetFrameName}'");
            if (e.NavigationAction != 0)
                Log($"  ActionFlags = {e.NavigationAction}");
            if (!string.IsNullOrEmpty(e.MessageHandler))
                Log($"  MessageHandler = '{e.MessageHandler}'");
            if (e.IsError)
                Log($"  IsError = {e.IsError}");
            if (e.NavigationError != null)
                Log($"  NavigationError = {e.NavigationError}");
            if (e.ClientData != IntPtr.Zero)
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

        private void WebBrowser1_Navigating(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            if (!canNavigate)
                e.Cancel = true;
        }

        private void WebBrowser1_Loaded(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            UpdateHistoryButtons();

            if (!pandaLoaded)
            {
                pandaLoaded = true;
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
            LoadUrl(e.Url);
        }

        private void WebBrowser1_TitleChanged(object sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            var backendVersion = WebBrowser.GetBackendVersionString(WebBrowser1.Backend);
            if (this.IsIEBackend())
                backendVersion = "Internet Explorer";
            HeaderLabel.Text = headerText + " : " + e.Text + " : "
                + backendVersion;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }

        private void ShowBrowserVersion()
        {
            var filename = CommonTestUtils.PathAddBackslash(CommonTestUtils.GetAppFolder() + "Html")
                + "version.html";
            WebBrowser1.LoadURL("file://" + filename.Replace('\\', '/'));
        }

        private void LoadUrl(string? s)
        {
            if (s == null)
            {
                WebBrowser1.LoadURL();
                return;
            }

            if (s == "i")
            {
                LogInfo(string.Empty);
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
                s = "https://www.google.com";

            var url = WebBrowser.GetCorrectUri(s).ToString();

            Log("==> LoadUrl: " + url);
            WebBrowser1.LoadURL(url);
        }

        private void LogProp(object? obj, string propName, string? prefix = null)
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

        private void LogInfo(string s = "")
        {
            Log("=======" + s);

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

            Log("isDebug = " + WebBrowser.DoCommandGlobal("IsDebug"));
            Log("os = " + WebBrowser.GetBackendOS().ToString());
            Log("backend = " + WebBrowser1.Backend.ToString());
            Log("wxWidgetsVersion = " + WebBrowser.GetLibraryVersionString());
            Log("GetCurrentTitle() = " + WebBrowser1.GetCurrentTitle());
            Log("GetCurrentURL() = " + WebBrowser1.GetCurrentURL());
            Log("IE = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.IE));
            Log("Edge = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge));
            Log("WebKit = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit));
            Log("GetUsefulDefines" + WebBrowser.DoCommandGlobal("GetUsefulDefines"));

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
            if (!historyCleared)
            {
                historyCleared = true;
                WebBrowser1.ClearHistory();
            }

            BackButton.Enabled = WebBrowser1.CanGoBack;
            ForwardButton.Enabled = WebBrowser1.CanGoForward;
        }

        private void TestNavigateToString1()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }

        private void TestNavigateToString2()
        {
            WebBrowser1.NavigateToString("<html><body>NavigateToString example 2</body></html>", "www.aa.com");
        }

        private void TestNavigateToStream()
        {
            var filename = CommonTestUtils.PathAddBackslash(CommonTestUtils.GetAppFolder() + "Html") + "version.html";
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

        private void ListBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox)sender;

            int? index = listbox.SelectedIndex;
            if (index == null)
                return;

            string? name = listbox.Items[(int)index].ToString();

            if (!testActions.TryGetValue(name!, out MethodCaller? action))
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

        private void AddTestAction(
            string? name = null,
            MethodCaller? action = null)
        {
            if (name == null)
            {
                ListBox1.Items.Add("----");
                return;
            }

            testActions.Add(name, action!);
            ListBox1.Items.Add(name);
        }

        private string GetPandaFileName()
        {
            return CommonTestUtils.GetAppFolder() + "Html\\SampleArchive\\Html\\page1.html";
        }

        private string GetPandaUrl()
        {
            return CommonTestUtils.PrepareFileUrl(GetPandaFileName());
        }

        private void AddTestActions()
        {
            AddTestAction(
                "Open Panda sample",
                () => { WebBrowser1.LoadURL(GetPandaUrl()); });
            AddTestAction(
                "Google",
                () => { WebBrowser1.LoadURL("https://www.google.com"); });
            AddTestAction("BrowserVersion", () => { ShowBrowserVersion(); });
            AddTestAction("RestartWithDefault", () => { RestartWithDefault(); });
            AddTestAction("RestartWithIE", () => { RestartWithIE(); });
            AddTestAction("RestartWithEdge", () => { RestartWithEdge(); });
            AddTestAction();
            AddTestAction("Info", () => { LogInfo(); });
            AddTestAction("Test", () => { Test(); });
            AddTestAction(
                "TestErrorEvent",
                () => { WebBrowser1.LoadURL("memoray:myFolder/indeax.html"); });
            AddTestAction();
            AddTestAction("Stop", () => { WebBrowser1.Stop(); });
            AddTestAction("ClearHistory", () => { WebBrowser1.ClearHistory(); });
            AddTestAction("Reload", () => { WebBrowser1.Reload(); });
            AddTestAction("Reload(true)", () => { WebBrowser1.Reload(true); });
            AddTestAction("Reload(false)", () => { WebBrowser1.Reload(false); });
            AddTestAction("SelectAll", () => { WebBrowser1.SelectAll(); });
            AddTestAction(
                "DeleteSelection",
                () => { WebBrowser1.DeleteSelection(); });
            AddTestAction(
                "ClearSelection",
                () => { WebBrowser1.ClearSelection(); });
            AddTestAction("Cut", () => { WebBrowser1.Cut(); });
            AddTestAction("Copy", () => { WebBrowser1.Copy(); });
            AddTestAction("Paste", () => { WebBrowser1.Paste(); });
            AddTestAction("Undo", () => { WebBrowser1.Undo(); });
            AddTestAction("Redo", () => { WebBrowser1.Redo(); });
            AddTestAction("Print", () => { WebBrowser1.Print(); });
            AddTestAction("DeleteLog", () => { CommonTestUtils.DeleteLog(); });
            AddTestAction("ZoomFactor+", () => { WebBrowser1.ZoomFactor += 1; });
            AddTestAction("ZoomFactor-", () => { WebBrowser1.ZoomFactor -= 1; });
            AddTestAction();
            AddTestAction("CanNavigate=false", () => { canNavigate = false; });
            AddTestAction("CanNavigate=true", () => { canNavigate = true; });
            AddTestAction(
                "ContextMenuEnabled=true",
                () => { WebBrowser1.ContextMenuEnabled = true; });
            AddTestAction(
                "ContextMenuEnabled=false",
                () => { WebBrowser1.ContextMenuEnabled = false; });
            AddTestAction("Editable=true", () => { WebBrowser1.Editable = true; });
            AddTestAction(
                "Editable=false",
                () => { WebBrowser1.Editable = false; });
            AddTestAction(
                "AccessToDevToolsEnabled=true",
                () => { WebBrowser1.AccessToDevToolsEnabled = true; });
            AddTestAction(
                "AccessToDevToolsEnabled=false",
                () => { WebBrowser1.AccessToDevToolsEnabled = false; });
            AddTestAction(
                "EnableHistory=true",
                () => { WebBrowser1.EnableHistory(true); });
            AddTestAction(
                "EnableHistory=false",
                () => { WebBrowser1.EnableHistory(false); });
            AddTestAction();

            var methods = GetType().GetRuntimeMethods();
            foreach (var item in methods)
            {
                if (!item.Name.StartsWith("DoTest"))
                    continue;

                MethodCaller mc = new (this, item);

                AddTestAction(item.Name, mc);
            }
        }

        private void DoTestRunScriptString()
        {
            DoRunScript("function f(a){return a;}f('Hello World!');");
        }

        private void DoTestRunScriptInteger()
        {
            DoRunScript("function f(a){return a;}f(123);");
        }

        private void DoTestRunScriptDouble()
        {
            DoRunScript("function f(a){return a;}f(2.34);");
        }

        private void DoTestRunScriptBool()
        {
            DoRunScript("function f(a){return a;}f(false);");
        }

        private void DoTestRunScriptObject()
        {
            DoRunScript("function f(){var person = new Object();person.name = 'Foo'; " +
                "person.lastName = 'Bar'; return person;}f();");
        }

        private void DoTestRunScriptArray()
        {
            DoRunScript("function f(){ return [\"foo\", \"bar\"]; }f();");
        }

        private void DoTestRunScriptDOM()
        {
            DoRunScript("document.write(\"Hello World!\");");
        }

        private void DoTestRunScriptUndefined()
        {
            DoRunScript("function f(){var person = new Object();}f();");
        }

        private void DoTestRunScriptNull()
        {
            DoRunScript("function f(){return null;}f();");
        }

        private void DoTestRunScriptDate()
        {
            DoRunScript("function f(){var d = new Date('10/08/2017 21:30:40');" +
                "var tzoffset = d.getTimezoneOffset() * 60000;" +
                "return new Date(d.getTime() - tzoffset);}f();");
        }

        private void DoTestAddUserScript()
        {
            WebBrowser1.RemoveAllUserScripts();
            WebBrowser1.AddUserScript("alert('hello');");
            WebBrowser1.Reload();
        }

        private void DoTestSerializeObject()
        {
            WeatherForecast value = new ()
            {
                Date = System.DateTime.Now,
                TemperatureCelsius = 15,
                Summary = "New York",
            };

            var s = WebBrowser1.ObjectToJSON(value);

            Log(s);
        }

        private class MethodCaller
        {
            private readonly MethodInfo? method;
            private readonly object? parent;
            private readonly Action? action;

            public MethodCaller()
            {
            }

            public MethodCaller(object? parent, Action? action)
            {
                this.parent = parent;
                this.action = action;
            }

            public MethodCaller(object? parent, MethodInfo? methodinfo)
            {
                this.parent = parent;
                method = methodinfo;
            }

            public void DoCall()
            {
                if (action != null)
                {
                    action();
                    return;
                }

                object[] prm = new object[0];
                method?.Invoke(parent, prm);
            }
        }

        private sealed class Destructor
        {
            ~Destructor()
            {
                CommonTestUtils.LogToFile("===================");
                CommonTestUtils.LogToFile("Application finished");
                CommonTestUtils.LogToFile("===================");
            }
        }

        private class WeatherForecast
        {
            public System.DateTime Date { get; set; }

            public int TemperatureCelsius { get; set; }

            public string? Summary { get; set; }

            public string? NullText { get; set; }
        }
    }
}