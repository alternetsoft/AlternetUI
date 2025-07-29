using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    [IsLocalized(true)]
    internal class ColorListBoxSamplePage : Panel
    {
        private readonly ColorListBox listBox = new()
        {
            SuggestedWidth = 200,
        };

        private readonly CheckBox textVisibleCheckBox = new(GenericStrings.TextVisible)
        {
            IsChecked = true,
        };

        private readonly VerticalStackPanel panel = new()
        {
            MinChildMargin = 5,
        };

        private readonly Button setColorButton = new(GenericStrings.SetColor)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly ColorPicker comboBox = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public ColorListBoxSamplePage()
        {
            Layout = LayoutStyle.Horizontal;
            MinChildMargin = 10;
            listBox.Parent = this;
            panel.Parent = this;
            comboBox.Value = Color.Red;
            comboBox.Parent = panel;
            textVisibleCheckBox.Parent = panel;
            setColorButton.Parent = panel;
            setColorButton.Click += SetColorButton_Click;
            textVisibleCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.TextVisible));
            comboBox.MinWidth = 150;
            comboBox.ValueChanged += ComboBox_SelectedItemChanged;

            listBox.HasBorder = VirtualListBox.DefaultUseInternalScrollBars;
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listBox.Value = comboBox.Value;
        }

        private void SetColorButton_Click(object? sender, EventArgs e)
        {
            if(listBox.Value == Color.Red)
            {
                listBox.Value = Color.Green;
                comboBox.Value = Color.Green;
            }
            else
            {
                listBox.Value = Color.Red;
                comboBox.Value = Color.Red;
            }
        }
    }
}
