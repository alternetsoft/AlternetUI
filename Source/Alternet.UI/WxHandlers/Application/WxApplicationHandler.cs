﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Implementation of the <see cref="IApplicationHandler"/> for the WxWidgets library.
    /// </summary>
    public class WxApplicationHandler : DisposableObject, IApplicationHandler
    {
        private const string RequireVersion = "3.2.5";

        private static readonly int[] eventIdentifiers = new int[(int)WxEventIdentifiers.Max + 1];
        private static readonly int minEventIdentifier;
        private static readonly WxEventIdentifiers[] eventIdentifierToEnum;
        private static Native.Application nativeApplication;
        private static readonly KeyboardInputProvider keyboardInputProvider;
        private static readonly MouseInputProvider mouseInputProvider;

        static WxApplicationHandler()
        {
            Native.Application.GetEventIdentifiers(eventIdentifiers);

            minEventIdentifier = eventIdentifiers.Min();
            var maxEventIdentifier = eventIdentifiers.Max();
            var length = maxEventIdentifier - minEventIdentifier + 1;
            eventIdentifierToEnum = new WxEventIdentifiers[length];
            for (int i = 0; i < eventIdentifiers.Length; i++)
                eventIdentifierToEnum[eventIdentifiers[i] - minEventIdentifier] = (WxEventIdentifiers)i;

            if (App.SupressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);

            nativeApplication = new Native.Application();
            nativeApplication.Idle = App.RaiseIdle;
            nativeApplication.LogMessage += NativeApplication_LogMessage;
            nativeApplication.Name = Path.GetFileNameWithoutExtension(
                Process.GetCurrentProcess()?.MainModule?.FileName!);

            keyboardInputProvider = new KeyboardInputProvider(
                nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WxApplicationHandler"/> class.
        /// </summary>
        public WxApplicationHandler()
        {
            var wxWidgets = WebBrowserHandlerApi.WebBrowser_GetLibraryVersionString_();

            var condition = wxWidgets.ToLower().Replace("wxwidgets ", string.Empty) != RequireVersion;
            if (condition)
            {
                App.LogSeparator();
                App.LogError($"Requires WxWidgets {RequireVersion}, yours is {wxWidgets}");
                App.Log(@"Delete External\WxWidgets\ sub-folder and call Install script");
                App.LogSeparator();
            }

            LogUtils.RegisterLogAction("Use Pless Caret", () => { UsePlessCaret = true; });
        }

        /// <summary>
        /// Gets or sets whether to use internal caret. This can be used for testing purposes.
        /// </summary>
        public static bool UsePlessCaret { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to use dummy timer which doesn't call timer event.
        /// This can be used for testing purposes.
        /// </summary>
        public static bool UseDummyTimer { get; set; } = false;

        internal static WxEventIdentifiers MapToEventIdentifier(int eventId)
        {
            var id = eventId - minEventIdentifier;

            if (id < 0 || id >= eventIdentifierToEnum.Length)
                return WxEventIdentifiers.None;

            return eventIdentifierToEnum[id];
        }

        internal static int MinEventIdentifier => minEventIdentifier;

        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        public bool ExitOnFrameDelete
        {
            get => nativeApplication.GetExitOnFrameDelete();
            set => nativeApplication.SetExitOnFrameDelete(value);
        }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        public bool IsActive => nativeApplication.IsActive();

        /// <inheritdoc/>
        public bool InUixmlPreviewerMode
        {
            get => nativeApplication.InUixmlPreviewerMode;
            set => nativeApplication.InUixmlPreviewerMode = value;
        }

        internal static Native.Clipboard NativeClipboard => nativeApplication.Clipboard;

        internal static Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;

        internal static Native.Application NativeApplication => nativeApplication;

        internal static Native.Mouse NativeMouse => nativeApplication.Mouse;

        internal static string EventArgString => nativeApplication.EventArgString;

        /// <summary>
        /// Gets pointer to WxWidget control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IntPtr WxWidget(IControl? control)
        {
            if (control is null)
                return default;
            return ((UI.Native.Control)control.NativeControl).WxWidget;
        }

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public void Exit()
        {
            nativeApplication.Exit();
        }

        /// <inheritdoc/>
        public IControlFactoryHandler CreateControlFactoryHandler()
        {
            return new WxControlFactoryHandler();
        }

        /// <inheritdoc/>
        public bool HasPendingEvents()
        {
            return nativeApplication.HasPendingEvents();
        }

        /// <inheritdoc/>
        public void Run(Window window)
        {
            nativeApplication.Run(
                ((WindowHandler)window.Handler).NativeControl);
        }

        /// <inheritdoc/>
        public IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            return new WxDialogFactoryHandler();
        }

        /// <inheritdoc/>
        public IClipboardHandler CreateClipboardHandler()
        {
            return new WxClipboardHandler();
        }

        /// <inheritdoc/>
        public void ProcessPendingEvents()
        {
            nativeApplication.ProcessPendingEvents();
        }

        /// <inheritdoc/>
        public void ExitMainLoop()
        {
            nativeApplication.ExitMainLoop();
        }

        /// <inheritdoc/>
        public void SetTopWindow(Window window)
        {
            nativeApplication.SetTopWindow(WxApplicationHandler.WxWidget(window));
        }

        /// <inheritdoc/>
        public void WakeUpIdle()
        {
            nativeApplication.WakeUpIdle();
        }

        /// <inheritdoc/>
        public void BeginInvoke(Action action)
        {
            nativeApplication.BeginInvoke(action);
        }

        private static void NativeApplication_LogMessage()
        {
            var s = nativeApplication.EventArgString;

            App.LogNativeMessage(s);
        }

        /// <inheritdoc/>
        public ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new WxSystemSettingsHandler();
        }

        /// <inheritdoc/>
        public void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        /// <inheritdoc/>
        public Control? GetFocusedControl()
        {
            var focusedNativeControl = Native.Control.GetFocusedControl();
            if (focusedNativeControl == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(focusedNativeControl);
            if (handler == null || !handler.IsAttached)
                return null;

            return handler.Control;
        }

        /// <inheritdoc/>
        public Window? GetActiveWindow()
        {
            var activeWindow = Native.Window.ActiveWindow;
            if (activeWindow == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(activeWindow) ??
                throw new InvalidOperationException();
            return ((WindowHandler)handler).Control;
        }

        /// <inheritdoc/>
        public IPrintingHandler CreatePrintingHandler()
        {
            return new WxPrintingHandler();
        }

        /// <inheritdoc/>
        public IGraphicsFactoryHandler CreateGraphicsFactoryHandler()
        {
            return new WxGraphicsFactoryHandler();
        }

        /// <inheritdoc/>
        public ICaretHandler CreateCaretHandler()
        {
            if (UsePlessCaret)
                return new PlessCaretHandler();
            return new WxCaretHandler();
        }

        /// <inheritdoc/>
        public ICaretHandler CreateCaretHandler(Control control, int width, int height)
        {
            if (UsePlessCaret)
                return new PlessCaretHandler(control, width, height);
            return new WxCaretHandler(control, width, height);
        }

        /// <inheritdoc/>
        public ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            return new WxSoundFactoryHandler();
        }

        /// <inheritdoc/>
        public bool InvokeRequired => nativeApplication.InvokeRequired;

        /// <inheritdoc/>
        public IControlPainterHandler CreateControlPainterHandler()
        {
            return new WxControlPainterHandler();
        }

        /// <inheritdoc/>
        public IMemoryHandler CreateMemoryHandler()
        {
            return new WxMemoryHandler();
        }

        /// <inheritdoc/>
        public IToolTipFactoryHandler CreateToolTipFactoryHandler()
        {
            return new WxToolTipFactoryHandler();
        }

        /// <inheritdoc/>
        public object? GetAttributeValue(string name)
        {
            if(name == "NotifyIcon.IsAvailable")
            {
                return Native.NotifyIcon.IsAvailable;
            }

            return null;
        }

        /// <inheritdoc/>
        public INotifyIconHandler CreateNotifyIconHandler()
        {
            return new UI.Native.NotifyIcon();
        }

        /// <inheritdoc/>
        public ITimerHandler CreateTimerHandler(Timer timer)
        {
            if (UseDummyTimer)
                return new DummyTimerHandler();
            return new UI.Native.Timer();
        }

        /// <inheritdoc/>
        public void CrtSetDbgFlag(int value)
        {
            WebBrowserHandlerApi.WebBrowser_CrtSetDbgFlag_(value);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            nativeApplication.Idle = null;
            nativeApplication.LogMessage = null;
            keyboardInputProvider.Dispose();
            mouseInputProvider.Dispose();
            nativeApplication.Dispose();
            nativeApplication = null!;
        }

        internal static void LogEventIdentifiers()
        {
            App.LogSeparator();

            foreach(var item in Enum.GetValues(typeof(WxEventIdentifiers)))
            {
                App.LogNameValue(item, eventIdentifiers[(int)item]);
            }

            App.LogSeparator();
        }

        /// <inheritdoc/>
        public IMouseHandler CreateMouseHandler()
        {
            return new WxMouseHandler();
        }

        /// <inheritdoc/>
        public IKeyboardHandler CreateKeyboardHandler()
        {
            return new WxKeyboardHandler();
        }
    }
}
