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
using static Alternet.UI.EventTrace;

namespace Alternet.UI
{
    
    /// <summary>
    ///     <para>
    ///         This control may be used to render full featured web documents. 
    ///         It supports using multiple backends, corresponding to different 
    ///         implementations of the same functionality.
    ///     </para>
    ///     <para>
    ///         Each backend is a full rendering engine (Internet Explorer, Edge or WebKit). 
    ///         This allows the correct viewing of complex web pages with full JavaScript 
    ///         and CSS support. Under macOS and Unix platforms a single backend is provided 
    ///         (WebKit-based). Under MSW both the old IE backends and the new Edge 
    ///         backend can be used. 
    ///     </para>
    ///     
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         WebBrowser has many asynchronous methods. They return immediately and 
    ///         perform their work in the background. This includes functions such as Reload() 
    ///         and LoadURL(). 
    ///     </para>
    ///     <para>
    ///         To receive notification of the progress and completion of these functions
    ///         you need to handle the events that are provided. 
    ///         Specifically Loaded event notifies when the page or a sub-frame
    ///         has finished loading and Error event notifies that an error has occurred.
    ///     </para>
    /// </remarks>
    public partial class WebBrowser : Control, IWebBrowser
    {
        
        private IWebBrowserMemoryFS? FMemoryFS;
        new internal WebBrowserHandler Handler => (WebBrowserHandler)base.Handler;
        internal IWebBrowserLite Browser => (IWebBrowserLite)base.Handler;
        
