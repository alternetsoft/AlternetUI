using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
        private readonly PanelTreeAndCards mainPanel;

        static MainTestWindow()
        {
            Test.DoTests();
        }

        public MainTestWindow()
        {
            mainPanel = new();
            mainPanel.LeftTreeViewAsListBox = true;
            mainPanel.RightPanel.MinWidth = 150;

            Icon = new("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;
            mainPanel.Parent = this;
            mainPanel.BindApplicationLog();

            int CreateWebBrowserPages()
            {
                int result = -1;

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                {
                    result = AddWebBrowserPage("Browser Edge", WebBrowserBackend.Edge);
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
                {
                    result = AddWebBrowserPage("Browser WebKit", WebBrowserBackend.WebKit);
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest))
                {
                    result = AddWebBrowserPage("Browser IE", WebBrowserBackend.IELatest);
                }

                return result;
            }

            mainPanel.Add("Custom Draw Test", new CustomDrawTestPage());
            CreateWebBrowserPages();

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

        internal PanelTreeAndCards MainPanel => mainPanel;

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as Control;
            var name = control?.Name ?? control?.GetType().Name;
            Application.LogIf($"{evName}: {name}, Bounds: {control!.Bounds}", false);
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private int AddWebBrowserPage(string title, WebBrowserBackend backend)
        {
            Control Fn()
            {
                PanelWebBrowser.UseBackend = backend;

                var result = new WebBrowserTestPage
                {
                    PageName = title,
                };

                result.Name = result.GetType().Name;

                result.SizeChanged += Log_SizeChanged;

                return result;
            }

            return mainPanel.Add(title, Fn);
        }
    }
}