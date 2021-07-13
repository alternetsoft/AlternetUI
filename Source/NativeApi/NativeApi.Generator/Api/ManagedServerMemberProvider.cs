using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal static class ManagedServerMemberProvider
    {
        public static string GetGetterTrampolineName(PropertyInfo property)
        {
            return $"{TypeProvider.GetNativeName(property.DeclaringType!)}_Get{property.Name}";
        }

        public static string GetSetterTrampolineName(PropertyInfo property)
        {
            return $"{TypeProvider.GetNativeName(property.DeclaringType!)}_Set{property.Name}";
        }

        public static string GetMethodTrampolineName(MethodInfo method)
        {
            return $"{TypeProvider.GetNativeName(method.DeclaringType!)}_{method.Name}";
        }

        public static IEnumerable<string> GetTrampolineNames(MemberInfo member)
        {
            if (member is PropertyInfo property)
            {
                if (property.GetMethod != null)
                    yield return GetGetterTrampolineName(property);
                if (property.SetMethod != null)
                    yield return GetSetterTrampolineName(property);
            }
            else if (member is MethodInfo method)
                yield return GetMethodTrampolineName(method);
            else
                throw new Exception();
        }

        public static MemberInfo[] GetTrampolineMembers(ApiType apiType) =>
            apiType.Methods.Select(x => x.Method).Cast<MemberInfo>().Concat(apiType.Properties.Select(x => x.Property)).ToArray();
    }
}