using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains different ToString methods.
    /// </summary>
    public interface IObjectToString
    {
        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/>.</returns>
        string? ToString(object value);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/> representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/> as
        /// specified by <paramref name="provider"/>.</returns>
        string? ToString(object value, IFormatProvider? provider);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/> representation,
        /// using the specified format.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="format">A standard or custom format string.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/> as
        /// specified by <paramref name="format"/>.</returns>
        string? ToString(object value, string? format);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/> representation using
        /// the specified format and culture-specific format information.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="format">A standard or custom format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/>
        /// as specified by <paramref name="format"/> and <paramref name="provider"/>.</returns>
        string? ToString(object value, string? format, IFormatProvider? provider);
    }
}