        /// <summary>
        ///     Retrieves or modifies the state of the debug flag to control the 
        ///     allocation behavior of the debug heap manager. This is for debug purposes.
        ///     CrtSetDbgFlag(0) allows to turn off debug output with heap manager information.
        /// </summary>
        public static void CrtSetDbgFlag(int value)
        {
            WebBrowserNativeApi.WebBrowser_CrtSetDbgFlag_(value);
        }
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommand/*'/>
        public string DoCommand(string cmdName, params object?[] args)
        {
            CheckDisposed();
            return Browser.DoCommand(cmdName, args);
        }
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommandGlobal/*'/>
        public static string DoCommandGlobal(string cmdName, params object?[] args)
        {
            return WebBrowserHandler.DoCommandGlobal(cmdName, args);
        }
        
        static WebBrowser()
        {
        }
        
        /// <summary>
        ///     Creates a handler for the control.
        /// </summary>
        /// <remarks>
        ///     You typically should not call this method directly.
        ///     The preferred method is to call the <see cref="Control.EnsureHandlerCreated"/> method, 
        ///     which forces a handler to be created for the control.
        /// </remarks>
        protected override ControlHandler CreateHandler()
        {
            return new WebBrowserHandler();
        }
        
        private void ZoomInOut(int delta)
        {
            var CurrentZoom = Zoom;
            int zoom = (int)CurrentZoom;
            zoom+=delta;
            CurrentZoom = (WebBrowserZoom)Enum.ToObject(typeof(WebBrowserZoom), zoom);
            Zoom = CurrentZoom;
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
        
        public IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                FMemoryFS ??= new WebBrowserMemoryFS(this);
                return FMemoryFS;
            }
        }
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        public virtual bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomOut/*'/>
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }
        
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
        
        //TODO: !KU help ?
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
        ///     Raises the <see cref="E:FullScreenChanged"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnFullScreenChanged(WebBrowserEventArgs e)
        {
            FullScreenChanged?.Invoke(this, e);
        }
        
        internal void OnNativeFullScreenChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.FullScreenChanged);
            OnFullScreenChanged(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:ScriptMessageReceived"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptMessageReceived(WebBrowserEventArgs e) 
        {
            ScriptMessageReceived?.Invoke(this, e);
        }
        
        internal void OnNativeScriptMessageReceived(object? sender,
            Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.ScriptMessageReceived);
            OnScriptMessageReceived(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:NativeScriptResult"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnScriptResult(WebBrowserEventArgs e)
        {
            ScriptResult?.Invoke(this, e);
        }
        
        internal void OnNativeScriptResult(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e,WebBrowserEvent.ScriptResult);
            OnScriptResult(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:Navigated"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigated(WebBrowserEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }
        
        internal void OnNativeNavigated(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Navigated);
            OnNavigated(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:Navigating"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigating(WebBrowserEventArgs e)
        {
            Navigating?.Invoke(this, e);
        }
        
        internal void OnNativeNavigating(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Navigating);
            OnNavigating(ea);
            e.Result = ea.CancelAsIntPtr();
        }
        
        /// <summary>
        ///     Raises the <see cref="E:BeforeBrowserCreate"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnBeforeBrowserCreate(WebBrowserEventArgs e)
        {
            BeforeBrowserCreate?.Invoke(this, e);
        }
        
        internal void OnNativeBeforeBrowserCreate(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.BeforeBrowserCreate);
            OnBeforeBrowserCreate(ea);
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
        
        internal void OnNativeLoaded(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Loaded);
            OnLoaded(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:Error"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnError(WebBrowserEventArgs e) 
        {
            Error?.Invoke(this, e);
        }
        
        internal void OnNativeError(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Error);
            OnError(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:NewWindow"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNewWindow(WebBrowserEventArgs e) 
        {
            NewWindow?.Invoke(this, e);
        }
        
        internal void OnNativeNewWindow(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.NewWindow);
            OnNewWindow(ea);
        }
        
        /// <summary>
        ///     Raises the <see cref="E:DocumentTitleChanged"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDocumentTitleChanged(WebBrowserEventArgs e) 
        {
            DocumentTitleChanged?.Invoke(this, e);
        }
        
        internal void OnNativeTitleChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.TitleChanged);
            OnDocumentTitleChanged(ea);
        }
        
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
        
        public event EventHandler<WebBrowserEventArgs>? BeforeBrowserCreate;
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Loaded/*'/>
        public event EventHandler<WebBrowserEventArgs>? Loaded;
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Error/*'/>
        public event EventHandler<WebBrowserEventArgs>? Error;
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/NewWindow/*'/>
        public event EventHandler<WebBrowserEventArgs>? NewWindow;
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/DocumentTitleChanged/*'/>
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;
        
        /// <summary>
        ///     Loads the document at the location indicated by the specified 
        ///     <see cref="T:System.Uri"/> into the <see cref="T:WebBrowser"/> control, 
        ///     replacing the previous document.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="T:System.Uri"/> representing the URL of the document to load.
        ///     If this parameter is null, WebBrowser navigates to a blank document. 
        /// </param>
        public void Navigate(Uri url)
        {
            if (url == null)
                LoadURL();
            else
                LoadURL(url.ToString());
        }
        
        /// <summary>
        ///     Loads the document at the specified Uniform Resource Locator (URL) 
        ///     into the <see cref="T:WebBrowser"/> control, replacing 
        ///     the previous document.
        /// </summary>
        /// <param name="urlString">
        ///     The URL of the document to load. If this parameter is null, WebBrowser 
        ///     navigates to a blank document.
        /// </param>
        public void Navigate(string urlString)
        {
            LoadURL(urlString);
        }
        
        /// <summary>
        ///     Gets or sets the Uri of the current document hosted in the WebBrowser.
        /// </summary>
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
        
        /// <summary>
        ///     Allows to check if a specific backend is currently available.
        ///     This method allows to enable some extra functionality available with 
        ///     the specific backend.
        /// </summary>
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
        
        /// <summary>
        ///     Sets the backend that will be used for the new WebBrowser instances.
        /// </summary>
        public static void SetBackend(WebBrowserBackend value)
        {
            WebBrowserNativeApi.WebBrowser_SetBackend_((int)value);
        }
        
        /// <summary>
        ///     Retrieve the version information about the underlying library implementation.
        /// </summary>
        public static string GetLibraryVersionString()
        {
            return WebBrowserNativeApi.WebBrowser_GetLibraryVersionString_();
        }
        
        /// <summary>
        ///     Retrieve the version information about the browser backend implementation.
        /// </summary>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return WebBrowserNativeApi.WebBrowser_GetBackendVersionString_((int)value);
        }
        
        /// <summary>
        ///     Sets the default web page that will be used for the 
        ///     new WebBrowser instances. Devault value is about:blank.
        /// </summary>
        public static void SetDefaultPage(string url)
        {
            WebBrowserNativeApi.WebBrowser_SetDefaultPage_(url);
        }
        
        /// <summary>
        ///     Sets the best possible backend to be used for the 
        ///     new WebBrowser instances.
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
            if (IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                SetBackend(WebBrowserBackend.WebKit);
                return;
            }
            SetBackend(WebBrowserBackend.Default);
        }
        
        /// <summary>
        ///     Gets a value indicating whether a previous page in navigation history 
        ///     is available, which allows the<see cref="M:GoBack" /> 
        ///     method to succeed.
        ///</summary>
        ///<returns>
        ///  <see langword = "true" /> if the control can navigate backward; otherwise, 
        ///  <see langword = "false" />.
        ///</returns>
        public virtual bool CanGoBack
        {
            get
            {
                CheckDisposed();
                return Browser.CanGoBack;
            }
        }
        
        /// <summary>
        ///     Gets a value indicating whether a subsequent page in navigation
        ///     history is available, which allows the
        ///     <see cref="M:GoForward" /> method to succeed.
        /// </summary>
        /// <returns>
	    ///     <see langword = "true" /> if the control can navigate forward; otherwise, 
        ///     <see langword = "false" />.
        /// </returns>
        public virtual bool CanGoForward
        {
            get
            {
                CheckDisposed();
                return Browser.CanGoForward;
            }
        }
        
        ///<summary>
        ///    Navigates the control 
        ///    to the previous page in the navigation history, if one is available.
        ///</summary>
        ///<returns>
        ///  <see langword="true"/> if the navigation succeeds; 
        ///  <see langword="false"/> if a previous page in the navigation history is not available.
        ///</returns>
        public virtual bool GoBack()
        {
            if (!CanGoBack)
                return false;
            CheckDisposed();
            return Browser.GoBack();
        }
        
        ///<summary>
        ///    Navigates the control 
        ///    to the subsequent page in the navigation history, if one is available.
        ///</summary>
        ///<returns>
        ///  <see langword="true"/> if the navigation succeeds; 
        ///  <see langword="false"/> if a subsequent page in the navigation history is not available.
        ///</returns>
        public virtual bool GoForward()
        {
            if (!CanGoForward)
                return false;
            CheckDisposed();
            return Browser.GoForward();
        }
        
        /// <summary>
        ///     Stops the current page loading process, if any. Cancels any pending navigation 
        ///     and stops any dynamic page elements, such as background sounds and animations.
        /// </summary>
        public virtual void Stop()
        {
            CheckDisposed();
            Browser.Stop();
        }
        
        /// <summary>
        ///     Clear the history, this will also remove the visible page. This is not 
        ///     implemented on macOS and the WebKit2GTK+ backend.         
        /// </summary>
        public virtual void ClearHistory()
        {
            CheckDisposed();
            Browser.ClearHistory();
        }
        
        /// <summary>
        ///     Enables or disables the history. This method will also clear the history.
        ///     This method is not implemented on macOS and the WebKit2GTK+ backend.
        /// </summary>
        public virtual void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Browser.EnableHistory(enable);
        }
        
        /// <summary>
        ///     Reloads the document currently displayed in the control by downloading
        ///     for an updated version from the server.
        /// </summary>
        public virtual void Reload()
        {
            CheckDisposed();
            Browser.Reload();
        }
        
        /// <summary>
        ///     Reloads the document currently displayed in the 
        ///     control using the specified refresh option.    
        /// </summary>
        /// <param name="noCache">
        ///     <see langword="true"/> if the reload will not use browser cache; otherwise, 
        ///     <see langword="false"/>. This parameter is ignored by the Edge backend.         
        /// </param>
        public virtual void Reload(bool noCache)
        {
            CheckDisposed();
            Browser.Reload(noCache);
        }
        
        /// <summary>
        ///     Sets the displayed page source to the contents of the given string.
        /// </summary>
        /// <remarks>
        ///     When using the IE backend you must wait for the current page to finish loading before 
        ///     calling this method. The baseURL parameter is not used in the IE and
        ///     and Edge backends.
        /// </remarks>
        /// <param name="html">
        ///     The string that contains the HTML data to display. If this parameter is null, 
        ///     WebBrowser navigates to a blank document. If this parameter is not in valid HTML 
        ///     format, it will be displayed as plain text.
        /// </param>
        /// <param name="baseUrl">
        ///     URL assigned to the HTML data, to be used to resolve relative paths, for instance.
        /// </param>
        public virtual void NavigateToString(string html, string baseUrl)
        {
            CheckDisposed();
            if (String.IsNullOrEmpty(html))
            {
                LoadURL();
                return;
            }
            Browser.NavigateToString(html, baseUrl);
        }
        
        /// <summary>
        ///     Sets the displayed page source to the contents of the given string.
        /// </summary>
        /// <param name="html">
        ///     The string that contains the HTML data to display. If this parameter is null, 
        ///     WebBrowser navigates to a blank document. If this parameter is not in valid HTML 
        ///     format, it will be displayed as plain text.
        /// </param>
        /// <remarks>
        ///     When using the IE backend you must wait for the current page to finish 
        ///     loading before calling this method. 
        /// </remarks>
        public virtual void NavigateToString(string html)
        {
            CheckDisposed();
            Browser.NavigateToString(html, String.Empty);
        }
        
        /// <summary>
        /// Navigate asynchronously to a Stream that contains the content for a document.
        /// </summary>
        /// <param name="stream">
        ///     The Stream that contains the content for a document.
        ///     If this parameter is null, WebBrowser navigates to a blank document.
        ///     If contents of the stream is not in a valid HTML format, it will 
        ///     be displayed as plain text.
        /// </param>
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
        
        /// <summary>
        ///     Gets or sets the zoom factor of the page. The zoom factor is an arbitrary 
        ///     number that specifies how much to zoom (scale) the HTML document.
        /// </summary>
        /// <remarks>
        ///     Zoom scale in IE will be converted into zoom levels if ZoomType property 
        ///     is set to WebBrowserZoomType.Text value.
        /// </remarks>
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
                Browser.ZoomFactor =value;
            }
        }
        
        /// <summary>
        ///     Gets or sets how the zoom factor is currently interpreted by the HTML engine.
        /// </summary>
        /// <remarks>
        ///     Invoke CanSetZoomType() first, some HTML renderers may not support all zoom types.
        /// </remarks>
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
                Browser.ZoomType=value;
            }
        }
        
        /// <summary>
        ///     Retrieve whether the current HTML engine supports a zoom type.
        /// </summary>
        /// <param name="zoomType">
        ///     The zoom type to test.
        /// </param>
        /// <returns>
        ///     Whether this type of zoom is supported by this HTML engine 
        ///     (and thus can be set through ZoomType property).              
        /// </returns>
        /// 
        public virtual bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            CheckDisposed();
            return Browser.CanSetZoomType(zoomType);
        }
        
        /// <summary>
        ///     Selects the entire page.         
        /// </summary>
        public virtual void SelectAll()
        {
            CheckDisposed();
            Browser.SelectAll(); 
        }
        
        /// <summary>
        ///     Returns true if there is a current selection.         
        /// </summary>
        public virtual bool HasSelection
        {
            get
            {
                CheckDisposed();
                return Browser.HasSelection;
            }
        }
        
        /// <summary>
        ///     Deletes the current selection.
        /// </summary>
        /// <remarks>
        ///     Note that for the Webkit backend the selection must be editable, 
        ///     either through the correct HTML attribute or Editable property.         
        /// </remarks>
        public virtual void DeleteSelection() 
        {
            CheckDisposed();
            Browser.DeleteSelection(); 
        }
        
        /// <summary>
        ///     Gets or sets a value indicating the currently selected text in the control.
        /// </summary>
        public virtual string SelectedText
        {
            get 
            {
                CheckDisposed();
                return Browser.SelectedText; 
            }
        }
        
        /// <summary>
        ///     Returns the currently selected source, if any. 
        /// </summary>
        public virtual string SelectedSource 
        { 
            get 
            {
                CheckDisposed();
                return Browser.SelectedSource; 
            }
        }
        
        /// <summary>
        ///     Clears the current selection. Specifies that no characters are 
        ///     selected in the control.        
        /// </summary>
        public virtual void ClearSelection() 
        {
            CheckDisposed();
            Browser.ClearSelection(); 
        }
        
        /// <summary>
        ///     Gets a value indicating whether the current selection can be cut, which allows 
        ///     the <see cref="M:Cut"/> method to succeed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the current selection can be cut; 
        ///     otherwise, <see langword="false"/>.
        /// </returns>         
        public virtual bool CanCut 
        {
            get 
            {
                CheckDisposed();
                return Browser.CanCut; 
            }
        }
        
        /// <summary>
        ///     Gets a value indicating whether the current selection can be copied, which allows 
        ///     the <see cref="M:Copy"/> method to succeed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the current selection can be copied; 
        ///     otherwise, <see langword="false"/>.
        /// </returns>         
        public virtual bool CanCopy 
        { 
            get 
            {
                CheckDisposed();
                return Browser.CanCopy; 
            } 
        }
        
        /// <summary>
        ///     Gets a value indicating whether the current selection can be replaced with 
        ///     the contents of the Clipboard, which allows 
        ///     the <see cref="M:Paste"/> method to succeed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the data can be pasted; 
        ///     otherwise, <see langword="false"/>.
        /// </returns>         
        public virtual bool CanPaste 
        { 
            get 
            {
                CheckDisposed();
                return Browser.CanPaste; 
            } 
        }
        
        /// <summary>
        ///     Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut() 
        {
            CheckDisposed();
            Browser.Cut(); 
        }
        
        /// <summary>
        ///     Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy() 
        {
            CheckDisposed();
            Browser.Copy(); 
        }
        
        /// <summary>
        ///     Replaces the current selection in the control with the contents of the Clipboard.
        /// </summary>
        public virtual void Paste() 
        {
            CheckDisposed();
            Browser.Paste();
        }
        
        /// <summary>
        ///     Gets a value indicating whether the user can undo the previous operation 
        ///     in a control.
        /// </summary>
        /// <returns>
        ///     <see langword = "true"/> if the user can undo the previous operation performed 
        ///     in a control; otherwise, <see langword = "false"/>.
        /// </returns >
        public bool CanUndo 
        { 
            get 
            {
                CheckDisposed();
                return Browser.CanUndo; 
            } 
        }
        
        /// <summary>
        ///     Gets a value indicating whether the user can redo the previous operation 
        ///     in a control.
        /// </summary>
        /// <returns>
        ///     <see langword = "true" /> if the user can redo the previous operation performed 
        ///     in a control; otherwise, <see langword = "false" />.
        /// </returns >
        public virtual bool CanRedo 
        { 
            get 
            {
                CheckDisposed();
                return Browser.CanRedo; 
            } 
        }
        
        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public virtual void Undo() 
        {
            CheckDisposed();
            Browser.Undo(); 
        }
        
        /// <summary>
        /// Redos the last edit operation in the control.
        /// </summary>
        public virtual void Redo() 
        {
            CheckDisposed();
            Browser.Redo(); 
        }
        
        /// <summary>
        ///     Finds a text on the current page and if found, the control will scroll 
        ///     the text into view and select it.
        /// </summary>
        /*


Parameters
text	The phrase to search for.
prm	    The parameters for the search.
Returns
If search phrase was not found in combination with the flags then wxNOT_FOUND is returned. If called for the first time with search phrase then the total number of results will be returned. Then for every time its called with the same search phrase it will return the number of the current match.
Note
This function will restart the search if the flags wxWEBVIEW_FIND_ENTIRE_WORD or wxWEBVIEW_FIND_MATCH_CASE are changed, since this will require a new search. To reset the search, for example resetting the highlights call the function with an empty search phrase.         
        */
        //TODO: !KU help
        public virtual long Find(string text, WebBrowserFindParams? prm=null) 
        {
            CheckDisposed();
            return Browser.Find(text,prm); 
        }
        
