using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible reset color methods.
    /// </summary>
    public enum ResetColorType
    {
        /// <summary>
        /// Uses default method best for the current operating system.
        /// </summary>
        Auto,

        /// <summary>
        /// Uses an empty color.
        /// </summary>
        EmptyColor,

        /// <summary>
        /// Uses <c>null</c>.
        /// </summary>
        NullColor,

        /// <summary>
        /// Uses colors from the default attributes record for the control.
        /// </summary>
        DefaultAttributes,

        /// <summary>
        /// Uses colors from the default attributes record for the <c>TextBox</c> control.
        /// </summary>
        DefaultAttributesTextBox,

        /// <summary>
        /// Uses colors from the default attributes record for the <c>ListBox</c> control.
        /// </summary>
        DefaultAttributesListBox,

        /// <summary>
        /// Uses colors from the default attributes record for the <c>Button</c> control.
        /// </summary>
        DefaultAttributesButton,

        /// <summary>
        /// Uses "SystemColors.Menu" for the background color and
        /// "SystemColors.MenuText" for the foreground color.
        /// </summary>
        ColorMenu,

        /// <summary>
        /// Uses "SystemColors.ActiveCaption" for the background color and
        /// "SystemColors.ActiveCaptionText" for the foreground color.
        /// </summary>
        ColorActiveCaption,

        /// <summary>
        /// Uses "SystemColors.InactiveCaption" for the background color and
        /// "SystemColors.InactiveCaptionText" for the foreground color.
        /// </summary>
        ColorInactiveCaption,

        /// <summary>
        /// Uses "SystemColors.Info" for the background color and
        /// "SystemColors.InfoText" for the foreground color.
        /// </summary>
        ColorInfo,

        /// <summary>
        /// Uses "SystemColors.Window" for the background color and
        /// "SystemColors.WindowText" for the foreground color.
        /// </summary>
        ColorWindow,

        /// <summary>
        /// Uses "SystemColors.Highlight" for the background color and
        /// "SystemColors.HighlightText" for the foreground color.
        /// </summary>
        ColorHighlight,

        /// <summary>
        /// Uses "SystemColors.ButtonFace" for the background color and
        /// "SystemColors.ControlText" for the foreground color.
        /// </summary>
        ColorButtonFace,
    }
}
