using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeAuiToolbarHandler : ControlHandler<AuiToolbar>
    {
        public NativeAuiToolbarHandler()
            : base()
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

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public new Native.AuiToolBar NativeControl =>
            (Native.AuiToolBar)base.NativeControl!;

        public override void OnLayout()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeAuiToolBar(AuiToolbar.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.ToolCommand -= NativeControl_ToolCommand;
            NativeControl.ToolDropDown -= NativeControl_ToolDropDown;
            NativeControl.BeginDrag -= NativeControl_BeginDrag;
            NativeControl.ToolMiddleClick -= NativeControl_ToolMiddleClick;
            NativeControl.OverflowClick -= NativeControl_OverflowClick;
            NativeControl.ToolRightClick -= NativeControl_ToolRightClick;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ToolCommand += NativeControl_ToolCommand;
            NativeControl.ToolDropDown += NativeControl_ToolDropDown;
            NativeControl.BeginDrag += NativeControl_BeginDrag;
            NativeControl.ToolMiddleClick += NativeControl_ToolMiddleClick;
            NativeControl.OverflowClick += NativeControl_OverflowClick;
            NativeControl.ToolRightClick += NativeControl_ToolRightClick;
        }

        private void NativeControl_ToolCommand(object? sender, EventArgs e)
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolCommand(e);
        }

        private void NativeControl_ToolRightClick(object? sender, EventArgs e)
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolRightClick(e);
        }

        private void NativeControl_OverflowClick(object? sender, EventArgs e)
        {
            Control.SetMouseCapture(false);
            Control.RaiseOverflowClick(e);
        }

        private void NativeControl_ToolMiddleClick(object? sender, EventArgs e)
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolMiddleClick(e);
        }

        private void NativeControl_BeginDrag(object? sender, EventArgs e)
        {
            Control.RaiseBeginDrag(e);
        }

        private void NativeControl_ToolDropDown(object? sender, EventArgs e)
        {
            Control.SetMouseCapture(false);
            Control.RaiseToolDropDown(e);
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