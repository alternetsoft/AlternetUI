using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseCaptured
        {
            get => PlessMouse.MouseTargetControlOverride == this;

            set
            {
                if (value == IsMouseCaptured)
                    return;

                if (value)
                {
                    CaptureMouse();
                }
                else
                {
                    ReleaseMouseCapture();
                }
            }
        }

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
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        [Browsable(false)]
        public virtual void ReleaseMouseCapture()
        {
            if (PlessMouse.MouseTargetControlOverride == this)
                PlessMouse.MouseTargetControlOverride = null;
        }
    }
}
