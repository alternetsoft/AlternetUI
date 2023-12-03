﻿namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseEventArgs"/> with parameter of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Value"/> property.</typeparam>
    public class BaseEventArgs<T> : BaseEventArgs
    {
        private T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value"></param>
        public BaseEventArgs(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets parameter value.
        /// </summary>
        public T Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
