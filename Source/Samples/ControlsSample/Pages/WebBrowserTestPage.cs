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

namespace ControlsSample
{
    internal partial class WebBrowserTestPage : Panel
    {
        private static readonly string ZipSchemeName = "zipfs";
        private static readonly bool SetDefaultUserAgent = false;
        private readonly PanelWebBrowser rootPanel;
        private readonly string headerLabelText = "Web Browser Control";

        private bool pandaInMemory = false;
        private bool mappingSet = false;
        private bool pandaLoaded = false;
        private bool scriptMessageHandlerAdded = false;

        static WebBrowserTestPage()
        {
            WebBrowser.SetDefaultFSNameMemory("memory");
            WebBrowser.SetDefaultFSNameArchive(ZipSchemeName);
            if (SetDefaultUserAgent)
                WebBrowser.SetDefaultUserAgent("Mozilla");
            PanelWebBrowser.SetBackendPathSmart("Edge");
        }

        public WebBrowserTestPage()
        {
            rootPanel = new(GetPandaUrl())
            {
                LogEvents = false,
                Visible = true,
                Name = "PanelWebBrowser",
            };

            rootPanel.ActionsControl.Required();
            rootPanel.Parent = this;

            RunWhenIdle(() =>
            {
                scriptMessageHandlerAdded = WebBrowser.AddScriptMessageHandler("wx_msg");
                if (scriptMessageHandlerAdded)
                    Log($"{PageName}: AddScriptMessageHandler added");
                else
                    Log($"{PageName}: AddScriptMessageHandler not supported");
                AddTestActions();
                if (rootPanel.IsIEBackend)
                    WebBrowser.DoCommand("IE.SetScriptErrorsSuppressed", "true");

                WebBrowser.Loaded += WebBrowser_Loaded;
                WebBrowser.DocumentTitleChanged += WebBrowser_TitleChanged;
            });
        }

        public PanelWebBrowser PanelWebBrowser => rootPanel;

        public bool LogTitleChanged => false;

        public WebBrowser WebBrowser => rootPanel.WebBrowser;

