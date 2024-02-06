using ApiGenerator.Api;
using ApiGenerator.Native;
using Namotion.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ApiGenerator.Managed
{
    internal abstract class Types
    {
        protected static readonly Dictionary<Type, string> aliases = new()
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(void), "void" }
        };

        protected abstract string GetApiClassTypeName(ContextualType type);

#pragma warning disable
        protected string GetNullableDecoratedName(ContextualType type, string name) =>
            type.Nullability == Nullability.Nullable ? name + "?" : name;
#pragma warning restore

        public string GetParameterTypeName(ParameterInfo parameter)
        {
            var attribute = MemberProvider.TryGetCallbackMarshalAttribute(parameter);
            if (attribute != null)
                return MemberProvider.PInvokeCallbackActionTypeName;

            return GetTypeName(parameter.ToContextualParameter());
        }

        public string GetTypeName(ContextualType type)
        {
            if (aliases.TryGetValue(type, out var aliasName))
                return GetNullableDecoratedName(type, aliasName);

            if (type.Type.IsArray)
            {
                var elementType = type.Type.GetElementType() ?? throw new Exception();
                if (TypeProvider.IsMarshaledStruct(elementType))
                    return GetMarshaledStructTypeName(elementType.ToContextualType()) + "[]";
            }

            if (type.OriginalType.IsEnum)
                return type.OriginalType.Name;

            if (TypeProvider.IsApiType(type) || TypeProvider.IsManagedServerApiType(type))
                return GetApiClassTypeName(type);

            if (TypeProvider.IsMarshaledStruct(type))
                return GetMarshaledStructTypeName(type);

            return GetNullableDecoratedName(type, type.OriginalType.FullName!);
        }

        protected virtual string GetMarshaledStructTypeName(ContextualType type) => GetNullableDecoratedName(type, type.OriginalType.FullName!);
    }

    internal class CSharpTypes : Types
    {
        protected override string GetApiClassTypeName(ContextualType type) => GetNullableDecoratedName(type, type.OriginalType.Name);
    }

    internal class PInvokeTypes : Types
    {
        protected override string GetApiClassTypeName(ContextualType type) => "IntPtr";

        protected override string GetMarshaledStructTypeName(ContextualType type)
        {
            return GetNullableDecoratedName(type, "NativeApiTypes." + type.OriginalType.Name);
        }
    }
}