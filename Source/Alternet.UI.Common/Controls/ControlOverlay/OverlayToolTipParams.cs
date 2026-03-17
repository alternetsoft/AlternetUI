using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the parameters for configuring an overlay tooltip.
    /// Extends <see cref="RichToolTipParams"/> to include additional
    /// configuration options for overlay tooltips, including
    /// options for automatic dismissal and dismissal timing.
    /// </summary>
    public class OverlayToolTipParams : BaseObjectWithAttr
    {
        private RichToolTipParams? toolTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayToolTipParams"/> class with the specified tooltip parameters.
        /// </summary>
        public OverlayToolTipParams(RichToolTipParams? toolTip)
        {
            this.toolTip = toolTip;
        }

        /// <summary>
        /// Initializes a new instance of the OverlayToolTipParams class.
        /// </summary>
        public OverlayToolTipParams()
        {
        }

        /// <summary>
        /// Gets or sets the parameters used to configure the appearance and behavior of the rich tool tip.
        /// </summary>
        public virtual RichToolTipParams ToolTipParams
        {
            get
            {
                return toolTip ??= new ();
            }

            set
            {
                toolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the tooltip.
        /// </summary>
        public string Text
        {
            get
            {
                return ToolTipParams.Text;
            }

            set
            {
                ToolTipParams.Text = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the font used to display the tooltip text.
        /// </summary>
        public Font? Font
        {
            get
            {
                return ToolTipParams.Font;
            }

            set
            {
                ToolTipParams.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial tooltip value.
        /// </summary>
        public virtual object? InitialToolTip { get; set; }

        /// <summary>
        /// Gets or sets the control for which tooltip is shown.
        /// </summary>
        public virtual AbstractControl? AssociatedControl { get; set; }

        /// <summary>
        /// Gets or sets the bounds of the container for the tooltip.
        /// </summary>
        public virtual RectD? ContainerBounds { get; set; }

        /// <summary>
        /// Gets or sets the location of the tooltip inside the control.
        /// </summary>
        public virtual PointD? LocationWithOffset
        {
            get
            {
                return LocationWithoutOffset.HasValue ? LocationWithoutOffset.Value + LocationOffset : null;
            }

            set
            {
                if (value.HasValue)
                    LocationWithoutOffset = value.Value - LocationOffset;
                else
                    LocationWithoutOffset = null;
            }
        }

        /// <summary>
        /// Gets or sets the offset which was additionally applied to the tooltip location.
        /// </summary>
        public virtual PointD LocationOffset { get; set; }

        /// <summary>
        /// Gets or sets the location of the tooltip without the offset applied.
        /// </summary>
        public PointD? LocationWithoutOffset { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment of the tooltip inside the control.
        /// </summary>
        public virtual VerticalAlignment? VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the tooltip inside the control.
        /// </summary>
        public virtual HorizontalAlignment? HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets a value indicating whether either vertical or horizontal alignment is specified.
        /// </summary>
        public bool HasAlignment =>
            VerticalAlignment.HasValue || HorizontalAlignment.HasValue;

        /// <summary>
        /// Gets or sets a value indicating whether the overlay should
        /// automatically dismiss after a specified interval.
        /// </summary>
        public bool DismissAfterInterval
        {
            get
            {
                return (Options & OverlayToolTipFlags.DismissAfterInterval) != 0;
            }

            set
            {
                if (value)
                    Options |= OverlayToolTipFlags.DismissAfterInterval;
                else
                    Options &= ~OverlayToolTipFlags.DismissAfterInterval;
            }
        }

        /// <summary>
        /// Gets or sets the options that determine the behavior of the overlay.
        /// </summary>
        public virtual OverlayToolTipFlags Options { get; set; }
            = OverlayToolTipFlags.DismissAfterInterval;

        /// <summary>
        /// Gets or sets the interval, in milliseconds, after which an overlay
        /// tooltip is automatically dismissed. Default is null. If value is not specified,
        /// the default dismiss interval is used.
        /// </summary>
        public virtual int? DismissInterval { get; set; }

        /// <summary>
        /// Gets or sets the title associated with this instance.
        /// </summary>
        public virtual string Title
        {
            get => ToolTipParams.Title;
            set
            {
                ToolTipParams.Title = value;
            }
        }

        /// <summary>
        /// Represents the maximum text width as a coordinate value. If text exceeds this width,
        /// it may be wrapped into multiple lines.
        /// </summary>
        /// <remarks>This field is nullable, meaning it can hold a value
        /// of type <see cref="Coord"/> or be
        /// null. Use this field to specify or retrieve the maximum title and message width.</remarks>
        public virtual Coord? MaxWidth
        {
            get => ToolTipParams.MaxWidth;
            set { ToolTipParams.MaxWidth = value; }
        }

        /// <summary>
        /// Represents the scale factor to be applied in calculations or transformations.
        /// </summary>
        public virtual Coord? ScaleFactor { get => ToolTipParams.ScaleFactor; set { ToolTipParams.ScaleFactor = value; } }

        /// <summary>
        /// Gets or sets the border settings for the element.
        /// </summary>
        public virtual BorderSettings? Border { get => ToolTipParams.Border; set { ToolTipParams.Border = value; } }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public virtual Color? BackgroundColor { get => ToolTipParams.BackgroundColor; set { ToolTipParams.BackgroundColor = value; } }

        /// <summary>
        /// Gets or sets the foreground color to be used for rendering.
        /// </summary>
        public virtual Color? ForegroundColor { get => ToolTipParams.ForegroundColor; set { ToolTipParams.ForegroundColor = value; } }

        /// <summary>
        /// Gets or sets the foreground color of the title.
        /// </summary>
        public virtual Color? TitleForegroundColor { get => ToolTipParams.TitleForegroundColor; set { ToolTipParams.TitleForegroundColor = value; } }

        /// <summary>
        /// Gets or sets the font used for rendering the title.
        /// </summary>
        public virtual Font? TitleFont { get => ToolTipParams.TitleFont; set { ToolTipParams.TitleFont = value; } }

        /// <summary>
        /// Gets or sets the icon to display in the tooltip.
        /// </summary>
        public virtual MessageBoxIcon? Icon { get => ToolTipParams.Icon; set { ToolTipParams.Icon = value; } }

        /// <summary>
        /// Gets or sets the brush used to paint the background.
        /// </summary>
        public virtual Brush? BackgroundBrush { get => ToolTipParams.BackgroundBrush; set { ToolTipParams.BackgroundBrush = value; } }

        /// <summary>
        /// Gets or sets an image to display in the tooltip.
        /// </summary>
        public virtual ImageSet? Image { get => ToolTipParams.Image; set { ToolTipParams.Image = value; } }

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
            ToolTipParams.UsesSystemColors();
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
            ToolTipParams.SetBorder(color, radiusIsPercent, cornerRadius);
        }
    }
}


/*
*/