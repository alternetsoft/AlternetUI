using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of 12 background colors for light and dark themes, allowing for easy retrieval
    /// of colors based on the current theme and specified background style. <see cref="LightTheme"/> background
    /// colors look good with black text color, while <see cref="DarkTheme"/> background colors look good with white text color.
    /// <see cref="LightTheme"/> and <see cref="DarkTheme"/> background colors are variations of the same base colors,
    /// but adjusted for light and dark themes, respectively. The colors are designed to be visually distinct
    /// and provide a good contrast for text readability.
    /// </summary>
    public static class LightDarkBackColors
    {
        /// <summary>
        /// Defines the kinds of background styles available in the light and dark themes.
        /// </summary>
        public enum BackStyleKind
        {
            /// <summary>
            /// Represents the red background style.
            /// </summary>
            Red,

            /// <summary>
            /// Represents the orange background style.
            /// </summary>
            Orange,

            /// <summary>
            /// Represents the yellow background style.
            /// </summary>
            Yellow,

            /// <summary>
            /// Represents the green background style.
            /// </summary>
            Green,

            /// <summary>
            /// Represents the teal background style.
            /// </summary>
            Teal,

            /// <summary>
            /// Represents the cyan background style.
            /// </summary>
            Cyan,

            /// <summary>
            /// Represents the blue background style.
            /// </summary>
            Blue,

            /// <summary>
            /// Represents the pink background style.
            /// </summary>
            Pink,

            /// <summary>
            /// Represents the brown background style.
            /// </summary>
            Brown,

            /// <summary>
            /// Represents the gray background style.
            /// </summary>
            Gray,

            /// <summary>
            /// Represents the indigo background style.
            /// </summary>
            Indigo,

            /// <summary>
            /// Represents the violet background style.
            /// </summary>
            Violet,
        }

        /// <summary>
        /// Gets the colors for all background styles based on the specified theme (light or dark).
        /// </summary>
        /// <param name="isDark">Indicates whether the dark theme is active.</param>
        /// <returns>An enumerable of tuples containing the background style kind and its corresponding color.</returns>
        public static IEnumerable<(BackStyleKind Kind, Color Color)> GetColors(bool isDark)
        {
            foreach (BackStyleKind kind in Enum.GetValues<BackStyleKind>())
            {
                yield return (kind, GetColor(isDark, kind));
            }
        }

        /// <summary>
        /// Gets the color for the specified background style based on the current theme (light or dark).
        /// </summary>
        /// <param name="isDark">Indicates whether the dark theme is active.</param>
        /// <param name="kind">The kind of background style.</param>
        /// <returns>The color for the specified background style and theme.</returns>
        public static Color GetColor(bool isDark, BackStyleKind kind)
        {
            if (isDark)
            {
                return DarkTheme.GetColor(kind);
            }
            else
            {
                return LightTheme.GetColor(kind);
            }
        }

        /// <summary>
        /// Defines the color values for the dark theme background styles.
        /// </summary>
        public static class DarkTheme
        {
            /// <summary>
            /// Gets the color for the specified red background style in the dark theme.
            /// </summary>
            public static readonly Color Red = Color.FromArgb(192, 57, 43);

            /// <summary>
            /// Gets the color for the specified orange background style in the dark theme.
            /// </summary>
            public static readonly Color Orange = Color.FromArgb(211, 84, 0);

            /// <summary>
            /// Gets the color for the specified yellow background style in the dark theme.
            /// </summary>
            public static readonly Color Yellow = Color.FromArgb(183, 149, 11);

            /// <summary>
            /// Gets the color for the specified green background style in the dark theme.
            /// </summary>
            public static readonly Color Green = Color.FromArgb(30, 132, 73);

            /// <summary>
            /// Gets the color for the specified teal background style in the dark theme.
            /// </summary>
            public static readonly Color Teal = Color.FromArgb(17, 122, 101);

            /// <summary>
            /// Gets the color for the specified cyan background style in the dark theme.
            /// </summary>
            public static readonly Color Cyan = Color.FromArgb(20, 143, 119);

            /// <summary>
            /// Gets the color for the specified blue background style in the dark theme.
            /// </summary>
            public static readonly Color Blue = Color.FromArgb(46, 134, 193);

            /// <summary>
            /// Gets the color for the specified pink background style in the dark theme.
            /// </summary>
            public static readonly Color Pink = Color.FromArgb(169, 50, 38);

            /// <summary>
            /// Gets the color for the specified brown background style in the dark theme.
            /// </summary>
            public static readonly Color Brown = Color.FromArgb(110, 44, 0);

            /// <summary>
            /// Gets the color for the specified gray background style in the dark theme.
            /// </summary>
            public static readonly Color Gray = Color.FromArgb(86, 101, 115);

            /// <summary>
            /// Gets the color for the specified indigo background style in the dark theme.
            /// </summary>
            public static readonly Color Indigo = Color.FromArgb(48, 63, 159);

            /// <summary>
            /// Gets the color for the specified violet background style in the dark theme.
            /// </summary>
            public static readonly Color Violet = Color.FromArgb(142, 36, 170);

            /// <summary>
            /// Gets the color for the specified <see cref="BackStyleKind"/> in the dark theme.
            /// </summary>
            /// <param name="kind">The kind of background color.</param>
            /// <returns>The corresponding <see cref="Color"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified
            /// <paramref name="kind"/> is not recognized.</exception>
            public static Color GetColor(BackStyleKind kind)
            {
                return kind switch
                {
                    BackStyleKind.Red => Red,
                    BackStyleKind.Orange => Orange,
                    BackStyleKind.Yellow => Yellow,
                    BackStyleKind.Green => Green,
                    BackStyleKind.Teal => Teal,
                    BackStyleKind.Cyan => Cyan,
                    BackStyleKind.Blue => Blue,
                    BackStyleKind.Pink => Pink,
                    BackStyleKind.Brown => Brown,
                    BackStyleKind.Gray => Gray,
                    BackStyleKind.Indigo => Indigo,
                    BackStyleKind.Violet => Violet,
                    _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
                };
            }
        }

        /// <summary>
        /// Defines the color values for the light theme background styles.
        /// </summary>
        public static class LightTheme
        {
            /// <summary>
            /// Gets the color for the specified red background style in the light theme.
            /// </summary>
            public static readonly Color Red = Color.FromArgb(245, 183, 177);

            /// <summary>
            /// Gets the color for the specified orange background style in the light theme.
            /// </summary>
            public static readonly Color Orange = Color.FromArgb(250, 215, 160);

            /// <summary>
            /// Gets the color for the specified yellow background style in the light theme.
            /// </summary>
            public static readonly Color Yellow = Color.FromArgb(249, 231, 159);

            /// <summary>
            /// Gets the color for the specified green background style in the light theme.
            /// </summary>
            public static readonly Color Green = Color.FromArgb(171, 235, 198);

            /// <summary>
            /// Gets the color for the specified teal background style in the light theme.
            /// </summary>
            public static readonly Color Teal = Color.FromArgb(163, 228, 215);

            /// <summary>
            /// Gets the color for the specified cyan background style in the light theme.
            /// </summary>
            public static readonly Color Cyan = Color.FromArgb(118, 215, 196);

            /// <summary>
            /// Gets the color for the specified blue background style in the light theme.
            /// </summary>
            public static readonly Color Blue = Color.FromArgb(174, 214, 241);

            /// <summary>
            /// Gets the color for the specified pink background style in the light theme.
            /// </summary>
            public static readonly Color Pink = Color.FromArgb(245, 203, 167);

            /// <summary>
            /// Gets the color for the specified brown background style in the light theme.
            /// </summary>
            public static readonly Color Brown = Color.FromArgb(215, 189, 226);

            /// <summary>
            /// Gets the color for the specified gray background style in the light theme.
            /// </summary>
            public static readonly Color Gray = Color.FromArgb(213, 219, 219);

            /// <summary>
            /// Gets the color for the specified violet background style in the light theme.
            /// </summary>
            public static readonly Color Violet = Color.FromArgb(232, 179, 230);

            /// <summary>
            /// Gets the color for the specified indigo background style in the light theme.
            /// </summary>
            public static readonly Color Indigo = Color.FromArgb(169, 196, 235);

            /// <summary>
            /// Gets the color for the specified <see cref="BackStyleKind"/> in the light theme.
            /// </summary>
            /// <param name="kind">The kind of background color.</param>
            /// <returns>The corresponding <see cref="Color"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified
            /// <paramref name="kind"/> is not recognized.</exception>
            public static Color GetColor(BackStyleKind kind)
            {
                return kind switch
                {
                    BackStyleKind.Red => Red,
                    BackStyleKind.Orange => Orange,
                    BackStyleKind.Yellow => Yellow,
                    BackStyleKind.Green => Green,
                    BackStyleKind.Teal => Teal,
                    BackStyleKind.Cyan => Cyan,
                    BackStyleKind.Blue => Blue,
                    BackStyleKind.Pink => Pink,
                    BackStyleKind.Brown => Brown,
                    BackStyleKind.Gray => Gray,
                    BackStyleKind.Indigo => Indigo,
                    BackStyleKind.Violet => Violet,
                    _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
                };
            }
        }
    }
}
