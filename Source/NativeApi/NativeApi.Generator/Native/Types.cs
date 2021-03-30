using System;
using System.Collections.Generic;
using System.Linq;
using ApiGenerator.Api;
using Namotion.Reflection;

namespace ApiGenerator.Native
{
    [Flags]
    enum TypeUsage
    {
        Default = 0,
        Argument = 1 << 0,
    }

    internal abstract class Types
    {
        HashSet<string> includes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public string GetIncludes() => string.Join("\r\n", includes.ToArray());

        protected void AddIncludedFile(string fileNameWithoutExtension) => includes.Add($"#include \"{fileNameWithoutExtension}.h\"");

        public abstract string GetTypeName(Type type, TypeUsage usage = TypeUsage.Default);
    }

    internal class CppTypes : Types
    {
        public override string GetTypeName(Type type, TypeUsage usage = TypeUsage.Default)
        {
            string GetTypeBase()
            {
                if (type == typeof(string))
                    return usage.HasFlag(TypeUsage.Argument) ? "const string&" : "string";
                if (type == typeof(void))
                    return "void";

                var name = GetComplexTypeName(type, usage);
                return name;
            }

            if (type.ToContextualType().Nullability == Nullability.Nullable)
                throw new NotImplementedException("std::optional");

            return GetTypeBase();
        }

        protected virtual string GetComplexTypeName(Type type, TypeUsage usage)
        {
            if (TypeProvider.IsStruct(type))
                return usage.HasFlag(TypeUsage.Argument) ? "const " + type.Name + "&" : type.Name;

            AddIncludedFile(type.Name);

            return type.Name + "&";
        }
    }

    internal class CTypes : Types
    {
        public override string GetTypeName(Type type, TypeUsage usage = TypeUsage.Default)
        {
            string GetTypeBase()
            {
                if (type == typeof(string))
                    return usage.HasFlag(TypeUsage.Argument) ? "const char16_t*" : "char16_t*";
                if (type == typeof(void))
                    return "void";

                var name = GetComplexTypeName(type, usage);
                return name;
            }

            return GetTypeBase();
        }

        protected virtual string GetComplexTypeName(Type type, TypeUsage usage)
        {
            if (TypeProvider.IsStruct(type))
                return type.Name;

            AddIncludedFile(type.Name);

            return type.Name + "*";
        }
    }
}