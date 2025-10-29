using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// System colors enumeration in wxWidgets.
    /// </summary>
    public enum WxSystemSettingsColor
    {
        /// <summary>
        /// The system-defined color of the background of a scroll bar.
        /// </summary>
        ScrollBar = 0,

        /// <summary>
        /// The system-defined color of the desktop.
        /// </summary>
        Desktop = 1,

        /// <summary>
        /// The system-defined color of the background of the active window's title bar.
        /// </summary>
        ActiveCaption = 2,

        /// <summary>
        /// The system-defined color of the background of an inactive window's title bar.
        /// </summary>
        InactiveCaption = 3,

        /// <summary>
        /// The system-defined color of a menu's background.
        /// </summary>
        Menu = 4,

        /// <summary>
        /// The system-defined color of the background in the client area of a window.
        /// </summary>
        Window = 5,

        /// <summary>
        /// The system-defined color of a window frame.
        /// </summary>
        WindowFrame = 6,

        /// <summary>
        /// The system-defined color of a menu's text.
        /// </summary>
        MenuText = 7,

        /// <summary>
        /// The system-defined color of the text in the client area of a window.
        /// </summary>
        WindowText = 8,

        /// <summary>
        /// The system-defined color of the text in the active window's title bar.
        /// </summary>
        ActiveCaptionText = 9,

        /// <summary>
        /// The system-defined color of the active window's border.
        /// </summary>
        ActiveBorder = 10, // 0x0A

        /// <summary>
        /// The system-defined color of an inactive window's border.
        /// </summary>
        InactiveBorder = 11, // 0x0B

        /// <summary>
        /// The system-defined color of the application workspace. The application workspace
        /// is the area in a multiple-document view that is not being occupied by documents.
        /// </summary>
        AppWorkspace = 12, // 0x0C

        /// <summary>
        /// The system-defined color of the background of selected items. This
        /// includes selected menu items as well as selected text.
        /// </summary>
        Highlight = 13, // 0x0D

        /// <summary>
        /// The system-defined color of the text of selected items.
        /// </summary>
        HighlightText = 14, // 0x0E

        /// <summary>
        /// The system-defined face color of a 3-D element.
        /// </summary>
        ButtonFace = 15, // 0x0F

        /// <summary>
        /// The system-defined color that is the shadow color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face away from the
        /// light source.
        /// </summary>
        ButtonShadow = 16, // 0x10

        /// <summary>
        /// The system-defined color of dimmed text. Items in a list that are disabled
        /// are displayed in dimmed text.
        /// </summary>
        GrayText = 17, // 0x11

        /// <summary>
        /// The system-defined color of text in a 3-D element.
        /// </summary>
        ControlText = 18, // 0x12

        /// <summary>
        /// The system-defined color of the text in an inactive window's title bar.
        /// </summary>
        InactiveCaptionText = 19, // 0x13

        /// <summary>
        /// The system-defined color that is the highlight color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ButtonHighlight = 20, // 0x14

        /// <summary>
        /// The system-defined color that is the dark shadow color of a 3-D element. The
        /// dark shadow color is applied to the parts of a 3-D element that are the darkest color.
        /// </summary>
        ControlDarkDark = 21, // 3DDKSHADOW 0x15

        /// <summary>
        /// The system-defined color that is the light color of a 3-D element. The light
        /// color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ControlLight = 22, // 3DLIGHT 0x16

        /// <summary>
        /// The system-defined color of the text of a ToolTip.
        /// </summary>
        InfoText = 23, // 0x17

        /// <summary>
        /// The system-defined color of the background of a ToolTip.
        /// </summary>
        Info = 24, // INFOBK 0x18

        /// <summary>
        /// The system-defined color.
        /// </summary>
        ListBox = 25, // 0x19

        /// <summary>
        /// The system-defined color used to designate a hot-tracked item.
        /// Single-clicking a hot-tracked item executes the item.
        /// </summary>
        HotTrack = 26, // HOTLIGHT 0x1A

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an active window's title bar.
        /// </summary>
        GradientActiveCaption = 27, // 0x1B

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an inactive window's title bar.
        /// </summary>
        GradientInactiveCaption = 28, // 0x1C

        /// <summary>
        /// The system-defined color used to highlight menu items when the menu appears
        /// as a flat menu.
        /// </summary>
        MenuHighlight = 29, // 0x1D

        /// <summary>
        /// The system-defined color of the background of a menu bar.
        /// </summary>
        MenuBar = 30, // 0x1E

        /// <summary>
        /// The system-defined color.
        /// </summary>
        ListBoxText = 31,

        /// <summary>
        /// The system-defined color.
        /// </summary>
        ListBoxHighlightText = 32,

        /// <summary>
        /// Color count.
        /// </summary>
        Max = 33,
    }
}