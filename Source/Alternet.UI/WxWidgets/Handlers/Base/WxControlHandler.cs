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

        public override LangDirection LangDirection
        {
            get => (LangDirection)NativeControl.LayoutDirection;
            set => NativeControl.LayoutDirection = (int)value;
        }

        public override ControlBorderStyle BorderStyle
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool WantChars
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsFocused => throw new NotImplementedException();

        public override bool ShowHorzScrollBar
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool ShowVertScrollBar
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool ScrollBarAlwaysVisible
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsScrollable
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override RectD Bounds
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool Visible
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool UserPaint
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override SizeD MinimumSize
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override SizeD MaximumSize
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Color BackgroundColor
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Color ForegroundColor
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Font? Font
        {
            get => Font.FromInternal(NativeControl.Font);
            set => NativeControl.Font = (UI.Native.Font?)value?.NativeObject;
        }

        public override bool IsBold
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool TabStop
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool AllowDrop
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool AcceptsFocus
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override ControlBackgroundStyle BackgroundStyle
        {
            get
            {
                var result = NativeControl.GetBackgroundStyle();
                return (ControlBackgroundStyle?)result ?? ControlBackgroundStyle.System;
            }

            set => NativeControl.SetBackgroundStyle((int)value);
        }

        public override bool AcceptsFocusFromKeyboard
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool AcceptsFocusRecursively
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool AcceptsFocusAll
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool ProcessIdle
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool BindScrollEvents
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override SizeD ClientSize
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool CanAcceptFocus => throw new NotImplementedException();

        public override bool IsMouseOver => throw new NotImplementedException();

        public override bool ProcessUIUpdates
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsMouseCaptured => throw new NotImplementedException();

        public override bool IsHandleCreated => throw new NotImplementedException();

        public override bool IsFocusable => throw new NotImplementedException();

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

        public override void OnLayoutChanged()
        {
            base.OnLayoutChanged();
        }

        public override void Raise()
        {
            throw new NotImplementedException();
        }

        public override void CenterOnParent(GenericOrientation direction)
        {
            throw new NotImplementedException();
        }

        public override void SetCursor(Cursor? value)
        {
            if (value is null)
                NativeControl.SetCursor(default);
            else
                NativeControl.SetCursor((IntPtr)value.Handler);
        }

        public override void SetToolTip(string? value)
        {
            throw new NotImplementedException();
        }

        public override void Lower()
        {
            throw new NotImplementedException();
        }

        public override void SendSizeEvent()
        {
            throw new NotImplementedException();
        }

        public override void UnsetToolTip()
        {
            throw new NotImplementedException();
        }

        public override Thickness GetIntrinsicLayoutPadding()
        {
            throw new NotImplementedException();
        }

        public override Thickness GetIntrinsicPreferredSizePadding()
        {
            throw new NotImplementedException();
        }

        public override void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            throw new NotImplementedException();
        }

        public override void HandleNeeded()
        {
            NativeControl.Required();
        }

        public override void CaptureMouse()
        {
            NativeControl.SetMouseCapture(true);
        }

        public override void ReleaseMouseCapture()
        {
            NativeControl.SetMouseCapture(false);
        }

        public override void DisableRecreate()
        {
            throw new NotImplementedException();
        }

        public override void EnableRecreate()
        {
            throw new NotImplementedException();
        }

        public override Graphics CreateDrawingContext()
        {
            return new WxGraphics(NativeControl.OpenClientDrawingContext());
        }

        public override PointD ScreenToClient(PointD point)
        {
            throw new NotImplementedException();
        }

        public override PointD ClientToScreen(PointD point)
        {
            throw new NotImplementedException();
        }

        public override PointI ScreenToDevice(PointD point)
        {
            throw new NotImplementedException();
        }

        public override PointD DeviceToScreen(PointI point)
        {
            throw new NotImplementedException();
        }

        public override void FocusNextControl(bool forward = true, bool nested = true)
        {
            throw new NotImplementedException();
        }

        public override DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            allowedEffects &= ~DragDropEffects.Scroll;
            return (DragDropEffects)NativeControl.DoDragDrop(
                UnmanagedDataObjectService.GetUnmanagedDataObject(data),
                (Native.DragDropEffects)allowedEffects);
        }

        public override void RecreateWindow()
        {
            throw new NotImplementedException();
        }

        public override void BeginUpdate()
        {
            throw new NotImplementedException();
        }

        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }

        public override void SetBounds(RectD rect, SetBoundsFlags flags)
        {
            NativeControl.SetBoundsEx(rect, (int)flags);
        }

        public override void BeginInit()
        {
            throw new NotImplementedException();
        }

        public override void EndInit()
        {
            throw new NotImplementedException();
        }

        public override bool SetFocus()
        {
            throw new NotImplementedException();
        }

        public override void SaveScreenshot(string fileName)
        {
            throw new NotImplementedException();
        }

        public override SizeD GetDPI()
        {
            throw new NotImplementedException();
        }

        public override bool IsTransparentBackgroundSupported()
        {
            throw new NotImplementedException();
        }

        public override void EndIgnoreRecreate()
        {
            throw new NotImplementedException();
        }

        public override void BeginIgnoreRecreate()
        {
            throw new NotImplementedException();
        }

        public override double GetPixelScaleFactor()
        {
            return Native.Control.DrawingDPIScaleFactor(NativeControl.WxWidget);
        }

        public override RectI GetUpdateClientRectI()
        {
            throw new NotImplementedException();
        }

        public override double PixelToDip(int value)
        {
            return Native.Control.DrawingToDip(value, NativeControl.WxWidget);
        }

        public override int PixelFromDip(double value)
        {
            return Native.Control.DrawingFromDip(value, NativeControl.WxWidget);
        }

        public override double PixelFromDipF(double value)
        {
            return Native.Control.DrawingFromDipF(value, NativeControl.WxWidget);
        }

        public override void SetScrollBar(
            IControl control,
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            NativeControl.SetScrollBar(orientation, visible, value, largeChange, maximum);
        }

        public override bool IsScrollBarVisible(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.IsScrollBarVisible(orientation);
        }

        public override int GetScrollBarValue(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarValue(orientation);
        }

        public override int GetScrollBarLargeChange(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarLargeChange(orientation);
        }

        public override int GetScrollBarMaximum(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarMaximum(orientation);
        }

        public override void ResetBackgroundColor()
        {
            throw new NotImplementedException();
        }

        public override void ResetForegroundColor()
        {
            throw new NotImplementedException();
        }

        public override void SetEnabled(bool value)
        {
            throw new NotImplementedException();
        }

        public override Color GetDefaultAttributesBgColor()
        {
            throw new NotImplementedException();
        }

        public override Color GetDefaultAttributesFgColor()
        {
            throw new NotImplementedException();
        }

        public override Font? GetDefaultAttributesFont()
        {
            return Font.FromInternal(NativeControl.GetDefaultAttributesFont());
        }

        public override void SendMouseDownEvent(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void SendMouseUpEvent(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override bool BeginRepositioningChildren()
        {
            throw new NotImplementedException();
        }

        public override void EndRepositioningChildren()
        {
            throw new NotImplementedException();
        }

        public override void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Invalidate()
        {
            throw new NotImplementedException();
        }

        public override nint GetHandle()
        {
            throw new NotImplementedException();
        }

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            throw new NotImplementedException();
        }

        public override int GetScrollBarEvtPosition()
        {
            throw new NotImplementedException();
        }

        public override ScrollEventType GetScrollBarEvtKind()
        {
            throw new NotImplementedException();
        }

        public override Graphics OpenPaintDrawingContext()
        {
            return new WxGraphics(NativeControl.OpenPaintDrawingContext());
        }

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

            if (Control.IsGraphicControl)
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