using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Control"/> behavior
    /// and appearance.
    /// </summary>
    internal abstract class ControlHandler : BaseObject
    {
        private Control? control;
        private Native.Control? nativeControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public ControlHandler()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the control has a native control associated with it.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if a native control has been assigned to the
        /// control; otherwise, <see langword="false" />.</returns>
        public bool IsNativeControlCreated
        {
            get => nativeControl is not null;
        }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ControlHandler"/> is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => control != null;

        internal Native.Control NativeControl
        {
            get
            {
                if (nativeControl == null)
                {
                    nativeControl = CreateNativeControl();
                    nativeControl.handler = this;
                    OnNativeControlCreated();
                }

                return nativeControl;
            }
        }

        internal bool NativeControlCreated => nativeControl != null;

        public void RaiseChildInserted(Control childControl)
        {
            Control.RaiseChildInserted(childControl);
            OnChildInserted(childControl);
        }

        public void RaiseChildRemoved(Control childControl)
        {
            Control.RaiseChildRemoved(childControl);
            OnChildRemoved(childControl);
        }

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        public void Attach(Control control)
        {
            this.control = control;
            OnAttach();
        }

        /// <summary>
        /// Attaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public void Detach()
        {
            OnDetach();

            DisposeNativeControl();
            control = null;
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public void Update()
        {
            if (nativeControl != null)
                Control.GetNative().Update(Control);
            else
            {
                var parent = Control.TryFindClosestParentWithNativeControl();
                parent?.Update();
            }
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public void Invalidate()
        {
            if (nativeControl != null)
                Control.GetNative().Invalidate(Control);
            else
            {
                var parent = Control.TryFindClosestParentWithNativeControl();
                parent?.Invalidate();
            }
        }

        internal static ControlHandler? NativeControlToHandler(
            Native.Control control)
        {
            return (ControlHandler?)control.handler;
        }

        internal virtual Native.Control CreateNativeControl() =>
            new Native.Panel();

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        protected internal virtual void OnLayoutChanged()
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into the
        /// <see cref="Control.Children"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
            TryInsertNativeControl(childControl);
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
            TryRemoveNativeControl(childControl);
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected virtual void OnDetach()
        {
            /*todo: consider clearing the native control's children.*/

            if (NativeControl != null)
            {
                NativeControl.HandleCreated = null;
                NativeControl.HandleDestroyed = null;
                NativeControl.Activated = null;
                NativeControl.Deactivated = null;
                NativeControl.Idle = null;
                NativeControl.Paint = null;
                NativeControl.VisibleChanged = null;
                NativeControl.MouseEnter = null;
                NativeControl.MouseLeave = null;
                NativeControl.MouseCaptureLost = null;
                NativeControl.DragOver -= NativeControl_DragOver;
                NativeControl.DragEnter -= NativeControl_DragEnter;
                NativeControl.DragLeave = null;
                NativeControl.DragDrop -= NativeControl_DragDrop;
                NativeControl.GotFocus = null;
                NativeControl.LostFocus = null;
                NativeControl.SizeChanged = null;
                NativeControl.VerticalScrollBarValueChanged = null;
                NativeControl.HorizontalScrollBarValueChanged = null;
            }
        }

        /// <summary>
        /// Called when native control size is changed.
        /// </summary>
        protected virtual void NativeControlSizeChanged()
        {
            Control.RaiseNativeSizeChanged();
        }

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected virtual void OnAttach()
        {
            NativeControl.Visible = Control.Visible;
            NativeControl.Enabled = Control.Enabled;
            ApplyChildren();
        }

        private protected virtual void OnNativeControlCreated()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();
            var parent = Control.Parent;
            if (parent != null)
            {
                parent.Handler.TryInsertNativeControl(Control);
                parent.PerformLayout();
            }

            NativeControl.HandleCreated = NativeControl_HandleCreated;
            NativeControl.HandleDestroyed = NativeControl_HandleDestroyed;
            NativeControl.Activated = NativeControl_Activated;
            NativeControl.Deactivated = NativeControl_Deactivated;
            NativeControl.Paint = NativeControl_Paint;
            NativeControl.VisibleChanged = NativeControl_VisibleChanged;
            NativeControl.MouseEnter = NativeControl_MouseEnter;
            NativeControl.MouseLeave = NativeControl_MouseLeave;
            NativeControl.MouseCaptureLost = NativeControl_MouseCaptureLost;
            NativeControl.DragOver += NativeControl_DragOver;
            NativeControl.DragEnter += NativeControl_DragEnter;
            NativeControl.DragLeave = NativeControl_DragLeave;
            NativeControl.DragDrop += NativeControl_DragDrop;
            NativeControl.GotFocus = NativeControl_GotFocus;
            NativeControl.LostFocus = NativeControl_LostFocus;
            NativeControl.SizeChanged = NativeControl_SizeChanged;
            NativeControl.Idle = NativeControl_Idle;
            NativeControl.VerticalScrollBarValueChanged =
                NativeControl_VerticalScrollBarValueChanged;
            NativeControl.HorizontalScrollBarValueChanged =
                NativeControl_HorizontalScrollBarValueChanged;
#if DEBUG
            /*Debug.WriteLine($"{GetType()} {NativeControl.Id} {NativeControl.Name}");*/
#endif
        }

        private static void DisposeNativeControlCore(Native.Control control)
        {
            control.handler = null;
            control.Dispose();
        }

        private void NativeControl_Deactivated()
        {
            Control.RaiseDeactivated();
        }

        private void NativeControl_HandleCreated()
        {
            Control.RaiseHandleCreated();
        }

        private void NativeControl_HandleDestroyed()
        {
            Control.RaiseHandleDestroyed();
        }

        private void NativeControl_Activated()
        {
            Control.RaiseActivated();
        }

        private void NativeControl_HorizontalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollOrientation.HorizontalScroll,
                NewValue = NativeControl.GetScrollBarEvtPosition(),
                Type = (ScrollEventType)NativeControl.GetScrollBarEvtKind(),
            };
            Control.RaiseScroll(args);
        }

        private void NativeControl_VerticalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollOrientation.VerticalScroll,
                NewValue = NativeControl.GetScrollBarEvtPosition(),
                Type = (ScrollEventType)NativeControl.GetScrollBarEvtKind(),
            };
            Control.RaiseScroll(args);
        }

        private void NativeControl_Idle()
        {
            Control.RaiseIdle(EventArgs.Empty);
        }

        private void NativeControl_SizeChanged()
        {
            NativeControlSizeChanged();
            Control.ReportBoundsChanged();
        }

        private void NativeControl_GotFocus()
        {
            Control.RaiseGotFocus(EventArgs.Empty);
        }

        private void NativeControl_LostFocus()
        {
            Control.RaiseLostFocus(EventArgs.Empty);
        }

