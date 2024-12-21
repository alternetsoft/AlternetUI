using System;
using System.ComponentModel;

namespace Alternet.UI.Port
{
    /// <summary>
    /// TypeConverter for System.Type.
    /// </summary>
    internal class TypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value)
        {
            if (context != null && value is string typeName)
            {
                IXamlTypeResolver xamlTypeResolver
                    = (IXamlTypeResolver)context.GetService(typeof(IXamlTypeResolver));

                if (xamlTypeResolver != null)
                {
                    return xamlTypeResolver.Resolve(typeName);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
