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

        internal static Control? GetControlUnderMouse()
        {
            var controlUnderMouse = Native.Control.HitTest(MouseDevice.GetScreenPosition());
            if (controlUnderMouse == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(controlUnderMouse);
            if (handler == null)
                return null;

            return handler.IsAttached ? handler.Control : null;
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
    }
}