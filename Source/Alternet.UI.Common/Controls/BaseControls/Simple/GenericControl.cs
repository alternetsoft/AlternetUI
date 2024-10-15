using System;
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
        protected override bool HasGenericPaint()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }
}
