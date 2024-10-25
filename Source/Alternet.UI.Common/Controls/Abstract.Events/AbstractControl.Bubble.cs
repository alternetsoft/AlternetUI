using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Bubbles long tap event with the specified argument.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="e"></param>
        public static void BubbleLongTap(
            AbstractControl? originalTarget,
            LongTapEventArgs e)
        {
            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;
            currentTarget.RaiseLongTap(e);
        }

        /// <summary>
        /// Bubbles mouse move event with the specified parameters.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="timestamp">Event time.</param>
        /// <param name="position">Mouse position.</param>
        /// <param name="handled">Result of the event procesing.</param>
        public static void BubbleMouseMove(
            AbstractControl? originalTarget,
            long timestamp,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs = new MouseEventArgs(
                currentTarget,
                originalTarget!,
                timestamp,
                position.Value);
            currentTarget.RaiseMouseMove(eventArgs);
        }

        /// <summary>
        /// Bubbles mouse wheel event with the specified parameters.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="timestamp">Event time.</param>
        /// <param name="delta">Mouse wheel delta value.</param>
        /// <param name="position">Mouse position.</param>
        /// <param name="handled">Result of the event procesing.</param>
        public static void BubbleMouseWheel(
            AbstractControl? originalTarget,
            long timestamp,
            int delta,
            PointD? position,
            out bool handled)
        {
            handled = false;

            if (mouseWheelTimestamp == timestamp)
                return;
            mouseWheelTimestamp = timestamp;

            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs
                = new MouseEventArgs(
                    currentTarget,
                    originalTarget!,
                    timestamp,
                    position.Value);
            eventArgs.Delta = delta;
            currentTarget.RaiseMouseWheel(eventArgs);
        }

        /// <summary>
        /// Bubbles mouse down event with the specified parameters.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="timestamp">Event time.</param>
        /// <param name="changedButton">Pressed button.</param>
        /// <param name="position">Mouse position.</param>
        /// <param name="deviceType">Device which raised the event.</param>
        /// <param name="handled">Result of the event procesing.</param>
        public static void BubbleMouseDown(
            AbstractControl? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled,
            TouchDeviceType deviceType = TouchDeviceType.Mouse)
        {
            PlessMouse.SetButtonPressed(changedButton);

            handled = false;
            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs = new MouseEventArgs(
                currentTarget,
                originalTarget!,
                changedButton,
                timestamp,
                position.Value);
            eventArgs.DeviceType = deviceType;

            currentTarget.RaiseMouseDown(eventArgs);
        }

        /// <summary>
        /// Bubbles mouse double-click event with the specified parameters.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="timestamp">Event time.</param>
        /// <param name="changedButton">Pressed button.</param>
        /// <param name="position">Mouse position.</param>
        /// <param name="handled">Result of the event procesing.</param>
        public static void BubbleMouseDoubleClick(
            AbstractControl? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs =
                new MouseEventArgs(
                    currentTarget,
                    originalTarget!,
                    changedButton,
                    timestamp,
                    position.Value);
            currentTarget.RaiseMouseDoubleClick(eventArgs);
        }

        /// <summary>
        /// Bubbles mouse up event with the specified parameters.
        /// </summary>
        /// <param name="originalTarget">Control on which event is originally fired.</param>
        /// <param name="timestamp">Event time.</param>
        /// <param name="changedButton">Pressed button.</param>
        /// <param name="deviceType">Device which raised the event.</param>
        /// <param name="position">Mouse position.</param>
        /// <param name="handled">Result of the event procesing.</param>
        public static void BubbleMouseUp(
            AbstractControl? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled,
            TouchDeviceType deviceType = TouchDeviceType.Mouse)
        {
            PlessMouse.SetButtonPressed(changedButton, false);

            handled = false;
            var currentTarget = AbstractControl.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs
                = new MouseEventArgs(
                    currentTarget,
                    originalTarget!,
                    changedButton,
                    timestamp,
                    position.Value);
            eventArgs.DeviceType = deviceType;

            currentTarget.RaiseMouseUp(eventArgs);
        }

        /// <summary>
        /// Calls <see cref="BubbleKeyDown(KeyEventArgs)"/> for the focused control with
        /// the specified parameters.
        /// </summary>
        /// <param name="modifiers">The set of modifier keys pressed
        /// at the time when event was received.</param>
        /// <param name="key">Key.</param>
        /// <param name="repeatCount">Key repeat count.</param>
        /// <param name="handled">Result of the key procesing.</param>
        /// <param name="keyStates">The state of the key referenced by the event. Optional.
        /// Equals <see cref="KeyStates.Down"/> if not specified.</param>
        public static void BubbleKeyDown(
            Key key,
            ModifierKeys modifiers,
            uint repeatCount,
            out bool handled,
            KeyStates keyStates = KeyStates.Down)
        {
            var control = AbstractControl.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, keyStates, modifiers, repeatCount);
            control.BubbleKeyDown(eventArgs);
            handled = eventArgs.Handled;
        }

        /// <summary>
        /// Calls <see cref="BubbleKeyUp(KeyEventArgs)"/> for the focused control with
        /// the specified parameters.
        /// </summary>
        /// <param name="modifiers">The set of modifier keys pressed
        /// at the time when event was received.</param>
        /// <param name="key">Key.</param>
        /// <param name="repeatCount">Key repeat count.</param>
        /// <param name="handled">Result of the key procesing.</param>
        /// <param name="keyStates">The state of the key referenced by the event. Optional.
        /// Equals <see cref="KeyStates.None"/> if not specified.</param>
        public static void BubbleKeyUp(
            Key key,
            ModifierKeys modifiers,
            uint repeatCount,
            out bool handled,
            KeyStates keyStates = KeyStates.None)
        {
            var control = AbstractControl.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, keyStates, modifiers, repeatCount);
            control.BubbleKeyUp(eventArgs);
            handled = eventArgs.Handled;
        }

        /// <summary>
        /// Calls <see cref="BubbleKeyPress(KeyPressEventArgs)"/> for the focused control with
        /// the specified parameters.
        /// </summary>
        /// <param name="keyChar">Character of the pressed Key.</param>
        /// <param name="handled">Result of the key procesing.</param>
        public static void BubbleTextInput(char keyChar, out bool handled)
        {
            var control = AbstractControl.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyPressEventArgs(control, keyChar);
            control.BubbleKeyPress(eventArgs);
            handled = eventArgs.Handled;
        }

        /// <summary>
        /// Bubbles specified key event action with arguments.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="e">Event arguments.</param>
        /// <param name="action">Action to call.</param>
        public virtual void BubbleKeyAction<T>(T e, Action<AbstractControl, T> action)
            where T : HandledEventArgs
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                action(form, e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                action(control, e);

                if (e.Handled || !control.BubbleKeys)
                    return;
                control = control.Parent;
            }
        }

        /// <summary>
        /// Bubbles <see cref="RaiseKeyPress"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void BubbleKeyPress(KeyPressEventArgs e)
        {
            BubbleKeyAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyPress(e);
            });
        }

        /// <summary>
        /// Bubbles <see cref="RaiseKeyUp"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void BubbleKeyUp(KeyEventArgs e)
        {
            BubbleKeyAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyUp(e);
            });
        }

        /// <summary>
        /// Bubbles <see cref="RaiseKeyDown"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void BubbleKeyDown(KeyEventArgs e)
        {
            BubbleKeyAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyDown(e);
            });
        }

        /// <summary>
        /// Bubbles <see cref="RaiseKeyUp"/> or <see cref="RaiseKeyDown"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="raiseUpEvent">Whether to raise up or down event.</param>
        public virtual void BubbleKeyUpOrDown(KeyEventArgs e, bool raiseUpEvent)
        {
            BubbleKeyAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                if(raiseUpEvent)
                    s.RaiseKeyUp(e);
                else
                    s.RaiseKeyDown(e);
            });
        }

        /// <summary>
        /// Bubbles <see cref="RaiseHelpRequested"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void BubbleHelpRequested(HelpEventArgs e)
        {
            RaiseHelpRequested(e);

            if (!e.Handled)
                Parent?.BubbleHelpRequested(e);
        }

        /// <summary>
        /// Bubbles <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void BubbleErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var currentTarget = this;

            while(currentTarget != null)
            {
                currentTarget.ErrorsChanged?.Invoke(currentTarget, e);
                currentTarget = currentTarget.Parent;
            }
        }
    }
}
