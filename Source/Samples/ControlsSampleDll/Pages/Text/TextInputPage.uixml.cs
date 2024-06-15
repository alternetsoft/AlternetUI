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
    internal partial class TextInputPage : Control
    {
        private readonly ValueEditorUInt32 minLengthEdit;
        private readonly ValueEditorUInt32 maxLengthEdit;
        private PopupPropertyGrid? popup;

        static TextInputPage()
        {
        }

        public TextInputPage()
        {
            minLengthEdit = new("Min Length", 0);
            maxLengthEdit = new("Max Length", 0);

            InitializeComponent();

            textBox.EmptyTextHint = "Sample Hint";
            textBox.Text = "sample text";
            textBox.ValidatorReporter = textImage;
            textBox.TextMaxLength += TextBox_TextMaxLength;
            textBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            textBox.Options |= TextBoxOptions.DefaultValidation;
            textBox.TextChanged += ReportValueChanged;
            TextBox.InitErrorPicture(textImage);

            ErrorsChanged += TextBox_ErrorsChanged;

            // ==== Other initializations

            textAlignEdit.ComboBox.BindEnumProp(textBox, nameof(TextBox.TextAlign));

            readOnlyCheckBox.BindBoolProp(textBox, nameof(TextBox.ReadOnly));
            passwordCheckBox.BindBoolProp(textBox, nameof(TextBox.IsPassword));
            hasBorderCheckBox.BindBoolProp(textBox, nameof(TextBox.HasBorder));
            logPositionCheckBox.BindBoolProp(this, nameof(LogPosition));

            Group(textAlignEdit, minLengthEdit, maxLengthEdit)
                .LabelSuggestedWidthToMax()
                .InnerSuggestedWidthToMax();

            // ==== Min and Max length editors

            Group(minLengthEdit, maxLengthEdit).Margin((0, 0, 0, 5)).Parent(textBoxOptionsPanel);

            minLengthEdit.TextBox.TextChanged += MinLengthBox_TextChanged;
            minLengthEdit.TextBox.IsRequired = true;
            maxLengthEdit.TextBox.TextChanged += MaxLengthBox_TextChanged;
            maxLengthEdit.TextBox.IsRequired = true;

            Idle += TextInputPage_Idle;
        }

        internal static void TextBox_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            if (sender is not Control control)
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

        private void ShowProperties_Click(object? sender, EventArgs e)
        {
            popup ??= PopupPropertyGrid.CreatePropertiesPopup();
            popup.MainControl.SetProps(textBox, true);
            popup.ShowPopup(showPropertiesButton);
        }
        
        private void TextInputPage_Idle(object? sender, EventArgs e)
        {
            if (tab1.Visible)
            {
                textBox.IdleAction();
            }
        }

        public static bool LogPosition { get; set; }

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
            var name = (sender as Control)?.Name;
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
            var result = DialogFactory.GetTextFromUser("Text", "Enter text value", textBox.Text);
            if(result is not null)
                textBox.Text = result;
        }
    }
}