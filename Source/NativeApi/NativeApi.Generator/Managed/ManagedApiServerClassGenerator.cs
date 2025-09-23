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
    internal class ManagedApiServerClassGenerator
    {
        public string Generate(ApiType managedApiType, ApiType pInvokeApiType)
        {
            var type = managedApiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CSharpTypes();

            var typeName = type.Name;

            w.WriteLine(@"#pragma warning disable");
            w.WriteLine(@"
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security;");

            w.WriteLine("namespace Alternet.UI.Native");
            w.WriteLine("{");
            w.Indent++;

            var baseTypeName = GetBaseClass(type, types) ?? "ManagedServerObject";
            var extendsClause = baseTypeName == null ? "" : " : " + baseTypeName;
            var abstractModifier = type.IsAbstract ? " abstract" : "";

            w.WriteLine($"internal{abstractModifier} partial class {typeName}{extendsClause}");
            w.WriteLine("{");
            w.Indent++;

            foreach (var property in managedApiType.Properties)
                WriteProperty(w, property, types);

            foreach (var method in managedApiType.Methods)
                WriteMethod(w, method, types);

            WriteTrampolineLocator(w, types, ManagedServerMemberProvider.GetTrampolineMembers(managedApiType));

            w.WriteLine();
            new ManagedServerPInvokeClassGenerator().Generate(pInvokeApiType, w);

            w.Indent--;
            w.WriteLine("}");

            w.Indent--;
            w.WriteLine("}");

            return GetFinalCode(codeWriter.ToString(), types);
        }

        private static void WriteTrampolineLocator(IndentedTextWriter w, Types types, MemberInfo[] members)
        {
            var declaringType = members[0].DeclaringType!;
            var declaringTypeName = TypeProvider.GetNativeName(members[0].DeclaringType!);

            w.WriteLine("static GCHandle trampolineLocatorCallbackGCHandle;");

            var dictionaryType = $"Dictionary<NativeApi.{declaringTypeName}Trampoline, (GCHandle GCHandle, IntPtr Pointer)>";

            w.WriteLine($"static readonly {dictionaryType} trampolineHandles = new {dictionaryType}();");

            w.WriteLine();

            w.WriteLine($"static {declaringTypeName}() {{ SetTrampolineLocatorCallback(); }}");

            w.WriteLine("static void SetTrampolineLocatorCallback()");
            using (new BlockIndent(w))
            {
                w.WriteLine("if (!trampolineLocatorCallbackGCHandle.IsAllocated)");
                using (new BlockIndent(w))
                {
                    w.WriteLine($"var sink = new NativeApi.{declaringTypeName}TrampolineLocatorCallbackType(trampoline =>");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine("switch (trampoline)");
                        using (new BlockIndent(w))
                        {
                            var names = members.SelectMany(ManagedServerMemberProvider.GetTrampolineNames);

                            foreach (var name in names)
                            {
                                var enumMemberName = $"NativeApi.{declaringTypeName}Trampoline.{name}";
                                w.WriteLine($"case {enumMemberName}:");
                                using (new BlockIndent(w))
                                {
                                    w.WriteLine($"if (!trampolineHandles.TryGetValue({enumMemberName}, out var handle))");
                                    using (new BlockIndent(w))
                                    {
                                        w.WriteLine($"var @delegate = (NativeApi.T{name}){name}_Trampoline;");
                                        w.WriteLine($"handle = (GCHandle.Alloc(@delegate), Marshal.GetFunctionPointerForDelegate(@delegate));");
                                        w.WriteLine($"trampolineHandles.Add(trampoline, handle);");
                                    }

                                    w.WriteLine($"return handle.Pointer;");
                                }
                            }

                            w.WriteLine($"default: throw new Exception(\"Unexpected {declaringTypeName}Trampoline value: \" + trampoline);");
                        }
                    }
                    w.WriteLine(");");

                    w.WriteLine("trampolineLocatorCallbackGCHandle = GCHandle.Alloc(sink);");
                    w.WriteLine($"NativeApi.{declaringTypeName}_SetTrampolineLocatorCallback(sink);");
                }
            }

            w.WriteLine();
        }

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var propertyName = property.Name;
            var propertyTypeName = types.GetTypeName(property.ToContextualProperty());
            var managedDeclaringTypeName = property.DeclaringType!.Name;
            bool isStatic = MemberProvider.IsStatic(property);

            if (property.GetMethod != null)
            {
                w.Write($"private static {propertyTypeName} {ManagedServerMemberProvider.GetGetterTrampolineName(property)}(");
                if (!isStatic)
                    w.Write("IntPtr obj");
                w.WriteLine(")");

                using (new BlockIndent(w))
                {
                    w.Write($"return ");
                    if (!isStatic)
                        w.Write($"(({managedDeclaringTypeName})GCHandle.FromIntPtr(obj).Target).");
                    w.WriteLine($"{propertyName};");
                }
            }

            if (property.SetMethod != null)
            {
                w.Write($"private static void {ManagedServerMemberProvider.GetSetterTrampolineName(property)}(");
                if (!isStatic)
                    w.Write("IntPtr obj, ");
                w.WriteLine($"{propertyTypeName} value)");

                using (new BlockIndent(w))
                {
                    if (!isStatic)
                        w.Write($"(({managedDeclaringTypeName})GCHandle.FromIntPtr(obj).Target).");
                    w.WriteLine($"{propertyName} = value;");
                }

                w.WriteLine();
            }

            w.WriteLine();
        }

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter());
            var managedDeclaringTypeName = method.DeclaringType!.Name;
            bool isStatic = MemberProvider.IsStatic(method);

            var signatureParametersString = new StringBuilder();
            var callParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            if (!isStatic)
            {
                signatureParametersString.Append("IntPtr obj");
                if (parameters.Length > 0)
                    signatureParametersString.Append(", ");
            }

            var isUnsafe = false;

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ToContextualParameter());
                signatureParametersString.Append(parameterType + " " + parameter.Name);

                var isPointer = false;

                if (parameterType.EndsWith('*'))
                {
                    isUnsafe = true;
                    isPointer = true;
                }

                if (isPointer)
                {
                }
                else
                {
                }

                callParametersString.Append(GetNativeToManagedArgument(parameter.ParameterType, parameter.Name!));

                if (i < parameters.Length - 1)
                {
                    signatureParametersString.Append(", ");
                    callParametersString.Append(", ");
                }
            }

            var unsafeSpecifier = isUnsafe ? "unsafe " : "";

            w.WriteLine($"private static {unsafeSpecifier}{returnTypeName} {ManagedServerMemberProvider.GetMethodTrampolineName(method)}({signatureParametersString})");

            using (new BlockIndent(w))
            {
                if (method.ReturnType != typeof(void))
                    w.Write("return ");

                if (!isStatic)
                    w.Write($"(({managedDeclaringTypeName})GCHandle.FromIntPtr(obj).Target).");
                w.WriteLine($"{methodName}({callParametersString});");
            }

            w.WriteLine();
        }

        private static string GetNativeToManagedArgument(Type type, string name)
        {
            return name;
        }

        private string? GetBaseClass(Type type, Types types)
        {
            var baseType = type.BaseType;
            if (baseType == null)
                return null;

            if (!TypeProvider.IsManagedServerApiType(baseType))
                return null;

            return types.GetTypeName(baseType.ToContextualType());
        }

        private string GetFinalCode(string code, Types types)
        {
            var finalCode = new StringBuilder();
            finalCode.AppendLine(GeneratorUtils.HeaderText);

            finalCode.Append(code);

            return finalCode.ToString();
        }
    }
}