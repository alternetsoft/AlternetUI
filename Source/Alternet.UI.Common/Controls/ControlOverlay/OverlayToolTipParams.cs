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
    public class OverlayToolTipParams : RichToolTipParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayToolTipParams"/> class.
        /// </summary>
        public OverlayToolTipParams()
        {
        }

        /// <summary>
        /// Gets or sets the initial tooltip value.
        /// </summary>
        public virtual object? ToolTip { get; set; }

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
        public virtual PointD? Location { get; set; }

        /// <summary>
        /// Gets or sets the offset which was additionally applied to the tooltip location.
        /// </summary>
        public virtual PointD LocationOffset { get; set; }

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
    }
}
