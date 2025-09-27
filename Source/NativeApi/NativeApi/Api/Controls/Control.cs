#pragma warning disable
using Alternet.UI;
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    using Coord = float;

    public abstract class Control
    {
        public IntPtr GetCGContextRef() { return default; }

        public void SetAllowDefaultContextMenu(bool value) { }

        public void SetScrollBar(ScrollBarOrientation orientation, HiddenOrVisible visible,
            int value, int largeChange, int maximum)
        { }
        public bool IsScrollBarVisible(ScrollBarOrientation orientation) => default;
        public int GetScrollBarValue(ScrollBarOrientation orientation) => default;
        public int GetScrollBarLargeChange(ScrollBarOrientation orientation) => default;
        public int GetScrollBarMaximum(ScrollBarOrientation orientation) => default;

        public static IntPtr CreateControl() => default;

        public bool WantChars { get; set; }

        public bool EnableTouchEvents(int flag) => default;

        public bool BindScrollEvents { get; set; }
        public bool BeginRepositioningChildren() => default;
        public void EndRepositioningChildren() { }

        public RectI GetUpdateClientRect() => default;

        public static DrawingContext OpenClientDrawingContextForWindow(IntPtr window) => default;
        public static DrawingContext OpenPaintDrawingContextForWindow(IntPtr window) => default;
        public static DrawingContext OpenDrawingContextForDC(IntPtr dc, bool deleteDc) => default;

        public void CenterOnParent(int orientation) { }

        public void RefreshRect(RectD rect, bool eraseBackground = true) { }

        public void Raise() { }
        public void Lower() { }

        public void DisableRecreate() { }
        public void EnableRecreate() { }
        public void UnsetToolTip() { }
        public bool IsTransparentBackgroundSupported() => default;
        public bool SetBackgroundStyle(int style) => default;
        public int GetBackgroundStyle() => default;
        public Color GetDefaultAttributesBgColor() => default;
        public Color GetDefaultAttributesFgColor() => default;
        public Font GetDefaultAttributesFont() => default;
        public static Color GetClassDefaultAttributesBgColor(int controlType, int windowVariant)
            => default;
        public static Color GetClassDefaultAttributesFgColor(int controlType, int windowVariant)
            => default;
        public static Font GetClassDefaultAttributesFont(int controlType, int windowVariant)
            => default;

        public static int DrawingFromDip(Coord value, IntPtr window) => default;
        public static Coord DrawingDPIScaleFactor(IntPtr window) => default;
        public static Coord DrawingToDip(int value, IntPtr window) => default;
        public static Coord DrawingFromDipF(Coord value, IntPtr window) => default;

        public bool ProcessIdle { get; set; }
        public bool ProcessUIUpdates { get; set; }

        public bool IsBold { get; set; }
        public void SetCursor(IntPtr handle) { }
        public bool AcceptsFocus { get; set; }
        public bool AcceptsFocusFromKeyboard { get; set; }
        public bool AcceptsFocusRecursively { get; set; }
        public bool AcceptsFocusAll { get; set; }

        public int BorderStyle { get; set; }
        public int LayoutDirection { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public SizeI EventOldDpi { get; }

        public SizeI EventNewDpi { get; }

        public Control? EventFocusedControl { get; }

        public event EventHandler? Idle;
        public event EventHandler? Paint;
        public event EventHandler? MouseClick;
        public event EventHandler? VisibleChanged;
        public event EventHandler? MouseCaptureLost;
        public event EventHandler? DpiChanged;
        public event EventHandler? RequestCursor;

        public event EventHandler? Destroyed;
        public event EventHandler? TextChanged;
        public event EventHandler? GotFocus;
        public event EventHandler? LostFocus;
        public event EventHandler? DragLeave;
        public event NativeEventHandler<DragEventData>? DragDrop;
        public event NativeEventHandler<DragEventData>? DragOver;
        public event NativeEventHandler<DragEventData>? DragEnter;
        public event EventHandler? VerticalScrollBarValueChanged;
        public event EventHandler? HorizontalScrollBarValueChanged;
        public event EventHandler? SizeChanged;
        public event EventHandler? LocationChanged;
        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler? HandleCreated;
        public event EventHandler? BeforeHandleDestroyed;
        public event EventHandler? HandleDestroyed;
        public event EventHandler? SystemColorsChanged;

        public virtual string Text { get; set; }
        public bool IsActive { get; }
        public bool IsHandleCreated { get; }
        public bool IsWxWidgetCreated { get; }

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
        public SizeD Size { get; set; }
        public PointD Location { get; set; }
        public virtual RectD Bounds { get; set; }
        public virtual RectI BoundsI { get; set; }
        public RectD EventBounds { get; }

        public SizeD ClientSize { get; set; }

        public virtual SizeD AutoPaddingLeftTop { get; }

        public virtual SizeD AutoPaddingRightBottom { get; }

        public bool Visible { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual bool UserPaint { get; set; }
        public bool IsMouseOver { get; }
        public bool HasWindowCreated { get; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Font? Font { get; set; }

        public static Control? HitTest(PointD screenPoint) => default;
        public static Control? GetFocusedControl() => default;
        public static void NotifyCaptureLost() { }
        public void Freeze() { }
        public void Thaw() { }
        public void BeginIgnoreRecreate() {}
        public void EndIgnoreRecreate() {}
        public SizeD GetDPI() => default;
        public void SetMouseCapture(bool value) { }
        public void AddChild(Control control) { }
        public void RemoveChild(Control control) { }
        public void Invalidate() { }
        public void Update() { }

        public virtual void InvalidateBestSize() { }

        public virtual SizeD GetPreferredSize(SizeD availableSize) => default;

        public void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively) { }

        public DragDropEffects DoDragDrop(UnmanagedDataObject data,
            DragDropEffects allowedEffects) => default;
        public DrawingContext OpenPaintDrawingContext() => default;
        public DrawingContext OpenClientDrawingContext() => default;
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void RecreateWindow() { }
        public void ResetBackgroundColor() { }
        public void ResetForegroundColor() { }
        public PointD ClientToScreen(PointD point) => default;
        public PointD ScreenToClient(PointD point) => default;
        public PointI ScreenToDevice(PointD point) => default;
        public PointD DeviceToScreen(PointI point) => default;
        public bool SetFocus() => default;
        public void FocusNextControl(bool forward, bool nested) { }
        public void BeginInit() { }
        public void EndInit() { }
        public void Destroy() { }
        public void SaveScreenshot(string fileName) { }
        public void SendSizeEvent() { }
        public void SendMouseDownEvent(int x, int y) { }
        public void SendMouseUpEvent(int x, int y) { }

        public void SetBoundsEx(RectD rect, int flags) { }

        public IntPtr GetContainingSizer() => default;
        public IntPtr GetSizer() => default;
        public void SetSizer(IntPtr sizer, bool deleteOld) { }
        public void SetSizerAndFit(IntPtr sizer, bool deleteOld) { }

        public int GetScrollBarEvtKind() => default;
        public int GetScrollBarEvtPosition() => default;
    }
}