        /// <summary>
        /// 
        /// </summary>
        /*
         
         */        
        //TODO: !KU help
        public virtual void FindClearResultSelection()
        {
            Find("", null);
        }
        
        /// <summary>
        ///     Returns whether the control is currently busy (e.g. loading a web page).         
        /// </summary>
        public virtual bool IsBusy 
        { 
            get 
            {
                CheckDisposed();
                return Browser.IsBusy; 
            } 
        }
        
        /// <summary>
        ///     Opens a print dialog so that the user may change the current print and 
        ///     page settings and print the currently displayed page.         
        /// </summary>
        public virtual void Print() 
        {
            CheckDisposed();
            Browser.Print(); 
        }
        
        /// <summary>
        ///     Get the HTML source code of the currently displayed document or 
        ///     an empty string if no page is currently shown.
        /// </summary>
        public virtual string PageSource 
        { 
            get 
            {
                CheckDisposed();
                return Browser.PageSource; 
            } 
        }
        
        /// <summary>
        ///     Get the text of the current page.      
        /// </summary>
        public virtual string PageText { 
            get 
            {
                CheckDisposed();
                return Browser.PageText; 
            }
        }
        
        /// <summary>
        ///     Removes all user scripts from the web view.
        /// </summary>
        public virtual void RemoveAllUserScripts() 
        {
            CheckDisposed();
            Browser.RemoveAllUserScripts(); 
        }
        
