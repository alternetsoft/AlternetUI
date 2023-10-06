using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
        private readonly PanelTreeAndCards mainPanel = new();

        static MainTestWindow()
        {
            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;
        }

        public MainTestWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;
            mainPanel.Parent = this;
            mainPanel.BindApplicationLog();

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
                mainPanel.Add("Custom Draw Test", new CustomDrawTestPage());

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;
            mainPanel.Manager.Update();
        }

        internal static bool AddCustomDrawPage { get; set; } = true;

        private void AddWebBrowserPage(string title)
        {
            Control Fn() => new WebBrowserTestPage { PageName = title };
            mainPanel.Add(title, Fn);
        }
    }
}