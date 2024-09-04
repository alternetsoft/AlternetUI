using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="Alternet.UI.Control"/> container using <see cref="SKCanvasView"/>.
    /// </summary>
    public partial class SkiaContainer : SKCanvasView
    {
        /// <summary>
        /// This is sample bindable property declaration.
        /// </summary>
        public static readonly BindableProperty SampleProperty = BindableProperty.Create(
            nameof(SampleProp),
            typeof(float),
            typeof(SkiaContainer),
            0.0f,
            propertyChanged: OnSamplePropChanged);

        private readonly InteriorDrawable interior = new();
        private readonly InteriorNotification interiorNotification;

        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        static SkiaContainer()
        {
            InitMauiHandler();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaContainer"/> class.
        /// </summary>
        public SkiaContainer()
        {
            interior.Metrics = ScrollBar.DefaultMetrics;
            interior.SetDefaultBorder(true);
            interior.SetThemeMetrics(ScrollBar.KnownTheme.MauiDark);
            interiorNotification = new(interior);

            EnableTouchEvents = true;
            Touch += Canvas_Touch;
            SizeChanged += SkiaContainer_SizeChanged;

            PaintSurface += Canvas_PaintSurface;

            Focused += SkiaContainer_Focused;
            Unfocused += SkiaContainer_Unfocused;
        }

        /// <summary>
        /// This is sample bindable property.
        /// </summary>
        public float SampleProp
        {
            get => (float)GetValue(SampleProperty);
            set => SetValue(SampleProperty, value);
        }

        /// <summary>
        /// Gets control interior element (border and scrollbars).
        /// </summary>
        public InteriorDrawable Interior => interior;

        /// <summary>
        /// Gets or sets whether 'DrawImage' methods draw unscaled image.
        /// </summary>
        public virtual bool UseUnscaledDrawImage { get; set; } = true;

        /// <summary>
        /// Gets or sets attached <see cref="Alternet.UI.Control"/>.
        /// </summary>
        public virtual Alternet.UI.Control? Control
        {
            get => control;

            set
            {
                if (control == value)
                    return;

                if (control is not null)
                {
                    control.RemoveNotification(interiorNotification);

                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = null;
                }

                control = value;

                if (control is not null)
                {
                    control.AddNotification(interiorNotification);
                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = this;
                }

                InvalidateSurface();
            }
        }

        /// <summary>
        /// Repaints the control.
        /// </summary>
        public virtual void Invalidate()
        {
            InvalidateSurface();
        }

        /// <summary>
        /// Adds message to log.
        /// </summary>
        /// <param name="s">Message text.</param>
        public virtual void Log(object? s)
        {
            Alternet.UI.App.Log(s);
        }

        /// <summary>
        /// Initializes application handler.
        /// </summary>
        protected static void InitMauiHandler()
        {
            App.Handler ??= new MauiApplicationHandler();
        }

        /// <inheritdoc/>
        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
        }

        /// <inheritdoc/>
        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);
        }

        /// <inheritdoc/>
        protected override void OnParentChanged()
        {
            base.OnParentChanged();
        }

        private static void OnSamplePropChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((SkiaContainer)bindable).InvalidateSurface();
        }

        private void SkiaContainer_SizeChanged(object? sender, EventArgs e)
        {
            UpdateBounds(Bounds.ToRectD());
            control?.RaiseHandlerSizeChanged();
        }

        private void Canvas_Touch(object? sender, SKTouchEventArgs e)
        {
#if WINDOWS
            var platformView = GetPlatformView();
            platformView?.Focus(Microsoft.UI.Xaml.FocusState.Pointer);
#endif

            if (control is null)
                return;

            TouchEventArgs args = MauiTouchUtils.Convert(e);
            control?.RaiseTouch(args);
            e.Handled = args.Handled;
        }

        private void UpdateBounds(RectD max)
        {
            if (control is null)
                return;

            var scaleFactor = (float)control.ScaleFactor;
            var bounds = Bounds;

            RectD newBounds = (
                0,
                0,
                Math.Min(bounds.Width, max.Width),
                Math.Min(bounds.Height, max.Height));

            interior.Bounds = newBounds;

            var rectangles = interior.GetLayoutRectangles(scaleFactor);
            var clientRect = rectangles[InteriorDrawable.HitTestResult.ClientRect];

            control.Bounds = (0, 0, clientRect.Width, clientRect.Height);
        }

        private void Canvas_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var dc = e.Surface.Canvas;

            if (control is null)
                return;

            var scaleFactor = (float)control.ScaleFactor;

            dc.Save();
            dc.Scale(scaleFactor);

            if (graphics is null)
            {
                graphics = new(dc);
            }
            else
            {
                graphics.Canvas = dc;
            }

            graphics.OriginalScaleFactor = scaleFactor;

            UpdateBounds(dc.LocalClipBounds);

            dc.Clear(control.BackColor);

            dc.Save();

            graphics.UseUnscaledDrawImage = UseUnscaledDrawImage;

            control.RaisePaint(new PaintEventArgs(graphics, control.Bounds));

            graphics.UseUnscaledDrawImage = false;

            dc.Restore();

            interior.VertPosition = control.VertScrollBarInfo;
            interior.HorzPosition = control.HorzScrollBarInfo;

            if (!interior.HorzPosition.Equals(ScrollBarInfo.Default))
            {
            }

            interior.Draw(control, graphics);

            dc.Flush();
            dc.Restore();
        }

        private void SkiaContainer_Focused(object? sender, FocusEventArgs e)
        {
            control?.RaiseGotFocus();
        }

        private void SkiaContainer_Unfocused(object? sender, FocusEventArgs e)
        {
            control?.RaiseLostFocus();
        }
    }
}
