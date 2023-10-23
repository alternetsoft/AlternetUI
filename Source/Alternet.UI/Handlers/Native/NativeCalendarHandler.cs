using System;

namespace Alternet.UI
{
    internal class NativeCalendarHandler : ControlHandler
    {
        public new Native.Calendar NativeControl => (Native.Calendar)base.NativeControl!;

        public new Calendar Control => (Calendar)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Calendar();
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