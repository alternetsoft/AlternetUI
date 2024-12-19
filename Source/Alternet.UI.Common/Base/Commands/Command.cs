using System;

namespace Alternet.UI
{
    /// <summary>
    /// Allows an application author to define a method to be invoked.
    /// </summary>
    public class Command : BaseObject, ICommand
    {
        private readonly ExecuteDelegate? executeDelegate;
        private readonly CanExecuteDelegate? canExecuteDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the
        /// specified <paramref name="executeDelegate"/>.
        /// </summary>
        public Command(ExecuteDelegate executeDelegate)
        {
            this.executeDelegate = executeDelegate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the
        /// specified <paramref name="executeDelegate"/> and <paramref name="canExecuteDelegate"/>.
        /// </summary>
        public Command(ExecuteDelegate executeDelegate, CanExecuteDelegate? canExecuteDelegate)
        {
            this.executeDelegate = executeDelegate;
            this.canExecuteDelegate = canExecuteDelegate;
        }

        /// <summary>
        /// Defines a delegate signature for <see cref="Command.CanExecute"/> logic.
        /// </summary>
        public delegate bool CanExecuteDelegate(object? parameter);

        /// <summary>
        /// Defines a delegate signature for <see cref="Command.Execute"/> logic.
        /// </summary>
        public delegate void ExecuteDelegate(object? parameter);

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Occurs after <see cref="Execute"/> method is called.
        /// </summary>
        public event ExecuteDelegate? AfterExecute;

        /// <summary>
        /// Occurs before <see cref="Execute"/> method is called.
        /// </summary>
        public event ExecuteDelegate? BeforeExecute;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public virtual bool CanExecute(object? parameter)
        {
            if (canExecuteDelegate != null)
                return canExecuteDelegate(parameter);

            return true;
        }

        /// <inheritdoc/>
        public virtual void Execute(object? parameter)
        {
            BeforeExecute?.Invoke(parameter);
            executeDelegate?.Invoke(parameter);
            AfterExecute?.Invoke(parameter);
        }
    }
}