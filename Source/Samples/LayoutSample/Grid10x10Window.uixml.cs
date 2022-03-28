using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class Grid10x10Window : Window
    {
        public Grid10x10Window()
        {
            InitializeComponent();

            grid = CreateGrid();
            grid.BeginInit();
            PrepopulateGrid(10, 10);
            grid.EndInit();
        }

        Grid grid;

        private void PrepopulateGrid(int rowCount, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowCount; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                {
                    var control = new Label { Text = $"{rowIndex}.{columnIndex}" };
                    grid.Children.Add(control);

                    Grid.SetColumn(control, columnIndex);
                    Grid.SetRow(control, rowIndex);
                }
            }
        }

        private Grid CreateGrid()
        {
            var grid = new Grid();
            generatedControlsRoot.Children.Add(grid);

            return grid;
        }

        private void AddColumnButton_Click(object? sender, EventArgs e)
        {
            AddColumn();
        }

        private void AddColumn()
        {
        }
    }
}