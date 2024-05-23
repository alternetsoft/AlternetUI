using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual style of the <see cref="SplitterPanel"/> controls.
    /// </summary>
    [Flags]
    public enum SplitterPanelCreateStyle
    {
        /// <summary>
        /// Do not draw any border in the control.
        /// </summary>
        NoBorder = 0x0000,

        /// <summary>
        /// Draws a thin splitter sash.
        /// </summary>
        ThinSash = NoBorder,

        /// <summary>
        /// Draws no sash.
        /// </summary>
        NoSash = 0x0010,

        /// <summary>
        /// Always allow to unsplit, even with the minimum pane size
        /// other than zero.
        /// </summary>
        PermitUnsplit = 0x0040,

        /// <summary>
        /// Don't draw XOR line but resize the child windows immediately.
        /// </summary>
        LiveUpdate = 0x0080,

        /// <summary>
        /// Draws a 3D effect splitter sash.
        /// </summary>
        Sash3d = 0x0100,

        /// <summary>
        /// Draws a standard border around the control.
        /// </summary>
        Border = 0x0200,

        /// <summary>
        /// Under Windows XP (and later versions), switches off the attempt to
        /// draw the splitter using Windows XP theming, so the borders and
        /// sash will take on the pre-XP look.
        /// </summary>
        NoXPTheme = 0x0400,
    }
}