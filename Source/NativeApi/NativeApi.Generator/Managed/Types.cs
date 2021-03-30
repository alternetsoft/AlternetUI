using System;
using System.Collections.Generic;

namespace ApiGenerator.Managed
{
    internal class Types
    {
        private static readonly Dictionary<Type, string> aliases =
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

        public string GetTypeName(Type type)
        {
            if (aliases.TryGetValue(type, out var aliasName))
                return aliasName;

            return type.FullName!;
        }
    }
}