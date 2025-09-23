using ApiGenerator.Api;
using ApiGenerator.Managed;
using Namotion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace ApiGenerator.Native
{
    [Flags]
    internal enum TypeUsage
    {
        None,
        Static = 1 << 0,
        Argument = 1 << 1,
        Return = 1 << 2,
    }

    internal abstract class Types
    {
        private static readonly Dictionary<Type, string> primitiveTypes = new ()
            {
                        { typeof(byte), "uint8_t" },
                        { typeof(sbyte), "int8_t" },
                        { typeof(short), "int16_t" },
                        { typeof(ushort), "uint16_t" },
                        { typeof(int), "int" },
                        { typeof(uint), "uint32_t" },
                        { typeof(long), "int64_t" },
                        { typeof(ulong), "uint64_t" },
                        { typeof(float), "float" },
                        { typeof(double), "double" },
                        { typeof(char), "char16_t" },
                        { typeof(void), "void" },
                        { typeof(IntPtr), "void*" }
            };

        private readonly HashSet<string> includes = new(StringComparer.OrdinalIgnoreCase);

        public string GetIncludes() => string.Join("\r\n", includes.ToArray());

        public abstract string GetTypeName(ContextualType type, TypeUsage usage);

        public string GetParameterTypeName(ParameterInfo parameter)
        {
            var attribute = MemberProvider.TryGetCallbackMarshalAttribute(parameter);
            if (attribute != null)
                return MemberProvider.PInvokeCallbackActionTypeName;

            return GetTypeName(parameter.ToContextualParameter(), TypeUsage.Argument);
        }

        protected virtual string? TryGetPrimitiveType(ContextualType type) =>
            primitiveTypes.TryGetValue(type, out var result) ? result : null;

        protected void AddIncludedFile(string fileNameWithoutExtension)
        {
            if (fileNameWithoutExtension.EndsWith("*"))
                return;
            includes.Add($"#include \"{fileNameWithoutExtension}.h\"");
        }
    }

    internal class CppTypes : Types
    {
        public override string GetTypeName(ContextualType type, TypeUsage usage)
        {
            string GetTypeBase()
            {
                if (type == typeof(string))
                {
                    if (type.Nullability == Nullability.Nullable && usage != TypeUsage.Static)
                        return "optional<string>";

                    return usage.HasFlag(TypeUsage.Argument) ? "const string&" : "string";
                }

                if (type == typeof(byte[]))
                {
                    return "void*";
                }

                var primitiveTypeName = TryGetPrimitiveType(type);
                if (primitiveTypeName != null)
                    return primitiveTypeName;

                var name = GetComplexTypeName(type, usage);
                return name;
            }

            return GetTypeBase();
        }

        protected override string? TryGetPrimitiveType(ContextualType type)
        {
            var t = base.TryGetPrimitiveType(type);
            if (t != null)
                return t;

            if (type == typeof(bool))
                return "bool";

            return null;
        }

        protected virtual string GetComplexTypeName(ContextualType type, TypeUsage usage)
        {
            var name = TypeProvider.GetNativeName(type);

            if (type.Type.IsArray)
            {
                var elementType = type.Type.GetElementType() ?? throw new Exception();
                if (TypeProvider.IsStruct(elementType) && !usage.HasFlag(TypeUsage.Return))
                    return TypeProvider.GetNativeName(elementType) + "*";
                if (TypeProvider.IsPrimitive(elementType) && !usage.HasFlag(TypeUsage.Return))
                    return TryGetPrimitiveType(elementType.ToContextualType()) + "*";
            }

            if (type.Type.IsEnum)
                return name;

            if (TypeProvider.IsStruct(type))
                return usage.HasFlag(TypeUsage.Argument) ? "const " + name + "&" : name;

            AddIncludedFile(name);

            if (usage.HasFlag(TypeUsage.Static))
                return name;

            if (TypeProvider.IsManagedServerApiType(type))
                return "void*";

            return name + "*";
        }
    }

    internal class CTypes : Types
    {
        public override string GetTypeName(ContextualType type, TypeUsage usage)
        {
            string GetTypeBase()
            {
                if (type == typeof(string))
                    return usage.HasFlag(TypeUsage.Argument) ? "const char16_t*" : "char16_t*";

                if (type == typeof(byte[]))
                    return "void*";

                var primitiveTypeName = TryGetPrimitiveType(type);
                if (primitiveTypeName != null)
                    return primitiveTypeName;

                var name = GetComplexTypeName(type, usage);
                return name;
            }

            return GetTypeBase();
        }

        protected virtual string GetComplexTypeName(ContextualType type, TypeUsage usage)
        {
            var name = TypeProvider.GetNativeName(type);

            if (type.Type.IsArray)
            {
                var elementType = type.Type.GetElementType() ?? throw new Exception();
                if (TypeProvider.IsStruct(elementType) && !usage.HasFlag(TypeUsage.Return))
                    return TypeProvider.GetNativeName(elementType) + "*";
                if (TypeProvider.IsPrimitive(elementType) && !usage.HasFlag(TypeUsage.Return))
                    return TryGetPrimitiveType(elementType.ToContextualType()) + "*";
            }

            if (TypeProvider.IsStruct(type))
            {
                if (usage.HasFlag(TypeUsage.Return))
                    return name + "_C";

                return name;
            }

            if (type.Type.IsEnum)
                return name;

            AddIncludedFile(name);

            if (usage.HasFlag(TypeUsage.Static))
                return name;

            if (TypeProvider.IsManagedServerApiType(type))
                return "void*";

            return name + "*";
        }

        protected override string? TryGetPrimitiveType(ContextualType type)
        {
            var t = base.TryGetPrimitiveType(type);
            if (t != null)
                return t;

            if (type == typeof(bool))
                return "c_bool";

            return null;
        }
    }
}