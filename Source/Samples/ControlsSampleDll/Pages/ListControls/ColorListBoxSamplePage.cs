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

        private readonly XCheckBox textVisibleCheckBox = new(GenericStrings.TextVisible)
        {
            IsChecked = true,
        };

        private readonly VerticalStackPanel panel = new()
        {
            MinChildMargin = 5,
        };

        private readonly XButton setColorButton = new(GenericStrings.SetColor)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly ColorPicker colorPicker;

        static ColorListBoxSamplePage()
        {
        }

        public ColorListBoxSamplePage()
        {
            colorPicker = new(useDefaultColors: true)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            Layout = LayoutStyle.Horizontal;
            MinChildMargin = 10;
            listBox.Parent = this;
            panel.Parent = this;
            colorPicker.Value = Color.Red;
            colorPicker.Parent = panel;
            textVisibleCheckBox.Parent = panel;
            setColorButton.Parent = panel;
            setColorButton.Click += SetColorButton_Click;
            textVisibleCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.TextVisible));
            colorPicker.MinWidth = 150;
            colorPicker.ValueChanged += ComboBox_SelectedItemChanged;

            this.ContextMenuStrip.Add("Add brush item", () =>
            {
                var brush = new HatchBrush(BrushHatchStyle.Horizontal, Color.Red);
                var brushName = $"HatchBrush";

                listBox.AddBrushItem(brush, brushName);
                colorPicker.ListBox.AddBrushItem(brush, brushName);
            });

            this.ContextMenuStrip.Add("Toggle color image alignment", () =>
            {
                listBox.IsColorRightAligned = !listBox.IsColorRightAligned;
            });
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listBox.Value = colorPicker.Value;
        }

        private void SetColorButton_Click(object? sender, EventArgs e)
        {
            if(listBox.Value == Color.Red)
            {
                listBox.Value = Color.Green;
                colorPicker.Value = Color.Green;
            }
            else
            {
                listBox.Value = Color.Red;
                colorPicker.Value = Color.Red;
            }
        }
    }
}
