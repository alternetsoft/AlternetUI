using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    public class PanelWebBrowser : PanelAuiManager
    {
        private static WebBrowserBackend useBackend = WebBrowserBackend.Default;
        private readonly WebBrowserFindParams findParams = new();

        private readonly TextBox urlTextBox = new()
        {
        };

        private readonly WebBrowser webBrowser = new()
        {
            HasBorder = false,
        };

        private readonly CheckBox findWrapCheckBox = new()
        {
            Text = "Wrap",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findEntireWordCheckBox = new()
        {
            Text = "Entire Word",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findMatchCaseCheckBox = new()
        {
            Text = "Match Case",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findHighlightResultCheckBox = new()
        {
            Text = "Highlight",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly CheckBox findBackwardsCheckBox = new()
        {
            Text = "Backwards",
            Margin = new Thickness(0, 5, 5, 5),
        };

        private readonly StackPanel findPanel = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            Orientation = StackPanelOrientation.Vertical,
        };

        private readonly TextBox findTextBox = new()
        {
            SuggestedWidth = 300,
            Margin = new Thickness(0, 10, 5, 5),
            Text = "panda",
        };

        private readonly Button findButton = new()
        {
            Text = "Find",
            Margin = new Thickness(0, 10, 5, 5),
        };

        private readonly Button findClearButton = new()
        {
            Text = "Find Clear",
            Margin = new Thickness(0, 10, 5, 5),
        };

        private bool historyCleared = false;
        private int buttonIdBack;
        private int buttonIdForward;
        private int buttonIdZoomIn;
        private int buttonIdZoomOut;
        private int buttonIdUrl;
        private int buttonIdGo;
        private bool logEvents;
        private bool canNavigate = true;
        private int scriptRunCounter = 0;

        public PanelWebBrowser()
        {
            DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);
            Toolbar.Required();
            CreateToolbarItems();

            WebBrowser.ZoomType = WebBrowserZoomType.Layout;

            findPanel.Children.Add(findTextBox);

            findButton.Click += FindButton_Click;
            findPanel.Children.Add(findButton);

            findClearButton.Click += FindClearButton_Click;
            findPanel.Children.Add(findClearButton);

            findPanel.Children.Add(findWrapCheckBox);
            findPanel.Children.Add(findEntireWordCheckBox);
            findPanel.Children.Add(findMatchCaseCheckBox);
            findPanel.Children.Add(findHighlightResultCheckBox);
            findPanel.Children.Add(findBackwardsCheckBox);

            FindParamsToControls();

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

        public static WebBrowserBackend UseBackend
        {
            get => useBackend;
            set
            {
                useBackend = value;
                WebBrowser.SetBackend(useBackend);
            }
        }

        public bool LogEvents { get => logEvents; set => logEvents = value; }

        public bool CanNavigate { get => canNavigate; set => canNavigate = value; }

        public WebBrowser WebBrowser => webBrowser;

        public TextBox UrlTextBox => urlTextBox;

        public bool IsIEBackend
        {
            get => WebBrowser.Backend == WebBrowserBackend.IE ||
                WebBrowser.Backend == WebBrowserBackend.IELatest;
        }

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

        public IntPtr GetNewClientData()
        {
            scriptRunCounter++;
            IntPtr clientData = new(scriptRunCounter);
            return clientData;
        }

        public void UpdateZoomButtons()
        {
            Toolbar.EnableTool(buttonIdZoomIn, webBrowser.CanZoomIn);
            Toolbar.EnableTool(buttonIdZoomOut, webBrowser.CanZoomOut);
            Toolbar.Refresh();
        }

        public void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                webBrowser.ClearHistory();
            }

            Toolbar.EnableTool(buttonIdBack, webBrowser.CanGoBack);
            Toolbar.EnableTool(buttonIdForward, webBrowser.CanGoForward);
            Toolbar.Refresh();
        }

        public void CreateToolbarItems()
        {
            var toolbar = Toolbar;
            var imageSize = GetToolBitmapSize();

            var images = KnownSvgImages.GetForSize(imageSize);

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

            toolbar.AddToolOnClick(buttonIdBack, BackButton_Click);
            toolbar.AddToolOnClick(buttonIdForward, ForwardBtn_Click);
            toolbar.AddToolOnClick(buttonIdZoomIn, ZoomInButton_Click);
            toolbar.AddToolOnClick(buttonIdZoomOut, ZoomOutButton_Click);
            toolbar.AddToolOnClick(buttonIdGo, GoButton_Click);

            urlTextBox.KeyDown += TextBox_KeyDown;

            toolbar.GrowToolMinWidth(buttonIdUrl, 350);

            toolbar.Realize();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || Flags.HasFlag(ControlFlags.ParentAssigned))
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

        private void FindButton_Click(object? sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = webBrowser.Find(findTextBox.Text, findParams);
            Application.DebugLog("Find Result = " + findResult.ToString());
        }

        private void FindParamsToControls()
        {
            findWrapCheckBox.IsChecked = findParams.Wrap;
            findEntireWordCheckBox.IsChecked = findParams.EntireWord;
            findMatchCaseCheckBox.IsChecked = findParams.MatchCase;
            findHighlightResultCheckBox.IsChecked = findParams.HighlightResult;
            findBackwardsCheckBox.IsChecked = findParams.Backwards;
        }

        private void FindParamsFromControls()
        {
            findParams.Wrap = findWrapCheckBox.IsChecked;
            findParams.EntireWord = findEntireWordCheckBox.IsChecked;
            findParams.MatchCase = findMatchCaseCheckBox.IsChecked;
            findParams.HighlightResult = findHighlightResultCheckBox.IsChecked;
            findParams.Backwards = findBackwardsCheckBox.IsChecked;
        }

        private void FindClearButton_Click(object? sender, EventArgs e)
        {
            findTextBox.Text = string.Empty;
            webBrowser.FindClearResult();
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
            webBrowser.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object? sender, EventArgs e)
        {
            webBrowser.ZoomOut();
            UpdateZoomButtons();
        }

        private void WebBrowser_Navigated(object? sender, WebBrowserEventArgs e)
        {
            if (LogEvents)
                LogWebBrowserEvent(e);
            urlTextBox.Text = webBrowser.GetCurrentURL();
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void ForwardBtn_Click(object? sender, EventArgs e)
        {
            webBrowser.GoForward();
        }
    }
}
