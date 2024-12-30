using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        internal void TestMemoFind()
        {
            TestMemoFindReplace(false);
        }

        internal void TestMemoReplace()
        {
            TestMemoFindReplace(true);
        }

        void TestRichFindReplace(bool replace)
        {
            var control = GetSelectedControl<RichTextBox>();
            if (control is null)
                return;

        }

        internal void TestRichFind()
        {
            TestRichFindReplace(false);
        }

        internal void TestRichReplace()
        {
            TestRichFindReplace(true);
        }

        void HandleTextChangedForTextAsValue(object? sender, EventArgs e)
        {
            if (sender is not TextBox c)
                return;

            var value = c.TextAsValue;

            string prefix = "TextBox.TextAsValue: ";

            if (c.TextAsValueError is null)
            {
                App.LogReplace($"{prefix}Value = {value}", prefix);
                c.ReportValidatorError(false);
            }
            else
            {
                App.LogReplace(
                    $"{prefix}Error = {c.TextAsValueError.Message}",
                    prefix,
                    LogItemKind.Error);
                c.ReportValidatorError(true);
            }

        }

        void InitTestsTextBoxAndButton()
        {
            AddControlAction<TextBoxAndButton>("Edit Thickness", (control) =>
            {
                control.AutoShowError = true;

                var c = control.TextBox;

                c.ResetInputSettings();
                c.Clear();
                c.DataType = typeof(Thickness);
                c.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.TextAsValue = new Thickness(10, 5, 5, 10);
                c.ValidatorErrorText = "Expected thickness. Example: 10, 5, 10, 5";
                c.Options |= TextBoxOptions.UseTypeConverter;

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;

            });

            AddControlAction<TextBoxAndButton>("Edit KeyGesture", (control) =>
            {
                control.AutoShowError = true;
                var c = control.TextBox;

                c.ResetInputSettings();
                c.Clear();
                c.DataType = typeof(KeyGesture);
                c.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.TextAsValue = new KeyGesture(Key.Space, Alternet.UI.ModifierKeys.ControlShift);
                c.ValidatorErrorText = "Expected key with modifier. Example: Alt+Shift+B";

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;
            });

            AddControlAction<TextBoxAndButton>("Edit DateTime", (control) =>
            {
                control.AutoShowError = true;
                var c = control.TextBox;

                c.ResetInputSettings();
                c.Clear();
                c.DataType = typeof(DateTime);
                c.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.TextAsValue = DateTime.Now;
                c.ValidatorErrorText = "Expected date and time";

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;

            });
        }

        void InitTestsTextBox()
        {
            AddControlAction<TextBox>("Edit sbyte", (c) =>
            {
                c.SetValueAndValidator((sbyte)5, true);
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("Change SelectedText", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;

                TextFromUserParams prm = new();
                prm.OnApply = (s) =>
                {
                    control.SelectedText = s ?? string.Empty;
                };

                DialogFactory.GetTextFromUserAsync(prm);
            });
        }
    }
}