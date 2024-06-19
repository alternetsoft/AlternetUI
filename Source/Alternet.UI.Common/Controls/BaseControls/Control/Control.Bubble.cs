using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        private static long? mouseWheelTimestamp;

        public static void BubbleMouseMove(
            Control? originalTarget,
            long timestamp,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
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

        public static void BubbleMouseWheel(
            Control? originalTarget,
            long timestamp,
            int delta,
            PointD? position,
            out bool handled)
        {
            handled = false;

            if (mouseWheelTimestamp == timestamp)
                return;
            mouseWheelTimestamp = timestamp;

            var currentTarget = Control.GetMouseTargetControl(originalTarget);
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

        public static void BubbleMouseDown(
            Control? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            PlessMouse.SetButtonPressed(changedButton);

            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = PlessMouse.UpdateMousePosition(position, currentTarget);

            var eventArgs = new MouseEventArgs(
                currentTarget,
                originalTarget!,
                changedButton,
                timestamp,
                position.Value);

            currentTarget.RaiseMouseDown(eventArgs);
        }

        public static void BubbleMouseDoubleClick(
            Control? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
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

        public static void BubbleMouseUp(
            Control? originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            PlessMouse.SetButtonPressed(changedButton, false);

            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
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
            currentTarget.RaiseMouseUp(eventArgs);
        }

        public static void BubbleKeyDown(Key key, uint repeatCount, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, repeatCount);
            control.BubbleKeyDown(eventArgs);
            handled = eventArgs.Handled;
        }

        public static void BubbleKeyUp(Key key, uint repeatCount, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, repeatCount);
            control.BubbleKeyUp(eventArgs);
            handled = eventArgs.Handled;
        }

        public static void BubbleTextInput(char keyChar, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyPressEventArgs(control, keyChar);
            control.BubbleKeyPress(eventArgs);
            handled = eventArgs.Handled;
        }

        public virtual void BubbleAction<T>(T e, Action<Control, T> action)
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

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        public virtual void BubbleKeyPress(KeyPressEventArgs e)
        {
            BubbleAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyPress(e);
            });
        }

        public virtual void BubbleKeyUp(KeyEventArgs e)
        {
            BubbleAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyUp(e);
            });
        }

        public virtual void BubbleKeyDown(KeyEventArgs e)
        {
            BubbleAction(e, (s, e) =>
            {
                e.CurrentTarget = s;
                s.RaiseKeyDown(e);
            });
        }

        public virtual void BubbleHelpRequested(HelpEventArgs e)
        {
            RaiseHelpRequested(e);

            if (!e.Handled)
                Parent?.BubbleHelpRequested(e);
        }

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
