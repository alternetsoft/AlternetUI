using System;

namespace Alternet.UI
{
    internal class AnimationControlHandler : WxControlHandler
    {
        public new Native.AnimationControl NativeControl => (Native.AnimationControl)base.NativeControl;

        public new AnimationPlayer Control => (AnimationPlayer)base.Control;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.AnimationControl();
        }
    }
}