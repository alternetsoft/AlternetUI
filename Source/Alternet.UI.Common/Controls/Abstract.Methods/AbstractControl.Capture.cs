using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Sets or releases mouse capture.
        /// </summary>
        /// <param name="value"><c>true</c> to set mouse capture; <c>false</c> to release it.</param>
        public void SetMouseCapture(bool value)
        {
            if (value)
                CaptureMouse();
            else
                ReleaseMouseCapture();
        }

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        [Browsable(false)]
        public virtual void CaptureMouse()
        {
            PlessMouse.MouseTargetControlOverride = this;

            if (IsPlatformControl)
            {
                (this as Control)?.SafeHandler?.CaptureMouse();
            }
            else
            {
                PlatformBackedParent?.SafeHandler?.CaptureMouse();
            }
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        [Browsable(false)]
        public virtual void ReleaseMouseCapture()
        {
            PlessMouse.MouseTargetControlOverride = null;
            if (IsPlatformControl)
            {
                (this as Control)?.SafeHandler?.ReleaseMouseCapture();
            }
            else
            {
                PlatformBackedParent?.SafeHandler?.ReleaseMouseCapture();
            }
        }
    }
}
