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
            /*mainPanel.BackgroundColor = Color.Beige;*/

            /*this.SetSizer(SizerFactory.Default.CreateBoxSizer(true));*/

            void CreateWebBrowserPages()
            {
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
            CreateWebBrowserPages();

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

            var firstCard = mainPanel.CardPanel.Cards[0];

            if (firstCard is not null)
            {
                firstCard.Control.Name = firstCard.Control.GetType().Name;

                if (!disableResize)
                {
                    firstCard.Control.LayoutUpdated += Log_LayoutUpdated;
                    firstCard.Control.SizeChanged += Log_SizeChanged;
                }
            }
        }

        internal PanelTreeAndCards MainPanel => mainPanel;

        internal static bool AddLinkLabelPage { get; set; } = false;

        private void InitPanel(PanelAuiManager panel)
        {
            panel.LeftTreeViewAsListBox = true;
            panel.DefaultRightPaneMinSize = new(150, 150);
            panel.DefaultRightPaneBestSize = new(150, 150);
        }

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
            Control Fn()
            {
                var result = new WebBrowserTestPage
                {
                    PageName = title,
                };

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