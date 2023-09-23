﻿using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window, ITestPageSite
    {
        private readonly PageContainer pageContainer = new ();
        private int lastEventNumber = 1;

        public MainTestWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            mainGrid.Children.Add(pageContainer);
            Grid.SetRow(pageContainer, 0);

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

            Grid.SetRow(eventsListBox, 1);

            pageContainer.SelectedIndex = 0;
        }

        void ITestPageSite.LogEvent(string? pageId, string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {pageId}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        private void AddWebBrowserPage(string title)
        {
            pageContainer.Pages.Add(new PageContainer.Page(
                title,
                new WebBrowserTestPage { Site = this, PageName = title }));
        }
    }
}