        public string? PageName { get; set; }

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
                WebBrowser.MemoryFS.AddString(
                    "index.html",
                    "<html><body><b>index.html</b></body></html>");
                WebBrowser.MemoryFS.AddString(
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
                    WebBrowser.MemoryFS.Add(name, sPath, mimeType);
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

        internal void DoTestScrollToBottomAsyncJs()
        {
            WebBrowser.ScrollToBottomAsyncJs();
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

        internal void DoTestNavigateToString1()
        {
            WebBrowser.NavigateToString("<html><body>NavigateToString example 1</body></html>");
        }

        internal void DoTestNavigateToString2()
        {
            WebBrowser.NavigateToString(
                "<html><body>NavigateToString example 2</body></html>", "www.aa.com");
        }

        internal void DoTestNavigateToString3()
        {
            string html = "<html><body><h1>Sample HTML File</h1><p>First Sample paragraph.</p>"
                + "<p>Second Sample paragraph.</p></body></html>";
            WebBrowser.NavigateToString(html);
        }

        internal void DoTestNavigateToStream()
        {
            var filename =
                PathUtils.AddDirectorySeparatorChar(CommonUtils.GetAppFolder() + "Html") + "version.html";
            FileStream stream = File.OpenRead(filename);
            WebBrowser.NavigateToStream(stream);
        }

        internal string? PrepareControlsSampleUrl(string suffix)
        {
            var path = App.StartupPath;
            if (path is null)
                return null;
            var s = WebBrowser.PrepareFileUrl(Path.Combine(path, suffix));
            return s;
        }

        internal void DoTestOpenPageAnimation()
        {
            var s = PrepareControlsSampleUrl("Resources/Animation/animation.html");
            WebBrowser.LoadUrlOrSearch(s);
        }

        internal void DoTestOpenPageImage()
        {
            var s = PrepareControlsSampleUrl("Html/SampleArchive/Images/panda1.jpg");
            WebBrowser.LoadUrlOrSearch(s);
        }

        internal void DoTestOpenPageMP3()
        {
            var s = PrepareControlsSampleUrl("Resources/Sounds/Mp3/sounds.html");
            WebBrowser.LoadUrlOrSearch(s);
        }

        internal void DoTestOpenPageWav()
        {
            var s = PrepareControlsSampleUrl("Resources/Sounds/Wav/sounds.html");
            WebBrowser.LoadUrlOrSearch(s);
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

            if (Parent == null || StateFlags.HasFlag(ControlFlags.ParentAssigned))
                return;
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
            App.Log(s);
        }

        private void WebBrowser_TitleChanged(object? sender, WebBrowserEventArgs e)
        {
            if (!LogTitleChanged)
                return;

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
            var filename = PathUtils.AddDirectorySeparatorChar(CommonUtils.GetAppFolder() + "Html")
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
                LogUtils.LogPropLimitLength(webBrowser, "SelectedText", "WebBrowser");
                LogUtils.LogPropLimitLength(webBrowser, "SelectedSource", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanCut", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanCopy", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanPaste", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanUndo", "WebBrowser");
                LogUtils.LogProp(webBrowser, "CanRedo", "WebBrowser");
                LogUtils.LogProp(webBrowser, "IsBusy", "WebBrowser");
                LogUtils.LogPropLimitLength(webBrowser, "PageSource", "WebBrowser");
                LogUtils.LogProp(webBrowser, "UserAgent", "WebBrowser");

                LogUtils.LogPropLimitLength(webBrowser, "PageText", "WebBrowser");
                LogUtils.LogProp(webBrowser, "Zoom", "WebBrowser");
                LogUtils.LogProp(webBrowser, "Editable", "WebBrowser");
                LogUtils.LogProp(webBrowser, "ZoomType", "WebBrowser");
                LogUtils.LogProp(webBrowser, "ZoomFactor", "WebBrowser");

                Log("DPI = " + webBrowser.GetDPI().ToString());
                Log("isEdge = " + webBrowser.IsEdgeBackend);
                Log("isDebug = " + WebBrowser.DoCommandGlobal("IsDebug"));
                Log("backend = " + webBrowser.Backend.ToString());
                Log("wxWidgetsVersion = " + WebBrowser.GetLibraryVersionString());
                Log("GetCurrentTitle() = " + webBrowser.GetCurrentTitle());
                Log("GetCurrentURL() = " + webBrowser.GetCurrentURL());
                Log("Support IE = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.IE));
                Log("Support Edge = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge));
                Log("Support WebKit = " + WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit));
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

            Fn();
        }

        private void AddTestAction(string? name = null, Action? action = null)
        {
            if (name == null)
            {
                rootPanel.ActionsControl.AddActionSpacer(true);
                return;
            }

            rootPanel.ActionsControl.AddAction(name, action);
        }

        private void AddTestActions()
        {
            rootPanel.ActionsControl.DoInsideUpdate(AddTestActionsInternal);
            
            void AddTestActionsInternal()
            {
                var webBrowser = rootPanel.WebBrowser;

                AddTestAction("Open Panda sample", () =>
                {
                    webBrowser.LoadURL(GetPandaUrl());
                });

                AddTestAction("Google", () =>
                {
                    webBrowser.LoadURL("https://www.google.com");
                });

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
                    AddTestAction(item.Name.Substring(6), AssemblyUtils.CreateAction(this, item));
                }
            }
        }

        private void WebBrowser_Loaded(object? sender, WebBrowserEventArgs e)
        {
            if (!pandaLoaded)
            {
                rootPanel.Visible = true;

                // Under Windows 'Black' scheme for other controls is not implemented
                // so we turn on Light scheme in browser.
                if (App.IsWindowsOS)
                    rootPanel.WebBrowser.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;
            }

            pandaLoaded = true;
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