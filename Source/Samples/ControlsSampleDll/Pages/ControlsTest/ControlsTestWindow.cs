using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class ControlsTestWindow : Window
    {
        public static BaseCollection<PageInfo> Pages = new BaseCollection<PageInfo>();

        public static bool TestEdgeBackend = true;

#pragma warning disable
        private readonly PanelListBoxAndCards mainPanel;
#pragma warning restore

        static ControlsTestWindow()
        {
            int CreateWebBrowserPages()
            {
                var result1 = AddWebBrowserPage("Browser IE", WebBrowserBackend.IELatest);
                var result2 = AddWebBrowserPage("Browser WebKit", WebBrowserBackend.WebKit);
                var result3 = AddEdgePage("Browser Edge");

                return MathUtils.Max(result1, result2, result3);
            }

            AddPage<ListBoxHeaderTestPage>("ListBoxHeader");

            CreateWebBrowserPages();

            if (!App.IsMaui)
            {
                AddPage<PopupToolBarPage>("Popup ToolBar");
            }

            /*
            if (App.IsMacOS)
            {
                AddPage<SkiaDirectPaintMacOsPage>("SkiaSharp macOs Direct Paint Sample");
            }

            if (App.IsLinuxOS)
            {
                AddPage<SkiaDirectPaintGtkPage>("SkiaSharp Gtk3 Direct Paint Sample");
            }
            */
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

            foreach (var page in Pages)
            {
                mainPanel.Add(page.Title, page.CreateFn);
            }

            mainPanel.LeftListBox.SelectFirstItem();

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

#pragma warning disable
        internal PanelListBoxAndCards MainPanel => mainPanel;
#pragma warning restore

        public static int AddPage<T>(string? title = null)
            where T : AbstractControl, new()
        {
            return AddPage(title ?? typeof(T).ToString(), () => new T());
        }

        public static int AddPage(string title, Func<AbstractControl> createFn)
        {
            Pages.Add(new PageInfo { Title = title, CreateFn = createFn });
            return Pages.Count - 1;
        }

        public static int AddEdgePage(string title)
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

        public static int AddWebBrowserPage(
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

        private static void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as AbstractControl;
            var name = control?.Name ?? control?.GetType().Name;
            App.LogIf($"{evName}: {name}, Bounds: {control!.Bounds}", false);
        }

        private static void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        public class PageInfo
        {
            public string? Title;

            public Func<AbstractControl>? CreateFn;
        }
    }
}