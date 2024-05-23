using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class MauiControlHandler : DisposableObject, IControlHandler
    {
        private Control control;

        public MauiControlHandler()
        {
        }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public Control Control { get; }

        public Action? Idle { get; set; }

        public Action? Paint { get; set; }

        public Action? MouseEnter { get; set; }

        public Action? MouseLeave { get; set; }

        public Action? MouseClick { get; set; }

        public Action? VisibleChanged { get; set; }

        public Action? MouseCaptureLost { get; set; }

        public Action? GotFocus { get; set; }

        public Action? LostFocus { get; set; }

        public Action? DragLeave { get; set; }

        public Action? VerticalScrollBarValueChanged { get; set; }

        public Action? HorizontalScrollBarValueChanged { get; set; }

        public Action? SizeChanged { get; set; }

        public Action? Activated { get; set; }

        public Action? Deactivated { get; set; }

        public Action? HandleCreated { get; set; }

        public Action? HandleDestroyed { get; set; }

        public bool WantChars { get; set; }

        public bool ShowHorzScrollBar { get; set; }

        public bool ShowVertScrollBar { get; set; }

        public bool ScrollBarAlwaysVisible { get; set; }

        public LangDirection LangDirection { get; set; }

        public ControlBorderStyle BorderStyle { get; set; }

        public bool IsAttached { get; }

        public bool IsNativeControlCreated { get; }

        public bool IsFocused { get; }

        public Thickness IntrinsicLayoutPadding { get; }

        public Thickness IntrinsicPreferredSizePadding { get; }

        public bool IsScrollable { get; set; }

        public RectD Bounds { get; set; }

        public bool Visible { get; set; }

        public bool UserPaint { get; set; }

        public SizeD MinimumSize { get; set; }

        public SizeD MaximumSize { get; set; }

        public Font? Font { get; set; }

        public bool IsBold { get; set; }

        public bool TabStop { get; set; }

        public bool AllowDrop { get; set; }

        public bool AcceptsFocus { get; set; }

        public ControlBackgroundStyle BackgroundStyle { get; set; }

        public bool AcceptsFocusFromKeyboard { get; set; }

        public bool AcceptsFocusRecursively { get; set; }

        public bool AcceptsFocusAll { get; set; }

        public bool ProcessIdle { get; set; }

        public bool BindScrollEvents { get; set; }

        public SizeD ClientSize { get; set; }

        public bool CanAcceptFocus { get; }

        public bool IsMouseOver { get; }

        public bool ProcessUIUpdates { get; set; }

        public bool IsMouseCaptured { get; }

        public bool IsHandleCreated { get; }

        public bool IsFocusable { get; }

        public void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
        }

        public void Attach(Control control)
        {
        }

        public void BeginIgnoreRecreate()
        {
        }

        public void BeginInit()
        {
        }

        public bool BeginRepositioningChildren()
        {
            return default;
        }

        public void BeginUpdate()
        {
        }

        public void CaptureMouse()
        {
        }

        public void CenterOnParent(GenericOrientation direction)
        {
        }

        public PointD ClientToScreen(PointD point)
        {
            return default;
        }

        public Graphics OpenPaintDrawingContext()
        {
            return default;
        }

        public Graphics CreateDrawingContext()
        {
            return default;
        }

        public void Detach()
        {
        }

        public PointD DeviceToScreen(PointI point)
        {
            return default;
        }

        public void DisableRecreate()
        {
        }

        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return default;
        }

        public void EnableRecreate()
        {
        }

        public void EndIgnoreRecreate()
        {
        }

        public void EndInit()
        {
        }

        public void EndRepositioningChildren()
        {
        }

        public void EndUpdate()
        {
        }

        public void FocusNextControl(bool forward = true, bool nested = true)
        {
        }

        public Color GetDefaultAttributesBgColor()
        {
            return default;
        }

        public Color GetDefaultAttributesFgColor()
        {
            return default;
        }

        public Font? GetDefaultAttributesFont()
        {
            return default;
        }

        public SizeD GetDPI()
        {
            return default;
        }

        public nint GetHandle()
        {
            return default;
        }

        public object GetNativeControl()
        {
            return default;
        }

        public double GetPixelScaleFactor()
        {
            return 0;
        }

        public SizeD GetPreferredSize(SizeD availableSize)
        {
            return default;
        }

        public ScrollEventType GetScrollBarEvtKind()
        {
            return default;
        }

        public int GetScrollBarEvtPosition()
        {
            return 0;
        }

        public int GetScrollBarLargeChange(bool isVertical)
        {
            return 0;
        }

        public int GetScrollBarMaximum(bool isVertical)
        {
            return 0;
        }

        public int GetScrollBarValue(bool isVertical)
        {
            return 0;
        }

        public RectI GetUpdateClientRectI()
        {
            return default;
        }

        public void HandleNeeded()
        {
        }

        public void Invalidate()
        {
        }

        public bool IsScrollBarVisible(bool isVertical)
        {
            return false;
        }

        public bool IsTransparentBackgroundSupported()
        {
            return default;
        }

        public void Lower()
        {
        }

        public void OnLayoutChanged()
        {
        }

        public int PixelFromDip(double value)
        {
            return 0;
        }

        public double PixelFromDipF(double value)
        {
            return 0;
        }

        public double PixelToDip(int value)
        {
            return 0;
        }

        public void Raise()
        {
        }

        public void RaiseChildInserted(Control childControl)
        {
        }

        public void RaiseChildRemoved(Control childControl)
        {
        }

        public void RecreateWindow()
        {
        }

        public void RefreshRect(RectD rect, bool eraseBackground = true)
        {
        }

        public void ReleaseMouseCapture()
        {
        }

        public void ResetBackgroundColor()
        {
        }

        public void ResetForegroundColor()
        {
        }

        public void SaveScreenshot(string fileName)
        {
        }

        public PointD ScreenToClient(PointD point)
        {
            return PointD.Empty;
        }

        public PointI ScreenToDevice(PointD point)
        {
            return PointI.Empty;
        }

        public void SendMouseDownEvent(int x, int y)
        {
        }

        public void SendMouseUpEvent(int x, int y)
        {
        }

        public void SendSizeEvent()
        {
        }

        public void SetBounds(RectD rect, SetBoundsFlags flags)
        {
        }

        public void SetCursor(Cursor? value)
        {
        }

        public void SetEnabled(bool value)
        {
        }

        public bool SetFocus()
        {
            return false;
        }

        public void SetScrollBar(IControl control, bool isVertical, bool visible, int value, int largeChange, int maximum)
        {
        }

        public void SetToolTip(string? value)
        {
        }

        public void UnsetToolTip()
        {
        }

        public void Update()
        {
        }
    }
}
