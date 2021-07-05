using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal static class ApiTypeFactory
    {
        public static ApiType Create(Type type, ApiTypeCreationMode mode)
        {
            var methods = new List<ApiMethod>();
            var properties = new List<ApiProperty>();

            foreach (var property in MemberProvider.GetProperties(type))
            {
                if (!ArrayMethodGenerator.IsArrayAccessor(property))
                    properties.Add(CreateProperty(property, ApiPropertyFlags.None));
                else
                {
                    if (mode == ApiTypeCreationMode.ManagedApiClass)
                        properties.Add(CreateProperty(property, ApiPropertyFlags.ManagedArrayAccessor));
                    else
                        methods.AddRange(ArrayMethodGenerator.GetArrayAccessorMethods(property).Select(CreateMethod));
                }
            }
            methods.AddRange(MemberProvider.GetMethods(type).Select(CreateMethod));

            return new ApiType(type, properties.ToArray(), methods.ToArray());
        }

        private static ApiMethod CreateMethod(MethodInfo method)
        {
            return new ApiMethod(method);
        }

        private static ApiProperty CreateProperty(PropertyInfo property, ApiPropertyFlags flags)
        {
            return new ApiProperty(property, flags);
        }
    }
}