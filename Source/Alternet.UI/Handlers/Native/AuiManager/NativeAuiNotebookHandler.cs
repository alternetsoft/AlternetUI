using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public override void OnLayout()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeAuiNotebook(AuiNotebook.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.PageClose -= NativeControl_PageClose;
            NativeControl.PageClosed -= NativeControl_PageClosed;
            NativeControl.PageChanged -= NativeControl_PageChanged;
            NativeControl.PageChanging -= NativeControl_PageChanging;
            NativeControl.PageButton -= NativeControl_PageButton;
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
            NativeControl.PageButton += NativeControl_PageButton;
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
            Control.RaiseBgDoubleClick(e);
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

        private void NativeControl_PageButton(object? sender, CancelEventArgs e)
        {
            Control.RaisePageButton(e);
        }

        private void NativeControl_PageChanging(object? sender, CancelEventArgs e)
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

        private void NativeControl_PageClose(object? sender, CancelEventArgs e)
        {
            Control.RaisePageClose(e);
        }

        public class NativeAuiNotebook : Native.AuiNotebook
        {
            public NativeAuiNotebook(AuiNotebookCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.AuiNotebook_CreateEx_((int)style));
            }
        }
    }
}