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
    public class PropertyGridColors
    {
        /// <summary>
        /// Returns <see cref="PropertyGridColors"/> for the White color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeWhite
        {
            get
            {
                var result = new PropertyGridColors();

                result.CaptionBackgroundColor = Color.White;
                result.CellBackgroundColor = Color.White;
                result.MarginColor = Color.White;

                return result;
            }
        }

        /// <summary>
        /// Returns <see cref="PropertyGridColors"/> for the .Net color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeNet
        {
            get
            {
                var result = new PropertyGridColors();
                Color my_grey_1 = Color.FromArgb(212, 208, 200);

                result.CaptionBackgroundColor = my_grey_1;
                result.LineColor = my_grey_1;
                result.MarginColor = my_grey_1;

                return result;
            }
        }

        /// <summary>
        /// Returns <see cref="PropertyGridColors"/> for the Cream color scheme.
        /// </summary>
        public static PropertyGridColors ColorSchemeCream
        {
            get
            {
                var result = new PropertyGridColors();

                Color my_grey_1 = Color.FromArgb(212, 208, 200);
                Color my_grey_2 = Color.FromArgb(241, 239, 226);

                result.CaptionBackgroundColor = my_grey_2;
                result.CellBackgroundColor = my_grey_2;
                result.EmptySpaceColor = my_grey_2;
                result.LineColor = my_grey_1;
                result.MarginColor = my_grey_2;

                return result;
            }
        }

        /// <summary>
        /// Gets or sets category caption background color.
        /// </summary>
        public Color? CaptionBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets category caption text color.
        /// </summary>
        public Color? CaptionForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets cell background color.
        /// </summary>
        public Color? CellBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets cell text color when disabled.
        /// </summary>
        public Color? CellDisabledTextColor { get; set; }

        /// <summary>
        /// Gets or sets cell text color.
        /// </summary>
        public Color? CellTextColor { get; set; }

        /// <summary>
        /// Gets or sets color of empty space below properties.
        /// </summary>
        public Color? EmptySpaceColor { get; set; }

        /// <summary>
        /// Gets or sets color of lines between cells.
        /// </summary>
        public Color? LineColor { get; set; }

        /// <summary>
        /// Gets or sets background color of margin.
        /// </summary>
        public Color? MarginColor { get; set; }

        /// <summary>
        /// Gets or sets selection background color.
        /// </summary>
        public Color? SelectionBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets selection text color.
        /// </summary>
        public Color? SelectionForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to reset all the colors to the default values before
        /// applying new colors.
        /// </summary>
        public bool ResetColors { get; set; } = true;
    }
}