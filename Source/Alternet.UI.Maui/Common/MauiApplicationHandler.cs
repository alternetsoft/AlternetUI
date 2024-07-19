using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IApplicationHandler"/> for the MAUI platform.
    /// </summary>
    public class MauiApplicationHandler : DisposableObject, IApplicationHandler
    {
        static MauiApplicationHandler()
        {
            if (App.IsUnknownOS)
            {
                App.IsAndroidOS = OperatingSystem.IsAndroid();

                if (App.IsAndroidOS)
                {
                    App.IsUnknownOS = false;
                    App.BackendOS = OperatingSystems.Android;
                    return;
                }

                App.IsIOS = OperatingSystem.IsIOS();

                if (App.IsIOS)
                {
                    App.IsUnknownOS = false;
                    App.BackendOS = OperatingSystems.IOS;
                    return;
                }
            }
        }

        /// <inheritdoc/>
        bool IApplicationHandler.ExitOnFrameDelete
        {
            get => false;
            set
            {
            }
        }

        /// <inheritdoc/>
        bool IApplicationHandler.IsActive
        {
            get => false;
        }

        /// <inheritdoc/>
        bool IApplicationHandler.InUixmlPreviewerMode
        {
            get => false;
            set
            {
            }
        }

        /// <inheritdoc/>
        bool IApplicationHandler.InvokeRequired
        {
            get => false;
        }

        /// <summary>
        /// Converts screen coordinates to client coordinates.
        /// </summary>
        /// <param name="position">Point in screen coordinates.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static PointD ScreenToClient(PointD position, Control control)
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
        public static PointD ClientToScreen(PointD position, Control control)
        {
            PointD absolutePos;

            if (control.Handler is not MauiControlHandler handler
                || handler.Container is null)
            {
                absolutePos = PointD.MinValue;
            }
            else
            {
                absolutePos = handler.Container.GetAbsolutePosition();
            }

            var x = absolutePos.X + position.X;
            var y = absolutePos.Y + position.Y;

            return (x, y);
        }

        /// <inheritdoc/>
        void IApplicationHandler.ExitMainLoop()
        {
            Microsoft.Maui.Controls.Application.Current?.Quit();
        }

        /// <inheritdoc/>
        IControlFactoryHandler IApplicationHandler.CreateControlFactoryHandler()
        {
            return new MauiControlFactoryHandler();
        }

        /// <inheritdoc/>
        IPrintingHandler IApplicationHandler.CreatePrintingHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void IApplicationHandler.ProcessPendingEvents()
        {
        }

        /// <inheritdoc/>
        Window? IApplicationHandler.GetActiveWindow()
        {
            return null;
        }

        /// <inheritdoc/>
        Control? IApplicationHandler.GetFocusedControl()
        {
            return null;
        }

        /// <inheritdoc/>
        ISystemSettingsHandler IApplicationHandler.CreateSystemSettingsHandler()
        {
            return new MauiSystemSettingsHandler();
        }

        /// <inheritdoc/>
        ISoundFactoryHandler IApplicationHandler.CreateSoundFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        ITimerHandler IApplicationHandler.CreateTimerHandler(Timer timer)
        {
            return new MauiTimerHandler();
        }

        /// <inheritdoc/>
        void IApplicationHandler.Run(Window window)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void IApplicationHandler.SetTopWindow(Window window)
        {
        }

        /// <inheritdoc/>
        void IApplicationHandler.BeginInvoke(Action action)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        bool IApplicationHandler.HasPendingEvents()
        {
            return false;
        }

        /// <inheritdoc/>
        void IApplicationHandler.Exit()
        {
            Microsoft.Maui.Controls.Application.Current?.Quit();
        }

        /// <inheritdoc/>
        IMemoryHandler IApplicationHandler.CreateMemoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IControlPainterHandler IApplicationHandler.CreateControlPainterHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IClipboardHandler IApplicationHandler.CreateClipboardHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IDialogFactoryHandler IApplicationHandler.CreateDialogFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        INotifyIconHandler IApplicationHandler.CreateNotifyIconHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        object? IApplicationHandler.GetAttributeValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IToolTipFactoryHandler IApplicationHandler.CreateToolTipFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        ICaretHandler IApplicationHandler.CreateCaretHandler(Control control, int width, int height)
        {
            return new PlessCaretHandler(control, width, height);
        }

        /// <inheritdoc/>
        IGraphicsFactoryHandler IApplicationHandler.CreateGraphicsFactoryHandler()
        {
            return new MauiGraphicsFactoryHandler();
        }

        /// <inheritdoc/>
        void IApplicationHandler.CrtSetDbgFlag(int value)
        {
        }

        /// <inheritdoc/>
        public IMouseHandler CreateMouseHandler()
        {
            return new PlessMouseHandler();
        }

        /// <inheritdoc/>
        public IKeyboardHandler CreateKeyboardHandler()
        {
#if WINDOWS
            return new MauiKeyboardHandler();
#else
            return new PlessKeyboardHandler();
#endif
        }
    }
}
