using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the value convertion events. <typeparamref name="TFrom"/> specifies
    /// type of the source data. <typeparamref name="TResult"/> specifies
    /// type of the destination data.
    /// </summary>
    public class ValueConvertEventArgs<TFrom, TResult> : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ValueConvertEventArgs{TFrom, TResult}"/> class.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ValueConvertEventArgs(TFrom value)
        {
            Value = value;
        }

        /// <summary>
        /// Value to convert from.
        /// </summary>
        public TFrom Value { get; set; }

        /// <summary>
        /// Result of the conversion.
        /// </summary>
        public TResult? Result { get; set; }
    }
}
