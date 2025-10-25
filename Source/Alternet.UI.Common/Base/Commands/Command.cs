using System;

namespace Alternet.UI
{
    /// <summary>
    /// Allows an application author to define a method to be invoked.
    /// </summary>
    public class Command : BaseObject, ICommand
    {
        private static WeakReferenceValue<object> currentTarget = new();

        private ExecuteDelegate? executeDelegate;
        private CanExecuteDelegate? canExecuteDelegate;
        private bool? canExecuteOverride;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the
        /// specified <paramref name="action"/>.
        /// </summary>
        public Command(Action action)
        {
            if(action is not null)
                this.executeDelegate = (parameter) => action();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the
        /// specified <paramref name="execute"/> and <paramref name="canExecute"/>.
        /// </summary>
        public Command(Action execute, Func<bool>? canExecute)
        {
            if(execute is not null)
                this.executeDelegate = (parameter) => execute();

            if(canExecute is not null)
                this.canExecuteDelegate = (parameter) => canExecute();
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
        /// Gets current target for the currently executed command.
        /// </summary>
        public static object? CurrentTarget
        {
            get
            {
                return currentTarget.Value;
            }

            set
            {
                currentTarget.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets execute action.
        /// </summary>
        public virtual ExecuteDelegate? ExecuteAction
        {
            get
            {
                return executeDelegate;
            }

            set
            {
                executeDelegate = value;
            }
        }

        /// <summary>
        /// Gets or sets 'CanExecute' function.
        /// </summary>
        public virtual CanExecuteDelegate? CanExecuteFunc
        {
            get
            {
                return canExecuteDelegate;
            }

            set
            {
                if (canExecuteDelegate == value)
                    return;
                canExecuteDelegate = value;
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets override for 'CanExecute'. If this property is not Null,
        /// it is used inside <see cref="CanExecute"/> method.
        /// </summary>
        public virtual bool? CanExecuteOverride
        {
            get
            {
                return canExecuteOverride;
            }

            set
            {
                if (canExecuteOverride == value)
                    return;
                canExecuteOverride = value;
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Notifies the command manager that the ability to execute the command may have changed.
        /// </summary>
        /// <remarks>This method raises the  <see cref="CanExecuteChanged"/> event.
        /// Call this method when the
        /// conditions that determine whether the command can execute have changed.</remarks>
        public void ChangeCanExecute()
        {
            RaiseCanExecuteChanged();
        }

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
            if (CanExecuteOverride is not null)
                return CanExecuteOverride.Value;

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