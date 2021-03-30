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

            var types = CreateTypes();

            var typeName = type.Name;

            var baseTypeName = GetBaseClass(type, types);
            var extendsClause = baseTypeName == null ? "" : " : " + baseTypeName;
            var abstractModifier = type.IsAbstract ? " abstract" : "";

            w.WriteLine($"internal{abstractModifier} class {typeName}{extendsClause}");
            w.WriteLine("{");
            w.Indent++;

            foreach (var property in MemberProvider.GetProperties(type))
                WriteProperty(w, property, types);

            foreach (var property in MemberProvider.GetMethods(type))
                WriteMethod(w, property, types);

            w.Indent--;
            w.WriteLine("}");

            return GetFinalCode(codeWriter.ToString(), types);
        }

        protected Types CreateTypes() => new Types();

        protected string? GetBaseClass(Type type, Types types)
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
            var name = property.Name;
            var typeName = types.GetTypeName(property.PropertyType);
            var modifiers = GetModifiers(property);

            w.WriteLine($"{modifiers} {typeName} {name} {{ get => throw new System.Exception(); set => throw new System.Exception(); }}");
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

                var parameterType = types.GetTypeName(parameter.ParameterType);
                signatureParameters.Append(parameterType + " " + parameter.Name);

                if (i < parameters.Length - 1)
                    signatureParameters.Append(", ");
            }

            w.WriteLine($"{GetModifiers(method)}{returnTypeName} {name}({signatureParameters}) => throw new System.Exception();");
        }

        static string GetModifiers(MemberInfo member) => MemberProvider.IsStatic(member) ? "static " : "";

        private string GetFinalCode(string code, Types types)
        {
            var finalCode = new StringBuilder();
            finalCode.AppendLine(GeneratorUtils.HeaderText).AppendLine();

            finalCode.Append(code);

            return finalCode.ToString();
        }
    }
}