        /// <summary>
        /// 
        /// </summary>
        //https://docs.wxwidgets.org/3.2/classwx_web_view.html#a2597c3371ed654bf03262ec6d34a0126
        /*
Add a script message handler with the given name.

To use the script message handler from javascript use window.<name>.postMessage(<messageBody>) where <name> corresponds the value of the name parameter. The <messageBody> will be available to the application via a wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED event.

Sample C++ code receiving a script message:

// Install message handler with the name wx_msg
m_webView->AddScriptMessageHandler('wx_msg');
// Bind handler
m_webView->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, [](wxWebViewEvent& evt) {
    wxLogMessage("Script message received; value = %s, handler = %s", evt.GetString(), evt.GetMessageHandler());
});
Sample javascript sending a script message:

// Send sample message body
window.wx_msg.postMessage('This is a message body');
Parameters
name	Name of the message handler that can be used from javascript
Returns
true if the handler could be added, false if it could not be added.
See also
RemoveScriptMessageHandler()
Note
The Edge backend only supports a single message handler and the IE backend does not support script message handlers.         
        */
        //TODO: !KU help
        public virtual bool AddScriptMessageHandler(string name) 
        {
            CheckDisposed();
            return Browser.AddScriptMessageHandler(name);
        }
        
        /// <summary>
        ///     Remove a script message handler with the given name that was previously 
        ///     added using AddScriptMessageHandler().
        /// </summary>
        /*
Returns
true if the handler could be removed, false if it could not be removed.
        */
        //TODO: !KU help
        public virtual bool RemoveScriptMessageHandler(string name) 
        {
            CheckDisposed();
            return Browser.RemoveScriptMessageHandler(name); 
        }
        
