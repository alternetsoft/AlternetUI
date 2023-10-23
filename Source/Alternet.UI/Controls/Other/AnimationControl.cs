using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AnimationControl : Control
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

        protected override ControlHandler CreateHandler()
        {
            return new NativeAnimationControlHandler();
        }
    }
}
