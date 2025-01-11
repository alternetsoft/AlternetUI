using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace ControlsSample
{
    internal partial class TextInputPage : Panel
    {
        public static bool ConsumeTabKey = false;

        public static string MinLengthEditLabel = "Min Length";
        public static string MaxLengthEditLabel = "Max Length";
        public static string TextBoxEmptyTextHint = "Sample Hint";
        public static string TextBoxSampleText = "Sample Text";

        private PopupPropertyGrid? popup;

        static TextInputPage()
        {
        }

        public TextInputPage()
        {
            InitializeComponent();

            textBox.EmptyTextHint = TextBoxEmptyTextHint;
            textBox.Text = TextBoxSampleText;
            textBox.ValidatorReporter = textImage;
            textBox.TextMaxLength += TextBox_TextMaxLength;
            textBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            textBox.Options |= TextBoxOptions.DefaultValidation;
            textBox.TextChanged += ReportValueChanged;
            textBox.KeyPress += TextBox_KeyPress;

            ErrorsChanged += TextBox_ErrorsChanged;

            // ==== Other initializations

            textAlignEdit.ComboBox.BindEnumProp(textBox, nameof(TextBox.TextAlign));

            Group(textAlignEdit, minLengthEdit, maxLengthEdit)
                .LabelSuggestedWidthToMax();

            // ==== Min and Max length editors

            Group(minLengthEdit, maxLengthEdit).Parent(textBoxOptionsPanel);

            minLengthEdit.TextBox.TextChanged += MinLengthBox_TextChanged;
            maxLengthEdit.TextBox.TextChanged += MaxLengthBox_TextChanged;

            // ==== Other

            Idle += TextInputPage_Idle;

            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            textBox.KeyDown += TextBox_KeyDown;

            Click += (s, e) =>
            {
                DoInsideUpdate(() =>
                {
                    ParentBackColor = false;
                    ParentForeColor = false;
                    BackColor = Color.Ivory;
                    ForeColor = Color.DarkRed;
                });
            };

            panelSettings.AddInput("ReadOnly", textBox, nameof(TextBox.ReadOnly));
            panelSettings.AddInput("Password", textBox, nameof(TextBox.IsPassword));
            panelSettings.AddInput("Has Border", textBox, nameof(TextBox.HasBorder));
            panelSettings.AddInput("Allow space char", this, nameof(AllowSpaceChar));
            panelSettings.AddInput("Log Position", this, nameof(LogPosition));

            panelSettings.AddButton("Change text...", ChangeTextButton_Click);
            panelSettings.AddButton("Properties...", ShowProperties_Click);
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    App.Log("Tab pressed while TextBox is focused.");
                    break;
            }
        }

        private void TextBox_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            // if ConsumeTabKey = true, TAB key will not focus next control.
            if (ConsumeTabKey)
            {
                switch (e.KeyCode)
                {
                    case Keys.Tab:
                        e.IsInputKey = true;
                        break;
                }
            }
        }

        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && !AllowSpaceChar)
                e.Handled = true;
        }

        internal static void TextBox_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            if (sender is not AbstractControl control)
                return;

            App.LogSection(() =>
            {
                App.LogNameValue("HasErrors", control.HasErrors);
                var errors = control.GetErrors(null);
                var index = 1;
                foreach (var error in errors)
                    App.LogNameValue($"Error {index++}", error);
            });
        }

        internal bool UsePopup { get; set; } = false;

        private void ShowProperties_Click(object? sender, EventArgs e)
        {
            if (UsePopup)
            {
                popup ??= PopupPropertyGrid.CreatePropertiesPopup();
                popup.StartLocation = WindowStartLocation.ScreenTopRight;
                popup.MainControl.SetProps(textBox, true);
                popup.Show();
            }
            else
            {
                WindowPropertyGrid.ShowDefault(null, textBox, true);
            }
        }
        
        private void TextInputPage_Idle(object? sender, EventArgs e)
        {
            if (memo.VisibleOnScreen)
            {
                textBox.IdleAction();

                object[] info =
                    [
                        "Information:", Environment.NewLine, Environment.NewLine,
                        "Text = ", $"<{textBox.Text}>", Environment.NewLine,
                        "SelectedText = ", $"<{textBox.SelectedText}> ({textBox.SelectedText.Length})", 
                        Environment.NewLine,
                        "SelectionStart = ", $"<{textBox.SelectionStart}>", Environment.NewLine,
                        "SelectionLength = ", $"<{textBox.SelectionLength}>", Environment.NewLine,
                    ];

                var s = StringUtils.ToStringSimple(info);
                memo.Text = s;
            }
        }

        public static bool LogPosition { get; set; }

        public static bool AllowSpaceChar { get; set; } = true;

        private void TextBox_CurrentPositionChanged(object? sender, EventArgs e)
        {
            if (LogPosition)
                TextBoxUtils.LogPosition(sender);
        }

        private void TextBox_TextMaxLength(object? sender, EventArgs e)
        {
            App.Log("TextBox: Text max length reached");
        }

        private void MaxLengthBox_TextChanged(object? sender, EventArgs e)
        {
            var value = maxLengthEdit.TextBox.TextAsNumberOrDefault<uint>(0);
            textBox.MaxLength = (int)value;
            textBox.RunDefaultValidation();
        }

        private void MinLengthBox_TextChanged(object? sender, EventArgs e)
        {
            var value = minLengthEdit.TextBox.TextAsNumberOrDefault<uint>(0);
            textBox.MinLength = (int)value;
            textBox.RunDefaultValidation();
        }

        internal static void ReportValueChanged(object? sender, EventArgs e)
        {
            var textBox = (sender as ValueEditorCustom)?.TextBox;
            textBox ??= sender as TextBox;
            if (textBox is null)
                return;
            var name = (sender as AbstractControl)?.Name;
            var value = textBox.Text;
            string prefix;
            if (name is null)
                prefix = "TextBox: ";
            else
                prefix = $"{name}: ";

            var asNumber = textBox.TextAsNumber;

            if (asNumber is not null)
                asNumber = $" => {asNumber} | {asNumber.GetType().Name}";

            App.LogReplace($"{prefix}{value}{asNumber}", prefix);
        }

        private void ChangeTextButton_Click(object? sender, EventArgs e)
        {
            TextFromUserParams prm = new()
            {
                Message = "Text",
                DefaultValue = textBox.Text,
                OnApply = (s) =>
                {
                    if (s is not null)
                        textBox.Text = s;
                },
            };

            DialogFactory.GetTextFromUserAsync(prm);
        }
    }
}