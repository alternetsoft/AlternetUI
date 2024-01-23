using Alternet.UI;
using System;
using System.Linq;

namespace Alternet.UI.Documentation.Examples.WebBrowser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void WebBrowserExample1()
        {
            #region WebBrowserCSharpCreation
            var wb = new Alternet.UI.WebBrowser();
            wb.LoadURL("https://www.google.com");
            #endregion
        }

        private static string? headerText;    

        private bool historyCleared = false;
        private bool pandaLoaded = false;
        private void FindClearButton_Click(object sender, EventArgs e)
        {
            FindTextBox.Text = string.Empty;
            WebBrowser1.FindClearResult();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            var prm = new WebBrowserFindParams();
            prm.HighlightResult = true;

            int findResult = WebBrowser1.Find(FindTextBox.Text, prm);
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var s = UrlTextBox.Text;

                LoadUrl(s);
            }
        }

        #region WebBrowserEventHandler
        private void WebBrowser1_Navigated(object sender, WebBrowserEventArgs e)
        {
            UrlTextBox.Text = WebBrowser1.GetCurrentURL();
        }

        private void WebBrowser1_Loaded(object sender, WebBrowserEventArgs e)
        {
            UpdateHistoryButtons();
            if (!pandaLoaded)
            {
                pandaLoaded = true;
                WebBrowser1.PreferredColorScheme = WebBrowserPreferredColorScheme.Light;

                WebBrowser1.LoadURL("https://www.google.com/");
            }
        }

        private void WebBrowser1_NewWindow(object sender, WebBrowserEventArgs e)
        {
            WebBrowser1.LoadURL(e.Url);
        }

        private void WebBrowser1_TitleChanged(object sender, WebBrowserEventArgs e)
        {
            HeaderLabel.Text = "Web Browser Control: " + e.Text;
        }
        #endregion    

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadUrl(UrlTextBox.Text);
        }

        private void LoadUrl(string s)
        {
            if (s == null)
            {
                WebBrowser1.LoadURL();
                return;
            }

            if (s == "g")
                s = "https://www.google.com";

            WebBrowser1.LoadUrlOrSearch(s);
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