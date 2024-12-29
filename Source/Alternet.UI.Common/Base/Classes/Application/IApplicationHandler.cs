﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to control platfrom behavior.
    /// </summary>
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

        /// <summary>
        /// Gets whether application is in Uixml previewer mode.
        /// </summary>
        bool InUixmlPreviewerMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the caller must call an invoke method when making method
        /// calls to the UI objects because the caller is not on the UI thread.
        /// </summary>
        bool IsInvokeRequired { get; }

        /// <summary>
        /// Creates <see cref="IActionSimulatorHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IActionSimulatorHandler CreateActionSimulatorHandler();

        /// <summary>
        /// Creates <see cref="IMouseHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IMouseHandler CreateMouseHandler();

        /// <summary>
        /// Creates <see cref="IKeyboardHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IKeyboardHandler CreateKeyboardHandler();

        /// <summary>
        /// Gets attribute value. Returns <c>null</c> if there is no such attribute.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        object? GetAttributeValue(string name);

        /// <summary>
        /// Gets currently active window.
        /// </summary>
        /// <returns></returns>
        Window? GetActiveWindow();

        /// <summary>
        /// Gets currently focused control.
        /// </summary>
        /// <returns></returns>
        AbstractControl? GetFocusedControl();

        /// <inheritdoc cref="App.Run"/>
        void Run(Window window);

        /// <inheritdoc cref="App.SetTopWindow(Window)"/>
        void SetTopWindow(Window window);

        /// <summary>
        /// Executes an action asynchronously on the UI thread.
        /// </summary>
        void BeginInvoke(Action action);

        /// <inheritdoc cref="App.HasPendingEvents"/>
        bool HasPendingEvents();

        /// <summary>
        /// Exits application's main loop.
        /// </summary>
        void ExitMainLoop();

        /// <inheritdoc cref="App.Exit"/>
        void Exit();

        /// <inheritdoc cref="App.WakeUpIdle"/>
        void WakeUpIdle();

        /// <summary>
        /// Creates <see cref="IControlFactoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IControlFactoryHandler CreateControlFactoryHandler();

        /// <summary>
        /// Creates <see cref="IToolTipFactoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IToolTipFactoryHandler CreateToolTipFactoryHandler();

        /// <summary>
        /// Creates <see cref="INotifyIconHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        INotifyIconHandler CreateNotifyIconHandler();

        /// <summary>
        /// Creates <see cref="IDialogFactoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IDialogFactoryHandler CreateDialogFactoryHandler();

        /// <summary>
        /// Creates <see cref="IClipboardHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IClipboardHandler CreateClipboardHandler();

        /// <summary>
        /// Creates <see cref="IControlPainterHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IControlPainterHandler CreateControlPainterHandler();

        /// <summary>
        /// Creates <see cref="IMemoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IMemoryHandler CreateMemoryHandler();

        /// <summary>
        /// Creates <see cref="ITimerHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        ITimerHandler CreateTimerHandler(Timer timer);

        /// <summary>
        /// Creates <see cref="ISoundFactoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        ISoundFactoryHandler CreateSoundFactoryHandler();

        /// <summary>
        /// Creates <see cref="ISystemSettingsHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        ISystemSettingsHandler CreateSystemSettingsHandler();

        /// <inheritdoc cref="App.ProcessPendingEvents"/>
        void ProcessPendingEvents();

        /// <summary>
        /// Creates <see cref="IGraphicsFactoryHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IGraphicsFactoryHandler CreateGraphicsFactoryHandler();

        /// <summary>
        /// Creates <see cref="IPrintingHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        IPrintingHandler CreatePrintingHandler();

        /// <summary>
        /// Creates <see cref="ICaretHandler"/> implementation specific to the platform.
        /// </summary>
        /// <returns></returns>
        ICaretHandler CreateCaretHandler(AbstractControl control, int width, int height);

        /// <summary>
        /// Sets log debug flag of the used C++ libraries.
        /// </summary>
        /// <param name="value">Value.</param>
        void CrtSetDbgFlag(int value);
    }
}
