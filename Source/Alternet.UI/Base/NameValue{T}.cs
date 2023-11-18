using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a named value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class NameValue<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValue{T}"/> class.
        /// </summary>
        /// <param name="name">A string that contains the name of the <paramref name="value"/>.</param>
        /// <param name="value">An object that contains the content that the
        /// <paramref name="name"/> identifies.</param>
        public NameValue(string name, T? value = default)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the value.
        /// </summary>
        /// <value>A <see cref="string"/> that contains the name of the value.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the content that the <see cref="Name"/> property value identifies.
        /// </summary>
        /// <value>A value of the <typeparamref name="T"/> type that contains the content.</value>
        public T? Value { get; set; }
    }
}
