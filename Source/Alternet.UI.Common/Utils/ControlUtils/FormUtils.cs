using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to forms handling
    /// </summary>
    public static class FormUtils
    {
        /// <summary>
        /// Gets name of the 'CloseOtherWindows' command.
        /// </summary>
        public const string CommandNameCloseOtherWindows = "App.CloseOtherWindows";

        static FormUtils()
        {
            NamedCommands.Default.Register(
                CommandNameCloseOtherWindows,
                (prm) => CloseOtherWindows(null, prm as Window));
        }

        /// <summary>
        /// Executes command which closes all windows.
        /// </summary>
        public static void ExecuteCommandCloseOtherWindows(Window? exceptWindow = null)
        {
            NamedCommands.Default.Execute(CommandNameCloseOtherWindows, exceptWindow);
        }

        /// <summary>
        /// Closes all windows using the specified close action. If <paramref name="action"/>
        /// is not specified, windows are disposed.
        /// </summary>
        public static void CloseOtherWindows(
            WindowCloseAction? action = WindowCloseAction.Dispose,
            Window? exceptWindow = null)
        {
            action ??= WindowCloseAction.Dispose;

            var windows = App.Current.Windows.ToArray();

            foreach(var window in windows)
            {
                if (window == exceptWindow)
                    continue;
                window.Close(action);
                App.DoEvents();
            }
        }
    }
}
