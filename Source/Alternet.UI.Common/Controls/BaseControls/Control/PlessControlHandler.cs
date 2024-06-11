using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessControlHandler : BaseControlHandler, IControlHandler
    {
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
        
        public virtual bool IsNativeControlCreated { get; set; }
        
        public virtual bool IsFocused
        {
            get => false;

            set
            {

            }
        }
        
        public virtual Thickness IntrinsicLayoutPadding { get; set; }
        
        public virtual Thickness IntrinsicPreferredSizePadding { get; set; }
        
        public virtual bool IsScrollable { get; set; }
        
        public virtual RectD Bounds { get; set; }

        public virtual RectD EventBounds { get; set; }

        public virtual bool Visible { get; set; }
        
        public virtual bool UserPaint { get; set; }
        
        public virtual SizeD MinimumSize { get; set; }
        
        public virtual SizeD MaximumSize { get; set; }

        public virtual Color BackgroundColor
        {
            get;
            set;
        } = SystemColors.Window;


        public virtual Color ForegroundColor
        {
            get;

            set;
        } = SystemColors.WindowText;

        public virtual Font? Font { get; set; }
        
        public virtual bool IsBold { get; set; }
        
        public virtual bool TabStop { get; set; }
        
        public virtual bool AllowDrop { get; set; }
        
        public virtual bool AcceptsFocus { get; set; }
        
        public virtual ControlBackgroundStyle BackgroundStyle { get; set; }
        
        public virtual bool AcceptsFocusFromKeyboard { get; set; }
        
        public virtual bool AcceptsFocusRecursively { get; set; }
        
        public virtual bool AcceptsFocusAll { get; set; }
        
        public virtual bool ProcessIdle { get; set; }
        
        public virtual bool BindScrollEvents { get; set; }
        
        public virtual SizeD ClientSize
        {
            get => Bounds.Size;
            set => Bounds = (Bounds.Location, value);
        }
        
        public virtual bool CanAcceptFocus { get; }
        
        public virtual bool IsMouseOver { get; set; }
        
        public virtual bool ProcessUIUpdates { get; set; }
        
        public virtual bool IsMouseCaptured { get; set; }
        
        public virtual bool IsHandleCreated { get; set; }
        
        public virtual bool IsFocusable { get; set; }
        public Action? SystemColorsChanged { get; set; }
        public SizeI EventOldDpi { get; }
        public SizeI EventNewDpi { get; }
        public Action? DpiChanged { get; set; }

        public virtual void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
        }

        public virtual void BeginIgnoreRecreate()
        {
        }

        public virtual void BeginInit()
        {
        }

        public virtual bool BeginRepositioningChildren()
        {
            return default;
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

        public virtual PointD ClientToScreen(PointD point)
        {
            return point;
        }

        public virtual Graphics CreateDrawingContext()
        {
            return PlessGraphics.Default;
        }

        public virtual PointD DeviceToScreen(PointI point)
        {
            return point;
        }

        public virtual void DisableRecreate()
        {
        }

        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return default;
        }

        public virtual void EnableRecreate()
        {
        }

        public virtual void EndIgnoreRecreate()
        {
        }

        public virtual void EndInit()
        {
        }

        public virtual void EndRepositioningChildren()
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

        public virtual void Invalidate()
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
            return GraphicsFactory.PixelFromDip(value, Control.GetPixelScaleFactor());
        }

        public virtual Coord PixelToDip(int value)
        {
            return GraphicsFactory.PixelToDip(value, Control.GetPixelScaleFactor()); 
        }

        public virtual void Raise()
        {
        }

        public virtual void RecreateWindow()
        {
        }

        public virtual void RefreshRect(RectD rect, bool eraseBackground = true)
        {
        }

        public virtual void ReleaseMouseCapture()
        {
        }

        public virtual void ResetBackgroundColor()
        {
        }

        public virtual void ResetForegroundColor()
        {
        }

        public virtual void SaveScreenshot(string fileName)
        {
        }

        public virtual PointD ScreenToClient(PointD point)
        {
            return point;
        }

        public virtual PointI ScreenToDevice(PointD point)
        {
            return point.ToPoint();
        }

        public virtual void SendMouseDownEvent(int x, int y)
        {
        }

        public virtual void SendMouseUpEvent(int x, int y)
        {
        }

        public virtual void SendSizeEvent()
        {
        }

        public virtual void SetBounds(RectD rect, SetBoundsFlags flags)
        {
        }

        public virtual void SetCursor(Cursor? value)
        {
        }

        public virtual void SetEnabled(bool value)
        {
        }

        public virtual bool SetFocus()
        {
            return default;
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

        public virtual void Update()
        {
        }
    }
}
