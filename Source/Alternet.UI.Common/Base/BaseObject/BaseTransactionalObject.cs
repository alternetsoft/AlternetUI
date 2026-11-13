using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides support for nested transactions.
    /// </summary>
    /// <remarks>
    /// Transactions may be nested. The <see cref="TransactionStarted"/> event
    /// is raised when the first transaction is started, and the
    /// <see cref="TransactionEnded"/> event is raised when the last transaction
    /// is ended.
    /// </remarks>
    public class BaseTransactionalObject : BaseObjectWithNotify
    {
        private int transactionLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTransactionalObject"/>
        /// class with the specified unique id.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the new instance.</param>
        public BaseTransactionalObject(ObjectUniqueId uniqueId)
            : base(uniqueId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTransactionalObject"/> class.
        /// </summary>
        public BaseTransactionalObject()
        {
        }

        /// <summary>
        /// Occurs when the first transaction is started.
        /// </summary>
        public event EventHandler? TransactionStarted;

        /// <summary>
        /// Occurs when the last active transaction is ended.
        /// </summary>
        public event EventHandler? TransactionEnded;

        /// <summary>
        /// Gets a value indicating whether a transaction is currently active.
        /// </summary>
        public bool IsTransactionActive => transactionLevel > 0;

        /// <summary>
        /// Gets the current transaction nesting level.
        /// </summary>
        public int TransactionLevel => transactionLevel;

        /// <summary>
        /// Executes the specified action inside a transaction.
        /// Before executing the action, a transaction is started, and after the action is executed,
        /// the transaction is ended.
        /// </summary>
        /// <param name="action">The action to execute inside the transaction.</param>
        public void DoInsideTransaction(Action action)
        {
            StartTransaction();
            try
            {
                action();
            }
            finally
            {
                EndTransaction();
            }
        }

        /// <summary>
        /// Executes the specified function inside a transaction and returns its result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="func">The function to execute inside the transaction.</param>
        /// <returns>The result of the function.</returns>
        public TResult DoInsideTransaction<TResult>(Func<TResult> func)
        {
            StartTransaction();
            try
            {
                return func();
            }
            finally
            {
                EndTransaction();
            }
        }

        /// <summary>
        /// Starts a transaction.
        /// </summary>
        /// <remarks>
        /// Transactions may be nested. The <see cref="TransactionStarted"/>
        /// event is raised only when entering the outermost transaction.
        /// </remarks>
        public virtual void StartTransaction()
        {
            if (transactionLevel == 0)
            {
                OnTransactionStarted();
            }

            transactionLevel++;
        }

        /// <summary>
        /// Ends a transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// No transaction is currently active.
        /// </exception>
        /// <remarks>
        /// The <see cref="TransactionEnded"/> event is raised only when leaving
        /// the outermost transaction.
        /// </remarks>
        public virtual void EndTransaction()
        {
            if (transactionLevel <= 0)
            {
                throw new InvalidOperationException("No active transaction.");
            }

            transactionLevel--;

            if (transactionLevel == 0)
            {
                OnTransactionEnded();
            }
        }

        /// <inheritdoc/>
        protected override void SetFieldValue<T>(ref T storage, T value, string propertyName)
        {
            if (transactionLevel == 0)
            {
                try
                {
                    StartTransaction();
                    base.SetFieldValue(ref storage, value, propertyName);
                }
                finally
                {
                    EndTransaction();
                }
            }
            else
            {
                base.SetFieldValue(ref storage, value, propertyName);
            }
        }

        /// <summary>
        /// Raises the <see cref="TransactionStarted"/> event.
        /// </summary>
        protected virtual void OnTransactionStarted()
        {
            if (TransactionStarted is not null)
            {
                TransactionStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="TransactionEnded"/> event.
        /// </summary>
        protected virtual void OnTransactionEnded()
        {
            if (TransactionEnded is not null)
            {
                TransactionEnded(this, EventArgs.Empty);
            }
        }
    }
}
