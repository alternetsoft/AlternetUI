using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="Alternet.UI.Control"/> container using <see cref="SKCanvasView"/>.
    /// </summary>
    public partial class ControlView : SKCanvasView
    {
        private InteriorDrawable? interior;

        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        static ControlView()
        {
            InitMauiHandler();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlView"/> class.
        /// </summary>
        public ControlView()
        {
            EnableTouchEvents = true;
            Touch += Canvas_Touch;
            SizeChanged += SkiaContainer_SizeChanged;

            PaintSurface += Canvas_PaintSurface;

            Focused += SkiaContainer_Focused;
            Unfocused += SkiaContainer_Unfocused;
        }

        /// <summary>
        /// Gets control interior element (border and scrollbars).
        /// </summary>
        public virtual InteriorDrawable Interior
        {
            get
            {
                if(interior is null)
                {
                    interior = new();
                    interior.Metrics = ScrollBar.DefaultMetrics;
                    interior.SetDefaultBorder(true);
                    interior.SetThemeMetrics(ScrollBar.KnownTheme.MauiDark);
                }

                return interior;
            }
        }

        /// <summary>
        /// Gets or sets whether 'DrawImage' methods draw unscaled image. Default is <c>true</c>.
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
                    if(interior is not null)
                        control.RemoveNotification(interior.Notification);

                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = null;
                }

                control = value;

                if (control is not null)
                {
                    if (interior is not null)
                        control.AddNotification(interior.Notification);
                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = this;
                }

                InvalidateSurface();
            }
        }

        /// <summary>
        /// Gets platform view.
        /// </summary>
        /// <param name="handler">Element handler.</param>
        /// <returns></returns>
        public virtual PlatformView? GetPlatformView(IElementHandler? handler = null)
        {
            handler ??= Handler;
            var platformView = handler?.PlatformView as PlatformView;
            return platformView;
        }

        /// <summary>
        /// Gets whether control is in the design mode.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetIsDesignMode()
        {
            ISite? site = ((IComponent)this).Site;
            var designMode = site != null && site.DesignMode;

#if IOS || MACCATALYST
            designMode = designMode || !PlatformView.IsValidEnvironment;
#endif
            return designMode;
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

        private void SkiaContainer_SizeChanged(object? sender, EventArgs e)
        {
            UpdateBounds(Bounds.ToRectD());
            control?.RaiseHandlerSizeChanged();
        }

        private void Canvas_Touch(object? sender, SKTouchEventArgs e)
        {
#if WINDOWS
            if(e.ActionType == SKTouchAction.Pressed && !IsFocused)
            {
                var platformView = GetPlatformView();
                platformView?.Focus(Microsoft.UI.Xaml.FocusState.Pointer);
            }
#endif
#if ANDROID
            if (e.ActionType == SKTouchAction.Pressed && !IsFocused)
            {
                var platformView = GetPlatformView();
                var request = new FocusRequest();
                platformView?.Focus(request);
            }
#endif
#if IOS || MACCATALYST
            if (e.ActionType == SKTouchAction.Pressed && !IsFocused)
            {
                var platformView = GetPlatformView();
                var request = new FocusRequest();
                App.DebugLogIf("Try to set focus", false);
                platformView?.Focus(request);
                platformView?.SetNeedsFocusUpdate();
                platformView?.UpdateFocusIfNeeded();
                control?.RaiseGotFocus();
            }
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

            if (interior is null)
            {
                control.Bounds = newBounds;
            }
            else
            {
                interior.Bounds = newBounds;

                var rectangles = interior.GetLayoutRectangles(scaleFactor);
                var clientRect = rectangles[InteriorDrawable.HitTestResult.ClientRect];

                control.Bounds = (0, 0, clientRect.Width, clientRect.Height);
            }
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

            if(interior is not null)
            {
                interior.VertPosition = control.VertScrollBarInfo;
                interior.HorzPosition = control.HorzScrollBarInfo;
                interior.Draw(control, graphics);
            }

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