        /// <summary>
        ///     Enables or disables access to developer tools for the user. 
        ///     Developer tools are disabled by default. 
        ///     This feature is not implemented for the IE backend.         
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        /*
Returns the current user agent string for the web browser.         

Specify a custom user agent string for the web view.

Returns true the user agent could be set.

If your first request should already use the custom user agent please 
        use two step creation and call SetUserAgent() before Create().

Note
This is not implemented for IE. For Edge SetUserAgent() MUST be called before Create().
        */
        //TODO: !KU help
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
        
        /// <summary>
        ///     Enables or disables the right click context menu. 
        ///     By default the standard context menu is enabled, this property 
        ///     can be used to disable it or re-enable it later.
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
        public virtual WebBrowserBackend Backend
        {
            get
            {
                CheckDisposed();
                return Browser.Backend;
            }
        }
        
        /// <summary>
        ///     Gets or sets whether the control is currently editable. 
        ///     This property allows the user to edit the page even 
        ///     if the contenteditable attribute is not set in HTML. 
        ///     The exact capabilities vary with the backend being used.
        ///     This feature is not implemented for macOS and the Edge backend.
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        /*
Get the zoom level of the page.

See GetZoomFactor() to get more precise zoom scale value other than as provided by wxWebViewZoom.

Returns
The current level of zoom.         

Set the zoom level of the page.

See SetZoomFactor() for more precise scaling other than the measured steps provided by wxWebViewZoom.

Parameters
zoom	How much to zoom (scale) the HTML document.

        */
        //TODO: !KU help
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
        
