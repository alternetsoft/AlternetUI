using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ButtonPage : Control
    {
        private IPageSite? site;

        public ButtonPage()
        {
            InitializeComponent();

            ApplyAll();

            comboBoxFontName.Items.AddRange(FontFamily.FamiliesNamesAscending);

            comboBoxFontSize.Items.AddRange(
                new object[] { 8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96 });

            var knownColors = Color.GetKnownColors();
            var colorsNames = knownColors.Select(x => x.Name).ToArray();
            comboBoxTextColor.Items.AddRange(colorsNames);
            comboBoxBackColor.Items.AddRange(colorsNames);
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void TextTextBox_TextChanged(object sender, Alternet.UI.TextChangedEventArgs e)
        {
            ApplyText();
        }

        private void ApplyText()
        {
            button.Text = textTextBox.Text;
        }

        private void ShowTextCheckBox_Changed(
            object? sender,
            System.EventArgs e)
        {
            button.TextVisible = showTextCheckBox.IsChecked;
            ApplyAll();
        }

        private void DisabledCheckBox_CheckedChanged(
            object? sender, 
            System.EventArgs e)
        {
            ApplyDisabled();
        }

        private void BoldCheckBox_CheckedChanged(
            object? sender,
            System.EventArgs e)
        {
            ApplyFont();
        }

        private void ApplyDisabled()
        {
            button.Enabled = !disabledCheckBox.IsChecked;
        }

        private void ImageCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ApplyImage();
        }

        private void ApplyImage()
        {
            if (button == null)
                return;
            button.StateImages = imageCheckBox.IsChecked ? 
                ResourceLoader.ButtonImages : new ControlStateImages();
        }

        private void TabStopCheckBox_Changed(object sender, System.EventArgs e)
        {
            button.TabStop = tabStopCheckBox.IsChecked;
            ApplyAll();
        }

        private void HasBorderCheckBox_Changed(object sender, System.EventArgs e) 
        {
            button.HasBorder = hasBorderCheckBox.IsChecked;
            ApplyAll();
        }

        private void ApplyAll()
        {
            ApplyText();
            ApplyDisabled();
            ApplyImage();
            ApplyDefault();
            ApplyFont();
            ApplyTextColor();
            ApplyBackColor();
            button.PerformLayout();
            button.Refresh();
            button.Update();
        }

        private void DefaultCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ApplyDefault();
        }

        private void ApplyDefault()
        {
            button.IsDefault = defaultCheckBox.IsChecked;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            site?.LogEvent("Button: Click");
        }

        private void ApplyFont()
        {
            if (site == null)
                return;

            object? fontNameItem = comboBoxFontName.SelectedItem;
            object? fontSizeItem = comboBoxFontSize.SelectedItem;

            FontStyle fontStyle = boldCheckBox.IsChecked ?
                FontStyle.Bold : FontStyle.Regular;

            double fontSize = fontSizeItem == null ?
                Font.Default.SizeInPoints : Double.Parse(fontSizeItem.ToString());

            string fontName = fontNameItem == null ?
                Font.Default.Name : (string)fontNameItem;

            Font font = new(fontName, fontSize, fontStyle);

            //site?.LogEvent("Button: Font changed to " + font.Name + ", "
            //    + font.SizeInPoints);

            button.Font = Font.Default;
            button.Font = font;
            button.PerformLayout();
            button.Refresh();
            button.Update();
        }

        private void ApplyTextColor()
        {
            if (site == null)
                return;

            object? item = comboBoxTextColor.SelectedItem;

            if (item == null)
                return;

            Color newColor = Color.FromName(item.ToString());

            if (ColorIsEqual(newColor, button.Foreground))
                return;

            site?.LogEvent("Button: Text Color changed to " + item);
            button.Foreground = new SolidBrush(newColor);
        }

        private bool ColorIsEqual(Color color, Brush? brush)
        {
            SolidBrush? solid = brush as SolidBrush;

            if (solid == null)
                return false;

            return solid.Color == color;
        }

        private void ApplyBackColor()
        {
            if (site == null)
                return;

            object? item = comboBoxBackColor.SelectedItem;

            if (item == null)
                return;

            Color newColor = Color.FromName(item.ToString());

            if (ColorIsEqual(newColor, button.Background))
                return;

            site?.LogEvent("Button: Back Color changed to " + item);
            button.Background = new SolidBrush(newColor);
        }

        private void ComboBoxFontName_Changed(object? sender, EventArgs e)
        {
            ApplyFont();
        }

        private void ComboBoxFontSize_Changed(object? sender, EventArgs e)
        {
            ApplyFont();
        }

        private void ComboBoxTextColor_Changed(object? sender, EventArgs e)
        {
            ApplyTextColor();
        }

        private void ComboBoxBackColor_Changed(object? sender, EventArgs e)
        {
            ApplyBackColor();
        }
    }
}