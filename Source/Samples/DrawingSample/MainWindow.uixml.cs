using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class MainWindow : Window
    {
        private DrawingPage[] drawingPages = new DrawingPage[]
        {
            new TextPage(),
            new BrushesAndPensPage()
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializePages();
        }

        private void InitializePages()
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