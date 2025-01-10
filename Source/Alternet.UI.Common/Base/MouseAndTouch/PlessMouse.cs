﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the platformless mouse implementation.
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

        private static Timer? longTapTimer;
        private static WeakReference<AbstractControl>? longTapControl;

        /// <summary>
        /// Occurs when <see cref="LastMousePosition"/> property is changed.
        /// </summary>
        public static event EventHandler? LastMousePositionChanged;

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
        /// <param name="position">Mouse position. If <c>null</c>, <see cref="Mouse.GetPosition(AbstractControl)"/>
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
    }
}
