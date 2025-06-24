using Alternet.UI;
using System;
using System.Linq;

namespace LayoutSample
{
    public partial class GridLengthControl : Panel
    {
        public GridLengthControl()
        {
            InitializeComponent();

            AddGridLengthItem(GridLength.Auto, "Auto");
            AddGridLengthItem(new GridLength(100), "100 dips");
            AddGridLengthItem(new GridLength(150), "150 dips");
            AddGridLengthItem(new GridLength(200), "200 dips");
            AddGridLengthItem(new GridLength(1, GridUnitType.Star), "1*");
            AddGridLengthItem(new GridLength(2, GridUnitType.Star), "2*");
            AddGridLengthItem(new GridLength(3, GridUnitType.Star), "3*");

            valueComboBox.Value = GridLength.Auto;
        }

        public event EventHandler? ValueChanged;

        public GridLength Value
        {
            get => (GridLength)valueComboBox.Value!;
            set => valueComboBox.Value = value;
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

        public class GridLengthItem : ListControlItem
        {             
            public GridLengthItem(GridLength value, string title)
            {
                Value = value;
                Text = title;
            }
        }

        public void AddGridLengthItem(GridLength length, string title)
        {
            var item = new GridLengthItem(length, title);
            valueComboBox.Items.Add(item);
        }
    }
}