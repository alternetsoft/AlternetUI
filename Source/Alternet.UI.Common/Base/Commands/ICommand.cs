using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// An interface that allows an application author to define a method to be invoked.
    /// </summary>
    [TypeConverter(typeof(CommandConverter))]
    public interface ICommand
    {
        /// <summary>
        /// Raised when the ability of the command to execute has changed.
        /// </summary>
        event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Returns whether the command can be executed.
        /// </summary>
        /// <param name="parameter">A parameter that may be used in executing the command.
        /// This parameter may be ignored by some implementations.</param>
        /// <returns>True if the command can be executed with the given parameter and current
        /// state; False otherwise.</returns>
        bool CanExecute(object? parameter = null);

        /// <summary>
        /// Defines the method that should be called when the command is executed.
        /// </summary>
        /// <param name="parameter">A parameter that may be used when the command is executed.
        /// This parameter may be ignored by some implementations.</param>
        void Execute(object? parameter = null);
    }
}