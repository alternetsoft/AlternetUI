using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains different methods which convert value to string.
    /// </summary>
    public interface IValueToString
    {
        /// <summary>
        /// Converts the value to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of the value.</returns>
        string ToString();

        /// <summary>
        /// Converts the value to its equivalent <see cref="string"/> representation,
        /// using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom format string.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="format"/>.</returns>
        string ToString(string format);

        /// <summary>
        /// Converts the value to its equivalent <see cref="string"/> representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="provider"/>.</returns>
        string ToString(IFormatProvider provider);

        /// <summary>
        /// Converts the value to its equivalent <see cref="string"/>
        /// representation using
        /// the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="format"/> and <paramref name="provider"/>.</returns>
        string ToString(string format, IFormatProvider provider);
    }
}
