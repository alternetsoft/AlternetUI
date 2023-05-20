using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static Alternet.UI.EventTrace;

namespace Alternet.UI
{
    /// <include file="Interfaces/IWebBrowser.xml" path='doc/WebBrowser/*'/>
    public partial class WebBrowser : Control, IWebBrowser
    {
        private static string stringFormatJs = "yyyy-MM-ddTHH:mm:ss.fffK";

        private IWebBrowserMemoryFS? fMemoryFS;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/FullScreenChanged/*'/>
        public event EventHandler<WebBrowserEventArgs>? FullScreenChanged;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/ScriptMessageReceived/*'/>
        public event EventHandler<WebBrowserEventArgs>? ScriptMessageReceived;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/ScriptResult/*'/>
        public event EventHandler<WebBrowserEventArgs>? ScriptResult;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Navigated/*'/>
        public event EventHandler<WebBrowserEventArgs>? Navigated;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Navigating/*'/>
        public event EventHandler<WebBrowserEventArgs>? Navigating;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/BeforeBrowserCreate/*'/>
        public event EventHandler<WebBrowserEventArgs>? BeforeBrowserCreate;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Loaded/*'/>
        public event EventHandler<WebBrowserEventArgs>? Loaded;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Error/*'/>
        public event EventHandler<WebBrowserEventArgs>? Error;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/NewWindow/*'/>
        public event EventHandler<WebBrowserEventArgs>? NewWindow;

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/Events/DocumentTitleChanged/*'/>
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Is64Bit/*'/>
        public static bool Is64Bit
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/StringFormatJs/*'/>
        public static string StringFormatJs
        {
            get => stringFormatJs;
            set => stringFormatJs = value;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/MemoryFS/*'/>
        public IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                fMemoryFS ??= new WebBrowserMemoryFS(this);
                return fMemoryFS;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/HasSelection/*'/>
        public virtual bool HasSelection
        {
            get
            {
                CheckDisposed();
                return Browser.HasSelection;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomFactor/*'/>
        public virtual float ZoomFactor
        {
            get
            {
                CheckDisposed();
                return Browser.ZoomFactor;
            }

            set
            {
                CheckDisposed();
                Browser.ZoomFactor = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomType/*'/>
        public virtual WebBrowserZoomType ZoomType
        {
            get
            {
                CheckDisposed();
                return Browser.ZoomType;
            }

            set
            {
                CheckDisposed();
                Browser.ZoomType = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Source/*'/>
        public virtual Uri Source
        {
            get
            {
                return new Uri(GetCurrentURL());
            }

            set
            {
                Navigate(value);
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        public virtual bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanGoBack/*'/>
        public virtual bool CanGoBack
        {
            get
            {
                CheckDisposed();
                return Browser.CanGoBack;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanUndo/*'/>
        public bool CanUndo
        {
            get
            {
                CheckDisposed();
                return Browser.CanUndo;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanRedo/*'/>
        public virtual bool CanRedo
        {
            get
            {
                CheckDisposed();
                return Browser.CanRedo;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AccessToDevToolsEnabled/*'/>
        public virtual bool AccessToDevToolsEnabled
        {
            get
            {
                CheckDisposed();
                return Browser.AccessToDevToolsEnabled;
            }

            set
            {
                CheckDisposed();
                Browser.AccessToDevToolsEnabled = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/UserAgent/*'/>
        public virtual string UserAgent
        {
            get
            {
                CheckDisposed();
                return Browser.UserAgent;
            }

            set
            {
                CheckDisposed();
                Browser.UserAgent = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ContextMenuEnabled/*'/>
        public virtual bool ContextMenuEnabled
        {
            get
            {
                CheckDisposed();
                return Browser.ContextMenuEnabled;
            }

            set
            {
                CheckDisposed();
                Browser.ContextMenuEnabled = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Backend/*'/>
        public virtual WebBrowserBackend Backend
        {
            get
            {
                CheckDisposed();
                return Browser.Backend;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Editable/*'/>
        public virtual bool Editable
        {
            get
            {
                CheckDisposed();
                return Browser.Editable;
            }

            set
            {
                CheckDisposed();
                Browser.Editable = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Zoom/*'/>
        public virtual WebBrowserZoom Zoom
        {
            get
            {
                CheckDisposed();
                return Browser.Zoom;
            }

            set
            {
                CheckDisposed();
                Browser.Zoom = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/IsBusy/*'/>
        public virtual bool IsBusy
        {
            get
            {
                CheckDisposed();
                return Browser.IsBusy;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/PageSource/*'/>
        public virtual string PageSource
        {
            get
            {
                CheckDisposed();
                return Browser.PageSource;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/PageText/*'/>
        public virtual string PageText
        {
            get
            {
                CheckDisposed();
                return Browser.PageText;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectedText/*'/>
        public virtual string SelectedText
        {
            get
            {
                CheckDisposed();
                return Browser.SelectedText;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectedSource/*'/>
        public virtual string SelectedSource
        {
            get
            {
                CheckDisposed();
                return Browser.SelectedSource;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanCut/*'/>
        public virtual bool CanCut
        {
            get
            {
                CheckDisposed();
                return Browser.CanCut;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        /// path='doc/PreferredColorScheme/*'/>
        public virtual WebBrowserPreferredColorScheme PreferredColorScheme
        {
            get
            {
                CheckDisposed();
                return Browser.PreferredColorScheme;
            }

            set
            {
                CheckDisposed();
                Browser.PreferredColorScheme = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanCopy/*'/>
        public virtual bool CanCopy
        {
            get
            {
                CheckDisposed();
                return Browser.CanCopy;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanPaste/*'/>
        public virtual bool CanPaste
        {
            get
            {
                CheckDisposed();
                return Browser.CanPaste;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanGoForward/*'/>
        public virtual bool CanGoForward
        {
            get
            {
                CheckDisposed();
                return Browser.CanGoForward;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomOut/*'/>
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }

        internal new WebBrowserHandler Handler => (WebBrowserHandler)base.Handler;

        internal IWebBrowserLite Browser => (IWebBrowserLite)base.Handler;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetBackendOS/*'/>
        public static WebBrowserBackendOS GetBackendOS()
        {
            return (WebBrowserBackendOS)Enum.ToObject(
                typeof(WebBrowserBackendOS),
                Native.WebBrowser.GetBackendOS());
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetBackendPath/*'/>
        public static void SetBackendPath(string path, bool isRelative = false)
        {
            if (isRelative)
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string s = Path.GetDirectoryName(location)!;
                string s2 = Path.Combine(s, path);
                path = s2;
            }

            if (!Directory.Exists(path))
                return;

            var files = Directory.EnumerateFileSystemEntries(path, "*.dll");

            #pragma warning disable IDE0059 // Unnecessary assignment of a value
            foreach (string file in files)
            {
                Native.WebBrowser.SetEdgePath(path);
                break;
            }
            #pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommandGlobal/*'/>
        public static string DoCommandGlobal(string cmdName, params object?[] args)
        {
            return WebBrowserHandler.DoCommandGlobal(cmdName, args);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CrtSetDbgFlag/*'/>
        public static void CrtSetDbgFlag(int value)
        {
            WebBrowserHandlerApi.WebBrowser_CrtSetDbgFlag_(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/IsBackendAvailable/*'/>
        public static bool IsBackendAvailable(WebBrowserBackend value)
        {
            return value switch
            {
                WebBrowserBackend.Default => true,
                WebBrowserBackend.IE or WebBrowserBackend.IELatest =>
                    WebBrowserHandler.IsBackendIEAvailable(),
                WebBrowserBackend.Edge =>
                    WebBrowserHandler.IsBackendEdgeAvailable(),
                WebBrowserBackend.WebKit =>
                    WebBrowserHandler.IsBackendWebKitAvailable(),
                _ => false,
            };
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetBackend/*'/>
        public static void SetBackend(WebBrowserBackend value)
        {
            if (value == WebBrowserBackend.IE || value == WebBrowserBackend.IELatest
                || value == WebBrowserBackend.Edge)
            {
                if (WebBrowser.GetBackendOS() != WebBrowserBackendOS.Windows)
                    value = WebBrowserBackend.Default;
            }

            WebBrowserHandlerApi.WebBrowser_SetBackend_((int)value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetLibraryVersionString/*'/>
        public static string GetLibraryVersionString()
        {
            return WebBrowserHandlerApi.WebBrowser_GetLibraryVersionString_();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetBackendVersionString/*'/>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return WebBrowserHandlerApi.WebBrowser_GetBackendVersionString_((int)value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultUserAgent/*'/>
        public static void SetDefaultUserAgent(string value)
        {
            Native.WebBrowser.SetDefaultUserAgent(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultScriptMesageName/*'/>
        public static void SetDefaultScriptMesageName(string value)
        {
            Native.WebBrowser.SetDefaultScriptMesageName(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultFSNameMemory/*'/>
        public static void SetDefaultFSNameMemory(string value)
        {
            Native.WebBrowser.SetDefaultFSNameMemory(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultFSNameArchive/*'/>
        public static void SetDefaultFSNameArchive(string value)
        {
            Native.WebBrowser.SetDefaultFSNameArchive(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetDefaultPage/*'/>
        public static void SetDefaultPage(string url)
        {
            WebBrowserHandlerApi.WebBrowser_SetDefaultPage_(url);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetLatestBackend/*'/>
        public static void SetLatestBackend()
        {
            if (IsBackendAvailable(WebBrowserBackend.Edge))
            {
                SetBackend(WebBrowserBackend.Edge);
                return;
            }

            if (IsBackendAvailable(WebBrowserBackend.IELatest))
            {
                SetBackend(WebBrowserBackend.IELatest);
                return;
            }

            if (IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                SetBackend(WebBrowserBackend.WebKit);
                return;
            }

            SetBackend(WebBrowserBackend.Default);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/LoadUrlOrSearch/*'/>
        public void LoadUrlOrSearch(string url)
        {
            if (url == null)
            {
                LoadURL();
                return;
            }

            Uri uri;

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                uri = new Uri(url);
            else
#pragma warning disable CA1847 // Use char literal for a single character lookup
            if (!url.Contains(" ") && url.Contains("."))
            {
                // An invalid URI contains a dot and no spaces,
                // try tacking http:// on the front.
                uri = new Uri("http://" + url);
            }
            else
            {
                // Otherwise treat it as a web search.
                uri = new Uri("https://google.com/search?q=" +
                    string.Join("+", Uri.EscapeDataString(url).Split(
                        new string[] { "%20" },
                        StringSplitOptions.RemoveEmptyEntries)));
            }
#pragma warning restore CA1847 // Use char literal for a single character lookup

            LoadURL(uri.ToString());
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommand/*'/>
        public string DoCommand(string cmdName, params object?[] args)
        {
            CheckDisposed();
            return Browser.DoCommand(cmdName, args);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomIn/*'/>
        public virtual void ZoomIn()
        {
            if (CanZoomIn)
                ZoomInOut(1);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomOut/*'/>
        public virtual void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }

        /*public static readonly RoutedEvent NavigatedEvent =
            EventManager.RegisterRoutedEvent(
            "Navigated",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent NavigatingEvent =
            EventManager.RegisterRoutedEvent(
            "Navigating",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent LoadedEvent = EventManager.RegisterRoutedEvent(
            "Loaded",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(
            "Error",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent NewWindowEvent =
            EventManager.RegisterRoutedEvent(
            "NewWindow",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent DocumentTitleChangedEvent = EventManager.RegisterRoutedEvent(
            "DocumentTitleChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent FullScreenChangedEvent = EventManager.RegisterRoutedEvent(
            "FullScreenChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent ScriptMessageReceivedEvent =
            EventManager.RegisterRoutedEvent(
            "ScriptMessageReceived",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /*public static readonly RoutedEvent ScriptResultEvent = EventManager.RegisterRoutedEvent(
            "ScriptResult",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/

        /// <summary>
        ///     Raises the <see cref="ScriptResult"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnScriptResult(WebBrowserEventArgs e)
        {
            ScriptResult?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Error"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnError(WebBrowserEventArgs e)
        {
            Error?.Invoke(this, e);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Navigate_string/*'/>
        public void Navigate(string urlString)
        {
            LoadURL(urlString);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Navigate_uri/*'/>
        public void Navigate(Uri url)
        {
            if (url == null)
                LoadURL();
            else
                LoadURL(url.ToString());
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GoBack/*'/>
        public virtual bool GoBack()
        {
            if (!CanGoBack)
                return false;
            CheckDisposed();
            return Browser.GoBack();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GoForward/*'/>
        public virtual bool GoForward()
        {
            if (!CanGoForward)
                return false;
            CheckDisposed();
            return Browser.GoForward();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Stop/*'/>
        public virtual void Stop()
        {
            CheckDisposed();
            Browser.Stop();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ClearHistory/*'/>
        public virtual void ClearHistory()
        {
            CheckDisposed();
            Browser.ClearHistory();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/EnableHistory/*'/>
        public virtual void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Browser.EnableHistory(enable);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Reload/*'/>
        public virtual void Reload()
        {
            CheckDisposed();
            Browser.Reload();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Reload_noCache/*'/>
        public virtual void Reload(bool noCache)
        {
            CheckDisposed();
            Browser.Reload(noCache);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/NavigateToString/*'/>
        public virtual void NavigateToString(string html, string? baseUrl = null)
        {
            CheckDisposed();
            if (string.IsNullOrEmpty(html))
            {
                LoadURL();
                return;
            }

            Browser.NavigateToString(html, baseUrl!);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/NavigateToStream/*'/>
        public virtual void NavigateToStream(Stream stream)
        {
            if (stream == null)
            {
                this.LoadURL("about:blank");
                return;
            }

            var s = StringFromStream(stream);
            NavigateToString(s);

            static string StringFromStream(Stream stream)
            {
                return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanSetZoomType/*'/>
        public virtual bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            CheckDisposed();
            return Browser.CanSetZoomType(zoomType);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectAll/*'/>
        public virtual void SelectAll()
        {
            CheckDisposed();
            Browser.SelectAll();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DeleteSelection/*'/>
        public virtual void DeleteSelection()
        {
            CheckDisposed();
            Browser.DeleteSelection();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Undo/*'/>
        public virtual void Undo()
        {
            if (!CanUndo)
                return;
            CheckDisposed();
            Browser.Undo();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Redo/*'/>
        public virtual void Redo()
        {
            if (!CanRedo)
                return;
            CheckDisposed();
            Browser.Redo();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ClearSelection/*'/>
        public virtual void ClearSelection()
        {
            CheckDisposed();
            Browser.ClearSelection();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Cut/*'/>
        public virtual void Cut()
        {
            if (!CanCut)
                return;
            CheckDisposed();
            Browser.Cut();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Copy/*'/>
        public virtual void Copy()
        {
            if (!CanCopy)
                return;
            CheckDisposed();
            Browser.Copy();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Paste/*'/>
        public virtual void Paste()
        {
            if (!CanPaste)
                return;
            CheckDisposed();
            Browser.Paste();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Find/*'/>
        public virtual int Find(string text, WebBrowserFindParams? prm = null)
        {
            CheckDisposed();
            return Browser.Find(text, prm);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/FindClearResult/*'/>
        public virtual void FindClearResult()
        {
            CheckDisposed();
            Browser.FindClearResult();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Print/*'/>
        public virtual void Print()
        {
            CheckDisposed();
            Browser.Print();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RemoveAllUserScripts/*'/>
        public virtual void RemoveAllUserScripts()
        {
            CheckDisposed();
            Browser.RemoveAllUserScripts();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AddScriptMessageHandler/*'/>
        public virtual bool AddScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Browser.AddScriptMessageHandler(name);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RemoveScriptMessageHandler/*'/>
        public virtual bool RemoveScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Browser.RemoveScriptMessageHandler(name);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AddUserScript/*'/>
        public virtual bool AddUserScript(
            string javascript,
            bool injectDocStart = true)
        {
            CheckDisposed();
            return Browser.AddUserScript(javascript, injectDocStart);
        }

        /// <summary>
        ///     Converts the value into a JSON string. It is a simple convertor.
        ///     It is better to use System.Text.Json.JsonSerializer.Serialize method.
        /// </summary>
        /// <param name="v">
        ///     The value to convert.
        /// </param>
        /// <returns>
        ///     The JSON string representation of the value.
        /// </returns>
        /// <remarks>
        ///     Do not call it for primitive values. It supposed to convert only
        ///     TypeCode.Object values. Outputs result like this:
        ///     {firstName:"John", lastName:"Doe", age:50, eyeColor:"blue"}
        /// </remarks>
        public virtual string ObjectToJSON(object? v)
        {
            if (v == null)
                return "null";

            string result = "{";

            var t = v.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                if (result.Length > 1)
                    result += ", ";
                var propValue = ToInvokeScriptArg(prop.GetValue(v));
                result += prop.Name + ": " + propValue;
            }

            result += "}";
            return result;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ToInvokeScriptArgs/*'/>
        public string ToInvokeScriptArgs(object?[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            string? s = ToInvokeScriptArg(args[0]);

            for (int i = 1; i < args.Length; i++)
            {
                var arg = ToInvokeScriptArg(args[i]);
                s += ',' + arg;
            }

            return s!;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/InvokeScriptAsync/*'/>
        public virtual void InvokeScriptAsync(
            string scriptName,
            IntPtr clientData,
            params object?[] args)
        {
            if (scriptName == null)
                return;
            if (args == null || args.Length == 0)
            {
                RunScriptAsync(scriptName + "()");
                return;
            }

            string? s = ToInvokeScriptArgs(args!);

            RunScriptAsync(scriptName + "(" + s + ")", clientData);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ToInvokeScriptArg/*'/>
        public virtual string? ToInvokeScriptArg(object? arg)
        {
            if (arg == null)
                return "null";

            TypeCode tcode = Type.GetTypeCode(arg.GetType());

            switch (tcode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return "null";
                case TypeCode.Object:
                    return ObjectToJSON(arg);
                case TypeCode.Boolean:
                    return ((bool)arg).ToString().ToLower();
                case TypeCode.Char:
                case TypeCode.String:
                    return "'" + arg.ToString() + "'";
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return arg.ToString();
                case TypeCode.Single:
                    return ((float)arg).ToString(
                        "G",
                        CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return ((double)arg).ToString(
                        "G",
                        CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return ((decimal)arg).ToString(CultureInfo.InvariantCulture);
                case TypeCode.DateTime:
                    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                    // https://javascript.info/date
                    return "'" + ((System.DateTime)arg).ToString(
                        WebBrowser.StringFormatJs,
                        CultureInfo.InvariantCulture) + "'";
                default:
                    break;
            }

            return arg.ToString();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/LoadURL/*'/>
        public virtual void LoadURL(string? url = null)
        {
            url ??= "about:blank";
            CheckDisposed();
            Browser.LoadURL(url);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetCurrentTitle/*'/>
        public virtual string GetCurrentTitle()
        {
            CheckDisposed();
            return Browser.GetCurrentTitle();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetCurrentURL/*'/>
        public virtual string GetCurrentURL()
        {
            CheckDisposed();
            return Browser.GetCurrentURL();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetVirtualHostNameToFolderMapping/*'/>
        public virtual void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind)
        {
            CheckDisposed();
            Browser.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetNativeBackend/*'/>
        public IntPtr GetNativeBackend()
        {
            CheckDisposed();
            return Browser.GetNativeBackend();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScriptAsync/*'/>
        public virtual void RunScriptAsync(
            string javascript,
            IntPtr? clientData = null)
        {
            CheckDisposed();
            Browser.RunScriptAsync(javascript, clientData);
        }

        internal void OnNativeScriptMessageReceived(
           object? sender,
           Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.ScriptMessageReceived);
            OnScriptMessageReceived(ea);
        }

        internal void OnNativeFullScreenChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.FullScreenChanged);
            OnFullScreenChanged(ea);
        }

        internal virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }

        internal virtual bool RunScript(string javascript, out string result)
        {
            CheckDisposed();
            return Handler.RunScript(javascript, out result);
        }

        internal virtual string? InvokeScript(string scriptName)
        {
            if (scriptName == null)
                return null;
            bool ok = RunScript(scriptName + "()", out string result);
            if (!ok)
                return null;
            return result;
        }

        internal virtual string? InvokeScript(string scriptName, params object?[] args)
        {
            if (scriptName == null)
                return null;
            if (args == null || args.Length == 0)
                return InvokeScript(scriptName);

            string? s = ToInvokeScriptArgs(args);

            bool ok = RunScript(scriptName + "(" + s + ")", out string result);
            if (!ok)
                return null;
            return result;
        }

        internal void OnNativeScriptResult(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.ScriptResult);
            OnScriptResult(ea);
        }

        internal void OnNativeNavigated(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.Navigated);
            OnNavigated(ea);
        }

        internal void OnNativeNavigating(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.Navigating);
            OnNavigating(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        internal void OnNativeBeforeBrowserCreate(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.BeforeBrowserCreate);
            OnBeforeBrowserCreate(ea);
        }

        internal void OnNativeLoaded(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.Loaded);
            OnLoaded(ea);
        }

        internal void OnNativeError(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.Error);
            OnError(ea);
        }

        internal void OnNativeNewWindow(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.NewWindow);
            OnNewWindow(ea);
        }

        internal void OnNativeTitleChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new (e, WebBrowserEvent.TitleChanged);
            OnDocumentTitleChanged(ea);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CreateHandler/*'/>
        protected override ControlHandler CreateHandler()
        {
            return new WebBrowserHandler();
        }

        /// <summary>
        ///     Raises the <see cref="DocumentTitleChanged"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDocumentTitleChanged(WebBrowserEventArgs e)
        {
            DocumentTitleChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ScriptMessageReceived"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptMessageReceived(WebBrowserEventArgs e)
        {
            ScriptMessageReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="FullScreenChanged"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnFullScreenChanged(WebBrowserEventArgs e)
        {
            FullScreenChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Navigated"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigated(WebBrowserEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Navigating"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigating(WebBrowserEventArgs e)
        {
            Navigating?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="BeforeBrowserCreate"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnBeforeBrowserCreate(WebBrowserEventArgs e)
        {
            BeforeBrowserCreate?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:Loaded"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnLoaded(WebBrowserEventArgs e)
        {
            Loaded?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="NewWindow"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNewWindow(WebBrowserEventArgs e)
        {
            NewWindow?.Invoke(this, e);
        }

        private void ZoomInOut(int delta)
        {
            var currentZoom = Zoom;
            int zoom = (int)currentZoom;
            zoom += delta;
            currentZoom = (WebBrowserZoom)Enum.ToObject(typeof(WebBrowserZoom), zoom);
            Zoom = currentZoom;
        }
    }
}