using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal abstract class BaseControlHandler : DisposableObject, IControlHandler
    {
        private Control? control;

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        public abstract Action? Idle { get; set; }

        public abstract Action? Paint { get; set; }

        public abstract Action? MouseEnter { get; set; }

        public abstract Action? MouseLeave { get; set; }

        public abstract Action? MouseClick { get; set; }

        public abstract Action? VisibleChanged { get; set; }

        public abstract Action? MouseCaptureLost { get; set; }

        public abstract Action? GotFocus { get; set; }

        public abstract Action? LostFocus { get; set; }

        public abstract Action? DragLeave { get; set; }

        public abstract Action? VerticalScrollBarValueChanged { get; set; }

        public abstract Action? HorizontalScrollBarValueChanged { get; set; }

        public abstract Action? SizeChanged { get; set; }

        public abstract Action? Activated { get; set; }

        public abstract Action? Deactivated { get; set; }

        public abstract Action? HandleCreated { get; set; }

        public abstract Action? HandleDestroyed { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WxControlHandler"/> is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => control != null;

        public abstract bool IsNativeControlCreated { get; }

        public abstract LangDirection LangDirection { get; set; }

        public abstract ControlBorderStyle BorderStyle { get; set; }

        public abstract bool WantChars { get; set; }

        public abstract bool IsFocused { get; }

        public abstract bool ShowHorzScrollBar { get; set; }

        public abstract bool ShowVertScrollBar { get; set; }

        public abstract bool ScrollBarAlwaysVisible { get; set; }

        public abstract bool IsScrollable { get; set; }

        public abstract RectD Bounds { get; set; }

        public abstract bool Visible { get; set; }

        public abstract bool UserPaint { get; set; }

        public abstract SizeD MinimumSize { get; set; }

        public abstract SizeD MaximumSize { get; set; }

        public abstract Color BackgroundColor { get; set; }

        public abstract Color ForegroundColor { get; set; }

        public abstract Font? Font { get; set; }

        public abstract bool IsBold { get; set; }

        public abstract bool TabStop { get; set; }

        public abstract bool AllowDrop { get; set; }

        public abstract bool AcceptsFocus { get; set; }

        public abstract ControlBackgroundStyle BackgroundStyle { get; set; }

        public abstract bool AcceptsFocusFromKeyboard { get; set; }

        public abstract bool AcceptsFocusRecursively { get; set; }

        public abstract bool AcceptsFocusAll { get; set; }

        public abstract bool ProcessIdle { get; set; }

        public abstract bool BindScrollEvents { get; set; }

        public abstract SizeD ClientSize { get; set; }

        public abstract bool CanAcceptFocus { get; }

        public abstract bool IsMouseOver { get; }

        public abstract bool ProcessUIUpdates { get; set; }

        public abstract bool IsMouseCaptured { get; }

        public abstract bool IsHandleCreated { get; }

        public abstract bool IsFocusable { get; }

        public abstract object GetNativeControl();

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
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public virtual void Detach()
        {
            OnDetach();

            control = null;
        }

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        public virtual void OnLayoutChanged()
        {
        }

        public abstract void Raise();

        public abstract void CenterOnParent(GenericOrientation direction);

        public abstract void SetCursor(Cursor? value);

        public abstract void SetToolTip(string? value);

        public abstract void Lower();

        public abstract void SendSizeEvent();

        public abstract void UnsetToolTip();

        public abstract Thickness GetIntrinsicLayoutPadding();

        public abstract Thickness GetIntrinsicPreferredSizePadding();

        public abstract void RefreshRect(RectD rect, bool eraseBackground = true);

        public abstract void HandleNeeded();

        public abstract void CaptureMouse();

        public abstract void ReleaseMouseCapture();

        public abstract void DisableRecreate();

        public abstract void EnableRecreate();

        public abstract Graphics CreateDrawingContext();

        public abstract PointD ScreenToClient(PointD point);

        public abstract PointD ClientToScreen(PointD point);

        public abstract PointI ScreenToDevice(PointD point);

        public abstract PointD DeviceToScreen(PointI point);

        public abstract void FocusNextControl(bool forward = true, bool nested = true);

        public abstract DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

        public abstract void RecreateWindow();

        public abstract void BeginUpdate();

        public abstract void EndUpdate();

        public abstract void SetBounds(RectD rect, SetBoundsFlags flags);

        public abstract void BeginInit();

        public abstract void EndInit();

        public abstract bool SetFocus();

        public abstract void SaveScreenshot(string fileName);

        public abstract SizeD GetDPI();

        public abstract bool IsTransparentBackgroundSupported();

        public abstract void EndIgnoreRecreate();

        public abstract void BeginIgnoreRecreate();

        public abstract double GetPixelScaleFactor();

        public abstract RectI GetUpdateClientRectI();

        public abstract double PixelToDip(int value);

        public abstract int PixelFromDip(double value);

        public abstract double PixelFromDipF(double value);

        public abstract void SetScrollBar(IControl control, bool isVertical, bool visible, int value, int largeChange, int maximum);

        public abstract bool IsScrollBarVisible(bool isVertical);

        public abstract int GetScrollBarValue(bool isVertical);

        public abstract int GetScrollBarLargeChange(bool isVertical);

        public abstract int GetScrollBarMaximum(bool isVertical);

        public abstract void ResetBackgroundColor();

        public abstract void ResetForegroundColor();

        public abstract void SetEnabled(bool value);

        public abstract Color GetDefaultAttributesBgColor();

        public abstract Color GetDefaultAttributesFgColor();

        public abstract Font? GetDefaultAttributesFont();

        public abstract void SendMouseDownEvent(int x, int y);

        public abstract void SendMouseUpEvent(int x, int y);

        public abstract bool BeginRepositioningChildren();

        public abstract void EndRepositioningChildren();

        public abstract void AlwaysShowScrollbars(bool hflag = true, bool vflag = true);

        public abstract void Update();

        public abstract void Invalidate();

        public abstract nint GetHandle();

        public abstract SizeD GetPreferredSize(SizeD availableSize);

        public abstract int GetScrollBarEvtPosition();

        public abstract ScrollEventType GetScrollBarEvtKind();

        public abstract Graphics OpenPaintDrawingContext();

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into the
        /// <see cref="Control.Children"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected abstract void OnDetach();

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected abstract void OnAttach();
    }
}
