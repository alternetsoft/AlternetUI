using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    public partial class DrawingMainWindow : Window
    {
        private readonly TabControl tabControl = new();
        private AbstractControl? selectedPage;

        public DrawingMainWindow()
        {
            Padding = 5;
            tabControl.Parent = this;
            tabControl.ContentPadding = 5;
            Title = "Alternet UI Drawing Sample";
            Size = (960, 700);
            StartLocation = WindowStartLocation.CenterScreen;
            tabControl.SelectedIndexChanged += TabControl_SelectedPageChanged;
            Icon = App.DefaultIcon;
            InitializePages();
        }

        private void InitializePages()
        {
            DrawingPage[] drawingPages =
            {
                new ShapesPage(),
                new TextPage(),
                new BrushesAndPensPage(),
                new GraphicsPathPage(),
                new TransformsPage(),
                new ClippingPage(),
                new ImagesPage(this),
            };

            foreach (var page in drawingPages)
                tabControl.Add(page.Name, () => {
                    return CreateTabPage(page);
                });

            drawingPages.First().OnActivated();
        }

        private AbstractControl CreateTabPage(DrawingPage page)
        {
            var tabPage = new Panel();
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

            selectedPage = tabControl.SelectedControl;

            if (selectedPage != null)
                ((DrawingPage)selectedPage.Tag!).OnActivated();
        }
    }
}