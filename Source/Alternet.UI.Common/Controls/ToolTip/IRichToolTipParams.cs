using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a set of parameters for configuring the appearance and content of a rich tooltip, including text,
    /// colors, fonts, icons, and layout options.
    /// </summary>
    /// <remarks>Implementations of this interface allow customization of tooltip presentation, such as
    /// specifying maximum text width, applying system color schemes, and configuring border and image settings. This
    /// interface is intended for scenarios where advanced tooltip customization is required beyond standard tooltip
    /// functionality.</remarks>
    public interface IRichToolTipParams
    {
        /// <summary>
        /// Gets or sets the title associated with this instance.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the text content associated with this instance.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the font used for rendering text.
        /// </summary>
        Font? Font { get; set; }

        /// <summary>
        /// Represents the maximum text width as a coordinate value. If text exceeds this width,
        /// it may be wrapped into multiple lines.
        /// </summary>
        /// <remarks>This field is nullable, meaning it can hold a value
        /// of type <see cref="Coord"/> or be
        /// null. Use this field to specify or retrieve the maximum title and message width.</remarks>
        Coord? MaxWidth { get; set; }

        /// <summary>
        /// Represents the scale factor to be applied in calculations or transformations.
        /// </summary>
        Coord? ScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the border settings for the element.
        /// </summary>
        BorderSettings? Border { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color to be used for rendering.
        /// </summary>
        Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the title.
        /// </summary>
        Color? TitleForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font used for rendering the title.
        /// </summary>
        Font? TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the icon to display in the tooltip.
        /// </summary>
        MessageBoxIcon? Icon { get; set; }

        /// <summary>
        /// Gets or sets the brush used to paint the background.
        /// </summary>
        Brush? BackgroundBrush { get; set; }

        /// <summary>
        /// Gets or sets an image to display in the tooltip.
        /// </summary>
        ImageSet? Image { get; set; }

        /// <summary>
        /// Updates the current instance to use system-defined colors
        /// for background and foreground elements.
        /// </summary>
        /// <remarks>This method retrieves system color information and
        /// applies it to the instance's
        /// background,  foreground, and title foreground colors.
        /// It ensures consistency with the system's color scheme.</remarks>
        void UsesSystemColors();

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
        void SetBorder(Color color, bool radiusIsPercent = true, Coord cornerRadius = 0);
    }
}
