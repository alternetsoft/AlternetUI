using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="PaintEventHandler"/> for different control states.
    /// </summary>
    public class ControlStatePaintActions : ControlStateObjects<PaintEventHandler>
    {
        /// <summary>
        /// Gets <see cref="ControlStatePaintActions"/> with empty state images.
        /// </summary>
        public static readonly ControlStatePaintActions Empty;

        static ControlStatePaintActions()
        {
            Empty = new();
            Empty.SetImmutable();
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStatePaintActions Clone()
        {
            var result = new ControlStatePaintActions();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }

        /// <summary>
        /// Sets background paint actions to draw push button background.
        /// <see cref="AbstractControl.DrawDefaultBackground"/> uses
        /// this information when it paints background of the control.
        /// </summary>
        public void SetAsPushButton()
        {
            Normal = DrawingUtils.DrawPushButtonNormal;
            Hovered = DrawingUtils.DrawPushButtonHovered;
            Pressed = DrawingUtils.DrawPushButtonPressed;
            Disabled = DrawingUtils.DrawPushButtonDisabled;
            Focused = DrawingUtils.DrawPushButtonFocused;
        }
    }
}
