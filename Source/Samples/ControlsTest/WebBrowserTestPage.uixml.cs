using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.Drawing;
using Alternet.UI;
using static System.Net.Mime.MediaTypeNames;

namespace ControlsTest
{
    internal partial class WebBrowserTestPage : Control
    {
        internal static readonly Destructor MyDestructor = new();

        private const int ImageSize = 16;
        private static readonly string ResPrefix =
            $"embres:ControlsTest.resources.Png._{ImageSize}.";

        private static readonly string ZipSchemeName = "zipfs";
        private static readonly bool SetDefaultUserAgent = false;
        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;
        private static bool insideUnhandledException;
        private static EmptyWindow? emptyWindow = null;

        private readonly WebBrowserFindParams findParams = new();
        private readonly Dictionary<string, MethodCaller> testActions = new();

        private ToolbarPanel toolbar;
        private StackPanel webBrowserToolbarPanel;
        private Button backButton;
        private Button forwardButton;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button goButton;
        private TextBox urlTextBox;
        private StackPanel findOptionsPanel;
        private CheckBox findWrapCheckBox;
        private CheckBox findEntireWordCheckBox;
        private CheckBox findMatchCaseCheckBox;
        private CheckBox findHighlightResultCheckBox;
        private CheckBox findBackwardsCheckBox;
        private StackPanel findPanel;
        private TextBox findTextBox;
        private Button findButton;
        private Button findClearButton;
        private StackPanel mainStackPanel;
        private ListBox listBox1;

        private bool pandaInMemory = false;
        private bool mappingSet = false;
        private int scriptRunCounter = 0;
        private bool canNavigate = true;
        private bool historyCleared = false;
        private bool pandaLoaded = false;
        private string? headerText;
        private bool scriptMessageHandlerAdded = false;
        private ITestPageSite? site;
        private WebBrowser webBrowser1;

        static WebBrowserTestPage()
        {
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
            var myListener = new CommonUtils.DebugTraceListener();
            Trace.Listeners.Add(myListener);
            InitializeComponent();

            AddNewToolbar();
            AddToolbar();
            AddMainPanel();
            AddFindPanel();
            AddFindOptionsPanel();
        }

        public void AddMainPanel()
        {
            mainStackPanel = new()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Orientation = StackPanelOrientation.Horizontal,
            };
            Grid.SetRowColumn(mainStackPanel, 2, 0);

            listBox1 = new()
            {
                Width = 180,
                Height = 400,
            };
            listBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
            mainStackPanel.Children.Add(listBox1);

            webBrowser1 = new()
            {
                Width = 500,
                Height = 300,
                Margin = new(5, 0, 0, 0),
            };
            webBrowser1.Navigated += WebBrowser1_Navigated;
            webBrowser1.Loaded += WebBrowser1_Loaded;
            webBrowser1.NewWindow += WebBrowser1_NewWindow;
            webBrowser1.DocumentTitleChanged += WebBrowser1_TitleChanged;
            webBrowser1.FullScreenChanged += WebBrowser1_FullScreenChanged;
            webBrowser1.ScriptMessageReceived += WebBrowser1_ScriptMessageReceived;
            webBrowser1.ScriptResult += WebBrowser1_ScriptResult;
            webBrowser1.Navigating += WebBrowser1_Navigating;
            webBrowser1.Error += WebBrowser1_Error;
            webBrowser1.BeforeBrowserCreate += WebBrowser1_BeforeBrowserCreate;
            mainStackPanel.Children.Add(webBrowser1);

            webBrowserGrid.Children.Add(mainStackPanel);
        }

