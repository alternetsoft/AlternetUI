using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlsSample
{
    internal partial class ButtonPage : Control
    {
        private IPageSite? site;

        public ButtonPage()
        {
            InitializeComponent();

            ApplyText();
            ApplyDisabled();
            ApplyImage();
            ApplyDefault();

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

            site?.LogEvent("Button: Font changed to " + font.Name + ", "
                + font.SizeInPoints);

            button.Font = font;
            button.PerformLayout();
        }

        private void ApplyTextColor()
        {
            if (site == null)
                return;

            object? item = comboBoxTextColor.SelectedItem;

            if (item == null)
                return;

            site?.LogEvent("Button: Text Color changed to " + item);

            Color newColor = Color.FromName(item.ToString());

            button.Foreground = new SolidBrush(newColor);
        }

        private void ApplyBackColor()
        {
            if (site == null)
                return;

            object? item = comboBoxBackColor.SelectedItem;

            if (item == null)
                return;

            site?.LogEvent("Button: Back Color changed to " + item);

            Color newColor = Color.FromName(item.ToString());

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