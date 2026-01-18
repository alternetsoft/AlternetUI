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
using Alternet.UI.Extensions;

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
    /// <see cref = "Reload()" /> and <see cref= "LoadURL" />.
    /// </para >
    /// <para >
    /// To receive notification of the progress and completion of these functions
    /// you need to handle the events that are provided.
    /// Specifically<see cref="Loaded"/> event notifies when the page or a sub-frame
    /// has finished loading and<see cref="Error"/> event notifies that an
    /// error has occurred.
    /// </para>
    /// <para>
    /// Call <see cref="WebBrowser.SetBackend"/> in order to specify backend used by
    /// the next created <see cref="WebBrowser"/> control.
    /// </para>
    /// </remarks>
    [DefaultProperty("Name")]
    [DefaultEvent("Enter")]
    [ControlCategory("Common")]
    public partial class WebBrowser : Control, IWebBrowser
    {
        /// <summary>
        /// Gets "about:blank" url.
        /// </summary>
        public const string AboutBlankUrl = "about:blank";

        private static IWebBrowserFactoryHandler? factory;

        private readonly string defaultUrl = AboutBlankUrl;

        private IWebBrowserMemoryFS? fMemoryFS;

        static WebBrowser()
        {
            IsEdgeBackendEnabled = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public WebBrowser(AbstractControl parent)
            : this(string.Empty)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                defaultUrl = AboutBlankUrl;
            }
            else
                defaultUrl = url;
        }

        /// <summary>
        /// Occurs when the the page wants to enter or leave full screen.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="WebBrowserEventArgs.IntVal"/> property of the event
        /// arguments to get the status.
        /// Not implemented for the IE backend.
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? FullScreenChanged;

        /// <summary>
        /// Occurs when your application receives message from JS code of the loaded web page.
        /// </summary>
        /// <remarks>
        /// For usage details see <see cref="WebBrowser.AddScriptMessageHandler"/>.
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? ScriptMessageReceived;

        /// <summary>
        /// Occurs when your application receives results after
        /// RunScriptAsync call.
        /// </summary>
        /// <remarks>
        /// For usage details see description of RunScriptAsync method.
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? ScriptResult;

        /// <summary>
        /// Occurs when the WebBrowser control has navigated to a new web page
        /// and has begun loading it.
        /// </summary>
        /// <remarks>
        /// <para>
        ///  This event may not be canceled. Note that
        ///  if the displayed HTML document has several frames, one such event will
        ///  be generated per frame.
        /// </para>
        /// <para>
        ///  Handle the Navigated event to receive notification when the WebBrowser
        ///  control has navigated to a new web page.
        /// </para>
        /// <para>
        ///  When the Navigated event occurs, the new web page has begun loading,
        ///  which means you can access the loaded content through the WebBrowser
        ///  properties and methods.
        /// </para>
        /// <para>
        ///  Handle the <see cref="Loaded"/> event to receive notification when the
        ///  WebBrowser control finishes loading the new document.
        /// </para>
        /// <para>
        ///  You can also receive notification before navigation begins by handling the
        ///  <see cref="Navigating"/> event. Handling this event lets you cancel navigation if
        ///  certain conditions have not been met. For example, the user has not
        ///  completely filled out a form.
        /// </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? Navigated;

        /// <summary>
        /// Occurs before the WebBrowser control navigates to a new web page.
        /// </summary>
        /// <remarks>
        /// <para>
        ///  This event may be canceled to prevent navigating to this resource.
        ///  Note that if the displayed HTML document has several frames, one such
        ///  event will be generated per frame.
        /// </para>
        /// <para>
        ///  You can handle the Navigating event to cancel navigation if certain
        ///  conditions
        ///  have not been met, for example, when the user has not completely filled
        ///  out a form. To cancel navigation, set the
        ///  <see cref="System.ComponentModel.CancelEventArgs.Cancel"/> property of the events
        ///   object passed to the event handler to <see langword = "true"/>.
        /// </para>
        /// <para>
        ///  Handle the <see cref="Navigated"/> event to receive notification when the WebBrowser
        ///  control finishes navigation and has begun loading the document at the
        ///  new location.
        /// </para>
        /// <para>
        ///  Handle the <see cref="Loaded"/> event to receive
        ///  notification when the WebBrowser control finishes loading the new document.
        /// </para>
        /// <para>
        ///  You can also use <see cref="WebBrowserEventArgs"/> object to retrieve the URL of the
        ///  new document through the <see cref="WebBrowserEventArgs.Url"/> property. If the new
        ///  document will be displayed in a Web page frame, you can retrieve the
        ///  name of the frame through the
        ///  <see cref="WebBrowserEventArgs.TargetFrameName"/> property.
        /// </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? Navigating;

        /// <summary>
        /// Occurs after backend is created, but before actual browser
        /// control creation.
        /// </summary>
        /// <remarks>
        /// You should not normally use it.
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? BeforeBrowserCreate;

        /// <summary>
        /// <para>
        ///  Occurs when the WebBrowser control finishes loading a document.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        ///  Handle the Loaded event to receive notification when the new
        ///  document finishes loading. When the Loaded event occurs, the
        ///  new document is fully loaded, which means you can access its
        ///  contents through WebBrowser properties and methods.
        /// </para>
        /// <para>
        ///  To receive notification before navigation begins, handle the
        ///  <see cref="Navigating"/> event. Handling this event lets you cancel
        ///  navigation if certain conditions have not been met,
        ///  for example, when the user has not completely filled out a form.
        /// </para>
        /// <para>
        ///  Handle the <see cref="Navigated"/> event to receive notification when the
        ///  WebBrowser control finishes navigation and has begun loading
        ///  the document at the new location.
        /// </para>
        /// <para>
        ///  Note that if the displayed HTML document has several
        ///  frames, one such event will be generated per frame.
        /// </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? Loaded;

        /// <summary>
        ///  Occurs when a navigation error occurs.
        /// </summary>
        /// <remarks>
        /// <para>
        ///  The <see cref="WebBrowserEventArgs.NavigationError"/> will contain an error type.
        ///  The <see cref="WebBrowserEventArgs.Text"/> may contain a backend-specific
        ///  more precise error message or code.
        /// </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? Error;

        /// <summary>
        /// <para>
        ///  Occurs when a new browser window is created.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        ///  You must handle this event if you want anything to happen, for example to
        ///  load the page in a new window or tab.
        /// </para>
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? NewWindow;

        /// <summary>
        /// Occurs when the web page title changes.
        /// Use <see cref="WebBrowserEventArgs.Text"/> to get the title.
        /// </summary>
        /// <remarks>
        /// You can handle this event to update the title bar of your
        /// application with the current title of the loaded document.
        /// </remarks>
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;

        /// <summary>
        /// Gets or sets <see cref="IWebBrowserFactoryHandler"/> used by the web browser control.
        /// </summary>
        public static IWebBrowserFactoryHandler Factory
        {
            get => factory ??= ControlFactory.Handler.CreateWebBrowserFactoryHandler();
            set => factory = value;
        }

        /// <summary>
        /// Gets a value indicating whether the WebBrowser runs on a 64 bit platform.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the WebBrowser runs on a 64 bit platform;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Is64Bit
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }

        /// <summary>
        /// Gets or sets whether Edge backend which uses WebView2 is available on Windows.
        /// Default is <c>false</c>.
        /// Even if this property is <c>false</c>, you can create Edge backend on Windows
        /// by calling <see cref="WebBrowser.SetBackend"/>
        /// with <see cref="WebBrowserBackend.Edge"/> parameter before
        /// creating <see cref="WebBrowser"/>.
        /// </summary>
        public static bool IsEdgeBackendEnabled
        {
            get
            {
                return App.IsWindowsOS && Factory.IsEdgeBackendEnabled;
            }

            set
            {
                if (IsEdgeBackendEnabled == value)
                    return;
                Factory.IsEdgeBackendEnabled = value;
            }
        }

        /// <summary>
        /// Contains methods related to the memory scheme WebBrowser protocol.
        /// </summary>
        /// <returns>
        /// A <see cref="IWebBrowserMemoryFS"/> containing methods
        /// related to the memory scheme handling.
        /// </returns>
        [Browsable(false)]
        public virtual IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                fMemoryFS ??= Factory.CreateMemoryFileSystem(this);
                return fMemoryFS;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override ControlTypeId ControlKind => ControlTypeId.WebBrowser;

        /// <summary>
        /// Gets default url (the first loaded url).
        /// </summary>
        [Browsable(false)]
        public string DefaultUrl => defaultUrl;

        /// <summary>
        /// Gets a value indicating whether there is a current selection in the control.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if there is a current selection;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool HasSelection
        {
            get
            {
                if (DisposingOrDisposed)
                    return false;
                return Handler.HasSelection;
            }
        }

        /// <summary>
        /// Gets or sets the zoom factor of the page.
        /// </summary>
        /// <returns>
        /// A <see cref="float"/> representing the the zoom factor of the page.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The zoom factor is an arbitrary
        /// number that specifies how much to zoom (scale) the HTML document.
        /// </para>
        /// <para>
        /// Zoom scale in IE will be converted into zoom levels if <see cref="ZoomType"/> property
        /// is set to <see cref="WebBrowserZoomType.Text"/> value.
        /// </para>
        /// </remarks>
        public virtual float ZoomFactor
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ZoomFactor;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ZoomFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return base.Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                base.Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets how the zoom factor is currently interpreted by the HTML engine.
        /// </summary>
        /// <returns>
        /// A <see cref="WebBrowserZoomType"/> representing how the zoom factor
        /// is currently interpreted by the HTML engine.
        /// </returns>
        /// <remarks>
        /// Invoke <see cref="CanSetZoomType"/> first, some HTML renderers may not
        /// support all zoom types.
        /// </remarks>
        public virtual WebBrowserZoomType ZoomType
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ZoomType;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ZoomType = value;
            }
        }

        /// <summary>
        /// Gets whether backend is WebView2.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEdgeBackend
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return App.IsWindowsOS && Handler.IsEdgeBackend;
            }
        }

        /// <summary>
        /// Gets or sets the Uri of the current document hosted in the WebBrowser.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Uri"/> representing the URL of the current document.
        /// </returns>
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

        /// <summary>
        /// Gets a value indicating whether the zoom factor of the page can be increased,
        /// which allows the <see cref="ZoomIn"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the web page can be zoomed in;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanZoomIn
        {
            get => Zoom != WebBrowserZoom.Largest;
        }

        /// <summary>
        /// Gets a value indicating whether a previous page in navigation history
        /// is available, which allows the<see cref="GoBack" />
        /// method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the control can navigate backward; otherwise,
        /// <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanGoBack
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanGoBack;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true"/> if the user can undo the previous operation performed
        /// in the control; otherwise, <see langword = "false"/>.
        /// </returns >
        [Browsable(false)]
        public virtual bool CanUndo
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanUndo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can redo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can redo the previous operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns >
        [Browsable(false)]
        public virtual bool CanRedo
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanRedo;
            }
        }

        /// <summary>
        /// Enables or disables access to developer tools for the user.
        /// </summary>
        /// <remarks>
        /// Developer tools are disabled by default.
        /// This feature is not implemented for the IE backend.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the developer tools are enabled for the user;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool AccessToDevToolsEnabled
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.AccessToDevToolsEnabled;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.AccessToDevToolsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom user agent string for the WebBrowser control.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the custom user agent string
        /// for the WebBrowser control.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If your first request should already use the custom user agent please
        /// use two step creation and set UserAgent before browser creation.
        /// </para>
        /// <para>
        /// This is not implemented for IE. For the Edge backend set UserAgent
        /// BEFORE backend creation.
        /// </para>
        /// </remarks>
        public virtual string UserAgent
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.UserAgent;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.UserAgent = value;
            }
        }

        /// <summary>
        /// Enables or disables the right click context menu.
        /// </summary>
        /// <remarks>
        /// By default the standard context menu is enabled, this property
        /// can be used to disable it or re-enable it later.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the right click context menu is enabled;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool ContextMenuEnabled
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ContextMenuEnabled;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ContextMenuEnabled = value;
            }
        }

        /// <summary>
        /// Gets the current WebBrowser backend.
        /// </summary>
        /// <returns>
        /// A <see cref="WebBrowserBackend"/> representing the current WebBrowser backend.
        /// </returns>
        [Browsable(false)]
        public virtual WebBrowserBackend Backend
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.Backend;
            }
        }

        /// <summary>
        /// Gets or sets whether the control is currently editable.
        /// </summary>
        /// <remarks>
        /// This property allows the user to edit the page even
        /// if the 'contenteditable' attribute is not set in HTML.
        /// The exact capabilities vary with the backend being used.
        /// This feature is not implemented for macOS and the Edge backend.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the control is currently editable;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Editable
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.Editable;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.Editable = value;
            }
        }

        /// <summary>
        /// Gets or sets the zoom level of the page.
        /// </summary>
        /// <remarks>
        /// See <see cref="ZoomFactor"/> to get more precise zoom scale value other than
        /// as provided by this property.
        /// </remarks>
        public virtual WebBrowserZoom Zoom
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.Zoom;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.Zoom = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is currently busy (e.g. loading a web page).
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the control is currently busy;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool IsBusy
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.IsBusy;
            }
        }

        /// <summary>
        /// Gets the HTML source code of the currently displayed document or
        /// an empty string if no page is currently shown.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the HTML source code of the currently
        /// displayed document.
        /// </returns>
        /// <seealso cref="PageText"/>
        [Browsable(false)]
        public virtual string PageSource
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.PageSource;
            }
        }

        /// <summary>
        /// Gets the text of the current page.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the text of the current page.
        /// </returns>
        /// <seealso cref="PageSource"/>
        [Browsable(false)]
        public virtual string PageText
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.PageText;
            }
        }

        /// <summary>
        /// Gets a value indicating the currently selected text in the control.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the currently selected text in the control.
        /// </returns>
        /// <seealso cref="SelectedSource"/>
        [Browsable(false)]
        public virtual string SelectedText
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.SelectedText;
            }
        }

        /// <summary>
        /// Gets the HTML source code of the currently selected portion
        /// of the web page or
        /// an empty string if no selection exists.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the HTML source code of
        /// the currently selected portion of the web page.
        /// </returns>
        /// <seealso cref="SelectedText"/>
        [Browsable(false)]
        public virtual string SelectedSource
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.SelectedSource;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be cut, which allows
        /// the <see cref="Cut"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current selection can be cut;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCut
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanCut;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the current document hosted in the WebBrowser.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the URL of the current document.
        /// </returns>
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

        /// <summary>
        /// Sets the overall color scheme of the WebBrowser.
        /// </summary>
        /// <returns>
        /// A <see cref="WebBrowserPreferredColorScheme"/> representing the overall
        /// color scheme of the WebBrowser.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This property sets the color scheme for WebBrowser UI like dialogs, prompts
        /// and menus. The default value is Auto, which will follow
        /// whatever color scheme the operating system is currently set to.
        /// </para>
        /// <para>
        /// This property works only for the Edge backend.
        /// </para>
        /// <para>
        /// Use this property only inside the <see cref="WebBrowser.Loaded"/> event.
        /// </para>
        /// </remarks>
        public virtual WebBrowserPreferredColorScheme PreferredColorScheme
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.PreferredColorScheme;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.PreferredColorScheme = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be copied, which allows
        /// the <see cref="Copy"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current selection can be copied;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCopy
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanCopy;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be replaced with
        /// the contents of the Clipboard, which allows
        /// the <see cref="Paste"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the data can be pasted;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanPaste
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanPaste;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a subsequent page in navigation
        /// history is available, which allows the
        /// <see cref="GoForward" /> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the control can navigate forward; otherwise,
        /// <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanGoForward
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.CanGoForward;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the zoom factor of the page can be decreased,
        /// which allows the <see cref="ZoomOut"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the web page can be zoomed out;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }

        /// <summary>
        /// Gets handler for the control.
        /// </summary>
        [Browsable(false)]
        public new IWebBrowserLite Handler => (IWebBrowserLite)base.Handler;

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

        /// <summary>
        /// Sets path to a fixed version of the WebView2 Edge runtime.
        /// </summary>
        /// <param name="path">
        /// Path to an extracted fixed version of the WebView2 Edge runtime.
        /// </param>
        /// <param name="isRelative">
        /// <see langword = "true"/> if specified path is relative to the application
        /// folder; otherwise, <see langword = "false"/>.
        /// </param>
        /// <example>
        /// <code language="C#">
        /// SetBackendPath("Edge",true);
        /// </code>
        /// </example>
        public static void SetBackendPath(string path, bool isRelative = false)
        {
            if (isRelative)
            {
                string location = App.ExecutingAssemblyLocation;
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

        /// <summary>
        /// Executes the browser command with the specified name and parameters.
        /// This is the static version of <see cref="DoCommand"/>.
        /// </summary>
        /// <param name="cmdName">
        /// Name of the command to execute.
        /// </param>
        /// <param name="args">
        /// Parameters of the command.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> representing the result of the command execution.
        /// </returns>
        public static string? DoCommandGlobal(string cmdName, params object?[] args)
        {
            if (cmdName == "CppThrow")
            {
                Factory.ThrowError(1);
                return string.Empty;
            }

            return Factory.DoCommand(cmdName, args);
        }

        /// <summary>
        /// Modifies the state of the debug flag to control the
        /// allocation behavior of the debug heap manager.
        /// </summary>
        /// <param name="value">
        /// New debug flag state.
        /// </param>
        /// <remarks>
        /// This is for debug purposes.
        /// CrtSetDbgFlag(0) allows to turn off debug output with heap manager information.
        /// </remarks>
        public static void CrtSetDbgFlag(int value)
        {
            App.Handler.CrtSetDbgFlag(value);
        }

        /// <summary>
        /// Allows to check if a specific WebBrowser backend is currently available.
        /// </summary>
        /// <remarks>
        /// This method allows to enable some extra functionality implemented only in
        /// the specific backend.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if a specific backend is currently available;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <seealso cref="SetBackend"/>
        public static bool IsBackendAvailable(WebBrowserBackend value)
        {
            return Factory.IsBackendAvailable(value);
        }

        /// <summary>
        /// Sets the backend that will be used for the new WebBrowser instances.
        /// </summary>
        /// <param name="value">
        /// Specifies the WebBrowser backend.
        /// </param>
        /// <seealso cref="IsBackendAvailable"/>
        public static void SetBackend(WebBrowserBackend value)
        {
            if (value == WebBrowserBackend.IE || value == WebBrowserBackend.IELatest
                || value == WebBrowserBackend.Edge)
            {
                if (!App.IsWindowsOS)
                    value = WebBrowserBackend.Default;
            }

            Factory.SetBackend(value);
        }

        /// <summary>
        /// Retrieve the version information about the underlying library implementation.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the version information about the
        /// underlying library implementation.
        /// </returns>
        public static string GetLibraryVersionString()
        {
            return Factory.GetLibraryVersionString();
        }

        /// <summary>
        /// Retrieve the version information about the browser backend implementation.
        /// </summary>
        /// <returns>
        ///     A <see cref="string"/> representing the version information of the browser backend.
        /// </returns>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return Factory.GetBackendVersionString(value);
        }

        /// <summary>
        /// Sets the default user agent that will be used for the
        /// new WebBrowser instances.
        /// </summary>
        /// <param name="value">
        /// Default user agent.
        /// </param>
        /// <remarks>
        /// If you specify non empty value here, WebBrowser will use it when web page
        /// asks for the user agent type
        /// instead of value which is normally returned by the WebBrowser backend.
        /// </remarks>
        public static void SetDefaultUserAgent(string value)
        {
            Factory.SetDefaultUserAgent(value);
        }

        /// <summary>
        /// Sets the default script message name that will be registered in all
        /// new WebBrowser instances.
        /// </summary>
        /// <param name="value">
        /// Default script message name.
        /// </param>
        /// <remarks>
        /// If you specify non empty value here, script messaging will be registered
        /// for use in JavaScript code of the loaded web pages.
        /// </remarks>
        public static void SetDefaultScriptMessageName(string value)
        {
            Factory.SetDefaultScriptMesageName(value);
        }

        /// <summary>
        /// Sets the default protocol name for the memory file system
        /// that will be used for the new WebBrowser instances.
        /// </summary>
        /// <param name="value">
        /// Protocol name for the memory file system.
        /// </param>
        /// <remarks>
        /// If you specify non empty value here, memory file system will be automatically
        /// registered for use with the WebBrowser control.
        /// </remarks>
        public static void SetDefaultFSNameMemory(string value)
        {
            Factory.SetDefaultFSNameMemory(value);
        }

        /// <summary>
        /// Sets the default protocol name for the archive file system that will
        /// be used for the new WebBrowser instances.
        /// </summary>
        /// <param name="value">
        /// Protocol name for the archive file system.
        /// </param>
        /// <remarks>
        /// If you specify non empty value here, archive file system will be automatically
        /// registered for use with the WebBrowser control.
        /// </remarks>
        public static void SetDefaultFSNameArchive(string value)
        {
            Factory.SetDefaultFSNameArchive(value);
        }

        /// <summary>
        /// Sets the best possible backend to be used for the
        /// new WebBrowser instances.
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
        /// Loads a web page from a URL if it is in a valid format.
        /// Otherwise opens passed URL in the Google search.
        /// </summary>
        /// <param name="url">
        /// A <see cref="string"/> representing the URL of the document to load.
        /// If this parameter is null, WebBrowser navigates to a blank document.
        /// </param>
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
            if (!url.ContainsSpace() && url.ContainsDot())
            {
                // An invalid URI contains a dot and no spaces,
                // try tacking http:// on the front.
                uri = new Uri("http://" + url);
            }
            else
            {
                // Otherwise treat it as a web search.
#pragma warning disable
                uri = new Uri("https://google.com/search?q=" +
                    string.Join("+", Uri.EscapeDataString(url).Split(
                        new string[] { "%20" }, // Avoid constant arrays as arguments
                        StringSplitOptions.RemoveEmptyEntries)));
#pragma warning restore
            }

            LoadURL(uri.ToString());
        }

        /// <summary>
        /// Executes a browser command with the specified name and parameters.
        /// </summary>
        /// <param name="cmdName">
        /// Name of the command to execute.
        /// </param>
        /// <param name="args">
        /// Parameters of the command.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> representing the result of the command execution.
        /// </returns>
        public virtual string? DoCommand(string cmdName, params object?[] args)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.DoCommand(cmdName, args);
        }

        /// <summary>
        /// Increases the zoom factor of the page.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The zoom factor is an arbitrary
        /// number that specifies how much to zoom (scale) the HTML document.
        /// </para>
        /// <para>
        /// Zoom scale in IE will be converted into zoom levels if ZoomType property
        /// is set to <see cref="WebBrowserZoomType.Text"/> value.
        /// </para>
        /// </remarks>
        public virtual void ZoomIn()
        {
            if (CanZoomIn)
                ZoomInOut(1);
        }

        /// <summary>
        /// Decreases the zoom factor of the page.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The zoom factor is an arbitrary
        /// number that specifies how much to zoom (scale) the HTML document.
        /// </para>
        /// <para>
        /// Zoom scale in IE will be converted into zoom levels if ZoomType property
        /// is set to <see cref="WebBrowserZoomType.Text"/> value.
        /// </para>
        /// </remarks>
        public virtual void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }

        /// <summary>
        /// Loads the document at the specified Uniform Resource Locator (URL)
        /// into the WebBrowser control, replacing
        /// the previous document.
        /// </summary>
        /// <param name="urlString">
        /// The URL of the document to load. If this parameter is null, WebBrowser
        /// navigates to a blank document. You must always specify protocol prefix
        /// (https, file or other). Not all browser backends support
        /// loading of the web pages without protocol prefix.
        /// </param>
        /// <seealso cref="NavigateToStream"/>
        public void Navigate(string urlString)
        {
            LoadURL(urlString);
        }

        /// <summary>
        /// Loads the document at the location indicated by the specified
        /// <see cref="System.Uri"/> into the WebBrowser control,
        /// replacing the previous document.
        /// </summary>
        /// <param name="source">
        /// A <see cref="System.Uri"/> representing the URL of the document to load.
        /// If this parameter is null, WebBrowser navigates to a blank document.
        /// </param>
        /// <seealso cref="NavigateToStream"/>
        public void Navigate(Uri? source)
        {
            if (source == null)
                LoadURL();
            else
                LoadURL(source.ToString());
        }

        /// <summary>
        /// Navigates the control
        /// to the previous page in the navigation history, if one is available.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the navigation succeeds;
        /// <see langword="false"/> if a previous page in the navigation history is not available.
        /// </returns>
        public virtual bool GoBack()
        {
            if (!CanGoBack)
                return false;
            if (DisposingOrDisposed)
                return default;
            return Handler.GoBack();
        }

        /// <summary>
        /// Navigates the control
        /// to the subsequent page in the navigation history, if one is available.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the navigation succeeds;
        /// <see langword="false"/> if a subsequent page in the navigation history is not available.
        /// </returns>
        public virtual bool GoForward()
        {
            if (!CanGoForward)
                return false;
            if (DisposingOrDisposed)
                return default;
            return Handler.GoForward();
        }

        /// <summary>
        /// Stops the current page loading process, if any. Cancels any pending navigation
        /// and stops any dynamic page elements, such as background sounds and animations.
        /// </summary>
        public virtual void Stop()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Stop();
        }

        /// <summary>
        /// Clear the history, this will also remove the visible page.
        /// </summary>
        /// <remarks>
        /// This is not implemented on macOS and the WebKit2GTK+ backend.
        /// </remarks>
        public virtual void ClearHistory()
        {
            if (DisposingOrDisposed)
                return;
            Handler.ClearHistory();
        }

        /// <summary>
        /// Enables or disables the history.
        /// </summary>
        /// <param name="enable">
        /// <see langword = "true"/> to enable history; otherwise, <see langword = "false"/>.
        /// </param>
        /// <remarks>
        /// This method will also clear the history.
        /// This method is not implemented on macOS and the WebKit2GTK+ backend.
        /// </remarks>
        public virtual void EnableHistory(bool enable = true)
        {
            if (DisposingOrDisposed)
                return;
            Handler.EnableHistory(enable);
        }

        /// <summary>
        /// Reloads the document currently displayed in the control by downloading
        /// for an updated version from the server.
        /// </summary>
        public virtual void Reload()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Reload();
        }

        /// <summary>
        /// Reloads the document currently displayed in the
        /// control using the specified refresh option.
        /// </summary>
        /// <param name="noCache">
        /// <see langword="true"/> if the reload will not use browser cache; otherwise,
        /// <see langword="false"/>. This parameter is ignored by the Edge backend.
        /// </param>
        public virtual void Reload(bool noCache)
        {
            if (DisposingOrDisposed)
                return;
            Handler.Reload(noCache);
        }

        /// <summary>
        /// Sets the displayed page source to the contents of the given string.
        /// </summary>
        /// <remarks>
        /// When using the IE backend you must wait for the current page to finish loading before
        /// calling this method. The baseURL parameter is not used in the IE and
        /// and Edge backends.
        /// </remarks>
        /// <param name="html">
        /// The string that contains the HTML data to display. If this parameter is null,
        /// WebBrowser navigates to a blank document. If value of this parameter is
        /// not in valid HTML
        /// format, it will be displayed as plain text.
        /// </param>
        /// <param name="baseUrl">
        /// URL assigned to the HTML data, to be used to resolve relative paths, for instance.
        /// </param>
        public virtual void NavigateToString(string html, string? baseUrl = null)
        {
            if (DisposingOrDisposed)
                return;
            if (string.IsNullOrEmpty(html))
            {
                LoadURL();
                return;
            }

            Handler.NavigateToString(html, baseUrl ?? string.Empty);
        }

        /// <summary>
        /// Navigate to a Stream that contains the content for a document.
        /// </summary>
        /// <param name="stream">
        /// The Stream that contains the content for a document.
        /// If this parameter is null, WebBrowser navigates to a blank document.
        /// If contents of the stream is not in a valid HTML format, it will
        /// be displayed as plain text.
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
        /// Retrieve whether the current HTML engine supports a zoom type.
        /// </summary>
        /// <param name="zoomType">
        /// The zoom type to test.
        /// </param>
        /// <returns>
        /// Whether this type of zoom is supported by this HTML engine
        /// (and thus can be set through <see cref="ZoomType"/> property).
        /// </returns>
        public virtual bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            if (DisposingOrDisposed)
                return default;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            NewWindow?.Invoke(this, e);
            OnNewWindow(e);
        }

        /// <summary>
        /// Selects the entire page.
        /// </summary>
        /// <seealso cref="SelectedSource"/>
        /// <seealso cref="SelectedText"/>
        public virtual void SelectAll()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SelectAll();
        }

        /// <summary>
        /// Deletes the current selection.
        /// </summary>
        /// <remarks>
        /// Note that for the Webkit backend the selection must be editable,
        /// either through the correct HTML attribute or <see cref="Editable"/> property.
        /// </remarks>
        /// <seealso cref="ClearSelection"/>
        /// <seealso cref="HasSelection"/>
        public virtual void DeleteSelection()
        {
            if (DisposingOrDisposed)
                return;
            Handler.DeleteSelection();
        }

        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        /// <seealso cref="CanUndo"/>
        public virtual void Undo()
        {
            if (!CanUndo)
                return;
            if (DisposingOrDisposed)
                return;
            Handler.Undo();
        }

        /// <summary>
        /// Redo the last edit operation in the control.
        /// </summary>
        /// <seealso cref="CanRedo"/>
        public virtual void Redo()
        {
            if (!CanRedo)
                return;
            if (DisposingOrDisposed)
                return;
            Handler.Redo();
        }

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        /// <remarks>
        /// Specifies that no characters are selected in the control.
        /// </remarks>
        /// <seealso cref="DeleteSelection"/>
        /// <seealso cref="HasSelection"/>
        public virtual void ClearSelection()
        {
            if (DisposingOrDisposed)
                return;
            Handler.ClearSelection();
        }

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            if (!CanCut)
                return;
            if (DisposingOrDisposed)
                return;
            Handler.Cut();
        }

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (!CanCopy)
                return;
            if (DisposingOrDisposed)
                return;
            Handler.Copy();
        }

        /// <summary>
        /// Replaces the current selection in the control with the contents of the Clipboard.
        /// </summary>
        /// <seealso cref="CanPaste"/>
        public virtual void Paste()
        {
            if (!CanPaste)
                return;
            if (DisposingOrDisposed)
                return;
            Handler.Paste();
        }

        /// <summary>
        /// Finds a text on the current page and if found, the control will scroll
        /// the text into view and select it.
        /// </summary>
        /// <param name="prm">
        /// The parameters for the search.
        /// </param>
        /// <param name="text">
        /// The phrase to search for.
        /// </param>
        /// <returns>
        /// <para>
        /// If search phrase was not found in combination with the
        /// flags then -1 is returned.
        /// </para>
        /// <para>
        /// If called for the first time with search phrase then the
        /// total number of results will be returned. Then for every
        /// time its called with the same search phrase it will
        /// return the number of the current match.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// This function will restart the search if the search params are changed,
        /// since this will require a new search.
        /// </para>
        /// <para>
        /// To reset the search, for example resetting the highlighted text call
        /// <see cref="FindClearResult"/>.
        /// </para>
        /// </remarks>
        public virtual int Find(string text, WebBrowserFindParams? prm = null)
        {
            if (DisposingOrDisposed)
                return -1;
            return Handler.Find(text, prm);
        }

        /// <summary>
        /// Resets the search and the highlighted results.
        /// </summary>
        /// <seealso cref="Find"/>
        public virtual void FindClearResult()
        {
            if (DisposingOrDisposed)
                return;
            Handler.FindClearResult();
        }

        /// <summary>
        /// Opens a print dialog so that the user may change the current print and
        /// page settings and print the currently displayed page.
        /// </summary>
        public virtual void Print()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Print();
        }

        /// <summary>
        /// Removes all user scripts from the WebBrowser.
        /// </summary>
        public virtual void RemoveAllUserScripts()
        {
            if (DisposingOrDisposed)
                return;
            Handler.RemoveAllUserScripts();
        }

        /// <summary>
        /// <para>
        /// Adds a script message handler with the given name.
        /// </para>
        /// </summary>
        /// <example>
        /// <code language="C#">
        /// bool ScriptMessageHandlerAdded = false;
        /// ScriptMessageHandlerAdded = WebBrowser1.AddScriptMessageHandler("wx_msg");
        /// if(!ScriptMessageHandlerAdded)
        /// Log("AddScriptMessageHandler not supported");
        /// if(ScriptMessageHandlerAdded)
        /// WebBrowser1.RunScriptAsync(
        /// "window.wx_msg.postMessage('This is a message body');");
        /// </code>
        /// </example>
        /// <returns>
        /// <see langword="true"/> if the handler could be added,
        /// <see langword="false"/> if it could not be added.
        /// </returns>
        /// <param name="name">
        /// Name of the message handler that can be used from JavaScript.
        /// </param>
        /// <remarks>
        /// <para>
        /// To use the script message handler from JavaScript
        /// use window._name_.postMessage(_messageBody_)
        /// where _name_ corresponds the value of the name parameter.
        /// The _messageBody_ will be available to the application via
        /// a <see cref="WebBrowser.ScriptMessageReceived"/> event.
        /// </para>
        /// <para>
        /// The Edge backend only supports a single message handler and
        /// the IE backend does not support script message handlers.
        /// </para>
        /// </remarks>
        public virtual bool AddScriptMessageHandler(string name)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.AddScriptMessageHandler(name);
        }

        /// <summary>
        /// Remove a script message handler with the given name that was previously
        /// added using <see cref="AddScriptMessageHandler"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the handler could be removed,
        /// <see langword="false"/> if it could not be removed.
        /// </returns>
        public virtual bool RemoveScriptMessageHandler(string name)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.RemoveScriptMessageHandler(name);
        }

        /// <summary>
        /// Injects the specified script into the webpage's content.
        /// </summary>
        /// <param name="javaScript">
        /// The JavaScript code to add.
        /// </param>
        /// <param name="injectDocStart">
        /// Specifies when the script will be executed.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the script was added successfully.
        /// </returns>
        /// <remarks>
        /// Please note that this is unsupported by the IE backend.
        /// The Edge backend does only support injecting at document start.
        /// </remarks>
        public virtual bool AddUserScript(
            string javaScript,
            bool injectDocStart = true)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.AddUserScript(javaScript, injectDocStart);
        }

        /// <summary>
        /// Converts the value into a JSON string. It is a simple convertor.
        /// It is better to use System.Text.Json.JsonSerializer.Serialize method.
        /// </summary>
        /// <param name="v">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The JSON string representation of the value.
        /// </returns>
        /// <remarks>
        /// Do not call it for primitive values. It supposed to convert only
        /// TypeCode.Object values. Outputs result like this:
        /// {firstName:"John", lastName:"Doe", age:50, eyeColor:"blue"}
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

        /// <summary>
        /// Converts the array of object values to the comma delimited JSON string.
        /// </summary>
        /// <remarks>
        /// Used internally by <see cref="InvokeScriptAsync"/>.
        /// String and DateTime values are returned enclosed in single quotes.
        /// </remarks>
        /// <param name="args">
        /// An array of object values for conversion to JSON format.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> representing the array of object values in JSON format.
        /// </returns>
        /// <seealso cref="ToInvokeScriptArg"/>
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

        /// <summary>
        /// Scrolls document opened in the browser to the bottom.
        /// Uses js script and <see cref="RunScriptAsync"/>. Works asynchronously.
        /// Requires loaded document, can be used in the <see cref="Loaded"/> event handler.
        /// </summary>
        public virtual void ScrollToBottomAsyncJs()
        {
            string script = @"window.scrollTo(0, document.body.scrollHeight);";
            RunScriptAsync(script);
        }

        /// <summary>
        /// <para>
        /// Executes the given JavaScript function asynchronously and returns the
        /// result via a <see cref="ScriptResult"/> event.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The script result value can be retrieved via
        /// <see cref="WebBrowserEventArgs.Text"/> parameter of ScriptResult event args.
        /// If the execution fails
        /// <see cref="WebBrowserEventArgs.IsError"/> will return <see langword = "true"/>.
        /// In this case additional script execution error information may be
        /// available via <see cref="WebBrowserEventArgs.Text"/>.
        /// </para>
        /// </remarks>
        /// <param name="scriptName">
        /// The name of the script function to execute.
        /// </param>
        /// <param name="args">
        /// The parameters to pass to the script function.
        /// </param>
        /// <param name="clientData">
        /// Arbitrary pointer to data that can be retrieved from the result event.
        /// You can use IntPtr.Zero, new IntPtr(SomeInt) or some useful data in
        /// this parameter.
        /// </param>
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

        /// <summary>
        /// Converts the value to the JSON string.
        /// </summary>
        /// <remarks>
        /// Used internally by <see cref="InvokeScriptAsync"/>.
        /// String and DateTime values are returned enclosed in single quotes.
        /// </remarks>
        /// <param name="arg">
        /// Value for conversion to JSON format.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> representing value in JSON format.
        /// </returns>
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

        /// <summary>
        /// Loads a web page from a URL.
        /// </summary>
        /// <param name="url">
        /// A <see cref="string"/> representing the URL of the document to load.
        /// If this parameter is null, WebBrowser navigates to a blank document.
        /// </param>
        /// <remarks>
        /// Web engines generally report errors asynchronously, so if you
        /// want to know whether loading process was successful, register to receive
        /// navigation error events.
        /// </remarks>
        public virtual void LoadURL(string? url = null)
        {
            if (DisposingOrDisposed)
                return;
            url ??= AboutBlankUrl;
            Handler.LoadURL(url);
        }

        /// <summary>
        /// Get the title of the current web page.
        /// </summary>
        /// <remarks>
        /// Returns URL/path if title of the current web page is not available.
        /// </remarks>
        /// <returns>
        /// A <see cref="string"/> representing the title of the current web page.
        /// </returns>
        public virtual string GetCurrentTitle()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetCurrentTitle();
        }

        /// <summary>
        /// Get the URL of the currently displayed document.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the URL of the currently displayed document.
        /// </returns>
        public virtual string GetCurrentURL()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetCurrentURL();
        }

        /// <summary>
        /// Sets a mapping between a virtual host name and a folder path to make
        /// available to web sites via that host name.
        /// </summary>
        /// <param name="hostName">
        /// A virtual host name.
        /// </param>
        /// <param name="folderPath">
        /// A folder path name to be mapped to the virtual host name.
        /// </param>
        /// <param name="accessKind">
        /// The level of access to resources under the virtual host from other sites.
        /// </param>
        /// <remarks>
        /// <para>
        /// After setting the mapping, documents loaded in the WebBrowser can use HTTP or
        /// HTTPS URLs at the specified host name specified by hostName to
        /// access files in the local folder specified by folderPath.
        /// </para>
        /// <para>
        /// This property works only for the Edge backend.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// WebControl1.SetVirtualHostNameToFolderMapping(
        /// "appAsset.example", "assets", WebBrowserHostResourceAccessKind.DenyCors);
        /// WebControl1.Source = new Uri("https://appassets.example/index.html");
        /// </code>
        /// </example>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.setvirtualhostnametofoldermapping">
        /// WebView2.SetVirtualHostNameToFolderMapping</seealso>
        public virtual void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }

        /// <summary>
        /// Ensures that the current URL starts with the specified prefix.
        /// </summary>
        /// <remarks>If the current URL is null, empty, or does not start with the specified prefix,
        /// it is set to the provided <paramref name="url"/>. The method does nothing
        /// if the object is in a disposing or disposed state.</remarks>
        /// <param name="url">The URL prefix to ensure. Cannot be null or empty.</param>
        public virtual bool EnsureUrlStartsWith(string url)
        {
            if (DisposingOrDisposed)
                return false;
            if (string.IsNullOrEmpty(url))
                return false;

            var s = Url;

            if (string.IsNullOrEmpty(s) || !s.StartsWith(url))
            {
                Url = url;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns pointer to the native backend interface.
        /// </summary>
        /// <remarks>
        /// For the IE backends it is a <c>IWebBrowser2</c> interface.
        /// For the Edge backend it is a <c>ICoreWebView2</c> interface.
        /// Under macOS it is a <c>WebView</c> pointer and under GTK
        /// it is a <c>WebKitWebView</c>.
        /// </remarks>
        /// <returns>
        /// A <see cref="IntPtr"/> value representing the native backend class or interface.
        /// </returns>
        public IntPtr GetNativeBackend()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetNativeBackend();
        }

        /// <summary>
        /// <para>
        /// Runs the given JavaScript code asynchronously and returns the
        /// result via a <see cref="WebBrowser.ScriptResult"/> event.
        /// </para>
        /// </summary>
        /// <param name="javaScript">
        /// JavaScript code to execute.
        /// </param>
        /// <param name="clientData">
        /// Arbitrary pointer to data that can be used in the result event.
        /// You can use IntPtr.Zero, new IntPtr(SomeInt) or pointer to some useful data in
        /// this parameter.
        /// </param>
        /// <remarks>
        /// <para>
        /// The script result value can be retrieved via
        /// <see cref="WebBrowserEventArgs.Text"/> parameter or a
        /// <see cref="WebBrowser.ScriptResult"/> event.
        /// If the execution fails
        /// <see cref="WebBrowserEventArgs.IsError"/> will return true.
        /// In this case additional script execution error information may be
        /// available via <see cref="WebBrowserEventArgs.Text"/>.
        /// </para>
        /// <para>
        /// This function has a few platform-specific limitations:
        /// </para>
        /// <para>
        /// When using WebKit v1 (wxGTK2), retrieving the result of script execution
        /// is unsupported. Running scripts functionality is fully supported when
        /// using WebKit v2 or later (wxGTK3).
        /// </para>
        /// <para>
        /// When using WebKit (macOS), code execution is limited to
        /// 10MiB of memory and 10 seconds of execution time.
        /// </para>
        /// <para>
        /// When using IE backend, scripts can only be executed when
        /// the current page is fully loaded and <see cref="WebBrowser.Loaded"/> event was received.
        /// A script tag inside the page HTML is required in order to run JavaScript.
        /// </para>
        /// <para>
        /// Under MSW converting JavaScript objects to JSON is not
        /// supported in the IE backend. WebBrowser implements its
        /// own conversion as a fallback for this case.
        ///
        /// However it is not as full-featured or performing as
        /// the implementation of this functionality in the browser
        /// itself. It is recommended to use IELatest or Edge backends
        /// in which JSON conversion is done by the browser itself.
        /// </para>
        /// </remarks>
        public virtual void RunScriptAsync(
            string javaScript,
            IntPtr? clientData = null)
        {
            if (DisposingOrDisposed)
                return;
            Handler.RunScriptAsync(javaScript, clientData);
        }

        /// <summary>
        /// Sets the default web page that will be used for the
        /// new WebBrowser instances.
        /// </summary>
        /// <remarks>
        /// Default page is automatically opened in WebBrowser after it is created.
        /// Default value is 'about:blank'.
        /// </remarks>
        /// <param name="url">
        /// New default web page URL.
        /// </param>
        internal static void SetDefaultPage(string url)
        {
            Factory.SetDefaultPage(url);
        }

        /// <summary>
        /// Runs the given JavaScript code.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     JavaScript code is executed inside the
        ///     browser and has full access to DOM and other browser-provided functionality.
        ///   </para>
        ///   <para>
        ///     Because of various potential issues it's recommended to use
        ///     <see cref="RunScriptAsync"/>
        ///     instead of this method.
        ///     This is especially true if you plan to run code from a WebBrowser event
        ///     and will also prevent unintended side effects on the UI outside of
        ///     the WebBrowser.
        ///   </para>
        /// </remarks>
        /// <example>
        ///   For example, this code
        ///   <code>
        ///     WebBrowser.RunScript("document.write('Hello!')");
        ///   </code>
        ///   will replace the current page contents with the provided string.
        /// </example>
        /// <param name="javascript">
        ///   JavaScript code to execute.
        /// </param>
        /// <returns>
        ///   <see langword="true"/> if there is a result,
        ///   <see langword="false"/> if there is an error.
        /// </returns>
        internal virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }

        /// <param name="javascript">
        ///   JavaScript code to execute.
        /// </param>
        /// <param name="result">
        ///   <para>
        ///     Result of the script execution.
        ///   </para>
        ///   <para>
        ///     If output is non-null, it is filled with the result of
        ///     executing this code on success, e.g. a JavaScript value
        ///     such as a string, a number (integer or floating point), a boolean
        ///     or JSON representation for non-primitive types such as arrays and objects.
        ///   </para>
        /// </param>
        internal virtual bool RunScript(string javascript, out string result)
        {
            result = string.Empty;
            return false;
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

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return Factory.CreateWebBrowserHandler(this);
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

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
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

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        /// <inheritdoc/>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
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