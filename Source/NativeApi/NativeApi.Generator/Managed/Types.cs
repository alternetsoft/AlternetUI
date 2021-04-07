using ApiGenerator.Api;
using System;
using System.Collections.Generic;

namespace ApiGenerator.Managed
{
    internal abstract class Types
    {
        protected static readonly Dictionary<Type, string> aliases =
            new Dictionary<Type, string>()
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

        protected abstract string GetApiClassTypeName(Type type);

        public string GetTypeName(Type type)
        {
            if (type == typeof(string))
                return "string?";

            if (aliases.TryGetValue(type, out var aliasName))
                return aliasName;

            if (type.IsEnum)
                return type.Name;

            if (TypeProvider.IsApiType(type))
                return GetApiClassTypeName(type);

            if (TypeProvider.IsDrawingStruct(type))
                return GetDrawingStructTypeName(type);

            return type.FullName!;
        }

        protected virtual string GetDrawingStructTypeName(Type type) => type.FullName!;
    }

    internal class CSharpTypes : Types
    {
        protected override string GetApiClassTypeName(Type type) => type.Name;
    }

    internal class PInvokeTypes : Types
    {
        protected override string GetApiClassTypeName(Type type) => "IntPtr";

        protected override string GetDrawingStructTypeName(Type type)
        {
            return "NativeApiTypes." + type.Name;
        }
    }
}