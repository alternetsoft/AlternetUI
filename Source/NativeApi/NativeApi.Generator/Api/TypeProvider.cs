using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiGenerator.Api
{
    internal static class TypeProvider
    {
        public static IEnumerable<Type> GetTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsApiType(x) && !IsStruct(x)).ToArray();
        }

        public static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive;

        public static bool IsApiType(Type type) =>
            type.GetCustomAttributes(typeof(ApiAttribute), false).Length > 0;

        public static string GetNativeName(Type type)
        {
            var nativeNameAttributes = type.GetCustomAttributes(typeof(NativeNameAttribute), false);
            if (nativeNameAttributes.Length == 0)
                return type.Name;

            return nativeNameAttributes.Cast<NativeNameAttribute>().Single().Name;
        }
    }
}