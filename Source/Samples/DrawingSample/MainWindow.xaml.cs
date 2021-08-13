using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class MainWindow : Window
    {
        private DrawingPage[] drawingPages = new DrawingPage[]
        {
            new TextPage(),
            new RectanglesPage()
        };

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            InitializePages((TabControl)FindControl("tabControl"));
        }

        private void InitializePages(TabControl tabControl)
        {
            foreach (var page in drawingPages)
                tabControl.Pages.Add(CreateTabPage(page));
        }

        private TabPage CreateTabPage(DrawingPage page)
        {
            var tabPage = new TabPage { Title = page.Name };
            var panel = new StackPanel { Orientation = StackPanelOrientation.Horizontal };
            page.SettingsControl.Width = 270;
            panel.Children.Add(page.SettingsControl);
            var canvas = new CanvasControl { Width = 705 };
            panel.Children.Add(canvas);
            page.Canvas = canvas;
            tabPage.Children.Add(panel);
            return tabPage;
        }
    }
}