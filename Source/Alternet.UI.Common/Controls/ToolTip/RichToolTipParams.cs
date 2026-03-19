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
    public partial class RichToolTipParams : BaseObjectWithAttr, IRichToolTipParams
    {
        private Record record;

        /// <summary>
        /// Gets or sets the title associated with this instance.
        /// </summary>
        public virtual string Title { get => record.Title; set => record.Title = value; }

        /// <summary>
        /// Gets or sets the text content associated with this instance.
        /// </summary>
        public virtual string Text { get => record.Text; set => record.Text = value; }

        /// <summary>
        /// Gets or sets the font used for rendering text.
        /// </summary>
        public virtual Font? Font { get => record.Font; set => record.Font = value; }

        /// <summary>
        /// Represents the maximum text width as a coordinate value. If text exceeds this width,
        /// it may be wrapped into multiple lines.
        /// </summary>
        /// <remarks>This field is nullable, meaning it can hold a value
        /// of type <see cref="Coord"/> or be
        /// null. Use this field to specify or retrieve the maximum title and message width.</remarks>
        public virtual Coord? MaxWidth { get => record.MaxWidth; set => record.MaxWidth = value; }

        /// <summary>
        /// Represents the scale factor to be applied in calculations or transformations.
        /// </summary>
        public virtual Coord? ScaleFactor { get => record.ScaleFactor; set => record.ScaleFactor = value; }

        /// <summary>
        /// Gets or sets the border settings for the element.
        /// </summary>
        public virtual BorderSettings? Border { get => record.Border; set => record.Border = value; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public virtual Color? BackgroundColor { get => record.BackgroundColor; set => record.BackgroundColor = value; }

        /// <summary>
        /// Gets or sets the foreground color to be used for rendering.
        /// </summary>
        public virtual Color? ForegroundColor { get => record.ForegroundColor; set => record.ForegroundColor = value; }

        /// <summary>
        /// Gets or sets the foreground color of the title.
        /// </summary>
        public virtual Color? TitleForegroundColor { get => record.TitleForegroundColor; set => record.TitleForegroundColor = value; }

        /// <summary>
        /// Gets or sets the font used for rendering the title.
        /// </summary>
        public virtual Font? TitleFont { get => record.TitleFont; set => record.TitleFont = value; }

        /// <summary>
        /// Gets or sets the icon to display in the tooltip.
        /// </summary>
        public virtual MessageBoxIcon? Icon { get => record.Icon; set => record.Icon = value; }

        /// <summary>
        /// Gets or sets the brush used to paint the background.
        /// </summary>
        public virtual Brush? BackgroundBrush { get => record.BackgroundBrush; set => record.BackgroundBrush = value; }

        /// <summary>
        /// Gets or sets an image to display in the tooltip.
        /// </summary>
        public virtual ImageSet? Image { get => record.Image; set => record.Image = value; }

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
            Coord cornerRadius = 0)
        {
            if (Border is null)
                Border = new BorderSettings();
            Border.Color = color;
            Border.UniformCornerRadius = cornerRadius;
            Border.UniformRadiusIsPercent = radiusIsPercent;
        }

        /// <summary>
        /// Resets the current state to its initial value.
        /// </summary>
        /// <remarks>Call this method to clear any existing data and restore the object to its default
        /// state. This is typically used to reuse the instance for a new operation or data set.</remarks>
        public virtual void Reset()
        {
            record = new ();
        }

        /// <summary>
        /// Creates a new copy of the current RichToolTipParams instance.
        /// </summary>
        /// <remarks>The returned object is a separate instance with property values copied from the
        /// original. Changes to the clone do not affect the original object.</remarks>
        /// <returns>A new RichToolTipParams object with the same values as the current instance.</returns>
        public virtual RichToolTipParams Clone()
        {
            var clone = new RichToolTipParams();
            clone.Assign(this);
            return clone;
        }

        /// <summary>
        /// Copies the values from the specified source parameters into the current instance.
        /// </summary>
        /// <param name="source">The source <see cref="RichToolTipParams"/> instance from which to copy values. If <see langword="null"/>,
        /// the current instance is reset to default values.</param>
        public virtual void Assign(RichToolTipParams? source)
        {
            if (source == null)
                Reset();
            else
                record = source.record;
        }

        internal struct Record
        {
            public Record()
            {
            }

            public string Title = string.Empty;
            public string Text = string.Empty;
            public Font? Font;
            public Coord? MaxWidth;
            public Coord? ScaleFactor;
            public BorderSettings? Border;
            public Color? BackgroundColor;
            public Color? ForegroundColor;
            public Color? TitleForegroundColor;
            public Font? TitleFont;
            public MessageBoxIcon? Icon;
            public Brush? BackgroundBrush;
            public ImageSet? Image;
        }
    }
}