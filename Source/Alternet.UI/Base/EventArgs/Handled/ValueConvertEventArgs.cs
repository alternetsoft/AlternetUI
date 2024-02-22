using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the value convertion events.
    /// </summary>
    public class ValueConvertEventArgs<T1, T2> : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConvertEventArgs{T1, T2}"/> class.
        /// </summary>
        /// <param name="value"></param>
        public ValueConvertEventArgs(T1 value)
        {
            Value = value;
        }

        /// <summary>
        /// Value to convert from.
        /// </summary>
        public T1 Value { get; set; }

        /// <summary>
        /// Result of the conversion.
        /// </summary>
        public T2? Result { get; set; }
    }
}
