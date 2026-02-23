using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to control button control.
    /// </summary>
    public interface IButtonHandler
    {
        /// <summary>
        /// Sets image for the 'Normal' state.
        /// </summary>
        Image? NormalImage { set; }

        /// <summary>
        /// Sets image for the 'Hovered' state.
        /// </summary>
        Image? HoveredImage { set; }

        /// <summary>
        /// Sets image for the 'Pressed' state.
        /// </summary>
        Image? PressedImage { set; }

        /// <summary>
        /// Sets image for the 'Disabled' state.
        /// </summary>
        Image? DisabledImage { set; }

        /// <summary>
        /// Sets image for the 'Focused' state.
        /// </summary>
        Image? FocusedImage { set; }

        /// <inheritdoc cref="Button.ExactFit"/>
        bool ExactFit { get; set; }

        /// <inheritdoc cref="Button.TextVisible"/>
        bool TextVisible { get; set; }

        /// <inheritdoc cref="Button.TextAlign"/>
        ElementContentAlign TextAlign { get; set; }

        /// <inheritdoc cref="Button.SetImagePosition"/>
        void SetImagePosition(ElementContentAlign dir);

        /// <inheritdoc cref="Button.SetImageMargins"/>
        void SetImageMargins(Coord x, Coord y);
    }
}
