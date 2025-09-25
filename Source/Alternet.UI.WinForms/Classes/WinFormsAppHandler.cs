using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI.WinForms
{
    public class WinFormsAppHandler : DisposableObject, IApplicationHandler
    {
        public bool ExitOnFrameDelete { get; set; }
        public bool IsActive { get; }
        public bool InUixmlPreviewerMode { get; set; }
        public bool IsInvokeRequired { get; }

        public void BeginInvoke(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            var context = System.Threading.SynchronizationContext.Current;
            if (context is not null)
            {
                context.Post(static s => ((Action)s!)(), action);
            }
            else
            {
                System.Threading.ThreadPool.QueueUserWorkItem(static s => ((Action)s!)());
            }
        }

        public IActionSimulatorHandler CreateActionSimulatorHandler()
        {
            throw new NotImplementedException();
        }

        public ICaretHandler CreateCaretHandler(AbstractControl control, int width, int height)
        {
            throw new NotImplementedException();
        }

        public IClipboardHandler CreateClipboardHandler()
        {
            throw new NotImplementedException();
        }

        public IControlFactoryHandler CreateControlFactoryHandler()
        {
            throw new NotImplementedException();
        }

        public IControlPainterHandler CreateControlPainterHandler()
        {
            throw new NotImplementedException();
        }

        public IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            throw new NotImplementedException();
        }

        public IGraphicsFactoryHandler CreateGraphicsFactoryHandler()
        {
            return new WinFormsGraphicsFactoryHandler();
        }

        public IKeyboardHandler CreateKeyboardHandler()
        {
            throw new NotImplementedException();
        }

        public IMemoryHandler CreateMemoryHandler()
        {
            throw new NotImplementedException();
        }

        public IMenuFactory? CreateMenuFactory()
        {
            throw new NotImplementedException();
        }

        public IMouseHandler CreateMouseHandler()
        {
            throw new NotImplementedException();
        }

        public INotifyIconHandler CreateNotifyIconHandler()
        {
            throw new NotImplementedException();
        }

        public IPrintingHandler CreatePrintingHandler()
        {
            throw new NotImplementedException();
        }

        public ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            throw new NotImplementedException();
        }

        public ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new WinFormsSystemSettingsHandler();
        }

        public ITimerHandler CreateTimerHandler(Timer timer)
        {
            throw new NotImplementedException();
        }

        public IToolTipFactoryHandler CreateToolTipFactoryHandler()
        {
            throw new NotImplementedException();
        }

        public void CrtSetDbgFlag(int value)
        {
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void ExitMainLoop()
        {
            throw new NotImplementedException();
        }

        public Window? GetActiveWindow()
        {
            throw new NotImplementedException();
        }

        public object? GetAttributeValue(string name)
        {
            throw new NotImplementedException();
        }

        public AbstractControl? GetFocusedControl()
        {
            throw new NotImplementedException();
        }

        public bool HasPendingEvents()
        {
            throw new NotImplementedException();
        }

        public bool IsPlatformControl(AbstractControl control)
        {
            throw new NotImplementedException();
        }

        public void ProcessPendingEvents()
        {
            throw new NotImplementedException();
        }

        public void Run(Window window)
        {
            throw new NotImplementedException();
        }

        public PropertyUpdateResult SetAppearance(ApplicationAppearance appearance)
        {
            throw new NotImplementedException();
        }

        public void SetTopWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void WakeUpIdle()
        {
            throw new NotImplementedException();
        }
    }
}
