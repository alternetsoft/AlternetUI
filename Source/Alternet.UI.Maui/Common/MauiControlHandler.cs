using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;

using SkiaSharp;

namespace Alternet.UI
{
    internal class MauiControlHandler : BaseControlHandler, IControlHandler
    {
        public static Color DefaultBackgroundColor = SystemColors.Window;

        public static Color DefaultForegroundColor = SystemColors.WindowText;

        private SkiaContainer? container;
        private Color backgroundColor = DefaultBackgroundColor;
        private Color foregroundColor = DefaultForegroundColor;

        public MauiControlHandler()
        {
        }

        public virtual bool UserPaint
        {
            get => true;

            set
            {
            }
        }

        public virtual bool IsNativeControlCreated
        {
            get => true;

            set
            {
            }
        }

        public virtual bool IsHandleCreated
        {
            get => true;

            set
            {
            }
        }

        public virtual bool IsFocused
        {
            get => Control.FocusedControl == container?.Control;

            set
            {
                if (IsFocused == value)
                    return;
                Control.FocusedControl?.RaiseLostFocus();
                Control.FocusedControl = container?.Control;
                Control.FocusedControl?.RaiseGotFocus();
            }
        }

        public SkiaContainer? Container
        {
            get => container;

            set => container = value;
        }

        public Action<DragEventArgs>? DragDrop { get; set; }

        public Action<DragEventArgs>? DragOver { get; set; }

        public Action<DragEventArgs>? DragEnter { get; set; }

        public Action? Idle { get; set; }

        public virtual string Text { get; set; } = string.Empty;

        public Action? TextChanged { get; set; }

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

        public Action? LocationChanged { get; set; }

        public Action? Activated { get; set; }

        public Action? Deactivated { get; set; }

        public Action? HandleCreated { get; set; }

        public Action? HandleDestroyed { get; set; }

        public virtual bool WantChars { get; set; }

        public virtual bool ShowHorzScrollBar { get; set; }

        public virtual bool ShowVertScrollBar { get; set; }

        public virtual bool ScrollBarAlwaysVisible { get; set; }

        public virtual LangDirection LangDirection { get; set; }

        public virtual ControlBorderStyle BorderStyle { get; set; }

        public virtual Thickness IntrinsicLayoutPadding { get; set; }

        public virtual Thickness IntrinsicPreferredSizePadding { get; set; }

        public virtual bool IsScrollable { get; set; }

        public virtual RectD Bounds { get; set; }

        public virtual RectD EventBounds { get; set; }

        public virtual bool Visible { get; set; }

        public virtual SizeD MinimumSize { get; set; }

        public virtual SizeD MaximumSize { get; set; }

        public virtual Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        public virtual Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        public virtual Font? Font { get; set; }

        public virtual bool IsBold { get; set; }

        public virtual bool TabStop
        {
            get => true;

            set
            {
            }
        }

        public virtual bool AllowDrop { get; set; }

        public virtual ControlBackgroundStyle BackgroundStyle { get; set; }

        public virtual bool ProcessIdle { get; set; }

        public virtual bool BindScrollEvents { get; set; }

        public virtual SizeD ClientSize
        {
            get => Bounds.Size;
            set => Bounds = (Bounds.Location, value);
        }

        public virtual bool ProcessUIUpdates { get; set; }

        public virtual bool IsMouseCaptured { get; set; }

        public Action? SystemColorsChanged { get; set; }

        public SizeI EventOldDpi { get; }

        public SizeI EventNewDpi { get; }

        public Action? DpiChanged { get; set; }

        public bool CanSelect
        {
            get => true;
        }

        public virtual void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
        }

        public virtual void BeginInit()
        {
        }

        public virtual void BeginUpdate()
        {
        }

        public virtual void CaptureMouse()
        {
        }

        public virtual void CenterOnParent(GenericOrientation direction)
        {
        }

