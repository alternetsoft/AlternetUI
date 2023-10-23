using System;

namespace Alternet.UI
{
    internal class NativeAnimationControlHandler : ControlHandler
    {
        public new Native.AnimationControl NativeControl => (Native.AnimationControl)base.NativeControl!;

        public new AnimationControl Control => (AnimationControl)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.AnimationControl();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
        }
    }
}