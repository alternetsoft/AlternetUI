using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all <see cref="TypeConverter"/> descendants in the library.
    /// </summary>
    public class BaseTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        internal static Uri? GetContextBaseUri(IServiceProvider? ctx)
            => GetService<UI.Port.IUixmlUriContext>(ctx)?.BaseUri;

        internal static T? GetService<T>(IServiceProvider? sp) => (T?)sp?.GetService(typeof(T));
    }
}
