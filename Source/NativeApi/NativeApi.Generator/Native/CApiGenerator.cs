using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ApiGenerator.Api;

namespace ApiGenerator.Native
{
    internal static class CApiGenerator
    {
        static string GetTypeName(Type type) => TypeProvider.GetNativeName(type);

        public static string Generate(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CTypes();

            var typeName = GetTypeName(type);

            w.WriteLine("#include \"ApiUtils.h\"");

            w.WriteLine();
            w.WriteLine("using namespace Alternet::UI;");
            w.WriteLine();

            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Public)
                WriteConstructor(w, types, type);

            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Public)
                WriteDestructor(w, types, type);

            foreach (var property in MemberProvider.GetProperties(type))
                WriteProperty(w, types, property);

            foreach (var property in MemberProvider.GetMethods(type))
                WriteMethod(w, types, property);

            return GetFinalCode(codeWriter.ToString(), types);
        }

        static string GetFinalCode(string code, Types types)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GeneratorUtils.HeaderText);
            sb.AppendLine();
            sb.AppendLine($"#pragma once");
            sb.AppendLine();
            sb.AppendLine(types.GetIncludes());

            sb.Append(code);

            return sb.ToString();
        }

        const string ExportMacro = "ALTERNET_UI_API";

        private static void WriteProperty(IndentedTextWriter w, Types types, PropertyInfo property)
        {
            var name = property.Name;
            var declaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);

            w.WriteLine($"{ExportMacro} {types.GetTypeName(property.PropertyType)} {declaringTypeName}_Get{name}() {{ throw 0; }}");
            w.WriteLine($"{ExportMacro} void {declaringTypeName}_Set{name}({types.GetTypeName(property.PropertyType, TypeUsage.Argument)} value) {{ throw 0; }}");
        }

        private static void WriteMethod(IndentedTextWriter w, Types types, MethodInfo method)
        {
            var name = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnType);
            var declaringTypeName = TypeProvider.GetNativeName(method.DeclaringType!);

            var signatureParameters = new StringBuilder();
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ParameterType, TypeUsage.Argument);
                signatureParameters.Append(parameterType + " " + parameter.Name);

                if (i < parameters.Length - 1)
                    signatureParameters.Append(", ");
            }

            w.WriteLine($"{ExportMacro} {returnTypeName} {declaringTypeName}_{name}({signatureParameters}) {{ throw 0; }}");
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);
            var instanceTypeName = types.GetTypeName(type);

            w.WriteLine($"{ExportMacro} {instanceTypeName} {declaringTypeName}_Create() {{ throw 0; }}");
        }

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);
            var instanceTypeName = types.GetTypeName(type, TypeUsage.Argument);

            w.WriteLine($"{ExportMacro} {declaringTypeName}_Destroy({instanceTypeName} obj) {{ throw 0; }}");
        }
    }
}