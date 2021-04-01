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
        None,
        Static = 1 << 0,
        Argument = 1 << 1,
        Return = 1 << 2,
    }

    internal abstract class Types
    {
        HashSet<string> includes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public string GetIncludes() => string.Join("\r\n", includes.ToArray());

        protected void AddIncludedFile(string fileNameWithoutExtension) => includes.Add($"#include \"{fileNameWithoutExtension}.h\"");

        public abstract string GetTypeName(Type type, TypeUsage usage);
    }

    internal class CppTypes : Types
    {
        public override string GetTypeName(Type type, TypeUsage usage)
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
            var name = TypeProvider.GetNativeName(type);
            
            if (type.IsEnum)
                return name;

            if (TypeProvider.IsStruct(type))
                return usage.HasFlag(TypeUsage.Argument) ? "const " + name + "&" : name;

            AddIncludedFile(name);

            if (usage.HasFlag(TypeUsage.Static))
                return name;
            
            return name + "&";
        }
    }

    internal class CTypes : Types
    {
        public override string GetTypeName(Type type, TypeUsage usage)
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
            var name = TypeProvider.GetNativeName(type);
            if (TypeProvider.IsStruct(type))
                return name;

            AddIncludedFile(name);

            if (usage.HasFlag(TypeUsage.Static))
                return name;

            return name + "*";
        }
    }
}