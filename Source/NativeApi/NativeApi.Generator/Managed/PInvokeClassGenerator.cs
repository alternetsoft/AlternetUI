using ApiGenerator.Api;
using Namotion.Reflection;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiGenerator.Managed
{
    internal class PInvokeClassGenerator
    {
        public void Generate(ApiType apiType, IndentedTextWriter w)
        {
            var types = new PInvokeTypes();

            var type = apiType.Type;
            /*var typeName = type.Name;*/

            w.WriteLine("[SuppressUnmanagedCodeSecurity]");
            w.WriteLine("public class NativeApi : NativeApiProvider");
            w.WriteLine("{");
            w.Indent++;
            w.WriteLine("static NativeApi() => Initialize();");
            w.WriteLine();

            WriteEvents(w, types, MemberProvider.GetEvents(type).ToArray());

            if (MemberProvider.GetConstructorVisibility(type) == MemberVisibility.Public)
                WriteConstructor(w, types, type);

            if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Public)
                WriteDestructor(w, types, type);

            foreach (var property in apiType.Properties)
                WriteProperty(w, property, types);

            foreach (var property in apiType.Methods)
                WriteMethod(w, property, types);

            w.Indent--;
            w.WriteLine("}");
        }

        static void WriteDllImport(IndentedTextWriter w) => w.WriteLine("[DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]");

        private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);

            WriteDllImport(w);
            w.WriteLine($"public static extern void {declaringTypeName}_Destroy_(IntPtr obj);");
            w.WriteLine();
        }

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = TypeProvider.GetNativeName(type);

            WriteDllImport(w);
            w.WriteLine($"public static extern IntPtr {declaringTypeName}_Create_();");
            w.WriteLine();
        }

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var declaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);
            var propertyName = property.Name;
            var propertyTypeName = types.GetTypeName(property.ToContextualProperty());

            var managedDeclaringTypeName = TypeProvider.GetManagedExternName(
                property.PropertyType!,
                propertyTypeName);

            bool isStatic = MemberProvider.IsStatic(property);

            if (property.GetMethod != null)
            {
                WriteDllImport(w);
                var argument = isStatic ? "" : "IntPtr obj";
                w.WriteLine($"public static extern {managedDeclaringTypeName} {declaringTypeName}_Get{propertyName}_({argument});");
                w.WriteLine();
            }

            if (property.SetMethod != null)
            {
                WriteDllImport(w);
                var thisArgument = isStatic ? "" : "IntPtr obj, ";
                w.WriteLine($"public static extern void {declaringTypeName}_Set{propertyName}_({thisArgument}{managedDeclaringTypeName} value);");
                w.WriteLine();
            }
        }

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
            var declaringTypeName = TypeProvider.GetNativeName(method.DeclaringType!);
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter());

            var returnTypeNameOverride = TypeProvider.GetManagedExternName(
                method.ReturnParameter.ParameterType!,
                returnTypeName);

            var signatureParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            bool isStatic = MemberProvider.IsStatic(method);

            if (!isStatic)
            {
                var parameterType = types.GetTypeName(method.DeclaringType!.ToContextualType());
                signatureParametersString.Append(parameterType + " obj");

                if (parameters.Length > 0)
                    signatureParametersString.Append(", ");
            }

            var isUnsafe = false;

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetParameterTypeName(parameter);

                var parameterTypeNameOverride = TypeProvider.GetManagedExternName(
                    parameter.ParameterType!,
                    parameterType);

                signatureParametersString.Append($"{MemberProvider.GetPInvokeAttributes(parameter)}{parameterTypeNameOverride} {parameter.Name}");

                if (parameter.ParameterType.Name.EndsWith("*"))
                {
                    isUnsafe = true;
                }
                else
                {
                }

                if (parameter.ParameterType.IsArray)
                {
                    signatureParametersString.Append(", ");

                    var sizeParameterName = 
                        MemberProvider.GetArraySizeParameterName(parameter.Name!);
                    signatureParametersString.Append("int " + sizeParameterName);
                }

                if (i < parameters.Length - 1)
                    signatureParametersString.Append(", ");
            }

            WriteDllImport(w);

            var needUnsafe = isUnsafe ? "unsafe " : "";

            w.WriteLine($"public {needUnsafe}static extern {returnTypeNameOverride} {declaringTypeName}_{methodName}_({signatureParametersString});");
            w.WriteLine();
        }

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declaringTypeName = TypeProvider.GetNativeName(events[0].DeclaringType!);

            w.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
            w.WriteLine($"public delegate IntPtr {declaringTypeName}EventCallbackType(IntPtr obj, {declaringTypeName}Event e, IntPtr param);");

            w.WriteLine();

            w.WriteLine($"public enum {declaringTypeName}Event");
            using (new BlockIndent(w))
            {
                foreach (var e in events)
                    w.WriteLine($"{e.Name},");
            }

            w.WriteLine();

            WriteDllImport(w);
            w.WriteLine($"public static extern void {declaringTypeName}_SetEventCallback_({declaringTypeName}EventCallbackType callback);");
            w.WriteLine();
        }
    }
}