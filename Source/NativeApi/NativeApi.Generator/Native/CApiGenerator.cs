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
            var propertyName = property.Name;
            var declaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);

            w.WriteLine($"{ExportMacro} {types.GetTypeName(property.PropertyType)} {declaringTypeName}_Get{propertyName}()");
            w.WriteLine("{");
            w.Indent++;

            w.Write("return ");
            w.Write(
                string.Format(
                    GetCppToCReturnValueFormatString(property.PropertyType),
                    $"return obj->Get{propertyName}()"));
            w.WriteLine(";");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();


            w.WriteLine($"{ExportMacro} void {declaringTypeName}_Set{propertyName}({types.GetTypeName(property.PropertyType, TypeUsage.Argument)} value)");
            w.WriteLine("{");
            w.Indent++;
            w.WriteLine($"obj->Set{propertyName}({GetCToCppArgument(property.PropertyType, "value")});");
            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        private static void WriteMethod(IndentedTextWriter w, Types types, MethodInfo method)
        {
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnType);
            var declaringTypeName = TypeProvider.GetNativeName(method.DeclaringType!);

            var signatureParametersString = new StringBuilder();
            var callParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            bool isStatic = MemberProvider.IsStatic(method);

            if (!isStatic)
            {
                var parameterType = types.GetTypeName(method.DeclaringType!, TypeUsage.Argument);
                signatureParametersString.Append(parameterType + " obj");

                if (parameters.Length > 0)
                    signatureParametersString.Append(", ");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ParameterType, TypeUsage.Argument);
                signatureParametersString.Append(parameterType + " " + parameter.Name);
                callParametersString.Append(GetCToCppArgument(parameter.ParameterType, parameter.Name!));

                if (i < parameters.Length - 1)
                {
                    signatureParametersString.Append(", ");
                    callParametersString.Append(", ");
                }
            }

            w.WriteLine($"{ExportMacro} {returnTypeName} {declaringTypeName}_{methodName}({signatureParametersString})");
            w.WriteLine("{");
            w.Indent++;

            if (method.ReturnType != typeof(void))
                w.Write("return ");

            w.Write(
                string.Format(
                    GetCppToCReturnValueFormatString(method.ReturnType),
                    $"{(isStatic ? declaringTypeName + "::" : "obj->")}{methodName}({callParametersString})"));

            w.WriteLine(";");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);
            var instanceTypeName = types.GetTypeName(type);

            w.WriteLine($"{ExportMacro} {instanceTypeName} {declaringTypeName}_Create()");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"return new {declaringTypeName}();");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        static string GetCToCppArgument(Type type, string name)
        {
            if (!TypeProvider.IsComplexType(type))
                return name;

            return "*" + name;
        }

        static string GetCppToCReturnValueFormatString(Type type)
        {
            if (type == typeof(string))
                return "AllocPInvokeReturnString({0})";

            if (TypeProvider.IsComplexType(type))
                return "&{0}";

            return "{0}";
        }

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);
            var instanceTypeName = types.GetTypeName(type, TypeUsage.Argument);

            w.WriteLine($"{ExportMacro} void {declaringTypeName}_Destroy({instanceTypeName} obj)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine("delete obj;");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }
    }
}