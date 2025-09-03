using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Control
    {
        public AbstractControl? EventUIFocusedControl
        {
            get
            {
                return WxApplicationHandler.FromNativeControl(EventFocusedControl);
            }
        }

        public WxControlHandler? Handler
        {
            get
            {
                return handler as WxControlHandler;
            }
        }

        public UI.Control? UIControl
        {
            get
            {
                var result = Handler?.Control;
                if (result?.DisposingOrDisposed ?? true)
                    return null;
                return result;
            }
        }

        public void OnPlatformEventIdle()
        {
        }

        public void OnPlatformEventPaint()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;
            if (!UserPaint)
                return;
            var clientRect = uiControl.ClientRectangle;
            if (clientRect.SizeIsEmpty)
                return;
            if (!uiControl.VisibleOnScreen)
                return;

            var e = new PaintEventArgs(() => Handler?.OpenPaintDrawingContext()
                ?? Alternet.Drawing.PlessGraphics.Default, clientRect);

            try
            {
                uiControl.RaisePaint(e);
            }
            finally
            {
                if (e.GraphicsAllocated)
                {
                    e.Graphics.Dispose();
                }
                else
                {
                }
            }
        }

        public void OnPlatformEventMouseEnter()
        {
            UIControl?.RaiseMouseEnterOnTarget(EventArgs.Empty);
        }

        public void OnPlatformEventMouseLeave()
        {
            UIControl?.RaiseMouseLeaveOnTarget(EventArgs.Empty);
        }

        public void OnPlatformEventMouseClick()
        {
        }

        public void OnPlatformEventVisibleChanged()
        {
            bool visible = Visible;
            /*UIControl?.SetVisible(visible);*/

            if (App.IsLinuxOS && visible)
            {
                // todo: this is a workaround for a problem on Linux when
                // ClientSize is not reported correctly until the window is shown
                // So we need to perform layout after the proper client size is available
                // This should be changed later in respect to RedrawOnResize functionality.
                // Also we may need to do this for top-level windows.
                // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                UIControl?.PerformLayout();
            }
        }

        public void OnPlatformEventMouseCaptureLost()
        {
            UIControl?.RaiseMouseCaptureLost(EventArgs.Empty);
        }

        public void OnPlatformEventRequestCursor()
        {
            var uiControl = UIControl;
            if (uiControl is null)
                return;
        }

        public void OnPlatformEventDpiChanged()
        {
            var uiControl = UIControl;
            if (uiControl is null)
                return;

            var oldDpi = EventOldDpi;
            var newDpi = EventNewDpi;

            var e = new DpiChangedEventArgs(oldDpi, newDpi);
            uiControl?.RaiseDpiChanged(e);
        }

        public void OnPlatformEventDestroyed()
        {
            Handler?.OnNativeControlDestroyed();
        }

        public void OnPlatformEventTextChanged()
        {
            UIControl?.RaiseHandlerTextChanged(Text);
        }

        public void OnPlatformEventGotFocus()
        {
            UIControl?.RaiseGotFocus(new(EventUIFocusedControl));
        }

        public void OnPlatformEventLostFocus()
        {
            UIControl?.RaiseLostFocus(new(EventUIFocusedControl));
        }

        public void OnPlatformEventDragLeave()
        {
            UIControl?.RaiseDragLeave(EventArgs.Empty);
        }

        public void OnPlatformEventVerticalScrollBarValueChanged()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;

            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Vertical,
                NewValue = GetScrollBarEvtPosition(),
                Type = (ScrollEventType)GetScrollBarEvtKind(),
            };

            uiControl.RaiseScroll(args);
        }

        public void OnPlatformEventHorizontalScrollBarValueChanged()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;

            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Horizontal,
                NewValue = GetScrollBarEvtPosition(),
                Type = (ScrollEventType)GetScrollBarEvtKind(),
            };

            uiControl.RaiseScroll(args);
        }

        public void OnPlatformEventSizeChanged()
        {
            UIControl?.RaiseHandlerSizeChanged(EventArgs.Empty);
        }

        public void OnPlatformEventLocationChanged()
        {
            UIControl?.RaiseContainerLocationChanged(EventArgs.Empty);
        }

        public void OnPlatformEventActivated()
        {
            UIControl?.RaiseActivated(EventArgs.Empty);
        }

        public void OnPlatformEventDeactivated()
        {
            UIControl?.RaiseDeactivated(EventArgs.Empty);
        }

        public virtual void OnPlatformEventHandleCreated()
        {
            if(UIControl is not null)
            {
                if(UIControl.ReportedBounds != Alternet.Drawing.RectD.MinusOne)
                    Bounds = UIControl.ReportedBounds;
            }

            Handler?.OnHandleCreated();
            UIControl?.RaiseHandleCreated(EventArgs.Empty);
        }

        public void OnPlatformEventHandleDestroyed()
        {
            UIControl?.RaiseHandleDestroyed(EventArgs.Empty);
        }

        public void OnPlatformEventSystemColorsChanged()
        {
            UIControl?.RaiseSystemColorsChanged(EventArgs.Empty);
        }

        public void OnPlatformEventDragDrop(NativeEventArgs<DragEventData> ea)
        {
            if(UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragDrop);
        }

        public void OnPlatformEventDragOver(NativeEventArgs<DragEventData> ea)
        {
            if (UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragOver);
        }

        public void OnPlatformEventDragEnter(NativeEventArgs<DragEventData> ea)
        {
            if (UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragEnter);
        }

        private void RaiseDragAndDropEvent(
            Native.NativeEventArgs<Native.DragEventData> e,
            Action<DragEventArgs>? raiseAction)
        {
            if (raiseAction is null || (!UIControl?.AllowDrop ?? false))
                return;

            var data = e.Data;
            var ea = new DragEventArgs(
                new UnmanagedDataObjectAdapter(
                    new Native.UnmanagedDataObject(data.data)),
                new Drawing.PointD(data.mouseClientLocationX, data.mouseClientLocationY),
                (DragDropEffects)data.effect);

            raiseAction?.Invoke(ea);

            e.Result = new IntPtr((int)ea.Effect);
        }

        internal class NonAbstractNativeControl : Native.Control
        {
            public NonAbstractNativeControl()
            {
                SetNativePointer(NativeApi.Control_CreateControl_());
            }
        }
    }
}

    