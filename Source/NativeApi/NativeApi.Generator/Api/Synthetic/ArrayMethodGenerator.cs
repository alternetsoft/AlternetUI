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

            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Get{property.Name}ItemCount",
                new SyntheticParameterInfo[0],
                typeof(int));

            yield return new SyntheticMethodInfo(
                property.DeclaringType,
                $"Get{property.Name}ItemAt",
                new[] { new SyntheticParameterInfo(typeof(int), "index") },
                property.PropertyType.GetElementType() ?? throw new Exception());
        }
    }
}