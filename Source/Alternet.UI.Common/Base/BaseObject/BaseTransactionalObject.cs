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

        /// <summary>
        /// Raises the <see cref="TransactionStarted"/> event.
        /// </summary>
        protected virtual void OnTransactionStarted()
        {
            TransactionStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="TransactionEnded"/> event.
        /// </summary>
        protected virtual void OnTransactionEnded()
        {
            TransactionEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
