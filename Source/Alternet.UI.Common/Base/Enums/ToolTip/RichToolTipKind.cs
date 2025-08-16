using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates tip kinds of the tooltip.
    /// </summary>
    /// <remarks>
    /// This enum describes the kind of the tip shown which combines both the tip position
    /// and appearance because the two are related(when the tip is positioned asymmetrically,
    /// a right handed triangle is used but an equilateral one when it's in the middle of a side).
    /// </remarks>
    /// <remarks>
    /// <see cref="Auto"/> selects the tip appearance best suited for the current platform and the
    /// position best suited for the window the tooltip is shown for, i.e.chosen in such
    /// a way that the tooltip is always fully on screen.
    /// </remarks>
    /// <remarks>
    /// Other values describe the position of the tooltip itself, not the window it relates
    /// to. E.g. <see cref="Top"/> places the tip on the top of the tooltip and so the tooltip
    /// itself is located beneath its associated window.
    /// </remarks>
    /// <remarks>
    /// Notice that currently <see cref="Top"/> or <see cref="Bottom"/> are used under Mac while
    /// one of the other four values is selected for the other platforms.
    /// </remarks>
    public enum RichToolTipKind
    {
        /// <summary>
        /// Don't show any tip, the tooltip will be (roughly) rectangular.
        /// </summary>
        None,

        /// <summary>
        /// Show a right triangle tip in the top left corner of the tooltip.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Show an equilateral triangle tip in the middle of the tooltip top side.
        /// </summary>
        Top,

        /// <summary>
        /// Show a right triangle tip in the top right corner of the tooltip.
        /// </summary>
        TopRight,

        /// <summary>
        /// Show a right triangle tip in the bottom left corner of the tooltip.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Show an equilateral triangle tip in the middle of the tooltip bottom side.
        /// </summary>
        Bottom,

        /// <summary>
        /// Show a right triangle tip in the bottom right corner of the tooltip.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Choose the appropriate tip shape and position automatically. This is the default
        /// and shouldn't normally need to be changed.
        /// </summary>
        Auto,
    }
}