        /*/// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScript/*'/>
         * internal virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }*/
        
        /*
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScript/*'/>
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScript2/*'/>
         internal virtual bool RunScript(string javascript,out string result)
        {
            CheckDisposed();
            return Browser.RunScript(javascript,out result);
        }*/
        
        /// <summary>
        ///     Injects the specified script into the webpage's content.
        /// </summary>
        /// <param name="javascript">
        ///     The javascript code to add.
        /// </param>
        /// <param name="injectDocStart">
        ///     Specifies when the script will be executed.
        /// </param>
        /// <returns>
        ///     Returns true if the script was added successfully.
        /// </returns>
        /// <remarks>
        ///     Please note that this is unsupported by the IE backend. 
        ///     The Edge backend does only support injecting at document start.
        /// </remarks>
        public virtual bool AddUserScript(string javascript, bool injectDocStart)
        {
            CheckDisposed();
            return Browser.AddUserScript(javascript, injectDocStart);
        }
        
        /// <summary>
        ///     Contains DateTime format for Javascript code.
        ///     Used internally by InvokeScriptAsync().
        /// </summary>
        public static string StringFormatJs = "yyyy-MM-ddTHH:mm:ss.fffK";
        
        /// <summary>
        ///     Converts object value to a string. Used internally by InvokeScriptAsync().
        ///     Only primitive types are supported. String and DateTime values 
        ///     are returned enclosed in single quotes.
        /// </summary>
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
                    //https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                    //https://javascript.info/date
                    return "'" + ((System.DateTime)arg).ToString(WebBrowser.StringFormatJs, 
                        CultureInfo.InvariantCulture) + "'";
                default:
                    break;
            }

