#pragma warning disable
using Alternet.UI;
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public abstract class Control
    {
        public void SetCursor(IntPtr handle) { }
        public bool AcceptsFocus { get; set; }
        public bool AcceptsFocusFromKeyboard { get; set; }
        public bool AcceptsFocusRecursively { get; set; }
        public bool AcceptsFocusAll { get; set; }

        public int BorderStyle { get; set; }
        public int LayoutDirection { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public event EventHandler? Paint;
        public event EventHandler? MouseEnter;
        public event EventHandler? MouseLeave;
        public event EventHandler? MouseClick;
        public event EventHandler? VisibleChanged;
        public event EventHandler? MouseCaptureLost;
        public event EventHandler? Destroyed;
        public event EventHandler? GotFocus;
        public event EventHandler? LostFocus;
        public event EventHandler? DragLeave;
        public event NativeEventHandler<DragEventData>? DragDrop;
        public event NativeEventHandler<DragEventData>? DragOver;
        public event NativeEventHandler<DragEventData>? DragEnter;
        public event EventHandler? VerticalScrollBarValueChanged;
        public event EventHandler? HorizontalScrollBarValueChanged;
        public event EventHandler? SizeChanged;

        public IntPtr Handle { get; }
        public IntPtr WxWidget { get; }
        public bool IsScrollable { get; set; }
        public bool IsMouseCaptured { get; }
        public bool TabStop { get; set; }
        public bool IsFocused { get; }
        public bool IsFocusable { get; }
        public bool CanAcceptFocus { get; }
        public Control? ParentRefCounted { get; }
        public string? ToolTip { get; set; }
        public bool AllowDrop { get; set; }
        public Size Size { get; set; }
        public Point Location { get; set; }
        public Rect Bounds { get; set; }
        public Size ClientSize { get; set; }
        public virtual Thickness IntrinsicLayoutPadding { get; }
        public virtual Thickness IntrinsicPreferredSizePadding { get; }
        public bool Visible { get; set; }
        public virtual bool Enabled { get; set; }
        public bool UserPaint { get; set; }
        public bool IsMouseOver { get; }
        public bool HasWindowCreated { get; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Font? Font { get; set; }
        public Size MinimumSize { get; set; }
        public Size MaximumSize { get; set; }

        public static Control? HitTest(Point screenPoint) => default;
        public static Control? GetFocusedControl() => default;
        public static void NotifyCaptureLost() { }
        public void Freeze() { }
        public void Thaw() { }
        public void ShowPopupMenu(IntPtr menu, int x, int y) { }
        public void BeginIgnoreRecreate() {}
        public void EndIgnoreRecreate() {}
        public Size GetDPI() => default;
        public void SetMouseCapture(bool value) { }
        public void AddChild(Control control) { }
        public void RemoveChild(Control control) { }
        public void Invalidate() { }
        public void Update() { }
        public virtual Size GetPreferredSize(Size availableSize) => default;
        public DragDropEffects DoDragDrop(UnmanagedDataObject data,
            DragDropEffects allowedEffects) => default;
        public DrawingContext OpenPaintDrawingContext() => default;
        public DrawingContext OpenClientDrawingContext() => default;
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void RecreateWindow() { }
        public void ResetBackgroundColor() { }
        public void ResetForegroundColor() { }
        public Point ClientToScreen(Point point) => default;
        public Point ScreenToClient(Point point) => default;
        public Int32Point ScreenToDevice(Point point) => default;
        public Point DeviceToScreen(Int32Point point) => default;
        public bool SetFocus() => default;
        public void FocusNextControl(bool forward, bool nested) { }
        public void BeginInit() { }
        public void EndInit() { }
        public void Destroy() { }
        public void SaveScreenshot(string fileName) { }
        public void SendSizeEvent() { }
        public void SendMouseDownEvent(int x, int y) { }
        public void SendMouseUpEvent(int x, int y) { }

        public IntPtr GetContainingSizer() => default;
        public IntPtr GetSizer() => default;
        public void SetSizer(IntPtr sizer, bool deleteOld) { }
        public void SetSizerAndFit(IntPtr sizer, bool deleteOld) { }

        public void SetScrollBar(ScrollBarOrientation orientation, bool visible, 
            int value, int largeChange, int maximum) { }
        public bool IsScrollBarVisible(ScrollBarOrientation orientation) => default;
        public int GetScrollBarValue(ScrollBarOrientation orientation) => default;
        public int GetScrollBarLargeChange(ScrollBarOrientation orientation) => default;
        public int GetScrollBarMaximum(ScrollBarOrientation orientation) => default;
    }
}