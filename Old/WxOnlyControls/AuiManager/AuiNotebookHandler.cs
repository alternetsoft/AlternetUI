using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class AuiNotebookHandler : ControlHandler<AuiNotebook>, IControlHandler
    {
        public AuiNotebookHandler()
        {
        }

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

        internal override Native.Control CreateNativeControl()
        {
            return new NativeAuiNotebook(AuiNotebook.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.PageClose -= NativeControl_PageClose;
            NativeControl.PageClosed = null;
            NativeControl.PageChanged = null;
            NativeControl.PageChanging -= NativeControl_PageChanging;
            NativeControl.PageButton -= NativeControl_PageButton;
            NativeControl.BeginDrag = null;
            NativeControl.EndDrag = null;
            NativeControl.DragMotion = null;
            NativeControl.AllowTabDrop = null;
            NativeControl.DragDone = null;
            NativeControl.TabMiddleMouseDown = null;
            NativeControl.TabMiddleMouseUp = null;
            NativeControl.TabRightMouseDown = null;
            NativeControl.TabRightMouseUp = null;
            NativeControl.BgDclickMouse = null;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.PageClose += NativeControl_PageClose;
            NativeControl.PageClosed = NativeControl_PageClosed;
            NativeControl.PageChanged = NativeControl_PageChanged;
            NativeControl.PageChanging += NativeControl_PageChanging;
            NativeControl.PageButton += NativeControl_PageButton;
            NativeControl.BeginDrag = NativeControl_BeginDrag;
            NativeControl.EndDrag = NativeControl_EndDrag;
            NativeControl.DragMotion = NativeControl_DragMotion;
            NativeControl.AllowTabDrop = NativeControl_AllowTabDrop;
            NativeControl.DragDone = NativeControl_DragDone;
            NativeControl.TabMiddleMouseDown = NativeControl_TabMiddleMouseDown;
            NativeControl.TabMiddleMouseUp = NativeControl_TabMiddleMouseUp;
            NativeControl.TabRightMouseDown = NativeControl_TabRightMouseDown;
            NativeControl.TabRightMouseUp = NativeControl_TabRightMouseUp;
            NativeControl.BgDclickMouse = NativeControl_BgDclickMouse;
        }

        private void NativeControl_BgDclickMouse()
        {
            Control.RaiseBgDoubleClick(EventArgs.Empty);
        }

        private void NativeControl_TabRightMouseUp()
        {
            Control.RaiseTabRightMouseUp(EventArgs.Empty);
        }

        private void NativeControl_TabRightMouseDown()
        {
            Control.RaiseTabRightMouseDown(EventArgs.Empty);
        }

        private void NativeControl_TabMiddleMouseUp()
        {
            Control.RaiseTabMiddleMouseUp(EventArgs.Empty);
        }

        private void NativeControl_TabMiddleMouseDown()
        {
            Control.RaiseTabMiddleMouseDown(EventArgs.Empty);
        }

        private void NativeControl_DragDone()
        {
            Control.RaiseDragDone(EventArgs.Empty);
        }

        private void NativeControl_AllowTabDrop()
        {
            Control.RaiseAllowTabDrop(EventArgs.Empty);
        }

        private void NativeControl_DragMotion()
        {
            Control.RaiseDragMotion(EventArgs.Empty);
        }

        private void NativeControl_EndDrag()
        {
            Control.RaiseEndDrag(EventArgs.Empty);
        }

        private void NativeControl_BeginDrag()
        {
            Control.RaiseBeginDrag(EventArgs.Empty);
        }

        private void NativeControl_PageButton(object? sender, CancelEventArgs e)
        {
            Control.RaisePageButton(e);
        }

        private void NativeControl_PageChanging(object? sender, CancelEventArgs e)
        {
            Control.RaisePageChanging(e);
        }

        private void NativeControl_PageChanged()
        {
            Control.RaisePageChanged(EventArgs.Empty);
        }

        private void NativeControl_PageClosed()
        {
            Control.RaisePageClosed(EventArgs.Empty);
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