using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides extension methods for the <see cref="IWxTextBoxHandler"/> interface
    /// to manipulate text formatting and styles.
    /// </summary>
    /// <remarks>These extension methods enable additional functionality
    /// for <see cref="IWxTextBoxHandler"/>
    /// instances, such as clearing text formatting and toggling font styles
    /// for selected text or the insertion point.
    /// They are designed to work with text boxes in rich edit mode.</remarks>
    public static class IWxTextBoxHandlerExtensions
    {
        /// <summary>
        /// Appends text and styles to the end of the text control.
        /// </summary>
        /// <param name="list">List containing strings or
        /// <see cref="ITextBoxTextAttr"/> instances.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behavior is not desired,
        /// the programmer should use <see cref="ITextBoxHandler.GetInsertionPoint"/>
        /// and <see cref="ITextBoxHandler.SetInsertionPoint"/>.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static void AppendTextAndStyles(this IWxTextBoxHandler handler, IEnumerable<object> list)
        {
            foreach (object item in list)
            {
                var ta = item as ITextBoxTextAttr;
                if (ta is not null)
                {
                    handler.SetDefaultStyle(ta);
                    continue;
                }

                if (item != null)
                    handler.AppendText(item.ToString()!);
            }
        }

        /// <summary>
        /// Sets foreground and background colors of the selection.  If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        /// <param name="textColor">Foreground color.</param>
        /// <param name="backColor">Background color.</param>
        /// <remarks>
        /// If any of the color parameters is null, it will not be changed.
        /// </remarks>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static bool SelectionSetColor(
            this IWxTextBoxHandler handler,
            Color? textColor,
            Color? backColor = null)
        {
            var position = handler.GetInsertionPoint();
            var fs = handler.GetStyle(position);
            if (fs is null)
                return false;
            var newStyle = handler.CreateTextAttr();
            newStyle.Copy(fs);
            if (backColor is not null)
                newStyle.SetBackgroundColor(backColor);
            if (textColor is not null)
                newStyle.SetTextColor(textColor);
            return handler.SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Bold"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public static void SelectionToggleBold(this IWxTextBoxHandler handler)
        {
            handler.SelectionToggleFontStyle(FontStyle.Bold);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Italic"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public static void SelectionToggleItalic(this IWxTextBoxHandler handler)
        {
            handler.SelectionToggleFontStyle(FontStyle.Italic);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Underline"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public static void SelectionToggleUnderline(this IWxTextBoxHandler handler)
        {
            handler.SelectionToggleFontStyle(FontStyle.Underline);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Strikeout"/> style of the selection. If no text is
        /// selected, style of the insertion point is changed.
        /// </summary>
        public static void SelectionToggleStrikethrough(this IWxTextBoxHandler handler)
        {
            handler.SelectionToggleFontStyle(FontStyle.Strikeout);
        }

        /// <summary>
        /// Clears text formatting when <see cref="TextBox"/> is in rich edit mode.
        /// </summary>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static void ClearTextFormatting(this IWxTextBoxHandler handler)
        {
            var attr = handler.CreateTextAttr();
            attr.SetFlags(TextBoxTextAttrFlags.All);
            handler.SetSelectionStyle(attr);
        }

        /// <summary>
        /// Executes a browser command with the specified name and parameters.
        /// </summary>
        /// <param name = "cmdName" >
        /// Name of the command to execute.
        /// </param>
        /// <param name = "args" >
        /// Parameters of the command.
        /// </param>
        /// <returns>
        /// An <see cref="object"/> representing the result of the command execution.
        /// </returns>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static object? DoCommand(
            this IWxTextBoxHandler handler,
            string cmdName,
            params object?[] args)
        {
            if (handler.DisposingOrDisposed)
                return default;
            if (cmdName == "GetReportedUrl")
            {
                return handler.ReportedUrl;
            }

            return null;
        }

        /// <summary>
        /// Sets text alignment in the current position to <paramref name="alignment"/>.
        /// </summary>
        /// <param name="alignment">New alignment value.</param>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static void SelectionSetAlignment(
            this IWxTextBoxHandler handler,
            TextBoxTextAttrAlignment alignment)
        {
            var fs = handler.CreateTextAttr();
            fs.SetAlignment(alignment);
            handler.SetSelectionStyle(fs);
        }

        /// <summary>
        /// Sets text alignment in the current position
        /// to <see cref="TextBoxTextAttrAlignment.Center"/>
        /// </summary>
        public static void SelectionAlignCenter(this IWxTextBoxHandler handler)
        {
            handler.SelectionSetAlignment(TextBoxTextAttrAlignment.Center);
        }

        /// <summary>
        /// Sets text alignment in the current position
        /// to <see cref="TextBoxTextAttrAlignment.Left"/>
        /// </summary>
        public static void SelectionAlignLeft(this IWxTextBoxHandler handler)
        {
            handler.SelectionSetAlignment(TextBoxTextAttrAlignment.Left);
        }

        /// <summary>
        /// Sets text alignment in the current position
        /// to <see cref="TextBoxTextAttrAlignment.Right"/>
        /// </summary>
        public static void SelectionAlignRight(this IWxTextBoxHandler handler)
        {
            handler.SelectionSetAlignment(TextBoxTextAttrAlignment.Right);
        }

        /// <summary>
        /// Sets text alignment in the current position to
        /// <see cref="TextBoxTextAttrAlignment.Justified"/>
        /// </summary>
        public static void SelectionAlignJustified(this IWxTextBoxHandler handler)
        {
            handler.SelectionSetAlignment(TextBoxTextAttrAlignment.Justified);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle"/> of the selection. If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        /// <param name="toggle">Font style to toggle</param>
        public static void SelectionToggleFontStyle(this IWxTextBoxHandler handler, FontStyle toggle)
        {
            var position = handler.GetInsertionPoint();
            var fs = handler.GetStyle(position);
            if (fs is null)
                return;
            var style = fs.GetFontStyle();
            style = Font.ChangeFontStyle(style, toggle, !style.HasFlag(toggle));

            var newStyle = handler.CreateTextAttr();
            newStyle.Copy(fs);
            newStyle.SetFontStyle(style);
            handler.SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Changes the style of selection (if any). If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="style">The new text style.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="IWxTextBoxHandler.GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        /// <param name="handler">The <see cref="IWxTextBoxHandler"/> instance.</param>
        public static bool SetSelectionStyle(this IWxTextBoxHandler handler, ITextBoxTextAttr style)
        {
            if (handler.HasSelection)
            {
                var selectionStart = handler.GetSelectionStart();
                var selectionEnd = handler.GetSelectionEnd();
                var result = handler.SetStyle(selectionStart, selectionEnd, style);
                return result;
            }
            else
            {
                var position = handler.GetInsertionPoint();
                var result = handler.SetStyle(position, position, style);
                return result;
            }
        }
    }
}
