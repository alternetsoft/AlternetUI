using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class SpeedButton
    {
        /// <summary>
        /// Specifies the kind of text displayed on the right side of the control.
        /// </summary>
        public enum RightSideElementKind
        {
            /// <summary>
            /// No right-side text is displayed.
            /// </summary>
            None,

            /// <summary>
            /// A key gesture (such as a keyboard shortcut) is displayed on the right side.
            /// </summary>
            KeyGesture,

            /// <summary>
            /// A formatted key gesture (such as a keyboard shortcut) is displayed on the right side.
            /// </summary>
            KeyGestureFormatted,

            /// <summary>
            /// A value of <see cref="RightSideText"/> property is displayed on the right side.
            /// </summary>
            Text,

            /// <summary>
            /// A value of <see cref="RightSideImage"/> property is displayed on the right side.
            /// </summary>
            Image,
        }

        /// <summary>
        /// Enumerates known color and style themes for the <see cref="SpeedButton"/>.
        /// </summary>
        public enum KnownTheme
        {
            /// <summary>
            /// An empty theme. Settings from <see cref="AbstractControl.StateObjects"/> are used.
            /// </summary>
            None,

            /// <summary>
            /// Theme <see cref="DefaultTheme"/> is used.
            /// </summary>
            Default,

            /// <summary>
            /// Theme <see cref="CustomTheme"/> is used.
            /// </summary>
            Custom,

            /// <summary>
            /// Theme <see cref="TabControlTheme"/> is used.
            /// </summary>
            TabControl,

            /// <summary>
            /// Theme <see cref="StaticBorderTheme"/> is used.
            /// </summary>
            StaticBorder,

            /// <summary>
            /// Theme <see cref="StaticBorderThemeNoHover"/> is used.
            /// </summary>
            StaticBorderNoHover,

            /// <summary>
            /// Theme <see cref="StickyBorderTheme"/> is used.
            /// </summary>
            StickyBorder,

            /// <summary>
            /// Theme <see cref="NoBorderTheme"/> is used.
            /// </summary>
            NoBorder,

            /// <summary>
            /// Theme <see cref="CheckBorderTheme"/> is used.
            /// </summary>
            CheckBorder,

            /// <summary>
            /// Theme <see cref="PushButtonTheme"/> is used.
            /// </summary>
            PushButton,

            /// <summary>
            /// Theme <see cref="PushButtonHoveredTheme"/> is used.
            /// </summary>
            PushButtonHovered,

            /// <summary>
            /// Theme <see cref="PushButtonPressedTheme"/> is used.
            /// </summary>
            PushButtonPressed,

            /// <summary>
            /// Theme <see cref="RoundBorderTheme"/> is used.
            /// </summary>
            RoundBorder,

            /// <summary>
            /// Theme <see cref="SquareCornersTheme"/> is used.
            /// </summary>
            SquareCorners,
        }
    }
}
