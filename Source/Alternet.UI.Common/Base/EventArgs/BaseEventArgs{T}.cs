namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseEventArgs"/> with parameter of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Value"/> property.</typeparam>
    public class BaseEventArgs<T> : BaseEventArgs
    {
        private T val;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs{T}"/> class.
        /// </summary>
        public BaseEventArgs()
            : this(default!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value to assign to the parameter.</param>
        public BaseEventArgs(T value)
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
