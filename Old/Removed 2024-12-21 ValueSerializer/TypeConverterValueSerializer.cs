using System.ComponentModel;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Uses a TypeConverter to implement the translation
    /// to and from a string. The caller of the constructor must ensure the TypeConverter supports
    /// converstion to and from string.
    /// </summary>
    internal sealed class TypeConverterValueSerializer : ValueSerializer
    {
        private readonly TypeConverter converter;

        public TypeConverterValueSerializer(TypeConverter converter)
        {
            this.converter = converter;
        }

        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
            return converter.CanConvertTo(context, typeof(string));
        }

        public override string ConvertToString(object value, IValueSerializerContext context)
        {
            return converter.ConvertToString(context, TypeConverterHelper.InvariantEnglishUS, value);
        }

        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        public override object ConvertFromString(string value, IValueSerializerContext context)
        {
            return converter.ConvertFrom(context, TypeConverterHelper.InvariantEnglishUS, value);
        }
    }
}
