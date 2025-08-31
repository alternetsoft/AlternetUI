using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Maui.Extensions;

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
    public partial class ControlView : SKCanvasView, IRaiseSystemColorsChanged
    {
        private SwipeGestureRecognizer? swipeGesture;
        private PanGestureRecognizer? panGesture;
        private InteriorDrawable? interior;
        private SkiaGraphics? graphics;
        private Alternet.UI.AbstractControl? control;
        private bool currentIsDark;

        static ControlView()
        {
            InitMauiHandler();
            ToolTipFactory.BindOverlayToolTips();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlView"/> class.
        /// </summary>
        public ControlView()
        {
            currentIsDark = IsDark;
            EnableTouchEvents = true;

            Touch += OnCanvasTouch;
            SizeChanged += OnSizeChanged;
            PaintSurface += OnPaintSurface;
            Focused += OnFocused;
            Unfocused += OnUnfocused;

            MauiApplicationHandler.RegisterThemeChangedHandler();

            RequireDoubleTapGesture();
        }

        /// <summary>
        /// Gets a value indicating whether the current theme is dark.
        /// </summary>
        public static bool IsDark
        {
            get
            {
                return Alternet.UI.SystemSettings.AppearanceIsDark;
            }
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
        /// Gets the parent page of the current view.
        /// </summary>
        public Page? ParentPage
        {
            get
            {
                return Alternet.UI.MauiUtils.GetPage(this);
            }
        }

        /// <summary>
        /// Gets or sets attached <see cref="Alternet.UI.AbstractControl"/>.
        /// </summary>
        public virtual Alternet.UI.AbstractControl? Control
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

                    var handler = (control as Control)?.Handler as MauiControlHandler;

                    if (handler is not null)
                    {
                        control.RaiseHandleDestroyed(EventArgs.Empty);
                        handler.Container = null;
                    }
                }

                control = value;

                if (control is not null)
                {
                    if (interior is not null && !control.HasOwnInterior)
                        control.AddNotification(interior.Notification);

                    var handler = (control as Control)?.Handler as MauiControlHandler;

                    if (handler is not null)
                    {
                        handler.Container = this;
                        control.RaiseHandleCreated(EventArgs.Empty);
                    }
                }

                InvalidateSurface();
            }
        }

        /// <summary>
        /// Determines whether the specified control has an associated container.
        /// </summary>
        /// <remarks>A container is considered to exist if the <see cref="GetContainer"/>
        /// method returns a non-<see langword="null"/> value for the specified control.</remarks>
        /// <param name="control">The control to check. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified control has
        /// an associated container; otherwise, <see langword="false"/>.</returns>
        public static bool HasContainer(AbstractControl? control)
        {
            return GetContainer(control) is not null;
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
            App.Current.Required();
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
            currentIsDark = IsDark;
            if (control is null)
                return;
            AbstractControl.BubbleSystemColorsChanged(control, EventArgs.Empty);

            if(!control.HasOwnInterior)
                Interior.UpdateThemeMetrics();

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
        /// Attempts to set focus to the view within the specified timeout.
        /// </summary>
        /// <param name="timeout">The maximum time (in milliseconds) to attempt
        /// setting focus. Default is 2000ms.</param>
        /// <remarks>
        /// This method continuously checks if the view is focused and tries to set focus if it isn't.
        /// It runs in a background task and stops once focus is achieved or the timeout expires.
        /// </remarks>
        public async virtual Task TrySetFocusWithTimeout(int timeout = 2000)
        {
            void FocusControl()
            {
                while (!IsFocused)
                {
                    Task.Delay(50).Wait();
                    Dispatcher.Dispatch(() => SetFocusIfPossible());
                }
            }

            await AsyncUtils.RunWithTimeout(() => Task.Run(FocusControl), timeout);
        }

        /// <summary>
        /// Sets focus to the control if it is not already focused and if the platform supports it.
        /// </summary>
        public virtual void SetFocusIfPossible()
        {
#if WINDOWS
            if(!IsFocused)
            {
                var platformView = GetPlatformView();
                var request = new FocusRequest();
                platformView?.Focus(request);
                platformView?.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
            }
#endif
#if ANDROID
            if (!IsFocused)
            {
                var platformView = GetPlatformView();
                var request = new FocusRequest();
                platformView?.Focus(request);
            }
#endif
#if IOS || MACCATALYST
            if (!IsFocused)
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

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Window) || propertyName == nameof(Parent))
            {
                if (currentIsDark != Alternet.UI.SystemSettings.AppearanceIsDark)
                    RaiseSystemColorsChanged();
            }
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
            if (control is null)
                return;

            var position = e.GetPosition(this);
            if (position is null)
                return;
            PointD pt = (position.Value.X, position.Value.Y);

            AbstractControl.BubbleMouseDoubleClick(
                        control,
                        DateTime.Now.Ticks,
                        MouseButton.Left,
                        pt,
                        out _,
                        TouchDeviceType.Mouse);
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

        /// <summary>
        /// Handles the event when the control gains focus.
        /// </summary>
        /// <remarks> Derived classes can override this method to provide
        /// custom behavior when the control gains focus.</remarks>
        /// <param name="sender">The source of the focus event.
        /// This can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the focus event.</param>
        protected virtual void OnFocused(object? sender, FocusEventArgs e)
        {
            control?.RaiseGotFocus(GotFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles the event when the control loses focus.
        /// </summary>
        /// <remarks> Derived classes can override this method
        /// to provide custom behavior when the control becomes unfocused.</remarks>
        /// <param name="sender">The source of the event, typically the control that lost focus.</param>
        /// <param name="e">The event data associated with the focus change.</param>
        protected virtual void OnUnfocused(object? sender, FocusEventArgs e)
        {
            control?.RaiseLostFocus(LostFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles the size change event for the control and updates its bounds accordingly.
        /// </summary>
        /// <remarks>Derived classes can override this method to provide custom behavior when
        /// the control's size changes.</remarks>
        /// <param name="sender">The source of the event, typically the control whose
        /// size has changed.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSizeChanged(object? sender, EventArgs e)
        {
            UpdateInnerControlBounds(Bounds.ToRectD());
            control?.RaiseHandlerSizeChanged(e);
        }

        /// <summary>
        /// Handles touch events on the canvas and raises the corresponding
        /// touch event on the associated control.
        /// </summary>
        /// <remarks>This method processes touch actions such as
        /// <see cref="SKTouchAction.Pressed"/> and
        /// converts them into platform-independent touch events.
        /// If the associated control is not null, the touch
        /// event is raised on the control. On Android, wheel change actions
        /// are handled at the platform level and are ignored by this method.</remarks>
        /// <param name="sender">The source of the touch event, typically the canvas.</param>
        /// <param name="e">The touch event arguments containing details about the touch action.</param>
        protected virtual void OnCanvasTouch(object? sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                SetFocusIfPossible();
            }

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

        /// <summary>
        /// Handles the paint surface event to render the control's visual content.
        /// </summary>
        /// <remarks>This method is responsible for drawing the control's content
        /// and its children onto the provided canvas. It applies scaling,
        /// clears the background, and invokes the control's paint logic.
        /// Subclasses can override this method to customize the painting behavior.
        /// When overriding, ensure that the
        /// base implementation is called to preserve the default rendering logic.</remarks>
        /// <param name="sender">The source of the event, typically the control
        /// triggering the paint operation.</param>
        /// <param name="e">The event data containing the surface and canvas to be painted.</param>
        protected virtual void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            if (control is null)
                return;

            if (interior is not null)
            {
                interior.UpdateThemeMetrics(control.IsDarkBackground);
                interior.VertPosition = control.VertScrollBarInfo;
                interior.HorzPosition = control.HorzScrollBarInfo;
            }

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

            UpdateInnerControlBounds(dc.LocalClipBounds);

            dc.Clear(control.BackColor);

            dc.Save();

            graphics.UseUnscaledDrawImage = UseUnscaledDrawImage;

            var paintArgs = new PaintEventArgs(graphics, control.Bounds);

            control.RaisePaint(paintArgs);

            dc.Restore();

#pragma warning disable
            if (interior is not null)
            {
                interior.Draw(control, graphics);
            }
#pragma warning restore

            graphics.UseUnscaledDrawImage = false;

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

        /// <summary>
        /// Updates the bounds of the inner control to fit within the specified maximum dimensions.
        /// </summary>
        /// <remarks>This method adjusts the size of the inner control to ensure it
        /// does not exceed the
        /// specified maximum dimensions. If the control has an associated interior layout,
        /// the bounds are further
        /// refined based on the layout's client area. The method ensures that the width
        /// and height of the control are non-negative.</remarks>
        /// <param name="max">The maximum allowable dimensions for the inner control,
        /// represented as a rectangle.</param>
        protected virtual void UpdateInnerControlBounds(RectD max)
        {
            if (control is null)
                return;

            control.ResetScaleFactor();

            var bounds = Bounds;

            var newWidth = Math.Min(bounds.Width, max.Width);
            var newHeight = Math.Min(bounds.Height, max.Height);

            newWidth = Math.Max(0, newWidth);
            newHeight = Math.Max(0, newHeight);

            RectD newBounds = (0, 0, newWidth, newHeight);

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
    }
}
