using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI.Enums
{
    /// <summary>
    /// Enumerates possible reset color methods for <see cref="Control.ResetBackgroundColor"/>
    /// and <see cref="Control.ResetForegroundColor"/>.
    /// </summary>
    public enum ResetColorType
    {
        /// <summary>
        /// Uses default method best for the current operating system.
        /// </summary>
        Auto,

        /// <summary>
        /// Uses <see cref="Color.Empty"/>.
        /// </summary>
        NullColor,

        /// <summary>
        /// Uses colors from the default attributes record using
        /// <see cref="Control.GetDefaultAttributesBgColor"/> for the background color and
        /// <see cref="Control.GetDefaultAttributesFgColor"/> for the foreground color.
        /// </summary>
        DefaultAttributes,

        /// <summary>
        /// Uses colors from the default attributes record for the <see cref="TextBox"/> control.
        /// </summary>
        DefaultAttributesTextBox,

        /// <summary>
        /// Uses colors from the default attributes record for the <see cref="ListBox"/> control.
        /// </summary>
        DefaultAttributesListBox,

        /// <summary>
        /// Uses colors from the default attributes record for the <see cref="Button"/> control.
        /// </summary>
        DefaultAttributesButton,

        /// <summary>
        /// Uses <see cref="SystemColors.Menu"/> for the background color and
        /// <see cref="SystemColors.MenuText"/> for the foreground color.
        /// </summary>
        ColorMenu,

        /// <summary>
        /// Uses <see cref="SystemColors.ActiveCaption"/> for the background color and
        /// <see cref="SystemColors.ActiveCaptionText"/> for the foreground color.
        /// </summary>
        ColorActiveCaption,

        /// <summary>
        /// Uses <see cref="SystemColors.InactiveCaption"/> for the background color and
        /// <see cref="SystemColors.InactiveCaptionText"/> for the foreground color.
        /// </summary>
        ColorInactiveCaption,

        /// <summary>
        /// Uses <see cref="SystemColors.Info"/> for the background color and
        /// <see cref="SystemColors.InfoText"/> for the foreground color.
        /// </summary>
        ColorInfo,

        /// <summary>
        /// Uses <see cref="SystemColors.Window"/> for the background color and
        /// <see cref="SystemColors.WindowText"/> for the foreground color.
        /// </summary>
        ColorWindow,

        /// <summary>
        /// Uses <see cref="SystemColors.Highlight"/> for the background color and
        /// <see cref="SystemColors.HighlightText"/> for the foreground color.
        /// </summary>
        ColorHighlight,

        /// <summary>
        /// Uses <see cref="SystemColors.ButtonFace"/> for the background color and
        /// <see cref="SystemColors.ControlText"/> for the foreground color.
        /// </summary>
        ColorButtonFace,
    }
}
