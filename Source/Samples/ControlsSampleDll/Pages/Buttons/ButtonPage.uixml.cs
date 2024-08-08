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
        private readonly Label labelBackColor = new("Back Color")
        {
            Margin = (0, 5, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            ColumnIndex = 0,
            RowIndex = 3,
        };

        private readonly ColorComboBox comboBoxBackColor = new()
        {
            Margin = (5, 5, 0, 0),
            IsEditable = false,
            ColumnIndex = 1,
            RowIndex = 3,
        };

        private readonly Label labelTextColor = new("Color")
        {
            Margin = (0, 5, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            ColumnIndex = 0,
            RowIndex = 2,
        };

        private readonly ColorComboBox comboBoxTextColor = new()
        {
            Margin = (5, 5, 0, 0),
            IsEditable = false,
            ColumnIndex = 1,
            RowIndex = 2,
        };

        private int imageMargins = 5;

        private static readonly object[] ValidAlign =
        {
                    new ListControlItem("Default", GenericDirection.Default),
                    new ListControlItem("Left", GenericDirection.Left),
                    new ListControlItem("Top", GenericDirection.Top),
                    new ListControlItem("Right", GenericDirection.Right),
                    new ListControlItem("Bottom", GenericDirection.Bottom),
        };

        private ControlStateImages? buttonImages;

        static ButtonPage()
        {
            Button.ImagesEnabled = true;
        }

        public ButtonPage()
        {
            InitializeComponent();

            DoInsideLayout(Fn);

            void Fn()
            {
                labelBackColor.Parent = propsContainer1;
                comboBoxBackColor.Parent = propsContainer1;

                labelTextColor.Parent = propsContainer2;
                comboBoxTextColor.Parent = propsContainer2;

                imageMarginsButton.Enabled = App.IsWindowsOS;
                imageMarginsButton.Click += ImageMarginsButton_Click;

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
                editors.SuggestedHeightToMax().SuggestedWidth(125);

            }

            ApplyAll();

            comboBoxFontName.SelectedItemChanged += Button_Changed;
            comboBoxFontSize.SelectedItemChanged += Button_Changed;
            comboBoxTextColor.SelectedItemChanged += Fore_Changed;
            comboBoxBackColor.SelectedItemChanged += Back_Changed;
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

        private void ImageMarginsButton_Click(object? sender, EventArgs e)
        {
            var value = DialogFactory.GetNumberFromUser("Image Margin", null, null, imageMargins);
            if (value is null)
                return;
            imageMargins = (int)value;
            button.SetImageMargins(value.Value);
        }

        public void DoInside(Action action)
        {
            action();
            button.PerformLayout();
            button.Refresh();
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
                if(App.IsLinuxOS)
                    button.RecreateWindow();
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
                button.StateImages.Assign(null);
                if (imageCheckBox.IsChecked)
                {
                    buttonImages ??= DemoResourceLoader.LoadButtonImages(button);
                    button.StateImages = buttonImages;
                }

                ApplyImageAlign();
                button.Enabled = !disabledCheckBox.IsChecked;
                button.SetImageMargins(imageMargins);
            });
        }

        private void Button_Click(object? sender, System.EventArgs e)
        {
            App.Log("Button: Click");
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

        internal bool TextIsDark()
        {
            var textColor = GetColor(comboBoxTextColor) ?? button.RealForegroundColor;
            return textColor.IsDark();
        }

        internal bool BackIsDark()
        {
            var backColor = GetColor(comboBoxBackColor) ?? button.RealBackgroundColor;
            return backColor.IsDark();
        }

        private void Back_Changed(object? sender, EventArgs e)
        {
            buttonImages = null;

            ApplyAll();
        }

        private void Fore_Changed(object? sender, EventArgs e)
        {
            ApplyAll();
        }
    }
}