using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeAuiToolbarHandler : ControlHandler<AuiToolbar>
    {
        public NativeAuiToolbarHandler()
            : base()
        {
        }

        public new Native.AuiToolBar NativeControl =>
            (Native.AuiToolBar)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.AuiToolBar();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }
    }
}