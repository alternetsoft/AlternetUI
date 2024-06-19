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
        Action<DragEventArgs>? IControlHandler.DragDrop { get; set; }

        Action<DragEventArgs>? IControlHandler.DragOver { get; set; }

        Action<DragEventArgs>? IControlHandler.DragEnter { get; set; }

        Action? IControlHandler.Idle { get; set; }

        public string Text { get; set; } = string.Empty;

        Action? IControlHandler.TextChanged { get; set; }

        Action? IControlHandler.Paint { get; set; }

        Action? IControlHandler.MouseEnter { get; set; }

        Action? IControlHandler.MouseLeave { get; set; }

        Action? IControlHandler.MouseClick { get; set; }

        Action? IControlHandler.VisibleChanged { get; set; }

        Action? IControlHandler.MouseCaptureLost { get; set; }

        Action? IControlHandler.GotFocus { get; set; }

        Action? IControlHandler.LostFocus { get; set; }

        Action? IControlHandler.DragLeave { get; set; }

        Action? IControlHandler.VerticalScrollBarValueChanged { get; set; }

        Action? IControlHandler.HorizontalScrollBarValueChanged { get; set; }

        Action? IControlHandler.SizeChanged { get; set; }

        Action? IControlHandler.LocationChanged { get; set; }

        Action? IControlHandler.Activated { get; set; }

        Action? IControlHandler.Deactivated { get; set; }

        Action? IControlHandler.HandleCreated { get; set; }

        Action? IControlHandler.HandleDestroyed { get; set; }

        bool IControlHandler.WantChars { get; set; }

        bool IControlHandler.ShowHorzScrollBar { get; set; }

        bool IControlHandler.ShowVertScrollBar { get; set; }

        bool IControlHandler.ScrollBarAlwaysVisible { get; set; }

        LangDirection IControlHandler.LangDirection { get; set; }

        ControlBorderStyle IControlHandler.BorderStyle { get; set; }

        bool IControlHandler.IsNativeControlCreated { get; }

        bool IControlHandler.IsFocused
        {
            get => false;
        }

        Thickness IControlHandler.IntrinsicLayoutPadding { get; }

        Thickness IControlHandler.IntrinsicPreferredSizePadding { get; }

        bool IControlHandler.IsScrollable { get; set; }

        public RectD Bounds { get; set; }

        RectD IControlHandler.EventBounds { get; }

        bool IControlHandler.Visible { get; set; }

        bool IControlHandler.UserPaint { get; set; }

        SizeD IControlHandler.MinimumSize { get; set; }

        SizeD IControlHandler.MaximumSize { get; set; }

        Color IControlHandler.BackgroundColor { get; set; } = SystemColors.Window;

        Color IControlHandler.ForegroundColor { get; set; } = SystemColors.WindowText;

        Font? IControlHandler.Font { get; set; }

        bool IControlHandler.IsBold { get; set; }

        bool IControlHandler.AllowDrop { get; set; }

        ControlBackgroundStyle IControlHandler.BackgroundStyle { get; set; }

        bool IControlHandler.ProcessIdle { get; set; }

        bool IControlHandler.BindScrollEvents { get; set; }

        public SizeD ClientSize
        {
            get => Bounds.Size;
            set => Bounds = (Bounds.Location, value);
        }

        bool IControlHandler.ProcessUIUpdates { get; set; }

        bool IControlHandler.IsMouseCaptured { get; }

        bool IControlHandler.IsHandleCreated { get; }

        Action? IControlHandler.SystemColorsChanged { get; set; }

        SizeI IControlHandler.EventOldDpi { get; }

        SizeI IControlHandler.EventNewDpi { get; }

        Action? IControlHandler.DpiChanged { get; set; }

        public bool TabStop => true;

        public bool CanSelect => true;

        void IControlHandler.AlwaysShowScrollbars(bool hflag, bool vflag)
        {
        }

        void IControlHandler.BeginInit()
        {
        }

        void IControlHandler.BeginUpdate()
        {
        }

        void IControlHandler.CaptureMouse()
        {
        }

        void IControlHandler.CenterOnParent(GenericOrientation direction)
        {
        }

        PointD IControlHandler.ClientToScreen(PointD point)
        {
            return point;
        }

        Graphics IControlHandler.CreateDrawingContext()
        {
            return PlessGraphics.Default;
        }

        DragDropEffects IControlHandler.DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return default;
        }

        void IControlHandler.EndInit()
        {
        }

        void IControlHandler.EndUpdate()
        {
        }

        void IControlHandler.FocusNextControl(bool forward, bool nested)
        {
        }

        Color IControlHandler.GetDefaultAttributesBgColor()
        {
            return SystemColors.Window;
        }

        Color IControlHandler.GetDefaultAttributesFgColor()
        {
            return SystemColors.WindowText;
        }

        Font? IControlHandler.GetDefaultAttributesFont()
        {
            return default;
        }

        nint IControlHandler.GetHandle()
        {
            return default;
        }

        object IControlHandler.GetNativeControl()
        {
            return AssemblyUtils.Default;
        }

        Coord IControlHandler.GetPixelScaleFactor()
        {
            return Display.Primary.ScaleFactor;
        }

        SizeD IControlHandler.GetPreferredSize(SizeD availableSize)
        {
            return availableSize;
        }

        ScrollEventType IControlHandler.GetScrollBarEvtKind()
        {
            return default;
        }

        int IControlHandler.GetScrollBarEvtPosition()
        {
            return default;
        }

        int IControlHandler.GetScrollBarLargeChange(bool isVertical)
        {
            return default;
        }

        int IControlHandler.GetScrollBarMaximum(bool isVertical)
        {
            return default;
        }

        int IControlHandler.GetScrollBarValue(bool isVertical)
        {
            return default;
        }

        RectI IControlHandler.GetUpdateClientRectI()
        {
            return new RectI((0, 0), Control.PixelFromDip(ClientSize));
        }

        void IControlHandler.HandleNeeded()
        {
        }

        void IControlHandler.Invalidate()
        {
        }

        bool IControlHandler.IsScrollBarVisible(bool isVertical)
        {
            return default;
        }

        bool IControlHandler.IsTransparentBackgroundSupported()
        {
            return default;
        }

        void IControlHandler.Lower()
        {
        }

        void IControlHandler.OnChildInserted(Control childControl)
        {
        }

        void IControlHandler.OnChildRemoved(Control childControl)
        {
        }

        Graphics IControlHandler.OpenPaintDrawingContext()
        {
            return PlessGraphics.Default;
        }

        void IControlHandler.Raise()
        {
        }

        void IControlHandler.RecreateWindow()
        {
        }

        void IControlHandler.RefreshRect(RectD rect, bool eraseBackground)
        {
        }

        void IControlHandler.ReleaseMouseCapture()
        {
        }

        void IControlHandler.ResetBackgroundColor()
        {
        }

        void IControlHandler.ResetForegroundColor()
        {
        }

        void IControlHandler.SaveScreenshot(string fileName)
        {
        }

        PointD IControlHandler.ScreenToClient(PointD point)
        {
            return point;
        }

        void IControlHandler.SetCursor(Cursor? value)
        {
        }

        void IControlHandler.SetEnabled(bool value)
        {
        }

        bool IControlHandler.SetFocus()
        {
            return default;
        }

        void IControlHandler.SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively)
        {
        }

        void IControlHandler.SetScrollBar(
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
        }

        void IControlHandler.SetToolTip(string? value)
        {
        }

        void IControlHandler.UnsetToolTip()
        {
        }

        void IControlHandler.Update()
        {
        }
    }
}
