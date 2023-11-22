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
            Application.LogReplace($"{prefix} {currentPos.Value + 1}", prefix);
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
            if(textBox is IFocusable focusable)
            {
                if (focusable.CanAcceptFocus)
                    focusable.SetFocus();
            }

            return true;
        }
    }
}
