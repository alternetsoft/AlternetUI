using Alternet.UI;
using System;
using System.Linq;

namespace LayoutSample
{
    public partial class ScrollingWindow : Window
    {
        public ScrollingWindow()
        {
            InitializeComponent();

            InitializeComboBoxes();

            InitializeStackPanel();
            InitializeGrid();

            SetSizeToContent();
            UpdateImageZoom();
        }

        private void InitializeStackPanel()
        {
            stackPanel.BeginInit();
            for (int i = 0; i < 50; i++)
                AddControlToStackPanel();
            stackPanel.EndInit();
        }

        private void InitializeGrid()
        {
            int rowCount = 10;
            int columnCount = 10;

            grid.BeginInit();
            for (int i = 0; i < columnCount; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowCount; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                {
                    AddControlToGrid(columnIndex, rowIndex);
                }
            }
            grid.EndInit();
        }

        private void AddControlToGrid(int columnIndex, int rowIndex)
        {
            var control = new Button { Text = $"{rowIndex}.{columnIndex}" };
            grid.Children.Add(control);

            Grid.SetColumn(control, columnIndex);
            Grid.SetRow(control, rowIndex);
        }

        private void AddControlToStackPanel()
        {
            stackPanel.Children.Add(new Button("Button " + (stackPanel.Children.Count + 1)));
        }

        private void InitializeComboBoxes()
        {
            InitializeEnumComboBox(orientationComboBox, StackPanelOrientation.Vertical);
            InitializeEnumComboBox(stackPanelVerticalAlignmentComboBox, VerticalAlignment.Stretch);
            InitializeEnumComboBox(stackPanelHorizontalAlignmentComboBox, HorizontalAlignment.Stretch);

            InitializeEnumComboBox(gridVerticalAlignmentComboBox, VerticalAlignment.Stretch);
            InitializeEnumComboBox(gridHorizontalAlignmentComboBox, HorizontalAlignment.Stretch);
        }

        private void InitializeEnumComboBox<TEnum>(ComboBox comboBox, TEnum defaultValue) where TEnum : Enum
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)))
                comboBox.Items.Add(item ?? throw new Exception());
            comboBox.SelectedItem = defaultValue;
        }

        private void OrientationComboBox_SelectedItemChanged(object? sender, System.EventArgs e)
        {
            stackPanel.Orientation = (StackPanelOrientation)orientationComboBox.SelectedItem!;
        }

        private void StackPanelVerticalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            stackPanel.VerticalAlignment = (VerticalAlignment)stackPanelVerticalAlignmentComboBox.SelectedItem!;
        }

        private void StackPanelHorizontalAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            stackPanel.HorizontalAlignment = (HorizontalAlignment)stackPanelHorizontalAlignmentComboBox.SelectedItem!;
        }

        private void AddControlToStackPanelButton_Click(object? sender, EventArgs e)
        {
            AddControlToStackPanel();
        }

        private void RemoveControlFromStackPanelButton_Click(object? sender, EventArgs e)
        {
            int count = stackPanel.Children.Count;
            if (count > 0)
                stackPanel.Children.RemoveAt(count - 1);
        }

        private void AddColumnToGridButton_Click(object? sender, EventArgs e)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            int columnIndex = grid.ColumnDefinitions.Count - 1;
            for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                AddControlToGrid(columnIndex, rowIndex);
        }

        private void RemoveColumnFromGridButton_Click(object? sender, EventArgs e)
        {
            int columnIndex = grid.ColumnDefinitions.Count - 1;
            var toRemove = grid.Children.Where(x => Grid.GetColumn(x) == columnIndex).ToArray();
            foreach (var control in toRemove)
                grid.Children.Remove(control);

            grid.ColumnDefinitions.RemoveAt(columnIndex);
        }

        private void AddRowToGridButton_Click(object? sender, EventArgs e)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            int rowIndex = grid.RowDefinitions.Count - 1;
            for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
                AddControlToGrid(columnIndex, rowIndex);
        }

        private void RemoveRowFromGridButton_Click(object? sender, EventArgs e)
        {
            int rowIndex = grid.RowDefinitions.Count - 1;
            var toRemove = grid.Children.Where(x => Grid.GetRow(x) == rowIndex).ToArray();
            foreach (var control in toRemove)
                grid.Children.Remove(control);

            grid.RowDefinitions.RemoveAt(rowIndex);
        }

        private void GridHorizontalAlignmentComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            grid.VerticalAlignment = (VerticalAlignment)gridVerticalAlignmentComboBox.SelectedItem!;
        }

        private void GridVerticalAlignmentComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (gridHorizontalAlignmentComboBox.SelectedItem != null)
                grid.HorizontalAlignment = (HorizontalAlignment)gridHorizontalAlignmentComboBox.SelectedItem!;
        }

        private void UpdateImageZoom()
        {
            imageControl.Zoom = zoomSlider.Value;
        }

        private void ZoomSlider_ValueChanged(object sender, EventArgs e)
        {
            UpdateImageZoom();
        }
    }
}