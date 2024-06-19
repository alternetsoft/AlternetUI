using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    public interface IApplicationHandler : IDisposable
    {
        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        bool ExitOnFrameDelete { get; set; }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        bool IsActive { get; }

        bool InUixmlPreviewerMode { get; set; }

        bool InvokeRequired { get; }

        IMouseHandler CreateMouseHandler();

        IKeyboardHandler CreateKeyboardHandler();

        /// <summary>
        /// Gets attribute value. Returns <c>null</c> if there is no such attribute.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        object? GetAttributeValue(string name);

        Window? GetActiveWindow();

        Control? GetFocusedControl();

        void Run(Window window);

        void SetTopWindow(Window window);

        void WakeUpIdle();

        void BeginInvoke(Action action);

        bool HasPendingEvents();

        void NotifyCaptureLost();

        void ExitMainLoop();

        void Exit();

        IControlFactoryHandler CreateControlFactoryHandler();

        IToolTipFactoryHandler CreateToolTipFactoryHandler();

        INotifyIconHandler CreateNotifyIconHandler();

        IDialogFactoryHandler CreateDialogFactoryHandler();

        IClipboardHandler CreateClipboardHandler();

        IControlPainterHandler CreateControlPainterHandler();

        IMemoryHandler CreateMemoryHandler();

        ITimerHandler CreateTimerHandler(Timer timer);

        ISoundFactoryHandler CreateSoundFactoryHandler();

        ISystemSettingsHandler CreateSystemSettingsHandler();

        void ProcessPendingEvents();

        IGraphicsFactoryHandler CreateGraphicsFactoryHandler();

        IPrintingHandler CreatePrintingHandler();

        /*ICaretHandler CreateCaretHandler();*/

        ICaretHandler CreateCaretHandler(Control control, int width, int height);

        void CrtSetDbgFlag(int value);
    }
}
