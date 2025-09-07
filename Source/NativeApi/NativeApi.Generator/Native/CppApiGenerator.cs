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
        public static string Generate(ApiType apiType)
        {
            var type = apiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CppTypes();

            w.WriteLine(GeneratorUtils.HeaderText);
            w.WriteLine();
            w.WriteLine("public:");

            foreach (var property in apiType.Properties)
                WriteProperty(w, property, types);

            foreach (var property in apiType.Methods)
                WriteMethod(w, property, types);

            WriteEvents(w, types, MemberProvider.GetEvents(type).ToArray());

            w.WriteLine();
            w.WriteLine(GetVisibility(MemberProvider.GetConstructorVisibility(type)));
            WriteConstructor(w, types, type);

            w.WriteLine();
            w.WriteLine(GetVisibility(MemberProvider.GetDestructorVisibility(type)));
            WriteDestructor(w, types, type);

            w.WriteLine();
            w.WriteLine("private:");

            var aa = types.GetTypeName(type.ToContextualType(), TypeUsage.Static);
            w.WriteLine($"BYREF_ONLY({aa});");

            return codeWriter.ToString();
        }

        static string GetVisibility(MemberVisibility visibility) =>
            visibility.ToString().ToLower() + ":";

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var name = property.Name;
            var modifiers = GetModifiers(property);
            
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

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
            var name = method.Name;
            var returnTypeName = types.GetTypeName(
                method.ReturnParameter.ToContextualParameter(),
                TypeUsage.Return);

            var signatureParameters = new StringBuilder();
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetParameterTypeName(parameter);
                signatureParameters.Append(parameterType + " " + parameter.Name);

                if (parameter.ParameterType.IsArray)
                {
                    signatureParameters.Append(", ");

                    var sizeParameterName = MemberProvider.GetArraySizeParameterName(parameter.Name!);
                    signatureParameters.Append("int " + sizeParameterName);
                }

                if (i < parameters.Length - 1)
                    signatureParameters.Append(", ");
            }

            w.WriteLine($"{GetModifiers(method)}{returnTypeName} {name}({signatureParameters});");
        }

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declTName = types.GetTypeName(
                events[0].DeclaringType!.ToContextualType(),
                TypeUsage.Static);
            w.WriteLine();
            w.WriteLine("public:");
            w.WriteLine();
            w.WriteLine($"enum class {declTName}Event");

            w.WriteLine("{");
            w.Indent++;

            foreach (var e in events)
                w.WriteLine(e.Name + ",");

            w.Indent--;
            w.WriteLine("};");
            w.WriteLine();

            w.WriteLine($"typedef void* (*{declTName}EventCallbackType)({declTName}* obj, {declTName}Event event, void* param);");
            w.WriteLine();

            w.Write($"static void SetEventCallback({declTName}EventCallbackType value)");
            w.WriteLine(" { eventCallback = value; }");

            w.WriteLine();
            w.WriteLine("public:");
            w.WriteLine();

            w.WriteLine($"bool RaiseEvent({declTName}Event event, void* parameter = nullptr)");
            w.WriteLine("{" + Environment.NewLine +
                "if (EventsSuspended()) return false;" + Environment.NewLine +
                "if (eventCallback != nullptr)" + Environment.NewLine +
                "   return eventCallback(this, event, parameter) != nullptr;" + Environment.NewLine +
                "else" + Environment.NewLine +
                "   return false;" + Environment.NewLine +
                "}");
            w.WriteLine();

            w.WriteLine(
                $"bool RaiseStaticEvent({declTName}Event event, void* parameter = nullptr)");
            w.WriteLine("{" + Environment.NewLine +
                "if (EventsSuspended()) return false;" + Environment.NewLine +
                "if (eventCallback != nullptr)" + Environment.NewLine +
                "   return eventCallback(nullptr, event, parameter) != nullptr;" + Environment.NewLine +
                "else" + Environment.NewLine +
                "   return false;" + Environment.NewLine +
                "}");
            w.WriteLine();

            w.WriteLine(
                $"void* RaiseEventWithPointerResult({declTName}Event event, void* parameter = nullptr)");
            w.WriteLine("{" + Environment.NewLine +
                "if (EventsSuspended()) return nullptr;" + Environment.NewLine +
                "if (eventCallback != nullptr)" + Environment.NewLine +
                "   return eventCallback(this, event, parameter);" + Environment.NewLine +
                "else" + Environment.NewLine +
                "   return nullptr;" + Environment.NewLine +
                "}");
            w.WriteLine();

            w.WriteLine("private:");
            w.WriteLine();
            w.WriteLine($"inline static {declTName}EventCallbackType eventCallback = nullptr;");
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