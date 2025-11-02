using System;
using System.ComponentModel;

namespace Alternet.UI.Port
{
    /// <summary>
    /// <see cref="TypeConverter"/> descendant for converting <see cref="System.Type"/>.
    /// </summary>
    internal class TypeTypeConverter : BaseTypeConverter
    {
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object value)
        {
            if (context != null && value is string typeName)
            {
                IXamlTypeResolver? xamlTypeResolver
                    = (IXamlTypeResolver?)context.GetService(typeof(IXamlTypeResolver));

                if (xamlTypeResolver != null)
                {
                    return xamlTypeResolver.Resolve(typeName);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
