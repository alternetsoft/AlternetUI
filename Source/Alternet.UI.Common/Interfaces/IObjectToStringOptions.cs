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
    /// Defines properties which allow to specify value to string convertion options.
    /// </summary>
    public interface IObjectToStringOptions
    {
        /// <summary>
        /// Gets or sets <see cref="CultureInfo"/> used for the conversion.
        /// </summary>
        CultureInfo? Culture { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ITypeDescriptorContext"/> used for the conversion.
        /// </summary>
        ITypeDescriptorContext? Context { get; set; }

        /// <summary>
        /// Gets or sets whether to use invariant conversion.
        /// </summary>
        bool? UseInvariantCulture { get; set; }

        /// <summary>
        /// Gets or sets a bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of the text.
        /// </summary>
        NumberStyles? NumberStyles { get; set; }

        /// <summary>
        /// Gets or sets an object that supplies culture-specific formatting information.
        /// </summary>
        IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IObjectToString"/> provider which is used in
        /// value to string convertion.
        /// </summary>
        IObjectToString? Converter { get; set; }

        /// <summary>
        /// Gets or sets default format used in value to string convertion.
        /// </summary>
        string? DefaultFormat { get; set; }
    }
}
