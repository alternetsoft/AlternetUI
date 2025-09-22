using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class ButtonPage : Panel
    {
        private readonly Label labelBackColor = new(GenericStrings.BackColor)
        {
            Margin = 5,
            VerticalAlignment = VerticalAlignment.Center,
            ColumnIndex = 0,
            RowIndex = 3,
        };

        private readonly ColorPicker comboBoxBackColor = new()
        {
            Margin = 5,
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

        private readonly ColorPicker comboBoxTextColor = new()
        {
            Margin = 5,
            ColumnIndex = 1,
            RowIndex = 2,
        };

        private int imageMargins = 5;

        public enum ButtonAlign
        {
            Default,
            Left,
            Top,
            Right,
            Bottom,
        }

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

                textAlignComboBox.EnumType = typeof(ButtonAlign);
                textAlignComboBox.Value = ButtonAlign.Default;

                imageAlignComboBox.EnumType = typeof(ButtonAlign);
                imageAlignComboBox.Value = ButtonAlign.Default;

                if (!Button.ImagesEnabled)
                {
                    imageAlignComboBox.Enabled = false;
                    imageCheckBox.Enabled = false;
                    showTextCheckBox.Enabled = false;
                }

                comboBoxTextColor.Select(Color.Empty, GenericStrings.Default);
                comboBoxBackColor.Select(Color.Empty, GenericStrings.Default);

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

            comboBoxFontName.ValueChanged += Button_Changed;
            comboBoxFontSize.ValueChanged += Button_Changed;
            comboBoxTextColor.ValueChanged += Fore_Changed;
            comboBoxBackColor.ValueChanged += Back_Changed;
            textAlignComboBox.ValueChanged += Button_Changed;
            imageAlignComboBox.ValueChanged += Button_Changed;
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

        private GenericDirection? ButtonAlignToDirection(ButtonAlign buttonAlign)
        {
            switch (buttonAlign)
            {
                case ButtonAlign.Default:
                    return GenericDirection.Default;
                case ButtonAlign.Left:
                    return GenericDirection.Left;
                case ButtonAlign.Top:
                    return GenericDirection.Top;
                case ButtonAlign.Right:
                    return GenericDirection.Top;
                case ButtonAlign.Bottom:
                    return GenericDirection.Bottom;
                default:
                    return null;
            }
        }

        private void ApplyAll()
        {
            void ApplyTextAlign()
            {
                if (textAlignComboBox.Value is not ButtonAlign buttonAlign)
                    return;

                var direction = ButtonAlignToDirection(buttonAlign);

                if (direction is not null)
                    button.TextAlign = direction.Value;
            }

            void ApplyImageAlign()
            {
                if (!imageAlignComboBox.Enabled)
                    return;
                if (imageAlignComboBox.Value is not ButtonAlign buttonAlign)
                    return;

                var direction = ButtonAlignToDirection(buttonAlign);

                if (direction is not null)
                    button.SetImagePosition(direction.Value);
            }

            void ApplyFont()
            {
                var defaultFont = AbstractControl.DefaultFont;

                FontStyle fontStyle = boldCheckBox.IsChecked ? FontStyle.Bold : FontStyle.Regular;

                var s = comboBoxFontSize.Value?.ToString();
                Coord fontSize = string.IsNullOrWhiteSpace(s) ?
                    defaultFont.SizeInPoints : Coord.Parse(s);

                s = comboBoxFontName.Value?.ToString();
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

        private Color? GetColor(ColorPicker? control)
        {
            var result = control?.Value;
            if (result == Color.Empty)
                result = null;
            return result;
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