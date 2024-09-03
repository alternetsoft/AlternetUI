using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Base attribute which has some value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class BaseValueAttribute<T> : BaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseValueAttribute{T}"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public BaseValueAttribute(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public virtual T Value { get; set; }
    }
}
