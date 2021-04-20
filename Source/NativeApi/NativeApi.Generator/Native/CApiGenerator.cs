using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ApiGenerator.Api;
using Namotion.Reflection;

namespace ApiGenerator.Native
{
    internal static class CApiGenerator
    {
        public static string Generate(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CTypes();

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

            WriteEvents(w, types, MemberProvider.GetEvents(type).ToArray());

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
            var declaringTypeName = types.GetTypeName(property.DeclaringType!.ToContextualType(), TypeUsage.Static);

            var instanceParameterSignaturePart = types.GetTypeName(property.DeclaringType!.ToContextualType(), TypeUsage.Argument) + " obj";

            if (property.GetMethod != null)
            {
                w.WriteLine($"{ExportMacro} {types.GetTypeName(property.ToContextualProperty(), TypeUsage.Return)} {declaringTypeName}_Get{propertyName}({instanceParameterSignaturePart})");
                w.WriteLine("{");
                w.Indent++;

                w.Write("return ");
                w.Write(
                    string.Format(
                        GetCppToCReturnValueFormatString(property.PropertyType),
                        $"obj->Get{propertyName}()"));
                w.WriteLine(";");

                w.Indent--;
                w.WriteLine("}");
                w.WriteLine();
            }

            if (property.SetMethod != null)
            {
                w.WriteLine($"{ExportMacro} void {declaringTypeName}_Set{propertyName}({instanceParameterSignaturePart}, {types.GetTypeName(property.ToContextualProperty(), TypeUsage.Argument)} value)");
                w.WriteLine("{");
                w.Indent++;
                w.WriteLine($"obj->Set{propertyName}({GetCToCppArgument(property.PropertyType, "value")});");
                w.Indent--;
                w.WriteLine("}");
                w.WriteLine();
            }
        }

        private static void WriteMethod(IndentedTextWriter w, Types types, MethodInfo method)
        {
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter(), TypeUsage.Return);
            var declaringTypeName = types.GetTypeName(method.DeclaringType!.ToContextualType(), TypeUsage.Static);

            var signatureParametersString = new StringBuilder();
            var callParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            bool isStatic = MemberProvider.IsStatic(method);

            if (!isStatic)
            {
                var parameterType = types.GetTypeName(method.DeclaringType!.ToContextualType(), TypeUsage.Argument);
                signatureParametersString.Append(parameterType + " obj");

                if (parameters.Length > 0)
                    signatureParametersString.Append(", ");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ToContextualParameter(), TypeUsage.Argument);
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

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declaringTypeName = types.GetTypeName(events[0].DeclaringType!.ToContextualType(), TypeUsage.Static);

            w.WriteLine($"{ExportMacro} void {declaringTypeName}_SetEventCallback({declaringTypeName}::{declaringTypeName}EventCallbackType callback)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"{declaringTypeName}::SetEventCallback(callback);");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);
            var returnTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Return);

            w.WriteLine($"{ExportMacro} {returnTypeName} {declaringTypeName}_Create()");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"return new {declaringTypeName}();");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        static string GetCToCppArgument(Type type, string name)
        {
            return name;
        }

        static string GetCppToCReturnValueFormatString(Type type)
        {
            if (type == typeof(string))
                return "AllocPInvokeReturnString({0})";

            return "{0}";
        }

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);
            var instanceTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Argument);

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