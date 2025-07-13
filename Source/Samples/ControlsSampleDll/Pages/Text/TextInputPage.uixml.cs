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

        private Timer timer = new(100);
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
            
            textBox.TextChanged += (s, e) =>
            { 
                if(LogText)
                    ReportValueChanged(s,e);
            };

            textBox.KeyPress += TextBox_KeyPress;

            ErrorsChanged += TextBox_ErrorsChanged;

            // ==== Other

            timer.TickAction = () =>
            {
                ReportSelection();
            };

            timer.StartRepeated();

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

            panelSettings.DoInsideLayout(() =>
            {
                panelSettings.AddInput("ReadOnly", textBox, nameof(TextBox.ReadOnly));
                panelSettings.AddInput("Password", textBox, nameof(TextBox.IsPassword));
                panelSettings.AddInput("Has Border", textBox, nameof(TextBox.HasBorder));
                panelSettings.AddInput("Allow Space Character", this, nameof(AllowSpaceChar));
                
                panelSettings.AddInput(
                    "Allow Default Context Menu",
                    textBox,
                    nameof(TextBox.AllowDefaultContextMenu));

                panelSettings.AddHorizontalLine();

                panelSettings.AddInput("Text Align", textBox, nameof(TextBox.TextAlign));

                var e = CustomEventArgs.CreateWithFlag("IsRequired");

                var itemMinLengthEdit = panelSettings.AddInput(
                    MinLengthEditLabel,
                    textBox,
                    nameof(TextBox.MinLength),
                    e);
                itemMinLengthEdit.ValueChanged += (s, e) =>
                {
                    textBox.RunDefaultValidation();
                };

                var itemMaxLengthEdit = panelSettings.AddInput(
                    MaxLengthEditLabel,
                    textBox,
                    nameof(TextBox.MaxLength),
                    e);
                itemMaxLengthEdit.ValueChanged += (s, e) =>
                {
                    textBox.RunDefaultValidation();
                };

                panelSettings.AddInput("Foreground Color", textBox, nameof(ForeColor));
                panelSettings.AddInput("Background Color", textBox, nameof(BackColor));

                panelSettings.AddLinkLabel("Change Text", ChangeTextButton_Click);
                panelSettings.AddLinkLabel("Show All Properties", ShowProperties_Click);

                panelSettings.AddHorizontalLine();

                panelSettings.AddInput("Log Text", this, nameof(LogText));
                panelSettings.AddInput("Log Position", this, nameof(LogPosition));
                panelSettings.AddInput("Log Selection", this, nameof(LogSelection));

                panelSettings.AddHorizontalLine();
            });
        }

        protected override void DisposeManaged()
        {
            timer.Stop();
            base.DisposeManaged();
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

            var errors = control.GetErrorsCollection(null);

            var errorCount = errors.Count();

            if (errorCount == 0)
                return;

            if(errorCount == 1)
            {
                var firstError = errors.FirstOrDefault();
                App.LogError(firstError);
                return;
            }

            var exception = new BaseException($"Input Validation: {errorCount} errors");

            var index = 1;
            string? s = null;
            foreach (var error in errors)
            {
                if (s != null)
                    s += Environment.NewLine;
                s+=$"Error {index++}: {error}";
            }

            exception.AdditionalInformation = s;

            App.LogError(exception);
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

        private string? reportedSelection;

        private void ReportSelection()
        {
            textBox.IdleAction();

            if (LogSelection)
            {
                var selLength = textBox.SelectedText.Length;
                var selText = textBox.SelectedText;
                var selStart = textBox.SelectionStart;
                var selLength2 = textBox.SelectionLength;
                var value = $"<{selText}>, Start = {selStart}, Length = {selLength}/{selLength2}";

                if (reportedSelection != value)
                {
                    App.LogNameValueReplace("TextBox.SelectedText", value);
                    reportedSelection = value;
                }
            }
        }

        private void TextInputPage_Idle(object? sender, EventArgs e)
        {
        }

        public static bool LogPosition { get; set; }

        public static bool LogSelection { get; set; }

        public static bool LogText { get; set; } = true;

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

        internal static void GetTextChangedInfo(
            object? sender,
            out string? varName,
            out string? varValue)
        {
            var textBox = (sender as ValueEditorCustom)?.TextBox;
            textBox ??= sender as TextBox;
            if (textBox is null)
            {
                varName = null;
                varValue = null;
                return;
            }

            var name = (sender as AbstractControl)?.Name;
            var value = textBox.Text;
            string prefix;
            if (name is null)
                prefix = "TextBox";
            else
                prefix = $"{name}";

            var asNumber = textBox.TextAsNumber;

            if (asNumber is not null)
                asNumber = $" => {asNumber} | {asNumber.GetType().Name}";

            varValue = $"{value}{asNumber}";
            varName = prefix;
        }

        internal static void ReportValueChanged(object? sender, EventArgs e)
        {
            GetTextChangedInfo(sender, out var varName, out var varValue);
            if(varName is not null)
                App.LogNameValueReplace(varName, varValue);
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