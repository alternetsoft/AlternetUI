using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window, ITestPageSite
    {
        private readonly StatusBar statusbar = new();
        private readonly PageContainer pageContainer = new();

        public MainTestWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;

            this.Children.Add(pageContainer);

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                WebBrowserTestPage.UseBackend = WebBrowserBackend.Edge;
                AddWebBrowserPage("Web Browser Edge");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                WebBrowserTestPage.UseBackend = WebBrowserBackend.WebKit;
                AddWebBrowserPage("Web Browser WebKit");
                AddWebBrowserPage("Web Browser WebKit2");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest) &&
                !WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                WebBrowserTestPage.UseBackend = WebBrowserBackend.IELatest;
                AddWebBrowserPage("Web Browser IE");
                AddWebBrowserPage("Web Browser IE2");
            }

            pageContainer.Pages.Add(new PageContainer.Page(
                "Custom Draw Test",
                new CustomDrawTestPage { Site = this }));

            pageContainer.SelectedIndex = 0;
        }

        private void AddWebBrowserPage(string title)
        {
            pageContainer.Pages.Add(new PageContainer.Page(
                title,
                new WebBrowserTestPage { Site = this, PageName = title }));
        }
    }
}