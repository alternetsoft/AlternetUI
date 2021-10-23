using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.Grid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void GridExample1()
        {
            #region GridCSharpCreation
            //var Grid = new Alternet.UI.Grid();
            //Grid.RowDefinitions.Add(new Alternet.UI.RowDefinition { Height = new Alternet.UI.GridLength(1, Alternet.UI.GridUnitType.Star) });
            //Grid.RowDefinitions.Add(new Alternet.UI.RowDefinition { Height = new Alternet.UI.GridLength(1, Alternet.UI.GridUnitType.Auto) });
            #endregion
        }

        #region GridEventHandler
        private void Grid_EnabledChanged(object? sender, EventArgs e)
        {
            MessageBox.Show(grid.Enabled.ToString(), string.Empty);
        }
        #endregion    
    }
}