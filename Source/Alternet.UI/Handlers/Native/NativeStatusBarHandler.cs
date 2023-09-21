using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeStatusBarHandler : StatusBarHandler
    {
        public new Native.StatusBar NativeControl => (Native.StatusBar)base.NativeControl!;

        public override bool SizingGripVisible
        {
            get => NativeControl.SizingGripVisible;
            set => NativeControl.SizingGripVisible = value;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.StatusBar();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            NativeControl.ControlRecreated -= NativeControl_ControlRecreated;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.ApplyPanels();

            NativeControl.ControlRecreated += NativeControl_ControlRecreated;
        }

        private void NativeControl_ControlRecreated(object sender, EventArgs e)
        {
            Control.ApplyPanels();
        }
    }
}