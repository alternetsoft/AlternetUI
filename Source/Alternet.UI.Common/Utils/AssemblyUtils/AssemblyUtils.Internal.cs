using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    public static partial class AssemblyUtils
    {
        internal static string? GetTypeConverterAttributeData(Type? type, out Type? converterType)
        {
            bool foundTC = false;
            return GetCustomAttributeData(
                type,
                typeof(TypeConverterAttribute),
                true,
                ref foundTC,
                out converterType);
        }

        internal static string GetTypeConverterAttributeData(MemberInfo mi, out Type? converterType)
        {
            return GetCustomAttributeData(
                mi,
                typeof(TypeConverterAttribute),
                out converterType);
        }

        // Special version of type-based GetCustomAttributeData that does two
        //  additional tasks:
        //  1) Retrieves the attributes even if it's defined on a base type, and
        //  2) Distinguishes between "attribute found and said null" and
        //     "no attribute found at all" via the ref bool.
        internal static string? GetCustomAttributeData(
            Type? t,
            Type attrType,
            bool allowTypeAlso,
            ref bool attributeDataFound,
            out Type? typeValue)
        {
            typeValue = null;
            attributeDataFound = false;
            Type? currentType = t;
            string? attributeDataString = null;
            CustomAttributeData cad;

            while (currentType != null && !attributeDataFound)
            {
                IList<CustomAttributeData> list = CustomAttributeData.GetCustomAttributes(currentType);

                for (int j = 0; j < list.Count && !attributeDataFound; j++)
                {
                    cad = list[j];

                    if (cad.Constructor.ReflectedType == attrType)
                    {
                        attributeDataFound = true;
                        attributeDataString = GetCustomAttributeData(
                            cad,
                            attrType,
                            out typeValue,
                            allowTypeAlso,
                            false,
                            false);
                    }
                }

                if (!attributeDataFound)
                {
                    currentType = currentType.BaseType;

                    // object.BaseType is null, used as terminating condition for the while() loop.
                }
            }

            return attributeDataString;
        }
    }
}
