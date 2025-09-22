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
        private static Alternet.UI.Command? exitCommand;

        /// <summary>
        /// Gets the command to exit the application.
        /// </summary>
        /// <remarks>
        /// This command executes the <see cref="Alternet.UI.MauiUtils.CloseApplication"/> method
        /// to close the application. The command is always executable.
        /// </remarks>
        public static Alternet.UI.Command ExitCommand
        {
            get
            {
                exitCommand ??= new Alternet.UI.Command(
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

        /// <summary>
        /// Converts an <see cref="Alternet.UI.Command"/> to
        /// a <see cref="Microsoft.Maui.Controls.Command"/>.
        /// </summary>
        /// <remarks>The returned <see cref="Microsoft.Maui.Controls.Command"/> uses
        /// the <paramref name="command"/>'s  <see cref="Alternet.UI.Command.Execute"/> and
        /// <see cref="Alternet.UI.Command.CanExecute"/> delegates for execution and
        /// command state evaluation.</remarks>
        /// <param name="command">The <see cref="Alternet.UI.Command"/> instance to convert.
        /// Cannot be null.</param>
        /// <returns>A <see cref="Microsoft.Maui.Controls.Command"/> instance that
        /// wraps the provided <see cref="Alternet.UI.Command"/>.</returns>
        public static Command ToMaui(this Alternet.UI.Command command)
        {
            var result = new Command(command.Execute, command.CanExecute);
            return result;
        }

        /// <summary>
        /// Converts an <see cref="Alternet.UI.ICommand"/> to a <see cref="Command"/>
        /// for use in .NET MAUI applications.
        /// </summary>
        /// <param name="command">The <see cref="Alternet.UI.ICommand"/> instance to convert.
        /// Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="Command"/> instance that wraps the specified
        /// <see cref="Alternet.UI.ICommand"/>.</returns>
        public static Command ToMaui(this Alternet.UI.ICommand command)
        {
            var result = new Command(command.Execute, command.CanExecute);
            return result;
        }
    }
}