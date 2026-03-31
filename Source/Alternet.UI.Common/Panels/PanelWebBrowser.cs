using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="WebBrowser"/> and toolbar with
    /// web navigation buttons.
    /// </summary>
    [ControlCategory("Panels")]
    public partial class PanelWebBrowser : HiddenBorder
    {
        /// <summary>
        /// Gets or sets find parameters.
        /// </summary>
        public static WebBrowserFindParams FindParams = new();

        private static WeakReference<WindowWebBrowserSearch>? searchWindow;

        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;

        private readonly SplittedPanel panel = new()
        {
            LeftVisible = false,
            RightVisible = false,
            TopVisible = false,
            BottomVisible = false,
            RightPanelWidth = 150,
            BottomPanelHeight = 200,
        };

        private readonly ToolBar toolBar = new()
        {
        };

        private readonly TextBox urlTextBox = new()
        {
            MinWidth = 350,
            HorizontalAlignment = HorizontalAlignment.Fill,
        };

        private readonly ContextMenu moreActionsMenu = new();
        private readonly string defaultUrl = "about:blank";
        private WebBrowser? webBrowser;
        private bool historyCleared = false;

        private ObjectUniqueId buttonIdBack;
        private ObjectUniqueId buttonIdMoreActions;
        private ObjectUniqueId buttonIdForward;
        private ObjectUniqueId buttonIdZoomIn;
        private ObjectUniqueId buttonIdZoomOut;
        private ObjectUniqueId buttonIdUrl;
        private ObjectUniqueId buttonIdGo;

        private bool logEvents;
        private bool canNavigate = true;
        private int scriptRunCounter = 0;
        private ActionsListBox? actionsControl;
        private BaseDictionary<string, string> urlMapping = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWebBrowser"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelWebBrowser(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWebBrowser"/> class.
        /// </summary>
        public PanelWebBrowser()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWebBrowser"/> class.
        /// </summary>
        /// <param name="url">Default url.</param>
        public PanelWebBrowser(string url)
        {
            defaultUrl = url;
            Initialize();
        }

        /// <summary>
        /// Gets or sets type of <see cref="WebBrowser"/> backend used for browsing.
        /// </summary>
        [Browsable(false)]
        public static WebBrowserBackend UseBackend
        {
            get => useBackend;
            set
            {
                useBackend = value;
                WebBrowser.SetBackend(useBackend);
            }
        }

        /// <summary>
        /// Gets dictionary for URL mappings which is used in <see cref="LoadUrl(string?)"/> method.
        /// By default mapping "g" => "https://www.google.com" is defined.
        /// It means when you call LoadUrl("g") or enter "g" in the URL bar, it will be translated to LoadUrl("https://www.google.com").
        /// This allows you to define short aliases for frequently used URLs.
        /// </summary>
        [Browsable(false)]
        public BaseDictionary<string, string> UrlMapping => urlMapping;

        /// <summary>
        /// Gets list box with list of actions. This control is created by demand, it will be shown
        /// at the right of the browser.
        /// </summary>
        [Browsable(false)]
        public ActionsListBox ActionsControl
        {
            get
            {
                if (actionsControl == null)
                {
                    actionsControl = new()
                    {
                        HasBorder = false,
                        Parent = panel.RightPanel,
                    };
                    panel.RightVisible = true;
                }

                return actionsControl;
            }
        }

        /// <summary>
        /// Gets main toolbar.
        /// </summary>
        [Browsable(false)]
        public ToolBar ToolBar => toolBar;

        /// <summary>
        /// Gets id of the 'Back' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdBack => buttonIdBack;

        /// <summary>
        /// Gets id of the 'More Actions' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdMoreActions => buttonIdMoreActions;

        /// <summary>
        /// Gets id of the 'Forward' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdForward => buttonIdForward;

        /// <summary>
        /// Gets id of the 'Zoom In' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdZoomIn => buttonIdZoomIn;

        /// <summary>
        /// Gets id of the 'Zoom Out' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdZoomOut => buttonIdZoomOut;

        /// <summary>
        /// Gets id of the 'Url' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdUrl => buttonIdUrl;

        /// <summary>
        /// Gets id of the 'Go' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdGo => buttonIdGo;

        /// <summary>
        /// Gets <see cref="ContextMenu"/> with additional actions like "Find" and "Print".
        /// </summary>
        public ContextMenu MoreActionsMenu => moreActionsMenu;

        /// <summary>
        /// Gets or sets whether to log <see cref="WebBrowser"/> events.
        /// </summary>
        public bool LogEvents { get => logEvents; set => logEvents = value; }

        /// <summary>
        /// Gets or sets whether navigation is allowed for the browser.
        /// </summary>
        public bool CanNavigate { get => canNavigate; set => canNavigate = value; }

        /// <summary>
        /// Gets <see cref="WebBrowser"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public WebBrowser WebBrowser
        {
            get
            {
                webBrowser ??= new WebBrowser(defaultUrl)
                {
                    HasBorder = false,
                    Parent = panel.FillPanel,
                };

                return webBrowser;
            }
        }

        /// <summary>
        /// Gets <see cref="TextBox"/> control used for url editing.
        /// </summary>
        [Browsable(false)]
        public TextBox UrlTextBox => urlTextBox;

        /// <summary>
        /// Gets whether IE backend is currently used.
        /// </summary>
        public bool IsIEBackend
        {
            get => WebBrowser.Backend == WebBrowserBackend.IE ||
                WebBrowser.Backend == WebBrowserBackend.IELatest;
        }

        /// <summary>
        /// Sets path for the Edge backend.
        /// </summary>
        /// <param name="folder">Name of the subfolder under application's path.</param>
        /// <remarks>
        /// You can place a fixed version of the WebView2 Edge runtime in the appropriate
        /// subfolder of this folder. In this case it will be used instead of the
        /// globally installed runtime. Subfolders:
        /// "win-arm64" - WebView2 runtime for a 64-bit ARM processor architecture.
        /// "win-x64" - WebView2 runtime for an Intel-based 64-bit processor architecture.
        /// "win-x86" - WebView2 runtime for an Intel-based 32-bit processor architecture.
        /// </remarks>
        public static void SetBackendPathSmart(string folder)
        {
            var os = Environment.OSVersion;
            var platform = os.Platform;

            if (platform != PlatformID.Win32NT)
                return;

            var pa = "win-" + RuntimeInformation.ProcessArchitecture.ToString().ToLower();
            string edgePath = Path.Combine(folder, pa);

            WebBrowser.SetBackendPath(edgePath, true);
        }

        /// <summary>
        /// Logs <see cref="WebBrowserEventArgs"/>.
        /// </summary>
        /// <param name="e">Event arguments to log.</param>
        public static void LogWebBrowserEvent(WebBrowserEventArgs e)
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
            App.Log(s);
        }

        /// <summary>
        /// Generates new <see cref="IntPtr"/> client data.
        /// </summary>
        public IntPtr GetNewClientData()
        {
            scriptRunCounter++;
            IntPtr clientData = new(scriptRunCounter);
            return clientData;
        }

        /// <summary>
        /// Updates enabled state of the "Zoom In" and "Zoom Out" buttons on the toolbar.
        /// </summary>
        public virtual void UpdateZoomButtons()
        {
            ToolBar.SetToolEnabled(buttonIdZoomIn, WebBrowser.CanZoomIn);
            ToolBar.SetToolEnabled(buttonIdZoomOut, WebBrowser.CanZoomOut);
        }

        /// <summary>
        /// Updates enabled state of the "Back" and "Forward" buttons on the toolbar.
        /// </summary>
        public virtual void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                WebBrowser.ClearHistory();
            }

            ToolBar.SetToolEnabled(buttonIdBack, WebBrowser.CanGoBack);
            ToolBar.SetToolEnabled(buttonIdForward, WebBrowser.CanGoForward);
        }

        /// <summary>
        /// Shows "Find" dialog window.
        /// </summary>
        public virtual void FindWithDialog()
        {
            if (searchWindow is null)
                Recreate();
            else
                if (searchWindow.TryGetTarget(out var target))
                {
                    if (target.IsDisposed)
                        Recreate();
                    else
                    {
                        target.ShowAndFocus();
                    }
                }
                else
                    Recreate();

            void Recreate()
            {
                WindowWebBrowserSearch win = new(FindParams);
                win.CloseAction = WindowCloseAction.Hide;
                win.TopMost = true;
                searchWindow = new(win);
                win.Show();
                win.Activate();
                win.Raise();
            }
        }

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
            buttonIdBack = toolBar.AddSpeedBtn(KnownButton.Back, OnBackButtonClick);
            buttonIdForward = toolBar.AddSpeedBtn(KnownButton.Forward, OnForwardButtonClick);
            buttonIdZoomIn = toolBar.AddSpeedBtn(KnownButton.ZoomIn, OnZoomInButtonClick);
            buttonIdZoomOut = toolBar.AddSpeedBtn(KnownButton.ZoomOut, OnZoomOutButtonClick);
            buttonIdUrl = toolBar.AddControl(urlTextBox);
            buttonIdGo = toolBar.AddSpeedBtn(KnownButton.BrowserGo, OnGoButtonClick);
            buttonIdMoreActions = toolBar.AddSpeedBtn(null, KnownSvgImages.ImgMoreActions);
            toolBar.SetToolAlignRight(buttonIdGo);
            toolBar.SetToolAlignRight(buttonIdMoreActions);

            MenuItem itemSearch = new(
                CommonStrings.Default.ButtonFind + "...",
                () => { FindWithDialog(); },
                new(Key.F, UI.ModifierKeys.Control));

            MenuItem itemPrint = new(
                CommonStrings.Default.ButtonPrint + "...",
                () => { WebBrowser.Print(); },
                new(Key.P, UI.ModifierKeys.Control));

            MoreActionsMenu.Add(itemSearch);
            MoreActionsMenu.Add(itemPrint);

            toolBar.SetToolDropDownMenu(buttonIdMoreActions, MoreActionsMenu);

            urlTextBox.KeyDown += OnTextBoxKeyDown;
        }

        /// <summary>
        /// Handles the event that occurs when the web browser has finished loading content.
        /// </summary>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains event data related to the web browser load event.</param>
        protected virtual void OnWebBrowserLoaded(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            UpdateHistoryButtons();
        }

        /// <summary>
        /// Handles the event that occurs when a new browser window is requested by the web browser control.
        /// </summary>
        /// <remarks>Override this method to customize how new window requests are handled in derived
        /// classes. This method is called when the web browser control signals that a new window should be opened, such
        /// as when a link with target='_blank' is activated.</remarks>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data, including the URL to be loaded in the new window.</param>
        protected virtual void OnWebBrowserNewWindow(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            LoadUrl(e.Url);
        }

        /// <summary>
        /// Handles the event that occurs when the web browser's document title changes.
        /// </summary>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data, including the new document title.</param>
        protected virtual void OnWebBrowserTitleChanged(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles the event that occurs before a web browser instance is created.
        /// </summary>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data.</param>
        protected virtual void OnWebBrowserBeforeBrowserCreate(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles the event that occurs when the web browser enters or exits full-screen mode.
        /// </summary>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains data related to the full-screen state change.</param>
        protected virtual void OnWebBrowserFullScreenChanged(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles the event that is raised when a script message is received from the web browser control.
        /// </summary>
        /// <remarks>Override this method to provide custom handling of script messages received from the
        /// web browser control in derived classes.</remarks>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data for the script message.</param>
        protected virtual void OnWebBrowserScriptMessageReceived(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles the result of a script execution in the web browser control.
        /// </summary>
        /// <remarks>Override this method to provide custom handling of script results from the web
        /// browser control. This method is called when a script execution completes and raises an event.</remarks>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data related to the script result.</param>
        protected virtual void OnWebBrowserScriptResult(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles errors that occur during web browser operations.
        /// </summary>
        /// <remarks>Override this method to provide custom error handling logic for web browser events in
        /// derived classes.</remarks>
        /// <param name="sender">The source of the event, typically the web browser control that encountered the error.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data related to the error.</param>
        protected virtual void OnWebBrowserError(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        /// <summary>
        /// Handles the event that occurs before the web browser navigates to a new document.
        /// </summary>
        /// <remarks>Override this method to perform custom processing or to cancel navigation before it
        /// occurs. Setting the Cancel property of the event arguments to <see langword="true"/> will prevent the
        /// navigation.</remarks>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains the event data, including navigation details and the ability to
        /// cancel navigation.</param>
        protected virtual void OnWebBrowserNavigating(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            if (!canNavigate)
                e.Cancel = true;
        }

        /// <summary>
        /// Handles the KeyDown event for the associated TextBox, triggering URL loading when the Enter key is pressed.
        /// </summary>
        /// <remarks>Override this method to customize the behavior when a key is pressed in the TextBox.
        /// The default implementation loads the URL when the Enter key is detected.</remarks>
        /// <param name="sender">The source of the event, typically the TextBox control.</param>
        /// <param name="e">A KeyEventArgs that contains the event data, including information about the pressed key.</param>
        protected virtual void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = urlTextBox.Text;

                LoadUrl(s);
            }
        }

        /// <summary>
        /// Handles the event when the Go button is clicked, initiating navigation to the URL specified in the input
        /// field.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Go button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected virtual void OnGoButtonClick(object? sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        /// <summary>
        /// Loads the specified URL or performs a search if the input is not a valid URL.
        /// </summary>
        /// <param name="s">The URL to load, or a search query if the string is not a valid URL. If the value is null, the default page
        /// is loaded.</param>
        protected virtual void LoadUrl(string? s)
        {
            if (s == null)
            {
                WebBrowser.LoadURL();
                return;
            }

            var hasMapping = UrlMapping.TryGetValue(s, out var mapped);

            if (hasMapping)
                s = mapped;

            WebBrowser.LoadUrlOrSearch(s);
        }

        /// <summary>
        /// Handles the event when the Zoom In button is clicked, increasing the zoom level of the web browser control.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Zoom In button.</param>
        /// <param name="e">An object that contains the event data.</param>
        protected virtual void OnZoomInButtonClick(object? sender, EventArgs e)
        {
            WebBrowser.ZoomIn();
            UpdateZoomButtons();
        }

        /// <summary>
        /// Handles the event when the Zoom Out button is clicked, decreasing the zoom level of the web browser control.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Zoom Out button.</param>
        /// <param name="e">An object that contains the event data.</param>
        protected virtual void OnZoomOutButtonClick(object? sender, EventArgs e)
        {
            WebBrowser.ZoomOut();
            UpdateZoomButtons();
        }

        /// <summary>
        /// Handles the event that occurs after the web browser has completed navigation to a new document.
        /// </summary>
        /// <param name="sender">The source of the event, typically the web browser control.</param>
        /// <param name="e">A WebBrowserEventArgs object that contains data related to the navigation event.</param>
        protected virtual void OnWebBrowserNavigated(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            urlTextBox.Text = WebBrowser.GetCurrentURL();
        }

        /// <summary>
        /// Handles the event when the Back button is clicked, navigating the web browser to the previous page if
        /// possible.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Back button control.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected virtual void OnBackButtonClick(object? sender, EventArgs e)
        {
            WebBrowser.GoBack();
        }

        /// <summary>
        /// Initializes the control and configures its layout, toolbar, panel, and web browser event handlers.
        /// </summary>
        /// <remarks>This method sets up the initial state of the control, including assigning parent
        /// relationships, layout styles, and subscribing to web browser events. Override this method in a derived class
        /// to customize initialization behavior. This method is typically called during control construction or setup
        /// and should not be called directly by user code.</remarks>
        protected virtual void Initialize()
        {
            UrlMapping.Add("g", "https://www.google.com");

            Layout = LayoutStyle.Vertical;
            toolBar.Parent = this;

            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = this;

            CreateToolbarItems();

            WebBrowser.ZoomType = WebBrowserZoomType.Layout;

            WebBrowser.Navigated += OnWebBrowserNavigated;
            WebBrowser.FullScreenChanged += OnWebBrowserFullScreenChanged;
            WebBrowser.ScriptMessageReceived += OnWebBrowserScriptMessageReceived;
            WebBrowser.ScriptResult += OnWebBrowserScriptResult;
            WebBrowser.Navigating += OnWebBrowserNavigating;
            WebBrowser.Error += OnWebBrowserError;
            WebBrowser.BeforeBrowserCreate += OnWebBrowserBeforeBrowserCreate;
            WebBrowser.Loaded += OnWebBrowserLoaded;
            WebBrowser.NewWindow += OnWebBrowserNewWindow;
            WebBrowser.DocumentTitleChanged += OnWebBrowserTitleChanged;
        }

        /// <summary>
        /// Handles the event when the Forward button is clicked, navigating the web browser to the next page in the
        /// history, if available.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Forward button control.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected virtual void OnForwardButtonClick(object? sender, EventArgs e)
        {
            WebBrowser.GoForward();
        }
    }
}
