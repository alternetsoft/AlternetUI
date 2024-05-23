using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IControlHandler : IDisposableObject
    {
        Action? Idle { get; set; }

        Action? Paint { get; set; }

        Action? MouseEnter { get; set; }

        Action? MouseLeave { get; set; }

        Action? MouseClick { get; set; }

        Action? VisibleChanged { get; set; }

        Action? MouseCaptureLost { get; set; }

        Action? GotFocus { get; set; }

        Action? LostFocus { get; set; }

        Action? DragLeave { get; set; }

        Action? VerticalScrollBarValueChanged { get; set; }

        Action? HorizontalScrollBarValueChanged { get; set; }

        Action? SizeChanged { get; set; }

        Action? Activated { get; set; }

        Action? Deactivated { get; set; }

        Action? HandleCreated { get; set; }

        Action? HandleDestroyed { get; set; }

        bool WantChars { get; set; }

        bool ShowHorzScrollBar { get; set; }

        bool ShowVertScrollBar { get; set; }

        bool ScrollBarAlwaysVisible { get; set; }

        LangDirection LangDirection { get; set; }

        ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Gets a value indicating whether handler is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        bool IsAttached { get; }

        bool IsNativeControlCreated { get; }

        bool IsFocused { get; }

        Thickness IntrinsicLayoutPadding { get; }

        Thickness IntrinsicPreferredSizePadding { get; }

        bool IsScrollable { get; set; }

        RectD Bounds { get; set; }

        bool Visible { get; set; }

        bool UserPaint { get; set; }

        SizeD MinimumSize { get; set; }

        SizeD MaximumSize { get; set; }

        Color BackgroundColor { get; set; }

        Color ForegroundColor { get; set; }

        Font? Font { get; set; }

        bool IsBold { get; set; }

        bool TabStop { get; set; }

        bool AllowDrop { get; set; }

        bool AcceptsFocus { get; set; }

        ControlBackgroundStyle BackgroundStyle { get; set; }

        bool AcceptsFocusFromKeyboard { get; set; }

        bool AcceptsFocusRecursively { get; set; }

        bool AcceptsFocusAll { get; set; }

        bool ProcessIdle { get; set; }

        bool BindScrollEvents { get; set; }

        SizeD ClientSize { get; set; }

        bool CanAcceptFocus { get; }

        bool IsMouseOver { get; }

        bool ProcessUIUpdates { get; set; }

        bool IsMouseCaptured { get; }

        bool IsHandleCreated { get; }

        bool IsFocusable { get; }

        void Raise();

        void CenterOnParent(GenericOrientation direction);

        void SetCursor(Cursor? value);

        void SetToolTip(string? value);

        void Lower();

        void SendSizeEvent();

        void UnsetToolTip();

        void RefreshRect(RectD rect, bool eraseBackground = true);

        void HandleNeeded();

        void CaptureMouse();

        void ReleaseMouseCapture();

        void DisableRecreate();

        void EnableRecreate();

        Graphics CreateDrawingContext();

        PointD ScreenToClient(PointD point);

        PointD ClientToScreen(PointD point);

        PointI ScreenToDevice(PointD point);

        PointD DeviceToScreen(PointI point);

        void FocusNextControl(bool forward = true, bool nested = true);

        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

        void RecreateWindow();

        void BeginUpdate();

        void EndUpdate();

        void SetBounds(RectD rect, SetBoundsFlags flags);

        void BeginInit();

        void EndInit();

        bool SetFocus();

        void SaveScreenshot(string fileName);

        SizeD GetDPI();

        bool IsTransparentBackgroundSupported();

        void EndIgnoreRecreate();

        void BeginIgnoreRecreate();

        double GetPixelScaleFactor();

        RectI GetUpdateClientRectI();

        double PixelToDip(int value);

        int PixelFromDip(double value);

        double PixelFromDipF(double value);

        void SetScrollBar(
            IControl control,
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum);

        bool IsScrollBarVisible(bool isVertical);

        int GetScrollBarValue(bool isVertical);

        int GetScrollBarLargeChange(bool isVertical);

        int GetScrollBarMaximum(bool isVertical);

        void ResetBackgroundColor();

        void ResetForegroundColor();

        void SetEnabled(bool value);

        Color GetDefaultAttributesBgColor();

        Color GetDefaultAttributesFgColor();

        Font? GetDefaultAttributesFont();

        void SendMouseDownEvent(int x, int y);

        void SendMouseUpEvent(int x, int y);

        bool BeginRepositioningChildren();

        void EndRepositioningChildren();

        void AlwaysShowScrollbars(bool hflag = true, bool vflag = true);

        void Update();

        void Invalidate();

        IntPtr GetHandle();

        SizeD GetPreferredSize(SizeD availableSize);

        int GetScrollBarEvtPosition();

        ScrollEventType GetScrollBarEvtKind();

        Graphics OpenPaintDrawingContext();

        object GetNativeControl();

        void RaiseChildInserted(Control childControl);

        void RaiseChildRemoved(Control childControl);

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        void Attach(Control control);

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        void Detach();

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        void OnLayoutChanged();
    }
}
