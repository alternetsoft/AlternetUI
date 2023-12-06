using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    internal partial class MainWindow : Window
    {
        private TabPage? selectedPage;

        public MainWindow()
        {
            Icon = new("embres:DrawingSample.Sample.ico");

            InitializeComponent();
            InitializePages();
        }

        private void InitializePages()
        {
            DrawingPage[] drawingPages =
            [
                new ShapesPage(),
                new TextPage(),
                new BrushesAndPensPage(),
                new GraphicsPathPage(),
                new TransformsPage(),
                new ClippingPage(),
                new ImagesPage(this),
            ];

            foreach (var page in drawingPages)
                tabControl.Pages.Add(CreateTabPage(page));

            drawingPages.First().OnActivated();
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
            tabPage.Tag = page;
            return tabPage;
        }

        private void TabControl_SelectedPageChanged(object? sender, EventArgs e)
        {
            if (selectedPage != null)
                ((DrawingPage)selectedPage.Tag!).OnDeactivated();

            selectedPage = tabControl.SelectedPage;

            if (selectedPage != null)
                ((DrawingPage)selectedPage.Tag!).OnActivated();
        }
    }
}