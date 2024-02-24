using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading;
using Alternet.Drawing;
using Alternet.UI.Threading;

namespace Alternet.UI
{
    /// <summary>
    ///     The InputManager class is responsible for coordinating all of the
    ///     input system in Alternet UI.
    /// </summary>
    public sealed class InputManager : DispatcherObject
    {
        private static readonly KeyboardDevice KeyboardDevice;
        private static readonly MouseDevice MouseDevice;
        private long? mouseWheelTimestamp;

        static InputManager()
        {
            KeyboardDevice = new NativeKeyboardDevice();
            MouseDevice = new NativeMouseDevice();
        }

        private InputManager()
        {
            CheckSTARequirement();
        }

        /// <summary>
        ///     Return the input manager associated with the current context.
        /// </summary>
        public static InputManager Current
        {
            get
            {
                return GetCurrentInputManagerImpl();
            }
        }

        /// <summary>
        ///     Read-only access to the primary keyboard device.
        /// </summary>
        public KeyboardDevice PrimaryKeyboardDevice
        {
            get { return KeyboardDevice; }
        }

        /// <summary>
        ///     Read-only access to the primary mouse device.
        /// </summary>
        public MouseDevice PrimaryMouseDevice
        {
            get { return MouseDevice; }
        }

        /// <summary>
        ///     Internal implementation of InputManager.Current.
        ///     Critical but not TAS - for internal's to use.
        ///     Only exists for perf. The link demand check
        ///     was causing perf in some XAF scenarios.
        /// </summary>
        internal static InputManager UnsecureCurrent
        {
            [FriendAccessAllowed]
            get
            {
                return GetCurrentInputManagerImpl();
            }
        }

        internal static Control? GetMouseTargetControl(Control? control)
        {
            var result = control ?? GetControlUnderMouse();

            while (result is not null)
            {
                if (result.BubbleMouse)
                    result = result.Parent;
                else
                    return result;
            }

            return result;
        }

        internal void ReportMouseMove(
            Control? targetControl,
            long timestamp,
            out bool handled)
        {
            handled = false;
            var control = GetMouseTargetControl(targetControl);
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, targetControl!, timestamp);
            control.RaiseMouseMove(eventArgs);
        }

        internal void ReportMouseDown(
            Control? targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = GetMouseTargetControl(targetControl);
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, targetControl!, changedButton, timestamp);
            control.RaiseMouseDown(eventArgs);
        }

        internal void ReportMouseDoubleClick(
            Control? targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = GetMouseTargetControl(targetControl);
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, targetControl!, changedButton, timestamp);
            control.RaiseMouseDoubleClick(eventArgs);
        }

        internal void ReportMouseUp(
            Control? targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = GetMouseTargetControl(targetControl);
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, targetControl!, changedButton, timestamp);
            control.RaiseMouseUp(eventArgs);
        }

        internal void ReportMouseWheel(
            Control? targetControl,
            long timestamp,
            int delta,
            out bool handled)
        {
            handled = false;

            if (mouseWheelTimestamp == timestamp)
                return;
            mouseWheelTimestamp = timestamp;

            var control = GetMouseTargetControl(targetControl);
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, targetControl!, timestamp);
            eventArgs.Delta = delta;
            control.RaiseMouseWheel(eventArgs);
        }

        internal void ReportKeyDown(Key key, bool isRepeat, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, isRepeat);
            control.RaiseKeyDown(eventArgs);
            handled = eventArgs.Handled;
        }

        internal void ReportKeyUp(Key key, bool isRepeat, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, isRepeat);
            control.RaiseKeyUp(eventArgs);
            handled = eventArgs.Handled;
        }

        internal void ReportTextInput(char keyChar, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyPressEventArgs(control, keyChar);
            control.RaiseKeyPress(eventArgs);
            handled = eventArgs.Handled;
        }

        /// <summary>
        ///     Implementation of InputManager.Current
        /// </summary>
        private static InputManager GetCurrentInputManagerImpl()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            if (dispatcher.InputManager is not InputManager inputManager)
            {
                inputManager = new InputManager();
                dispatcher.InputManager = inputManager;
            }

            return inputManager;
        }

        private static Control? GetControlUnderMouse()
        {
            var controlUnderMouse = Native.Control.HitTest(MouseDevice.GetScreenPosition());
            if (controlUnderMouse == null)
                return null;

            var handler = ControlHandler.NativeControlToHandler(controlUnderMouse);
            if (handler == null)
                return null;

            return handler.IsAttached ? handler.Control : null;
        }

        private void CheckSTARequirement()
        {
            if (!Application.IsWindowsOS)
                return;

            // STA Requirement
            // Alternet UI doesn't necessarily require STA, but many components do.  Examples
            // include Cicero, OLE, COM, etc.  So we throw an exception here if the
            // thread is not STA.
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new InvalidOperationException(SR.Get(SRID.RequiresSTA));
            }
        }
    }
}