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
                    new ListControlItem("Default", GenericDirection.Default),
                    new ListControlItem("Left", GenericDirection.Left),
                    new ListControlItem("Top", GenericDirection.Top),
                    new ListControlItem("Right", GenericDirection.Right),
                    new ListControlItem("Bottom", GenericDirection.Bottom),
            };

        static ButtonPage()
        {
#if DEBUG
            Button.ImagesEnabled = true;
#endif
        }

        public ButtonPage()
        {
            InitializeComponent();

            button.Padding = 5;

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

            ControlSet editors = new(
                textTextBox,
                comboBoxFontName,
                comboBoxFontSize,
                comboBoxBackColor,
                textAlignComboBox,
                imageAlignComboBox,
                comboBoxTextColor);
            editors.SuggestedHeightToMax();

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

        public void DoInside(Action action)
        {
            button?.Parent?.DoInsideUpdate(() =>
            {
                button.Parent?.DoInsideLayout(() =>
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
                if (textAlignComboBox.SelectedItem is not ListControlItem item)
                    return;

                var direction = (GenericDirection?)item.Value;

                if (direction is not null)
                    button.TextAlign = direction.Value;
            }

            void ApplyImageAlign()
            {
                if (!imageAlignComboBox.Enabled)
                    return;
                if (imageAlignComboBox.SelectedItem == null)
                    return;
                if (imageAlignComboBox.SelectedItem is not ListControlItem item)
                    return;
                var direction = (GenericDirection?)item.Value;
                if (direction is not null)
                    button.SetImagePosition(direction.Value);
            }

            void ApplyFont()
            {
                var defaultFont = Control.DefaultFont;

                FontStyle fontStyle = boldCheckBox.IsChecked ? FontStyle.Bold : FontStyle.Regular;

                var s = comboBoxFontSize.SelectedItem?.ToString();
                double fontSize = string.IsNullOrWhiteSpace(s) ?
                    defaultFont.SizeInPoints : Double.Parse(s);

                s = comboBoxFontName.SelectedItem?.ToString();
                string fontName = string.IsNullOrWhiteSpace(s) ? defaultFont.Name : s!;

                Font font = Font.GetDefaultOrNew(fontName, fontSize, fontStyle, defaultFont);
                button.Font = font.Clone();
            }

            if (button == null)
                return;

            DoInside(() =>
            {
                button.TextVisible = showTextCheckBox.IsChecked;
                button.ExactFit = exactFitCheckBox.IsChecked;
                button.HasBorder = hasBorderCheckBox.IsChecked;
                ApplyTextAlign();
                button.TabStop = tabStopCheckBox.IsChecked;
                button.IsDefault = defaultCheckBox.IsChecked;
                button.Text = textTextBox.Text;
                ApplyFont();
                var color = GetColor(comboBoxTextColor);
                button.ForegroundColor = color;
                color = GetColor(comboBoxBackColor);
                button.BackgroundColor = color;
                button.StateImages = ControlStateImages.Empty;
                if (imageCheckBox.IsChecked)
                    button.StateImages = ResourceLoader.ButtonImages;
                ApplyImageAlign();
                button.Enabled = !disabledCheckBox.IsChecked;
            });
            button.Refresh();
        }

        private void Button_Click(object? sender, System.EventArgs e)
        {
            Application.Log("Button: Click");
        }

        private Color? GetColor(ListControl? control)
        {
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