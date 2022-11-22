using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Slider"/> behavior and appearance.
    /// </summary>
    public abstract class SliderHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new Slider Control => (Slider)base.Control;

        /// <summary>
        /// Gets or sets a value indicating the horizontal or vertical orientation of the slider.
        /// </summary>
        public abstract SliderOrientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how to display the tick marks on the slider.
        /// </summary>
        public abstract SliderTickStyle TickStyle { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}