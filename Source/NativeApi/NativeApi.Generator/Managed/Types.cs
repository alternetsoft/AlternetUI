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

        public abstract string GetTypeName(Type type);

        protected bool IsApiType(Type type)
        {
            return type.FullName!.Replace("NativeApi.Api.", "") == type.Name;
        }
    }

    internal class CSharpTypes : Types
    {
        public override string GetTypeName(Type type)
        {
            if (aliases.TryGetValue(type, out var aliasName))
                return aliasName;

            if (IsApiType(type))
                return type.Name;

            return type.FullName!;
        }
    }

    internal class PInvokeTypes : Types
    {
        public override string GetTypeName(Type type)
        {
            if (aliases.TryGetValue(type, out var aliasName))
                return aliasName;

            if (IsApiType(type))
                return "IntPtr";

            return type.FullName!;
        }
    }
}