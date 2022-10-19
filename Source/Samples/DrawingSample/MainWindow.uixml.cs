using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class MainWindow : Window
    {
        private DrawingPage[] drawingPages = new DrawingPage[]
        {
            new TransformsPage(),
            new TextPage(),
            new BrushesAndPensPage(),
            new GraphicsPathPage(),
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
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(270) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(page.SettingsControl);
            var canvas = new CanvasControl();
            grid.Children.Add(canvas);
            Grid.SetColumn(canvas, 1);
            page.Canvas = canvas;
            tabPage.Children.Add(grid);
            return tabPage;
        }
    }
}