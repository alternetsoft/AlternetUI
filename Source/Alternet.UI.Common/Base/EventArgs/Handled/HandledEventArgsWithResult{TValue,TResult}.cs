using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="HandledEventArgs{T}"/> with the <see cref="Result"/> property.
    /// </summary>
    /// <typeparam name="TValue">Type of value property.</typeparam>
    /// <typeparam name="TResult">Type of result property.</typeparam>
    public class HandledEventArgsWithResult<TValue, TResult> : HandledEventArgs<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value"></param>
        public HandledEventArgsWithResult(TValue value)
            : base(value)
        {
        }

        /// <summary>
        /// Gets or sets result value.
        /// </summary>
        public TResult? Result { get; set; }
    }
}
