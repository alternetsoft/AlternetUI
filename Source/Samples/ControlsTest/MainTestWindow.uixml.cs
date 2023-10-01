using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window, ITestPageSite
    {
        private readonly StatusBar statusbar = new();
        private readonly PanelChildSwitcher pageContainer = new();
        private readonly PanelAuiManager rootPanel = new();

        static MainTestWindow()
        {
            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;
        }

        public MainTestWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;

            rootPanel.LeftTreeView.Required();

            Children.Add(rootPanel);

            rootPanel.Manager.AddPane(pageContainer, rootPanel.CenterPane);

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.Edge;
                AddWebBrowserPage("Web Browser Edge");
                AddWebBrowserPage("Web Browser Edge2");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.WebKit;
                AddWebBrowserPage("Web Browser WebKit");
                AddWebBrowserPage("Web Browser WebKit2");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest) &&
                !WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.IELatest;
                AddWebBrowserPage("Web Browser IE");
                AddWebBrowserPage("Web Browser IE2");
            }

            if(AddCustomDrawPage)
                Add("Custom Draw Test", new CustomDrawTestPage { Site = this });

            rootPanel.LeftTreeView.SelectionChanged += PagesListBox_SelectionChanged;

            rootPanel.Manager.Update();

            rootPanel.LeftTreeView.SelectedItem = rootPanel.LeftTreeView.FirstItem;
        }

        internal static bool AddCustomDrawPage { get; set; } = false;

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            var tag = rootPanel.LeftTreeView.SelectedItem?.Tag;
            pageContainer.SetActivePage(tag as int?);
        }

        private void AddWebBrowserPage(string title)
        {
            Control Fn() => new WebBrowserTestPage { Site = this, PageName = title };

            Add(title, Fn);
        }

        private void Add(string title, Func<Control> fn)
        {
            var index = pageContainer.Add(title, fn);
            var item = rootPanel.LeftTreeView.Add(title);
            item.Tag = index;
        }

        private void Add(string title, Control control)
        {
            var index = pageContainer.Add(title, control);
            var item = rootPanel.LeftTreeView.Add(title);
            item.Tag = index;
        }
    }
}