using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains helper properties and methods for the value conversion.
    /// </summary>
    public static class ConversionUtils
    {
        /// <summary>
        /// Calls the specified conversion action.
        /// </summary>
        /// <typeparam name="TConvertFrom">Type of the value to convert from.</typeparam>
        /// <typeparam name="TConvertTo">Type of the value to convert to.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="convert">Conversion action.</param>
        /// <param name="result">Result of the conversion.</param>
        /// <returns>True if value was converted using the conversion event;
        /// False otherwise.</returns>
        public static bool ConvertWithAction<TConvertFrom, TConvertTo>(
            TConvertFrom? value,
            Action<ValueConvertEventArgs<TConvertFrom?, TConvertTo?>>? convert,
            out TConvertTo? result)
        {
            if (convert is not null)
            {
                var e = new ValueConvertEventArgs<TConvertFrom?, TConvertTo?>(value);
                convert(e);
                if (e.Handled)
                {
                    result = e.Result;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
