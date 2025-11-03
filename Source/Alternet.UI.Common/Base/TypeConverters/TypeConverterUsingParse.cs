using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    internal class TypeConverterUsingParse : BaseTypeConverter
    {
        private readonly MethodInfo method;

        public TypeConverterUsingParse(MethodInfo method)
        {
            this.method = method;
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value)
        {
            if(value is string str)
            {
                var result = method.Invoke(null, [str]);
                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType == typeof(string) && value is not null)
                return value.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
