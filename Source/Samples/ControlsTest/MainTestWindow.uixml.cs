using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
        private readonly PanelTreeAndCards mainPanel = new();
        private CardPanelItem? firstCard;

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

            int firstPageIndex = -1;

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.Edge;
                firstPageIndex = AddWebBrowserPage("Web Browser Edge");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.WebKit;
                firstPageIndex = AddWebBrowserPage("Web Browser WebKit");
                AddWebBrowserPage("Web Browser WebKit2");
            }

            if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest) &&
                !WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.IELatest;
                firstPageIndex = AddWebBrowserPage("Web Browser IE");
                AddWebBrowserPage("Web Browser IE2");
            }

            if(AddCustomDrawPage)
                mainPanel.Add("Custom Draw Test", new CustomDrawTestPage());

            if(AddLinkLabelPage)
                mainPanel.Add("PanelLinkLabels Test", new PanelLinkLabelsPage());

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;
            mainPanel.Manager.Update();

            mainPanel.SizeChanged += Log_SizeChanged;
            mainPanel.CardPanel.SizeChanged += Log_SizeChanged;
            mainPanel.CardPanel.LayoutUpdated += Log_LayoutUpdated;

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";

            if (firstPageIndex >= 0)
            {
                firstCard = mainPanel.CardPanel.Cards[firstPageIndex];
                firstCard.Control.Name = firstCard.Control.GetType().Name;
                firstCard.Control.LayoutUpdated += Log_LayoutUpdated;
                firstCard.Control.SizeChanged += Log_SizeChanged;
            }
        }

        internal static bool AddCustomDrawPage { get; set; } = true;

        internal static bool AddLinkLabelPage { get; set; } = false;

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as Control;
            var name = control?.Name;
            Application.Log($"{evName}: {name}, Bounds: {control!.Bounds}");
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private void Log_LayoutUpdated(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "LayoutUpdated");
        }

        private int AddWebBrowserPage(string title)
        {
            Control Fn() => new WebBrowserTestPage { PageName = title };
            return mainPanel.Add(title, Fn);
        }
    }
}