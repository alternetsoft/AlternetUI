using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class WebBrowserPage : Control
    {
        private const string SItemPanda = "Panda Web Site";
        private const string SItemGoogle = "Google Search";
        private const string SItemPDF = "PDF Document";
        private const string SItemImage = "View Image";

        private static string? headerText;

        private readonly WebBrowser webBrowser;
        private bool historyCleared = false;
        private bool pandaLoaded = false;

        static WebBrowserPage()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            ControlsSampleUtils.ParseCmdLine(commandLineArgs);
            if (ControlsSampleUtils.CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);
        }

        public WebBrowserPage()
        {

            InitializeComponent();

            var width = BackButton.Height * 1.5;

            BackButton.SuggestedWidth = width;
            ForwardButton.SuggestedWidth = width;
            ZoomInButton.SuggestedWidth = width;
            ZoomOutButton.SuggestedWidth = width;
            GoButton.SuggestedWidth = width;

            webBrowser = new(GetPandaUrl())
            {
                Visible = false,
            };
            webBrowser.Navigated += WebBrowser1_Navigated;
            webBrowser.Loaded += WebBrowser1_Loaded;
            webBrowser.NewWindow += WebBrowser1_NewWindow;
            webBrowser.DocumentTitleChanged += WebBrowser1_TitleChanged;
            Grid.SetRowColumn(webBrowser, 2, 0);
            mainGrid.Children.Add(webBrowser);

            headerText = HeaderLabel.Text;
            webBrowser.ZoomType = WebBrowserZoomType.Layout;
            UrlTextBox.Items.Add(SItemPanda);
            UrlTextBox.Items.Add(SItemGoogle);

            if (webBrowser.Backend != WebBrowserBackend.IE &&
                webBrowser.Backend != WebBrowserBackend.IELatest)
                UrlTextBox.Items.Add(SItemPDF);
            UrlTextBox.Items.Add(SItemImage);
        }

        private static string GetPandaFileName()
        {
            var s = PathUtils.GetAppFolder() +
                "Html/SampleArchive/Html/page1.html";
            return s;
        }

        private static string GetPandaUrl()
        {
            var s = WebBrowser.PrepareFileUrl(GetPandaFileName());
            return s;
        }

        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = string.Empty;
            webBrowser.FindClearResult();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            int findResult = webBrowser.Find(FindTextBox.Text);
            Log("Find Result = " + findResult.ToString());
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = UrlTextBox.Text;

                LoadUrl(s);
            }
        }

        private void Log(string s)
        {
            Application.Log(s);
        }

        private void WebBrowser1_Navigated(object? sender, WebBrowserEventArgs e)
        {
            UrlTextBox.Text = webBrowser.GetCurrentURL();
        }

        private void WebBrowser1_Loaded(object? sender, WebBrowserEventArgs e)
        {
            UpdateHistoryButtons();

            // Under Windows 'Black' scheme for other controls is not implemented
            // so we turn on Light scheme in browser.
            if (!pandaLoaded && Application.IsWindowsOS)
            {
                webBrowser.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;
            }

            pandaLoaded = true;
            webBrowser.Visible = true;
        }

        private void WebBrowser1_NewWindow(object? sender, WebBrowserEventArgs e)
        {
            webBrowser.LoadURL(e.Url);
        }

        private void WebBrowser1_TitleChanged(object? sender, WebBrowserEventArgs e)
        {
            HeaderLabel.Text = headerText + " : " + e.Text;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }

        private void UrlTextBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (UrlTextBox.SelectedIndex != null)
            {
                LoadUrl(UrlTextBox.Text);
            }
        }

        private void LoadUrl(string s)
        {
            if (s == null)
            {
                webBrowser.LoadURL();
                return;
            }

            if (s == "s")
            {
                WebBrowser.DoCommandGlobal("Screenshot", this);
                return;
            }

            if (s == SItemPanda)
                s = GetPandaUrl();

            if (s == SItemImage)
            {
                s = WebBrowser.PrepareFileUrl(PathUtils.GetAppFolder() +
                "Html/SampleArchive/Images/panda1.jpg");
            }

            if (s == SItemPDF)
            {
                s = WebBrowser.PrepareFileUrl(PathUtils.GetAppFolder() +
                    "Resources/SamplePandaPdf.pdf");
            }

            if (s == "g" || s == "SItemGoogle")
                s = "https://www.google.com";

            Log("==> LoadUrl: " + s);

            webBrowser.LoadUrlOrSearch(s);
        }

        private void UpdateZoomButtons()
        {
            ZoomInButton.Enabled = webBrowser.CanZoomIn;
            ZoomOutButton.Enabled = webBrowser.CanZoomOut;
        }

        private void UpdateHistoryButtons()
        {
            if (webBrowser == null || BackButton == null || ForwardButton == null)
                return;

            if (!historyCleared)
            {
                historyCleared = true;
                webBrowser.ClearHistory();
            }

            BackButton.Enabled = webBrowser.CanGoBack;
            ForwardButton.Enabled = webBrowser.CanGoForward;
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            webBrowser.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            webBrowser.ZoomOut();
            UpdateZoomButtons();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            webBrowser.GoForward();
        }
    }
}
