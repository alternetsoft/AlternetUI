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
    public partial class MainControl
    {
        internal void TestMemoFind()
        {
            TestMemoFindReplace(false);
        }

        internal void TestMemoReplace()
        {
            TestMemoFindReplace(true);
        }

        void HandleTextChangedForTextAsValue(object? sender, EventArgs e)
        {
            if (sender is not TextBox c)
                return;

            var value = c.ValueHelper.TextAsValue;

            string prefix = "TextBox.TextAsValue: ";

            if (c.ValueHelper.TextAsValueError is null)
            {
                App.LogReplace($"{prefix}Value = {value}", prefix);
                c.ValueHelper.ReportValidatorError(false);
            }
            else
            {
                App.LogReplace(
                    $"{prefix}Error = {c.ValueHelper.TextAsValueError.Message}",
                    prefix,
                    LogItemKind.Error);
                c.ValueHelper.ReportValidatorError(true);
            }

        }

        void InitTestsTextBoxAndButton()
        {
            AddControlAction<TextBoxAndButton>("Edit Thickness", (control) =>
            {
                control.AutoShowError = true;

                var c = control.TextBox;

                c.ValueHelper.ResetInputSettings();
                c.Clear();
                c.ValueHelper.DataType = typeof(Thickness);
                c.ValueHelper.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.ValueHelper.TextAsValue = new Thickness(10, 5, 5, 10);
                c.ValueHelper.ValidatorErrorText = "Expected thickness. Example: 10, 5, 10, 5";
                c.ValueHelper.Options |= TextBoxOptions.UseTypeConverter;

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;

            });

            AddControlAction<TextBoxAndButton>("Edit KeyGesture", (control) =>
            {
                control.AutoShowError = true;
                var c = control.TextBox;

                c.ValueHelper.ResetInputSettings();
                c.Clear();
                c.ValueHelper.DataType = typeof(KeyGesture);
                c.ValueHelper.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.ValueHelper.TextAsValue = new KeyGesture(Key.Space, Alternet.UI.ModifierKeys.ControlShift);
                c.ValueHelper.ValidatorErrorText = "Expected key with modifier. Example: Alt+Shift+B";

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;
            });

            AddControlAction<TextBoxAndButton>("Edit DateTime", (control) =>
            {
                control.AutoShowError = true;
                var c = control.TextBox;

                c.ValueHelper.ResetInputSettings();
                c.Clear();
                c.ValueHelper.DataType = typeof(DateTime);
                c.ValueHelper.TrimTextRules = TrimTextRules.TrimWhiteChars | TrimTextRules.TrimBrackets;
                c.ValueHelper.TextAsValue = DateTime.Now;
                c.ValueHelper.ValidatorErrorText = "Expected date and time";

                c.DelayedTextChanged -= HandleTextChangedForTextAsValue;
                c.DelayedTextChanged += HandleTextChangedForTextAsValue;

            });
        }

        void InitTestsTextBox()
        {
            AddControlAction<EditableListPicker>("Log sizes", (c) =>
            {
                App.Log($"Hidden TextBox height with border {c.ParentWindow?.GetTextBoxHeight(c.RealFont, true)}");
                App.Log($"Hidden TextBox height {c.ParentWindow?.GetTextBoxHeight(c.RealFont, false)}");
                App.Log($"EditableListPicker height {c.Height}");
            });

            AddControlAction<TextBox>("Edit sbyte", (c) =>
            {
                c.ValueHelper.SetValueAndValidator((sbyte)5, true);
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