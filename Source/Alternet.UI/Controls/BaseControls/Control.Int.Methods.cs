﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Enumerates known handler types.
        /// </summary>
        public enum HandlerType
        {
            /// <summary>
            /// Native handler type.
            /// </summary>
            Native,

            /// <summary>
            /// Generic type.
            /// </summary>
            Generic,
        }

        internal static void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        internal static void PerformDefaultLayout(
            Control container,
            RectD childrenLayoutBounds,
            IReadOnlyList<Control> childs)
        {
            foreach (var control in childs)
            {
                var preferredSize = control.GetPreferredSizeLimited(childrenLayoutBounds.Size);

                var horizontalPosition =
                    LayoutFactory.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.HorizontalAlignment);
                var verticalPosition =
                    LayoutFactory.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        internal static SizeD GetPreferredSizeDefaultLayout(Control container, SizeD availableSize)
        {
            if (container.HasChildren)
                return container.GetSpecifiedOrChildrenPreferredSize(availableSize);
            return container.GetNativeControlSize(availableSize);
        }

        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return NativeControl.Default.GetClassDefaultAttributesBgColor(controlType, renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return NativeControl.Default.GetClassDefaultAttributesFgColor(controlType, renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return NativeControl.Default.GetClassDefaultAttributesFont(controlType, renderSize);
        }

        internal void ReportBoundsChanged()
        {
            var newBounds = Bounds;

            var locationChanged = reportedBounds?.Location != newBounds.Location;
            var sizeChanged = reportedBounds?.Size != newBounds.Size;

            reportedBounds = newBounds;

            if (locationChanged)
                RaiseLocationChanged(EventArgs.Empty);

            if (sizeChanged)
                RaiseSizeChanged(EventArgs.Empty);

            if (sizeChanged)
                PerformLayout(true);
        }

        /// <summary>
        /// Creates a handler for the control.
        /// </summary>
        /// <remarks>
        /// You typically should not call the <see cref="CreateHandler"/>
        /// method directly.
        /// The preferred method is to call the
        /// <see cref="EnsureHandlerCreated"/> method, which forces a handler
        /// to be created for the control.
        /// </remarks>
        internal virtual BaseControlHandler CreateHandler()
        {
            return new GenericControlHandler();
        }

        internal void RaiseNativeSizeChanged()
        {
            OnNativeSizeChanged(EventArgs.Empty);
        }

        internal void RaiseDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseHandleCreated()
        {
            OnHandleCreated(EventArgs.Empty);
            HandleCreated?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseHandleDestroyed()
        {
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseActivated()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever
        /// re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        internal void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        internal void RaiseKeyPress(KeyPressEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                form.OnKeyPress(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                control.OnKeyPress(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        internal void RaiseMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                OnMouseLeftButtonUp(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                OnMouseRightButtonUp(e);
            }
        }

        internal void RaiseMouseDown(MouseEventArgs e)
        {
            /*Application.Log($"{GetType()}.RaiseMouseDown");*/
            OnMouseDown(e);
            /*Application.Log($"{GetType()}.RaiseMouseDown 2: {e}");*/

            if (e.ChangedButton == MouseButton.Left)
            {
                /*Application.Log($"{GetType()}.RaiseMouseDown 3");*/
                OnMouseLeftButtonDown(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                OnMouseRightButtonDown(e);
            }
        }

        internal void RaiseProcessException(ControlExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
        }

        internal void RaiseMouseWheel(MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        internal void RaiseMouseDoubleClick(MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        internal void RaiseKeyDown(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null)
            {
                if (form.KeyPreview)
                {
                    e.CurrentTarget = form;
                    form.OnKeyDown(e);
                    if (e.Handled)
                        return;
                }
                else
                    form = null;
            }

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
                control.OnKeyDown(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseKeyUp(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                e.CurrentTarget = form;
                form.OnKeyUp(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
                control.OnKeyUp(e);
                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseTextChanged(EventArgs e) => OnTextChanged(e);

        internal void RaiseSizeChanged(EventArgs e) => OnSizeChanged(e);

        internal void RaiseScroll(ScrollEventArgs e) => OnScroll(e);

        internal void RaiseLocationChanged(EventArgs e) => OnLocationChanged(e);

        internal void RaiseMouseCaptureLost()
        {
            OnMouseCaptureLost(EventArgs.Empty);
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMouseEnter()
        {
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseCurrentStateChanged()
        {
            OnCurrentStateChanged(EventArgs.Empty);
            CurrentStateChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseIsMouseOverChanged()
        {
            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseCurrentStateChanged();
        }

        internal void RaiseMouseLeave()
        {
            RaiseIsMouseOverChanged();
            OnMouseLeave(EventArgs.Empty);
            MouseLeave?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseChildInserted(Control childControl)
        {
            OnChildInserted(childControl);
            ChildInserted?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        internal void RaiseChildRemoved(Control childControl)
        {
            OnChildInserted(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        internal void RaisePaint(PaintEventArgs e)
        {
            OnPaint(e);
            Paint?.Invoke(this, e);
        }

        internal void RaiseGotFocus()
        {
            OnGotFocus(EventArgs.Empty);
            GotFocus?.Invoke(this, EventArgs.Empty);
            Designer?.RaiseGotFocus(this);
            RaiseCurrentStateChanged();
        }

        internal void RaiseLostFocus()
        {
            OnLostFocus(EventArgs.Empty);
            LostFocus?.Invoke(this, EventArgs.Empty);
            RaiseCurrentStateChanged();
        }

        internal void SetParentInternal(Control? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal virtual SizeD GetPreferredSizeLimited(SizeD availableSize)
        {
            var result = GetPreferredSize(availableSize);
            var minSize = MinimumSize;
            var maxSize = MaximumSize;
            var preferredSize = result.ApplyMinMax(minSize, maxSize);
            return preferredSize;
        }

        internal void RaiseDragStart(DragStartEventArgs e) => OnDragStart(e);

        internal void RaiseDragDrop(DragEventArgs e) => OnDragDrop(e);

        internal void RaiseDragOver(DragEventArgs e) => OnDragOver(e);

        internal void RaiseDragEnter(DragEventArgs e) => OnDragEnter(e);

        internal void RaiseDragLeave(EventArgs e) => OnDragLeave(e);

        internal void SendMouseDownEvent(int x, int y)
        {
            GetNative().SendMouseDownEvent(this, x, y);
        }

        internal void SendMouseUpEvent(int x, int y)
        {
            GetNative().SendMouseUpEvent(this, x, y);
        }

        internal bool BeginRepositioningChildren()
        {
            return GetNative().BeginRepositioningChildren(this);
        }

        internal void EndRepositioningChildren()
        {
            GetNative().EndRepositioningChildren(this);
        }

        internal IntPtr GetHandle()
        {
            return GetNative().GetHandle(this);
        }

        internal SizeD GetNativeControlSize(SizeD availableSize)
        {
            if (IsDummy)
                return SizeD.Empty;
            var s = GetNative().GetPreferredSize(this, availableSize);
            s += Padding.Size;
            return new SizeD(
                double.IsNaN(SuggestedWidth) ? s.Width : SuggestedWidth,
                double.IsNaN(SuggestedHeight) ? s.Height : SuggestedHeight);
        }

        internal void DoInsideRepositioningChildren(Action action)
        {
            var repositioning = BeginRepositioningChildren();
            if (repositioning)
            {
                try
                {
                    action();
                }
                finally
                {
                    EndRepositioningChildren();
                }
            }
            else
                action();
        }

        internal void NativeControl_Paint()
        {
            if (!UserPaint)
                return;

            using var dc = GetNative().OpenPaintDrawingContext(this);

            RaisePaint(new PaintEventArgs(dc, ClientRectangle));
        }

        internal void NativeControl_HorizontalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollOrientation.HorizontalScroll,
                NewValue = GetNative().GetScrollBarEvtPosition(this),
                Type = GetNative().GetScrollBarEvtKind(this),
            };
            RaiseScroll(args);
        }

        internal void NativeControl_VerticalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollOrientation.VerticalScroll,
                NewValue = GetNative().GetScrollBarEvtPosition(this),
                Type = GetNative().GetScrollBarEvtKind(this),
            };
            RaiseScroll(args);
        }

        internal void NativeControl_VisibleChanged()
        {
            bool visible = GetNative().GetVisible(this);
            Visible = visible;

            if (BaseApplication.IsLinuxOS && visible)
            {
                // todo: this is a workaround for a problem on Linux when
                // ClientSize is not reported correctly until the window is shown
                // So we need to relayout all after the proper client size is available
                // This should be changed later in respect to RedrawOnResize functionality.
                // Also we may need to do this for top-level windows.
                // Doing this on Windows results in strange glitches like disappearing
                // tab controls' tab.
                // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                PerformLayout();
            }
        }
    }
}
