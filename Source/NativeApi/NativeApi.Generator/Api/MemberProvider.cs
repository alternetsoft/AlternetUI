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

        public static IEnumerable<EventInfo> GetEvents(Type type) =>
            type.GetEvents(BindingFlags.Public | BindingFlags.Instance).Where(x => x.DeclaringType == type);

        public static IEnumerable<MethodInfo> GetMethods(Type type) =>
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(x => x.DeclaringType == type && !x.IsSpecialName);


        public static MemberVisibility GetDestructorVisibility(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IDisposable)) ? MemberVisibility.Public : MemberVisibility.Private;
        }

        public static MemberVisibility GetConstructorVisibility(Type type)
        {
            if (type.IsAbstract)
                return type.IsSealed ? MemberVisibility.Private : MemberVisibility.Protected;

            var ctor = type.GetConstructor(new Type[0]);
            if (ctor == null)
                return MemberVisibility.Private;

            if (ctor.IsPublic)
                return MemberVisibility.Public;

            if (ctor.IsFamily)
                return MemberVisibility.Protected;

            return MemberVisibility.Private;
        }

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