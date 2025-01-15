using System;
using System.Windows.Input;

namespace Alternet.UI
{
    /// <summary>
    /// An interface for classes that know how to invoke a <see cref="ICommand"/>.
    /// </summary>
    public interface ICommandSource
    {
        /// <summary>
        /// The command that will be executed when the class is "invoked".
        /// Classes that implement this interface should enable or disable based on the
        /// command's <see cref="ICommand.CanExecute"/> return value.
        /// </summary>
        ICommand? Command
        {
            get;
        }

        /// <summary>
        /// Gets the parameter that will be passed to the command when executing it.
        /// </summary>
        object? CommandParameter
        {
            get;
        }

        /// <summary>
        /// An element that an implementor may wish to target as the destination
        /// for the command.
        /// The property may be implemented as read-write if desired.
        /// </summary>
        object? CommandTarget
        {
            get;
        }
    }
}
