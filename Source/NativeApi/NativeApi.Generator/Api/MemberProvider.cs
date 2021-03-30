using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal static class MemberProvider
    {
        public static IEnumerable<PropertyInfo> GetProperties(Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(x => x.DeclaringType == type);

        public static IEnumerable<MethodInfo> GetMethods(Type type) =>
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(x => x.DeclaringType == type && !x.IsSpecialName);

        public static bool IsVirtual(MemberInfo member) => member switch
        {
            MethodInfo x => x.IsVirtual,
            PropertyInfo x => x.GetAccessors(true)[0].IsVirtual,
            _ => throw new Exception(),
        };


        public static bool IsStatic(MemberInfo member) => member switch
        {
            MethodInfo x => x.IsStatic,
            PropertyInfo x => x.GetAccessors(true)[0].IsStatic,
            EventInfo x => x.GetAddMethod()?.IsStatic ?? false,
            _ => throw new Exception(),
        };
    }
}