using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the system known colors.
    /// </summary>
    public enum KnownSystemColor : short
    {
        /// <summary>
        /// The system-defined color of the active window's border.
        /// </summary>
        ActiveBorder = 1,

        /// <summary>
        /// The system-defined color of the background of the active window's title bar.
        /// </summary>
        ActiveCaption = 2,

        /// <summary>
        /// The system-defined color of the text in the active window's title bar.
        /// </summary>
        ActiveCaptionText = 3,

        /// <summary>
        /// The system-defined color of the application workspace. The application workspace
        /// is the area in a multiple-document view that is not being occupied by documents.
        /// </summary>
        AppWorkspace = 4,

        /// <summary>
        /// The system-defined face color of a 3-D element.
        /// </summary>
        Control = 5,

        /// <summary>
        /// The system-defined shadow color of a 3-D element. The shadow color is applied
        /// to parts of a 3-D element that face away from the light source.
        /// </summary>
        ControlDark = 6,

        /// <summary>
        /// The system-defined color that is the dark shadow color of a 3-D element. The
        /// dark shadow color is applied to the parts of a 3-D element that are the darkest color.
        /// </summary>
        ControlDarkDark = 7,

        /// <summary>
        /// The system-defined color that is the light color of a 3-D element. The light
        /// color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ControlLight = 8,

        /// <summary>
        /// The system-defined highlight color of a 3-D element. The highlight color is
        /// applied to the parts of a 3-D element that are the lightest color.
        /// </summary>
        ControlLightLight = 9,

        /// <summary>
        /// The system-defined color of text in a 3-D element.
        /// </summary>
        ControlText = 10,

        /// <summary>
        /// The system-defined color of the desktop.
        /// </summary>
        Desktop = 11,

        /// <summary>
        /// The system-defined color of dimmed text. Items in a list that are disabled
        /// are displayed in dimmed text.
        /// </summary>
        GrayText = 12,

        /// <summary>
        /// The system-defined color of the background of selected items. This includes selected
        /// menu items as well as selected text.
        /// </summary>
        Highlight = 13,

        /// <summary>
        /// The system-defined color of the text of selected items.
        /// </summary>
        HighlightText = 14,

        /// <summary>
        /// The system-defined color used to designate a hot-tracked item.
        /// Single-clicking a hot-tracked item executes the item.
        /// </summary>
        HotTrack = 15,

        /// <summary>
        /// The system-defined color of an inactive window's border.
        /// </summary>
        InactiveBorder = 16,

        /// <summary>
        /// The system-defined color of the background of an inactive window's title bar.
        /// </summary>
        InactiveCaption = 17,

        /// <summary>
        /// The system-defined color of the text in an inactive window's title bar.
        /// </summary>
        InactiveCaptionText = 18,

        /// <summary>
        /// The system-defined color of the background of a ToolTip.
        /// </summary>
        Info = 19,

        /// <summary>
        /// The system-defined color of the text of a ToolTip.
        /// </summary>
        InfoText = 20,

        /// <summary>
        /// The system-defined color of a menu's background.
        /// </summary>
        Menu = 21,

        /// <summary>
        /// The system-defined color of a menu's text.
        /// </summary>
        MenuText = 22,

        /// <summary>
        /// The system-defined color of the background of a scroll bar.
        /// </summary>
        ScrollBar = 23,

        /// <summary>
        /// The system-defined color of the background in the client area of a window.
        /// </summary>
        Window = 24,

        /// <summary>
        /// The system-defined color of a window frame.
        /// </summary>
        WindowFrame = 25,

        /// <summary>
        /// The system-defined color of the text in the client area of a window.
        /// </summary>
        WindowText = 26,

        /// <summary>
        /// The system-defined face color of a 3-D element.
        /// </summary>
        ButtonFace = 168,

        /// <summary>
        /// The system-defined color that is the highlight color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ButtonHighlight = 169,

        /// <summary>
        /// The system-defined color that is the shadow color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face away from the
        /// light source.
        /// </summary>
        ButtonShadow = 170,

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an active window's title bar.
        /// </summary>
        GradientActiveCaption = 171,

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an inactive window's title bar.
        /// </summary>
        GradientInactiveCaption = 172,

        /// <summary>
        /// The system-defined color of the background of a menu bar.
        /// </summary>
        MenuBar = 173,

        /// <summary>
        /// The system-defined color used to highlight menu items when the menu appears
        /// as a flat menu.
        /// </summary>
        MenuHighlight = 174,
    }
}
