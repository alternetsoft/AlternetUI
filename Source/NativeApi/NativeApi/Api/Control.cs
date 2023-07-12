using Alternet.UI;
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public abstract class Control
    {
        public event EventHandler? Paint { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseEnter { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeave { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseClick { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? VisibleChanged { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseCaptureLost { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? Destroyed { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<DragEventData>? DragDrop { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<DragEventData>? DragOver { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<DragEventData>? DragEnter { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler? DragLeave { add => throw new Exception(); remove => throw new Exception(); }

        public void SetMouseCapture(bool value) => throw new Exception();

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

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public void Invalidate() => throw new Exception();

        public void Update() => throw new Exception();

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public Font? Font { get; set; }

        public virtual Size GetPreferredSize(Size availableSize) => throw new Exception();

        public DragDropEffects DoDragDrop(UnmanagedDataObject data, DragDropEffects allowedEffects) => throw new Exception();

        public DrawingContext OpenPaintDrawingContext() => throw new Exception();
        public DrawingContext OpenClientDrawingContext() => throw new Exception();

        public void BeginUpdate() => throw new Exception();
        public void EndUpdate() => throw new Exception();

        public void RecreateWindow() => throw new Exception();

        public static Control? HitTest(Point screenPoint) => throw new Exception();

        public Point ClientToScreen(Point point) => throw new Exception();
        public Point ScreenToClient(Point point) => throw new Exception();

        public Int32Point ScreenToDevice(Point point) => throw new Exception();
        public Point DeviceToScreen(Int32Point point) => throw new Exception();

        public bool IsMouseCaptured { get; }

        public static Control? GetFocusedControl() => throw new Exception();
        public bool SetFocus() => throw new Exception();
        public bool TabStop { get; set; }
        public bool IsFocused { get; }
        public void FocusNextControl(bool forward, bool nested) => throw new Exception();
        public event EventHandler? GotFocus { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler? LostFocus { add => throw new Exception(); remove => throw new Exception(); }

        public void BeginInit() => throw new Exception();
        public void EndInit() => throw new Exception();

        public void Destroy() => throw new Exception();

        public IntPtr Handle { get => throw new Exception(); }

        public void SaveScreenshot(string fileName) => throw new Exception();

        public void SendSizeEvent() => throw new Exception();

        public bool IsScrollable { get; set; }

        public IntPtr GetContainingSizer() => throw new Exception();

        public IntPtr GetSizer() => throw new Exception();

        public void SetSizer(IntPtr sizer, bool deleteOld) => throw new Exception();

        public void SetSizerAndFit(IntPtr sizer, bool deleteOld) => throw new Exception();

        public void SetScrollBar(ScrollBarOrientation orientation, bool visible, int value, int largeChange, int maximum) => throw new Exception();

        public bool IsScrollBarVisible(ScrollBarOrientation orientation) => throw new Exception();
        public int GetScrollBarValue(ScrollBarOrientation orientation) => throw new Exception();
        public int GetScrollBarLargeChange(ScrollBarOrientation orientation) => throw new Exception();
        public int GetScrollBarMaximum(ScrollBarOrientation orientation) => throw new Exception();

        public event EventHandler? VerticalScrollBarValueChanged { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler? HorizontalScrollBarValueChanged { add => throw new Exception(); remove => throw new Exception(); }
    }
}