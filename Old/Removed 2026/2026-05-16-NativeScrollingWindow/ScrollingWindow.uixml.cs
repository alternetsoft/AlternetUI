using Alternet.UI;
using System;
using System.Linq;
using Alternet.Drawing;
using System.ComponentModel;
using System.IO;

namespace LayoutSample
{
    public partial class ScrollingWindow : Window
    {
        private readonly ImageControl imageControl = new();
        private Coord zoomFactor = 30;

        public ScrollingWindow()
        {
            if (HasScaleFactor)
                zoomFactor = 30;
            else
                zoomFactor = 20;

            InitializeComponent();

            imageControl.Image = Image.FromAssemblyUrl(GetType().Assembly, "Resources.logo128x128.png");
            imageScrollViewer.Children.Add(imageControl);

            InitializeComboBoxes();

            InitializeStackPanel();
            InitializeGrid();

            SetSizeToContent();
            UpdateImageZoom();
        }

        private void InitializeStackPanel()
        {
            stackPanel.DoInsideInit(() =>
            {
                for (int i = 0; i < 50; i++)
                    AddControlToStackPanel();
            });
        }

        private void InitializeGrid()
        {
            grid.DoInsideInit(() =>
            {
                int rowCount = 10;
                int columnCount = 10;

                grid.RowColumnCount = new(rowCount, columnCount);

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        AddControlToGrid(columnIndex, rowIndex);
                    }
                }
            });
        }

        private void AddControlToGrid(int columnIndex, int rowIndex)
        {
            new Button
            {
                Text = $"{rowIndex}.{columnIndex}",
                RowColumn = new(rowIndex, columnIndex),
                Parent = grid,
            };
        }

        private void AddControlToStackPanel()
        {
            stackPanel.Children.Add(new Button("Button " + (stackPanel.Children.Count + 1)));
        }

        private void InitializeComboBoxes()
        {
            orientationComboBox.EnumType = typeof(StackPanelOrientation);
            orientationComboBox.Value = StackPanelOrientation.Vertical;

            stackPanelVerticalAlignmentComboBox.EnumType = typeof(VerticalAlignment);
            stackPanelVerticalAlignmentComboBox.Value = VerticalAlignment.Stretch;

            stackPanelHorizontalAlignmentComboBox.EnumType = typeof(HorizontalAlignment);
            stackPanelHorizontalAlignmentComboBox.Value = HorizontalAlignment.Stretch;

            gridHorizontalAlignmentComboBox.EnumType = typeof(HorizontalAlignment);
            gridHorizontalAlignmentComboBox.Value = HorizontalAlignment.Stretch;

            gridVerticalAlignmentComboBox.EnumType = typeof(VerticalAlignment);
            gridVerticalAlignmentComboBox.Value = VerticalAlignment.Stretch;
        }

        private void OrientationComboBox_SelectedItemChanged(object? sender, System.EventArgs e)
        {
            scrollViewerStack.LayoutOffset = PointD.Empty;
            stackPanel.Orientation = (StackPanelOrientation)orientationComboBox.Value!;
        }

        private void StackPanelVerticalAlignmentComboBox_SelectedItemChanged(
            object? sender,
            EventArgs e)
        {
            stackPanel.VerticalAlignment
                = (VerticalAlignment)stackPanelVerticalAlignmentComboBox.Value!;
        }

        private void StackPanelHorizontalAlignmentComboBox_SelectedItemChanged(
            object? sender,
            EventArgs e)
        {
            stackPanel.HorizontalAlignment
                = (HorizontalAlignment)stackPanelHorizontalAlignmentComboBox.Value!;
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
            if (gridHorizontalAlignmentComboBox.Value != null)
                grid.HorizontalAlignment = (HorizontalAlignment)gridHorizontalAlignmentComboBox.Value;
        }

        private void GridVerticalAlignmentComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (gridVerticalAlignmentComboBox.Value != null)
            {
                grid.VerticalAlignment
                    = (VerticalAlignment)gridVerticalAlignmentComboBox.Value;
            }
        }

        private void ZoomMinusButtonClick(object? sender, EventArgs e)
        {
            if (zoomFactor > 0)
                zoomFactor--;
            UpdateImageZoom();
        }

        private void ZoomPlusButtonClick(object? sender, EventArgs e)
        {
            if (zoomFactor < 70)
                zoomFactor++;
            UpdateImageZoom();
        }

        private void UpdateImageZoom()
        {
            if (imageControl != null)
                imageControl.Zoom = 1 + 0.1f * zoomFactor;
        }
    }
}