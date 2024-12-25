using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

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
            return assembly.GetTypes().Where(
                x => IsManagedServerApiType(x) && !IsStruct(x) && !x.IsEnum).ToArray();
        }

        public static IEnumerable<Type> GetNativeEventDataTypes()
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            return assembly.GetTypes().Where(
                x => IsNativeEventDataType(x) && !IsStruct(x) && !x.IsEnum).ToArray();
        }

        public static IEnumerable<Type> GetEnumTypes(Assembly assembly, bool checkApiType)
        {
            return assembly.GetTypes().Where(x => IsMyType(x) && x.IsEnum).ToArray();

            bool IsMyType(Type x)
            {
                if (checkApiType)
                    return IsApiType(x);
                return true;
            }
        }

        public static IEnumerable<Type> GetEnumTypes(bool managed)
        {
            var assembly = typeof(NativeApi.Api.Window).Assembly;
            var result = GetEnumTypes(assembly, true);

            if (managed)
                return result;

            var assembly2 = typeof(Alternet.UI.CommonUtils).Assembly;
            var result2 = GetEnumTypes(assembly2, false);

            return result.Concat(result2);
        }

        /// <summary>
        /// Gets 'Alternet.UI.Interfaces' assembly.
        /// </summary>
        public static readonly Assembly LibraryInterfaces = typeof(Alternet.UI.CommonUtils).Assembly;

        public static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum;

        public static bool IsPrimitive(Type type) => type.IsPrimitive;

        public static bool IsComplexType(Type type)
            => !type.IsValueType && !type.IsPrimitive && type != typeof(string);

        public static bool IsMarshaledStruct(Type type) => type.IsValueType &&
            (type.FullName!.Replace("Alternet.Drawing.", "") == type.Name ||
             type.FullName!.Replace("Alternet.UI.", "") == type.Name);

        public static bool IsApiType(Type type)
        {
            return type.FullName!.Replace("NativeApi.Api.", "") == type.Name
                && !IsManagedServerApiType(type) && !IsNativeEventDataType(type);
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
            if (type.Assembly == LibraryInterfaces)
                return type.FullName!;

            return GetCustomManagedName<ManagedNameAttribute>(
                type,
                defaultName);
        }

        public static string GetManagedExternName(Type type, string defaultName)
        {
            if (type.Assembly == LibraryInterfaces)
                return type.FullName!;

            return GetCustomManagedName<ManagedExternNameAttribute>(
                type, 
                defaultName);
        }

        public static string GetCustomManagedName<T>(Type type, string defaultName)
            where T : Attribute
        {
            object? GetPropValue(object src, string propName) => 
                src.GetType()?.GetProperty(propName)?.GetValue(src, null);

            bool typeIsArray = type.IsArray;

            if (typeIsArray)
                type = type.GetElementType()!;

            var attributes = type.GetCustomAttributes(typeof(T), false);
            if (attributes.Length == 0)
                return defaultName;

            var attr = attributes.Cast<T>().Single();
            var s = (string)GetPropValue(attr, "Name")!;
            if (typeIsArray)
                s += "[]";
            return s;
        }        

        public static string GetNativeName(Type type, bool fullName = false)
        {
            var nativeNameAttributes = type.GetCustomAttributes(typeof(NativeNameAttribute), false);
            if (nativeNameAttributes.Length == 0)
                return fullName ? type.FullName ?? type.Name : type.Name;

            return nativeNameAttributes.Cast<NativeNameAttribute>().Single().Name;
        }

        public static string? GetAdditionalInterfaceName(Type type)
        {
            var nativeNameAttributes
                = type.GetCustomAttributes(typeof(ManagedInterfaceAttribute), false);
            if (nativeNameAttributes.Length == 0)
                return null;

            return nativeNameAttributes.Cast<ManagedInterfaceAttribute>().Single().Name;
        }

        public static bool IsFlagsEnum(Type type)
        {
            if (!type.IsEnum)
                return false;
            
            return type.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }
    }
}