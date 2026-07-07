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
    internal partial class MauiControlHandler : DisposableObject, IControlHandler
    {
        public static Color DefaultBackgroundColor = SystemColors.Window;

        public static Color DefaultForegroundColor = SystemColors.WindowText;

        private View? container;
        private Color backgroundColor = DefaultBackgroundColor;
        private Color foregroundColor = DefaultForegroundColor;
        private bool visible = true;
        private RectD bounds;
        private Control? control;

        static MauiControlHandler()
        {
            PlessMouse.InitMouseTargetTracking();
        }

        public MauiControlHandler()
        {
        }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for
        /// or Null if control is disposed or not assigned.
        /// </summary>
        public Control? ControlOrNull
        {
            get
            {
                return control;
            }
        }

        /// <inheritdoc cref="AbstractControl.HasBorder"/>
        public virtual bool HasBorder
        {
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// If control is disposed or not attached to the handler, returns dummy control.
        /// </summary>
        public Control? Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => ControlOrNull != null;

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        public void Attach(Control control)
        {
            if (DisposingOrDisposed)
                return;
            this.control = control;
            OnAttach();
        }

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public virtual void Detach()
        {
            OnDetach();

            control = null;
        }

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        public virtual void OnLayoutChanged()
        {
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected virtual void OnDetach()
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

        public virtual View? Container
        {
            get => container;

            set
            {
                if (container == value)
                    return;
                container = value;
                OnContainerChanged();
            }
        }

        public virtual string Text { get; set; } = string.Empty;

        public virtual bool WantChars { get; set; }

        public virtual LangDirection LangDirection { get; set; }

        public virtual ControlBorderStyle BorderStyle { get; set; }

        public virtual Thickness NativePadding { get; set; }

        public virtual RectD Bounds
        {
            get => bounds;

            set
            {
                if (bounds == value)
                    return;
                var oldBounds = bounds;
                bounds = value;

                if (oldBounds.Location != value.Location)
                    Control?.RaiseContainerLocationChanged(EventArgs.Empty);
                if (oldBounds.Size != value.Size)
                    Control?.RaiseHandlerSizeChanged(EventArgs.Empty);

                InvalidateContainer();
            }
        }

        public virtual bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;
                visible = value;
                InvalidateContainer();
            }
        }

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

        public virtual bool AllowDrop { get; set; }

        public virtual ControlBackgroundStyle BackgroundStyle { get; set; }

        public virtual SizeD ClientSize
        {
            get => Bounds.Size;
            set
            {
                Bounds = (Bounds.Location, value);
            }
        }

        public virtual bool ProcessUIUpdates { get; set; }

        public virtual RectI BoundsI
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

        public virtual bool VisibleOnScreen
        {
            get
            {
                return Control?.Parent?.VisibleOnScreen ?? Container?.IsVisible ?? false;
            }
        }

        public virtual nint NativeGraphicsContext { get; }

        public virtual void BeginInit()
        {
        }

        public virtual void BeginUpdate()
        {
        }

        public virtual void CaptureMouse()
        {
        }

        public virtual void ReleaseMouseCapture()
        {
        }

        public PointD ScreenToClient(PointD point, string debugId)
        {
            return MauiApplicationHandler.ScreenToClient(point, Control);
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

        public virtual Color GetDefaultAttributesBgColor()
        {
            return SystemColors.Window;
        }

        public virtual Color GetDefaultAttributesFgColor()
        {
            return SystemColors.WindowText;
        }

        public virtual nint GetHandle()
        {
            return default;
        }

        public virtual object GetNativeControl()
        {
            object? result = container;
            return result ?? AssemblyUtils.Default;
        }

        public virtual Coord? GetPixelScaleFactor()
        {
            var result = MauiDisplayHandler.GetDefaultScaleFactor();

            if (result >= 1f)
                return result;

            return null;
        }

        public virtual SizeD GetPreferredSize(PreferredSizeContext context)
        {
            return context.AvailableSize;
        }

        public virtual RectI GetUpdateClientRectI()
        {
            if (Control is null)
                return RectI.Empty;
            return new RectI(PointI.Empty, Control.PixelFromDip(ClientSize));
        }

        public virtual void HandleNeeded()
        {
        }

        public virtual bool IsTransparentBackgroundSupported()
        {
            return default;
        }

        public virtual void Lower()
        {
        }

        public virtual void OnChildInserted(AbstractControl childControl)
        {
            if (childControl.Visible)
            {
                InvalidateContainer();
            }
        }

        public virtual void OnChildRemoved(AbstractControl childControl)
        {
            if (childControl.Visible)
            {
                InvalidateContainer();
            }
        }

        public virtual void Raise()
        {
        }

        public virtual void RecreateWindow()
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

        public virtual void SetCursor(Cursor? value)
        {
#if WINDOWS
            var platformView = ThisOrRootContainer?.GetPlatformView();

            if (platformView is null)
                return;

            if (value is null || value.KnownCursorType is null)
            {
                platformView.InputCursor = null;
                return;
            }

            var newCursor = MauiWindowsUtils.GetOrCreateSystemCursor(value.KnownCursorType.Value);

            if (newCursor == platformView.InputCursor)
            {
            }
            else
            {
                platformView.InputCursor = newCursor;
            }
#endif
        }

        public virtual void SetEnabled(bool value)
        {
            container?.IsEnabled = value;
        }

        public virtual void SetToolTip(object? value)
        {
        }

        public virtual void UnsetToolTip()
        {
        }

        public virtual void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            InvalidateContainer(rect);
        }

        public virtual void Update()
        {
        }

        public virtual void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
        }

        public virtual void Invalidate()
        {
            InvalidateContainer();
        }

        public virtual Graphics CreateDrawingContext()
        {
            SKBitmap bitmap = new();
            SKCanvas canvas = new(bitmap);
            return new SkiaGraphics(canvas);
        }

        public virtual bool EnableTouchEvents(TouchEventsMask flag)
        {
            return false;
        }

        public virtual void InvalidateBestSize()
        {
        }

        public virtual void SetAllowDefaultContextMenu(bool value)
        {
        }

        public virtual void OnSystemColorsChanged()
        {
        }

        public virtual MauiControlHandler? GetRootHandler()
        {
            var result = (Control?.Root as Control)?.Handler as MauiControlHandler;
            return result;
        }

        public virtual ControlView? ThisOrRootContainer => (container ?? GetRootHandler()?.container) as ControlView;

        public virtual void InvalidateContainer(RectD? rect = null)
        {
            ThisOrRootContainer?.Invalidate();
        }

        public virtual void OnRemovedFromParent(AbstractControl parentControl)
        {
        }

        public virtual void OnInsertedToParent(AbstractControl parentControl)
        {
        }

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected virtual void OnAttach()
        {
        }

        protected virtual void OnContainerChanged()
        {
        }
    }
}