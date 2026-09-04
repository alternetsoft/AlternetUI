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
        };

        private readonly XButton setColorButton = new(GenericStrings.SetColor)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly ColorPicker colorPicker;
        private readonly DrawingResourcePicker drawingResourcePicker;

        private readonly ScrollablePanelSettings settingsContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Fill,
            VerticalAlignment = VerticalAlignment.Fill,
        };

        static ColorListBoxSamplePage()
        {
        }

        public ColorListBoxSamplePage()
        {
            panel.HorizontalAlignment = HorizontalAlignment.Fill;

            settingsContainer.VerticalAlignment = VerticalAlignment.Fill;
            settingsContainer.HasBorder = true;
            settingsContainer.MinChildMargin = settingsContainer.MinChildMargin?.WithLeft(0);

            var settings = settingsContainer.ScrolledControl;

            HorizontalAlignment = HorizontalAlignment.Fill;

            drawingResourcePicker = new();

            colorPicker = new(useDefaultColors: true)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            Layout = LayoutStyle.Horizontal;
            MinChildMargin = 5;
            listBox.Parent = this;
            panel.Parent = this;
            textVisibleCheckBox.Parent = settings;

            setColorButton.Parent = settings;
            setColorButton.Click += SetColorButton_Click;
            textVisibleCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.TextVisible));

            new Label("Color picker:").Parent = settings;

            colorPicker.Value = Color.Red;
            colorPicker.Parent = settings;
            colorPicker.MinWidth = 150;
            colorPicker.ValueChanged += ComboBox_SelectedItemChanged;

            new Label("Brush picker:").Parent = settings;

            drawingResourcePicker.Parent = settings;
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

            settings.AddInput("Item Image Shape:", listBox, nameof(ColorListBox.ItemImageShape));
            settings.AddInput("Show Checkboxes", listBox, nameof(ColorListBox.CheckBoxVisible));
            settings.AddInput("Show Accent Marker", listBox, nameof(ColorListBox.ShowAccentMarker));

            // Load Sample Colors Buttons

            settings.AddHorizontalLine();

            settings.Add<BoldLabel>("Load Sample Colors");

            settings.AddButton("Load Default Colors", () =>
            {
                listBox.ItemImageShape = DrawingShapeType.Circle;
                listBox.TextVisible = true;
                listBox.DrawTextOverItemImage = false;

                listBox.BeginUpdate();

                listBox.Items.Clear();

                ListControlUtils.AddColors(listBox);

                listBox.EndUpdate();
            });

            settings.AddButton("Load All Dark Backgrounds", () =>
            {
                PrepareFoPaletter(isDark: true);

                listBox.BeginUpdate();

                listBox.Items.Clear();

                foreach (var color in LightDarkColors.LightTextBackgrounds.AllColors)
                {
                    listBox.AddColor(color);
                }

                listBox.EndUpdate();
            });

            settings.AddButton("Load All Light Backgrounds", () =>
            {
                PrepareFoPaletter(isDark: false);

                listBox.BeginUpdate();

                listBox.Items.Clear();

                foreach (var color in LightDarkColors.DarkTextBackgrounds.AllColors)
                {
                    listBox.AddColor(color);
                }

                listBox.EndUpdate();
            });

            settings.AddButton("Load 12 Dark Backgrounds", () =>
            {
                AddTwelve(true);
            });

            settings.AddButton("Load 12 Light Backgrounds", () =>
            {
                AddTwelve(false);
            });

            // Actions

            settings.AddHorizontalLine();

            settings.Add<BoldLabel>("Actions");

            settings.AddButton("Copy checked items to clipboard", () =>
            {
                StringBuilder sb = new();

                foreach (var itemIndex in listBox.CheckedIndices)
                {
                    sb.AppendLine(listBox.Items[itemIndex].Text);
                }

                Clipboard.SetText(sb.ToString());
            });

            settings.AddButton("Toggle draw text over color", () =>
            {
                listBox.DrawTextOverItemImage = !listBox.DrawTextOverItemImage;
            });

            settings.AddButton("Toggle text over color style", () =>
            {
                if (listBox.TextOverItemImageStyle?.Equals(Color.White) == true)
                    listBox.TextOverItemImageStyle = Color.Black;
                else
                    listBox.TextOverItemImageStyle = Color.White;
            });

            settings.AddButton("Add Brush Item", () =>
            {
                listBox.AddBrushItem(brush1, brush1Name);
                listBox.ScrollToLastRow();
                listBox.Invalidate();
                colorPicker.ListBox.AddBrushItem(brush1, brush1Name);
            });

            settings.AddButton("Toggle Color Image Align", () =>
            {
                listBox.IsColorRightAligned = !listBox.IsColorRightAligned;
            });

            // Context Menu and other initializations

            void PrepareFoPaletter(bool isDark)
            {
                listBox.ItemImageShape = DrawingShapeType.Rectangle;
                listBox.TextVisible = false;
                listBox.TextOverItemImageStyle = isDark ? Color.White : Color.Black;
                listBox.DrawTextOverItemImage = true;
            }

            void AddTwelve(bool isDark)
            {
                PrepareFoPaletter(isDark);

                listBox.BeginUpdate();

                listBox.Items.Clear();

                foreach (var color in LightDarkBackColors.GetColors(isDark))
                {
                    listBox.AddColor(color.Color, color.Kind.ToString());
                }

                listBox.EndUpdate();
            }

            settingsContainer.Parent = panel;
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
