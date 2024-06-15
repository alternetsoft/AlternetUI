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
        /// <returns></returns>
        public static DialogResult ShowDialogGoToLine(
            int lines,
            ref int line,
            Control? owner)
        {
            var lastLineNumber = lines;
            var prompt = string.Format(CommonStrings.Default.LineNumberTemplate, 1, lastLineNumber);
            var result = DialogFactory.GetNumberFromUser(
                null,
                prompt,
                CommonStrings.Default.WindowTitleGoToLine,
                line + 1,
                1,
                lastLineNumber,
                owner);
            if (result is null)
                return DialogResult.Cancel;
            line = (int)result.Value - 1;
            return DialogResult.OK;
        }

        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        /// <remarks>
        /// <paramref name="textBox"/> parameter must support <see cref="ISimpleRichTextBox"/>
        /// interface.
        /// </remarks>
        public static bool ShowDialogGoToLine(object? textBox)
        {
            if (textBox is not ISimpleRichTextBox richTextBox)
                return false;

            var lastLineNumber = richTextBox.LastLineNumber + 1;
            var prompt = string.Format(CommonStrings.Default.LineNumberTemplate, 1, lastLineNumber);
            var result = DialogFactory.GetNumberFromUser(
                null,
                prompt,
                CommonStrings.Default.WindowTitleGoToLine,
                richTextBox.InsertionPointLineNumber + 1,
                1,
                lastLineNumber,
                textBox as Control);
            if (result is null)
                return false;
            var newPosition = richTextBox.XYToPosition(0, result.Value - 1);
            richTextBox.SetInsertionPoint(newPosition);
            richTextBox.ShowPosition(newPosition);
            if (textBox is IFocusable focusable)
            {
                if (focusable.CanFocus)
                    focusable.SetFocus();
            }

            return true;
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
        public static void AdjustTextBoxesHeight(Control container)
        {
            if (container == null || !AllPlatformDefaults.PlatformCurrent.AdjustTextBoxesHeight)
                return;

            Control? comboBox = null;
            Control? textBox = null;

            FindTextEditors(container);
            if (comboBox == null)
                return;
            AdjustTextBoxesHeightInternal(container, comboBox, textBox);

            void FindTextEditors(Control container)
            {
                if (comboBox != null && textBox != null)
                    return;
                foreach (Control control in container.Children)
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
            Control container,
            Control comboBox,
            Control? textBox)
        {
            var comboBoxHeight = comboBox.Bounds.Height;
            double textBoxHeight = 0;
            if (textBox != null)
            {
                textBoxHeight = textBox.Bounds.Height;
                if (comboBoxHeight == textBoxHeight)
                    return;
            }

            var maxHeight = Math.Max(comboBoxHeight, textBoxHeight);

            if (maxHeight <= 0)
                return;

            var editors = new List<Control>();
            AddTextEditors(container);

            void AddTextEditors(Control container)
            {
                if (!container.HasChildren)
                    return;
                foreach (Control control in container.Children)
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
                foreach (Control control in editors)
                    control.SuggestedHeight = maxHeight;
            });
        }
    }
}
