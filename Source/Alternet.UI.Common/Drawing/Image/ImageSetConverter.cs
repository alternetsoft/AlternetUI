using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Alternet.UI;
using Alternet.UI.Markup;

namespace Alternet.Drawing
{
    /// <summary>Converts brushes from one data type to another. Access this
    /// class through the
    /// <see cref="System.ComponentModel.TypeDescriptor" />.</summary>
    public class ImageSetConverter : TypeConverter
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
            var uri = s.StartsWith("/")
                ? new Uri(s, UriKind.Relative)
                : new Uri(s, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                using var stream = File.OpenRead(uri.LocalPath);
                return new ImageSet(stream);
            }

            var assets = new UI.ResourceLoader();
            return new ImageSet(assets.Open(uri, ImageConverter.GetContextBaseUri(context)));
        }
    }
}