        public virtual PointD ScreenToClient(PointD point)
        {
            return MauiApplicationHandler.ScreenToClient(point, Control);
        }

        public virtual PointD ClientToScreen(PointD point)
        {
            return MauiApplicationHandler.ClientToScreen(point, Control);
        }

        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return default;
        }

        public virtual void EndInit()
        {
        }

        public virtual void EndUpdate()
        {
        }

        public virtual void FocusNextControl(bool forward = true, bool nested = true)
        {
        }

        public virtual Color GetDefaultAttributesBgColor()
        {
            return SystemColors.Window;
        }

        public virtual Color GetDefaultAttributesFgColor()
        {
            return SystemColors.WindowText;
        }

        public virtual Font? GetDefaultAttributesFont()
        {
            return default;
        }

        public virtual SizeD GetDPI()
        {
            return 96 * GetPixelScaleFactor();
        }

        public virtual nint GetHandle()
        {
            return default;
        }

        public virtual object GetNativeControl()
        {
            return AssemblyUtils.Default;
        }

        public virtual Coord GetPixelScaleFactor()
        {
            return Display.Primary.ScaleFactor;
        }

        public virtual SizeD GetPreferredSize(SizeD availableSize)
        {
            return availableSize;
        }

        public virtual ScrollEventType GetScrollBarEvtKind()
        {
            return default;
        }

        public virtual int GetScrollBarEvtPosition()
        {
            return default;
        }

        public virtual int GetScrollBarLargeChange(bool isVertical)
        {
            return default;
        }

        public virtual int GetScrollBarMaximum(bool isVertical)
        {
            return default;
        }

        public virtual int GetScrollBarValue(bool isVertical)
        {
            return default;
        }

        public virtual RectI GetUpdateClientRectI()
        {
            return new RectI((0, 0), Control.PixelFromDip(ClientSize));
        }

        public virtual void HandleNeeded()
        {
        }

        public virtual bool IsScrollBarVisible(bool isVertical)
        {
            return default;
        }

        public virtual bool IsTransparentBackgroundSupported()
        {
            return default;
        }

        public virtual void Lower()
        {
        }

        public virtual void OnChildInserted(Control childControl)
        {
        }

        public virtual void OnChildRemoved(Control childControl)
        {
        }

        public virtual Graphics OpenPaintDrawingContext()
        {
            return PlessGraphics.Default;
        }

        public virtual int PixelFromDip(Coord value)
        {
            return GraphicsFactory.PixelFromDip(value, Control.ScaleFactor);
        }

        public virtual Coord PixelToDip(int value)
        {
            return GraphicsFactory.PixelToDip(value, Control.ScaleFactor);
        }

        public virtual void Raise()
        {
        }

        public virtual void RecreateWindow()
        {
        }

        public virtual void ReleaseMouseCapture()
        {
        }

        public virtual void ResetBackgroundColor()
        {
            BackgroundColor = DefaultBackgroundColor;
        }

        public virtual void ResetForegroundColor()
        {
            ForegroundColor = DefaultForegroundColor;
        }

        public virtual void SaveScreenshot(string fileName)
        {
        }

        public virtual void SetCursor(Cursor? value)
        {
        }

        public virtual void SetEnabled(bool value)
        {
            if (container is not null)
                container.IsEnabled = value;
        }

        public virtual bool SetFocus()
        {
            return true;
        }

        public virtual void SetScrollBar(
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
        }

        public virtual void SetToolTip(string? value)
        {
        }

        public virtual void UnsetToolTip()
        {
        }

        public virtual void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            Invalidate();
        }

        public virtual void Update()
        {
            Invalidate();
        }

        public virtual void Invalidate()
        {
            container?.Invalidate();
        }

        public virtual Graphics CreateDrawingContext()
        {
            SKBitmap bitmap = new();
            SKCanvas canvas = new(bitmap);
            return new SkiaGraphics(canvas);
        }

        public void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively)
        {
        }
    }
}
