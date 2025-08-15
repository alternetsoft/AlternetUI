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
        /// Defines a set of flags that specify options or behaviors for the overlay.
        /// </summary>
        /// <remarks>This enumeration supports bitwise combination of its member
        /// values using the bitwise OR operator. Use the <see cref="None"/> value
        /// to represent the absence of any options.</remarks>
        [Flags]
        public enum Flags
        {
            /// <summary>
            /// Represents the absence of any specific value or option.
            /// </summary>
            None = 0,

            /// <summary>
            /// Specifies that the overlay should be automatically dismissed
            /// after a predefined time interval.
            /// </summary>
            DismissAfterInterval = 1 << 0,

            UseSystemColors = 1 << 1,
        }

        /// <summary>
        /// Gets or sets the bounds of the container for the tooltip.
        /// </summary>
        public virtual RectD? ContainerBounds { get; set; }

        /// <summary>
        /// Gets or sets the location of the tooltip inside the control.
        /// </summary>
        public virtual PointD? Location { get; set; }

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
                return (Options & Flags.DismissAfterInterval) != 0;
            }

            set
            {
                if (value)
                    Options |= Flags.DismissAfterInterval;
                else
                    Options &= ~Flags.DismissAfterInterval;
            }
        }

        /// <summary>
        /// Gets or sets the options that determine the behavior of the overlay.
        /// </summary>
        public virtual Flags Options { get; set; } = Flags.DismissAfterInterval;

        /// <summary>
        /// Gets or sets the interval, in milliseconds, after which an overlay
        /// tooltip is automatically dismissed. Default is null. If value is not specified,
        /// the default dismiss interval is used.
        /// </summary>
        public virtual int? DismissInterval { get; set; }
    }
}
