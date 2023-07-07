using ApiGenerator.Api;
using Namotion.Reflection;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiGenerator.Managed
{
    internal class ManagedServerPInvokeClassGenerator
    {
        public void Generate(ApiType apiType, IndentedTextWriter w)
        {
            var types = new PInvokeTypes();

            var type = apiType.Type;

            w.WriteLine("[SuppressUnmanagedCodeSecurity]");
            w.WriteLine("public class NativeApi : NativeApiProvider");
            w.WriteLine("{");
            w.Indent++;
            w.WriteLine("static NativeApi() => Initialize();");
            w.WriteLine();

            WriteTrampolineLocator(
                w,
                types,
                ManagedServerMemberProvider.GetTrampolineMembers(apiType));

            foreach (var property in apiType.Properties)
                WriteProperty(w, property, types);

            foreach (var property in apiType.Methods)
                WriteMethod(w, property, types);

            w.Indent--;
            w.WriteLine("}");
        }

        static void WriteDllImport(IndentedTextWriter w) => w.WriteLine("[DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]");
        static void WriteDelegateAttributes(IndentedTextWriter w) => w.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]");

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var propertyTypeName = types.GetTypeName(property.ToContextualProperty());

            if (property.GetMethod != null)
            {
                WriteDelegateAttributes(w);
                w.WriteLine($"public delegate {propertyTypeName} TGet{property.Name}(IntPtr obj);");
                w.WriteLine();
            }

            if (property.SetMethod != null)
            {
                WriteDelegateAttributes(w);
                w.WriteLine($"public delegate void TSet{property.Name}(IntPtr obj, {propertyTypeName} value);");
                w.WriteLine();
            }
        }

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter());

            var signatureParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            bool isStatic = MemberProvider.IsStatic(method);

            if (!isStatic)
            {
                signatureParametersString.Append("IntPtr obj");

                if (parameters.Length > 0)
                    signatureParametersString.Append(", ");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ToContextualParameter());
                signatureParametersString.Append($"{MemberProvider.GetPInvokeAttributes(parameter)}{parameterType} {parameter.Name}");

                if (i < parameters.Length - 1)
                    signatureParametersString.Append(", ");
            }

            WriteDelegateAttributes(w);
            w.WriteLine($"public delegate {returnTypeName} T{method.Name}({signatureParametersString});");
            w.WriteLine();
        }

        private static void WriteTrampolineLocator(IndentedTextWriter w, Types types, MemberInfo[] members)
        {
            var declaringTypeName = TypeProvider.GetNativeName(members[0].DeclaringType!);

            var trampolineEnumName = $"{declaringTypeName}Trampoline";

            w.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
            w.WriteLine($"public delegate IntPtr {declaringTypeName}TrampolineLocatorCallbackType({trampolineEnumName} value);");

            w.WriteLine();

            var names = members.SelectMany(ManagedServerMemberProvider.GetTrampolineNames);

            w.WriteLine($"public enum {trampolineEnumName}");
            using (new BlockIndent(w))
            {
                foreach (var name in names)
                    w.WriteLine($"{name},");
            }

            w.WriteLine();

            WriteDllImport(w);
            w.WriteLine($"public static extern void {declaringTypeName}_SetTrampolineLocatorCallback({declaringTypeName}TrampolineLocatorCallbackType callback);");
            w.WriteLine();
        }
    }
}