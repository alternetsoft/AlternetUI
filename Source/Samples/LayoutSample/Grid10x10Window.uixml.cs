using Alternet.UI;
using System;

namespace LayoutSample
{
    public partial class Grid10x10Window : Window
    {
        private Grid? grid;

        public Grid10x10Window()
        {
            InitializeComponent();

            grid = CreateGrid();
            Grid.BeginInit();
            PrepopulateGrid(10, 10);
            Grid.EndInit();
        }

        Grid Grid => grid ?? throw new Exception();

        private void PrepopulateGrid(int rowCount, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
                Grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowCount; i++)
                Grid.RowDefinitions.Add(new RowDefinition());

            for (int columnIndex = 0; columnIndex < Grid.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < Grid.RowDefinitions.Count; rowIndex++)
                {
                    var control = new Label { Text = $"{rowIndex}.{columnIndex}" };
                    Grid.Children.Add(control);

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