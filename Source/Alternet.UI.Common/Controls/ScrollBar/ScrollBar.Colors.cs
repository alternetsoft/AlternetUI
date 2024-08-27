using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class ScrollBar
    {
        /// <summary>
        /// Enumerates known color and style themes for the <see cref="ScrollBar"/>.
        /// </summary>
        public enum KnownTheme
        {
            /// <summary>
            /// System color theme is used.
            /// </summary>
            System,

            /// <summary>
            /// Visual Studio Dark theme is used if control has dark background
            /// and Visual Studio Light theme is used if control has light background.
            /// </summary>
            VisualStudioAuto,

            /// <summary>
            /// Visual Studio Light theme is used.
            /// </summary>
            VisualStudioLight,

            /// <summary>
            /// Visual Studio Dark theme is used.
            /// </summary>
            VisualStudioDark,
        }

        /// <summary>
        /// Contains colors for the different scrollbar parts used in Visual Studio Dark theme.
        /// </summary>
        public static class VisualStudioDarkThemeColors
        {
            /// <summary>
            /// Gets or sets scrollbar background color used in Visual Studio Dark theme.
            /// </summary>
            public static Color Background = (62, 62, 66);

            /// <summary>
            /// Gets or sets scrollbar arrow color used in Visual Studio Dark theme.
            /// </summary>
            public static Color Arrow = (153, 153, 153);

            /// <summary>
            /// Gets or sets scrollbar arrow color in the hovered state used in Visual Studio Dark theme.
            /// </summary>
            public static Color ArrowHovered = (28, 151, 234);

            /// <summary>
            /// Gets or sets scrollbar thumb background color used in Visual Studio Dark theme.
            /// </summary>
            public static Color ThumbBackground = (0, 0, 0);

            /// <summary>
            /// Gets or sets scrollbar thumb border color used in Visual Studio Dark theme.
            /// </summary>
            public static Color ThumbBorder = (104, 104, 104);

            /// <summary>
            /// Gets Visual Studio light hheme colors as <see cref="ThemeColors"/>.
            /// </summary>
            /// <returns></returns>
            public static ThemeColors GetColors()
            {
                ThemeColors result = new()
                {
                    Background = VisualStudioDarkThemeColors.Background,
                    Arrow = VisualStudioDarkThemeColors.Arrow,
                    ArrowHovered = VisualStudioDarkThemeColors.ArrowHovered,
                    ThumbBackground = VisualStudioDarkThemeColors.ThumbBackground,
                    ThumbBorder = VisualStudioDarkThemeColors.ThumbBorder,
                };

                return result;
            }
        }

        /// <summary>
        /// Contains colors for the different scrollbar part used in Visual Studio Light theme.
        /// </summary>
        public static class VisualStudioLightThemeColors
        {
            /// <summary>
            /// Gets or sets scrollbar background color used in Visual Studio Light theme.
            /// </summary>
            public static Color Background = (245, 245, 245);

            /// <summary>
            /// Gets or sets scrollbar arrow color used in Visual Studio Light theme.
            /// </summary>
            public static Color Arrow = (134, 137, 153);

            /// <summary>
            /// Gets or sets scrollbar arrow color in the hovered state used in Visual Studio Light theme.
            /// </summary>
            public static Color ArrowHovered = (28, 151, 234);

            /// <summary>
            /// Gets or sets scrollbar thumb background color used in Visual Studio Light theme.
            /// </summary>
            public static Color ThumbBackground = (255, 255, 255);

            /// <summary>
            /// Gets or sets scrollbar thumb border color used in Visual Studio Light theme.
            /// </summary>
            public static Color ThumbBorder = (0, 0, 0);

            /// <summary>
            /// Gets Visual Studio light hheme colors as <see cref="ThemeColors"/>.
            /// </summary>
            /// <returns></returns>
            public static ThemeColors GetColors()
            {
                ThemeColors result = new()
                {
                    Background = VisualStudioLightThemeColors.Background,
                    Arrow = VisualStudioLightThemeColors.Arrow,
                    ArrowHovered = VisualStudioLightThemeColors.ArrowHovered,
                    ThumbBackground = VisualStudioLightThemeColors.ThumbBackground,
                    ThumbBorder = VisualStudioLightThemeColors.ThumbBorder,
                };

                return result;
            }
        }

        /// <summary>
        /// Contains colors for the different scrollbar parts.
        /// </summary>
        public class ThemeColors
        {
            /// <summary>
            /// Gets or sets scrollbar background color.
            /// </summary>
            public Color Background = Color.Empty;

            /// <summary>
            /// Gets or sets scrollbar arrow color.
            /// </summary>
            public Color Arrow = Color.Empty;

            /// <summary>
            /// Gets or sets scrollbar arrow color in the hovered state.
            /// </summary>
            public Color ArrowHovered = Color.Empty;

            /// <summary>
            /// Gets or sets scrollbar thumb background color.
            /// </summary>
            public Color ThumbBackground = Color.Empty;

            /// <summary>
            /// Gets or sets scrollbar thumb border color.
            /// </summary>
            public Color ThumbBorder = Color.Empty;
        }
    }
}