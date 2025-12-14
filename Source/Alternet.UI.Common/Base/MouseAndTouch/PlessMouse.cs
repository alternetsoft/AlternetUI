using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the platform
    /// independent mouse implementation.
    /// </summary>
    public static class PlessMouse
    {
        /// <summary>
        /// Gets or sets long tap interval in milliseconds.
        /// </summary>
        /// <remarks>
        /// Long tap event is raised, when time after mouse down event is
        /// greater than this value.
        /// </remarks>
        public static int LongTapInterval = 1500;

        /// <summary>
        /// Gets or sets whether to draw test mouse pointer inside the control.
        /// </summary>
        public static bool ShowTestMouseInControl = false;

        /// <summary>
        /// Gets or sets color of the test mouse pointer.
        /// </summary>
        public static Color TestMouseColor = Color.Red;

        /// <summary>
        /// Gets or sets size of the test mouse pointer.
        /// </summary>
        public static SizeD TestMouseSize = 5;

        private static readonly bool[] Buttons = new bool[(int)MouseButton.Unknown + 1];

        private static (PointD? Position, AbstractControl? Control) lastMousePosition;

        private static bool? ignoreHoveredState;
        private static Timer? longTapTimer;
        private static WeakReference<AbstractControl>? longTapControl;
        private static WeakReferenceValue<AbstractControl> mouseTargetControlOverride = new();

        /// <summary>
        /// Occurs when <see cref="LastMousePosition"/> property is changed.
        /// </summary>
        public static event EventHandler? LastMousePositionChanged;

        /// <summary>
        /// Gets or sets whether to ignore hovered state in the controls.
        /// By default it returns true when application is executed on phone or tablet device.
        /// </summary>
        public static bool IgnoreHoveredState
        {
            get
            {
                return ignoreHoveredState ??= App.IsTabletOrPhoneDevice;
            }

            set
            {
                ignoreHoveredState = value;
            }
        }

        /// <summary>
        /// Gets or sets the control that overrides the default mouse target for input handling.
        /// </summary>
        /// <remarks>When set, mouse input will be directed to the specified control
        /// instead of the default target. This property is typically used
        /// to redirect mouse events in scenarios such as custom
        /// drag-and-drop operations or modal overlays.</remarks>
        public static AbstractControl? MouseTargetControlOverride
        {
            get
            {
                return mouseTargetControlOverride.Value;
            }

            set
            {
                var oldValue = mouseTargetControlOverride.Value;

                if (oldValue == value)
                    return;

                mouseTargetControlOverride.Value = value;
            }
        }

        /// <summary>
        /// Gets last mouse position passed to mouse event handlers.
        /// </summary>
        public static (PointD? Position, AbstractControl? Control) LastMousePosition
        {
            get
            {
                return lastMousePosition;
            }

            set
            {
                if (lastMousePosition == value)
                    return;

                var lastControl = LastMousePosition.Control;

                lastMousePosition = value;

                LastMousePositionChanged?.Invoke(null, EventArgs.Empty);

                if (ShowTestMouseInControl)
                {
                    var hovered = AbstractControl.HoveredControl;

                    lastControl?.Refresh();
                    if (hovered != lastControl)
                        hovered?.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets rectangle for the test mouse pointer.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static RectD GetTestMouseRect(AbstractControl control)
        {
            var mouseLocation = Mouse.GetPosition(control);
            return (mouseLocation, PlessMouse.TestMouseSize);
        }

        /// <summary>
        /// Updates <see cref="LastMousePosition"/>
        /// </summary>
        /// <param name="position">Mouse position.
        /// If <c>null</c>, <see cref="Mouse.GetPosition(AbstractControl)"/>
        /// is used to get mouse position.</param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static PointD UpdateMousePosition(PointD? position, AbstractControl control)
        {
            position ??= Mouse.GetPosition(control);
            PlessMouse.LastMousePosition = (position, control);
            return position.Value;
        }

        /// <summary>
        /// Draws test mouse pointer inside the control.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <param name="dc">Drawing context.</param>
        public static void DrawTestMouseRect(AbstractControl control, Func<Graphics> dc)
        {
            if (control != AbstractControl.HoveredControl)
                return;

            if (control.UserPaint && ShowTestMouseInControl)
            {
                dc().FillRectangle(TestMouseColor.AsBrush, GetTestMouseRect(control));
            }
        }

        /// <summary>
        /// Stops long tap timer if it was started.
        /// </summary>
        public static void CancelLongTapTimer()
        {
            longTapTimer?.Stop();
            longTapControl = null;
        }

        /// <summary>
        /// Starts long tap timer.
        /// </summary>
        /// <param name="control">Control where tap event is started.</param>
        public static void StartLongTapTimer(AbstractControl control)
        {
            if (!control.CanLongTap)
                return;

            if (longTapTimer is null)
            {
                longTapTimer = new();
                longTapTimer.AutoReset = false;

                longTapTimer.TickAction = () =>
                {
                    if (longTapControl is null)
                        return;
                    if (longTapControl.TryGetTarget(out var target))
                    {
                        longTapControl = null;

                        if (LastMousePosition.Control != target || LastMousePosition.Position is null)
                            return;

                        var distanceIsLess = DrawingUtils.DistanceIsLess(
                            target.LastMouseDownPos,
                            LastMousePosition.Position.Value,
                            DragStartEventArgs.MinDragStartDistance);

                        if (!distanceIsLess)
                            return;

                        LongTapEventArgs e
                            = new(TouchDeviceType.Unknown, LastMousePosition.Position.Value);
                        if (target.IsDisposed)
                            return;
                        try
                        {
                            target.RaiseLongTap(e);
                        }
                        catch
                        {
                            if (DebugUtils.IsDebugDefined)
                                throw;
                        }
                    }

                    longTapControl = null;
                };
            }

            longTapTimer.Stop();
            longTapTimer.Interval = LongTapInterval;
            longTapControl = new(control);
            longTapTimer.Start();
        }

        /// <summary>
        /// Gets mouse button pressed state which was changed using
        /// <see cref="SetButtonPressed"/> method.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <returns></returns>
        public static bool IsButtonPressed(MouseButton mouseButton)
        {
            if (mouseButton >= MouseButton.Unknown || mouseButton < 0)
                return false;
            return Buttons[(int)mouseButton];
        }

        /// <summary>
        /// Gets mouse button pressed state which was changed using
        /// <see cref="SetButtonPressed"/> method.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <returns></returns>
        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return IsButtonPressed(mouseButton) ? MouseButtonState.Pressed : MouseButtonState.Released;
        }

        /// <summary>
        /// Sets internal mouse button pressed state. Do not call directly.
        /// </summary>
        /// <param name="button">Mouse button.</param>
        /// <param name="value">Pressed state.</param>
        public static void SetButtonPressed(MouseButton button, bool value = true)
        {
            Buttons[(int)button] = value;
        }

        /// <summary>
        /// Initializes mouse target tracking by subscribing to control
        /// parent change, visibility change and other events. You should not call this method,
        /// this is reserved for internal use.
        /// </summary>
        /// <remarks>This method enables automatic tracking of mouse target controls
        /// by handling changes in control hierarchy and visibility.
        /// It should be called once during application startup or when mouse
        /// target tracking needs to be activated. Subsequent calls have no additional effect.</remarks>
        public static void InitMouseTargetTracking()
        {
            StaticControlEvents.ParentChanged -= HandleEvent;
            StaticControlEvents.VisibleChanged -= HandleEvent;
            StaticControlEvents.Disposed -= HandleEventNoCaptureLost;

            StaticControlEvents.ParentChanged += HandleEvent;
            StaticControlEvents.VisibleChanged += HandleEvent;
            StaticControlEvents.Disposed += HandleEventNoCaptureLost;

            void HandleEvent(object? s, EventArgs e)
            {
                HandleEventEx(s, e, true);
            }

            void HandleEventNoCaptureLost(object? s, EventArgs e)
            {
                HandleEventEx(s, e, false);
            }

            void HandleEventEx(object? s, EventArgs e, bool raiseCaptureLost)
            {
                var c = MouseTargetControlOverride;
                if (c is null)
                    return;
                var sender = s as AbstractControl;
                if (c == sender || c.HasIndirectParent(sender))
                {
                    MouseTargetControlOverride = null;
                    c?.RaiseMouseCaptureLost(EventArgs.Empty);
                }
            }
        }
    }
}
