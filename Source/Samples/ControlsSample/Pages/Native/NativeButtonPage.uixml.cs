using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class NativeButtonPage : Panel
    {
        private int imageMargins = 5;

        private ControlStateImages? buttonImages;

        static NativeButtonPage()
        {
            Button.ImagesEnabled = true;
        }

        public NativeButtonPage()
        {
            InitializeComponent();

            exactFitCheckBox.Checked = button.ExactFit;

            DoInsideLayout(Fn);

            void Fn()
            {
                imageMarginsButton.Enabled = App.IsWindowsOS;
                imageMarginsButton.Click += ImageMarginsButton_Click;

                button.Padding = 5;

                textAlignComboBox.EnumType = typeof(ElementContentAlign);
                textAlignComboBox.Value = ElementContentAlign.Default;

                imageAlignComboBox.EnumType = typeof(ElementContentAlign);
                imageAlignComboBox.Value = ElementContentAlign.Default;

                if (!Button.ImagesEnabled)
                {
                    imageAlignComboBox.Enabled = false;
                    imageCheckBox.Enabled = false;
                    showTextCheckBox.Enabled = false;
                }

                ControlSet editors = new(
                    textTextBox,
                    comboBoxFontName,
                    comboBoxFontSize,
                    textAlignComboBox,
                    imageAlignComboBox);
                editors.SuggestedHeightToMax().SuggestedWidth(125);
            }

            ApplyAll();

            comboBoxFontName.SetValue(button.RealFont);

            comboBoxFontName.ValueChanged += Button_Changed;
            comboBoxFontSize.ValueChanged += Button_Changed;
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

            button.Parent?.ContextMenuStrip.Add("Set ToolTipObject", () =>
            {
                button.ResetMouseHoverEvent();
                ButtonPage.SetRichToolTip(button);
            });
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
                if (textAlignComboBox.Value is not ElementContentAlign buttonAlign)
                    return;
                button.TextAlign = buttonAlign;
            }

            void ApplyImageAlign()
            {
                if (!imageAlignComboBox.Enabled)
                    return;
                if (imageAlignComboBox.Value is not ElementContentAlign buttonAlign)
                    return;
                button.SetImagePosition(buttonAlign);
            }

            void ApplyFont()
            {
                var defaultFont = AbstractControl.DefaultFont;

                FontStyle fontStyle = boldCheckBox.IsChecked ? FontStyle.Bold : FontStyle.Regular;

                var s = comboBoxFontSize.Value?.ToString();
                float fontSize = string.IsNullOrWhiteSpace(s) ?
                    defaultFont.SizeInPoints : float.Parse(s);

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

        private void Button_Changed(object? sender, EventArgs e)
        {
            ApplyAll();
        }
    }
}