using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides a collection of reusable commands for .NET MAUI applications.
    /// </summary>
    public static class MauiCommands
    {
        private static Command? exitCommand;

        /// <summary>
        /// Gets the command to exit the application.
        /// </summary>
        /// <remarks>
        /// This command executes the <see cref="Alternet.UI.MauiUtils.CloseApplication"/> method
        /// to close the application. The command is always executable.
        /// </remarks>
        public static Command ExitCommand
        {
            get
            {
                exitCommand ??= new Command(
                    execute: () =>
                    {
                        Alternet.UI.MauiUtils.CloseApplication();
                    },
                    canExecute: () =>
                    {
                        return true;
                    });
                return exitCommand;
            }
        }
    }
}