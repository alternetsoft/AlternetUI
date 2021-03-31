using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace ApiGenerator.Managed
{
    internal class ManagedApiClassGenerator
    {
        public string Generate(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CSharpTypes();

            var typeName = type.Name;

            w.WriteLine(@"
using System;
using System.Runtime.InteropServices;
using System.Security;");

            w.WriteLine("namespace Alternet.UI.Native");
            w.WriteLine("{");
            w.Indent++;

            var baseTypeName = GetBaseClass(type, types) ?? "NativeObject";
            var extendsClause = baseTypeName == null ? "" : " : " + baseTypeName;
            var abstractModifier = type.IsAbstract ? " abstract" : "";

            w.WriteLine($"internal{abstractModifier} class {typeName}{extendsClause}");
            w.WriteLine("{");
            w.Indent++;

            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Public)
                WriteConstructor(w, types, type);

            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Public)
                WriteDestructor(w, types, type);

            foreach (var property in MemberProvider.GetProperties(type))
                WriteProperty(w, property, types);

            foreach (var property in MemberProvider.GetMethods(type))
                WriteMethod(w, property, types);

            w.WriteLine();
            new PInvokeClassGenerator().Generate(type, w);

            w.Indent--;
            w.WriteLine("}");

            w.Indent--;
            w.WriteLine("}");

            return GetFinalCode(codeWriter.ToString(), types);
        }

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            w.WriteLine("protected override void Dispose(bool disposing)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine("base.Dispose(disposing);");
            w.WriteLine("if (!IsDisposed)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine("if (NativePointer != IntPtr.Zero)");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"NativeApi.{TypeProvider.GetNativeName(type)}_Destroy(NativePointer);");
            w.WriteLine("NativePointer = IntPtr.Zero;");

            w.Indent--;
            w.WriteLine("}");

            w.Indent--;
            w.WriteLine("}");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type);

            w.WriteLine($"public {declaringTypeName}()");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine($"NativePointer = NativeApi.{TypeProvider.GetNativeName(type)}_Create();");

            w.Indent--;
            w.WriteLine("}");
            w.WriteLine();
        }

        string? GetBaseClass(Type type, Types types)
        {
            var baseType = type.BaseType;
            if (baseType == null)
                return null;

            if (!TypeProvider.IsApiType(baseType))
                return null;

            return types.GetTypeName(baseType);
        }

        private static void WriteProperty(IndentedTextWriter w, PropertyInfo property, Types types)
        {
            var propertyName = property.Name;
            var propertyTypeName = types.GetTypeName(property.PropertyType);
            var nativeDeclaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);

            w.WriteLine($"public {GetModifiers(property)}{propertyTypeName} {propertyName}");
            using (new BlockIndent(w))
            {
                w.WriteLine("get");
                using (new BlockIndent(w))
                {
                    w.WriteLine("CheckDisposed();");
                    w.WriteLine($"return NativeApi.{nativeDeclaringTypeName}_Get{propertyName}(NativePointer);");
                }

                w.WriteLine();

                w.WriteLine("set");
                using (new BlockIndent(w))
                {
                    w.WriteLine("CheckDisposed();");
                    w.WriteLine($"NativeApi.{nativeDeclaringTypeName}_Set{propertyName}(NativePointer, {GetManagedToNativeArgument(property.PropertyType, "value")});");
                }
            }
        }

        private static void WriteMethod(IndentedTextWriter w, MethodInfo method, Types types)
        {
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnType);

            var signatureParametersString = new StringBuilder();
            var callParametersString = new StringBuilder();
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ParameterType);
                signatureParametersString.Append(parameterType + " " + parameter.Name);
                callParametersString.Append(GetManagedToNativeArgument(parameter.ParameterType, parameter.Name!));

                if (i < parameters.Length - 1)
                {
                    signatureParametersString.Append(", ");
                    callParametersString.Append(", ");
                }
            }

            w.WriteLine($"public {GetModifiers(method)}{returnTypeName} {methodName}({signatureParametersString})");
            w.WriteLine("{");
            w.Indent++;

            w.WriteLine("CheckDisposed();");

            if (method.ReturnType != typeof(void))
                w.Write("return ");

            w.WriteLine($"NativeApi.{TypeProvider.GetNativeName(method.DeclaringType!)}_{methodName}(NativePointer, {callParametersString});");

            w.Indent--;
            w.WriteLine("}");
        }

        static string GetManagedToNativeArgument(Type type, string name)
        {
            if (TypeProvider.IsComplexType(type))
                return name + ".NativePointer";

            return name;
        }

        static string GetModifiers(MemberInfo member) => MemberProvider.IsStatic(member) ? "static " : "";

        private string GetFinalCode(string code, Types types)
        {
            var finalCode = new StringBuilder();
            finalCode.AppendLine(GeneratorUtils.HeaderText);

            finalCode.Append(code);

            return finalCode.ToString();
        }
    }
}