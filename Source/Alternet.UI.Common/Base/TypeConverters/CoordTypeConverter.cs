using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    internal class CoordTypeConverter : DoubleConverter
    {
        public static string NanString = "nan";

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if(value is Coord coord)
            {
                if (destinationType == typeof(string))
                {
                    if (Coord.IsNaN(coord))
                    {
                        return NanString;
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
