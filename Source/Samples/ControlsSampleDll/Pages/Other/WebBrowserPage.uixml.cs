using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Alternet.UI;

namespace ControlsSample
{
    internal partial class WebBrowserPage : Panel
    {
        [IsTextLocalized(true)]
        private const string SItemPanda = "Panda Web Site";

        [IsTextLocalized(true)]
        private const string SItemGoogle = "Google Search";

        [IsTextLocalized(true)]
        private const string SItemPDF = "PDF Document";

        [IsTextLocalized(true)]
        private const string SItemImage = "View Image";

        [IsTextLocalized(true)]
        private const string SItemMP3 = "Audio MP3";

        [IsTextLocalized(true)]
        private const string SItemWAV = "Audio WAV";

        [IsTextLocalized(true)]
        private const string SItemGIF = "Animated GIF";

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

            BackButton.Image = KnownSvgImages.ImgArrowLeft.AsNormalImage(this);
            ForwardButton.Image = KnownSvgImages.ImgArrowRight.AsNormalImage(this);
            ZoomInButton.Image = KnownSvgImages.ImgZoomIn.AsNormalImage(this);
            ZoomOutButton.Image = KnownSvgImages.ImgZoomOut.AsNormalImage(this);
            GoButton.Image = KnownSvgImages.ImgTriangleArrowRight.AsNormalImage(this);

            webBrowser = new(GetPandaUrl())
            {
                Visible = false,
            };
            webBrowser.Navigated += WebBrowser1_Navigated;
            webBrowser.Loaded += WebBrowser1_Loaded;
            webBrowser.NewWindow += WebBrowser1_NewWindow;
            webBrowser.DocumentTitleChanged += WebBrowser1_TitleChanged;

            mainPanel.Children.Add(webBrowser);

            headerText = HeaderLabel.Text;
            webBrowser.ZoomType = WebBrowserZoomType.Layout;

            List<string> items = new()
            {
                SItemPanda,
                SItemGoogle,
                SItemImage,
                SItemMP3,
                SItemWAV,
                SItemGIF
            };

            if (webBrowser.Backend != WebBrowserBackend.IE &&
                webBrowser.Backend != WebBrowserBackend.IELatest)
                items.Add(SItemPDF);

            openButton.Image = KnownSvgImages.ImgFileOpen.AsNormalImage(this);
            openButton.AddRange(items);
            openButton.ValueChanged += OpenButton_ValueChanged;
            openButton.ImageVisible = true;
            openButton.TextVisible = false;

            HeaderLabel.WordWrap = true;

            PerformLayout();
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
            App.Log(s);
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
            if (!pandaLoaded && App.IsWindowsOS)
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
            if (e.Text == null)
            {
                HeaderLabel.Text = headerText ?? string.Empty;
                return;
            }

            var title = e.Text.Replace(@"/", @" / ");
            title = title.Replace(@"\", @" \ ");

            HeaderLabel.Text = headerText + " : " + title;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }

        private void OpenButton_ValueChanged(object? sender, EventArgs e)
        {
            var v = openButton.Value?.ToString();

            if (v != null)
            {
                UrlTextBox.Text = v;
                LoadUrl(v);
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

            if (s == SItemMP3)
            {
                s = WebBrowser.PrepareFileUrl(PathUtils.GetAppFolder() +
                    "Resources/Sounds/Mp3/sounds.html");
            }

            if (s == SItemWAV)
            {
                s = WebBrowser.PrepareFileUrl(PathUtils.GetAppFolder() +
                    "Resources/Sounds/Wav/sounds.html");
            }

            if (s == SItemGIF)
            {
                s = WebBrowser.PrepareFileUrl(PathUtils.GetAppFolder() +
                    "Resources/Animation/animation.html");
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
