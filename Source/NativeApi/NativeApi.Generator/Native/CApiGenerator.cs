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
        public static string Generate(ApiType apiType)
        {
            var type = apiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CTypes();

            w.WriteLine("#include \"ApiUtils.h\"");
            w.WriteLine("#include \"Exceptions.h\"");

            w.WriteLine();
            w.WriteLine("using namespace Alternet::UI;");
            w.WriteLine();

            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Public)
                WriteConstructor(w, types, type);

            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Public)
                WriteDestructor(w, types, type);

            foreach (var property in apiType.Properties)
                WriteProperty(w, types, property);

            foreach (var method in apiType.Methods)
                WriteMethod(w, types, method);

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

        public const string ExportMacro = "ALTERNET_UI_API";

        private static void WriteProperty(IndentedTextWriter w, Types types, ApiProperty apiProperty)
        {
            var property = apiProperty.Property;
            var propertyName = property.Name;
            var declaringTypeName = types.GetTypeName(property.DeclaringType!.ToContextualType(), TypeUsage.Static);

            var instanceParameterSignaturePart = types.GetTypeName(property.DeclaringType!.ToContextualType(), TypeUsage.Argument) + " obj";

            if (property.GetMethod != null)
            {
                var propertyTypeName = types.GetTypeName(property.ToContextualProperty(), TypeUsage.Return);
                w.WriteLine($"{ExportMacro} {propertyTypeName} {declaringTypeName}_Get{propertyName}_({instanceParameterSignaturePart})");
                w.WriteLine("{");
                w.Indent++;

                using (new MarshalExceptionsScope(w, propertyTypeName))
                {
                    w.Write("return ");
                    w.Write(
                        string.Format(
                            GetCppToCReturnValueFormatString(property.PropertyType),
                            $"obj->Get{propertyName}()"));
                    w.WriteLine(";");
                }

                w.Indent--;
                w.WriteLine("}");
                w.WriteLine();
            }

            if (property.SetMethod != null)
            {
                w.WriteLine($"{ExportMacro} void {declaringTypeName}_Set{propertyName}_({instanceParameterSignaturePart}, {types.GetTypeName(property.ToContextualProperty(), TypeUsage.Argument)} value)");
                w.WriteLine("{");
                w.Indent++;
                using (new MarshalExceptionsScope(w, "void"))
                    w.WriteLine($"obj->Set{propertyName}({GetCToCppArgument(property.ToContextualProperty(), "value")});");
                w.Indent--;
                w.WriteLine("}");
                w.WriteLine();
            }
        }

        class MarshalExceptionsScope : IDisposable
        {
            private readonly IndentedTextWriter writer;

            public MarshalExceptionsScope(IndentedTextWriter writer, string returnTypeName)
            {
                this.writer = writer;

                if (returnTypeName != "void")
                    writer.Write("return ");

                writer.Write($"MarshalExceptions<{returnTypeName}>([&]()");
                writer.Indent++;
                writer.WriteLine("{");
                writer.Indent++;
            }

            public void Dispose()
            {
                writer.Indent--;
                writer.WriteLine("});");
                writer.Indent--;
            }
        }

        private static void WriteMethod(IndentedTextWriter w, Types types, ApiMethod apiMethod)
        {
            var method = apiMethod.Method;
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
                callParametersString.Append(GetCToCppArgument(parameter.ToContextualParameter(), parameter.Name!));

                if (parameter.ParameterType.IsArray)
                {
                    signatureParametersString.Append(", ");
                    callParametersString.Append(", ");

                    var sizeParameterName = MemberProvider.GetArraySizeParameterName(parameter.Name!);
                    signatureParametersString.Append("int " + sizeParameterName);
                    callParametersString.Append(sizeParameterName);
                }

                if (i < parameters.Length - 1)
                {
                    signatureParametersString.Append(", ");
                    callParametersString.Append(", ");
                }
            }

            w.WriteLine($"{ExportMacro} {returnTypeName} {declaringTypeName}_{methodName}_({signatureParametersString})");
            w.WriteLine("{");
            w.Indent++;

            using (new MarshalExceptionsScope(w, returnTypeName))
            {
                if (method.ReturnType != typeof(void))
                    w.Write("return ");

                w.Write(
                    string.Format(
                        GetCppToCReturnValueFormatString(method.ReturnType),
                        $"{(isStatic ? declaringTypeName + "::" : "obj->")}{methodName}({callParametersString})"));

                w.WriteLine(";");
            }

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declaringTypeName = types.GetTypeName(events[0].DeclaringType!.ToContextualType(), TypeUsage.Static);

            w.WriteLine($"{ExportMacro} void {declaringTypeName}_SetEventCallback_({declaringTypeName}::{declaringTypeName}EventCallbackType callback)");
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

            w.WriteLine($"{ExportMacro} {returnTypeName} {declaringTypeName}_Create_()");
            w.WriteLine("{");
            w.Indent++;

            using (new MarshalExceptionsScope(w, returnTypeName))
                w.WriteLine($"return new {declaringTypeName}();");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        static string GetCToCppArgument(ContextualType type, string name)
        {
            if (type.Nullability == Nullability.Nullable && !TypeProvider.IsComplexType(type))
                return $"ToOptional({name})";
            
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

            w.WriteLine($"{ExportMacro} void {declaringTypeName}_Destroy_({instanceTypeName} obj)");
            w.WriteLine("{");
            w.Indent++;

            using (new MarshalExceptionsScope(w, "void"))
                w.WriteLine("delete obj;");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }
    }
}