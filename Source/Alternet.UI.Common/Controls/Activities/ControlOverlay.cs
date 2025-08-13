using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation for overlays that can be drawn on top of controls.
    /// </summary>
    public class ControlOverlay : BaseObjectWithId, IControlOverlay
    {
        /// <inheritdoc/>
        public virtual void OnPaint(AbstractControl control, PaintEventArgs e)
        {
        }
    }
}
