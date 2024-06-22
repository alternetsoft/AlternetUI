using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

using Alternet.UI;
using Alternet.UI.Markup;

namespace Alternet.Drawing
{
    /// <summary>
    /// Converts <see cref="Image"/> from one data type to another.
    /// Access this class through the
    /// <see cref="System.ComponentModel.TypeDescriptor" />.</summary>
    public class ImageConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? context,
            Type? sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <inheritdoc/>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value)
        {
            var s = (string?)value;
            if (s == null)
                return null;

            return new Bitmap(s, ImageConverter.GetContextBaseUri(context));
        }

        internal static Uri? GetContextBaseUri(IServiceProvider? ctx)
            => GetService<UI.Port.IUixmlUriContext>(ctx)?.BaseUri;

        internal static T? GetService<T>(IServiceProvider? sp) => (T?)sp?.GetService(typeof(T));
    }
}