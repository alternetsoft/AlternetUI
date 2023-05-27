using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using static Alternet.UI.EventTrace;

namespace Alternet.UI
{
    //-------------------------------------------------
    /// <summary>
    ///     <para>
    ///         This control may be used to render full featured web documents. 
    ///         It supports using multiple backends, corresponding to different 
    ///         implementations of the same functionality.
    ///     </para>
    ///     <para>
    ///         Each backend is a full rendering engine (Internet Explorer, Edge or WebKit). 
    ///         This allows the correct viewing of complex web pages with full JavaScript and CSS 
    ///         support. Under macOS and Unix platforms a single backend is provided (WebKit-based). 
    ///         Under MSW both the old IE backends and the new Edge backend can be used. 
    ///     </para>
    ///     
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         WebVrowser many asynchronous methods. They return immediately and 
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
        //-------------------------------------------------
        /// <summary>
        ///     Retrieves or modifies the state of the debug flag to control the 
        ///     allocation behavior of the debug heap manager. This is for debug purposes.
        ///     CrtSetDbgFlag(0) allows to turn off debug output with heap manager information.
        /// </summary>
        public static void CrtSetDbgFlag(int value)
        {
            WebBrowserNativeApi.WebBrowser_CrtSetDbgFlag_(value);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Executes a browser command with the specified name and parameters.
        /// </summary>
        /// <param name="cmdName">
        ///     Name of the command to execute.
        /// </param>
        /// <param name="args">
        ///     Parameters of the command.
        /// </param>
        public string DoCommand(string cmdName, params object?[] args)
        {
            CheckDisposed();
            return Browser.DoCommand(cmdName, args);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Executes a browser command with the specified name and parameters. 
        ///     This is static version of DoCommand.
        /// </summary>
        /// <param name="cmdName">
        ///     Name of the command to execute.
        /// </param>
        /// <param name="args">
        ///     Parameters of the command.
        /// </param>
        public static string DoCommandGlobal(string cmdName, params object?[] args)
        {
            return WebBrowserHandler.DoCommandGlobal(cmdName, args);
        }
        //-------------------------------------------------
        static WebBrowser()
        {
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
        new internal WebBrowserHandler Handler2 => (WebBrowserHandler)base.Handler;
        internal IWebBrowserLite Browser => (IWebBrowserLite)base.Handler;
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
        ///     Increases the zoom factor of the page. The zoom factor is an arbitrary 
        ///     number that specifies how much to zoom (scale) the HTML document.
        /// </summary>
        /// <remarks>
        ///     Zoom scale in IE will be converted into zoom levels if ZoomType property 
        ///     is set to WebBrowserZoomType.Text value.
        /// </remarks>
        public virtual void ZoomIn()
        {
            if (CanZoomIn)                
                ZoomInOut(1);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Decreases the zoom factor of the page. The zoom factor is an arbitrary 
        ///     number that specifies how much to zoom (scale) the HTML document.
        /// </summary>
        /// <remarks>
        ///     Zoom scale in IE will be converted into zoom levels if ZoomType property 
        ///     is set to WebBrowserZoomType.Text value.
        /// </remarks>
        public virtual void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }
        //-------------------------------------------------
        private IWebBrowserMemoryFS? FMemoryFS;
        public IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                FMemoryFS ??= new WebBrowserMemoryFS(this);
                return FMemoryFS;
            }
        }
        //-------------------------------------------------
        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        public virtual bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }
        //-------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether the zoom factor of the page can be decreased, 
        ///     which allows the <see cref="M:ZoomOut"/> method to succeed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the web page can be zoomed out; 
        ///     otherwise, <see langword="false"/>.
        /// </returns>         
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }
        //-------------------------------------------------
        /*public static readonly RoutedEvent NavigatedEvent = 
            EventManager.RegisterRoutedEvent(
            "Navigated", 
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent NavigatingEvent = 
            EventManager.RegisterRoutedEvent(
            "Navigating",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent LoadedEvent = EventManager.RegisterRoutedEvent(
            "Loaded",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(
            "Error",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        //TODO: !KU help ?
        /*public static readonly RoutedEvent NewWindowEvent = 
            EventManager.RegisterRoutedEvent(
            "NewWindow",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent DocumentTitleChangedEvent = EventManager.RegisterRoutedEvent(
            "DocumentTitleChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent FullScreenChangedEvent = EventManager.RegisterRoutedEvent(
            "FullScreenChanged",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent ScriptMessageReceivedEvent = 
            EventManager.RegisterRoutedEvent(
            "ScriptMessageReceived",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        /*public static readonly RoutedEvent ScriptResultEvent = EventManager.RegisterRoutedEvent(
            "ScriptResult",
            RoutingStrategy.Bubble,
            typeof(EventHandler<WebBrowserEventArgs>),
            typeof(WebBrowser));*/
        //-------------------------------------------------
        internal protected virtual void OnFullScreenChanged(WebBrowserEventArgs e)
        {
        }
        //-------------------------------------------------
        internal void RaiseFullScreenChanged(WebBrowserEventArgs e)
        {
            OnFullScreenChanged(e);
            FullScreenChanged?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeFullScreenChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.FullScreenChanged);
            RaiseFullScreenChanged(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnScriptMessageReceived(WebBrowserEventArgs e) 
        {
        }
        //-------------------------------------------------
        public void RaiseScriptMessageReceived(WebBrowserEventArgs e) 
        {
            OnScriptMessageReceived(e);
            ScriptMessageReceived?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeScriptMessageReceived(object? sender,
            Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.ScriptMessageReceived);
            RaiseScriptMessageReceived(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnNativeScriptResult(WebBrowserEventArgs e)
        {

        }
        //-------------------------------------------------
        internal void RaiseNativeScriptResult(WebBrowserEventArgs e)
        {
            OnNativeScriptResult(e);
            ScriptResult?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeScriptResult(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e,WebBrowserEvent.ScriptResult);
            RaiseNativeScriptResult(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnNavigated(WebBrowserEventArgs e)
        {

        }
        //-------------------------------------------------
        internal void RaiseNavigated(WebBrowserEventArgs e)
        {
            OnNavigated(e);
            Navigated?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeNavigated(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Navigated);
            RaiseNavigated(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnNavigating(WebBrowserEventArgs e)
        {

        }
        //-------------------------------------------------
        internal void RaiseNavigating(WebBrowserEventArgs e)
        {
            OnNavigating(e);
            Navigating?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeNavigating(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Navigating);
            RaiseNavigating(ea);
            e.Result = ea.CancelAsIntPtr();
        }
        //-------------------------------------------------
        internal protected virtual void OnLoaded(WebBrowserEventArgs e)
        {

        }
        //-------------------------------------------------
        internal void RaiseLoaded(WebBrowserEventArgs e)
        {
            OnLoaded(e);
            Loaded?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeLoaded(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Loaded);
            RaiseLoaded(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnError(WebBrowserEventArgs e) 
        { 
        }
        //-------------------------------------------------
        internal void RaiseError(WebBrowserEventArgs e) 
        {
            OnError(e);
            Error?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeError(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.Error);
            RaiseError(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnNewWindow(WebBrowserEventArgs e) 
        { 
        }
        //-------------------------------------------------
        internal void RaiseNewWindow(WebBrowserEventArgs e) 
        {
            OnNewWindow(e);
            NewWindow?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeNewWindow(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.NewWindow);
            RaiseNewWindow(ea);
        }
        //-------------------------------------------------
        internal protected virtual void OnDocumentTitleChanged(WebBrowserEventArgs e) 
        { 
        }
        //-------------------------------------------------
        internal void RaiseDocumentTitleChanged(WebBrowserEventArgs e) 
        {
            OnDocumentTitleChanged(e);
            DocumentTitleChanged?.Invoke(this, e);
        }
        //-------------------------------------------------
        internal void OnNativeTitleChanged(object? sender, Native.NativeEventArgs<Native.WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = new(e, WebBrowserEvent.TitleChanged);
            RaiseDocumentTitleChanged(ea);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Occurs when the the page wants to enter or leave fullscreen. 
        ///     Use the IntVal property of the event arguments to get the status. 
        ///     Not implemented for the IE backend.
        /// </summary>
        public event EventHandler<WebBrowserEventArgs>? FullScreenChanged;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED event only available 
        in wxWidgets 3.1.5 or later. For usage details see AddScriptMessageHandler().

         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? ScriptMessageReceived;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_SCRIPT_RESULT event only available in 
        wxWidgets 3.1.6 or later. For usage details see RunScriptAsync(). 

         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? ScriptResult;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///         Occurs when the WebBrowser control has navigated to a new web page 
        ///         and has begun loading it. This event may not be canceled. Note that 
        ///         if the displayed HTML document has several frames, one such event will
        ///         be generated per frame.
        ///     </para>
        ///     <para>
        ///         Handle the Navigated event to receive notification when the WebBrowser 
        ///         control has navigated to a new web page.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         When the Navigated event occurs, the new web page has begun loading, 
        ///         which means you can access the loaded content through the WebBrowser properties.
        ///         Handle the Loaded event to receive notification when the
        ///         WebBrowser control finishes loading the new document.
        ///     </para>
        ///     <para>
        ///         You can also receive notification before navigation begins by handling the
        ///         Navigating event. Handling this event lets you cancel navigation if 
        ///         certain conditions have not been met.For example, the user has not 
        ///         completely filled out a form.
        ///     </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? Navigated;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///         Occurs before the WebBrowser control navigates to a new web page.
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>

        /*
        Process a wxEVT_WEBVIEW_NAVIGATING event, generated before trying to get 
        a resource. This event may be vetoed to prevent navigating to this resource. 
        Note that if the displayed HTML document has several frames, one such event 
        will be generated per frame.



C#

Copy
public event System.Windows.Forms.WebBrowserNavigatingEventHandler Navigating;
Event Type
WebBrowserNavigatingEventHandler
Examples
The following code example demonstrates how to use a handler for the Navigating event to cancel navigation when a Web page form has not been filled in. The Document property is used to determine whether the form input field contains a value.

This example requires that your form contains a WebBrowser control called webBrowser1 and that your form class has a ComVisibleAttribute making it accessible to COM.

Remarks
The WebBrowser control navigates to a new document whenever one of the following properties is set or methods is called:

Url

DocumentText

DocumentStream

Navigate

GoBack

GoForward

GoHome

GoSearch

You can handle the Navigating event to cancel navigation if certain conditions have not been met, for example, when the user has not completely filled out a form. To cancel navigation, set the Cancel property of the WebBrowserNavigatingEventArgs object passed to the event handler to true. You can also use this object to retrieve the URL of the new document through the WebBrowserNavigatingEventArgs.Url property. If the new document will be displayed in a Web page frame, you can retrieve the name of the frame through the WebBrowserNavigatingEventArgs.TargetFrameName property.

Handle the Navigated event to receive notification when the WebBrowser control finishes navigation and has begun loading the document at the new location. Handle the DocumentCompleted event to receive notification when the WebBrowser control finishes loading the new document.

For more information about handling events, see Handling and Raising Events.

         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? Navigating;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_LOADED event generated when the document is fully 
        loaded and displayed. Note that if the displayed HTML document has several 
        frames, one such event will be generated per frame.


Occurs when the WebBrowser control finishes loading a document.

C#

Copy
public event System.Windows.Forms.WebBrowserDocumentCompletedEventHandler DocumentCompleted;
Event Type
WebBrowserDocumentCompletedEventHandler
Examples
The following code example demonstrates the use of this event to print a document after it has fully loaded.

Remarks
The WebBrowser control navigates to a new document whenever one of the following properties is set or methods is called:

Url

DocumentText

DocumentStream

Navigate

GoBack

GoForward

GoHome

GoSearch

Handle the DocumentCompleted event to receive notification when the new document finishes loading. When the DocumentCompleted event occurs, the new document is fully loaded, which means you can access its contents through the Document, DocumentText, or DocumentStream property.

To receive notification before navigation begins, handle the Navigating event. Handling this event lets you cancel navigation if certain conditions have not been met, for example, when the user has not completely filled out a form. Handle the Navigated event to receive notification when the WebBrowser control finishes navigation and has begun loading the document at the new location.

For more information about handling events, see Handling and Raising Events.
         */
        public event EventHandler<WebBrowserEventArgs>? Loaded;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_ERROR event generated when a navigation error occurs. 
        The integer associated with this event will be a wxWebNavigationError item. 
        The string associated with this event may contain a backend-specific 
        more precise error message/code.

         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? Error;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_NEWWINDOW event, generated when a new window is created. 
        You must handle this event if you want anything to happen, for example to 
        load the page in a new window or tab.

Occurs before a new browser window is opened.

public event System.ComponentModel.CancelEventHandler NewWindow;
Event Type
CancelEventHandler

Remarks
The WebBrowser control opens a separate browser window when the appropriate overload of the Navigate method is called or when the user clicks the Open in New Window option of the browser shortcut menu when the mouse pointer hovers over a hyperlink. You can disable the shortcut menu by setting the IsWebBrowserContextMenuEnabled property to false.

The NewWindow event occurs before the new browser window is opened. You can handle this event, for example, to prevent the window from opening when certain conditions have not been met.
         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? NewWindow;
        //-------------------------------------------------
        /// <summary>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     </para>
        ///     <para>
        ///     </para>
        /// </remarks>
        /*
        Process a wxEVT_WEBVIEW_TITLE_CHANGED event, generated when 
        the page title changes. Use GetString to get the title.

Occurs when the DocumentTitle property value changes.

C#

Copy
[System.ComponentModel.Browsable(false)]
public event EventHandler DocumentTitleChanged;
Event Type
EventHandler
Attributes
BrowsableAttribute
Examples
The following code example demonstrates how to use a handler for the DocumentTitleChanged event to update the form title bar with the title of the current document. This example requires that your form contains a WebBrowser control called webBrowser1.

For the complete code example, see How to: Add Web Browser Capabilities to a Windows Forms Application.

C#

Copy
// Updates the title bar with the current document title.
private void webBrowser1_DocumentTitleChanged(object sender, EventArgs e)
{
    this.Text = webBrowser1.DocumentTitle;
}
Remarks
You can handle this event to update the title bar of your application with the current value of the DocumentTitle property.

For more information about handling events, see Handling and Raising Events.
         */
        //TODO: !KU help
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
Gets or sets the Uri of the current document hosted in the WebBrowser.

C#

Copy
public Uri Source { get; set; }
Property Value
Uri
The Uri for the current HTML document.

Exceptions
ObjectDisposedException
The WebBrowser instance is no longer valid.

InvalidOperationException
A reference to the underlying native WebBrowser could not be retrieved.

SecurityException
Navigation from an application that is running in partial trust to a Uri that is not located at the site of origin.

Examples
The following example shows how to configure WebBrowser to navigate to an HTML document by using markup only.

XAML

Copy
<!-- Web Browser Control that hosts a web page. -->  
<WebBrowser x:Name="webBrowser" Source="http://msdn.com"   
  Width="600" Height="600"  />  
Remarks
Setting the source property causes WebBrowser to navigate to the document specified by the Uri. If the Uri is null, a blank document is displayed ("about:blank").         
        */
        //TODO: !KU help
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
        public static void SetBackend(WebBrowserBackend value)
        {
            WebBrowserNativeApi.WebBrowser_SetBackend_((int)value);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Retrieve the version information about the underlying library implementation.
        /// </summary>
        public static string GetLibraryVersionString()
        {
            return WebBrowserNativeApi.WebBrowser_GetLibraryVersionString_();
        }
        //-------------------------------------------------
        /// <summary>
        ///     Retrieve the version information about the browser backend implementation.
        /// </summary>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return WebBrowserNativeApi.WebBrowser_GetBackendVersionString_((int)value);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
        public static void SetDefaultPage(string url)
        {
            WebBrowserNativeApi.WebBrowser_SetDefaultPage_(url);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Stops the current page loading process, if any. Cancels any pending navigation 
        ///     and stops any dynamic page elements, such as background sounds and animations.
        /// </summary>
        public virtual void Stop()
        {
            CheckDisposed();
            Browser.Stop();
        }
        //-------------------------------------------------
        /// <summary>
        ///     Clear the history, this will also remove the visible page. This is not 
        ///     implemented on macOS and the WebKit2GTK+ backend.         
        /// </summary>
        public virtual void ClearHistory()
        {
            CheckDisposed();
            Browser.ClearHistory();
        }
        //-------------------------------------------------
        /// <summary>
        ///     Enables or disables the history. This method will also clear the history.
        ///     This method is not implemented on macOS and the WebKit2GTK+ backend.
        /// </summary>
        public virtual void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Browser.EnableHistory(enable);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Reloads the document currently displayed in the control by downloading
        ///     for an updated version from the server.
        /// </summary>
        public virtual void Reload()
        {
            CheckDisposed();
            Browser.Reload();
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Selects the entire page.         
        /// </summary>
        public virtual void SelectAll()
        {
            CheckDisposed();
            Browser.SelectAll(); 
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Clears the current selection. Specifies that no characters are 
        ///     selected in the control.        
        /// </summary>
        public virtual void ClearSelection() 
        {
            CheckDisposed();
            Browser.ClearSelection(); 
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut() 
        {
            CheckDisposed();
            Browser.Cut(); 
        }
        //-------------------------------------------------
        /// <summary>
        ///     Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy() 
        {
            CheckDisposed();
            Browser.Copy(); 
        }
        //-------------------------------------------------
        /// <summary>
        ///     Replaces the current selection in the control with the contents of the Clipboard.
        /// </summary>
        public virtual void Paste() 
        {
            CheckDisposed();
            Browser.Paste();
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public virtual void Undo() 
        {
            CheckDisposed();
            Browser.Undo(); 
        }
        //-------------------------------------------------
        /// <summary>
        /// Redos the last edit operation in the control.
        /// </summary>
        public virtual void Redo() 
        {
            CheckDisposed();
            Browser.Redo(); 
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Opens a print dialog so that the user may change the current print and 
        ///     page settings and print the currently displayed page.         
        /// </summary>
        public virtual void Print() 
        {
            CheckDisposed();
            Browser.Print(); 
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        ///     Removes all user scripts from the web view.
        /// </summary>
        public virtual void RemoveAllUserScripts() 
        {
            CheckDisposed();
            Browser.RemoveAllUserScripts(); 
        }
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
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
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
        public virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }
        //-------------------------------------------------
        /// <summary>
        ///     Runs the given JavaScript code.
        /// </summary>
        /*


Note
Because of various potential issues it's recommended to use RunScriptAsync() instead of this method. This is especially true if you plan to run code from a webview event and will also prevent unintended side effects on the UI outside of the webview.
JavaScript code is executed inside the browser control and has full access to DOM and other browser-provided functionality. For example, this code

webview->RunScript("document.write('Hello from wxWidgets!')");
will replace the current page contents with the provided string.

If output is non-null, it is filled with the result of executing this code on success, e.g. a JavaScript value such as a string, a number (integer or floating point), a boolean or JSON representation for non-primitive types such as arrays and objects. For example:

wxString result;
if ( webview->RunScript
              (
                "document.getElementById('some_id').innerHTML",
                &result
              ) )
{
    ... result contains the contents of the given element ...
}
//else: the element with this ID probably doesn't exist.
This function has a few platform-specific limitations:

When using WebKit v1 in wxGTK2, retrieving the result of JavaScript execution is unsupported and this function will always return false if output is non-null to indicate this. This functionality is fully supported when using WebKit v2 or later in wxGTK3.
When using WebKit under macOS, code execution is limited to at most 10MiB of memory and 10 seconds of execution time.
When using IE backend under MSW, scripts can only be executed when the current page is fully loaded (i.e. wxEVT_WEBVIEW_LOADED event was received). A script tag inside the page HTML is required in order to run JavaScript.
Also notice that under MSW converting JavaScript objects to JSON is not supported in the default emulation mode. wxWebView implements its own object-to-JSON conversion as a fallback for this case, however it is not as full-featured, well-tested or performing as the implementation of this functionality in the browser control itself, so it is recommended to use MSWSetEmulationLevel() to change emulation level to a more modern one in which JSON conversion is done by the control itself.

Parameters
javascript	JavaScript code to execute.
output	Pointer to a string to be filled with the result value or NULL if it is not needed. This parameter is new since wxWidgets version 3.1.1.
Returns
true if there is a result, false if there is an error.         
        */
        //TODO: !KU help
        public virtual bool RunScript(string javascript,out string result)
        {
            CheckDisposed();
            return Browser.RunScript(javascript,out result);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
Executes a script function that is implemented by the currently loaded document.

C#

Copy
public object InvokeScript (string scriptName);
Parameters
scriptName
String
The name of the script function to execute.

Returns
Object
The object returned by the Active Scripting call.

Exceptions
ObjectDisposedException
The WebBrowser instance is no longer valid.

InvalidOperationException
A reference to the underlying native WebBrowser could not be retrieved.

COMException
The script function does not exist.

Examples
The following example shows how to call a script function in a document from a WPF application by using InvokeScript(String). In this example, the script function has no parameters.

The following is the HTML document that implements the script function that will be called from WPF.

HTML

Copy
<html>  
    <head>  
        <script type="text/javascript">  
            // Function Without Parameters  
            function JavaScriptFunctionWithoutParameters()    
            {  
              outputID.innerHTML = "JavaScript function called!";  
            }  
        </script>  
    </head>  
    <body>  
    <div id="outputID" style="color:Red; font-size:16">  
        Hello from HTML document with script!  
    </div>  
    </body>  
</html>  
The following shows the WPF implementation to call the script function in the HTML document.

C#

Copy
private void callScriptFunctionNoParamButton_Click(object sender, RoutedEventArgs e)  
{  
  // Make sure the HTML document has loaded before attempting to  
  // invoke script of the document page. You could set loadCompleted  
  // to true when the LoadCompleted event on the WebBrowser fires.  
  if (this.loadCompleted)  
  {  
    try  
    {  
      this.webBrowser.InvokeScript("JavaScriptFunctionWithoutParameters");  
    }  
    catch (Exception ex)  
    {  
      string msg = "Could not call script: " +  
                   ex.Message +  
                  "\n\nPlease click the 'Load HTML Document with Script' button to load.";  
      MessageBox.Show(msg);  
    }  
  }  
}  
Remarks
InvokeScript(String) should not be called before the document that implements it has finished loading. You can detect when a document has finished loading by handling the LoadCompleted event.        
        */
        public virtual string? InvokeScript(string scriptName)
        {
            if (scriptName == null)
                return null;
            bool ok = RunScript(scriptName+"()",out string result);
            if (!ok)
                return null;
            return result;
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
Injects the specified script into the webpage's content.

Parameters
javascript	The javascript code to add.
injectionTime	Specifies when the script will be executed.
Returns
Returns true if the script was added successfully.
Note
Please note that this is unsupported by the IE backend and the Edge backend does only support wxWEBVIEW_INJECT_AT_DOCUMENT_START.
See also
RemoveAllUserScripts()         
        */
        //TODO: !KU help
        public virtual bool AddUserScript(string javascript, bool injectDocStart)
        {
            CheckDisposed();
            return Browser.AddUserScript(javascript, injectDocStart);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
        public static string StringFormatJs = "yyyy-MM-ddTHH:mm:ss.fffK";
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        //TODO: !KU help
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
                //https://learn.microsoft.com/ru-ru/dotnet/standard/base-types/custom-date-and-time-format-strings
                //https://javascript.info/date
                    return "'" + ((System.DateTime)arg).ToString(WebBrowser.StringFormatJs, 
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
        /*
Executes a script function that is defined in the currently loaded document.

C#

Copy
public object InvokeScript (string scriptName, params object[] args);
Parameters
scriptName
String
The name of the script function to execute.

args
Object[]
The parameters to pass to the script function.

Returns
Object
The object returned by the Active Scripting call.
        


        */
        //TODO: !KU help
        public virtual string? InvokeScript(string scriptName, params object?[] args)
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
        //-------------------------------------------------
        /// <summary>
        ///     Get the title of the current web page, or its URL/path if title is not available.
        /// </summary>
        public virtual string GetCurrentTitle()
        {
            CheckDisposed();
            return Browser.GetCurrentTitle();
        }
        //-------------------------------------------------
        /// <summary>
        ///     Get the URL of the currently displayed document.    
        /// </summary>
        public virtual string GetCurrentURL()
        {
            CheckDisposed();
            return Browser.GetCurrentURL();
        }
        //-------------------------------------------------
        public IntPtr GetNativeBackend()
        {
            CheckDisposed();
            return Browser.GetNativeBackend();
        }
        //-------------------------------------------------
        /// <summary>
        ///     Runs the given JavaScript code asynchronously and returns the 
        ///     result via a ScriptResult event.
        /// </summary>
        /// <remarks>
        ///     The IE backend does not support async script execution.
        /// </remarks>
        /*
        The script result value can be retrieved via 
        wxWebViewEvent::GetString(). If the execution fails 
        WebBrowserEventArgs.IsError will return true. 
        In this case additional script execution error information maybe 
        available via wxWebViewEvent::GetString().

        Parameters
        javascript	JavaScript code to execute.
        clientData	Arbirary pointer to data that can be retrieved from the result event.
        Note
        
         */
        //TODO: !KU help
        public virtual void RunScriptAsync(string javascript, IntPtr clientData) 
        {
            CheckDisposed(); 
            Browser.RunScriptAsync(javascript, clientData);
        }
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
        /// <remarks>
        /// 
        /// </remarks>
         
        */
        //TODO: !KU help
        public virtual void RunScriptAsync(string javascript)
        {
            RunScriptAsync(javascript, IntPtr.Zero);
        }
        //-------------------------------------------------
    }
    //-------------------------------------------------
}
//-------------------------------------------------
