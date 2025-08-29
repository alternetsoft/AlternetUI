using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IApplicationHandler"/> interface which does nothing.
    /// </summary>
    public partial class PlessApplicationHandler : DisposableObject, IApplicationHandler
    {
        static PlessApplicationHandler()
        {
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
                return !App.IsAppThread;
            }
        }

        /// <inheritdoc/>
        public virtual void ExitMainLoop()
        {
            Environment.Exit(0);
        }

        /// <inheritdoc/>
        public virtual IControlFactoryHandler CreateControlFactoryHandler()
        {
            throw new NotImplementedException();
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
            return AbstractControl.FocusedControl;
        }

        /// <inheritdoc/>
        public virtual ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new PlessSystemSettingsHandler();
        }

        /// <inheritdoc/>
        public virtual ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual ITimerHandler CreateTimerHandler(Timer timer)
        {
            return new DummyTimerHandler();
        }

        /// <inheritdoc/>
        public virtual void Run(Window window)
        {
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
        }

        /// <inheritdoc/>
        public virtual bool HasPendingEvents()
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void Exit()
        {
            Environment.Exit(0);
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
            return new PlessClipboardHandler();
        }

        /// <inheritdoc/>
        public virtual IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            throw new NotImplementedException();
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
            return new PlessCaretHandler(control, width, height);
        }

        /// <inheritdoc/>
        public virtual IGraphicsFactoryHandler CreateGraphicsFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void CrtSetDbgFlag(int value)
        {
        }

        /// <inheritdoc/>
        public virtual bool IsPlatformControl(AbstractControl control)
        {
            return control is Control;
        }

        /// <inheritdoc/>
        public virtual PropertyUpdateResult SetAppearance(ApplicationAppearance appearance)
        {
            return PropertyUpdateResult.Failure;
        }

        /// <inheritdoc/>
        public virtual IMouseHandler CreateMouseHandler()
        {
            return new PlessMouseHandler();
        }

        /// <inheritdoc/>
        public virtual IKeyboardHandler CreateKeyboardHandler()
        {
            return new PlessKeyboardHandler();
        }

        /// <inheritdoc/>
        public virtual void WakeUpIdle()
        {
        }
    }
}
