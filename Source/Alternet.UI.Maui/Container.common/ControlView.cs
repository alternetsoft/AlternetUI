using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Implements <see cref="Alternet.UI.AbstractControl"/>
    /// container using <see cref="SKCanvasView"/>.
    /// </summary>
    public partial class ControlView : SKCanvasView
    {
        private SwipeGestureRecognizer? swipeGesture;
        private PanGestureRecognizer? panGesture;
        private InteriorDrawable? interior;
        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        static ControlView()
        {
            App.Current.Required();
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

            MauiApplicationHandler.RegisterThemeChangedHandler();
        }

        /// <summary>
        /// Gets swipe gesture created after call to <see cref="RequireSwipeGesture"/>.
        /// </summary>
        public SwipeGestureRecognizer? SwipeGesture => swipeGesture;

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
                }

                return interior;
            }
        }

        /// <summary>
        /// Gets or sets whether 'DrawImage' methods draw unscaled image. Default is <c>true</c>.
        /// </summary>
        public virtual bool UseUnscaledDrawImage { get; set; } = true;

        /// <summary>
        /// Gets or sets attached <see cref="Alternet.UI.AbstractControl"/>.
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
                    {
                        control.RaiseHandleDestroyed(EventArgs.Empty);
                        handler.Container = null;
                    }
                }

                control = value;

                if (control is not null)
                {
                    if (interior is not null)
                        control.AddNotification(interior.Notification);
                    if (control.Handler is MauiControlHandler handler)
                    {
                        handler.Container = this;
                        control.RaiseHandleCreated(EventArgs.Empty);
                    }
                }

                InvalidateSurface();
            }
        }

        /// <summary>
        /// Gets <see cref="ControlView"/> for the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">Control to get container from.</param>
        /// <returns></returns>
        public static ControlView? GetContainer(AbstractControl? control)
        {
            if (control is Control { Handler: MauiControlHandler handler })
                return handler.Container;
            return null;
        }

        /// <summary>
        /// Gets <see cref="PlatformView"/> for the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">Control to get <see cref="PlatformView"/> from.</param>
        /// <returns></returns>
        public static PlatformView? GetPlatformView(AbstractControl? control)
        {
            var container = GetContainer(control);
            if (container is null)
                return null;
            var platformView = container.GetPlatformView(container.Handler);
            return platformView;
        }

        /// <summary>
        /// Initializes application handler. You should not call this method directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public static void InitMauiHandler()
        {
            App.Handler ??= new MauiApplicationHandler();
        }

        /// <summary>
        /// Registers to receive pan gestures.
        /// </summary>
        public virtual void RequirePanGesture()
        {
            if (panGesture is not null)
                return;

            panGesture = new();
            panGesture.PanUpdated += (sender, e) =>
            {
                OnPanGesture(e);
            };

            GestureRecognizers.Add(panGesture);
        }

        /// <summary>
        /// Registers to receive double tap gestures.
        /// </summary>
        public virtual void RequireDoubleTapGesture()
        {
            TapGestureRecognizer tapGestureRecognizer = new()
            {
                Buttons = ButtonsMask.Primary,
                NumberOfTapsRequired = 2,
            };

            tapGestureRecognizer.Tapped += (s, e) =>
            {
                OnDoubleTapGesture(e);
            };

            GestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Registers to receive swipe gestures.
        /// </summary>
        /// <param name="direction">Swipe directions to subscribe.</param>
        public virtual void RequireSwipeGesture(SwipeDirection direction
            = SwipeDirection.Down | SwipeDirection.Right | SwipeDirection.Left | SwipeDirection.Up)
        {
            if (swipeGesture is not null)
                return;

            swipeGesture = new() { Direction = direction };
            swipeGesture.Swiped += (sender, e) =>
            {
                OnSwipeGesture(e);
            };

            GestureRecognizers.Add(swipeGesture);
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
            if(GetPlatformView() is not null)
                InvalidateSurface();
        }

        /// <summary>
        /// Adds message to log.
        /// </summary>
        /// <param name="s">Message text.</param>
        public virtual void Log(object? s)
        {
            if (s is null)
                return;
            Debug.WriteLine(s);
        }

        /// <summary>
        /// Updates colors after application theme is changed.
        /// </summary>
        public virtual void RaiseSystemColorsChanged()
        {
            if (control is null)
                return;
            control.RaiseSystemColorsChanged(EventArgs.Empty);

            if(Interior.ScrollBarTheme is not null)
            {
                Interior.SetThemeMetrics(
                    Interior.ScrollBarTheme.Value,
                    SystemSettings.AppearanceIsDark);
            }

            Invalidate();
        }

        /// <summary>
        /// Gets whether on-screen keyboard is shown for this control.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsKeyboardShowing() => Alternet.UI.Keyboard.IsSoftKeyboardShowing(Control);

        /// <summary>
        /// Shows on-screen keyboard for this control.
        /// </summary>
        public virtual void ShowKeyboard()
        {
            Alternet.UI.Keyboard.ShowKeyboard(Control);
        }

        /// <summary>
        /// Hides on-screen keyboard for this control.
        /// </summary>
        public virtual void HideKeyboard()
        {
            Alternet.UI.Keyboard.HideKeyboard(Control);
        }

        /// <summary>
        /// Toggles on-screen keyboard visibility for this control.
        /// </summary>
        public virtual void ToggleKeyboard()
        {
            Alternet.UI.Keyboard.ToggleKeyboardVisibility(Control);
        }

        /// <summary>
        /// Raised when swipe gesture with direction to the right is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSwipeRight(SwipedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when swipe gesture with direction to the right is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSwipeLeft(SwipedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when swipe gesture with direction to the right is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSwipeUp(SwipedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when swipe gesture with direction to the right is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSwipeDown(SwipedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when pan gesture is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPanGesture(PanUpdatedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when double tap gesture is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnDoubleTapGesture(TappedEventArgs e)
        {
        }

        /// <summary>
        /// Raised when swipe gesture is recognized.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSwipeGesture(SwipedEventArgs e)
        {
            if (e.Direction.HasFlag(SwipeDirection.Right))
            {
                OnSwipeRight(e);
                return;
            }

            if (e.Direction.HasFlag(SwipeDirection.Left))
            {
                OnSwipeLeft(e);
                return;
            }

            if (e.Direction.HasFlag(SwipeDirection.Up))
            {
                OnSwipeUp(e);
                return;
            }

            if (e.Direction.HasFlag(SwipeDirection.Down))
            {
                OnSwipeDown(e);
                return;
            }
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
            control?.RaiseHandlerSizeChanged(e);
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
                control?.RaiseGotFocus(GotFocusEventArgs.Empty);
            }
#endif
            if (control is null)
                return;

            TouchEventArgs args = MauiUtils.Convert(e, Control);

#if ANDROID
            if(args.ActionType == TouchAction.WheelChanged)
            {
                // Wheel changed is handled in the platform view for android.
                return;
            }
#endif

            control?.RaiseTouch(args);
            e.Handled = args.Handled;
        }

        private void UpdateBounds(RectD max)
        {
            if (control is null)
                return;

            control.ResetScaleFactor();

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

                var rectangles = interior.GetLayoutRectangles(control);
                var clientRect = rectangles[InteriorDrawable.HitTestResult.ClientRect];

                control.Bounds = (0, 0, clientRect.Width, clientRect.Height);
            }
        }

        private void Canvas_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            if (control is null)
                return;

            var dc = e.Surface.Canvas;

            control.ResetScaleFactor();
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

            TemplateUtils.RaisePaintForChildren(control, graphics);

            graphics.UseUnscaledDrawImage = false;

            dc.Restore();

            if(interior is not null)
            {
                interior.VertPosition = control.VertScrollBarInfo;
                interior.HorzPosition = control.HorzScrollBarInfo;
                interior.Draw(control, graphics);
            }

