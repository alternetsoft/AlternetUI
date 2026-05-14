using Alternet.UI;
using System;
using System.Linq;
using Alternet.Drawing;
using System.ComponentModel;
using System.IO;

namespace ControlsSample
{
    public partial class StdScrollViewerWindow : Window
    {
        private readonly LayoutSample.ImageControl imageControl = new();
        private readonly Control imageContainer;

        private Coord zoomFactor = 30;

        public StdScrollViewerWindow()
        {
            if (HasScaleFactor)
                zoomFactor = 60;
            else
                zoomFactor = 40;

            InitializeComponent();

            void LogLayoutEvents(AbstractControl control)
            {
                control.LayoutUpdated += (sender, e) =>
                {
                    App.LogIf($"{control.Name} layout updated", false);
                };
            }

            LogLayoutEvents(imageScrollViewer);

            imageControl.Image = Image.FromAssemblyUrl(typeof(LayoutSample.ImageControl).Assembly, "Resources.logo128x128.png");

            imageContainer = new Panel
            {
                Parent = imageScrollViewer,
            };

            imageTopContainer.LayoutUpdated += (sender, e) =>
            {
                App.LogIf($"Image top container layout updated", false);
            };

            imageContainer.LayoutUpdated += (sender, e) =>
            {
                App.LogIf($"Image container layout updated", false);
            };

            imageContainer.SizeChanged += (sender, e) =>
            {
                App.LogIf($"Image container size changed: {imageContainer.Size}. IsLayoutPerform: {imageContainer.IsLayoutPerform}", false);
            };

            imageControl.SizeChanged += (sender, e) =>
            {
                App.LogIf($"Image size changed: {imageControl.Size}. IsLayoutPerform: {imageControl.IsLayoutPerform}", false);
            };

            imageControl.LayoutUpdated += (sender, e) =>
            {
                App.LogIf($"Image layout updated", false);
            };

            imageControl.Parent = imageContainer;
            imageControl.Name = "imageControl";
            imageContainer.Name = "imageContainer";

            InitializeComboBoxes();

            InitializeStackPanel();
            InitializeGrid();

            SetSizeToContent();
            UpdateImageZoom();

            MenuUtils.AddItemsForPublicParameterlessMethods(stackPanelOptionsGrid.ContextMenuStrip, scrollViewerStack, "DoAction");
        }

        private void ImageTopContainer_LayoutUpdated(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitializeStackPanel()
        {
            stackPanel.DoInsideInit(() =>
            {
                for (int i = 0; i < 50; i++)
                    AddControlToStackPanel();
            });

            var setDebugBorder = false;
            if (setDebugBorder)
            {
                stackPanel.HasBorder = true;
                stackPanel.BorderColor = Color.Red;
            }
        }

        private void InitializeGrid()
        {
            grid.DoInsideInit(() =>
            {
                int rowCount = 17;
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
            var button = new Button
            {
                Text = $"{rowIndex}.{columnIndex}",
                RowColumn = new(rowIndex, columnIndex),
            };

            grid.Children.Add(button);
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
            stackPanel.Children.RemoveLast();
        }

        private void AddColumnToGridButton_Click(object? sender, EventArgs e)
        {
            grid.DoInsideLayout(() =>
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                int columnIndex = grid.ColumnDefinitions.Count - 1;
                for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                    AddControlToGrid(columnIndex, rowIndex);
            });
        }

        private void RemoveColumnFromGridButton_Click(object? sender, EventArgs e)
        {
            grid.DoInsideLayout(() =>
            {
                int columnIndex = grid.ColumnDefinitions.Count - 1;
                var toRemove = grid.Children.Where(x => Grid.GetColumn(x) == columnIndex).ToArray();
                foreach (var control in toRemove)
                    grid.Children.Remove(control);

                grid.ColumnDefinitions.RemoveAt(columnIndex);
            });
        }

        private void AddRowToGridButton_Click(object? sender, EventArgs e)
        {
            grid.DoInsideLayout(() =>
            {
                grid.RowDefinitions.Add(new RowDefinition());
                int rowIndex = grid.RowDefinitions.Count - 1;
                for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
                    AddControlToGrid(columnIndex, rowIndex);
            });
        }

        private void RemoveRowFromGridButton_Click(object? sender, EventArgs e)
        {
            grid.DoInsideLayout(() =>
            {
                int rowIndex = grid.RowDefinitions.Count - 1;
                var toRemove = grid.Children.Where(x => Grid.GetRow(x) == rowIndex).ToArray();
                foreach (var control in toRemove)
                    grid.Children.Remove(control);

                grid.RowDefinitions.RemoveAt(rowIndex);
            });
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

        private void WidthPlusButtonClick(object? sender, EventArgs e)
        {
            imageTopContainer.SuggestedWidth += 10;
        }

        private void WidthMinusButtonClick(object? sender, EventArgs e)
        {
            imageTopContainer.SuggestedWidth -= 10;
        }

        private void HeightPlusButtonClick(object? sender, EventArgs e)
        {
            imageTopContainer.SuggestedHeight += 10;
        }

        private void HeightMinusButtonClick(object? sender, EventArgs e)
        {
            imageTopContainer.SuggestedHeight -= 10;
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

        private void ZoomMinButtonClick(object? sender, EventArgs e)
        {
            if (HasScaleFactor)
                zoomFactor = 14;
            else
                zoomFactor = 7;
            UpdateImageZoom();
        }

        private void ZoomMaxButtonClick(object? sender, EventArgs e)
        {
            if (HasScaleFactor)
                zoomFactor = 60;
            else
                zoomFactor = 40;
            UpdateImageZoom();
        }

        private void UpdateImageZoom()
        {
            if (imageControl != null)
                imageControl.Zoom = 1 + 0.1f * zoomFactor;
        }
    }
}