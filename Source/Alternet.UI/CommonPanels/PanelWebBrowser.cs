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
    /// Implements panel with <see cref="WebBrowser"/> and <see cref="AuiToolbar"/> with
    /// web navigation buttons.
    /// </summary>
    public class PanelWebBrowser : PanelAuiManager
    {
        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;
        private readonly WebBrowserFindParams findParams = new();

        private readonly TextBox urlTextBox = new()
        {
        };

        private readonly ContextMenu moreActionsMenu = new();
        private readonly string defaultUrl = "about:blank";
        private WebBrowser? webBrowser;
        private bool historyCleared = false;
        private int buttonIdBack;
        private int buttonIdMoreActions;
        private int buttonIdForward;
        private int buttonIdZoomIn;
        private int buttonIdZoomOut;
        private int buttonIdUrl;
        private int buttonIdGo;
        private bool logEvents;
        private bool canNavigate = true;
        private int scriptRunCounter = 0;

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
        /// Gets id of the 'Back' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdBack => buttonIdBack;

        /// <summary>
        /// Gets id of the 'More Actions' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdMoreActions => buttonIdMoreActions;

        /// <summary>
        /// Gets id of the 'Forward' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdForward => buttonIdForward;

        /// <summary>
        /// Gets id of the 'Zoom In' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdZoomIn => buttonIdZoomIn;

        /// <summary>
        /// Gets id of the 'Zoom Out' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdZoomOut => buttonIdZoomOut;

        /// <summary>
        /// Gets id of the 'Url' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdUrl => buttonIdUrl;

        /// <summary>
        /// Gets id of the 'Go' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdGo => buttonIdGo;

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
                webBrowser ??= new WebBrowser(defaultUrl);
                return webBrowser;
            }
        }

        /// <summary>
        /// Gets <see cref="TextBox"/> control used for url editing.
        /// </summary>
        public TextBox UrlTextBox => urlTextBox;

        /// <summary>
        /// Gets whetner IE backend is currently used.
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
            Application.Log(s);
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
        public void UpdateZoomButtons()
        {
            Toolbar.EnableTool(buttonIdZoomIn, WebBrowser.CanZoomIn);
            Toolbar.EnableTool(buttonIdZoomOut, WebBrowser.CanZoomOut);
            Toolbar.Refresh();
        }

        /// <summary>
        /// Updates enabled state of the "Back" and "Forward" buttons on the toolbar.
        /// </summary>
        public void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                WebBrowser.ClearHistory();
            }

            Toolbar.EnableTool(buttonIdBack, WebBrowser.CanGoBack);
            Toolbar.EnableTool(buttonIdForward, WebBrowser.CanGoForward);
            Toolbar.Refresh();
        }

        /// <summary>
        /// Shows "Find" dialog window.
        /// </summary>
        public void FindWithDialog()
        {
            WebBrowserSearchWindow win = new(WebBrowser, findParams);
            win.ShowModal(this.ParentWindow);
        }

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
            var toolbar = Toolbar;
            var imageSize = GetBaseToolSvgSize();
            toolbar.ToolBitmapSizeInPixels = imageSize;

            var images = KnownSvgImages.GetForSize(
                toolbar.GetSvgColor(KnownSvgColor.Normal),
                imageSize);

            buttonIdBack = toolbar.AddTool(
                CommonStrings.Default.ButtonBack,
                images.ImgBrowserBack,
                CommonStrings.Default.ButtonBack);

            buttonIdForward = toolbar.AddTool(
                CommonStrings.Default.ButtonForward,
                images.ImgBrowserForward,
                CommonStrings.Default.ButtonForward);

            buttonIdZoomIn = toolbar.AddTool(
                CommonStrings.Default.ButtonZoomIn,
                images.ImgZoomIn,
                CommonStrings.Default.ButtonZoomIn);

            buttonIdZoomOut = toolbar.AddTool(
                CommonStrings.Default.ButtonZoomOut,
                images.ImgZoomOut,
                CommonStrings.Default.ButtonZoomOut);

            buttonIdUrl = toolbar.AddControl(urlTextBox);

            buttonIdGo = toolbar.AddTool(
                CommonStrings.Default.ButtonGo,
                images.ImgBrowserGo,
                CommonStrings.Default.ButtonGo);

            buttonIdMoreActions = toolbar.AddTool(
                null,
                images.ImgMoreActions,
                null);

            MenuItem itemSearch = new(
                CommonStrings.Default.ButtonFind + "...",
                () => { FindWithDialog(); }, new(Key.F, UI.ModifierKeys.Control));

            MenuItem itemPrint = new(
                CommonStrings.Default.ButtonPrint + "...",
                () => { WebBrowser.Print(); }, new(Key.P, UI.ModifierKeys.Control));

            MoreActionsMenu.Add(itemSearch);
            MoreActionsMenu.Add(itemPrint);

            toolbar.AddToolOnClick(buttonIdBack, BackButton_Click);
            toolbar.AddToolOnClick(buttonIdForward, ForwardBtn_Click);
            toolbar.AddToolOnClick(buttonIdZoomIn, ZoomInButton_Click);
            toolbar.AddToolOnClick(buttonIdZoomOut, ZoomOutButton_Click);
            toolbar.AddToolOnClick(buttonIdGo, GoButton_Click);
            toolbar.SetToolDropDownMenu(buttonIdMoreActions, MoreActionsMenu);
            toolbar.SetToolDropDownOnEvent(buttonIdMoreActions, AuiToolbarItemDropDownOnEvent.Click);

            urlTextBox.KeyDown += TextBox_KeyDown;

            toolbar.GrowToolMinWidth(buttonIdUrl, 350);

            toolbar.Realize();
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || StateFlags.HasFlag(ControlFlags.ParentAssigned))
                return;

            Manager.AddPane(WebBrowser, CenterPane);
            Manager.AddPane(Toolbar, ToolbarPane);

            Manager.Update();
        }

        private void WebBrowser_Loaded(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            UpdateHistoryButtons();
        }

        private void WebBrowser_NewWindow(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            LoadUrl(e.Url);
        }

        private void WebBrowser_TitleChanged(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_BeforeBrowserCreate(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_FullScreenChanged(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_ScriptMessageReceived(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_ScriptResult(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_Error(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
        }

        private void WebBrowser_Navigating(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            if (!canNavigate)
                e.Cancel = true;
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = urlTextBox.Text;

                LoadUrl(s);
            }
        }

        private void GoButton_Click(object? sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string? s)
        {
            if (s == null)
            {
                WebBrowser.LoadURL();
                return;
            }

            if (s == "g")
                s = "https://www.google.com";

            WebBrowser.LoadUrlOrSearch(s);
        }

        private void ZoomInButton_Click(object? sender, EventArgs e)
        {
            WebBrowser.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object? sender, EventArgs e)
        {
            WebBrowser.ZoomOut();
            UpdateZoomButtons();
        }

        private void WebBrowser_Navigated(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            urlTextBox.Text = WebBrowser.GetCurrentURL();
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            WebBrowser.GoBack();
        }

        private void Initialize()
        {
            WebBrowser.HasBorder = false;
            DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);
            Toolbar.Required();
            CreateToolbarItems();

            WebBrowser.ZoomType = WebBrowserZoomType.Layout;

            WebBrowser.Navigated += WebBrowser_Navigated;
            WebBrowser.FullScreenChanged += WebBrowser_FullScreenChanged;
            WebBrowser.ScriptMessageReceived += WebBrowser_ScriptMessageReceived;
            WebBrowser.ScriptResult += WebBrowser_ScriptResult;
            WebBrowser.Navigating += WebBrowser_Navigating;
            WebBrowser.Error += WebBrowser_Error;
            WebBrowser.BeforeBrowserCreate += WebBrowser_BeforeBrowserCreate;
            WebBrowser.Loaded += WebBrowser_Loaded;
            WebBrowser.NewWindow += WebBrowser_NewWindow;
            WebBrowser.DocumentTitleChanged += WebBrowser_TitleChanged;
        }

        private void ForwardBtn_Click(object? sender, EventArgs e)
        {
            WebBrowser.GoForward();
        }
    }
}
