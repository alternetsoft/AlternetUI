using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    public static partial class AssemblyUtils
    {
        // Helper that inspects a specific CustomAttributeData obtained via ReflectionOnlyLoad, and
        // returns its value if the Type of the attribiutes matches the passed in attrType. It only
        // looks for attributes with no values or a single value of Type string that is passed in via
        // a ctor. If allowTypeAlso is true, then it looks for values of typeof(Type) as well in the
        // single value case. If noArgs == false and zeroArgsAllowed = true, that means 0 or 1 args
        // are permissible.
        private static string? GetCustomAttributeData(
            CustomAttributeData cad,
            Type attrType,
            out Type? typeValue,
            bool allowTypeAlso,
            bool noArgs,
            bool zeroArgsAllowed)
        {
            string? attrValue = null;
            typeValue = null;

            // get the Constructor info
            ConstructorInfo cinfo = cad.Constructor;
            if (cinfo.ReflectedType == attrType)
            {
                // typedConstructorArguments (the Attribute constructor arguments)
                // [MyAttribute("test", Name=Hello)]
                // "test" is the Constructor Argument
                IList<CustomAttributeTypedArgument> constructorArguments = cad.ConstructorArguments;
                if (constructorArguments.Count == 1 && !noArgs)
                {
                    CustomAttributeTypedArgument tca = constructorArguments[0];
                    attrValue = tca.Value as string;
                    if (attrValue == null && allowTypeAlso && tca.ArgumentType == typeof(Type))
                    {
                        typeValue = tca.Value as Type;
                        attrValue = typeValue?.AssemblyQualifiedName;
                    }

                    if (attrValue == null)
                    {
                        throw new ArgumentException(SR.Get(SRID.ParserAttributeArgsLow, attrType.Name));
                    }
                }
                else if (constructorArguments.Count == 0)
                {
                    // zeroArgsAllowed = true for CPA for example.
                    // CPA with no args is valid and would mean that this type is overriding a base CPA
                    if (noArgs || zeroArgsAllowed)
                    {
                        attrValue = string.Empty;
                    }
                    else
                    {
                        throw new ArgumentException(SR.Get(SRID.ParserAttributeArgsLow, attrType.Name));
                    }
                }
                else
                {
                    throw new ArgumentException(SR.Get(SRID.ParserAttributeArgsHigh, attrType.Name));
                }
            }

            return attrValue;
        }

        // Given a ReflectionOnlyLoaded member, returns the value of a metadata attribute of
        // Type attrType if set on that member. Looks only for attributes that have a ctor with
        // one parameter that is of Type string or Type.
        private static string GetCustomAttributeData(MemberInfo mi, Type attrType, out Type? typeValue)
        {
            IList<CustomAttributeData> list = CustomAttributeData.GetCustomAttributes(mi);
            string? attrValue = GetCustomAttributeData(list, attrType, out typeValue, true, false);
            return attrValue ?? string.Empty;
        }

        // Helper that enumerates a list of CustomAttributeData obtained
        // via ReflectionOnlyLoad, and
        // looks for a specific attribute of Type attrType.
        // It only looks for attribiutes with a single
        // value of Type string that is passed in via a ctor.
        // If allowTypeAlso is true, then it looks for
        // values of typeof(Type) as well.
        private static string? GetCustomAttributeData(
            IList<CustomAttributeData> list,
            Type attrType,
            out Type? typeValue,
            bool allowTypeAlso,
            bool allowZeroArgs)
        {
            typeValue = null;
            string? attrValue = null;
            for (int j = 0; j < list.Count; j++)
            {
                attrValue = GetCustomAttributeData(
                    list[j],
                    attrType,
                    out typeValue,
                    allowTypeAlso,
                    false,
                    allowZeroArgs);
                if (attrValue != null)
                {
                    break;
                }
            }

            return attrValue;
        }
    }
}
