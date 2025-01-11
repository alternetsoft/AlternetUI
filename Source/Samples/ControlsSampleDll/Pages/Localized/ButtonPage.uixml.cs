using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class ButtonPage : Control
    {
        private readonly Label labelBackColor = new(GenericStrings.BackColor)
        {
            Margin = 5,
            VerticalAlignment = VerticalAlignment.Center,
            ColumnIndex = 0,
            RowIndex = 3,
        };

        private readonly ColorComboBox comboBoxBackColor = new()
        {
            Margin = 5,
            IsEditable = false,
            ColumnIndex = 1,
            RowIndex = 3,
        };

        private readonly Label labelTextColor = new(GenericStrings.Color)
        {
            Margin = 5,
            VerticalAlignment = VerticalAlignment.Center,
            ColumnIndex = 0,
            RowIndex = 2,
        };

        private readonly ColorComboBox comboBoxTextColor = new()
        {
            Margin = 5,
            IsEditable = false,
            ColumnIndex = 1,
            RowIndex = 2,
        };

        private int imageMargins = 5;

        private static readonly object[] ValidAlign =
        {
                    new ListControlItem(GenericStrings.Default, GenericDirection.Default),
                    new ListControlItem(GenericStrings.GenericDirectionLeft, GenericDirection.Left),
                    new ListControlItem(GenericStrings.GenericDirectionTop, GenericDirection.Top),
                    new ListControlItem(GenericStrings.GenericDirectionRight, GenericDirection.Right),
                    new ListControlItem(GenericStrings.GenericDirectionBottom, GenericDirection.Bottom),
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
                textAlignComboBox.SelectedIndex
                    = textAlignComboBox.FindStringExact(GenericStrings.Default);
                imageAlignComboBox.Items.AddRange(ValidAlign);
                imageAlignComboBox.SelectedIndex
                    = imageAlignComboBox.FindStringExact(GenericStrings.Default);

                if (!Button.ImagesEnabled)
                {
                    imageAlignComboBox.Enabled = false;
                    imageCheckBox.Enabled = false;
                    showTextCheckBox.Enabled = false;
                }

                ListControlUtils.AddFontSizes(comboBoxFontSize, true);
                ListControlUtils.AddFontNames(comboBoxFontName, true);

                comboBoxTextColor.Add(GenericStrings.Default);
                comboBoxBackColor.Add(GenericStrings.Default);
                comboBoxTextColor.SelectedIndex
                    = comboBoxTextColor.FindStringExact(GenericStrings.Default);
                comboBoxBackColor.SelectedIndex
                    = comboBoxBackColor.FindStringExact(GenericStrings.Default);

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

            button.PreviewKeyDown += Button_PreviewKeyDown;
            button.KeyDown += Button_KeyDown;
        }

        private void Button_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                case Keys.Down:
                case Keys.Up:
                    App.Log("Up or Down or Tab is pressed");
                    break;
            }
        }

        private void Button_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void ImageMarginsButton_Click(object? sender, EventArgs e)
        {
            ByteFromUserParams prm = new()
            {
                Message = GenericStrings.ImageMargin,
                DefaultValue = imageMargins,
                OnApply = (value) =>
                {
                    if (value is null)
                        return;
                    imageMargins = (int)value;
                    button.SetImageMargins(imageMargins);
                },
            };

            DialogFactory.GetNumberFromUserAsync(prm);
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
                var defaultFont = AbstractControl.DefaultFont;

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

            if (string.IsNullOrWhiteSpace(text) || text == GenericStrings.Default
                || StringUtils.IsHexNumber(text))
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