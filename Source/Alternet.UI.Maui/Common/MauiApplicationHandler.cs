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

        public static PointD ScreenToClient(PointD position, Control control)
        {
            var topLeft = ClientToScreen(PointD.Empty, control);

            var x = position.X - topLeft.X;
            var y = position.Y - topLeft.Y;

            return (x, y);
        }

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

        IControlFactoryHandler IApplicationHandler.CreateControlFactoryHandler()
        {
            return new MauiControlFactoryHandler();
        }

        IPrintingHandler IApplicationHandler.CreatePrintingHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void IApplicationHandler.ProcessPendingEvents()
        {
        }

        Window? IApplicationHandler.GetActiveWindow()
        {
            return null;
        }

        Control? IApplicationHandler.GetFocusedControl()
        {
            return null;
        }

        ISystemSettingsHandler IApplicationHandler.CreateSystemSettingsHandler()
        {
            return new MauiSystemSettingsHandler();
        }

        ISoundFactoryHandler IApplicationHandler.CreateSoundFactoryHandler()
        {
            throw new NotImplementedException();
        }

        ITimerHandler IApplicationHandler.CreateTimerHandler(Timer timer)
        {
            return new MauiTimerHandler();
        }

        void IApplicationHandler.Run(Window window)
        {
            throw new NotImplementedException();
        }

        void IApplicationHandler.SetTopWindow(Window window)
        {
        }

        void IApplicationHandler.BeginInvoke(Action action)
        {
            throw new NotImplementedException();
        }

        bool IApplicationHandler.HasPendingEvents()
        {
            return false;
        }

        void IApplicationHandler.Exit()
        {
            Microsoft.Maui.Controls.Application.Current?.Quit();
        }

        IMemoryHandler IApplicationHandler.CreateMemoryHandler()
        {
            throw new NotImplementedException();
        }

        IControlPainterHandler IApplicationHandler.CreateControlPainterHandler()
        {
            throw new NotImplementedException();
        }

        IClipboardHandler IApplicationHandler.CreateClipboardHandler()
        {
            throw new NotImplementedException();
        }

        IDialogFactoryHandler IApplicationHandler.CreateDialogFactoryHandler()
        {
            throw new NotImplementedException();
        }

        INotifyIconHandler IApplicationHandler.CreateNotifyIconHandler()
        {
            throw new NotImplementedException();
        }

        object? IApplicationHandler.GetAttributeValue(string name)
        {
            throw new NotImplementedException();
        }

        IToolTipFactoryHandler IApplicationHandler.CreateToolTipFactoryHandler()
        {
            throw new NotImplementedException();
        }

        ICaretHandler IApplicationHandler.CreateCaretHandler(Control control, int width, int height)
        {
            return new PlessCaretHandler(control, width, height);
        }

        IGraphicsFactoryHandler IApplicationHandler.CreateGraphicsFactoryHandler()
        {
            return new MauiGraphicsFactoryHandler();
        }

        void IApplicationHandler.CrtSetDbgFlag(int value)
        {
        }

        public IMouseHandler CreateMouseHandler()
        {
            return new PlessMouseHandler();
        }

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
