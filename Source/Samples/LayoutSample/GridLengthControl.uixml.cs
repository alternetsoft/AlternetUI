using Alternet.UI;
using System;
using System.Linq;

namespace LayoutSample
{
    public partial class GridLengthControl : Control
    {
        public GridLengthControl()
        {
            InitializeComponent();

            valueComboBox.Items.AddRange(
                new[]
                {
                    GridLength.Auto,
                    new GridLength(50),
                    new GridLength(100),
                    new GridLength(150),
                    new GridLength(2, GridUnitType.Star),
                }.Cast<object>());

            valueComboBox.SelectedIndex = 0;
        }

        public event EventHandler? ValueChanged;

        public GridLength Value
        {
            get => (GridLength)valueComboBox.SelectedItem!;
            set => valueComboBox.SelectedItem = value;
        }

        public string Label
        {
            get => label.Text ?? throw new Exception();
            set => label.Text = value;
        }

        private void ValueComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}