using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains different methods which convert object to string.
    /// </summary>
    public interface IObjectToString
    {
        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/>
        /// representation.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <returns>The <see cref="string"/> representation of the
        /// <paramref name="value"/>.</returns>
        string? ToString(object value);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/>
        /// representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/> as
        /// specified by <paramref name="provider"/>.</returns>
        string? ToString(object value, IFormatProvider? provider);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/>
        /// representation,
        /// using the specified format.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="format">A standard or custom format string.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/> as
        /// specified by <paramref name="format"/>.</returns>
        string? ToString(object value, string? format);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/>
        /// representation using
        /// the specified format and culture-specific format information.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="format">A standard or custom format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the <paramref name="value"/>
        /// as specified by <paramref name="format"/> and <paramref name="provider"/>.</returns>
        string? ToString(object value, string? format, IFormatProvider? provider);

        /// <summary>
        /// Converts the <paramref name="value"/> to its equivalent <see cref="string"/>
        /// representation, using the specified context and culture information.
        /// </summary>
        /// <param name="value">A value for convertion.</param>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that
        /// provides a format context.</param>
        /// <param name="culture"><see cref="CultureInfo"/> used for the conversion.
        /// If null is passed, the current culture is assumed.</param>
        /// <param name="useInvariantConversion"></param>
        /// <returns></returns>
        string? ToString(
            object value,
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            bool useInvariantConversion);

        /// <summary>
        /// Conversion arguments.
        /// </summary>
        public struct Arguments
        {
            /// <summary>
            /// Gets or sets <see cref="CultureInfo"/> used for the conversion.
            /// </summary>
            public CultureInfo? Culture;

            /// <summary>
            /// Gets or sets <see cref="ITypeDescriptorContext"/> used for the conversion.
            /// </summary>
            public ITypeDescriptorContext? Context;

            /// <summary>
            /// Gets or sets whether to use invariant conversion.
            /// </summary>
            public bool UseInvariantConversion;

            /// <summary>
            /// Gets or sets a standard or custom format string.
            /// </summary>
            public string? Format;

            /// <summary>
            /// Gets or sets an object that supplies culture-specific
            /// formatting information.
            /// </summary>
            public IFormatProvider? FormatProvider;
        }
    }
}
