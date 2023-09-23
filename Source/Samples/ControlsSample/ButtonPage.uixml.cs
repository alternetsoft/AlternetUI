using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ButtonPage : Control
    {
        public class TextValuePair
        {
            public string Text {get;set;}
            public object Value {get;set;}
            public TextValuePair(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private static readonly object[] ValidAlign =
            new object[]
            {
                    new TextValuePair("Default", GenericDirection.Default),
                    new TextValuePair("Left", GenericDirection.Left),
                    new TextValuePair("Top", GenericDirection.Top),
                    new TextValuePair("Right", GenericDirection.Right),
                    new TextValuePair("Bottom", GenericDirection.Bottom),
            };

        private IPageSite? site;

        public ButtonPage()
        {
            InitializeComponent();

            ApplyAll();

            textAlignComboBox.Items.AddRange(ValidAlign);
            textAlignComboBox.SelectedIndex = textAlignComboBox.FindStringExact("Default");
            imageAlignComboBox.Items.AddRange(ValidAlign);
            imageAlignComboBox.SelectedIndex = imageAlignComboBox.FindStringExact("Default");

            if (Application.IsMacOs)
            {
                imageAlignComboBox.Enabled = false;
                imageCheckBox.Enabled = false;
                showTextCheckBox.Enabled = false;
            }

            ListControlUtils.AddFontSizes(comboBoxFontSize, true);
            ListControlUtils.AddFontNames(comboBoxFontName, true);

            comboBoxTextColor.Add("Default");
            comboBoxBackColor.Add("Default");
            ListControlUtils.AddColorNames(comboBoxTextColor, false);
            ListControlUtils.AddColorNames(comboBoxBackColor, false);
            comboBoxTextColor.SelectedIndex = comboBoxTextColor.FindStringExact("Default");
            comboBoxBackColor.SelectedIndex = comboBoxBackColor.FindStringExact("Default");

            comboBoxFontName.SelectedItemChanged += ComboBoxFontName_Changed;
            comboBoxFontSize.SelectedItemChanged += ComboBoxFontSize_Changed;
            comboBoxTextColor.SelectedItemChanged += ComboBoxTextColor_Changed;
            comboBoxBackColor.SelectedItemChanged += ComboBoxBackColor_Changed;
            textAlignComboBox.SelectedItemChanged += TextAlignComboBox_Changed;
            imageAlignComboBox.SelectedItemChanged += ImageAlignComboBox_Changed;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void TextAlignComboBox_Changed(object? sender, EventArgs e) 
        {
            if (textAlignComboBox.SelectedItem == null)
                return;
            if (textAlignComboBox.SelectedItem is not TextValuePair item)
                return;
            button.TextAlign = (GenericDirection)item.Value;
            ApplyAll();
        }

        private void ApplyImageAlign()
        {
            if (!imageAlignComboBox.Enabled)
                return;
            if (imageAlignComboBox.SelectedItem == null)
                return;
            if (imageAlignComboBox.SelectedItem is not TextValuePair item)
                return;
            button.SetImagePosition((GenericDirection)item.Value);
        }

        private void ImageAlignComboBox_Changed(object? sender, EventArgs e)
        {
            button.RecreateWindow();
            ApplyAll();
        }

        private void TextTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            ApplyText();
        }

        private void ApplyText()
        {
            button.Text = textTextBox.Text;
        }

        private void ShowTextCheckBox_Changed(object? sender, EventArgs e)
        {
            button.TextVisible = showTextCheckBox.IsChecked;
            ApplyAll();
        }

        private void DisabledCheckBox_CheckedChanged(object? sender, EventArgs e)
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
            ApplyImageAlign();
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

            FontStyle fontStyle = boldCheckBox.IsChecked ? FontStyle.Bold : FontStyle.Regular;

            var s = comboBoxFontSize.SelectedItem?.ToString();
            double fontSize = string.IsNullOrWhiteSpace(s) ?
                Font.Default.SizeInPoints : Double.Parse(s);

            s = comboBoxFontName.SelectedItem?.ToString();
            string fontName = string.IsNullOrWhiteSpace(s) ? Font.Default.Name : s!;

            Font font = Alternet.Drawing.Font.GetDefaultOrNew(fontName, fontSize, fontStyle);

            //site?.LogEvent("Button: Font changed to " + font.Name + ", "
            //    + font.SizeInPoints);

            button.DoInsideUpdate(() =>
            {
                button.Font = Font.Default;
                button.Font = font;
                button.PerformLayout();
                button.Refresh();
                button.Update();
            });
        }

        private Color? GetColor(ListControl? control)
        {
            if (site == null)
                return null;

            var text = control?.SelectedItem?.ToString();

            if(string.IsNullOrWhiteSpace(text) || text == "Default" || StringUtils.IsHexNumber(text))
                return null;
            else
            {
                Color newColor = Color.FromName(text!);
                return newColor;
            }
        }

        private void ApplyTextColor()
        {
            var color = GetColor(comboBoxTextColor);
            button.ForegroundColor = color;
            site?.LogEvent($"Button: Text Color = {comboBoxTextColor.SelectedItem}");
        }

        private void ApplyBackColor()
        {
            var color = GetColor(comboBoxBackColor);
            button.BackgroundColor = color;
            site?.LogEvent($"Button: Back Color = {comboBoxBackColor.SelectedItem}");
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