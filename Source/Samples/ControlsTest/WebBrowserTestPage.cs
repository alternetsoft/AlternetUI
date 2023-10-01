using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace ControlsTest
{
    internal partial class WebBrowserTestPage : Control
    {
        internal static readonly Destructor MyDestructor = new();

        private static readonly string ZipSchemeName = "zipfs";
        private static readonly bool SetDefaultUserAgent = false;
        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;
        private static bool insideUnhandledException;
        private static EmptyWindow? emptyWindow = null;

        private readonly WebBrowserFindParams findParams = new();

        private readonly PanelAuiManager rootPanel = new();

        private readonly TextBox urlTextBox = new()
        {
        };

        private readonly string headerLabelText = "Web Browser Control";

        private readonly WebBrowser webBrowser = new()
        {
            HasBorder = false,
        };

        private readonly CheckBox findWrapCheckBox = new()
        {
            Text = "Wrap",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findEntireWordCheckBox = new()
        {
            Text = "Entire Word",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findMatchCaseCheckBox = new()
        {
            Text = "Match Case",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findHighlightResultCheckBox = new()
        {
            Text = "Highlight",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findBackwardsCheckBox = new()
        {
            Text = "Backwards",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly StackPanel findPanel = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            Orientation = StackPanelOrientation.Vertical,
        };

        private readonly TextBox findTextBox = new()
        {
            SuggestedWidth = 300,
            Margin = new Thickness(0, 10, 5, 5),
            Text = "panda",
        };

        private readonly Button findButton = new()
        {
            Text = "Find",
            Margin = new Thickness(0, 10, 5, 5),
        };

        private readonly Button findClearButton = new()
        {
            Text = "Find Clear",
            Margin = new Thickness(0, 10, 5, 5),
        };

        private readonly int buttonIdBack;
        private readonly int buttonIdForward;
        private readonly int buttonIdZoomIn;
        private readonly int buttonIdZoomOut;
        private readonly int buttonIdUrl;
        private readonly int buttonIdGo;

        private bool pandaInMemory = false;
        private bool mappingSet = false;
        private int scriptRunCounter = 0;
        private bool canNavigate = true;
        private bool historyCleared = false;
        private bool pandaLoaded = false;
        private bool scriptMessageHandlerAdded = false;
        private ITestPageSite? site;

        static WebBrowserTestPage()
        {
            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;

            WebBrowser.SetDefaultFSNameMemory("memory");
            WebBrowser.SetDefaultFSNameArchive(ZipSchemeName);
            if (SetDefaultUserAgent)
                WebBrowser.SetDefaultUserAgent("Mozilla");
            SetBackendPathSmart("Edge");

            string[] commandLineArgs = Environment.GetCommandLineArgs();
            CommonUtils.ParseCmdLine(commandLineArgs);
            if (CommonUtils.CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);

            CommonUtils.LogToFile("======================================");
            CommonUtils.LogToFile("Application started");
            CommonUtils.LogToFile("======================================");
        }

        public WebBrowserTestPage()
        {
            rootPanel.DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);

            rootPanel.BindApplicationLogMessage();
            rootPanel.DefaultRightPaneBestSize = new(150, 200);
            rootPanel.DefaultRightPaneMinSize = new(150, 200);
            rootPanel.LogControl.Required();
            rootPanel.ActionsControl.Required();
            rootPanel.LogContextMenu.Required();
            rootPanel.Toolbar.Required();

            var toolbar = rootPanel.Toolbar;
            var imageSize = rootPanel.GetToolBitmapSize();

            var images = KnownSvgImages.GetForSize(imageSize);

            buttonIdBack = toolbar.AddTool(
                CommonStrings.Default.ButtonBack,
                images.ImgBrowserBack,
                CommonStrings.Default.ButtonBack);

            buttonIdForward = toolbar.AddTool(
                CommonStrings.Default.ButtonForward,
                images.ImgBrowserForward,
                CommonStrings.Default.ButtonForward);

            buttonIdZoomIn = toolbar.AddTool(
                CommonStrings.Default.ButtonZoomIn,
                images.ImgZoomIn,
                CommonStrings.Default.ButtonZoomIn);

            buttonIdZoomOut = toolbar.AddTool(
                CommonStrings.Default.ButtonZoomOut,
                images.ImgZoomOut,
                CommonStrings.Default.ButtonZoomOut);

            buttonIdUrl = toolbar.AddControl(urlTextBox);

            buttonIdGo = toolbar.AddTool(
                CommonStrings.Default.ButtonGo,
                images.ImgBrowserGo,
                CommonStrings.Default.ButtonGo);

            toolbar.AddToolOnClick(buttonIdBack, BackButton_Click);
            toolbar.AddToolOnClick(buttonIdForward, ForwardBtn_Click);
            toolbar.AddToolOnClick(buttonIdZoomIn, ZoomInButton_Click);
            toolbar.AddToolOnClick(buttonIdZoomOut, ZoomOutButton_Click);
            toolbar.AddToolOnClick(buttonIdGo, GoButton_Click);

            urlTextBox.KeyDown += TextBox_KeyDown;

            toolbar.GrowToolMinWidth(buttonIdUrl, 350);

            toolbar.Realize();

            var myListener = new CommonUtils.DebugTraceListener();
            Trace.Listeners.Add(myListener);
        }

        public static WebBrowserBackend UseBackend
        {
            get => useBackend;
            set
            {
                useBackend = value;
                WebBrowser.SetBackend(useBackend);
            }
        }

        public string? PageName { get; set; }

        public ITestPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
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

        internal void DoTestIEShowPrintPreviewDialog()
        {
            if (IsIEBackend())
                webBrowser.DoCommand("IE.ShowPrintPreviewDialog");
        }

        internal void DoTestPandaFromMemory()
        {
            if (webBrowser.Backend == WebBrowserBackend.Edge)
            {
                Log("Memory scheme not supported in Edge backend");
                return;
            }

            void PandaToMemory()
            {
                if (pandaInMemory)
                    return;
                pandaInMemory = true;
                webBrowser.MemoryFS.AddTextFile(
                    "index.html",
                    "<html><body><b>index.html</b></body></html>");
                webBrowser.MemoryFS.AddTextFile(
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
                    string sPath = CommonUtils.GetAppFolder() + "Html/SampleArchive/" + name;

                    if (mimeType == null)
                        webBrowser.MemoryFS.AddOSFile(name, sPath);
                    else
                        webBrowser.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }

            PandaToMemory();
            webBrowser.LoadURL("memory:Html/page1.html");
        }

        internal void DoTestMessageHandler()
        {
            if (scriptMessageHandlerAdded)
                DoRunScript("window.wx_msg.postMessage('This is a message body');");
        }

        internal void DoTestInvokeScript()
        {
            DoInvokeScript("alert", "hello");
        }

        internal void DoTestInvokeScript3()
        {
            DoInvokeScript("alert", 16325.62901F);
            DoInvokeScript("alert", webBrowser.ToInvokeScriptArg(16325.62901F)?.ToString());
            DoInvokeScript("alert", System.DateTime.Now);
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
            webBrowser.RemoveAllUserScripts();
            webBrowser.AddUserScript("alert('hello');");
            webBrowser.Reload();
        }

        internal void DoTestSerializeObject()
        {
            WeatherForecast value = new()
            {
                Date = System.DateTime.Now,
                TemperatureCelsius = 15,
                Summary = "New York",
            };

            var s = webBrowser.ObjectToJSON(value);

            Log(s);
        }

        internal void DoTestInvokeScript2()
        {
            DoInvokeScript("document.URL.toUpperCase");
        }

        internal void DoTestRunScriptGetBrowser()
        {
            DoRunScript("get_browser()");
        }

        internal void TestNavigateToString1()
        {
            webBrowser.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }

        internal void TestNavigateToString2()
        {
            webBrowser.NavigateToString("<html><body>NavigateToString example 2</body></html>", "www.aa.com");
        }

        internal void TestNavigateToStream()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            webBrowser.NavigateToStream(stream);
        }

        internal void TestMapping()
        {
            if (webBrowser.Backend != WebBrowserBackend.Edge)
                return;
            if (!mappingSet)
            {
                mappingSet = true;
                webBrowser.SetVirtualHostNameToFolderMapping(
                    "assets.example",
                    "Html",
                    WebBrowserHostResourceAccessKind.DenyCors);
            }

            webBrowser.LoadURL("https://assets.example/SampleArchive/Html/page1.html");
        }

        internal void DoTestRunScript2()
        {
            DoRunScript("alert('hello');");
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || Flags.HasFlag(ControlFlags.ParentAssigned))
                return;

            webBrowser.ZoomType = WebBrowserZoomType.Layout;
            scriptMessageHandlerAdded = webBrowser.AddScriptMessageHandler("wx_msg");
            if (!scriptMessageHandlerAdded)
                Log("AddScriptMessageHandler not supported");
            FindParamsToControls();
            AddTestActions();
            if (IsIEBackend())
                webBrowser.DoCommand("IE.SetScriptErrorsSuppressed", "true");

            if (CommonUtils.CmdLineTest)
            {
                findClearButton.Visible = true;
            }

            rootPanel.Parent = this;

            webBrowser.Navigated += WebBrowser1_Navigated;
            webBrowser.Loaded += WebBrowser1_Loaded;
            webBrowser.NewWindow += WebBrowser1_NewWindow;
            webBrowser.DocumentTitleChanged += WebBrowser1_TitleChanged;
            webBrowser.FullScreenChanged += WebBrowser1_FullScreenChanged;
            webBrowser.ScriptMessageReceived += WebBrowser1_ScriptMessageReceived;
            webBrowser.ScriptResult += WebBrowser1_ScriptResult;
            webBrowser.Navigating += WebBrowser1_Navigating;
            webBrowser.Error += WebBrowser1_Error;
            webBrowser.BeforeBrowserCreate += WebBrowser1_BeforeBrowserCreate;

            findPanel.Children.Add(findTextBox);

            findButton.Click += FindButton_Click;
            findPanel.Children.Add(findButton);

            findClearButton.Click += FindClearButton_Click;
            findPanel.Children.Add(findClearButton);

            findPanel.Children.Add(findWrapCheckBox);
            findPanel.Children.Add(findEntireWordCheckBox);
            findPanel.Children.Add(findMatchCaseCheckBox);
            findPanel.Children.Add(findHighlightResultCheckBox);
            findPanel.Children.Add(findBackwardsCheckBox);

            findPanel.Parent = rootPanel.RightNotebook;

            /*
            rootPanel.RightNotebook.AddPage(
                findPanel,
                CommonStrings.Default.NotebookTabTitleSearch);*/

            rootPanel.CenterNotebook.AddPage(
                webBrowser,
                CommonStrings.Default.NotebookTabTitleBrowser);

            rootPanel.Manager.AddPane(rootPanel.Toolbar, rootPanel.ToolbarPane);

            rootPanel.Manager.Update();
        }

        private static void HandleException(Exception e)
        {
            CommonUtils.LogException(e);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(
            object? sender,
            UnhandledExceptionEventArgs e)
        {
            if (!insideUnhandledException)
            {
                insideUnhandledException = true;
                HandleException((e.ExceptionObject as Exception)!);
                insideUnhandledException = false;
            }
        }

        private static string GetPandaFileName()
        {
            return CommonUtils.GetAppFolder() + "Html/SampleArchive/Html/page1.html";
        }

        private static string GetPandaUrl()
        {
            return CommonUtils.PrepareFileUrl(GetPandaFileName());
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

        private bool IsIEBackend()
        {
            return webBrowser.Backend == WebBrowserBackend.IE ||
                webBrowser.Backend == WebBrowserBackend.IELatest;
        }

        private void DoTestZip()
        {
            if (webBrowser.Backend == WebBrowserBackend.Edge)
            {
                Log("Archive scheme not supported in Edge backend");
                return;
            }

            string arcSubPath = "Html/SampleArchive.zip";
            string webPagePath = "root.html";

            string archivePath = Path.Combine(CommonUtils.GetAppFolder(), arcSubPath);
            if (File.Exists(archivePath))
            {
                string url = CommonUtils.PrepareZipUrl(ZipSchemeName, archivePath, webPagePath);
                webBrowser.LoadURL(url);
            }
        }

        private void Test()
        {
            DoTestZip();
        }

        private void FindParamsToControls()
        {
            findWrapCheckBox.IsChecked = findParams.Wrap;
            findEntireWordCheckBox.IsChecked = findParams.EntireWord;
            findMatchCaseCheckBox.IsChecked = findParams.MatchCase;
            findHighlightResultCheckBox.IsChecked = findParams.HighlightResult;
            findBackwardsCheckBox.IsChecked = findParams.Backwards;
        }

        private void FindParamsFromControls()
        {
            findParams.Wrap = findWrapCheckBox.IsChecked;
            findParams.EntireWord = findEntireWordCheckBox.IsChecked;
            findParams.MatchCase = findMatchCaseCheckBox.IsChecked;
            findParams.HighlightResult = findHighlightResultCheckBox.IsChecked;
            findParams.Backwards = findBackwardsCheckBox.IsChecked;
        }

        private void FindClearButton_Click(object? sender, EventArgs e)
        {
            findTextBox.Text = string.Empty;
            webBrowser.FindClearResult();
        }

        private void FindButton_Click(object? sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = webBrowser.Find(findTextBox.Text, findParams);
            Log("Find Result = " + findResult.ToString());
        }

        private IntPtr GetNewClientData()
        {
            scriptRunCounter++;
            IntPtr clientData = new(scriptRunCounter);
            return clientData;
        }

        private void DoInvokeScript(string scriptName, params object?[] args)
        {
            var clientData = GetNewClientData();
            Log($"InvokeScript {scriptName}({webBrowser.ToInvokeScriptArgs(args)})");
            webBrowser.InvokeScriptAsync(scriptName, clientData, args);
        }

        private void DoRunScript(string script)
        {
            var clientData = GetNewClientData();
            Log($"RunScript {clientData} > {script}");
            webBrowser.RunScriptAsync(script, clientData);
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = urlTextBox.Text;

                LoadUrl(s);
            }
        }

        private void Log(string s)
        {
            rootPanel.Log(s);
        }

        private void LogWebBrowserEvent(WebBrowserEventArgs e)
        {
            string s = "EVENT: " + e.EventType;

            if (!string.IsNullOrEmpty(e.Text))
                s += $"; Text = '{e.Text}'";
            if (!string.IsNullOrEmpty(e.Url))
                s += $"; Url = '{e.Url}'";
            if (!string.IsNullOrEmpty(e.TargetFrameName))
                s += $"; Target = '{e.TargetFrameName}'";
            if (e.NavigationAction != 0)
                s += $"; ActionFlags = {e.NavigationAction}";
            if (!string.IsNullOrEmpty(e.MessageHandler))
                s += $"; MessageHandler = '{e.MessageHandler}'";
            if (e.IsError)
                s += $"; IsError = {e.IsError}";
            if (e.NavigationError != null)
                s += $"; NavigationError = {e.NavigationError}";
            if (e.ClientData != IntPtr.Zero)
                s += $"; ClientData = {e.ClientData}";
            Log(s);
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

        private void WebBrowser1_Navigated(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            urlTextBox.Text = webBrowser.GetCurrentURL();
        }

        private void WebBrowser1_Navigating(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            if (!canNavigate)
                e.Cancel = true;
        }

        private void WebBrowser1_Loaded(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            UpdateHistoryButtons();

            if (!pandaLoaded)
            {
                webBrowser.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;

                pandaLoaded = true;
                try
                {
                    webBrowser.LoadURL(GetPandaUrl());
                }
                catch (Exception)
                {
                }
            }
        }

        private void WebBrowser1_Error(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
        }

        private void WebBrowser1_NewWindow(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            LoadUrl(e.Url);
        }

        private void WebBrowser1_TitleChanged(object? sender, WebBrowserEventArgs e)
        {
            LogWebBrowserEvent(e);
            var backendVersion = WebBrowser.GetBackendVersionString(webBrowser.Backend);
            if (this.IsIEBackend())
                backendVersion = "Internet Explorer";

            Assembly thisAssembly = typeof(WebBrowser).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName.Version;

            Log($"{headerLabelText} {ver} : {e.Text} : {backendVersion}");
        }

        private void GoButton_Click(object? sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void ShowBrowserVersion()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html")
                + "version.html";
            webBrowser.LoadURL("file://" + filename.Replace('\\', '/'));
        }

        private void LoadUrl(string? s)
        {
            if (s == null)
            {
                webBrowser.LoadURL();
                return;
            }

            if (s == "s")
            {
                WebBrowser.DoCommandGlobal("Screenshot", this);
                return;
            }

            if (s == "s2" && emptyWindow == null)
            {
                emptyWindow = new();
                emptyWindow.Show();
                return;
            }

            if (s == "s3")
            {
                if (emptyWindow is not null)
                    WebBrowser.DoCommandGlobal("Screenshot", emptyWindow);
                return;
            }

            if (s == "i")
            {
                LogInfo(string.Empty);
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

            Log("==> LoadUrl: " + s);
            webBrowser.LoadUrlOrSearch(s);
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
            void Fn()
            {
                Log("=======" + s);

                LogProp(webBrowser, "HasSelection", "WebBrowser");
                LogProp(webBrowser, "SelectedText", "WebBrowser");
                LogProp(webBrowser, "SelectedSource", "WebBrowser");
                LogProp(webBrowser, "CanCut", "WebBrowser");
                LogProp(webBrowser, "CanCopy", "WebBrowser");
                LogProp(webBrowser, "CanPaste", "WebBrowser");
                LogProp(webBrowser, "CanUndo", "WebBrowser");
                LogProp(webBrowser, "CanRedo", "WebBrowser");
                LogProp(webBrowser, "IsBusy", "WebBrowser");
                LogProp(webBrowser, "PageSource", "WebBrowser");
                LogProp(webBrowser, "UserAgent", "WebBrowser");

                LogProp(webBrowser, "PageText", "WebBrowser");
                LogProp(webBrowser, "Zoom", "WebBrowser");
                LogProp(webBrowser, "Editable", "WebBrowser");
                LogProp(webBrowser, "ZoomType", "WebBrowser");
                LogProp(webBrowser, "ZoomFactor", "WebBrowser");

                // Log("GetDefaultImageSize = " + Toolbar.GetDefaultImageSize());
                Log("DPI = " + webBrowser.GetDPI().ToString());
                Log("isDebug = " + WebBrowser.DoCommandGlobal("IsDebug"));
                Log("os = " + WebBrowser.GetBackendOS().ToString());
                Log("backend = " + webBrowser.Backend.ToString());
                Log("wxWidgetsVersion = " + WebBrowser.GetLibraryVersionString());
                Log("GetCurrentTitle() = " + webBrowser.GetCurrentTitle());
                Log("GetCurrentURL() = " + webBrowser.GetCurrentURL());
                Log("IE = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.IE));
                Log("Edge = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge));
                Log("WebKit = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit));
                Log("Net Version = " + Environment.Version.ToString());

                Log("GetUsefulDefines" + WebBrowser.DoCommandGlobal("GetUsefulDefines"));

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                    Log("Edge version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.Edge));
                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IE))
                    Log("IE version = " + WebBrowser.GetBackendVersionString(WebBrowserBackend.IE));
                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
                {
                    Log("Webkit version = "
                        + WebBrowser.GetBackendVersionString(WebBrowserBackend.WebKit));
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Default))
                {
                    Log("Default browser version = "
                        + WebBrowser.GetBackendVersionString(WebBrowserBackend.Default));
                }

                Log("=======");
            }

            rootPanel.LogControl.DoInsideUpdate(Fn);
        }

        private void UpdateZoomButtons()
        {
            rootPanel.Toolbar.EnableTool(buttonIdZoomIn, webBrowser.CanZoomIn);
            rootPanel.Toolbar.EnableTool(buttonIdZoomOut, webBrowser.CanZoomOut);
            rootPanel.Toolbar.Refresh();
        }

        private void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                webBrowser.ClearHistory();
            }

            rootPanel.Toolbar.EnableTool(buttonIdBack, webBrowser.CanGoBack);
            rootPanel.Toolbar.EnableTool(buttonIdForward, webBrowser.CanGoForward);
            rootPanel.Toolbar.Refresh();
        }

        private void ZoomInButton_Click(object? sender, EventArgs e)
        {
            webBrowser.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object? sender, EventArgs e)
        {
            webBrowser.ZoomOut();
            UpdateZoomButtons();
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void ForwardBtn_Click(object? sender, EventArgs e)
        {
            webBrowser.GoForward();
        }

        private void AddTestAction(string? name = null, Action? action = null)
        {
            if (name == null)
            {
                rootPanel.AddActionSpacer();
                return;
            }

            rootPanel.AddAction(name, action);
        }

        private void AddTestActions()
        {
            AddTestAction(
                "Open Panda sample",
                () => { webBrowser.LoadURL(GetPandaUrl()); });
            AddTestAction(
                "Google",
                () => { webBrowser.LoadURL("https://www.google.com"); });
            AddTestAction("BrowserVersion", () => { ShowBrowserVersion(); });
            AddTestAction();
            AddTestAction("Info", () => { LogInfo(); });
            AddTestAction("Test", () => { Test(); });
            AddTestAction(
                "TestErrorEvent",
                () => { webBrowser.LoadURL("memoray:myFolder/indeax.html"); });
            AddTestAction();
            AddTestAction("Stop", () => { webBrowser.Stop(); });
            AddTestAction("ClearHistory", () => { webBrowser.ClearHistory(); });
            AddTestAction("Reload", () => { webBrowser.Reload(); });
            AddTestAction("Reload(true)", () => { webBrowser.Reload(true); });
            AddTestAction("Reload(false)", () => { webBrowser.Reload(false); });
            AddTestAction("SelectAll", () => { webBrowser.SelectAll(); });
            AddTestAction(
                "DeleteSelection",
                () => { webBrowser.DeleteSelection(); });
            AddTestAction(
                "ClearSelection",
                () => { webBrowser.ClearSelection(); });
            AddTestAction("Cut", () => { webBrowser.Cut(); });
            AddTestAction("Copy", () => { webBrowser.Copy(); });
            AddTestAction("Paste", () => { webBrowser.Paste(); });
            AddTestAction("Undo", () => { webBrowser.Undo(); });
            AddTestAction("Redo", () => { webBrowser.Redo(); });
            AddTestAction("Print", () => { webBrowser.Print(); });
            AddTestAction("DeleteLog", () => { CommonUtils.DeleteLog(); });
            AddTestAction("ZoomFactor+", () => { webBrowser.ZoomFactor += 1; });
            AddTestAction("ZoomFactor-", () => { webBrowser.ZoomFactor -= 1; });
            AddTestAction();
            AddTestAction("HasBorder", () =>
            {
                webBrowser.HasBorder = !webBrowser.HasBorder;
            });
            AddTestAction("CanNavigate=false", () => { canNavigate = false; });
            AddTestAction("CanNavigate=true", () => { canNavigate = true; });
            AddTestAction(
                "ContextMenuEnabled=true",
                () => { webBrowser.ContextMenuEnabled = true; });
            AddTestAction(
                "ContextMenuEnabled=false",
                () => { webBrowser.ContextMenuEnabled = false; });
            AddTestAction("Editable=true", () => { webBrowser.Editable = true; });
            AddTestAction(
                "Editable=false",
                () => { webBrowser.Editable = false; });
            AddTestAction(
                "AccessToDevToolsEnabled=true",
                () => { webBrowser.AccessToDevToolsEnabled = true; });
            AddTestAction(
                "AccessToDevToolsEnabled=false",
                () => { webBrowser.AccessToDevToolsEnabled = false; });
            AddTestAction(
                "EnableHistory=true",
                () => { webBrowser.EnableHistory(true); });
            AddTestAction(
                "EnableHistory=false",
                () => { webBrowser.EnableHistory(false); });

            if (webBrowser.Backend == WebBrowserBackend.Edge)
            {
                AddTestAction(
                    "Test Edge Mapping",
                    () => { TestMapping(); });
            }

            if (!IsIEBackend())
            {
                AddTestAction(
                    "Load PDF",
                    () =>
                    {
                        string sPdfPath = CommonUtils.GetAppFolder() +
                            "Resources/SamplePandaPdf.pdf";
                        webBrowser.LoadURL(CommonUtils.PrepareFileUrl(sPdfPath));
                    });
            }

            AddTestAction();

            var methods = GetType().GetRuntimeMethods();
            foreach (var item in methods)
            {
                if (!item.Name.StartsWith("DoTest"))
                    continue;
                AddTestAction(item.Name, AssemblyUtils.CreateAction(this, item));
            }
        }

        internal sealed class Destructor
        {
            ~Destructor()
            {
                CommonUtils.LogToFile("===================");
                CommonUtils.LogToFile("Application finished");
                CommonUtils.LogToFile("===================");
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