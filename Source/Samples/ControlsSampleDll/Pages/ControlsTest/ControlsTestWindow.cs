using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ControlsTestWindow : Window
    {
        public static bool TestEdgeBackend = true;

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

            mainPanel.Parent = this;
            mainPanel.BindApplicationLog();

            int CreateWebBrowserPages()
            {
                var result1 = AddWebBrowserPage("Browser IE", WebBrowserBackend.IELatest);
                var result2 = AddWebBrowserPage("Browser WebKit", WebBrowserBackend.WebKit);
                var result3 = AddEdgePage("Browser Edge");

                return MathUtils.Max(result1, result2, result3);
            }

            AddPage<ListBoxHeaderTestPage>("ListBoxHeader");

            CreateWebBrowserPages();

            AddPage<NativeSliderPage>("Native Slider");

            mainPanel.LeftListBox.SelectFirstItem();

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

#pragma warning disable
        internal PanelListBoxAndCards MainPanel => mainPanel;
#pragma warning restore

        public int AddPage<T>(string? title = null)
            where T : AbstractControl, new()
        {
            return AddPage(title ?? typeof(T).ToString(), () => new T());
        }

        public int AddPage(string title, Func<AbstractControl> createFn)
        {
            return mainPanel.Add(title, createFn);
        }

        public int AddEdgePage(string title)
        {
            var savedValue = WebBrowser.IsEdgeBackendEnabled;

            if (TestEdgeBackend)
                WebBrowser.IsEdgeBackendEnabled = true;

            if (!WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                return -1;

            AbstractControl Fn()
            {
                PanelWebBrowser.UseBackend = WebBrowserBackend.Edge;

                var result = new WebBrowserTestPage
                {
                    PageName = title,
                };

                result.Name = result.GetType().Name;

                result.SizeChanged += Log_SizeChanged;

                return result;
            }

            AbstractControl OuterFn()
            {
                try
                {
                    return Fn();
                }
                finally
                {
                    WebBrowser.IsEdgeBackendEnabled = savedValue;
                }
            }

            return AddPage(title, OuterFn);
        }

        public int AddWebBrowserPage(
            string title,
            WebBrowserBackend backend)
        {
            if (!WebBrowser.IsBackendAvailable(backend))
                return -1;

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
    }
}