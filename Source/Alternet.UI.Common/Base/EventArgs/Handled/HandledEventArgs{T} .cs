namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseEventArgs"/> with parameter of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Value"/> property.</typeparam>
    public partial class HandledEventArgs<T> : HandledEventArgs
    {
        private T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value"></param>
        public HandledEventArgs(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets parameter value.
        /// </summary>
        public T Value
        {
            get => this.value;
            set => this.value = value;
        }
    }
}
