using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// This is a static control which displays an animation.
    /// This control is useful to display a (small) animation while doing
    /// a long task (e.g. a "throbber").
    /// </summary>
    /// <remarks>
    /// <see cref="AnimationControl"/> API is as simple as possible and won't give you full
    /// control on the animation; if you need it then use other controls.
    /// </remarks>
    /// <remarks>
    /// For the platforms where this control has a native implementation, it
    /// may have only limited support for the animation types. Set UseGeneric if you need to
    /// support all of them.
    /// </remarks>
    public class AnimationControl : Control
    {
        [Browsable(false)]
        internal new NativeAnimationControlHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAnimationControlHandler)base.Handler;
            }
        }

        internal Native.AnimationControl NativeControl => Handler.NativeControl;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeAnimationControlHandler();
        }
    }
}