using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Control"/> behavior
    /// and appearance.
    /// </summary>
    internal class WxControlHandler : BaseControlHandler
    {
        private Native.Control? nativeControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public WxControlHandler()
        {
        }

        public override Action? Idle
        {
            get => NativeControl.Idle;
            set => NativeControl.Idle = value;
        }

        public override Action? Paint
        {
            get => NativeControl.Paint;
            set => NativeControl.Paint = value;
        }

        public override Action? MouseEnter
        {
            get => NativeControl.MouseEnter;
            set => NativeControl.MouseEnter = value;
        }

        public override Action? MouseLeave
        {
            get => NativeControl.MouseLeave;
            set => NativeControl.MouseLeave = value;
        }

        public override Action? MouseClick
        {
            get => NativeControl.MouseClick;
            set => NativeControl.MouseClick = value;
        }

        public override Action? VisibleChanged
        {
            get => NativeControl.VisibleChanged;
            set => NativeControl.VisibleChanged = value;
        }

        public override Action? MouseCaptureLost
        {
            get => NativeControl.MouseCaptureLost;
            set => NativeControl.MouseCaptureLost = value;
        }

        public override Action? GotFocus
        {
            get => NativeControl.GotFocus;
            set => NativeControl.GotFocus = value;
        }

        public override Action? LostFocus
        {
            get => NativeControl.LostFocus;
            set => NativeControl.LostFocus = value;
        }

        public override Action? DragLeave
        {
            get => NativeControl.DragLeave;
            set => NativeControl.DragLeave = value;
        }

        public override Action? VerticalScrollBarValueChanged
        {
            get => NativeControl.VerticalScrollBarValueChanged;
            set => NativeControl.VerticalScrollBarValueChanged = value;
        }

        public override Action? HorizontalScrollBarValueChanged
        {
            get => NativeControl.HorizontalScrollBarValueChanged;
            set => NativeControl.HorizontalScrollBarValueChanged = value;
        }

        public override Action? SizeChanged
        {
            get => NativeControl.SizeChanged;
            set => NativeControl.SizeChanged = value;
        }

        public override Action? Activated
        {
            get => NativeControl.Activated;
            set => NativeControl.Activated = value;
        }

        public override Action? Deactivated
        {
            get => NativeControl.Deactivated;
            set => NativeControl.Deactivated = value;
        }

        public override Action? HandleCreated
        {
            get => NativeControl.HandleCreated;
            set => NativeControl.HandleCreated = value;
        }

        public override Action? HandleDestroyed
        {
            get => NativeControl.HandleDestroyed;
            set => NativeControl.HandleDestroyed = value;
        }

        /// <summary>
        /// Gets a value indicating whether the control has a native control associated with it.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if a native control has been assigned to the
        /// control; otherwise, <see langword="false" />.</returns>
        public override bool IsNativeControlCreated
        {
            get => nativeControl is not null;
        }

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

        public override object GetNativeControl() => NativeControl;

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public override void Detach()
        {
            base.Detach();
            DisposeNativeControl();
        }

        internal static WxControlHandler? NativeControlToHandler(
            Native.Control control)
        {
            return (WxControlHandler?)control.handler;
        }

        internal virtual Native.Control CreateNativeControl()
        {
            var result = new Native.Panel();

            if(Control.IsGraphicControl)
                result.AcceptsFocusAll = false;

            return result;
        }

        protected override void OnChildInserted(Control childControl)
        {
            TryInsertNativeControl(childControl);
        }

        protected override void OnChildRemoved(Control childControl)
        {
            TryRemoveNativeControl(childControl);
        }

        protected override void OnDetach()
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

        protected override void OnAttach()
        {
            NativeControl.Visible = Control.Visible;
            NativeControl.Enabled = Control.Enabled;
            ApplyChildren();
        }

        private protected virtual void OnNativeControlCreated()
        {
            var parent = Control.Parent;

            if (parent is not null)
            {
                (parent.Handler as WxControlHandler)?.TryInsertNativeControl(Control);
                parent.PerformLayout();
            }

            NativeControl.HandleCreated = Control.RaiseHandleCreated;
            NativeControl.HandleDestroyed = Control.RaiseHandleDestroyed;
            NativeControl.Activated = Control.RaiseActivated;
            NativeControl.Deactivated = Control.RaiseDeactivated;
            NativeControl.Paint = Control.OnNativeControlPaint;
            NativeControl.VisibleChanged = Control.OnNativeControlVisibleChanged;
            NativeControl.MouseEnter = NativeControl_MouseEnter;
            NativeControl.MouseLeave = NativeControl_MouseLeave;
            NativeControl.MouseCaptureLost = Control.RaiseMouseCaptureLost;
            NativeControl.DragOver += NativeControl_DragOver;
            NativeControl.DragEnter += NativeControl_DragEnter;
            NativeControl.DragLeave = NativeControl_DragLeave;
            NativeControl.DragDrop += NativeControl_DragDrop;
            NativeControl.GotFocus = Control.RaiseGotFocus;
            NativeControl.LostFocus = Control.RaiseLostFocus;
            NativeControl.SizeChanged = NativeControl_SizeChanged;
            NativeControl.Idle = Control.RaiseIdle;
            NativeControl.VerticalScrollBarValueChanged =
                Control.OnNativeControlVerticalScrollBarValueChanged;
            NativeControl.HorizontalScrollBarValueChanged =
                Control.OnNativeControlHorizontalScrollBarValueChanged;
        }

        private static void DisposeNativeControlCore(Native.Control control)
        {
            control.handler = null;
            control.Dispose();
        }

        private void NativeControl_SizeChanged()
        {
            NativeControlSizeChanged();
            Control.ReportBoundsChanged();
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
            var childNativeControl = (childControl.Handler as WxControlHandler)?.NativeControl;
            if (childNativeControl == null)
                return;

            if (childNativeControl.ParentRefCounted != null)
                return;

            var parentNativeControl = NativeControl;
            parentNativeControl?.AddChild(childNativeControl);
        }

        private void TryRemoveNativeControl(Control childControl)
        {
            var childHandler = (childControl.Handler as WxControlHandler)?.nativeControl;
            if (childHandler != null)
                nativeControl?.RemoveChild(childHandler);
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
    }
}