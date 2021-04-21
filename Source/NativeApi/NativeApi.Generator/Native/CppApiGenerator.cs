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

            WriteEvents(w, types, MemberProvider.GetEvents(type).ToArray());

            w.WriteLine(GetVisibility(MemberProvider.GetConstructorVisibility(type)));
            WriteConstructor(w, types, type);

            w.WriteLine(GetVisibility(MemberProvider.GetDestructorVisibility(type)));
            WriteDestructor(w, types, type);

            w.WriteLine("private:");
            w.WriteLine($"BYREF_ONLY({types.GetTypeName(type.ToContextualType(), TypeUsage.Static)});");

            return codeWriter.ToString();
        }

        static string GetVisibility(MemberVisibility visibility) => visibility.ToString().ToLower() + ":";

        private static void WriteProperty(IndentedTextWriter w, PropertyInfo property, Types types)
        {
            var name = property.Name;
            var modifiers = GetModifiers(property);
            w.WriteLine();
            
            if (property.GetMethod != null)
                w.WriteLine($"{modifiers}{types.GetTypeName(property.ToContextualProperty(), TypeUsage.Return)} Get{name}();");
            
            if (property.SetMethod != null)
                w.WriteLine($"{modifiers}void Set{name}({types.GetTypeName(property.ToContextualProperty(), TypeUsage.Argument)} value);");
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
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter(), TypeUsage.Return);

            var signatureParameters = new StringBuilder();
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ToContextualParameter(), TypeUsage.Argument);
                signatureParameters.Append(parameterType + " " + parameter.Name);

                if (i < parameters.Length - 1)
                    signatureParameters.Append(", ");
            }

            w.WriteLine($"{GetModifiers(method)}{returnTypeName} {name}({signatureParameters});");
        }

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declaringTypeName = types.GetTypeName(events[0].DeclaringType!.ToContextualType(), TypeUsage.Static);
            w.WriteLine("public:");
            w.WriteLine($"enum class {declaringTypeName}Event");

            w.WriteLine("{");
            w.Indent++;

            foreach (var e in events)
                w.WriteLine(e.Name + ",");

            w.Indent--;
            w.WriteLine("};");

            w.WriteLine($"typedef void* (*{declaringTypeName}EventCallbackType)({declaringTypeName}* obj, {declaringTypeName}Event event, void* param);");

            w.Write($"static void SetEventCallback({declaringTypeName}EventCallbackType value)");
            w.WriteLine(" { eventCallback = value; }");

            w.WriteLine("protected:");
            w.Write($"bool RaiseEvent({declaringTypeName}Event event)");
            w.WriteLine(" { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }");

            w.WriteLine("private:");
            w.WriteLine($"inline static {declaringTypeName}EventCallbackType eventCallback = nullptr;");
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);

            w.Write($"{declaringTypeName}()");
            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Private)
                w.WriteLine(" {}");
            else
                w.WriteLine(";");
        }

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);

            w.Write($"virtual ~{declaringTypeName}()");
            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Private)
                w.WriteLine(" {}");
            else
                w.WriteLine(";");
        }

    }
}