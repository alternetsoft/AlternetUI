﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic control which is not handled by the operating system.
    /// </summary>
    public class GenericControl : AbstractControl
    {
        private bool isClipped = true;

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
        public override bool IsHandleCreated => true;

        /// <inheritdoc/>
        public override bool ReportBoundsChanged()
        {
            var result = base.ReportBoundsChanged();
            /*if (result)
                Invalidate();*/
            return result;
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public override void Invalidate()
        {
            Parent?.Invalidate();
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public override void Update()
        {
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            /*Invalidate();*/
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override bool HasGenericPaint()
        {
            return true;
        }
    }
}