            return arg.ToString();
        }
        
        /*/// <include file="Interfaces/IWebBrowser.xml" path='doc/InvokeScript/*'/>
         * internal virtual string? InvokeScript(string scriptName)
        {
            if (scriptName == null)
                return null;
            bool ok = RunScript(scriptName + "()", out string result);
            if (!ok)
                return null;
            return result;
        }*/
        
        /*public virtual void InvokeScriptAsync(string scriptName, IntPtr? clientData = null)
        {
            if (scriptName == null)
                return;
            RunScriptAsync(scriptName + "()", clientData);
        }*/
        
        /// <summary>
        ///     Converts array of object values to comma delimited string. 
        ///     Used internally by InvokeScriptAsync().
        ///     Only primitive types are supported. String and DateTime values 
        ///     are returned enclosed in single quotes.
        /// </summary>
        public string ToInvokeScriptArgs(object?[] args)
        {
            if (args == null || args.Length == 0)
                return String.Empty;

            string? s = ToInvokeScriptArg(args[0]);

            for (int i = 1; i < args.Length; i++)
            {
                var arg = ToInvokeScriptArg(args[i]);
                s += (',' + arg);
            }
            return s;
        }
        
        /*/// <include file="Interfaces/IWebBrowser.xml" path='doc/InvokeScript/*'/>
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/InvokeScript2/*'/>
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
        }*/
        
        public virtual void InvokeScriptAsync(string scriptName, IntPtr clientData,
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

            RunScriptAsync(scriptName + "(" + s + ")",clientData);
        }
        
        /// <summary>
        ///     Loads a web page from a URL.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="T:System.String"/> representing the URL of the document to load.
        ///     If this parameter is null, WebBrowser navigates to a blank document. 
        /// </param>
        /// <remarks>
        ///     Web engines generally report errors asynchronously, so if you 
        ///     want to know whether loading process was successful,         
        ///     register to receive navigation error events.         
        /// </remarks>
        public virtual void LoadURL(string? url = null)
        {
            url ??= "about:blank";
            CheckDisposed();
            Browser.LoadURL(url);
        }
        
        /// <summary>
        ///     Get the title of the current web page, or its URL/path if title is not available.
        /// </summary>
        public virtual string GetCurrentTitle()
        {
            CheckDisposed();
            return Browser.GetCurrentTitle();
        }
        
        /// <summary>
        ///     Get the URL of the currently displayed document.    
        /// </summary>
        public virtual string GetCurrentURL()
        {
            CheckDisposed();
            return Browser.GetCurrentURL();
        }
        
        /// <summary>
        ///     Returns pointer to the native backend interface. For example, 
        ///     for the IE backens it is IWebBrowser2 interface. 
        ///     Under macOS it's a WebView pointer and under GTK it's a WebKitWebView.
        /// </summary>
        public IntPtr GetNativeBackend()
        {
            CheckDisposed();
            return Browser.GetNativeBackend();
        }
        
        /// <summary>
        ///     Returns type of the OS for which the WebBrowser was compiled.
        /// </summary>
        public static WebBrowserBackendOS GetBackendOS() 
        {
            return (WebBrowserBackendOS)Enum.ToObject(typeof(WebBrowserBackendOS),
                Native.WebBrowser.GetBackendOS());
        }
        
        /// <summary>
        ///     Sets path to a fixed version of the WebView2 Edge runtime.
        /// </summary>
        /// <param name="path">
        ///     Path to an extracted fixed version of the WebView2 Edge runtime.
        /// </param>
        /// <param name="isRelative">
        ///     <see langword = "true"/> if specified path is relative to the application 
        ///     folder; otherwise, <see langword = "false"/>.
        /// </param>
        /// <example>
        ///     <code>
        ///         SetBackendPath("Edge",true);
        ///     </code>
        /// </example>
        public static void SetBackendPath(string path,bool isRelative = false)
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

            var files = Directory.EnumerateFileSystemEntries(path,"*.dll");

            foreach(string file in files)
            {
                Native.WebBrowser.SetEdgePath(path);
                break;
            }
        }
        
        /// <summary>
        ///     Returns true if the WebBrowser runs on 64 bit platform.
        /// </summary>
        public static bool Is64Bit
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }
        
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScriptAsync/*'/>
        public virtual void RunScriptAsync(string javascript, IntPtr? clientData=null) 
        {
            CheckDisposed(); 
            Browser.RunScriptAsync(javascript, clientData);
        }
        
    }
    
}

