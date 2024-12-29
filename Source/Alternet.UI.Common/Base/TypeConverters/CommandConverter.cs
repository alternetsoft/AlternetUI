using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string and the
    /// <see cref="ICommand"/>.
    /// </summary>
    public class CommandConverter : BaseTypeConverter
    {
        /// <summary>
        /// Occurs when <see cref="string"/> is converted to <see cref="ICommand"/>.
        /// </summary>
        public static event EventHandler<ValueConvertEventArgs<string?, ICommand?>>? StringToCommand;

        /// <summary>
        /// Parses string representation of the key modifiers.
        /// </summary>
        /// <param name="source">String value to parse.</param>
        /// <returns>Enumeration with key modifiers.</returns>
        public static ICommand? FromString(string? source)
        {
            if (StringToCommand is not null)
            {
                var e = new ValueConvertEventArgs<string?, ICommand?>(source);
                StringToCommand(null, e);
                if (e.Handled)
                    return e.Result ?? null;
            }

            return string.IsNullOrWhiteSpace(source) ? null : new NamedCommand(source);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            return false;
        }

        /// <inheritdoc/>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? source)
        {
            if (source is string v)
                return FromString(v);
            throw GetConvertFromException(source);
        }

        /// <inheritdoc/>
        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (value is null)
                return string.Empty;

            if (destinationType == typeof(string))
            {
                return string.Empty;
            }

            throw GetConvertToException(value, destinationType);
        }
    }
}
