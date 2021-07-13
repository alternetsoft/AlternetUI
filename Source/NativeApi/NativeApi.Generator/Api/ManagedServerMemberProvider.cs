using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal static class ManagedServerMemberProvider
    {
        private const string Suffix = "_Trampoline";

        public static string GetGetterTrampolineName(PropertyInfo property)
        {
            return $"Get{property.Name}{Suffix}";
        }

        public static string GetSetterTrampolineName(PropertyInfo property)
        {
            return $"Set{property.Name}{Suffix}";
        }

        public static string GetMethodTrampolineName(MethodInfo method)
        {
            return $"{method.Name}{Suffix}";
        }

        public static IEnumerable<string> GetTrampolineNames(MemberInfo member)
        {
            if (member is PropertyInfo property)
            {
                if (property.GetMethod != null)
                    yield return "Get" + property.Name;
                if (property.SetMethod != null)
                    yield return "Set" + property.Name;
            }
            else if (member is MethodInfo method)
                yield return method.Name;
            else
                throw new Exception();
        }

        public static MemberInfo[] GetTrampolineMembers(ApiType apiType) =>
            apiType.Methods.Select(x => x.Method).Cast<MemberInfo>().Concat(apiType.Properties.Select(x => x.Property)).ToArray();
    }
}