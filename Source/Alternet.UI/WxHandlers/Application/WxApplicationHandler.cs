using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implementation of the <see cref="IApplicationHandler"/> for the WxWidgets library.
    /// </summary>
    public class WxApplicationHandler : DisposableObject, IApplicationHandler
    {
        /// <summary>  
        /// Gets or sets a value indicating whether to use the internal control painter.  
        /// </summary>  
        /// <remarks>  
        /// This property determines whether the application uses the internal control painter  
        /// or an external one.  
        /// </remarks>  
        public static bool UseInternalControlPainter = true;

        private const string RequireVersion = "3.3.1";

        private static readonly int[] eventIdentifiers = new int[(int)WxEventIdentifiers.Max + 1];
        private static readonly int minEventIdentifier;
        private static readonly WxEventIdentifiers[] eventIdentifierToEnum;
        private static Native.Application nativeApplication;
        private static readonly KeyboardInputProvider keyboardInputProvider;
        private static readonly MouseInputProvider mouseInputProvider;

        private static bool assertedWxWidgetsVersion;

        static WxApplicationHandler()
        {
            Native.Application.GetEventIdentifiers(eventIdentifiers);

            minEventIdentifier = eventIdentifiers.Min();
            var maxEventIdentifier = eventIdentifiers.Max();
            var length = maxEventIdentifier - minEventIdentifier + 1;
            eventIdentifierToEnum = new WxEventIdentifiers[length];
            
            for (int i = 0; i < eventIdentifiers.Length; i++)
            {
                eventIdentifierToEnum[eventIdentifiers[i] - minEventIdentifier]
                    = (WxEventIdentifiers)i;
            }

            if (App.SuppressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);

            nativeApplication = new Native.Application();
            nativeApplication.AssertFailure = OnNativeApplicationAssertFailure;

            nativeApplication.QueryEndSession = OnNativeApplicationQueryEndSession;
            nativeApplication.EndSession = OnNativeApplicationEndSession;
            nativeApplication.ActivateApp = OnNativeApplicationActivateApp;
            nativeApplication.Hibernate = OnNativeApplicationHibernate;
            nativeApplication.DialupConnected = OnNativeApplicationDialupConnected;
            nativeApplication.DialupDisconnected = OnNativeApplicationDialupDisconnected;
            nativeApplication.ExceptionInMainLoop = OnNativeApplicationExceptionInMainLoop;
            nativeApplication.UnhandledException = OnNativeApplicationUnhandledException;
            nativeApplication.FatalException = OnNativeApplicationFatalException;

            Native.Application.GlobalObject = nativeApplication;
            nativeApplication.LogMessage += OnNativeApplicationLogMessage;
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
            if (DebugUtils.IsDebugDefined)
            {
                LogUtils.RegisterLogAction(
                    "Log mapping: Key <-> WxWidgetsKeyCode",
                    WxKeyboardHandler.KeyAndWxMapping.LogToFile);
            }

            if (!App.IsWindowsOS)
                Caret.UseGeneric = true;
        }

        /// <summary>
        /// Gets or sets whether to use dummy timer which doesn't call timer event.
        /// This can be used for testing purposes.
        /// </summary>
        public static bool UseDummyTimer { get; set; } = false;

        /// <summary>
        /// Gets or sets idle action of the native application.
        /// </summary>
        public Action? IdleAction
        {
            get
            {
                return nativeApplication.Idle;
            }

            set
            {
                nativeApplication.Idle = value;
            }
        }

        /// <summary>
        /// Gets the version of the used WxWidgets library.
        /// </summary>
        /// <returns>A string representing the version of the WxWidgets library.</returns>
        public static string GetWxWidgetsVersion()
        {
            var wxWidgets = WebBrowserHandlerApi.WebBrowser_GetLibraryVersionString_();
            var s = wxWidgets.ToLower().Replace("wxwidgets ", string.Empty);
            return s;
        }

        /// <summary>
        /// Asserts that the version of the WxWidgets library being used
        /// matches the required version.
        /// Logs an error if the versions do not match.
        /// </summary>
        public static void AssertWxWidgetsVersion()
        {
            if (assertedWxWidgetsVersion)
                return;
            assertedWxWidgetsVersion = true;

            var s = GetWxWidgetsVersion();
            var condition = s != RequireVersion;
            if (condition)
            {
                App.LogSeparator();
                App.LogError($"Requires WxWidgets {RequireVersion}, yours is {s}");
                App.Log(@"Delete External\WxWidgets\ sub-folder and call Install script");
                App.LogSeparator();
            }
        }

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
        public virtual bool ExitOnFrameDelete
        {
            get => nativeApplication.GetExitOnFrameDelete();
            set => nativeApplication.SetExitOnFrameDelete(value);
        }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        public virtual bool IsActive => nativeApplication.IsActive();

        /// <inheritdoc/>
        public virtual bool InUixmlPreviewerMode
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
        public virtual void Exit()
        {
            nativeApplication.Exit();
        }

        /// <inheritdoc/>
        public virtual IControlFactoryHandler CreateControlFactoryHandler()
        {
            return new WxControlFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual bool HasPendingEvents()
        {
            return nativeApplication.HasPendingEvents();
        }

        /// <inheritdoc/>
        public virtual void Run(Window window)
        {
            AssertWxWidgetsVersion();

            var nativeWindow = UI.Native.NativeObject.GetNativeWindow(window)
                ?? throw new Exception("Not a window");
            nativeApplication.Run(nativeWindow);
        }

        /// <inheritdoc/>
        public virtual IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            return new WxDialogFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual IClipboardHandler CreateClipboardHandler()
        {
            return new WxClipboardHandler();
        }

        /// <inheritdoc/>
        public virtual bool IsPlatformControl(AbstractControl control)
        {
            return control is Control;
        }

        /// <inheritdoc/>
        public virtual void ProcessPendingEvents()
        {
            nativeApplication.ProcessPendingEvents();
        }

        /// <inheritdoc/>
        public virtual void ExitMainLoop()
        {
            nativeApplication.ExitMainLoop();
        }

        /// <inheritdoc/>
        public virtual void SetTopWindow(Window window)
        {
            nativeApplication.SetTopWindow(WxApplicationHandler.WxWidget(window));
        }

        /// <inheritdoc/>
        public virtual void WakeUpIdle()
        {
            Native.Application.WakeUpIdle();
        }

        /// <inheritdoc/>
        public virtual void BeginInvoke(Action action)
        {
            nativeApplication.BeginInvoke(action);
        }

        private static void OnNativeApplicationLogMessage()
        {
            var s = nativeApplication.EventArgString;

            App.LogNativeMessage(s, LogItemKind.Information);
        }

        private static void OnNativeApplicationQueryEndSession()
        {

        }
        private static void OnNativeApplicationEndSession()
        {
        }
    
        private static void OnNativeApplicationActivateApp()
        {
        }

        private static void OnNativeApplicationHibernate()
        {
        }

        private static void OnNativeApplicationDialupConnected()
        {
        }

        private static void OnNativeApplicationDialupDisconnected()
        {
        }

        private static void OnNativeApplicationExceptionInMainLoop()
        {
        }

        private static void OnNativeApplicationUnhandledException()
        {
        }

        private static void OnNativeApplicationFatalException()
        {
        }

        private static void OnNativeApplicationAssertFailure()
        {
            var s = NativeApplication.EventArgString;

            App.LogNativeMessage(s, LogItemKind.Warning);
        }

        /// <inheritdoc/>
        public virtual ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new WxSystemSettingsHandler();
        }

        /// <inheritdoc/>
        public virtual void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        internal static AbstractControl? FromNativeControl(Native.Control? nativeControl)
        {
            if (nativeControl == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(nativeControl);
            if (handler == null || !handler.IsAttached)
                return null;

            return handler.Control;
        }

        /// <inheritdoc/>
        public virtual AbstractControl? GetFocusedControl()
        {
            return FromNativeControl(Native.Control.GetFocusedControl());
        }

        /// <inheritdoc/>
        public virtual Window? GetActiveWindow()
        {
            var activeWindow = Native.Window.ActiveWindow;
            if (activeWindow == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(activeWindow) ??
                throw new InvalidOperationException();
            return handler.Control as Window;
        }

        /// <inheritdoc/>
        public virtual IActionSimulatorHandler CreateActionSimulatorHandler()
        {
            return new WxActionSimulatorHandler();
        }

        /// <inheritdoc/>
        public virtual IPrintingHandler CreatePrintingHandler()
        {
            return new WxPrintingHandler();
        }

        /// <inheritdoc/>
        public virtual IGraphicsFactoryHandler CreateGraphicsFactoryHandler()
        {
            return new WxGraphicsFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual ICaretHandler CreateCaretHandler()
        {
            if (Caret.UseGeneric)
                return new PlessCaretHandler();
            return new WxCaretHandler();
        }

        /// <inheritdoc/>
        public ICaretHandler CreateCaretHandler(AbstractControl control, int width, int height)
        {
            if (Caret.UseGeneric)
                return new PlessCaretHandler(control, width, height);
            return new WxCaretHandler(control, width, height);
        }

        /// <inheritdoc/>
        public virtual ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            return new WxSoundFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual bool IsInvokeRequired => nativeApplication.InvokeRequired;

        /// <inheritdoc/>
        public virtual IControlPainterHandler CreateControlPainterHandler()
        {
            PlessControlPainterHandler.NativeHandler = new WxControlPainterHandler();
            if (UseInternalControlPainter)
                return new PlessControlPainterHandler();
            return PlessControlPainterHandler.NativeHandler;
        }

        /// <inheritdoc/>
        public virtual IMemoryHandler CreateMemoryHandler()
        {
            return new WxMemoryHandler();
        }

        /// <inheritdoc/>
        public virtual IToolTipFactoryHandler CreateToolTipFactoryHandler()
        {
            return new WxToolTipFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual object? GetAttributeValue(string name)
        {
            if (name == "NotifyIcon.IsAvailable")
            {
                return Native.NotifyIcon.IsAvailable;
            }

            return null;
        }

        /// <inheritdoc/>
        public virtual INotifyIconHandler CreateNotifyIconHandler()
        {
            return new UI.Native.NotifyIcon();
        }

        /// <inheritdoc/>
        public virtual ITimerHandler CreateTimerHandler(Timer timer)
        {
            if (UseDummyTimer)
                return new DummyTimerHandler();
            return new UI.Native.Timer();
        }

        /// <inheritdoc/>
        public virtual void CrtSetDbgFlag(int value)
        {
            WebBrowserHandlerApi.WebBrowser_CrtSetDbgFlag_(value);
        }

        /// <inheritdoc/>
        public virtual PropertyUpdateResult SetAppearance(ApplicationAppearance appearance)
        {
            return nativeApplication.SetAppearance(appearance);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            nativeApplication.Idle = null;
            nativeApplication.LogMessage = null;
            nativeApplication.AssertFailure = null;

            nativeApplication.QueryEndSession = null;
            nativeApplication.EndSession = null;
            nativeApplication.ActivateApp = null;
            nativeApplication.Hibernate = null;
            nativeApplication.DialupConnected = null;
            nativeApplication.DialupDisconnected = null;
            nativeApplication.ExceptionInMainLoop = null;
            nativeApplication.UnhandledException = null;
            nativeApplication.FatalException = null;

            keyboardInputProvider.Dispose();
            mouseInputProvider.Dispose();
            nativeApplication.Dispose();
            nativeApplication = null!;
        }

        internal static void LogEventIdentifiers()
        {
            App.LogSeparator();

            foreach (var item in Enum.GetValues(typeof(WxEventIdentifiers)))
            {
                App.LogNameValue(item, eventIdentifiers[(int)item]);
            }

            App.LogSeparator();
        }

        /// <inheritdoc/>
        public virtual IMouseHandler CreateMouseHandler()
        {
            return new WxMouseHandler();
        }

        /// <inheritdoc/>
        public virtual IKeyboardHandler CreateKeyboardHandler()
        {
            return new WxKeyboardHandler();
        }

        /// <inheritdoc/>
        public IMenuFactory? CreateMenuFactory()
        {
            return new WxMenuFactory();
        }
    }
}
