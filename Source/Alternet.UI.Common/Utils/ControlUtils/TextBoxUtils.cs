using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to the <see cref="TextBox"/> and
    /// <see cref="RichTextBox"/> controls.
    /// </summary>
    public static class TextBoxUtils
    {
        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        /// <param name="lines">Number of lines.</param>
        /// <param name="line">Current line number.</param>
        /// <param name="owner">Dialog owner.</param>
        /// <param name="onApply">Action to call when the user clicks 'Ok' button.</param>
        /// <param name="onBeforeShow">Action to call before dialog is shown.</param>
        public static void ShowDialogGoToLine(
            int lines,
            int line,
            AbstractControl? owner,
            Action<int> onApply,
            Action<LongFromUserParams>? onBeforeShow = null)
        {
            var lastLineNumber = lines;

            LongFromUserParams prm = new()
            {
                Parent = owner,
                MinValue = 1,
                MaxValue = lastLineNumber,
                DefaultValue = line + 1,
                OnApply = (value) =>
                {
                    if (value is null)
                        return;
                    line = (int)value.Value - 1;
                    onApply(line);
                },
            };

            onBeforeShow?.Invoke(prm);

            prm.Title ??= CommonStrings.Default.WindowTitleGoToLine;

            var template = (prm.Message ?? CommonStrings.Default.LineNumber) + " ({0} - {1})";
            var prompt = string.Format(template, 1, lastLineNumber);

            prm.Message = prompt;

            DialogFactory.GetNumberFromUserAsync(prm);
        }

        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        /// <remarks>
        /// <paramref name="textBox"/> parameter must support <see cref="ISimpleRichTextBox"/>
        /// interface.
        /// </remarks>
        public static void ShowDialogGoToLine(object? textBox)
        {
            if (textBox is not ISimpleRichTextBox richTextBox)
                return;

            var lastLineNumber = richTextBox.LastLineNumber + 1;
            var template = CommonStrings.Default.LineNumber + " ({0} - {1})";
            var prompt = string.Format(template, 1, lastLineNumber);

            LongFromUserParams prm = new()
            {
                Title = CommonStrings.Default.WindowTitleGoToLine,
                Message = prompt,
                Parent = textBox as AbstractControl,
                MinValue = 1,
                MaxValue = lastLineNumber,
                DefaultValue = richTextBox.InsertionPointLineNumber + 1,
                OnApply = (value) =>
                {
                    if (value is null)
                        return;
                    var newPosition = richTextBox.XYToPosition(0, value.Value - 1);
                    richTextBox.SetInsertionPoint(newPosition);
                    richTextBox.ShowPosition(newPosition);
                    if (textBox is IFocusable focusable)
                    {
                        if (focusable.CanFocus)
                            focusable.SetFocus();
                    }
                },
            };

            DialogFactory.GetNumberFromUserAsync(prm);
        }

        /// <summary>
        /// Increases height of all <see cref="TextBox"/> controls in the specified
        /// container to height of the <see cref="ComboBox"/> control, if it
        /// is present in the container.
        /// </summary>
        /// <remarks>
        /// Used in Linux where height of the <see cref="ComboBox"/>
        /// is bigger than height of the <see cref="TextBox"/>.
        /// </remarks>
        /// <remarks>
        /// All <see cref="TextBox"/> of the child controls are also affected
        /// as this procedure works recursively.
        /// </remarks>
        /// <param name="container">Specifies container control in which
        /// operation is performed</param>
        public static void AdjustTextBoxesHeight(AbstractControl container)
        {
            if (container == null || !AllPlatformDefaults.PlatformCurrent.AdjustTextBoxesHeight)
                return;

            AbstractControl? comboBox = null;
            AbstractControl? textBox = null;

            FindTextEditors(container);
            if (comboBox == null)
                return;
            AdjustTextBoxesHeightInternal(container, comboBox, textBox);

            void FindTextEditors(AbstractControl container)
            {
                if (comboBox != null && textBox != null)
                    return;
                foreach (AbstractControl control in container.Children)
                {
                    if (control is TextBox box)
                        textBox = box;
                    else
                    if (control is ComboBox box1)
                        comboBox = box1;
                    else
                        FindTextEditors(control);
                    if (comboBox != null && textBox != null)
                        return;
                }
            }
        }

        /// <summary>
        /// Logs current insert position of the control.
        /// </summary>
        /// <param name="textBox"></param>
        /// <remarks>
        /// <paramref name="textBox"/> parameter must support <see cref="ISimpleRichTextBox"/>
        /// interface.
        /// </remarks>
        public static void LogPosition(object? textBox)
        {
            if (textBox is not ISimpleRichTextBox richTextBox)
                return;

            var currentPos = richTextBox.CurrentPosition;
            if (currentPos is null)
                return;
            var name = richTextBox.Name ?? textBox.GetType().Name;
            var prefix = $"{name}.CurrentPos:";
            App.LogReplace($"{prefix} {currentPos.Value + 1}", prefix);
        }

        internal static void AdjustTextBoxesHeightInternal(
            AbstractControl container,
            AbstractControl comboBox,
            AbstractControl? textBox)
        {
            var comboBoxHeight = comboBox.Bounds.Height;
            Coord textBoxHeight = 0;
            if (textBox != null)
            {
                textBoxHeight = textBox.Bounds.Height;
                if (comboBoxHeight == textBoxHeight)
                    return;
            }

            var maxHeight = Math.Max(comboBoxHeight, textBoxHeight);

            if (maxHeight <= 0)
                return;

            var editors = new List<AbstractControl>();
            AddTextEditors(container);

            void AddTextEditors(AbstractControl container)
            {
                if (!container.HasChildren)
                    return;
                foreach (AbstractControl control in container.Children)
                {
                    if (control is TextBox || control is ComboBox)
                    {
                        if (control.Bounds.Height < maxHeight)
                            editors.Add(control);
                    }
                    else
                        AddTextEditors(control);
                }
            }

            if (editors.Count == 0)
                return;
            container.DoInsideLayout(() =>
            {
                foreach (AbstractControl control in editors)
                    control.SuggestedHeight = maxHeight;
            });
        }
    }
}
