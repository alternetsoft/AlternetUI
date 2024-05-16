using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class AuiToolbarHandler : ControlHandler<AuiToolbar>
    {
        public AuiToolbarHandler()
        {
        }

        public long CreateStyle
        {
            get
            {
                return NativeControl.CreateStyle;
            }

            set
            {
                NativeControl.CreateStyle = value;
            }
        }

        public new Native.AuiToolBar NativeControl =>
            (Native.AuiToolBar)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new NativeAuiToolBar(AuiToolbar.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.ToolCommand = null;
            NativeControl.ToolDropDown = null;
            NativeControl.BeginDrag = null;
            NativeControl.ToolMiddleClick = null;
            NativeControl.OverflowClick = null;
            NativeControl.ToolRightClick = null;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ToolCommand = NativeControl_ToolCommand;
            NativeControl.ToolDropDown = NativeControl_ToolDropDown;
            NativeControl.BeginDrag = NativeControl_BeginDrag;
            NativeControl.ToolMiddleClick = NativeControl_ToolMiddleClick;
            NativeControl.OverflowClick = NativeControl_OverflowClick;
            NativeControl.ToolRightClick = NativeControl_ToolRightClick;
        }

        private void NativeControl_ToolCommand()
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolCommand(EventArgs.Empty);
        }

        private void NativeControl_ToolRightClick()
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolRightClick(EventArgs.Empty);
        }

        private void NativeControl_OverflowClick()
        {
            Control.SetMouseCapture(false);
            Control.RaiseOverflowClick(EventArgs.Empty);
        }

        private void NativeControl_ToolMiddleClick()
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolMiddleClick(EventArgs.Empty);
        }

        private void NativeControl_BeginDrag()
        {
            Control.RaiseBeginDrag(EventArgs.Empty);
        }

        private void NativeControl_ToolDropDown()
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolDropDown(EventArgs.Empty);
        }

        public class NativeAuiToolBar : Native.AuiToolBar
        {
            public NativeAuiToolBar(AuiToolbarCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.AuiToolBar_CreateEx_((int)style));
            }
        }
    }
}