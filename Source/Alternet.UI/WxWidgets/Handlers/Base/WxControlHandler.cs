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
    internal class WxControlHandler : BaseControlHandler, IControlHandler
    {
        private Native.Control? nativeControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public WxControlHandler()
        {
        }

        public Action? Idle
        {
            get => NativeControl.Idle;
            set => NativeControl.Idle = value;
        }

        public Action? Paint
        {
            get => NativeControl.Paint;
            set => NativeControl.Paint = value;
        }

        public Action? MouseEnter
        {
            get => NativeControl.MouseEnter;
            set => NativeControl.MouseEnter = value;
        }

        public Action? MouseLeave
        {
            get => NativeControl.MouseLeave;
            set => NativeControl.MouseLeave = value;
        }

        public Action? MouseClick
        {
            get => NativeControl.MouseClick;
            set => NativeControl.MouseClick = value;
        }

        public Action? VisibleChanged
        {
            get => NativeControl.VisibleChanged;
            set => NativeControl.VisibleChanged = value;
        }

        public Action? MouseCaptureLost
        {
            get => NativeControl.MouseCaptureLost;
            set => NativeControl.MouseCaptureLost = value;
        }

        public Action? GotFocus
        {
            get => NativeControl.GotFocus;
            set => NativeControl.GotFocus = value;
        }

        public Action? LostFocus
        {
            get => NativeControl.LostFocus;
            set => NativeControl.LostFocus = value;
        }

        public Action? DragLeave
        {
            get => NativeControl.DragLeave;
            set => NativeControl.DragLeave = value;
        }

        public Action? VerticalScrollBarValueChanged
        {
            get => NativeControl.VerticalScrollBarValueChanged;
            set => NativeControl.VerticalScrollBarValueChanged = value;
        }

        public Action? HorizontalScrollBarValueChanged
        {
            get => NativeControl.HorizontalScrollBarValueChanged;
            set => NativeControl.HorizontalScrollBarValueChanged = value;
        }

        public Action? SizeChanged
        {
            get => NativeControl.SizeChanged;
            set => NativeControl.SizeChanged = value;
        }

        public Action? Activated
        {
            get => NativeControl.Activated;
            set => NativeControl.Activated = value;
        }

        public Action? Deactivated
        {
            get => NativeControl.Deactivated;
            set => NativeControl.Deactivated = value;
        }

        public Action? HandleCreated
        {
            get => NativeControl.HandleCreated;
            set => NativeControl.HandleCreated = value;
        }

        public Action? HandleDestroyed
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
        public bool IsNativeControlCreated
        {
            get => nativeControl is not null;
        }

        public LangDirection LangDirection
        {
            get => (LangDirection)NativeControl.LayoutDirection;
            set => NativeControl.LayoutDirection = (int)value;
        }

        public ControlBorderStyle BorderStyle
        {
            get => (ControlBorderStyle)NativeControl.BorderStyle;
            set => NativeControl.BorderStyle = (int)value;
        }

        public bool IsFocused => NativeControl.IsFocused;

        public bool IsScrollable
        {
            get => NativeControl.IsScrollable;
            set => NativeControl.IsScrollable = value;
        }

        public RectD Bounds
        {
            get => NativeControl.Bounds;
            set => NativeControl.Bounds = value;
        }

        public Thickness IntrinsicLayoutPadding
        {
            get => NativeControl.IntrinsicLayoutPadding;
        }

        public Thickness IntrinsicPreferredSizePadding
        {
            get => NativeControl.IntrinsicPreferredSizePadding;
        }

        public bool Visible
        {
            get => NativeControl.Visible;
            set => NativeControl.Visible = value;
        }

        public bool UserPaint
        {
            get => NativeControl.UserPaint;
            set => NativeControl.UserPaint = value;
        }

        public SizeD MinimumSize
        {
            get => NativeControl.MinimumSize;
            set => NativeControl.MinimumSize = value;
        }

        public SizeD MaximumSize
        {
            get => NativeControl.MaximumSize;
            set => NativeControl.MaximumSize = value;
        }

        public Color BackgroundColor
        {
            get => NativeControl.BackgroundColor;
            set => NativeControl.BackgroundColor = value;
        }

        public Color ForegroundColor
        {
            get => NativeControl.ForegroundColor;
            set => NativeControl.ForegroundColor = value;
        }

        public Font? Font
        {
            get => Font.FromInternal(NativeControl.Font);
            set => NativeControl.Font = (UI.Native.Font?)value?.Handler;
        }

        public bool IsBold
        {
            get => NativeControl.IsBold;
            set => NativeControl.IsBold = value;
        }

        public bool WantChars
        {
            get => (NativeControl as Native.Panel)?.WantChars ?? true;

            set
            {
                if (NativeControl is Native.Panel panel)
                    panel.WantChars = value;
            }
        }

        public bool ShowHorzScrollBar
        {
            get => (NativeControl as Native.Panel)?.ShowHorzScrollBar ?? false;

            set
            {
                if (NativeControl is Native.Panel panel)
                    panel.ShowHorzScrollBar = value;
            }
        }

        public bool ShowVertScrollBar
        {
            get => (NativeControl as Native.Panel)?.ShowVertScrollBar ?? false;

            set
            {
                if (NativeControl is Native.Panel panel)
                    panel.ShowVertScrollBar = value;
            }
        }

        public bool ScrollBarAlwaysVisible
        {
            get => (NativeControl as Native.Panel)?.ScrollBarAlwaysVisible ?? false;

            set
            {
                if (NativeControl is Native.Panel panel)
                    panel.ScrollBarAlwaysVisible = value;
            }
        }

        public bool TabStop
        {
            get => NativeControl.TabStop;
            set => NativeControl.TabStop = value;
        }

        public bool AllowDrop
        {
            get => NativeControl.AllowDrop;
            set => NativeControl.AllowDrop = value;
        }

        public bool AcceptsFocus
        {
            get => NativeControl.AcceptsFocus;
            set => NativeControl.AcceptsFocus = value;
        }

        public ControlBackgroundStyle BackgroundStyle
        {
            get
            {
                var result = NativeControl.GetBackgroundStyle();
                return (ControlBackgroundStyle?)result ?? ControlBackgroundStyle.System;
            }

            set => NativeControl.SetBackgroundStyle((int)value);
        }

        public bool AcceptsFocusFromKeyboard
        {
            get => NativeControl.AcceptsFocusFromKeyboard;
            set => NativeControl.AcceptsFocusFromKeyboard = value;
        }

        public bool AcceptsFocusRecursively
        {
            get => NativeControl.AcceptsFocusRecursively;
            set => NativeControl.AcceptsFocusRecursively = value;
        }

        public bool AcceptsFocusAll
        {
            get => NativeControl.AcceptsFocusAll;
            set => NativeControl.AcceptsFocusAll = value;
        }

        public bool ProcessIdle
        {
            get => NativeControl.ProcessIdle;
            set => NativeControl.ProcessIdle = value;
        }

        public bool BindScrollEvents
        {
            get => NativeControl.BindScrollEvents;
            set => NativeControl.BindScrollEvents = value;
        }

        public SizeD ClientSize
        {
            get => NativeControl.ClientSize;
            set => NativeControl.ClientSize = value;
        }

        public bool CanAcceptFocus => NativeControl.CanAcceptFocus;

        public bool IsMouseOver => NativeControl.IsMouseOver;

        public bool ProcessUIUpdates
        {
            get => NativeControl.ProcessUIUpdates;
            set => NativeControl.ProcessUIUpdates = value;
        }

        public bool IsMouseCaptured => NativeControl.IsMouseCaptured;

        public bool IsHandleCreated => NativeControl.IsHandleCreated;

        public bool IsFocusable => NativeControl.IsFocusable;

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

        public object GetNativeControl() => NativeControl;

        public override void OnLayoutChanged()
        {
            base.OnLayoutChanged();
        }

        public void Raise()
        {
            NativeControl.Raise();
        }

        public void CenterOnParent(GenericOrientation direction)
        {
            NativeControl.CenterOnParent((int)direction);
        }

        public void SetCursor(Cursor? value)
        {
            if (value is null)
                NativeControl.SetCursor(default);
            else
                NativeControl.SetCursor((IntPtr)value.Handler);
        }

        public void SetToolTip(string? value)
        {
            NativeControl.ToolTip = value;
        }

        public void Lower()
        {
            NativeControl.Lower();
        }

        public void SendSizeEvent()
        {
            NativeControl.SendSizeEvent();
        }

        public void UnsetToolTip()
        {
            NativeControl.UnsetToolTip();
        }

        public void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            NativeControl.RefreshRect(rect, eraseBackground);
        }

        public void HandleNeeded()
        {
            NativeControl.Required();
        }

        public void CaptureMouse()
        {
            NativeControl.SetMouseCapture(true);
        }

        public void ReleaseMouseCapture()
        {
            NativeControl.SetMouseCapture(false);
        }

        public void DisableRecreate()
        {
            NativeControl.DisableRecreate();
        }

        public void EnableRecreate()
        {
            NativeControl.EnableRecreate();
        }

        public Graphics CreateDrawingContext()
        {
            return new WxGraphics(NativeControl.OpenClientDrawingContext());
        }

        public PointD ScreenToClient(PointD point)
        {
            return NativeControl.ScreenToClient(point);
        }

        public PointD ClientToScreen(PointD point)
        {
            return NativeControl.ClientToScreen(point);
        }

        public PointI ScreenToDevice(PointD point)
        {
            return NativeControl.ScreenToDevice(point);
        }

        public PointD DeviceToScreen(PointI point)
        {
            return NativeControl.DeviceToScreen(point);
        }

        public void FocusNextControl(bool forward = true, bool nested = true)
        {
            NativeControl.FocusNextControl(forward, nested);
        }

        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            allowedEffects &= ~DragDropEffects.Scroll;
            return (DragDropEffects)NativeControl.DoDragDrop(
                UnmanagedDataObjectService.GetUnmanagedDataObject(data),
                (Native.DragDropEffects)allowedEffects);
        }

        public void RecreateWindow()
        {
            NativeControl.RecreateWindow();
        }

        public void BeginUpdate()
        {
            NativeControl.BeginUpdate();
        }

        public void EndUpdate()
        {
            NativeControl.EndUpdate();
        }

        public void SetBounds(RectD rect, SetBoundsFlags flags)
        {
            NativeControl.SetBoundsEx(rect, (int)flags);
        }

        public void BeginInit()
        {
            NativeControl.BeginInit();
        }

        public void EndInit()
        {
            NativeControl.EndInit();
        }

        public bool SetFocus()
        {
            return NativeControl.SetFocus();
        }

        public void SaveScreenshot(string fileName)
        {
            NativeControl.SaveScreenshot(fileName);
        }

        public SizeD GetDPI()
        {
            return NativeControl.GetDPI();
        }

        public bool IsTransparentBackgroundSupported()
        {
            return NativeControl.IsTransparentBackgroundSupported();
        }

        public void EndIgnoreRecreate()
        {
            NativeControl.EndIgnoreRecreate();
        }

        public void BeginIgnoreRecreate()
        {
            NativeControl.BeginIgnoreRecreate();
        }

        public double GetPixelScaleFactor()
        {
            return Native.Control.DrawingDPIScaleFactor(NativeControl.WxWidget);
        }

        public RectI GetUpdateClientRectI()
        {
            return NativeControl.GetUpdateClientRect();
        }

        public double PixelToDip(int value)
        {
            return Native.Control.DrawingToDip(value, NativeControl.WxWidget);
        }

        public int PixelFromDip(double value)
        {
            return Native.Control.DrawingFromDip(value, NativeControl.WxWidget);
        }

        public double PixelFromDipF(double value)
        {
            return Native.Control.DrawingFromDipF(value, NativeControl.WxWidget);
        }

        public void SetScrollBar(
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

        public bool IsScrollBarVisible(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.IsScrollBarVisible(orientation);
        }

        public int GetScrollBarValue(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarValue(orientation);
        }

        public int GetScrollBarLargeChange(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarLargeChange(orientation);
        }

        public int GetScrollBarMaximum(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarMaximum(orientation);
        }

        public void ResetBackgroundColor()
        {
            NativeControl.ResetBackgroundColor();
        }

        public void ResetForegroundColor()
        {
            NativeControl.ResetForegroundColor();
        }

        public void SetEnabled(bool value)
        {
            NativeControl.Enabled = value;
        }

        public Color GetDefaultAttributesBgColor()
        {
            return NativeControl.GetDefaultAttributesBgColor();
        }

        public Color GetDefaultAttributesFgColor()
        {
            return NativeControl.GetDefaultAttributesFgColor();
        }

        public Font? GetDefaultAttributesFont()
        {
            return Font.FromInternal(NativeControl.GetDefaultAttributesFont());
        }

        public void SendMouseDownEvent(int x, int y)
        {
            NativeControl.SendMouseDownEvent(x, y);
        }

        public void SendMouseUpEvent(int x, int y)
        {
            NativeControl.SendMouseUpEvent(x, y);
        }

        public bool BeginRepositioningChildren()
        {
            return NativeControl.BeginRepositioningChildren();
        }

        public void EndRepositioningChildren()
        {
            NativeControl.EndRepositioningChildren();
        }

        public void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
            NativeControl.AlwaysShowScrollbars(hflag, vflag);
        }

        public void Update()
        {
            NativeControl.Update();
        }

        public void Invalidate()
        {
            NativeControl.Invalidate();
        }

        public nint GetHandle()
        {
            return NativeControl.Handle;
        }

        public SizeD GetPreferredSize(SizeD availableSize)
        {
            return NativeControl.GetPreferredSize(availableSize);
        }

        public int GetScrollBarEvtPosition()
        {
            return NativeControl.GetScrollBarEvtPosition();
        }

        public ScrollEventType GetScrollBarEvtKind()
        {
            return (ScrollEventType)NativeControl.GetScrollBarEvtKind();
        }

        public Graphics OpenPaintDrawingContext()
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