using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window, ITestPageSite
    {
        private readonly StatusBar statusbar = new();
        private readonly ListBox pagesListBox = new();
        private readonly PanelChildSwitcher pageContainer = new();
        private readonly Grid grid;

        public MainTestWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;

            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Children.Add(grid);

            grid.Children.Add(pagesListBox);
            Grid.SetColumn(pagesListBox, 0);

            grid.Children.Add(pageContainer);
            Grid.SetColumn(pageContainer, 1);

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

            Add("Custom Draw Test", new CustomDrawTestPage { Site = this });

            SelectedIndex = 0;
            pagesListBox.SelectionChanged += PagesListBox_SelectionChanged;
        }

        public int? SelectedIndex
        {
            get => pagesListBox.SelectedIndex;
            set
            {
                pagesListBox.SelectedIndex = value;
                pageContainer.SetActivePage(pagesListBox.SelectedIndex);
            }
        }

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            pageContainer.SetActivePage(pagesListBox.SelectedIndex);
        }

        private void AddWebBrowserPage(string title)
        {
            Control Fn() => new WebBrowserTestPage { Site = this, PageName = title };

            Add(title, Fn);
        }

        private void Add(string title, Func<Control> fn)
        {
            pageContainer.Add(title, fn);
            pagesListBox.Add(title);
        }

        private void Add(string title, Control control)
        {
            pageContainer.Add(title, control);
            pagesListBox.Add(title);
        }
    }
}