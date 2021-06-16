using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiGenerator.Api
{

    internal static class TypeProvider
    {
        public static IEnumerable<Type> GetClassTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsApiType(x) && !IsStruct(x) && !x.IsEnum).ToArray();
        }

        public static IEnumerable<Type> GetEnumTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsApiType(x) && x.IsEnum).ToArray();
        }

        public static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum;

        public static bool IsComplexType(Type type) => !type.IsValueType && !type.IsPrimitive && type != typeof(string);

        public static bool IsDrawingStruct(Type type) => type.IsValueType && type.FullName!.Replace("System.Drawing.", "") == type.Name;

        public static bool IsApiType(Type type)
        {
            return type.FullName!.Replace("NativeApi.Api.", "") == type.Name;
        }

        public static string GetNativeName(Type type)
        {
            var nativeNameAttributes = type.GetCustomAttributes(typeof(NativeNameAttribute), false);
            if (nativeNameAttributes.Length == 0)
                return type.Name;

            return nativeNameAttributes.Cast<NativeNameAttribute>().Single().Name;
        }
    }
}