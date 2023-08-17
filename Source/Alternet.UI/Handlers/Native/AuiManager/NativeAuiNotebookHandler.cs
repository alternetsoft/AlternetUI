using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeAuiNotebookHandler : ControlHandler<AuiNotebook>
    {
        public NativeAuiNotebookHandler()
            : base()
        {
        }

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public new Native.AuiNotebook NativeControl =>
            (Native.AuiNotebook)base.NativeControl!;

        public override void OnLayout()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.AuiNotebook();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.PageClose -= NativeControl_PageClose;
            NativeControl.PageClosed -= NativeControl_PageClosed;
            NativeControl.PageChanged -= NativeControl_PageChanged;
            NativeControl.PageChanging -= NativeControl_PageChanging;
            NativeControl.WindowListButton -= NativeControl_WindowListButton;
            NativeControl.BeginDrag -= NativeControl_BeginDrag;
            NativeControl.EndDrag -= NativeControl_EndDrag;
            NativeControl.DragMotion -= NativeControl_DragMotion;
            NativeControl.AllowTabDrop -= NativeControl_AllowTabDrop;
            NativeControl.DragDone -= NativeControl_DragDone;
            NativeControl.TabMiddleMouseDown -= NativeControl_TabMiddleMouseDown;
            NativeControl.TabMiddleMouseUp -= NativeControl_TabMiddleMouseUp;
            NativeControl.TabRightMouseDown -= NativeControl_TabRightMouseDown;
            NativeControl.TabRightMouseUp -= NativeControl_TabRightMouseUp;
            NativeControl.BgDclickMouse -= NativeControl_BgDclickMouse;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.PageClose += NativeControl_PageClose;
            NativeControl.PageClosed += NativeControl_PageClosed;
            NativeControl.PageChanged += NativeControl_PageChanged;
            NativeControl.PageChanging += NativeControl_PageChanging;
            NativeControl.WindowListButton += NativeControl_WindowListButton;
            NativeControl.BeginDrag += NativeControl_BeginDrag;
            NativeControl.EndDrag += NativeControl_EndDrag;
            NativeControl.DragMotion += NativeControl_DragMotion;
            NativeControl.AllowTabDrop += NativeControl_AllowTabDrop;
            NativeControl.DragDone += NativeControl_DragDone;
            NativeControl.TabMiddleMouseDown += NativeControl_TabMiddleMouseDown;
            NativeControl.TabMiddleMouseUp += NativeControl_TabMiddleMouseUp;
            NativeControl.TabRightMouseDown += NativeControl_TabRightMouseDown;
            NativeControl.TabRightMouseUp += NativeControl_TabRightMouseUp;
            NativeControl.BgDclickMouse += NativeControl_BgDclickMouse;
        }

        private void NativeControl_BgDclickMouse(object? sender, EventArgs e)
        {
            Control.RaiseBgDclickMouse(e);
        }

        private void NativeControl_TabRightMouseUp(object? sender, EventArgs e)
        {
            Control.RaiseTabRightMouseUp(e);
        }

        private void NativeControl_TabRightMouseDown(object? sender, EventArgs e)
        {
            Control.RaiseTabRightMouseDown(e);
        }

        private void NativeControl_TabMiddleMouseUp(object? sender, EventArgs e)
        {
            Control.RaiseTabMiddleMouseUp(e);
        }

        private void NativeControl_TabMiddleMouseDown(object? sender, EventArgs e)
        {
            Control.RaiseTabMiddleMouseDown(e);
        }

        private void NativeControl_DragDone(object? sender, EventArgs e)
        {
            Control.RaiseDragDone(e);
        }

        private void NativeControl_AllowTabDrop(object? sender, EventArgs e)
        {
            Control.RaiseAllowTabDrop(e);
        }

        private void NativeControl_DragMotion(object? sender, EventArgs e)
        {
            Control.RaiseDragMotion(e);
        }

        private void NativeControl_EndDrag(object? sender, EventArgs e)
        {
            Control.RaiseEndDrag(e);
        }

        private void NativeControl_BeginDrag(object? sender, EventArgs e)
        {
            Control.RaiseBeginDrag(e);
        }

        private void NativeControl_WindowListButton(object? sender, EventArgs e)
        {
            Control.RaiseWindowListButton(e);
        }

        private void NativeControl_PageChanging(object? sender, EventArgs e)
        {
            Control.RaisePageChanging(e);
        }

        private void NativeControl_PageChanged(object? sender, EventArgs e)
        {
            Control.RaisePageChanged(e);
        }

        private void NativeControl_PageClosed(object? sender, EventArgs e)
        {
            Control.RaisePageClosed(e);
        }

        private void NativeControl_PageClose(object? sender, EventArgs e)
        {
            Control.RaisePageClose(e);
        }
    }
}