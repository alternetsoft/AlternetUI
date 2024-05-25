using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// This control may be used to render full featured web documents.
    /// It supports using multiple backends, corresponding to different
    /// implementations of the same functionality.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each backend is a full rendering engine (Internet Explorer, Edge or WebKit). 
    /// This allows the correct viewing of complex web pages with full JavaScript
    /// and CSS support.Under macOS and Unix platforms a single backend is provided
    /// (WebKit-based). Under MSW both the old IE backend and the new Edge
    /// backend can be used.
    /// </para>
    /// <para>
    /// WebBrowser has many asynchronous methods.They return immediately and
    /// perform their work in the background.This includes functions such as 
    /// <see cref = "Reload()" /> and < see cref= "LoadURL" />.
    /// </para >
    /// <para >
    /// To receive notification of the progress and completion of these functions
    /// you need to handle the events that are provided.
    /// Specifically<see cref="Loaded"/> event notifies when the page or a sub-frame
    /// has finished loading and<see cref="Error"/> event notifies that an
    /// error has occurred.
    /// </para>
    /// </remarks>
    [DefaultProperty("Name")]
    [DefaultEvent("Enter")]
    [ControlCategory("Common")]
    public partial class WebBrowser : Control, IWebBrowser
    {
        private static IWebBrowserFactoryHandler? factory;

        private readonly string defaultUrl = "about:blank";

        private IWebBrowserMemoryFS? fMemoryFS;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser(string url)
        {
            defaultUrl = url;
        }

        /*
<summary>
Occurs when the the page wants to enter or leave fullscreen. 
</summary>
<remarks>
Use the <see cref="WebBrowserEventArgs.IntVal"/> property of the event 
arguments to get the status. 
Not implemented for the IE backend.
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? FullScreenChanged;

        /*
<summary>
Occurs when your application receives message from JS code of the loaded web page.
</summary>
<remarks>
For usage details see <see cref="WebBrowser.AddScriptMessageHandler"/>.
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? ScriptMessageReceived;

        /*
<summary>
Occurs when your application receives results after 
RunScriptAsync call.
</summary>
<remarks>
For usage details see description of RunScriptAsync method.
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? ScriptResult;

        /*
<summary>
Occurs when the WebBrowser control has navigated to a new web page 
and has begun loading it. 
</summary>
<remarks>
 <para>
	 This event may not be canceled. Note that 
	 if the displayed HTML document has several frames, one such event will
	 be generated per frame.
 </para>
 <para>
	 Handle the Navigated event to receive notification when the WebBrowser
	 control has navigated to a new web page.
 </para>
 <para>
	 When the Navigated event occurs, the new web page has begun loading, 
	 which means you can access the loaded content through the WebBrowser properties and methods.
 </para>
 <para>
	 Handle the <see cref="Loaded"/> event to receive notification when the
	 WebBrowser control finishes loading the new document.
 </para>
 <para>
	 You can also receive notification before navigation begins by handling the
	 <see cref="Navigating"/> event. Handling this event lets you cancel navigation if 
	 certain conditions have not been met. For example, the user has not 
	 completely filled out a form.
 </para>
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? Navigated;

        /*
<summary>
Occurs before the WebBrowser control navigates to a new web page.
</summary>
<remarks>		    
 <para>
	 This event may be canceled to prevent navigating to this resource. 
	 Note that if the displayed HTML document has several frames, one such 
	 event will be generated per frame.
 </para>
 <para>
	 You can handle the Navigating event to cancel navigation if certain 
	 conditions 
	 have not been met, for example, when the user has not completely filled 
	 out a form. To cancel navigation, set the 
	 <see cref="System.ComponentModel.CancelEventArgs.Cancel"/> property of the events
	  object passed to the event handler to <see langword = "true"/>. 
 </para>
 <para>
	 Handle the <see cref="Navigated"/> event to receive notification when the WebBrowser 
	 control finishes navigation and has begun loading the document at the
	 new location.
 </para>
 <para>
	 Handle the <see cref="Loaded"/> event to receive 
	 notification when the WebBrowser control finishes loading the new document.
 </para>
 <para>
	 You can also use <see cref="WebBrowserEventArgs"/> object to retrieve the URL of the 
	 new document through the <see cref="WebBrowserEventArgs.Url"/> property. If the new 
	 document will be displayed in a Web page frame, you can retrieve the 
	 name of the frame through the 
	 <see cref="WebBrowserEventArgs.TargetFrameName"/> property.
 </para>
</remarks>	
        */
        public event EventHandler<WebBrowserEventArgs>? Navigating;

        /*
<summary>
Occurs after backend is created, but before actual browser
control creation. 
</summary>
<remarks>
You should not normally use it.
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? BeforeBrowserCreate;

        /*
<summary>
<para>
	Occurs when the WebBrowser control finishes loading a document.
</para>
</summary>
<remarks>
<para>
	Handle the Loaded event to receive notification when the new 
	document finishes loading. When the Loaded event occurs, the
	new document is fully loaded, which means you can access its
	contents through WebBrowser properties and methods.
</para>
<para>
	To receive notification before navigation begins, handle the 
	<see cref="Navigating"/> event. Handling this event lets you cancel
	navigation if certain conditions have not been met,
	for example, when the user has not completely filled out a form.
</para>
<para>
	Handle the <see cref="Navigated"/> event to receive notification when the 
	WebBrowser control finishes navigation and has begun loading
	the document at the new location.
</para>
<para>
	Note that if the displayed HTML document has several 
	frames, one such event will be generated per frame.
</para>
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? Loaded;

        /*
<summary>
	Occurs when a navigation error occurs.
</summary>
<remarks>
<para>
	The <see cref="WebBrowserEventArgs.NavigationError"/> will contain an error type. 
	The <see cref="WebBrowserEventArgs.Text"/> may contain a backend-specific
	more precise error message or code.
</para>
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? Error;

        /*
<summary>
<para>
	Occurs when a new browser window is created.
</para>
</summary>
<remarks>
<para>
	You must handle this event if you want anything to happen, for example to 
	load the page in a new window or tab.
</para>
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? NewWindow;

        /*
<summary>
Occurs when the web page title changes. 
Use <see cref="WebBrowserEventArgs.Text"/> to get the title.
</summary>
<remarks>
You can handle this event to update the title bar of your 
application with the current title of the loaded document.
</remarks>
        */
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;

        /*
        
        */
        public static IWebBrowserFactoryHandler Factory
        {
            get => factory ??= BaseApplication.Handler.CreateWebBrowserFactoryHandler();
            set => factory = value;
        }

        /*
<summary>
Gets a value indicating whether the WebBrowser runs on a 64 bit platform.
</summary>
<returns>
<see langword="true"/> if the WebBrowser runs on a 64 bit platform;
otherwise, <see langword="false"/>.
</returns>
        */
        public static bool Is64Bit
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                fMemoryFS ??= Factory.CreateMemoryFileSystem(this);
                return fMemoryFS;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public override ControlTypeId ControlKind => ControlTypeId.WebBrowser;

        /// <summary>
        /// Gets default url (the first loaded url).
        /// </summary>
        [Browsable(false)]
        public string DefaultUrl => defaultUrl;

        /*
        
        */
        [Browsable(false)]
        public virtual bool HasSelection
        {
            get
            {
                CheckDisposed();
                return Handler.HasSelection;
            }
        }

        /*
        
        */
        public virtual float ZoomFactor
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomFactor;
            }

            set
            {
                CheckDisposed();
                Handler.ZoomFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                CheckDisposed();
                return Handler.HasBorder;
            }

            set
            {
                CheckDisposed();
                Handler.HasBorder = value;
            }
        }

        /*
        
        */
        public virtual WebBrowserZoomType ZoomType
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomType;
            }

            set
            {
                CheckDisposed();
                Handler.ZoomType = value;
            }
        }

        /*
        
        */
        [Browsable(false)]
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

        /*
<summary>
Gets a value indicating whether the zoom factor of the page can be increased,
which allows the <see cref="ZoomIn"/> method to succeed.
</summary>
<returns>
<see langword="true"/> if the web page can be zoomed in;
otherwise, <see langword="false"/>.
</returns>        
        */
        [Browsable(false)]
        public virtual bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanGoBack
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoBack;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanUndo
        {
            get
            {
                CheckDisposed();
                return Handler.CanUndo;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanRedo
        {
            get
            {
                CheckDisposed();
                return Handler.CanRedo;
            }
        }

        /*
        
        */
        public virtual bool AccessToDevToolsEnabled
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

        /*
        
        */
        public virtual string UserAgent
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

        /*
        
        */
        public virtual bool ContextMenuEnabled
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

        /*
        
        */
        [Browsable(false)]
        public virtual WebBrowserBackend Backend
        {
            get
            {
                CheckDisposed();
                return Handler.Backend;
            }
        }

        /*
        
        */
        public virtual bool Editable
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

        /*
        
        */
        public virtual WebBrowserZoom Zoom
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

        /*
        
        */
        [Browsable(false)]
        public virtual bool IsBusy
        {
            get
            {
                CheckDisposed();
                return Handler.IsBusy;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual string PageSource
        {
            get
            {
                CheckDisposed();
                return Handler.PageSource;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual string PageText
        {
            get
            {
                CheckDisposed();
                return Handler.PageText;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual string SelectedText
        {
            get
            {
                CheckDisposed();
                return Handler.SelectedText;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual string SelectedSource
        {
            get
            {
                CheckDisposed();
                return Handler.SelectedSource;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanCut
        {
            get
            {
                CheckDisposed();
                return Handler.CanCut;
            }
        }

        /*
        
        */
        public string Url
        {
            get
            {
                return GetCurrentURL();
            }

            set
            {
                LoadURL(value);
            }
        }

        /*
        
        */
        public virtual WebBrowserPreferredColorScheme PreferredColorScheme
        {
            get
            {
                CheckDisposed();
                return Handler.PreferredColorScheme;
            }

            set
            {
                CheckDisposed();
                Handler.PreferredColorScheme = value;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanCopy
        {
            get
            {
                CheckDisposed();
                return Handler.CanCopy;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanPaste
        {
            get
            {
                CheckDisposed();
                return Handler.CanPaste;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanGoForward
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoForward;
            }
        }

        /*
        
        */
        [Browsable(false)]
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }

        /*
        
        */
        [Browsable(false)]
        internal new IWebBrowserHandler Handler => (IWebBrowserHandler)base.Handler;

        /*
        
        */
        public static WebBrowserBackendOS GetBackendOS()
        {
            return Factory.GetBackendOS();
        }

        /// <summary>
        /// Prepends filename with "file" url protocol prefix.
        /// </summary>
        /// <param name="filename">Path to the file.</param>
        /// <returns><see cref="string"/> containing filename with "file" url
        /// protocol prefix.</returns>
        /// <remarks>
        /// Under Windows file path starts with drive letter, so we need
        /// to add additional "/" separator between protocol and file path.
        /// This functions checks for OS and adds "file" url protocol correctly.
        /// Also spaces are replaced with %20, other non url chars are replaced
        /// with corresponding html codes.
        /// </remarks>
        public static string PrepareFileUrl(string filename) => CommonUtils.PrepareFileUrl(filename);

        /*
<summary>
Sets path to a fixed version of the WebView2 Edge runtime.
</summary>
<param name="path">
Path to an extracted fixed version of the WebView2 Edge runtime.
</param>
<param name="isRelative">
<see langword = "true"/> if specified path is relative to the application 
folder; otherwise, <see langword = "false"/>.
</param>
<example>
<code language="C#">
SetBackendPath("Edge",true);
</code>
</example>        
        */
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

#pragma warning disable
            foreach (string file in files)
            {
                Factory.SetEdgePath(path);
                break;
            }
#pragma warning restore
        }

        /*
        
        */
        public static string? DoCommandGlobal(string cmdName, params object?[] args)
        {
            if(cmdName == "CppThrow")
            {
                Factory.ThrowError(1);
                return string.Empty;
            }

            if (cmdName == "Screenshot")
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string s = Path.GetDirectoryName(location)!;

                var screenshotFilename = s + @"\screenshot.bmp";
                if (args.Length == 0)
                    return string.Empty;
                Control? control = args[0] as Control;
                control?.SaveScreenshot(screenshotFilename);
                return screenshotFilename;
            }

            if (cmdName == "UIVersion")
            {
                Assembly thisAssembly = typeof(BaseApplication).Assembly;
                AssemblyName thisAssemblyName = thisAssembly.GetName();
                Version? ver = thisAssemblyName?.Version;
                return ver?.ToString();
            }

            return Factory.DoCommand(cmdName, args);
        }

        /*
<summary>
Retrieves or modifies the state of the debug flag to control the 
allocation behavior of the debug heap manager. 
</summary>
<param name="value">
New debug flag state.
</param>
<remarks>
This is for debug purposes.
CrtSetDbgFlag(0) allows to turn off debug output with heap manager information.
</remarks>
        */
        public static void CrtSetDbgFlag(int value)
        {
            Factory.CrtSetDbgFlag(value);
        }

        /*
        
        */
        public static bool IsBackendAvailable(WebBrowserBackend value)
        {
            return Factory.IsBackendAvailable(value);
        }

        /*
        
        */
        public static void SetBackend(WebBrowserBackend value)
        {
            if (value == WebBrowserBackend.IE || value == WebBrowserBackend.IELatest
                || value == WebBrowserBackend.Edge)
            {
                if (WebBrowser.GetBackendOS() != WebBrowserBackendOS.Windows)
                    value = WebBrowserBackend.Default;
            }

            Factory.SetBackend(value);
        }

        /*
        
        */
        public static string GetLibraryVersionString()
        {
            return Factory.GetLibraryVersionString();
        }

        /*
        
        */
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return Factory.GetBackendVersionString(value);
        }

        /*
        
        */
        public static void SetDefaultUserAgent(string value)
        {
            Factory.SetDefaultUserAgent(value);
        }

        /*
        
        */
        public static void SetDefaultScriptMesageName(string value)
        {
            Factory.SetDefaultScriptMesageName(value);
        }

        /*
        
        */
        public static void SetDefaultFSNameMemory(string value)
        {
            Factory.SetDefaultFSNameMemory(value);
        }

        /*
        
        */
        public static void SetDefaultFSNameArchive(string value)
        {
            Factory.SetDefaultFSNameArchive(value);
        }

        /*
        
        */
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

        /*
        
        */
        public virtual void LoadUrlOrSearch(string? url)
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
#pragma warning disable IDE0079
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
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                uri = new Uri("https://google.com/search?q=" +
                    string.Join("+", Uri.EscapeDataString(url).Split(
                        new string[] { "%20" },
                        StringSplitOptions.RemoveEmptyEntries)));
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            }
#pragma warning restore CA1847 // Use char literal for a single character lookup
#pragma warning restore IDE0079

            LoadURL(uri.ToString());
        }

        /*

        */
        public virtual string? DoCommand(string cmdName, params object?[] args)
        {
            CheckDisposed();
            return Handler.DoCommand(cmdName, args);
        }

        /*

        */
        public virtual void ZoomIn()
        {
            if (CanZoomIn)
                ZoomInOut(1);
        }

        /*

        */
        public virtual void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }

        /*

        */
        public void Navigate(string urlString)
        {
            LoadURL(urlString);
        }

        /*

        */
        public void Navigate(Uri? source)
        {
            if (source == null)
                LoadURL();
            else
                LoadURL(source.ToString());
        }

        /*

        */
        public virtual bool GoBack()
        {
            if (!CanGoBack)
                return false;
            CheckDisposed();
            return Handler.GoBack();
        }

        /*

        */
        public virtual bool GoForward()
        {
            if (!CanGoForward)
                return false;
            CheckDisposed();
            return Handler.GoForward();
        }

        /*

        */
        public virtual void Stop()
        {
            CheckDisposed();
            Handler.Stop();
        }

        /*

        */
        public virtual void ClearHistory()
        {
            CheckDisposed();
            Handler.ClearHistory();
        }

        /*

        */
        public virtual void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Handler.EnableHistory(enable);
        }

        /*

        */
        public virtual void Reload()
        {
            CheckDisposed();
            Handler.Reload();
        }

        /*

        */
        public virtual void Reload(bool noCache)
        {
            CheckDisposed();
            Handler.Reload(noCache);
        }

        /*

        */
        public virtual void NavigateToString(string html, string? baseUrl = null)
        {
            CheckDisposed();
            if (string.IsNullOrEmpty(html))
            {
                LoadURL();
                return;
            }

            Handler.NavigateToString(html, baseUrl ?? string.Empty);
        }

        /*

        */
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

        /*

        */
        public virtual bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            CheckDisposed();
            return Handler.CanSetZoomType(zoomType);
        }

        /// <summary>
        /// Raises the <see cref="ScriptMessageReceived"/> event and
        /// <see cref="OnScriptMessageReceived"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseScriptMessageReceived(WebBrowserEventArgs e)
        {
            ScriptMessageReceived?.Invoke(this, e);
            OnScriptMessageReceived(e);
        }

        /// <summary>
        /// Raises the <see cref="FullScreenChanged"/> event and
        /// <see cref="OnFullScreenChanged"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseFullScreenChanged(WebBrowserEventArgs e)
        {
            FullScreenChanged?.Invoke(this, e);
            OnFullScreenChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="ScriptResult"/> event and
        /// <see cref="OnScriptResult"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseScriptResult(WebBrowserEventArgs e)
        {
            ScriptResult?.Invoke(this, e);
            OnScriptResult(e);
        }

        /// <summary>
        /// Raises the <see cref="Navigated"/> event and
        /// <see cref="OnNavigated"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNavigated(WebBrowserEventArgs e)
        {
            Navigated?.Invoke(this, e);
            OnNavigated(e);
        }

        /// <summary>
        /// Raises the <see cref="DocumentTitleChanged"/> event and
        /// <see cref="OnDocumentTitleChanged"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseDocumentTitleChanged(WebBrowserEventArgs e)
        {
            DocumentTitleChanged?.Invoke(this, e);
            OnDocumentTitleChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeBrowserCreate"/> event and
        /// <see cref="OnBeforeBrowserCreate"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseBeforeBrowserCreate(WebBrowserEventArgs e)
        {
            BeforeBrowserCreate?.Invoke(this, e);
            OnBeforeBrowserCreate(e);
        }

        /// <summary>
        /// Raises the <see cref="Loaded"/> event and
        /// <see cref="OnLoaded"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseLoaded(WebBrowserEventArgs e)
        {
            Loaded?.Invoke(this, e);
            OnLoaded(e);
        }

        /// <summary>
        /// Raises the <see cref="Error"/> event and
        /// <see cref="OnError"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseError(WebBrowserEventArgs e)
        {
            Error?.Invoke(this, e);
            OnError(e);
        }

        /// <summary>
        /// Raises the <see cref="Navigating"/> event and
        /// <see cref="OnNavigating"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNavigating(WebBrowserEventArgs e)
        {
            Navigating?.Invoke(this, e);
            OnNavigating(e);
        }

        /// <summary>
        /// Raises the <see cref="NewWindow"/> event and
        /// <see cref="OnNewWindow"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNewWindow(WebBrowserEventArgs e)
        {
            NewWindow?.Invoke(this, e);
            OnNewWindow(e);
        }

        /*

        */
        public virtual void SelectAll()
        {
            CheckDisposed();
            Handler.SelectAll();
        }

        /*

        */
        public virtual void DeleteSelection()
        {
            CheckDisposed();
            Handler.DeleteSelection();
        }

        /*

        */
        public virtual void Undo()
        {
            if (!CanUndo)
                return;
            CheckDisposed();
            Handler.Undo();
        }

        /*

        */
        public virtual void Redo()
        {
            if (!CanRedo)
                return;
            CheckDisposed();
            Handler.Redo();
        }

        /*

        */
        public virtual void ClearSelection()
        {
            CheckDisposed();
            Handler.ClearSelection();
        }

        /*

        */
        public virtual void Cut()
        {
            if (!CanCut)
                return;
            CheckDisposed();
            Handler.Cut();
        }

        /*

        */
        public virtual void Copy()
        {
            if (!CanCopy)
                return;
            CheckDisposed();
            Handler.Copy();
        }

        /*

        */
        public virtual void Paste()
        {
            if (!CanPaste)
                return;
            CheckDisposed();
            Handler.Paste();
        }

        /*

        */
        public virtual int Find(string text, WebBrowserFindParams? prm = null)
        {
            CheckDisposed();
            return Handler.Find(text, prm);
        }

        /*

        */
        public virtual void FindClearResult()
        {
            CheckDisposed();
            Handler.FindClearResult();
        }

        /*

        */
        public virtual void Print()
        {
            CheckDisposed();
            Handler.Print();
        }

        /*

        */
        public virtual void RemoveAllUserScripts()
        {
            CheckDisposed();
            Handler.RemoveAllUserScripts();
        }

        /*

        */
        public virtual bool AddScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Handler.AddScriptMessageHandler(name);
        }

        /*

        */
        public virtual bool RemoveScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Handler.RemoveScriptMessageHandler(name);
        }

        /*

        */
        public virtual bool AddUserScript(
            string javascript,
            bool injectDocStart = true)
        {
            CheckDisposed();
            return Handler.AddUserScript(javascript, injectDocStart);
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

        /*

        */
        public virtual string ToInvokeScriptArgs(object?[] args)
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

        /*

        */
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

        /*

        */
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
                        DateUtils.DateFormatJs,
                        CultureInfo.InvariantCulture) + "'";
                default:
                    break;
            }

            return arg.ToString();
        }

        /*

        */
        public virtual void LoadURL(string? url = null)
        {
            url ??= "about:blank";
            CheckDisposed();
            Handler.LoadURL(url);
        }

        /*

        */
        public virtual string GetCurrentTitle()
        {
            CheckDisposed();
            return Handler.GetCurrentTitle();
        }

        /*

        */
        public virtual string GetCurrentURL()
        {
            CheckDisposed();
            return Handler.GetCurrentURL();
        }

        /*

        */
        public virtual void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind)
        {
            CheckDisposed();
            Handler.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }

        /*

        */
        public IntPtr GetNativeBackend()
        {
            CheckDisposed();
            return Handler.GetNativeBackend();
        }

        /*

        */
        public virtual void RunScriptAsync(
            string javascript,
            IntPtr? clientData = null)
        {
            CheckDisposed();
            Handler.RunScriptAsync(javascript, clientData);
        }

        /*

        */
        internal static void SetDefaultPage(string url)
        {
            Factory.SetDefaultPage(url);
        }

        /*internal virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }

        internal virtual bool RunScript(string javascript, out string result)
        {
            CheckDisposed();
            return Handler.RunScript(javascript, out result);
        }*/

        /*internal virtual string? InvokeScript(string scriptName)
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
        }*/

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return BaseApplication.Handler.CreateWebBrowserHandler(this);
        }

        /// <summary>
        /// Called when <see cref="DocumentTitleChanged"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDocumentTitleChanged(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="ScriptMessageReceived"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptMessageReceived(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Navigated"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigated(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Navigating"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigating(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Loaded"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnLoaded(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="NewWindow"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNewWindow(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="BeforeBrowserCreate"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnBeforeBrowserCreate(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="FullScreenChanged"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnFullScreenChanged(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="ScriptResult"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptResult(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Error"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnError(WebBrowserEventArgs e)
        {
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