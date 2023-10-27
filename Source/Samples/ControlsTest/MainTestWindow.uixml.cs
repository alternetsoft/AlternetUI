using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
        private readonly bool disableResize = false;
        private readonly PanelTreeAndCards mainPanel;

        static MainTestWindow()
        {
            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;
            Test.DoTests();
        }

        public MainTestWindow()
        {
            mainPanel = new(InitPanel);
            mainPanel.ActionsControl.Required();

            if (disableResize)
            {
                this.Resizable = false;
                this.MaximizeEnabled = false;
                this.MinimizeEnabled = false;
                mainPanel.RightPane.Resizable(false).DockFixed(true).Fixed();
                mainPanel.BottomPane.Resizable(false);
                mainPanel.CenterPane.DockFixed(true).Fixed();
                mainPanel.LeftPane.Resizable(false).DockFixed(true)
                    .MaxSize(mainPanel.DefaultRightPaneBestSize).Fixed();
            }

            Icon = ImageSet.FromUrlOrNull("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;
            mainPanel.Parent = this;
            mainPanel.BindApplicationLog();

            int CreateWebBrowserPages()
            {
                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.Edge;
                    return AddWebBrowserPage("Web Browser Edge");
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.WebKit;
                    return AddWebBrowserPage("Web Browser WebKit");
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest) &&
                    !WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.IELatest;
                    return AddWebBrowserPage("Web Browser IE");
                }

                return -1;
            }

            if (AddLinkLabelPage)
            {
                mainPanel.AddAction("Create LinkLabels", () =>
                {
                    mainPanel.Add("PanelLinkLabels Test", new PanelLinkLabelsPage());
                });
            }

            mainPanel.Add("Sizer Test", new SizerTestPage());
            mainPanel.Add("Custom Draw Test", new CustomDrawTestPage());
            int webBrowserPageIndex = CreateWebBrowserPages();

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;
            mainPanel.Manager.Update();

            if (!disableResize)
            {
                mainPanel.SizeChanged += Log_SizeChanged;
                mainPanel.CardPanel.SizeChanged += Log_SizeChanged;
                mainPanel.CardPanel.LayoutUpdated += Log_LayoutUpdated;
            }

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

        internal static bool AddLinkLabelPage { get; set; } = false;

        internal PanelTreeAndCards MainPanel => mainPanel;

        private void InitPanel(PanelAuiManager panel)
        {
            panel.LeftTreeViewAsListBox = true;
            panel.DefaultRightPaneMinSize = new(150, 150);
            panel.DefaultRightPaneBestSize = new(150, 150);
        }

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as Control;
            var name = control?.Name ?? control?.GetType().Name;
            Application.Log($"{evName}: {name}, Bounds: {control!.Bounds}");
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private void Log_LayoutUpdated(object? sender, EventArgs e)
        {
            // LogSizeEvent(sender, "LayoutUpdated");
        }

        private int AddWebBrowserPage(string title)
        {
            Control Fn()
            {
                var result = new WebBrowserTestPage
                {
                    PageName = title,
                };

                result.Name = result.GetType().Name;

                result.LayoutUpdated += Log_LayoutUpdated;
                result.SizeChanged += Log_SizeChanged;

                if (disableResize)
                {
                    result.PanelWebBrowser.RightPane.Resizable(false);
                }

                return result;
            }

            return mainPanel.Add(title, Fn);
        }
    }
}