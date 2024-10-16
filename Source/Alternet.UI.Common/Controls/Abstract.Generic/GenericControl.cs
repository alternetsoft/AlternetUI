using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic control which is not handled by the operating system.
    /// </summary>
    public class GenericControl : AbstractControl
    {
        private bool isClipped = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericControl"/> class.
        /// </summary>
        public GenericControl()
        {
        }

        /// <inheritdoc/>
        public override bool IsHandleCreated => true;

        /// <summary>
        /// Gets or sets whether control contents is clipped and is not painted outside it's bounds.
        /// </summary>
        [Browsable(true)]
        public virtual bool IsClipped
        {
            get
            {
                return isClipped;
            }

            set
            {
                if (isClipped == value)
                    return;
                isClipped = value;
                Refresh();
            }
        }

        /// <inheritdoc/>
        public override bool ReportBoundsChanged()
        {
            var result = base.ReportBoundsChanged();
            return result;
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public override void Invalidate()
        {
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// Default painting method of the <see cref="GenericControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void DefaultPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }

        /// <inheritdoc/>
        protected sealed override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DoInsideClipped(
                e.ClipRectangle,
                () =>
                {
                    DefaultPaint(e);
                },
                IsClipped);
        }
    }
}
