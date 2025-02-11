using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class GridWindow : Window
    {
        public GridWindow()
        {
            InitializeComponent();
            grid.HasBorder = true;
            grid.Padding = 5;
        }

        public void GridExample1()
        {
            #region GridCSharpCreation
            var Grid = new Alternet.UI.Grid();
            Grid.RowDefinitions.Add(new Alternet.UI.RowDefinition { Height = new Alternet.UI.GridLength(1, Alternet.UI.GridUnitType.Star) });
            Grid.RowDefinitions.Add(new Alternet.UI.RowDefinition { Height = new Alternet.UI.GridLength(1, Alternet.UI.GridUnitType.Auto) });
            var bt1 = new Button { Text = "First button" };
            var bt2 = new Button { Text = "Second button" };
            Grid.Children.Add(bt1);
            Grid.Children.Add(bt2);
            Alternet.UI.Grid.SetRow(bt1, 0);
            Alternet.UI.Grid.SetRow(bt2, 1);
            #endregion
        }
    }
}