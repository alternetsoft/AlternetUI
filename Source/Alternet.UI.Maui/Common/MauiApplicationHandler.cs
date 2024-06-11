using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    public class MauiApplicationHandler : DisposableObject, IApplicationHandler
    {
        public static PointI MousePosition;

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

        void IApplicationHandler.NotifyCaptureLost()
        {
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

        void IApplicationHandler.WakeUpIdle()
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

        ICursorFactoryHandler IApplicationHandler.CreateCursorFactoryHandler()
        {
            return new PlessCursorFactoryHandler();
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

        ICaretHandler IApplicationHandler.CreateCaretHandler()
        {
            return new PlessCaretHandler();
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

        public KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }

        public MouseButtonState GetMouseButtonStateFromSystem(MouseButton mouseButton)
        {
            return PlessMouse.GetButtonState(mouseButton);
        }

        public PointI GetMousePositionFromSystem()
        {
            return MousePosition;
        }
    }
}
