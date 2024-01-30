#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Security;
using System;
using System.Diagnostics;
using Alternet.UI.Threading;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    ///     The InputManager class is responsible for coordinating all of the
    ///     input system in Alternet UI.
    /// </summary>
    public sealed class InputManager : DispatcherObject
    {
        private KeyboardDevice _primaryKeyboardDevice;
        private MouseDevice _primaryMouseDevice;

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

        ///<summary>
        ///     Internal implementation of InputManager.Current.
        ///     Critical but not TAS - for internal's to use.
        ///     Only exists for perf. The link demand check was causing perf in some XAF scenarios.
        ///</summary>
        internal static InputManager UnsecureCurrent
        {
            [FriendAccessAllowed]
            get
            {
                return GetCurrentInputManagerImpl();
            }
        }

        ///<summary>
        ///     Implementation of InputManager.Current
        ///</summary>
        private static InputManager GetCurrentInputManagerImpl()
        {
            InputManager inputManager = null;

            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            inputManager = dispatcher.InputManager as InputManager;

            if (inputManager == null)
            {
                inputManager = new InputManager();
                dispatcher.InputManager = inputManager;
            }

            return inputManager;
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

        private InputManager()
        {
            CheckSTARequirement();
            _primaryKeyboardDevice = new NativeKeyboardDevice(this);
            _primaryMouseDevice = new NativeMouseDevice(this);
        }

        /// <summary>
        ///     Read-only access to the primary keyboard device.
        /// </summary>
        public KeyboardDevice PrimaryKeyboardDevice
        {
            get { return _primaryKeyboardDevice; }
        }

        /// <summary>
        ///     Read-only access to the primary mouse device.
        /// </summary>
        public MouseDevice PrimaryMouseDevice
        {
            get { return _primaryMouseDevice; }
        }

        internal void ReportMouseMove(
            Control targetControl,
            long timestamp,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, timestamp);
            control.RaiseMouseMove(eventArgs);
        }

        internal void ReportMouseDown(
            Control targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, changedButton, timestamp);
            control.RaiseMouseDown(eventArgs);
        }

        internal void ReportMouseDoubleClick(
            Control targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, changedButton, timestamp);
            control.RaiseMouseDoubleClick(eventArgs);
        }

        internal void ReportMouseUp(
            Control targetControl,
            long timestamp,
            MouseButton changedButton,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, changedButton, timestamp);
            control.RaiseMouseUp(eventArgs);
        }

        internal void ReportMouseWheel(
            Control targetControl,
            long timestamp,
            int delta,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;

            var eventArgs = new MouseEventArgs(control, timestamp);
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

            var eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, key, isRepeat);
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

            var eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, key, isRepeat);
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

            var eventArgs = new KeyPressEventArgs(Keyboard.PrimaryDevice, keyChar);
            control.RaiseKeyPress(eventArgs);
            handled = eventArgs.Handled;
        }

        private Control GetControlUnderMouse()
        {
            var controlUnderMouse = Native.Control.HitTest(PrimaryMouseDevice.GetScreenPosition());
            if (controlUnderMouse == null)
                return null;

            var handler = ControlHandler.NativeControlToHandler(controlUnderMouse);
            if (handler == null)
                return null;

            return handler.IsAttached ? handler.Control : null;
        }

        internal void ReportMouseEvent(
            Control targetControl,
            RoutedEvent @event,
            InputEventArgs eventArgs,
            out bool handled)
        {
            handled = false;
            var control = targetControl ?? GetControlUnderMouse();
            if (control == null)
                return;
            eventArgs.RoutedEvent = @event;
            control.RaiseEvent(eventArgs);
            handled = eventArgs.Handled;
        }
    }
}

