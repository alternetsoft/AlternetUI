using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class WebBrowserPage : Control
    {
        private static string? headerText;

        private IPageSite? site;
        private bool historyCleared = false;
        private bool pandaLoaded = false;

        static WebBrowserPage()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            CommonUtils.ParseCmdLine(commandLineArgs);
            if (CommonUtils.CmdLineNoMfcDedug)
                WebBrowser.CrtSetDbgFlag(0);
        }

        public WebBrowserPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                headerText = HeaderLabel.Text;
                WebBrowser1.ZoomType = WebBrowserZoomType.Layout;
                site = value;
            }
        }

        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = string.Empty;
            WebBrowser1.FindClearResult();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            int findResult = WebBrowser1.Find(FindTextBox.Text);
            Log("Find Result = " + findResult.ToString());
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = UrlTextBox.Text;

                LoadUrl(s);
            }
        }

        private void Log(string s)
        {
            site?.LogEvent(s);
        }

        private void WebBrowser1_Navigated(object sender, WebBrowserEventArgs e)
        {
            UrlTextBox.Text = WebBrowser1.GetCurrentURL();
        }

        private string GetPandaFileName()
        {
            return CommonUtils.GetAppFolder() +
                "Html\\SampleArchive\\Html\\page1.html";
        }

        private string GetPandaUrl()
        {
            return CommonUtils.PrepareFileUrl(GetPandaFileName());
        }

        private void WebBrowser1_Loaded(object sender, WebBrowserEventArgs e)
        {
            UpdateHistoryButtons();
            if (!pandaLoaded)
            {
                pandaLoaded = true;
                try
                {
                    WebBrowser1.LoadURL(GetPandaUrl());
                }
                catch (Exception)
                {
                }
            }
        }

        private void WebBrowser1_NewWindow(object sender, WebBrowserEventArgs e)
        {
            WebBrowser1.LoadURL(e.Url);
        }

        private void WebBrowser1_TitleChanged(object sender, WebBrowserEventArgs e)
        {
            HeaderLabel.Text = headerText + " : " + e.Text;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }

        private void LoadUrl(string s)
        {
            if (s == "g")
                s = "https://www.google.com";

            if (!s.Contains("://"))
                s = "https://" + s;

            Log("==> LoadUrl: " + s);

            WebBrowser1.LoadURL(s);
        }

        private void UpdateZoomButtons()
        {
            ZoomInButton.Enabled = WebBrowser1.CanZoomIn;
            ZoomOutButton.Enabled = WebBrowser1.CanZoomOut;
        }

        private void UpdateHistoryButtons()
        {
            if (!historyCleared)
            {
                historyCleared = true;
                WebBrowser1.ClearHistory();
            }

            BackButton.Enabled = WebBrowser1.CanGoBack;
            ForwardButton.Enabled = WebBrowser1.CanGoForward;
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomIn();
            UpdateZoomButtons();
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.ZoomOut();
            UpdateZoomButtons();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoBack();
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            WebBrowser1.GoForward();
        }
    }
}
