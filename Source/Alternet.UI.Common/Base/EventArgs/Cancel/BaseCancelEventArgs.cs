using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI <see cref="CancelEventArgs"/>
    /// descendants.
    /// </summary>
    public class BaseCancelEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs"/> class.
        /// </summary>
        public BaseCancelEventArgs()
                : base(cancel: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs"/> class.
        /// </summary>
        /// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
        public BaseCancelEventArgs(bool cancel)
                : base(cancel)
        {
        }
    }

    /// <summary>
    /// Extends <see cref="BaseCancelEventArgs"/> with parameter of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Value"/> property.</typeparam>
    public class BaseCancelEventArgs<T> : BaseCancelEventArgs
    {
        private T val;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs{T}"/> class.
        /// </summary>
        public BaseCancelEventArgs()
            : this(default!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value to assign to the parameter.</param>
        public BaseCancelEventArgs(T value)
        {
            this.val = value;
        }

        /// <summary>
        /// Gets parameter value.
        /// </summary>
        public virtual T Value
        {
            get => val;
            set => this.val = value;
        }
    }
}