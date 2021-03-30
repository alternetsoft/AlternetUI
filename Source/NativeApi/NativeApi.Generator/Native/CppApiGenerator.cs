using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace ApiGenerator.Native
{
    internal static class CppApiGenerator
    {
        public static string Generate(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CppTypes();

            w.WriteLine(GeneratorUtils.HeaderText);
            w.WriteLine("public:");

            foreach (var property in MemberProvider.GetProperties(type))
                WriteProperty(w, property, types);

            foreach (var property in MemberProvider.GetMethods(type))
                WriteMethod(w, property, types);

            w.WriteLine(GetVisibility(MemberProvider.GetConstructorVisibility(type)));
            WriteConstructor(w, type);

            w.WriteLine(GetVisibility(MemberProvider.GetDestructorVisibility(type)));
            WriteDestructor(w, type);

            return codeWriter.ToString();
        }

        static string GetVisibility(MemberVisibility visibility) => visibility.ToString().ToLower() + ":";

        private static void WriteProperty(IndentedTextWriter w, PropertyInfo property, Types types)
        {
            var name = property.Name;
            var modifiers = GetModifiers(property);
            w.WriteLine();
            w.WriteLine($"{modifiers}{types.GetTypeName(property.PropertyType)} Get{name}();");
            w.WriteLine($"{modifiers}void Set{name}({types.GetTypeName(property.PropertyType, TypeUsage.Argument)} value);");
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

        private static void WriteMethod(IndentedTextWriter w, MethodInfo method, Types types)
        {
            var name = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnType);

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

            w.WriteLine($"{GetModifiers(method)}{returnTypeName} {name}({signatureParameters});");
        }

        private static void WriteConstructor(IndentedTextWriter w, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);

            w.Write($"{declaringTypeName}()");
            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Private)
                w.WriteLine(" {}");
            else
                w.WriteLine(";");
        }

        private static void WriteDestructor(IndentedTextWriter w, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);

            w.Write($"virtual ~{declaringTypeName}()");
            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Private)
                w.WriteLine(" {}");
            else
                w.WriteLine(";");
        }

    }
}