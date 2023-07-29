﻿using ApiCommon;
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

        public static IEnumerable<Type> GetManagedServerClassTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsManagedServerApiType(x) && !IsStruct(x) && !x.IsEnum).ToArray();
        }

        public static IEnumerable<Type> GetNativeEventDataTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsNativeEventDataType(x) && !IsStruct(x) && !x.IsEnum).ToArray();
        }

        public static IEnumerable<Type> GetEnumTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(x => IsApiType(x) && x.IsEnum).ToArray();
        }

        public static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum;

        public static bool IsPrimitive(Type type) => type.IsPrimitive;

        public static bool IsComplexType(Type type) => !type.IsValueType && !type.IsPrimitive && type != typeof(string);

        public static bool IsMarshaledStruct(Type type) => type.IsValueType &&
            (type.FullName!.Replace("Alternet.Drawing.", "") == type.Name ||
             type.FullName!.Replace("Alternet.UI.", "") == type.Name);

        public static bool IsApiType(Type type)
        {
            return type.FullName!.Replace("NativeApi.Api.", "") == type.Name && !IsManagedServerApiType(type) && !IsNativeEventDataType(type);
        }

        public static bool IsManagedServerApiType(Type type)
        {
            return type.FullName!.Replace("NativeApi.Api.ManagedServers.", "") == type.Name;
        }

        public static bool IsNativeEventDataType(Type type)
        {
            return type.IsSubclassOf(typeof(NativeEventData));
        }

        public static string GetManagedName(Type type, string defaultName)
        {
            var attributes = type.GetCustomAttributes(typeof(ManagedNameAttribute), false);
            if (attributes.Length == 0)
                return defaultName;

            return attributes.Cast<ManagedNameAttribute>().Single().Name;
        }

        public static string GetManagedExternName(Type type, string defaultName)
        {
            var attributes = type.GetCustomAttributes(typeof(ManagedExternNameAttribute), false);
            if (attributes.Length == 0)
                return defaultName;

            return attributes.Cast<ManagedExternNameAttribute>().Single().Name;
        }

        public static string GetNativeName(Type type)
        {
            var nativeNameAttributes = type.GetCustomAttributes(typeof(NativeNameAttribute), false);
            if (nativeNameAttributes.Length == 0)
                return type.Name;

            return nativeNameAttributes.Cast<NativeNameAttribute>().Single().Name;
        }

        public static bool IsFlagsEnum(Type type)
        {
            if (!type.IsEnum)
                return false;
            
            return type.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }
    }
}