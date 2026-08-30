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
    public class ColorListBoxSamplePage : Panel
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
        private readonly DrawingResourcePicker drawingResourcePicker;
        private readonly PanelSettings settings = new();

        static ColorListBoxSamplePage()
        {
        }

        public ColorListBoxSamplePage()
        {
            drawingResourcePicker = new();

            colorPicker = new(useDefaultColors: true)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            Layout = LayoutStyle.Horizontal;
            MinChildMargin = 10;
            listBox.Parent = this;
            panel.Parent = this;
            textVisibleCheckBox.Parent = panel;
            setColorButton.Parent = panel;
            setColorButton.Click += SetColorButton_Click;
            textVisibleCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.TextVisible));

            new Label("Color picker:").Parent = panel;

            colorPicker.Value = Color.Red;
            colorPicker.Parent = panel;
            colorPicker.MinWidth = 150;
            colorPicker.ValueChanged += ComboBox_SelectedItemChanged;

            new Label("Brush picker:").Parent = panel;

            drawingResourcePicker.Parent = panel;
            drawingResourcePicker.MinWidth = 150;

            var brush1 = new HatchBrush(BrushHatchStyle.Horizontal, LightDarkColors.Red);
            DrawingResource brush1resource = new(brush1);
            var brush1Name = $"Horizontal Red";
            brush1resource.Title = brush1Name;

            var brush2 = new HatchBrush(BrushHatchStyle.Vertical, LightDarkColors.Green);
            DrawingResource brush2resource = new(brush2);
            var brush2Name = $"Vertical Green";
            brush2resource.Title = brush2Name;

            drawingResourcePicker.Add(brush1resource);
            drawingResourcePicker.Add(brush2resource);

            drawingResourcePicker.Value = brush1resource;

            this.ContextMenuStrip.Add("Add brush item", () =>
            {
                listBox.AddBrushItem(brush1, brush1Name);
                colorPicker.ListBox.AddBrushItem(brush1, brush1Name);
            });

            this.ContextMenuStrip.Add("Toggle color image alignment", () =>
            {
                listBox.IsColorRightAligned = !listBox.IsColorRightAligned;
            });

            settings.VerticalAlignment = VerticalAlignment.Fill;
            settings.MinChildMargin = settings.MinChildMargin?.WithLeft(0);
            settings.Parent = panel;

            settings.AddInput("Item Image Shape:", listBox, nameof(ColorListBox.ItemImageShape));
            settings.AddInput("Show Checkboxes", listBox, nameof(ColorListBox.CheckBoxVisible));
            settings.AddInput("Show Accent Marker", listBox, nameof(ColorListBox.ShowAccentMarker));

            this.ContextMenuStrip.Add("Toggle draw text over item image", () =>
            {
                listBox.DrawTextOverItemImage = !listBox.DrawTextOverItemImage;
            });

            this.ContextMenuStrip.Add("Toggle text over item image style", () =>
            {
                if(listBox.TextOverItemImageStyle?.Equals(Color.White) == true)
                    listBox.TextOverItemImageStyle = Color.Black;
                else
                    listBox.TextOverItemImageStyle = Color.White;
            });

            this.ContextMenuStrip.Add("Toggle checkboxes", () =>
            {
                listBox.CheckBoxVisible = !listBox.CheckBoxVisible;
            });
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listBox.Value = colorPicker.Value;
        }

        private void SetColorButton_Click(object? sender, EventArgs e)
        {
            if (listBox.Value == Color.Red)
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
