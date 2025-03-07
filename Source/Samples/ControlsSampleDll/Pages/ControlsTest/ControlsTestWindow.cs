using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ControlsTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
#pragma warning disable
        private readonly PanelListBoxAndCards mainPanel;
#pragma warning restore

        static ControlsTestWindow()
        {
        }

        public ControlsTestWindow()
        {
            Title = "Alternet UI Controls Test";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            mainPanel = new();
            mainPanel.RightPanel.MinWidth = 150;

            Icon = new("embres:ControlsSampleDll.Sample.ico");

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

            CreateWebBrowserPages();

            mainPanel.LeftListBox.SelectedIndex = 0;

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

#pragma warning disable
        internal PanelListBoxAndCards MainPanel => mainPanel;
#pragma warning restore

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as AbstractControl;
            var name = control?.Name ?? control?.GetType().Name;
            App.LogIf($"{evName}: {name}, Bounds: {control!.Bounds}", false);
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private int AddPage<T>(string? title = null)
            where T : AbstractControl, new()
        {
            return AddPage(title ?? typeof(T).ToString(), () => new T());
        }

        private int AddPage(string title, Func<AbstractControl> createFn)
        {
            return mainPanel.Add(title, createFn);
        }

        private int AddWebBrowserPage(string title, WebBrowserBackend backend)
        {
            AbstractControl Fn()
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

            return AddPage(title, Fn);
        }
    }
}