#pragma warning disable
        private void RaiseDragAndDropEvent(
            Native.NativeEventArgs<Native.DragEventData> e,
            Action<DragEventArgs> raiseAction)
#pragma warning restore
        {
            var data = e.Data;
            var ea = new DragEventArgs(
                new UnmanagedDataObjectAdapter(
                    new Native.UnmanagedDataObject(data.data)),
                new PointD(data.mouseClientLocationX, data.mouseClientLocationY),
                (DragDropEffects)data.effect);

            raiseAction(ea);

            e.Result = new IntPtr((int)ea.Effect);
        }

        private void NativeControl_DragOver(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragOver(ea));

        private void NativeControl_DragEnter(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragEnter(ea));

        private void NativeControl_DragDrop(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragDrop(ea));

        private void NativeControl_DragLeave() =>
            Control.RaiseDragLeave(EventArgs.Empty);

        private void NativeControl_MouseCaptureLost()
        {
            Control.RaiseMouseCaptureLost();
        }

        private void ApplyChildren()
        {
            if (!Control.HasChildren)
                return;
            for (var i = 0; i < Control.Children.Count; i++)
                RaiseChildInserted(Control.Children[i]);
        }

        private void DisposeNativeControl()
        {
            if (nativeControl != null)
            {
                if (nativeControl.HasWindowCreated)
                {
                    nativeControl.Destroyed += NativeControl_Destroyed;
                    nativeControl.Destroy();
                }
                else
                    DisposeNativeControlCore(nativeControl);

                nativeControl = null;
            }
        }

        private void NativeControl_Destroyed(object? sender, CancelEventArgs e)
        {
            var nativeControl = (Native.Control)sender!;
            nativeControl.Destroyed -= NativeControl_Destroyed;
            DisposeNativeControlCore(nativeControl);
        }

        private void TryInsertNativeControl(Control childControl)
        {
            // todo: use index
            var childNativeControl = childControl.NativeControl;
            if (childNativeControl == null)
                return;

            if (childNativeControl.ParentRefCounted != null)
                return;

            var parentNativeControl = NativeControl;
            parentNativeControl ??=
                Control.TryFindClosestParentWithNativeControl()?.NativeControl;

            parentNativeControl?.AddChild(childNativeControl);
        }

        private void TryRemoveNativeControl(Control childControl)
        {
            if (nativeControl != null && childControl.Handler.nativeControl != null)
                nativeControl?.RemoveChild(childControl.Handler.nativeControl);
        }

        private void NativeControl_VisibleChanged()
        {
            bool visible = Control.GetNative().GetVisible(Control);
            Control.Visible = visible;

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
                Control.PerformLayout();
            }
        }

        private void NativeControl_MouseEnter()
        {
            var myControl = Control;
            var currentTarget = InputManager.GetMouseTargetControl(ref myControl);
            currentTarget?.RaiseMouseEnter();
        }

        private void NativeControl_MouseLeave()
        {
            var myControl = Control;
            var currentTarget = InputManager.GetMouseTargetControl(ref myControl);
            currentTarget?.RaiseMouseLeave();
        }

        private void NativeControl_Paint()
        {
            if (NativeControl == null)
                return;

            if (!Control.UserPaint)
                return;

            using var dc =
                new WxGraphics(NativeControl.OpenPaintDrawingContext());

            Control.RaisePaint(new PaintEventArgs(dc, Control.ClientRectangle));
        }
    }
}