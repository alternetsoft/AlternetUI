using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class GridLayoutPropertiesWindow : Window
    {
        public GridLayoutPropertiesWindow()
        {
            InitializeComponent();

            containerAlignmentControl.Control = subjectGroupBox;
            
            subjectColumnWidthControl.Value = subjectGridColumn.Width;
            subjectRowHeightControl.Value = subjectGridRow.Height;
        }

        Thickness IncreaseThickness(Thickness value)
        {
            const int D = 10;
            return new Thickness(value.Left + D, value.Top + D, value.Right + D, value.Bottom + D);
        }

        private void SubjectColumnWidthControl_ValueChanged(object? sender, EventArgs e)
        {
            subjectGridColumn.Width = subjectColumnWidthControl.Value;
            subjectColumnWidthLabel.Text = subjectColumnWidthControl.Value.ToString();
        }

        private void SubjectRowHeightControl_ValueChanged(object? sender, EventArgs e)
        {
            subjectGridRow.Height = subjectRowHeightControl.Value;
            subjectRowHeightLabel.Text = subjectRowHeightControl.Value.ToString();
        }

        private void AddGridColumnButton_Click(object? sender, EventArgs e)
        {
            subjectGrid.BeginInit();
            try
            {
                var column = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                subjectGrid.ColumnDefinitions.Add(column);

                int columnIndex = subjectGrid.ColumnDefinitions.Count - 1;

                var lengthLabel = new Label
                {
                    Text = column.Width.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                subjectGrid.Children.Add(lengthLabel);
                Grid.SetColumn(lengthLabel, columnIndex);
                Grid.SetRow(lengthLabel, 0);

                for (int rowIndex = 1; rowIndex < subjectGrid.RowDefinitions.Count; rowIndex++)
                {
                    var border = new Border();
                    subjectGrid.Children.Add(border);
                    Grid.SetColumn(border, columnIndex);
                    Grid.SetRow(border, rowIndex);

                    var label = new Label { Text = $"({columnIndex}, {rowIndex})" };
                    border.Children.Add(label);
                }

            }
            finally
            {
                subjectGrid.EndInit();
            }
        }

        private void AddGridRowButton_Click(object? sender, EventArgs e)
        {
            subjectGrid.BeginInit();
            try
            {
                var row = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
                subjectGrid.RowDefinitions.Add(row);

                int rowIndex = subjectGrid.RowDefinitions.Count - 1;

                var lengthLabel = new Label
                {
                    Text = row.Height.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 3, 0)
                };

                subjectGrid.Children.Add(lengthLabel);
                Grid.SetColumn(lengthLabel, 0);
                Grid.SetRow(lengthLabel, rowIndex);

                for (int columnIndex = 1; columnIndex < subjectGrid.ColumnDefinitions.Count; columnIndex++)
                {
                    var border = new Border();
                    subjectGrid.Children.Add(border);
                    Grid.SetColumn(border, columnIndex);
                    Grid.SetRow(border, rowIndex);

                    var label = new Label { Text = $"({columnIndex}, {rowIndex})" };
                    border.Children.Add(label);
                }

            }
            finally
            {
                subjectGrid.EndInit();
            }
        }

        private void IncreaseButtonMarginButton_Click(object? sender, EventArgs e) =>
            subjectButton.Margin = IncreaseThickness(subjectButton.Margin);

        private void IncreaseButtonPaddingButton_Click(object? sender, EventArgs e) =>
            subjectButton.Padding = IncreaseThickness(subjectButton.Padding);

        private void IncreaseContainerMarginButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Margin = IncreaseThickness(subjectGroupBox.Margin);

        private void IncreaseContainerPaddingButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Padding = IncreaseThickness(subjectGroupBox.Padding);
    }
}