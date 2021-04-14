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

            var events = MemberProvider.GetEvents(type).ToArray();

            WriteConstructor(w, types, type, events.Length > 0);
            WriteFromPointerConstructor(w, types, type);

            //if (MemberProvider.GetDestructorVisibility(type) == MemberVisibility.Public)
            //    WriteDestructor(w, types, type);

            foreach (var property in MemberProvider.GetProperties(type))
                WriteProperty(w, property, types);

            foreach (var property in MemberProvider.GetMethods(type))
                WriteMethod(w, property, types);

            WriteEvents(w, types, events);

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

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type, bool hasEvents)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType());
            var visibility = MemberProvider.GetConstructorVisibility(type);

            w.WriteLine($"{GetVisibilityString(visibility)} {declaringTypeName}()");
            using (new BlockIndent(w))
            {
                if (visibility == MemberVisibility.Public)
                    w.WriteLine($"SetNativePointer(NativeApi.{TypeProvider.GetNativeName(type)}_Create());");

                if (hasEvents)
                    w.WriteLine("SetEventCallback();");
            }
            w.WriteLine();
        }

        static string GetVisibilityString(MemberVisibility value) => value.ToString().ToLower();

        private static void WriteFromPointerConstructor(IndentedTextWriter w, Types types, Type type)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType());

            w.WriteLine($"public {declaringTypeName}(IntPtr nativePointer) : base(nativePointer)");
            using (new BlockIndent(w))
            {
            }
            w.WriteLine();
        }

        string? GetBaseClass(Type type, Types types)
        {
            var baseType = type.BaseType;
            if (baseType == null)
                return null;

            if (!TypeProvider.IsApiType(baseType))
                return null;

            return types.GetTypeName(baseType.ToContextualType());
        }

        private static void WriteProperty(IndentedTextWriter w, PropertyInfo property, Types types)
        {
            var propertyName = property.Name;
            var propertyTypeName = types.GetTypeName(property.ToContextualProperty());
            var nativeDeclaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);

            w.WriteLine($"public {GetModifiers(property)}{propertyTypeName} {propertyName}");
            using (new BlockIndent(w))
            {
                if (property.GetMethod != null)
                {
                    w.WriteLine("get");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine("CheckDisposed();");

                        w.Write("return ");
                        w.Write(
                        string.Format(
                            GetNativeToManagedFormatString(property.ToContextualProperty()),
                            $"NativeApi.{nativeDeclaringTypeName}_Get{propertyName}(NativePointer)"));
                        w.WriteLine(";");
                    }

                    w.WriteLine();
                }

                if (property.SetMethod != null)
                {
                    w.WriteLine("set");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine("CheckDisposed();");
                        w.WriteLine($"NativeApi.{nativeDeclaringTypeName}_Set{propertyName}(NativePointer, {GetManagedToNativeArgument(property.PropertyType, "value")});");
                    }
                }
            }

            w.WriteLine();
        }

        private static void WriteMethod(IndentedTextWriter w, MethodInfo method, Types types)
        {
            var methodName = method.Name;
            var returnTypeName = types.GetTypeName(method.ReturnParameter.ToContextualParameter());

            var signatureParametersString = new StringBuilder();
            var callParametersString = new StringBuilder();
            var parameters = method.GetParameters();

            if (!method.IsStatic)
            {
                callParametersString.Append("NativePointer");
                if (parameters.Length > 0)
                    callParametersString.Append(", ");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var parameterType = types.GetTypeName(parameter.ToContextualParameter());
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

            if (!method.IsStatic)
                w.WriteLine("CheckDisposed();");

            if (method.ReturnType != typeof(void))
                w.Write("return ");

            w.Write(
                string.Format(
                    GetNativeToManagedFormatString(method.ReturnParameter.ToContextualParameter()),
                    $"NativeApi.{TypeProvider.GetNativeName(method.DeclaringType!)}_{methodName}({callParametersString})"));
            w.WriteLine(";");


            w.Indent--;
            w.WriteLine("}");

            w.WriteLine();
        }

        private static void WriteEvents(IndentedTextWriter w, Types types, EventInfo[] events)
        {
            if (events.Length == 0)
                return;

            var declaringType = events[0].DeclaringType!;
            var declaringTypeName = types.GetTypeName(declaringType.ToContextualType());

            w.WriteLine("static GCHandle eventCallbackGCHandle;");

            w.WriteLine();

            w.WriteLine("static void SetEventCallback()");
            using (new BlockIndent(w))
            {
                w.WriteLine("if (!eventCallbackGCHandle.IsAllocated)");
                using (new BlockIndent(w))
                {
                    w.WriteLine($"var sink = new NativeApi.{declaringTypeName}EventCallbackType((obj, e) =>");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine($"var w = {string.Format(GetNativeToManagedFormatString(declaringType.ToContextualType()), "obj")};");
                        w.WriteLine("if (w == null) return;");
                        w.WriteLine("w.OnEvent(e);");
                    }
                    w.WriteLine(");");

                    w.WriteLine("eventCallbackGCHandle = GCHandle.Alloc(sink);");
                    w.WriteLine($"NativeApi.{declaringTypeName}_SetEventCallback(sink);");
                }
            }

            w.WriteLine();

            w.WriteLine($"void OnEvent(NativeApi.{declaringTypeName}Event e)");
            using (new BlockIndent(w))
            {
                w.WriteLine("switch (e)");
                using (new BlockIndent(w))
                {
                    foreach(var e in events)
                        w.WriteLine($"case NativeApi.{declaringTypeName}Event.{e.Name}: {e.Name}?.Invoke(this, EventArgs.Empty); break;");

                    w.WriteLine($"default: throw new Exception(\"Unexpected {declaringTypeName}Event value: \" + e);");
                }
            }

            w.WriteLine();

            foreach (var e in events)
                w.WriteLine($"public event EventHandler? {e.Name};");
        }

        static string GetManagedToNativeArgument(Type type, string name)
        {
            if (TypeProvider.IsComplexType(type))
                return name + ".NativePointer";

            return name;
        }

        static string GetNativeToManagedFormatString(ContextualType type)
        {

            var factory = type.OriginalType.IsAbstract ? "null" : $"p => new {type.OriginalType.Name}(p)";

            if (TypeProvider.IsComplexType(type))
                return $"NativeObject.GetFromNativePointer<{type.OriginalType.Name}>({{0}}, {factory})" + (type.Nullability == Nullability.NotNullable ? "!" : "");

            return "{0}";
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