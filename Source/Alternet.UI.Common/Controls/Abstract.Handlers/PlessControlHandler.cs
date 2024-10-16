using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Platformless implementation of the <see cref="IControlHandler"/> interface.
    /// </summary>
    public class PlessControlHandler : BaseControlHandler, IControlHandler
    {
#pragma warning disable
        private bool enabled = true;
        private string? toolTip;
        private bool canSelect = true;
        private bool tabStop = true;
        private bool acceptsFocusRecursively = true;
        private Cursor? cursor;
#pragma warning restore

        /// <inheritdoc cref="AbstractControl.VertScrollBarInfo"/>
        ScrollBarInfo IControlHandler.VertScrollBarInfo { get; set; }

        /// <inheritdoc cref="AbstractControl.HorzScrollBarInfo"/>
        ScrollBarInfo IControlHandler.HorzScrollBarInfo { get; set; }

        Action<DragEventArgs>? IControlHandler.DragDrop { get; set; }

        Action<DragEventArgs>? IControlHandler.DragOver { get; set; }

        Action<DragEventArgs>? IControlHandler.DragEnter { get; set; }

        Action? IControlHandler.Idle { get; set; }

        string IControlHandler.Text { get; set; } = string.Empty;

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

        /// <inheritdoc/>
        public RectD Bounds { get; set; }

        RectI IControlHandler.BoundsI
        {
            get
            {
                return GraphicsFactory.PixelFromDip(Bounds, GetPixelScaleFactor());
            }

            set
            {
                Bounds = GraphicsFactory.PixelToDip(value, GetPixelScaleFactor());
            }
        }

        RectD IControlHandler.EventBounds { get; }

        bool IControlHandler.Visible { get; set; }

        bool IControlHandler.UserPaint
        {
            get => true;

            set
            {
            }
        }

        SizeD IControlHandler.MinimumSize { get; set; }

        SizeD IControlHandler.MaximumSize { get; set; }

        /// <inheritdoc/>
        public virtual Color BackgroundColor { get; set; } = SystemColors.Window;

        /// <inheritdoc/>
        public virtual Color ForegroundColor { get; set; } = SystemColors.WindowText;

        /// <inheritdoc/>
        public virtual Font? Font { get; set; }

        /// <inheritdoc/>
        public virtual bool IsBold
        {
            get
            {
                if (Font is null)
                    return false;
                return Font.IsBold;
            }

            set
            {
                if (IsBold == value)
                    return;
                if (Font is null)
                    Font = AbstractControl.DefaultFont.AsBold;
                else
                    Font = Font.AsBold;
            }
        }

        bool IControlHandler.AllowDrop { get; set; }

        ControlBackgroundStyle IControlHandler.BackgroundStyle { get; set; }

        bool IControlHandler.ProcessIdle { get; set; }

        bool IControlHandler.BindScrollEvents { get; set; }

        SizeD IControlHandler.ClientSize
        {
            get => ((IControlHandler)this).Bounds.Size;
            set => ((IControlHandler)this).Bounds = (((IControlHandler)this).Bounds.Location, value);
        }

        bool IControlHandler.ProcessUIUpdates { get; set; }

        bool IControlHandler.IsMouseCaptured { get; }

        bool IControlHandler.IsHandleCreated => true;

        Action? IControlHandler.SystemColorsChanged { get; set; }

        SizeI IControlHandler.EventOldDpi { get; }

        SizeI IControlHandler.EventNewDpi { get; }

        Action? IControlHandler.DpiChanged { get; set; }

        bool IControlHandler.TabStop => tabStop;

        bool IControlHandler.CanSelect => canSelect;

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
            PointD result;
            var parent = Control.Parent;

            if (parent == null)
            {
                result = PlessUtils.ClientToScreen(point, Control);
                return result;
            }

            result = parent.ClientToScreen(point) + Control.Location;
            return result;
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
            return Control.Parent?.GetDefaultAttributesBgColor() ?? SystemColors.Window;
        }

        Color IControlHandler.GetDefaultAttributesFgColor()
        {
            return Control.Parent?.GetDefaultAttributesFgColor() ?? SystemColors.WindowText;
        }

        Font? IControlHandler.GetDefaultAttributesFont()
        {
            return Control.Parent?.GetDefaultAttributesFont() ?? AbstractControl.DefaultFont;
        }

        nint IControlHandler.GetHandle()
        {
            return Control.Parent?.GetHandle() ?? default;
        }

        object IControlHandler.GetNativeControl()
        {
            return Control.Parent?.NativeControl ?? AssemblyUtils.Default;
        }

        /// <inheritdoc/>
        public Coord GetPixelScaleFactor()
        {
            return Control.Parent?.ScaleFactor ?? Display.Primary.ScaleFactor;
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

        RectI IControlHandler.GetUpdateClientRectI()
        {
            return new RectI((0, 0), Control.PixelFromDip(((IControlHandler)this).ClientSize));
        }

        void IControlHandler.HandleNeeded()
        {
            Control.Parent?.HandleNeeded();
        }

        void IControlHandler.Invalidate()
        {
            Control.Parent?.Invalidate(Control.Bounds);
        }

        bool IControlHandler.IsTransparentBackgroundSupported()
        {
            return Control.Parent?.IsTransparentBackgroundSupported() ?? false;
        }

        void IControlHandler.Lower()
        {
        }

        void IControlHandler.OnChildInserted(AbstractControl childControl)
        {
        }

        void IControlHandler.OnChildRemoved(AbstractControl childControl)
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
            Control.Parent?.Invalidate((rect.Location + Control.Location, rect.Size));
        }

        void IControlHandler.ReleaseMouseCapture()
        {
            Control.Parent?.ReleaseMouseCapture();
        }

        void IControlHandler.ResetBackgroundColor()
        {
            BackgroundColor = SystemColors.Window;
        }

        void IControlHandler.ResetForegroundColor()
        {
            ForegroundColor = SystemColors.WindowText;
        }

        void IControlHandler.SaveScreenshot(string fileName)
        {
        }

        PointD IControlHandler.ScreenToClient(PointD point)
        {
            PointD result;
            var parent = Control.Parent;

            if (parent == null)
            {
                result = PlessUtils.ScreenToClient(point, Control);
                return result;
            }

            result = parent.ScreenToClient(point) - Control.Location;
            return result;
        }

        void IControlHandler.SetCursor(Cursor? value)
        {
            cursor = value;
        }

        void IControlHandler.SetEnabled(bool value)
        {
            enabled = value;
        }

        bool IControlHandler.SetFocus()
        {
            return default;
        }

        void IControlHandler.SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively)
        {
            this.canSelect = canSelect;
            this.tabStop = tabStop;
            this.acceptsFocusRecursively = acceptsFocusRecursively;
        }

        /// <inheritdoc/>
        public void SetToolTip(string? value)
        {
            toolTip = value;
        }

        void IControlHandler.UnsetToolTip()
        {
            toolTip = null;
        }

        void IControlHandler.Update()
        {
            Control.Parent?.Update();
        }
    }
}
