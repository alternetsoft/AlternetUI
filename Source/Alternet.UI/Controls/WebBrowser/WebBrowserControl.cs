using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using static Alternet.UI.EventTrace;

/*
  
enum WebBrowserNavigationActionFlags
{
    None,
    User,
    Other
};
  
class WXDLLIMPEXP_WEBVIEW wxWebViewEvent : public wxNotifyEvent
    wxWebViewEvent() {}
    bool IsError {get;}
    string URL {get;}
    string Target {get;}
    wxWebViewNavigationActionFlags NavigationAction {get;}
    string MessageHandler {get;}
 */


/*
enum WebBrowserNavigationError
{
    CONNECTION,
    CERTIFICATE,
    AUTH,
    SECURITY,
    NOT_FOUND,
    REQUEST,
    USER_CANCELLED,
    OTHER
};

WX_DECLARE_STRING_HASH_MAP(wxSharedPtr<wxWebViewFactory>, wxStringWebViewFactoryMap);
 */


//TODO: KU edge
//TODO: KU linux test
//TODO: KU error info when page opened where in event?
//TODO: KU speed buttons in demo

//TODO: KU wxWebViewEdge::MSWSetBrowserExecutableDir	
//https://docs.wxwidgets.org/3.2/classwx_web_view_edge.html#acf603d714e7a7b5f131097b7a6790c0a