        public void AddNewToolbar()
        {
            toolbar = new();
            toolbar.Toolbar.ItemTextVisible = false;
            toolbar.Toolbar.ImageToTextDisplayMode =
                ToolbarItemImageToTextDisplayMode.Vertical;
            ToolbarItem toolbarItem;

            toolbarItem = new ToolbarItem("Back", BackButton_Click)
            {
                ToolTip = "Back",
                Image = ImageSet.FromUrl($"{ResPrefix}arrow-left-{ImageSize}.png"),
            };
            toolbar.Toolbar.Items.Add(toolbarItem);

            toolbarItem = new ToolbarItem("Forward", ForwardButton_Click)
            {
                ToolTip = "Forward",
                Image = ImageSet.FromUrl($"{ResPrefix}arrow-right-{ImageSize}.png"),
            };
            toolbar.Toolbar.Items.Add(toolbarItem);

            toolbarItem = new ToolbarItem("Zoom In", ZoomInButton_Click)
            {
                ToolTip = "Zoom In",
                Image = ImageSet.FromUrl($"{ResPrefix}plus-{ImageSize}.png"),
            };
            toolbar.Toolbar.Items.Add(toolbarItem);

            toolbarItem = new ToolbarItem("Zoom out", ZoomOutButton_Click)
            {
                ToolTip = "Zoom out",
                Image = ImageSet.FromUrl($"{ResPrefix}minus-{ImageSize}.png"),
            };
            toolbar.Toolbar.Items.Add(toolbarItem);

            toolbarItem = new ToolbarItem("Go", GoButton_Click)
            {
                ToolTip = "Go",
                Image = ImageSet.FromUrl($"{ResPrefix}caret-right-{ImageSize}.png"),
            };
            toolbar.Toolbar.Items.Add(toolbarItem);

            Grid.SetRowColumn(toolbar, 1, 0);
            webBrowserGrid.Children.Add(toolbar);


            //LayoutFactory.SetDebugBackgroundToParents(toolbar);

            //LayoutFactory.AddToolbar(webBrowserGrid, toolbar);

            /*
            urlTextBox = new()
                Width = 300,
            urlTextBox.KeyDown += TextBox_KeyDown;
             */
        }

        public void AddToolbar()
        {
            webBrowserToolbarPanel = new()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Orientation = StackPanelOrientation.Horizontal,
            };
            Grid.SetRowColumn(webBrowserToolbarPanel,1, 0);

            backButton = new()
            {
                Margin = new Thickness(0, 5, 5, 5),
                Image = Bitmap.FromUrl($"{ResPrefix}arrow-left-{ImageSize}.png"),
            };
            backButton.Click += BackButton_Click;

            forwardButton = new()
            {
                Margin = new Thickness(0, 5, 5, 5),
                Image = Bitmap.FromUrl($"{ResPrefix}arrow-right-{ImageSize}.png"),
            };
            forwardButton.Click += ForwardButton_Click;

            zoomInButton = new()
            {
                Margin = new Thickness(0, 5, 5, 5),
                Image = Bitmap.FromUrl($"{ResPrefix}plus-{ImageSize}.png"),
                Visible = true,
            };
            zoomInButton.Click += ZoomInButton_Click;

            zoomOutButton = new()
            {
                Margin = new Thickness(0, 5, 5, 5),
                Image = Bitmap.FromUrl($"{ResPrefix}minus-{ImageSize}.png"),
                Visible = true,
            };
            zoomOutButton.Click += ZoomOutButton_Click;

            goButton = new()
            {
                Margin = new Thickness(0, 5, 0, 5),
                Image = Bitmap.FromUrl($"{ResPrefix}caret-right-{ImageSize}.png"),
            };
            goButton.Click += GoButton_Click;

            urlTextBox = new()
            {
                Width = 300,
                Margin = new Thickness(0, 5, 5, 5),
            };
            urlTextBox.KeyDown += TextBox_KeyDown;

