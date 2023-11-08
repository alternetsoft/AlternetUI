using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all <see cref="PropertyGrid"/> colors.
    /// </summary>
    public class PropertyGridColors : IPropertyGridColors
    {
        private static PropertyGridColors? colorSchemeCream;
        private static PropertyGridColors? colorSchemeCustom;
        private static PropertyGridColors? colorSchemeNet;
        private static PropertyGridColors? colorSchemeWhite;
        private static PropertyGridColors? colorSchemeBlack;

        private bool immutable;
        private Color? captionBackgroundColor;
        private Color? captionForegroundColor;
        private Color? cellBackgroundColor;
        private Color? cellDisabledTextColor;
        private Color? cellTextColor;
        private Color? emptySpaceColor;
        private Color? lineColor;
        private Color? marginColor;
        private Color? selectionBackgroundColor;
        private Color? selectionForegroundColor;
        private bool resetColors = true;

        /// <summary>
        /// Gets or sets <see cref="PropertyGridColors"/> for the Custom color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeCustom
        {
            get
            {
                colorSchemeCustom ??= new();
                return colorSchemeCustom;
            }

            set
            {
                colorSchemeCustom = value;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGridColors"/> for the White color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeWhite
        {
            get
            {
                colorSchemeWhite ??= new()
                {
                    CaptionBackgroundColor = Color.White,
                    CellBackgroundColor = Color.White,
                    MarginColor = Color.White,
                    CaptionForegroundColor = Color.Gray,
                    CellDisabledTextColor = Color.Gray,
                    CellTextColor = Color.Black,
                    EmptySpaceColor = Color.White,
                    LineColor = Color.FromArgb(200, 200, 200),
                    immutable = true,
                };

                return colorSchemeWhite;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGridColors"/> for the .Net color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeNet
        {
            get
            {
                Color my_grey_1 = Color.FromArgb(212, 208, 200);

                colorSchemeNet ??= new()
                {
                    CaptionBackgroundColor = my_grey_1,
                    LineColor = my_grey_1,
                    MarginColor = my_grey_1,
                    CaptionForegroundColor = Color.Gray,
                    CellBackgroundColor = Color.White,
                    CellDisabledTextColor = Color.Gray,
                    CellTextColor = Color.Black,
                    EmptySpaceColor = Color.White,
                    immutable = true,
                };

                return colorSchemeNet;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGridColors"/> for the Cream color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeCream
        {
            get
            {
                Color my_grey_1 = Color.FromArgb(212, 208, 200);
                Color my_grey_2 = Color.FromArgb(241, 239, 226);

                colorSchemeCream ??= new()
                {
                    CaptionBackgroundColor = my_grey_2,
                    CellBackgroundColor = my_grey_2,
                    EmptySpaceColor = my_grey_2,
                    LineColor = my_grey_1,
                    MarginColor = my_grey_2,
                    CaptionForegroundColor = Color.Gray,
                    CellDisabledTextColor = Color.Gray,
                    CellTextColor = Color.Black,
                    immutable = true,
                };

                return colorSchemeCream;
            }
        }

        /// <summary>
        /// Returns <see cref="PropertyGridColors"/> for the Black color scheme.
        /// </summary>
        /// <remarks>
        /// This is not fully works if application theme is not set to black.
        /// </remarks>
        public static PropertyGridColors ColorSchemeBlack
        {
            get
            {
                /*
                                Color color2 = Color.FromArgb(250, 250, 250);
                                Color color3 = Color.FromArgb(30, 30, 30);

                                colorSchemeBlack ??= new PropertyGridColors
                                {
                                    CaptionBackgroundColor = color3,
                                    CellBackgroundColor = color3,
                                    EmptySpaceColor = color3,
                                    MarginColor = color3,
                                    CaptionForegroundColor = color2,
                                    CellDisabledTextColor = Color.FromArgb(192, 192, 192),
                                    CellTextColor = color2,
                                    LineColor = Color.FromArgb(113, 113, 113),
                                    SelectionBackgroundColor = Color.FromArgb(38, 79, 120),
                                    SelectionForegroundColor = Color.FromArgb(220, 220, 220),
                                    immutable = true,
                                };
                */

                var color1 = Color.FromArgb(55, 55, 55);
                var color2 = Color.FromArgb(39, 39, 39);

                colorSchemeBlack ??= new PropertyGridColors
                {
                    CaptionBackgroundColor = color1,
                    CaptionForegroundColor = Color.White,
                    CellBackgroundColor = color2,
                    CellDisabledTextColor = Color.White,
                    CellTextColor = Color.White,
                    EmptySpaceColor = color2,
                    LineColor = color1,
                    MarginColor = color1,
/*                    SelectionBackgroundColor = Color.FromArgb(233, 84, 32),
                    SelectionForegroundColor = Color.White,*/
                    immutable = true,
                };

                return colorSchemeBlack;
            }
        }

        /// <summary>
        /// Gets or sets category caption background color.
        /// </summary>
        public Color? CaptionBackgroundColor
        {
            get => captionBackgroundColor;
            set
            {
                if (!immutable) captionBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets category caption text color.
        /// </summary>
        public Color? CaptionForegroundColor
        {
            get => captionForegroundColor;
            set
            {
                if (!immutable) captionForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets cell background color.
        /// </summary>
        public Color? CellBackgroundColor
        {
            get => cellBackgroundColor;
            set
            {
                if (!immutable) cellBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets cell text color when disabled.
        /// </summary>
        public Color? CellDisabledTextColor
        {
            get => cellDisabledTextColor;
            set
            {
                if (!immutable) cellDisabledTextColor = value;
            }
        }

        /// <summary>
        /// Gets or sets cell text color.
        /// </summary>
        public Color? CellTextColor
        {
            get => cellTextColor;
            set
            {
                if (!immutable) cellTextColor = value;
            }
        }

        /// <summary>
        /// Gets or sets color of empty space below properties.
        /// </summary>
        public Color? EmptySpaceColor
        {
            get => emptySpaceColor;
            set
            {
                if (!immutable) emptySpaceColor = value;
            }
        }

        /// <summary>
        /// Gets or sets color of lines between cells.
        /// </summary>
        public Color? LineColor
        {
            get => lineColor;
            set
            {
                if (!immutable) lineColor = value;
            }
        }

        /// <summary>
        /// Gets or sets background color of margin.
        /// </summary>
        public Color? MarginColor
        {
            get => marginColor;
            set
            {
                if (!immutable) marginColor = value;
            }
        }

        /// <summary>
        /// Gets or sets selection background color.
        /// </summary>
        public Color? SelectionBackgroundColor
        {
            get => selectionBackgroundColor;
            set => selectionBackgroundColor = value;
        }

        /// <summary>
        /// Gets or sets selection text color.
        /// </summary>
        public Color? SelectionForegroundColor
        {
            get => selectionForegroundColor;
            set
            {
                if (!immutable) selectionForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to reset all the colors to the default values before
        /// applying new colors.
        /// </summary>
        public bool ResetColors
        {
            get => resetColors;
            set
            {
                if (!immutable) resetColors = value;
            }
        }

        /// <summary>
        /// Creates known <see cref="IPropertyGridColors"/> instance.
        /// </summary>
        /// <param name="colors">Known <see cref="PropertyGrid"/> colors.</param>
        public static IPropertyGridColors CreateColors(PropertyGridKnownColors colors)
        {
            return colors switch
            {
                PropertyGridKnownColors.White => ColorSchemeWhite,
                PropertyGridKnownColors.Net => ColorSchemeNet,
                PropertyGridKnownColors.Cream => ColorSchemeCream,
                PropertyGridKnownColors.Black => ColorSchemeBlack,
                PropertyGridKnownColors.Custom => ColorSchemeCustom,
                _ => new PropertyGridColors(),
            };
        }

        /// <summary>
        ///  Logs <see cref="PropertyGridColors"/> to file.
        /// </summary>
        public void LogToFile()
        {
            LogUtils.LogColor("CaptionBackgroundColor", CaptionBackgroundColor);
            LogUtils.LogColor("CaptionForegroundColor", CaptionForegroundColor);
            LogUtils.LogColor("CellBackgroundColor", CellBackgroundColor);
            LogUtils.LogColor("CellDisabledTextColor", CellDisabledTextColor);
            LogUtils.LogColor("CellTextColor", CellTextColor);
            LogUtils.LogColor("EmptySpaceColor", EmptySpaceColor);
            LogUtils.LogColor("LineColor", LineColor);
            LogUtils.LogColor("MarginColor", MarginColor);
            LogUtils.LogColor("SelectionBackgroundColor", SelectionBackgroundColor);
            LogUtils.LogColor("SelectionForegroundColor", SelectionForegroundColor);
        }

        internal void ApplyDefaultWhiteScheme()
        {
            CaptionBackgroundColor = Color.FromArgb(200, 200, 200);
            CaptionForegroundColor = Color.Gray;
            CellBackgroundColor = Color.White;
            CellDisabledTextColor = Color.Gray;
            CellTextColor = Color.Black;
            EmptySpaceColor = Color.White;
            LineColor = Color.FromArgb(200, 200, 200);
            MarginColor = Color.FromArgb(200, 200, 200);
            SelectionBackgroundColor = Color.FromArgb(0, 120, 215);
            SelectionForegroundColor = Color.White;
        }
    }
}