#if ANDROID
            DebugUtils.DebugCallIf(false, () =>
            {
                var platformView = GetPlatformView();
                if (platformView != null)
                {
                    var visibleRect = AndroidUtils.GetWindowVisibleDisplayFrame(platformView);

                    var visibleHeightDips = Alternet.Drawing.GraphicsFactory.PixelToDip(
                        visibleRect.Height,
                        control.ScaleFactor);

                    visibleHeightDips = Math.Min(visibleHeightDips, control.Height);

                    graphics.DrawHorzLine(
                        Alternet.Drawing.Color.Red.AsBrush,
                        (0, visibleHeightDips),
                        control.Width,
                        1);

                    Log($"==============");
                    Log($"DrawingRect: {AndroidUtils.GetDrawingRect(platformView)}");
                    Log($"VisibleRect: {AndroidUtils.GetWindowVisibleDisplayFrame(platformView)}");
                    Log($"LocationInWindow: {AndroidUtils.GetLocationInWindow(platformView)}");
                    Log($"LocationOnScreen: {AndroidUtils.GetLocationOnScreen(platformView)}");
                    Log($"Window.Top: {AndroidUtils.GetDecorView(platformView)?.Top}");
                    Log($"Decor.VisibleRect: {AndroidUtils.GetDecorViewVisibleDisplayFrame(platformView)}");
                }
            });
#endif

            dc.Flush();
            dc.Restore();
        }

        private void SkiaContainer_Focused(object? sender, FocusEventArgs e)
        {
            control?.RaiseGotFocus(GotFocusEventArgs.Empty);
        }

        private void SkiaContainer_Unfocused(object? sender, FocusEventArgs e)
        {
            control?.RaiseLostFocus(LostFocusEventArgs.Empty);
        }
    }
}
