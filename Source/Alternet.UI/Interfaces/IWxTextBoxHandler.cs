using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines additional methods and properties for handling operations in text box controls
    /// implemented via WxWidgets library,
    /// including retrieving and setting text styles,
    /// and accessing the URL at the current cursor position.
    /// </summary>
    public interface IWxTextBoxHandler : ITextBoxHandler
    {
        /// <summary>
        /// Gets or sets a value indicating whether the editor supports rich text formatting.
        /// </summary>
        bool IsRichEdit { get; set; }

        /// <summary>
        /// Gets url at current position of the cursor.
        /// This property is only valid in the event handler.
        /// </summary>
        string ReportedUrl { get; }

        /// <summary>
        /// Creates new custom text style.
        /// </summary>
        /// <returns></returns>
        ITextBoxTextAttr CreateTextAttr();

        /// <summary>
        /// Returns the style currently used for the new text.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        ITextBoxTextAttr GetDefaultStyle();

        /// <summary>
        /// Returns the style at this position in the text control.
        /// </summary>
        /// <param name="pos">The position for which text style is returned.</param>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        ITextBoxTextAttr GetStyle(long pos);

        /// <summary>
        /// Changes the default style to use for the new text which is
        /// going to be added to the control using <see cref="ITextBoxHandler.WriteText"/>
        /// or <see cref="ITextBoxHandler.AppendText"/>.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// true on success, false if an error occurred(this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If either of the font, foreground, or background color is not set in
        /// style, the values of the previous default style are used for them. If the
        /// previous default style didn't set them neither, the global font or colors
        /// of the text control itself are used as fall back.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        bool SetDefaultStyle(ITextBoxTextAttr style);

        /// <summary>
        /// Changes the style of the given range.
        /// </summary>
        /// <param name="start">The start of the range to change.</param>
        /// <param name="end">The end of the range to change.</param>
        /// <param name="style">The new style for the range.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="IWxTextBoxHandler.IsRichEdit"/> in order to use this method.
        /// </remarks>
        bool SetStyle(long start, long end, ITextBoxTextAttr style);
    }
}