            webBrowserToolbarPanel.Children.Add(backButton);
            webBrowserToolbarPanel.Children.Add(forwardButton);
            webBrowserToolbarPanel.Children.Add(zoomInButton);
            webBrowserToolbarPanel.Children.Add(zoomOutButton);
            webBrowserToolbarPanel.Children.Add(urlTextBox);
            webBrowserToolbarPanel.Children.Add(goButton);
            //webBrowserGrid.Children.Add(webBrowserToolbarPanel);

        }

        public void AddFindPanel()
        {
            findPanel = new()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Orientation = StackPanelOrientation.Horizontal,
            };
            Grid.SetRowColumn(findPanel, 3, 0);

            findTextBox = new()
            {
                Width = 300,
                Margin = new Thickness(0, 10, 5, 5),
                Text = "panda",
            };
            findPanel.Children.Add(findTextBox);

            findButton = new()
            {
                Text = "Find",
                Margin = new Thickness(0, 10, 5, 5),
            };
            findButton.Click += FindButton_Click;
            findPanel.Children.Add(findButton);

            findClearButton = new()
            {
                Text = "Find Clear",
                Margin = new Thickness(0, 10, 5, 5),
            };
            findClearButton.Click += FindClearButton_Click;
            findPanel.Children.Add(findClearButton);

            webBrowserGrid.Children.Add(findPanel);
        }

        public void AddFindOptionsPanel()
        {
            findOptionsPanel = new()
            {
                Margin = new(5, 5, 5, 5),
                Orientation = StackPanelOrientation.Horizontal,
            };
            Grid.SetRowColumn(findOptionsPanel, 4, 0);

            findWrapCheckBox = new()
            {
                Text = "Wrap",
                Margin = new Thickness(0, 5, 5, 5),
            };
            findOptionsPanel.Children.Add(findWrapCheckBox);

            findEntireWordCheckBox = new()
            {
                Text = "Entire Word",
                Margin = new Thickness(0, 5, 5, 5),
            };
            findOptionsPanel.Children.Add(findEntireWordCheckBox);

            findMatchCaseCheckBox = new()
            {
                Text = "Match Case",
                Margin = new Thickness(0, 5, 5, 5),
            };
            findOptionsPanel.Children.Add(findMatchCaseCheckBox);

            findHighlightResultCheckBox = new()
            {
                Text = "Highlight",
                Margin = new Thickness(0, 5, 5, 5),
            };
            findOptionsPanel.Children.Add(findHighlightResultCheckBox);

            findBackwardsCheckBox = new()
            {
                Text = "Backwards",
                Margin = new Thickness(0, 5, 5, 5),
            };
            findOptionsPanel.Children.Add(findBackwardsCheckBox);

            webBrowserGrid.Children.Add(findOptionsPanel);
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
                headerText = HeaderLabel.Text;
                webBrowser1.ZoomType = WebBrowserZoomType.Layout;
                scriptMessageHandlerAdded = webBrowser1.AddScriptMessageHandler("wx_msg");
                if (!scriptMessageHandlerAdded)
                    Log("AddScriptMessageHandler not supported");
                FindParamsToControls();
                AddTestActions();
                if (IsIEBackend())
                    webBrowser1.DoCommand("IE.SetScriptErrorsSuppressed", "true");

                if (CommonUtils.CmdLineTest)
                {
                    listBox1.Visible = true;
                    findOptionsPanel.Visible = true;
                    findClearButton.Visible = true;
                }

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
                webBrowser1.DoCommand("IE.ShowPrintPreviewDialog");
        }

        internal void DoTestPandaFromMemory()
        {
            if (webBrowser1.Backend == WebBrowserBackend.Edge)
            {
                Log("Memory scheme not supported in Edge backend");
                return;
            }

            void PandaToMemory()
            {
                if (pandaInMemory)
                    return;
                pandaInMemory = true;
                webBrowser1.MemoryFS.AddTextFile(
                    "index.html",
                    "<html><body><b>index.html</b></body></html>");
                webBrowser1.MemoryFS.AddTextFile(
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
                        webBrowser1.MemoryFS.AddOSFile(name, sPath);
                    else
                        webBrowser1.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }

            PandaToMemory();
            webBrowser1.LoadURL("memory:Html/page1.html");
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
            DoInvokeScript("alert", webBrowser1.ToInvokeScriptArg(16325.62901F)?.ToString());
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
            webBrowser1.RemoveAllUserScripts();
            webBrowser1.AddUserScript("alert('hello');");
            webBrowser1.Reload();
        }

        internal void DoTestSerializeObject()
        {
            WeatherForecast value = new()
            {
                Date = System.DateTime.Now,
                TemperatureCelsius = 15,
                Summary = "New York",
            };

            var s = webBrowser1.ObjectToJSON(value);

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
            webBrowser1.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }

        internal void TestNavigateToString2()
        {
            webBrowser1.NavigateToString("<html><body>NavigateToString example 2</body></html>", "www.aa.com");
        }

        internal void TestNavigateToStream()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            webBrowser1.NavigateToStream(stream);
        }

        internal void TestMapping()
        {
            if (webBrowser1.Backend != WebBrowserBackend.Edge)
                return;
            if (!mappingSet)
            {
                mappingSet = true;
                webBrowser1.SetVirtualHostNameToFolderMapping("assets.example", "Html", WebBrowserHostResourceAccessKind.DenyCors);
            }

            webBrowser1.LoadURL("https://assets.example/SampleArchive/Html/page1.html");
        }

        internal void DoTestRunScript2()
        {
            DoRunScript("alert('hello');");
        }

        private static void HandleException(Exception e)
        {
            CommonUtils.LogException(e);
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
            return webBrowser1.Backend == WebBrowserBackend.IE ||
                webBrowser1.Backend == WebBrowserBackend.IELatest;
        }

        private void DoTestZip()
        {
            if (webBrowser1.Backend == WebBrowserBackend.Edge)
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
                webBrowser1.LoadURL(url);
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
            webBrowser1.FindClearResult();
        }

        private void FindButton_Click(object? sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = webBrowser1.Find(findTextBox.Text, findParams);
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
            Log($"InvokeScript {scriptName}({webBrowser1.ToInvokeScriptArgs(args)})");
            webBrowser1.InvokeScriptAsync(scriptName, clientData, args);
        }

        private void DoRunScript(string script)
        {
            var clientData = GetNewClientData();
            Log($"RunScript {clientData} > {script}");
            webBrowser1.RunScriptAsync(script, clientData);
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
            CommonUtils.LogToFile(s);
            site?.LogEvent(PageName, s);
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
            urlTextBox.Text = webBrowser1.GetCurrentURL();
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
                webBrowser1.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;

                pandaLoaded = true;
                try
                {
                    webBrowser1.LoadURL(GetPandaUrl());
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
            var backendVersion = WebBrowser.GetBackendVersionString(webBrowser1.Backend);
            if (this.IsIEBackend())
                backendVersion = "Internet Explorer";

            Assembly thisAssembly = typeof(WebBrowser).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName.Version;

            HeaderLabel.Text = $"{headerText} {ver} : {e.Text} : {backendVersion}";
        }

        private void GoButton_Click(object? sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void ShowBrowserVersion()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html")
                + "version.html";
            webBrowser1.LoadURL("file://" + filename.Replace('\\', '/'));
        }

        private void LoadUrl(string? s)
        {
            if (s == null)
            {
                webBrowser1.LoadURL();
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

            if (s == "l")
            {
                listBox1.Visible = true;
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
            webBrowser1.LoadUrlOrSearch(s);
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

            LogProp(webBrowser1, "HasSelection", "WebBrowser");
            LogProp(webBrowser1, "SelectedText", "WebBrowser");
            LogProp(webBrowser1, "SelectedSource", "WebBrowser");
            LogProp(webBrowser1, "CanCut", "WebBrowser");
            LogProp(webBrowser1, "CanCopy", "WebBrowser");
            LogProp(webBrowser1, "CanPaste", "WebBrowser");
            LogProp(webBrowser1, "CanUndo", "WebBrowser");
            LogProp(webBrowser1, "CanRedo", "WebBrowser");
            LogProp(webBrowser1, "IsBusy", "WebBrowser");
            LogProp(webBrowser1, "PageSource", "WebBrowser");
            LogProp(webBrowser1, "UserAgent", "WebBrowser");

            LogProp(webBrowser1, "PageText", "WebBrowser");
            LogProp(webBrowser1, "Zoom", "WebBrowser");
            LogProp(webBrowser1, "Editable", "WebBrowser");
            LogProp(webBrowser1, "ZoomType", "WebBrowser");
            LogProp(webBrowser1, "ZoomFactor", "WebBrowser");

            Log("isDebug = " + WebBrowser.DoCommandGlobal("IsDebug"));
            Log("os = " + WebBrowser.GetBackendOS().ToString());
            Log("backend = " + webBrowser1.Backend.ToString());
            Log("wxWidgetsVersion = " + WebBrowser.GetLibraryVersionString());
            Log("GetCurrentTitle() = " + webBrowser1.GetCurrentTitle());
            Log("GetCurrentURL() = " + webBrowser1.GetCurrentURL());
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
            zoomInButton.Enabled = webBrowser1.CanZoomIn;
            zoomOutButton.Enabled = webBrowser1.CanZoomOut;
        }

        private void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                webBrowser1.ClearHistory();
            }

            backButton.Enabled = webBrowser1.CanGoBack;
            forwardButton.Enabled = webBrowser1.CanGoForward;
        }

        private void ZoomInButton_Click(object? sender, EventArgs e)
        {
            webBrowser1.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object? sender, EventArgs e)
        {
            webBrowser1.ZoomOut();
            UpdateZoomButtons();
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void ForwardButton_Click(object? sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void ListBox1_MouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;

            int? index = listBox.SelectedIndex;
            if (index == null)
                return;

            string? name = listBox.Items[(int)index].ToString();

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
                listBox1.Items.Add("----");
                return;
            }

            testActions.Add(name, action!);
            listBox1.Items.Add(name);
        }

        private void AddTestActions()
        {
            AddTestAction(
                "Open Panda sample",
                () => { webBrowser1.LoadURL(GetPandaUrl()); });
            AddTestAction(
                "Google",
                () => { webBrowser1.LoadURL("https://www.google.com"); });
            AddTestAction("BrowserVersion", () => { ShowBrowserVersion(); });
            AddTestAction();
            AddTestAction("Info", () => { LogInfo(); });
            AddTestAction("Test", () => { Test(); });
            AddTestAction(
                "TestErrorEvent",
                () => { webBrowser1.LoadURL("memoray:myFolder/indeax.html"); });
            AddTestAction();
            AddTestAction("Stop", () => { webBrowser1.Stop(); });
            AddTestAction("ClearHistory", () => { webBrowser1.ClearHistory(); });
            AddTestAction("Reload", () => { webBrowser1.Reload(); });
            AddTestAction("Reload(true)", () => { webBrowser1.Reload(true); });
            AddTestAction("Reload(false)", () => { webBrowser1.Reload(false); });
            AddTestAction("SelectAll", () => { webBrowser1.SelectAll(); });
            AddTestAction(
                "DeleteSelection",
                () => { webBrowser1.DeleteSelection(); });
            AddTestAction(
                "ClearSelection",
                () => { webBrowser1.ClearSelection(); });
            AddTestAction("Cut", () => { webBrowser1.Cut(); });
            AddTestAction("Copy", () => { webBrowser1.Copy(); });
            AddTestAction("Paste", () => { webBrowser1.Paste(); });
            AddTestAction("Undo", () => { webBrowser1.Undo(); });
            AddTestAction("Redo", () => { webBrowser1.Redo(); });
            AddTestAction("Print", () => { webBrowser1.Print(); });
            AddTestAction("DeleteLog", () => { CommonUtils.DeleteLog(); });
            AddTestAction("ZoomFactor+", () => { webBrowser1.ZoomFactor += 1; });
            AddTestAction("ZoomFactor-", () => { webBrowser1.ZoomFactor -= 1; });
            AddTestAction();
            AddTestAction("CanNavigate=false", () => { canNavigate = false; });
            AddTestAction("CanNavigate=true", () => { canNavigate = true; });
            AddTestAction(
                "ContextMenuEnabled=true",
                () => { webBrowser1.ContextMenuEnabled = true; });
            AddTestAction(
                "ContextMenuEnabled=false",
                () => { webBrowser1.ContextMenuEnabled = false; });
            AddTestAction("Editable=true", () => { webBrowser1.Editable = true; });
            AddTestAction(
                "Editable=false",
                () => { webBrowser1.Editable = false; });
            AddTestAction(
                "AccessToDevToolsEnabled=true",
                () => { webBrowser1.AccessToDevToolsEnabled = true; });
            AddTestAction(
                "AccessToDevToolsEnabled=false",
                () => { webBrowser1.AccessToDevToolsEnabled = false; });
            AddTestAction(
                "EnableHistory=true",
                () => { webBrowser1.EnableHistory(true); });
            AddTestAction(
                "EnableHistory=false",
                () => { webBrowser1.EnableHistory(false); });

            if (webBrowser1.Backend == WebBrowserBackend.Edge)
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
                        webBrowser1.LoadURL(CommonUtils.PrepareFileUrl(sPdfPath));
                    });
            }

            AddTestAction();

            var methods = GetType().GetRuntimeMethods();
            foreach (var item in methods)
            {
                if (!item.Name.StartsWith("DoTest"))
                    continue;

                MethodCaller mc = new(this, item);

                AddTestAction(item.Name, mc);
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

        private class MethodCaller
        {
            private static readonly object[] Prm = Array.Empty<object>();

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

            public MethodCaller(object? parent, MethodInfo? methodInfo)
            {
                this.parent = parent;
                method = methodInfo;
            }

            public void DoCall()
            {
                if (action != null)
                {
                    action();
                    return;
                }

                method?.Invoke(parent, Prm);
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