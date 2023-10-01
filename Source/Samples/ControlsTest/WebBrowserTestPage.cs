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
        private static bool insideUnhandledException;

        private readonly PanelWebBrowser rootPanel = new();

        private readonly string headerLabelText = "Web Browser Control";

        private bool pandaInMemory = false;
        private bool mappingSet = false;
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
            PanelWebBrowser.SetBackendPathSmart("Edge");

            LogUtils.LogToFileAppStarted();
        }

        public WebBrowserTestPage()
        {
            rootPanel.LogEvents = true;

            rootPanel.BindApplicationLogMessage();
            rootPanel.DefaultRightPaneBestSize = new(150, 200);
            rootPanel.DefaultRightPaneMinSize = new(150, 200);
            rootPanel.LogControl.Required();
            rootPanel.ActionsControl.Required();
            rootPanel.LogContextMenu.Required();

            var myListener = new CommonUtils.DebugTraceListener();
            Trace.Listeners.Add(myListener);
        }

        public WebBrowser WebBrowser => rootPanel.WebBrowser;

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
            if (rootPanel.IsIEBackend)
                WebBrowser.DoCommand("IE.ShowPrintPreviewDialog");
        }

        internal void DoTestPandaFromMemory()
        {
            if (WebBrowser.Backend == WebBrowserBackend.Edge)
            {
                Log("Memory scheme not supported in Edge backend");
                return;
            }

            void PandaToMemory()
            {
                if (pandaInMemory)
                    return;
                pandaInMemory = true;
                WebBrowser.MemoryFS.AddTextFile(
                    "index.html",
                    "<html><body><b>index.html</b></body></html>");
                WebBrowser.MemoryFS.AddTextFile(
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
                        WebBrowser.MemoryFS.AddOSFile(name, sPath);
                    else
                        WebBrowser.MemoryFS.AddOSFileWithMimeType(name, sPath, mimeType);
                }
            }

            PandaToMemory();
            WebBrowser.LoadURL("memory:Html/page1.html");
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
            DoInvokeScript("alert", WebBrowser.ToInvokeScriptArg(16325.62901F)?.ToString());
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
            WebBrowser.RemoveAllUserScripts();
            WebBrowser.AddUserScript("alert('hello');");
            WebBrowser.Reload();
        }

        internal void DoTestSerializeObject()
        {
            WeatherForecast value = new()
            {
                Date = System.DateTime.Now,
                TemperatureCelsius = 15,
                Summary = "New York",
            };

            var s = WebBrowser.ObjectToJSON(value);

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
            WebBrowser.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }

        internal void TestNavigateToString2()
        {
            WebBrowser.NavigateToString(
                "<html><body>NavigateToString example 2</body></html>", "www.aa.com");
        }

        internal void TestNavigateToStream()
        {
            var filename =
                CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            WebBrowser.NavigateToStream(stream);
        }

        internal void TestMapping()
        {
            if (WebBrowser.Backend != WebBrowserBackend.Edge)
                return;
            if (!mappingSet)
            {
                mappingSet = true;
                WebBrowser.SetVirtualHostNameToFolderMapping(
                    "assets.example",
                    "Html",
                    WebBrowserHostResourceAccessKind.DenyCors);
            }

            WebBrowser.LoadURL("https://assets.example/SampleArchive/Html/page1.html");
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

            scriptMessageHandlerAdded = WebBrowser.AddScriptMessageHandler("wx_msg");
            if (!scriptMessageHandlerAdded)
                Log("AddScriptMessageHandler not supported");
            AddTestActions();
            if (rootPanel.IsIEBackend)
                WebBrowser.DoCommand("IE.SetScriptErrorsSuppressed", "true");

            rootPanel.Parent = this;

            WebBrowser.Loaded += WebBrowser_Loaded;
            WebBrowser.DocumentTitleChanged += WebBrowser_TitleChanged;
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

        private void DoTestZip()
        {
            if (WebBrowser.Backend == WebBrowserBackend.Edge)
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
                WebBrowser.LoadURL(url);
            }
        }

        private void Test()
        {
            DoTestZip();
        }

        private void DoInvokeScript(string scriptName, params object?[] args)
        {
            var clientData = rootPanel.GetNewClientData();
            Log($"InvokeScript {scriptName}({WebBrowser.ToInvokeScriptArgs(args)})");
            WebBrowser.InvokeScriptAsync(scriptName, clientData, args);
        }

        private void DoRunScript(string script)
        {
            var clientData = rootPanel.GetNewClientData();
            Log($"RunScript {clientData} > {script}");
            WebBrowser.RunScriptAsync(script, clientData);
        }

        private void Log(string s)
        {
            rootPanel.Log(s);
        }

        private void WebBrowser_Loaded(object? sender, WebBrowserEventArgs e)
        {
            if (!pandaLoaded)
            {
                rootPanel.WebBrowser.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;

                pandaLoaded = true;
                try
                {
                    rootPanel.WebBrowser.LoadURL(GetPandaUrl());
                }
                catch (Exception)
                {
                }
            }
        }

        private void WebBrowser_TitleChanged(object? sender, WebBrowserEventArgs e)
        {
            var backendVersion = WebBrowser.GetBackendVersionString(rootPanel.WebBrowser.Backend);
            if (rootPanel.IsIEBackend)
                backendVersion = "Internet Explorer";

            Assembly thisAssembly = typeof(WebBrowser).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName.Version;

            Log($"{headerLabelText} {ver} : {e.Text} : {backendVersion}");
        }

        private void ShowBrowserVersion()
        {
            var filename = CommonUtils.PathAddBackslash(CommonUtils.GetAppFolder() + "Html")
                + "version.html";
            rootPanel.WebBrowser.LoadURL("file://" + filename.Replace('\\', '/'));
        }

        private void LogInfo(string s = "")
        {
            void Fn()
            {
                var webBrowser = rootPanel.WebBrowser;

                Log("=======" + s);

                LogUtils.LogProp(webBrowser, "HasSelection", "WebBrowser");
                LogUtils.LogProp(webBrowser, "SelectedText", "WebBrowser");
                LogUtils.LogProp(webBrowser, "SelectedSource", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanCut", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanCopy", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanPaste", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanUndo", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanRedo", "WebBrowser");
                LogUtils.LogProp(webBrowser, "IsBusy", "WebBrowser");
                LogUtils.LogProp(webBrowser, "PageSource", "WebBrowser");
                LogUtils.LogProp(webBrowser, "UserAgent", "WebBrowser");

                LogUtils.LogProp(webBrowser, "PageText", "WebBrowser");
                LogUtils.LogProp(webBrowser, "Zoom", "WebBrowser");
                LogUtils.LogProp(webBrowser, "Editable", "WebBrowser");
                LogUtils.LogProp(webBrowser, "ZoomType", "WebBrowser");
                LogUtils.LogProp(webBrowser, "ZoomFactor", "WebBrowser");

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
            var webBrowser = rootPanel.WebBrowser;

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
            AddTestAction("CanNavigate=false", () => { rootPanel.CanNavigate = false; });
            AddTestAction("CanNavigate=true", () => { rootPanel.CanNavigate = true; });
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

            if (!rootPanel.IsIEBackend)
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
                LogUtils.LogToFileAppFinished();
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