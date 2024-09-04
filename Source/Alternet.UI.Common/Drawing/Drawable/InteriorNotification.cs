using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements mouse handling for the <see cref="InteriorDrawable"/>.
    /// </summary>
    public class InteriorNotification : ControlNotification
    {
        private readonly InteriorDrawable interior;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorNotification"/> class.
        /// </summary>
        /// <param name="interior">Interior drawable.</param>
        public InteriorNotification(InteriorDrawable interior)
        {
            this.interior = interior;
        }

        /// <inheritdoc/>
        public override void AfterMouseMove(Control sender, MouseEventArgs e)
        {
            var hitTests = interior.HitTests(sender.ScaleFactor, e.Location);
            App.LogIf(hitTests.ToString(), false);
        }

        /// <inheritdoc/>
        public override void AfterSetScrollBarInfo(
            Control sender,
            bool isVertical,
            ScrollBarInfo value)
        {
            if (isVertical)
                return;
            var prefix = isVertical ? "V: " : "H: ";
            var s = $"{prefix}{value}";
            LogUtils.LogAndToFile(s);
        }

        /// <inheritdoc/>
        public override void AfterScroll(Control sender, ScrollEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonDown(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonUp(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseWheel(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(Control sender)
        {
        }
    }
}
