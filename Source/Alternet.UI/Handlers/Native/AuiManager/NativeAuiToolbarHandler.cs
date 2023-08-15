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
            NativeControl.ToolDropDown -= NativeControl_ToolDropDown;
            NativeControl.BeginDrag -= NativeControl_BeginDrag;
            NativeControl.ToolMiddleClick -= NativeControl_ToolMiddleClick;
            NativeControl.OverflowClick -= NativeControl_OverflowClick;
            NativeControl.ToolRightClick -= NativeControl_ToolRightClick;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ToolDropDown += NativeControl_ToolDropDown;
            NativeControl.BeginDrag += NativeControl_BeginDrag;
            NativeControl.ToolMiddleClick += NativeControl_ToolMiddleClick;
            NativeControl.OverflowClick += NativeControl_OverflowClick;
            NativeControl.ToolRightClick += NativeControl_ToolRightClick;
        }

        private void NativeControl_ToolRightClick(object sender, EventArgs e)
        {
        }

        private void NativeControl_OverflowClick(object sender, EventArgs e)
        {
        }

        private void NativeControl_ToolMiddleClick(object sender, EventArgs e)
        {
        }

        private void NativeControl_BeginDrag(object sender, EventArgs e)
        {
        }

        private void NativeControl_ToolDropDown(object sender, EventArgs e)
        {
            Control.RaiseToolDropDown(e);
        }
    }
}