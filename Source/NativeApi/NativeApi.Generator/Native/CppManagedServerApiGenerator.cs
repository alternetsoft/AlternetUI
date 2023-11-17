using ApiGenerator.Api;
using Namotion.Reflection;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiGenerator.Native
{
    internal static class CppManagedServerApiGenerator
    {
        public static string Generate(ApiType apiType)
        {
            var type = apiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CppTypes();

            w.WriteLine(GeneratorUtils.HeaderText);
            w.WriteLine("#pragma once");
            w.WriteLine("#include \"../Common.h\"");
            w.WriteLine("#include \"../ApiTypes.h\"");

            w.WriteLine();
            w.WriteLine("namespace Alternet::UI");
            using (new BlockIndent(w))
            {
                w.WriteLine("class " + type.Name);
                using (new BlockIndent(w))
                {
                    w.WriteLine("public:");
                    w.Indent++;

                    foreach (var property in apiType.Properties)
                        WriteProperty(w, property, types);

                    foreach (var property in apiType.Methods)
                        WriteMethod(w, property, types);

                    WriteTrampolineLocator(
                        w,
                        types,
                        ManagedServerMemberProvider.GetTrampolineMembers(apiType));

                    WriteConstructor(w, types, type);

                    w.Indent--;

                    w.WriteLine("private:");
                    w.Indent++;
#pragma warning disable
                    string typeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);
#pragma warning restore
                    w.WriteLine("void* objectHandle;");
                    w.WriteLine(
                        "inline static TTrampolineLocatorCallback trampolineLocatorCallback = nullptr;");
                    w.Indent--;
                }
                w.WriteLine(";");
            }

            return codeWriter.ToString();
        }

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var name = property.Name;
            var modifiers = GetModifiers(property);
            w.WriteLine();

            var throwText = $"throwExInvalidOpWithInfo(wxStr(\"{name}\"))";

            if (property.GetMethod != null)
            {
                var returnType = types.GetTypeName(property.ToContextualProperty(), TypeUsage.Return);
                w.WriteLine($"{modifiers}{returnType} Get{name}()");
                using (new BlockIndent(w))
                {
                    w.WriteLine($"if (trampolineLocatorCallback == nullptr) {throwText};");
                    w.WriteLine(
                        $"auto trampoline = (TGet{name})trampolineLocatorCallback(Trampoline::Get{name});");
                    w.Write($"return trampoline(");
                    if (!MemberProvider.IsStatic(property))
                        w.Write($"objectHandle");
                    w.WriteLine($");");
                }
                w.Write($"typedef {returnType} (*TGet{property.Name})(");
                if (!MemberProvider.IsStatic(property))
                    w.Write($"void* objectHandle");
                w.WriteLine($");");
            }

            if (property.SetMethod != null)
            {
                var valueType = types.GetTypeName(property.ToContextualProperty(), TypeUsage.Argument);
                w.WriteLine($"{modifiers}void Set{name}({valueType} value)");
                {
                    using (new BlockIndent(w))
                    {
                        w.WriteLine($"if (trampolineLocatorCallback == nullptr) {throwText};");
                        w.WriteLine(
                            $"auto trampoline = (TSet{name})trampolineLocatorCallback(Trampoline::Set{name});");
                        w.Write($"trampoline(");
                        if (!MemberProvider.IsStatic(property))
                            w.Write($"objectHandle, ");
                        w.WriteLine($"value);");
                    }
                }
                w.Write($"typedef void (*TSet{property.Name})(");
                if (!MemberProvider.IsStatic(property))
                    w.Write($"void* objectHandle, ");
                w.WriteLine($"{valueType} value);");
            }
            w.WriteLine();
        }

        private static string GetModifiers(MemberInfo member)
        {
            var result = new StringBuilder();
            if (MemberProvider.IsVirtual(member))
                result.Append("virtual ");
            if (MemberProvider.IsStatic(member))
                result.Append("static ");
            return result.ToString();
        }

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
            var name = method.Name;
            var returnTypeName = types.GetTypeName(
                method.ReturnParameter.ToContextualParameter(),
                TypeUsage.Return);
            var throwText = $"throwExInvalidOpWithInfo(wxStr(\"{name}\"))";

            var signatureParameters = new StringBuilder();
            var callParameters = new StringBuilder();
            var parameters = method.GetParameters();

            if (!MemberProvider.IsStatic(method))
            {
                callParameters.Append($"objectHandle");
                if (parameters.Length > 0)
                    callParameters.Append($", ");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetParameterTypeName(parameter);
                signatureParameters.Append(parameterType + " " + parameter.Name);
                callParameters.Append(parameter.Name);

                if (i < parameters.Length - 1)
                {
                    signatureParameters.Append(", ");
                    callParameters.Append(", ");
                }
            }

            w.WriteLine($"{GetModifiers(method)}{returnTypeName} {name}({signatureParameters})");
            
            using (new BlockIndent(w))
            {
                w.WriteLine($"if (trampolineLocatorCallback == nullptr) {throwText};");
                w.WriteLine(
                    $"auto trampoline = (T{name})trampolineLocatorCallback(Trampoline::{name});");
                if (returnTypeName != "void")
                    w.Write($"return ");
                w.WriteLine($"trampoline({callParameters});");
            }

            w.Write($"typedef {returnTypeName} (*T{method.Name})(");
            if (!MemberProvider.IsStatic(method))
            {
                w.Write($"void* objectHandle");
                if (parameters.Length > 0)
                    w.Write($", ");
            }
            w.WriteLine($"{signatureParameters});");
        }

        private static void WriteTrampolineLocator(
            IndentedTextWriter w,
            Types types,
            MemberInfo[] members)
        {
            if (members.Length == 0)
                return;

            var declaringTypeName =
                types.GetTypeName(members[0].DeclaringType!.ToContextualType(), TypeUsage.Static);
            w.WriteLine("public:");
            w.WriteLine($"enum class Trampoline");

            w.WriteLine("{");
            w.Indent++;

            foreach (var e in members.SelectMany(ManagedServerMemberProvider.GetTrampolineNames))
                w.WriteLine(e + ",");

            w.Indent--;
            w.WriteLine("};");

            w.WriteLine($"typedef void* (*TTrampolineLocatorCallback)(Trampoline trampoline);");

            w.Write($"static void SetTrampolineLocatorCallback(TTrampolineLocatorCallback value)");
            w.WriteLine(" { trampolineLocatorCallback = value; }");
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);

            w.WriteLine(
                $"{declaringTypeName}(void* objectHandle_) : objectHandle(objectHandle_) {{}}");
        }
    }
}