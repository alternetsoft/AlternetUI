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

            if(AddLinkLabelPage)
                mainPanel.Add("PanelLinkLabels Test", new PanelLinkLabelsPage());

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;
            mainPanel.Manager.Update();

            this.LayoutUpdated += Log_LayoutUpdated;
            this.SizeChanged += Log_SizeChanged;

            mainPanel.LayoutUpdated += Log_LayoutUpdated;
            mainPanel.SizeChanged += Log_SizeChanged;

            mainPanel.CardPanel.LayoutUpdated += Log_LayoutUpdated;
            mainPanel.CardPanel.SizeChanged += Log_SizeChanged;

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as Control;
            var name = control?.Name;
            Application.Log($"{evName}: {name}, Bounds: {control!.Bounds}");

            Application.Log(mainPanel.CardPanel.Bounds);
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private void Log_LayoutUpdated(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "LayoutUpdated");
        }

        internal static bool AddCustomDrawPage { get; set; } = true;

        internal static bool AddLinkLabelPage { get; set; } = false;

        private void AddWebBrowserPage(string title)
        {
            Control Fn() => new WebBrowserTestPage { PageName = title };
            mainPanel.Add(title, Fn);
        }
    }
}