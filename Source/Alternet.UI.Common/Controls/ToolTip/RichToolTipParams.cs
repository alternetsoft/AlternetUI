using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the parameters used to configure the appearance and content of a rich tooltip.
    /// </summary>
    /// <remarks>This structure provides a set of optional properties that allow customization
    /// of a tooltip's visual style, including colors, fonts, borders, and icons.
    /// Each property is nullable, enabling selective
    /// customization while falling back to default values for unspecified properties.</remarks>
    public class RichToolTipParams : BaseObjectWithAttr
    {
        /// <summary>
        /// Gets or sets the title associated with this instance.
        /// </summary>
        public virtual string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text content associated with this instance.
        /// </summary>
        public virtual string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font used for rendering text.
        /// </summary>
        public virtual Font? Font { get; set; }

        /// <summary>
        /// Represents the maximum text width as a coordinate value. If text exceeds this width,
        /// it may be wrapped into multiple lines.
        /// </summary>
        /// <remarks>This field is nullable, meaning it can hold a value
        /// of type <see cref="Coord"/> or be
        /// null. Use this field to specify or retrieve the maximum title and message width.</remarks>
        public virtual Coord? MaxWidth { get; set; }

        /// <summary>
        /// Represents the scale factor to be applied in calculations or transformations.
        /// </summary>
        public virtual Coord? ScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the border settings for the element.
        /// </summary>
        public virtual BorderSettings? Border { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public virtual Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color to be used for rendering.
        /// </summary>
        public virtual Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the title.
        /// </summary>
        public virtual Color? TitleForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font used for rendering the title.
        /// </summary>
        public virtual Font? TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the icon to display in the tooltip.
        /// </summary>
        public virtual MessageBoxIcon? Icon { get; set; }

        /// <summary>
        /// Gets or sets the brush used to paint the background.
        /// </summary>
        public virtual Brush? BackgroundBrush { get; set; }

        /// <summary>
        /// Gets or sets an image to display in the tooltip.
        /// </summary>
        public virtual ImageSet? Image { get; set; }

        /// <summary>
        /// Updates the current instance to use system-defined colors
        /// for background and foreground elements.
        /// </summary>
        /// <remarks>This method retrieves system color information and
        /// applies it to the instance's
        /// background,  foreground, and title foreground colors.
        /// It ensures consistency with the system's color scheme.</remarks>
        public virtual void UsesSystemColors()
        {
            var colors = FontAndColor.SystemColorInfo;
            BackgroundColor = colors.BackgroundColor;
            ForegroundColor = colors.ForegroundColor;
            TitleForegroundColor = colors.ForegroundColor;
        }

        /// <summary>
        /// Configures the border settings with the specified color, corner radius, and radius unit.
        /// </summary>
        /// <remarks>If the border settings are not already initialized,
        /// this method will create a new
        /// instance of the border configuration.</remarks>
        /// <param name="color">The color to apply to the border.</param>
        /// <param name="radiusIsPercent">A value indicating whether the corner radius
        /// is specified as a percentage of the element's size.
        /// <see langword="true"/> if the radius is a percentage;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="cornerRadius">The uniform corner radius to apply to the border.
        /// Must be a non-negative value.</param>
        public virtual void SetBorder(
            Color color,
            bool radiusIsPercent = true,
            double cornerRadius = 0)
        {
            if (Border is null)
                Border = new BorderSettings();
            Border.Color = color;
            Border.UniformCornerRadius = cornerRadius;
            Border.UniformRadiusIsPercent = radiusIsPercent;
        }
    }
}