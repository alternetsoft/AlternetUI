using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI.Extensions;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Dispatching;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IApplicationHandler"/> for the MAUI platform.
    /// </summary>
    public partial class MauiApplicationHandler : DisposableObject, IApplicationHandler
    {
        private static bool themeChangedHandlerRegistered;

        static MauiApplicationHandler()
        {
            App.SetUnhandledExceptionModes(UnhandledExceptionMode.CatchWithThrow);
            DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
        }

        /// <inheritdoc/>
        public virtual bool ExitOnFrameDelete
        {
            get => false;
            set
            {
            }
        }

        /// <inheritdoc/>
        public virtual bool IsActive
        {
            get => false;
        }

        /// <inheritdoc/>
        public virtual bool InUixmlPreviewerMode
        {
            get => false;
            set
            {
            }
        }

        /// <inheritdoc/>
        public virtual bool IsInvokeRequired
        {
            get
            {
                var isMainThread = MainThread.IsMainThread;
                return !isMainThread;
            }
        }

        /// <summary>
        /// Registers <see cref="Microsoft.Maui.Controls.Application.RequestedThemeChanged"/>
        /// event handler.
        /// </summary>
        public static void RegisterThemeChangedHandler()
        {
            if (Microsoft.Maui.Controls.Application.Current is null)
                return;

            if (themeChangedHandlerRegistered)
                return;
            themeChangedHandlerRegistered = true;

            Microsoft.Maui.Controls.Application.Current.RequestedThemeChanged += (s, a) =>
            {
                SystemSettings.ResetColors();

                var elements = MauiUtils.GetAllChildren<IRaiseSystemColorsChanged>();

                foreach(var element in elements)
                {
                    element.RaiseSystemColorsChanged();
                }
            };
        }

        /// <summary>
        /// Gets main page from the first window of the application.
        /// </summary>
        /// <returns></returns>
        public static Page? GetMainPageFromApplication()
        {
            return MauiUtils.GetMainPageFromApplication();
        }

        /// <summary>
        /// Gets parent <see cref="Microsoft.Maui.Controls.Page"/> for the specified control.
        /// If control is not attached to the parent, this function returns main page.
        /// </summary>
        /// <param name="control">Control for which to get the parent page.</param>
        /// <returns></returns>
        public static Microsoft.Maui.Controls.Page? GetParentPage(AbstractControl? control)
        {
            return MauiUtils.GetParentPage(control);
        }

        /// <summary>
        /// Converts screen coordinates to client coordinates.
        /// </summary>
        /// <param name="position">Point in screen coordinates.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static PointD ScreenToClient(PointD position, AbstractControl? control)
        {
            var topLeft = ClientToScreen(PointD.Empty, control);

            var x = position.X - topLeft.X;
            var y = position.Y - topLeft.Y;

            return (x, y);
        }

        /// <summary>
        /// Converts client coordinates to screen coordinates.
        /// </summary>
        /// <param name="position">Point in client coordinates.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static PointD ClientToScreen(PointD position, AbstractControl? control)
        {
            PointD absolutePos = PointD.Empty;

            ControlView? container = null;

            while (control is not null)
            {
                container = ControlView.GetContainer(control);

                if(container is null)
                {
                    absolutePos += control.Location;
                    control = control.Parent;
                }
                else
                {
                    break;
                }
            }

            if (container is null)
            {
                absolutePos = PointD.MinValue;
            }
            else
            {
                absolutePos = MauiUtils.GetAbsolutePosition(container);
            }

            var x = absolutePos.X + position.X;
            var y = absolutePos.Y + position.Y;

            return (x, y);
        }

        /// <inheritdoc/>
        public virtual void ExitMainLoop()
        {
            MauiUtils.CloseApplication();
        }

        /// <inheritdoc/>
        public virtual IControlFactoryHandler CreateControlFactoryHandler()
        {
            return new MauiControlFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual IPrintingHandler CreatePrintingHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void ProcessPendingEvents()
        {
        }

        /// <inheritdoc/>
        public virtual Window? GetActiveWindow()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual AbstractControl? GetFocusedControl()
        {
            // This should return null.
            return null;
        }

        /// <inheritdoc/>
        public virtual ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new MauiSystemSettingsHandler();
        }

        /// <inheritdoc/>
        public virtual ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual ITimerHandler CreateTimerHandler(Timer timer)
        {
            return new MauiTimerHandler();
        }

        /// <inheritdoc/>
        public virtual void Run(Window window)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void SetTopWindow(Window window)
        {
        }

        /// <inheritdoc/>
        public virtual IActionSimulatorHandler CreateActionSimulatorHandler()
        {
            return new DummyActionSimulatorHandler();
        }

        /// <inheritdoc/>
        public virtual void BeginInvoke(Action action)
        {
            var isMainThread = MainThread.IsMainThread;
            if (isMainThread)
                action();
            else
                MainThread.BeginInvokeOnMainThread(action);
        }

        /// <inheritdoc/>
        public virtual bool HasPendingEvents()
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void Exit()
        {
            MauiUtils.CloseApplication();
        }

        /// <inheritdoc/>
        public virtual IMemoryHandler CreateMemoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IControlPainterHandler CreateControlPainterHandler()
        {
            return new PlessControlPainterHandler();
        }

        /// <inheritdoc/>
        public virtual IClipboardHandler CreateClipboardHandler()
        {
            return new MauiClipboardHandler();
        }

        /// <inheritdoc/>
        public virtual IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            return new MauiDialogFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual INotifyIconHandler CreateNotifyIconHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual object? GetAttributeValue(string name)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IToolTipFactoryHandler CreateToolTipFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual ICaretHandler CreateCaretHandler(AbstractControl control, int width, int height)
        {
            if(WindowsCaretHandler.UseIfPossible)
                return new WindowsCaretHandler(control, width, height);
            return new PlessCaretHandler(control, width, height);
        }

        /// <inheritdoc/>
        public virtual IGraphicsFactoryHandler CreateGraphicsFactoryHandler()
        {
            return new MauiGraphicsFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual void CrtSetDbgFlag(int value)
        {
        }

        /// <inheritdoc/>
        public virtual IMouseHandler CreateMouseHandler()
        {
            return new MauiMouseHandler();
        }

        /// <inheritdoc/>
        public virtual IKeyboardHandler CreateKeyboardHandler()
        {
            return MauiKeyboardHandler.Default;
        }

        /// <inheritdoc/>
        public virtual void WakeUpIdle()
        {
        }

        /// <inheritdoc/>
        public virtual PropertyUpdateResult SetAppearance(ApplicationAppearance appearance)
        {
            return PropertyUpdateResult.Failure;
        }

        private static void DeviceDisplay_MainDisplayInfoChanged(
            object? sender,
            DisplayInfoChangedEventArgs e)
        {
            Display.Reset();
        }
    }
}
