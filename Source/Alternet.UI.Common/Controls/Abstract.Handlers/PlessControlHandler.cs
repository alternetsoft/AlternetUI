using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Platform independent implementation of the <see cref="IControlHandler"/> interface.
    /// </summary>
    public class PlessControlHandler : BaseControlHandler, IControlHandler
    {
#pragma warning disable
        private bool enabled = true;
        private object? toolTip;
        private bool canSelect = true;
        private bool tabStop = true;
        private bool acceptsFocusRecursively = true;
        private Cursor? cursor;
#pragma warning restore

        string IControlHandler.Text { get; set; } = string.Empty;

        bool IControlHandler.WantChars { get; set; }

        LangDirection IControlHandler.LangDirection { get; set; }

        ControlBorderStyle IControlHandler.BorderStyle { get; set; }

        bool IControlHandler.IsNativeControlCreated { get; }

        bool IControlHandler.CanSelect { get; }

        bool IControlHandler.IsFocused
        {
            get => false;
        }

        Thickness IControlHandler.NativePadding { get; }

        /// <inheritdoc/>
        public RectD Bounds { get; set; }

        RectI IControlHandler.BoundsI
        {
            get
            {
                return GraphicsFactory.PixelFromDip(Bounds, Control?.ScaleFactor);
            }

            set
            {
                Bounds = GraphicsFactory.PixelToDip(value, Control?.ScaleFactor);
            }
        }

        /// <inheritdoc/>
        public bool Visible { get; set; }

        bool IControlHandler.UserPaint
        {
            get => true;

            set
            {
            }
        }

        /// <inheritdoc/>
        public virtual Color BackgroundColor { get; set; } = SystemColors.Window;

        /// <inheritdoc/>
        public virtual Color ForegroundColor { get; set; } = SystemColors.WindowText;

        /// <inheritdoc/>
        public virtual Font? Font { get; set; }

        bool IControlHandler.AllowDrop { get; set; }

        ControlBackgroundStyle IControlHandler.BackgroundStyle { get; set; }

        SizeD IControlHandler.ClientSize
        {
            get => ((IControlHandler)this).Bounds.Size;
            set => ((IControlHandler)this).Bounds = (((IControlHandler)this).Bounds.Location, value);
        }

        bool IControlHandler.ProcessUIUpdates { get; set; }

        bool IControlHandler.IsMouseCaptured { get; }

        bool IControlHandler.IsHandleCreated => true;

        /// <inheritdoc/>
        public virtual bool VisibleOnScreen => Visible;

        /// <inheritdoc/>
        public ScrollBarInfo VertScrollBarInfo { get; set; }

        /// <inheritdoc/>
        public ScrollBarInfo HorzScrollBarInfo { get; set; }

        /// <inheritdoc/>
        public bool IsScrollable { get; set; }

        /// <inheritdoc/>
        public IntPtr NativeGraphicsContext { get; }

        void IControlHandler.BeginInit()
        {
        }

        void IControlHandler.BeginUpdate()
        {
        }

        void IControlHandler.CaptureMouse()
        {
        }

        PointD IControlHandler.ClientToScreen(PointD point)
        {
            if (Control is null)
                return PointD.MinValue;

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

        Color IControlHandler.GetDefaultAttributesBgColor()
        {
            return Control?.Parent?.GetDefaultAttributesBgColor() ?? SystemColors.Window;
        }

        Color IControlHandler.GetDefaultAttributesFgColor()
        {
            return Control?.Parent?.GetDefaultAttributesFgColor() ?? SystemColors.WindowText;
        }

        Font? IControlHandler.GetDefaultAttributesFont()
        {
            return Control?.Parent?.GetDefaultAttributesFont() ?? AbstractControl.DefaultFont;
        }

        nint IControlHandler.GetHandle()
        {
            return Control?.Parent?.GetHandle() ?? default;
        }

        object IControlHandler.GetNativeControl()
        {
            return Control?.Parent?.NativeControl ?? AssemblyUtils.Default;
        }

        /// <inheritdoc/>
        public Coord? GetPixelScaleFactor()
        {
            return Control?.Parent?.ScaleFactor ?? Display.Primary.ScaleFactor;
        }

        SizeD IControlHandler.GetPreferredSize(PreferredSizeContext context)
        {
            return context.AvailableSize;
        }

        RectI IControlHandler.GetUpdateClientRectI()
        {
            return new RectI((0, 0), Control?.PixelFromDip(((IControlHandler)this).ClientSize) ?? 0);
        }

        void IControlHandler.HandleNeeded()
        {
            Control?.Parent?.HandleNeeded();
        }

        void IControlHandler.Invalidate()
        {
            Control?.Parent?.Invalidate(Control.Bounds);
        }

        bool IControlHandler.IsTransparentBackgroundSupported()
        {
            return Control?.Parent?.IsTransparentBackgroundSupported() ?? false;
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
            Control?.Parent?.Invalidate((rect.Location + Control.Location, rect.Size));
        }

        void IControlHandler.ReleaseMouseCapture()
        {
            Control?.Parent?.ReleaseMouseCapture();
        }

        void IControlHandler.ResetBackgroundColor()
        {
            BackgroundColor = SystemColors.Window;
        }

        void IControlHandler.ResetForegroundColor()
        {
            ForegroundColor = SystemColors.WindowText;
        }

        PointD IControlHandler.ScreenToClient(PointD point)
        {
            if (Control is null)
                return PointD.MinValue;

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

        /// <inheritdoc/>
        public void SetToolTip(object? value)
        {
            toolTip = value;
        }

        void IControlHandler.UnsetToolTip()
        {
            toolTip = null;
        }

        void IControlHandler.Update()
        {
            Control?.Parent?.Update();
        }

        /// <inheritdoc/>
        public virtual bool EnableTouchEvents(TouchEventsMask flag)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void InvalidateBestSize()
        {
        }

        /// <inheritdoc/>
        public void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            this.canSelect = canSelect;
            this.tabStop = tabStop;
        }

        void IControlHandler.SetAllowDefaultContextMenu(bool value)
        {
        }

        /// <inheritdoc/>
        public void OnSystemColorsChanged()
        {
        }

        /// <inheritdoc/>
        public void SetRenderingFlags(ControlRenderingFlags flags)
        {
        }
    }
}
