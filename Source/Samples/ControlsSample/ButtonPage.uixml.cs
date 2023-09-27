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
        private static readonly object[] ValidAlign =
            new object[]
            {
                    new ListControlTextValuePair("Default", GenericDirection.Default),
                    new ListControlTextValuePair("Left", GenericDirection.Left),
                    new ListControlTextValuePair("Top", GenericDirection.Top),
                    new ListControlTextValuePair("Right", GenericDirection.Right),
                    new ListControlTextValuePair("Bottom", GenericDirection.Bottom),
            };

        private IPageSite? site;

        static ButtonPage()
        {
#if DEBUG
            Button.ImagesEnabled = true;
#endif
        }

        public ButtonPage()
        {
            InitializeComponent();

            textAlignComboBox.Items.AddRange(ValidAlign);
            textAlignComboBox.SelectedIndex = textAlignComboBox.FindStringExact("Default");
            imageAlignComboBox.Items.AddRange(ValidAlign);
            imageAlignComboBox.SelectedIndex = imageAlignComboBox.FindStringExact("Default");

            if (!Button.ImagesEnabled)
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
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;

                ApplyAll();

                comboBoxFontName.SelectedItemChanged += Button_Changed;
                comboBoxFontSize.SelectedItemChanged += Button_Changed;
                comboBoxTextColor.SelectedItemChanged += Button_Changed;
                comboBoxBackColor.SelectedItemChanged += Button_Changed;
                textAlignComboBox.SelectedItemChanged += Button_Changed;
                imageAlignComboBox.SelectedItemChanged += Button_Changed;
                disabledCheckBox.CheckedChanged += Button_Changed;
                imageCheckBox.CheckedChanged += Button_Changed;
                defaultCheckBox.CheckedChanged += Button_Changed;
                boldCheckBox.CheckedChanged += Button_Changed;
                hasBorderCheckBox.CheckedChanged += Button_Changed;
                tabStopCheckBox.CheckedChanged += Button_Changed;
                showTextCheckBox.CheckedChanged += Button_Changed;
                exactFitCheckBox.CheckedChanged += Button_Changed;
            }
        }

        public void DoInside(Action action)
        {
            button?.Parent?.DoInsideUpdate(() =>
            {
                button.DoInsideLayout(() =>
                {
                    action();
                });
            });
        }

        private void ApplyAll()
        {
            void ApplyTextAlign()
            {
                if (textAlignComboBox.SelectedItem == null)
                    return;
                if (textAlignComboBox.SelectedItem is not ListControlTextValuePair item)
                    return;
                button.TextAlign = (GenericDirection)item.Value;
            }

            void ApplyImageAlign()
            {
                if (!imageAlignComboBox.Enabled)
                    return;
                if (imageAlignComboBox.SelectedItem == null)
                    return;
                if (imageAlignComboBox.SelectedItem is not ListControlTextValuePair item)
                    return;
                button.SetImagePosition((GenericDirection)item.Value);
            }

            void ApplyFont()
            {
                FontStyle fontStyle = boldCheckBox.IsChecked ? FontStyle.Bold : FontStyle.Regular;

                var s = comboBoxFontSize.SelectedItem?.ToString();
                double fontSize = string.IsNullOrWhiteSpace(s) ?
                    Font.Default.SizeInPoints : Double.Parse(s);

                s = comboBoxFontName.SelectedItem?.ToString();
                string fontName = string.IsNullOrWhiteSpace(s) ? Font.Default.Name : s!;

                Font font = Font.GetDefaultOrNew(fontName, fontSize, fontStyle);

                button.Font = Font.Default;
                button.Font = font;
            }

            if (site == null)
                return;
            if (button == null)
                return;

            DoInside(() =>
            {
                button.ExactFit = exactFitCheckBox.IsChecked;
                button.TabStop = tabStopCheckBox.IsChecked;
                button.HasBorder = hasBorderCheckBox.IsChecked;
                button.TextVisible = showTextCheckBox.IsChecked;
                ApplyTextAlign();
                button.Enabled = !disabledCheckBox.IsChecked;
                button.IsDefault = defaultCheckBox.IsChecked;
                button.Text = textTextBox.Text;
                ApplyFont();
                button.StateImages = imageCheckBox.IsChecked ?
                    ResourceLoader.ButtonImages : new ControlStateImages();
                ApplyImageAlign();
                var color = GetColor(comboBoxTextColor);
                button.ForegroundColor = color;
                color = GetColor(comboBoxBackColor);
                button.BackgroundColor = color;
            });
            button.Refresh();
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            site?.LogEvent("Button: Click");
        }

        private Color? GetColor(ListControl? control)
        {
            if (site == null)
                return null;

            var text = control?.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(text) || text == "Default" || StringUtils.IsHexNumber(text))
                return null;
            else
            {
                Color newColor = Color.FromName(text!);
                return newColor;
            }
        }

        private void Button_Changed(object? sender, EventArgs e)
        {
            ApplyAll();
        }
    }
}