namespace Alternet.UI
{
    //-------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public partial class WebBrowser : Control, IWebBrowser
    {
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static void CrtSetDbgFlag(int value)
        {
            WebBrowserHandler.WebBrowser_CrtSetDbgFlag_(value);
        }
        //-------------------------------------------------
        static WebBrowser()
        {
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        protected override ControlHandler CreateHandler()
        {
            return new WebBrowserHandler();
        }
        //-------------------------------------------------
        new internal WebBrowserHandler Handler => (WebBrowserHandler)base.Handler;
        //-------------------------------------------------
        private void ZoomInOut(int delta)
        {
            var CurrentZoom = Zoom;
            int zoom = (int)CurrentZoom;
            zoom+=delta;
            CurrentZoom = (WebBrowserZoom)Enum.ToObject(typeof(WebBrowserZoom), zoom);
            Zoom = CurrentZoom;
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void ZoomIn()
        {
            if (CanZoomIn)                
                ZoomInOut(1);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(
            "Navigated", 
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent NavigatingEvent = EventManager.RegisterRoutedEvent(
            "Navigating",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent LoadedEvent = EventManager.RegisterRoutedEvent(
            "Loaded",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(
            "Error",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent NewWindowEvent = EventManager.RegisterRoutedEvent(
            "NewWindow",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent TitleChangedEvent = EventManager.RegisterRoutedEvent(
            "TitleChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent FullScreenChangedEvent = EventManager.RegisterRoutedEvent(
            "FullScreenChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent ScriptMessageReceivedEvent = EventManager.RegisterRoutedEvent(
            "ScriptMessageReceived",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent ScriptResultEvent = EventManager.RegisterRoutedEvent(
            "ScriptResult",
            RoutingStrategy.Bubble,
            typeof(EventHandler),
            typeof(WebBrowser));
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler FullScreenChanged
        {
            add
            {
                AddHandler(FullScreenChangedEvent, value);
            }

            remove
            {
                RemoveHandler(FullScreenChangedEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ScriptMessageReceived
        {
            add
            {
                AddHandler(ScriptMessageReceivedEvent, value);
            }

            remove
            {
                RemoveHandler(ScriptMessageReceivedEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ScriptResult
        {
            add
            {
                AddHandler(ScriptResultEvent, value);
            }

            remove
            {
                RemoveHandler(ScriptResultEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Navigated
        {
            add
            {
                AddHandler(NavigatedEvent, value);
            }

            remove
            {
                RemoveHandler(NavigatedEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Navigating
        {
            add
            {
                AddHandler(NavigatingEvent, value);
            }

            remove
            {
                RemoveHandler(NavigatingEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Loaded
        {
            add
            {
                AddHandler(LoadedEvent, value);
            }

            remove
            {
                RemoveHandler(LoadedEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Error
        {
            add
            {
                AddHandler(ErrorEvent, value);
            }

            remove
            {
                RemoveHandler(ErrorEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler NewWindow
        {
            add
            {
                AddHandler(NewWindowEvent, value);
            }

            remove
            {
                RemoveHandler(NewWindowEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler TitleChanged
        {
            add
            {
                AddHandler(TitleChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TitleChangedEvent, value);
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFullScreenChanged(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FullScreenChangedEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnScriptMessageReceived(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ScriptMessageReceivedEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnScriptResult(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ScriptResultEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnNavigated(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NavigatedEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnNavigating(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NavigatingEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnLoaded(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LoadedEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnError(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ErrorEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnNewWindow(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewWindowEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnTitleChanged(object? sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(TitleChangedEvent));
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Navigate(Uri source)
        {
            LoadURL(source.ToString());
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public Uri Source
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
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static bool IsBackendAvailable(WebBrowserBackend value)
        {
            switch (value)
            {
                case WebBrowserBackend.Default:
                    return true;
                case WebBrowserBackend.IE:
                case WebBrowserBackend.IELatest:
                    return WebBrowserHandler.IsBackendIEAvailable();
                case WebBrowserBackend.Edge:
                    return WebBrowserHandler.IsBackendEdgeAvailable();
                case WebBrowserBackend.WebKit:
                    return WebBrowserHandler.IsBackendWebKitAvailable();
                default:
                    return false;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static void SetBackend(WebBrowserBackend value)
        {
            WebBrowserHandler.WebBrowser_SetBackend_((int)value);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static string GetLibraryVersionString()
        {
            return WebBrowserHandler.WebBrowser_GetLibraryVersionString_();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return WebBrowserHandler.WebBrowser_GetBackendVersionString_((int)value);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static void SetDefaultPage(string url)
        {
            WebBrowserHandler.WebBrowser_SetDefaultPage_(url);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
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
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoBack;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoForward;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void GoBack()
        {
            if (!CanGoBack)
                return;
            CheckDisposed();
            Handler.GoBack();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void GoForward()
        {
            if (!CanGoForward)
                return;
            CheckDisposed();
            Handler.GoForward();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            CheckDisposed();
            Handler.Stop();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void ClearHistory()
        {
            CheckDisposed();
            Handler.ClearHistory();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Handler.EnableHistory(enable);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            CheckDisposed();
            Handler.Reload();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Reload(bool noCache)
        {
            CheckDisposed();
            Handler.Reload(noCache);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void NavigateToString(string text, string baseUrl)
        {
            CheckDisposed();
            Handler.NavigateToString(text, baseUrl);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void NavigateToStream(Stream stream)
        {
            var s = StringFromStream(stream);
            NavigateToString(s);

            string StringFromStream(Stream stream)
            {
                return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void NavigateToString(string text)
        {
            CheckDisposed();
            Handler.NavigateToString(text, String.Empty);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public float ZoomFactor
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomFactor;
            }
            set
            {
                CheckDisposed();
                Handler.ZoomFactor =value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public WebBrowserZoomType ZoomType
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomType;
            }
            set
            {
                CheckDisposed();
                Handler.ZoomType=value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanSetZoomType(WebBrowserZoomType value)
        {
            CheckDisposed();
            return Handler.CanSetZoomType(value);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void SelectAll()
        {
            CheckDisposed();
            Handler.SelectAll(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool HasSelection
        {
            get
            {
                CheckDisposed();
                return Handler.HasSelection;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void DeleteSelection() 
        {
            CheckDisposed();
            Handler.DeleteSelection(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string SelectedText
        {
            get 
            {
                CheckDisposed();
                return Handler.SelectedText; 
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string SelectedSource 
        { 
            get 
            {
                CheckDisposed();
                return Handler.SelectedSource; 
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void ClearSelection() 
        {
            CheckDisposed();
            Handler.ClearSelection(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanCut 
        {
            get 
            {
                CheckDisposed();
                return Handler.CanCut; 
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanCopy 
        { 
            get 
            {
                CheckDisposed();
                return Handler.CanCopy; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanPaste 
        { 
            get 
            {
                CheckDisposed();
                return Handler.CanPaste; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Cut() 
        {
            CheckDisposed();
            Handler.Cut(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Copy() 
        {
            CheckDisposed();
            Handler.Copy(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Paste() 
        {
            CheckDisposed();
            Handler.Paste(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanUndo 
        { 
            get 
            {
                CheckDisposed();
                return Handler.CanUndo; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool CanRedo 
        { 
            get 
            {
                CheckDisposed();
                return Handler.CanRedo; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Undo() 
        {
            CheckDisposed();
            Handler.Undo(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Redo() 
        {
            CheckDisposed();
            Handler.Redo(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public long Find(string text, WebBrowserFindParams? prm=null) 
        {
            CheckDisposed();
            return Handler.Find(text,prm); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy 
        { 
            get 
            {
                CheckDisposed();
                return Handler.IsBusy; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Print() 
        {
            CheckDisposed();
            Handler.Print(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string PageSource 
        { 
            get 
            {
                CheckDisposed();
                return Handler.PageSource; 
            } 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string PageText { 
            get 
            {
                CheckDisposed();
                return Handler.PageText; 
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllUserScripts() 
        {
            CheckDisposed();
            Handler.RemoveAllUserScripts(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //https://docs.wxwidgets.org/3.2/classwx_web_view.html#a2597c3371ed654bf03262ec6d34a0126
        public bool AddScriptMessageHandler(string name) 
        {
            CheckDisposed();
            return Handler.AddScriptMessageHandler(name);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool RemoveScriptMessageHandler(string name) 
        {
            CheckDisposed();
            return Handler.RemoveScriptMessageHandler(name); 
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool AccessToDevToolsEnabled
        {
            get
            {
                CheckDisposed();
                return Handler.AccessToDevToolsEnabled;
            }
            set
            {
                CheckDisposed();
                Handler.AccessToDevToolsEnabled = value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string UserAgent
        {
            get
            {
                CheckDisposed();
                return Handler.UserAgent;
            }
            set
            {
                CheckDisposed();
                Handler.UserAgent = value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool ContextMenuEnabled
        {
            get
            {
                CheckDisposed();
                return Handler.ContextMenuEnabled;
            }
            set
            {
                CheckDisposed();
                Handler.ContextMenuEnabled = value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public WebBrowserBackend Backend
        {
            get
            {
                CheckDisposed();
                return Handler.Backend;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool Editable 
        {
            get
            {
                CheckDisposed();
                return Handler.Editable;
            }
            set
            {
                CheckDisposed();
                Handler.Editable = value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public WebBrowserZoom Zoom 
        {
            get
            {
                CheckDisposed();
                return Handler.Zoom;
            }
            set
            {
                CheckDisposed();
                Handler.Zoom = value;
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool RunScript(string javascript)
        {
            string result;
            return RunScript(javascript, out result);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool RunScript(string javascript,out string result)
        {
            CheckDisposed();
            return Handler.RunScript(javascript,out result);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string? InvokeScript(string scriptName)
        {
            if (scriptName == null)
                return null;
            string result;
            bool ok = RunScript(scriptName+"()",out result);
            if (!ok)
                return null;
            return result;
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool AddUserScript(string javascript, bool injectDocStart)
        {
            CheckDisposed();
            return Handler.AddUserScript(javascript, injectDocStart);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string? ToInvokeScriptArg(object? arg)
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
                    //TODO: KU support object types in arg
                    throw new ArgumentException(
                        "Only primitive types are supported","arg");
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
                    return ((Single)arg).ToString("G",CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return ((double)arg).ToString("G",CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return ((decimal)arg).ToString(CultureInfo.InvariantCulture);
                case TypeCode.DateTime:
                //https://learn.microsoft.com/ru-ru/dotnet/standard/base-types/custom-date-and-time-format-strings
                //https://javascript.info/date
                    return "'" + ((System.DateTime)arg).ToString("yyyy-MM-ddTHH:mm:ss.fffK", 
                        CultureInfo.InvariantCulture) + "'";
                default:
                    break;
            }

            return arg.ToString();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string? InvokeScript(string scriptName, params object?[] args)
        {
            if (scriptName == null)
                return null;
            if (args == null || args.Length == 0)
                return InvokeScript(scriptName);

            string? s = ToInvokeScriptArg(args[0]);

            for(int i=1;i<args.Length; i++)
            {
                var arg = ToInvokeScriptArg(args[i]);
                s += (','+arg);
            }

            return InvokeScript(scriptName + "("+s+")");

        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void LoadURL(string url)
        {
            CheckDisposed();
            Handler.LoadURL(url);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string GetCurrentTitle()
        {
            CheckDisposed();
            return Handler.GetCurrentTitle();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string GetCurrentURL()
        {
            CheckDisposed();
            return Handler.GetCurrentURL();
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
        Runs the given JavaScript code asynchronously and returns the result via 
        a wxEVT_WEBVIEW_SCRIPT_RESULT.

        The script result value can be retrieved via 
        wxWebViewEvent::GetString(). If the execution fails 
        wxWebViewEvent::IsError() will return true. 
        In this case additional script execution error information maybe 
        available via wxWebViewEvent::GetString().

        Parameters
        javascript	JavaScript code to execute.
        clientData	Arbirary pointer to data that can be retrieved from the result event.
        Note
        The IE backend does not support async script execution.
        Since 3.1.6 

        void RunScriptAsync(const wxString& javascript, void* clientData = NULL);
         */
        public void RunScriptAsync(string javascript, IntPtr clientData) 
        {
            CheckDisposed();
            Handler.RunScriptAsync(javascript, clientData);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void RunScriptAsync(string javascript)
        {
            RunScriptAsync(javascript, IntPtr.Zero);
        }
        //-------------------------------------------------
    }
    //-------------------------------------------------
}
//-------------------------------------------------
