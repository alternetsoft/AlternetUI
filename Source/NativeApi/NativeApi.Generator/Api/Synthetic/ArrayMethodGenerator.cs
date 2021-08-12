using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal static class ArrayMethodGenerator
    {
        public static bool IsArrayAccessor(PropertyInfo property)
        {
            return property.PropertyType.IsArray;
        }

        public static IEnumerable<MethodInfo> GetArrayAccessorMethods(PropertyInfo property)
        {
            if (!IsArrayAccessor(property))
                throw new Exception();

            bool isStatic = MemberProvider.IsStatic(property);
            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Open{property.Name}Array",
                isStatic,
                new SyntheticParameterInfo[0],
                typeof(IntPtr));

            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Get{property.Name}ItemCount",
                isStatic,
                new SyntheticParameterInfo[] { new SyntheticParameterInfo(typeof(IntPtr), "array") },
                typeof(int));

            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Get{property.Name}ItemAt",
                isStatic,
                new[] { new SyntheticParameterInfo(typeof(IntPtr), "array"), new SyntheticParameterInfo(typeof(int), "index") },
                property.PropertyType.GetElementType() ?? throw new Exception());

            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Close{property.Name}Array",
                isStatic,
                new SyntheticParameterInfo[] { new SyntheticParameterInfo(typeof(IntPtr), "array") },
                typeof(void));
        }
    }
}