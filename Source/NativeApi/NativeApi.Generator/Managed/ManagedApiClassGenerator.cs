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
        public string Generate(ApiType managedApiType, ApiType pInvokeApiType)
        {
            var type = managedApiType.Type;
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new CSharpTypes();

            var typeName = type.Name;

            w.WriteLine("#nullable enable");

            w.WriteLine(@"
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
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

            foreach (var property in managedApiType.Properties)
                WriteProperty(w, property, types);

            foreach (var method in managedApiType.Methods)
                WriteMethod(w, method, types);

            WriteEvents(w, types, events);

            w.WriteLine();
            new PInvokeClassGenerator().Generate(pInvokeApiType, w);

            w.Indent--;
            w.WriteLine("}");

            w.Indent--;
            w.WriteLine("}");

            return GetFinalCode(codeWriter.ToString(), types);
        }

        //private static void WriteDestructor(IndentedTextWriter w, Types types, Type type)
        //{
        //    w.WriteLine("protected override void Dispose(bool disposing)");
        //    using (new BlockIndent(w))
        //    {
        //        w.WriteLine("if (!IsDisposed)");
        //        using (new BlockIndent(w))
        //        {
        //            w.WriteLine("if (NativePointer != IntPtr.Zero)");
        //            using (new BlockIndent(w))
        //            {
        //                w.WriteLine($"NativeApi.{TypeProvider.GetNativeName(type)}_Destroy_(NativePointer);");
        //                w.WriteLine("SetNativePointer(IntPtr.Zero);");
        //            }
        //        }
        //        w.WriteLine("base.Dispose(disposing);");
        //    }

        //    w.WriteLine();
        //}

        private static void WriteConstructor(IndentedTextWriter w, Types types, Type type, bool hasEvents)
        {
            var declaringTypeName = types.GetTypeName(type.ToContextualType());
            var visibility = MemberProvider.GetConstructorVisibility(type);

            w.WriteLine($"{GetVisibilityString(visibility)} {declaringTypeName}()");
            using (new BlockIndent(w))
            {
                if (visibility == MemberVisibility.Public)
                    w.WriteLine($"SetNativePointer(NativeApi.{TypeProvider.GetNativeName(type)}_Create_());");

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

        private static void WriteProperty(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var property = apiProperty.Property;
            var propertyName = property.Name;
            var contextualProperty = property.ToContextualProperty();
            var propertyTypeName = types.GetTypeName(contextualProperty);
            var nativeDeclaringTypeName = TypeProvider.GetNativeName(property.DeclaringType!);
            bool isStatic = MemberProvider.IsStatic(property);

            w.WriteLine($"public {GetModifiers(property)}{propertyTypeName} {propertyName}");
            using (new BlockIndent(w))
            {
                if (property.GetMethod != null)
                {
                    w.WriteLine("get");
                    using (new BlockIndent(w))
                    {
                        if (!isStatic)
                            w.WriteLine("CheckDisposed();");

                        if (apiProperty.Flags.HasFlag(ApiPropertyFlags.ManagedArrayAccessor))
                            WriteArrayAccessorPropertyGetterBody(w, apiProperty, types);
                        else
                        {
                            w.Write("return ");
                            w.Write(
                            string.Format(
                                GetNativeToManagedFormatString(contextualProperty),
                                $"NativeApi.{nativeDeclaringTypeName}_Get{propertyName}_({(isStatic ? "" : "NativePointer")})"));
                            w.WriteLine(";");
                        }

                    }

                    w.WriteLine();
                }

                if (property.SetMethod != null)
                {
                    w.WriteLine("set");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine("CheckDisposed();");
                        w.WriteLine(
                            $"NativeApi.{nativeDeclaringTypeName}_Set{propertyName}_({(isStatic ? "" : "NativePointer, ")}{GetManagedToNativeArgument(contextualProperty, "value")});");
                    }
            
                }
            }

            w.WriteLine();
        }

        private static void WriteArrayAccessorPropertyGetterBody(IndentedTextWriter w, ApiProperty apiProperty, Types types)
        {
            var nativeDeclaringTypeName = TypeProvider.GetNativeName(apiProperty.Property.DeclaringType!);
            var propertyName = apiProperty.Property.Name;
            var arrayElementType = apiProperty.Property.PropertyType.GetElementType().ToContextualType();
            var arrayElementTypeName = types.GetTypeName(arrayElementType);

            bool isStatic = MemberProvider.IsStatic(apiProperty.Property);

            w.WriteLine($"var array = NativeApi.{nativeDeclaringTypeName}_Open{propertyName}Array_({(isStatic ? "" : "NativePointer")});");
            w.WriteLine("try");
            using (new BlockIndent(w))
            {
                w.WriteLine($"var count = NativeApi.{nativeDeclaringTypeName}_Get{propertyName}ItemCount_({(isStatic ? "" : "NativePointer, ")}array);");
                w.WriteLine($"var result = new System.Collections.Generic.List<{arrayElementTypeName}>(count);");
                w.WriteLine($"for (int i = 0; i < count; i++)");
                using (new BlockIndent(w))
                {
                    w.Write("var item = ");
                    w.Write(
                    string.Format(
                        GetNativeToManagedFormatString(arrayElementType),
                        $"NativeApi.{nativeDeclaringTypeName}_Get{propertyName}ItemAt_({(isStatic ? "" : "NativePointer, ")}array, i)"));
                    w.WriteLine(";");

                    w.WriteLine($"result.Add(item);");
                }

                w.WriteLine("return result.ToArray();");
            }
            w.WriteLine("finally");
            using (new BlockIndent(w))
            {
                w.WriteLine($"NativeApi.{nativeDeclaringTypeName}_Close{propertyName}Array_({(isStatic ? "" : "NativePointer, ")}array);");
            }
        }

        private static void WriteMethod(IndentedTextWriter w, ApiMethod apiMethod, Types types)
        {
            var method = apiMethod.Method;
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

                var contextualParameter = parameter.ToContextualParameter();
                var parameterType = types.GetTypeName(contextualParameter);
                signatureParametersString.Append(parameterType + " " + parameter.Name);
                callParametersString.Append(GetManagedToNativeArgument(contextualParameter, parameter.Name!));

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
                    $"NativeApi.{TypeProvider.GetNativeName(method.DeclaringType!)}_{methodName}_({callParametersString})"));
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
                    w.WriteLine($"var sink = new NativeApi.{declaringTypeName}EventCallbackType((obj, e, parameter) =>");
                    using (new BlockIndent(w))
                    {
                        w.WriteLine($"var w = {string.Format(GetNativeToManagedFormatString(declaringType.ToContextualType()), "obj")};");
                        w.WriteLine("if (w == null) return IntPtr.Zero;");
                        w.WriteLine("return w.OnEvent(e, parameter);");
                    }
                    w.WriteLine(");");

                    w.WriteLine("eventCallbackGCHandle = GCHandle.Alloc(sink);");
                    w.WriteLine($"NativeApi.{declaringTypeName}_SetEventCallback_(sink);");
                }
            }

            w.WriteLine();

            w.WriteLine($"IntPtr OnEvent(NativeApi.{declaringTypeName}Event e, IntPtr parameter)");
            using (new BlockIndent(w))
            {
                w.WriteLine("switch (e)");
                using (new BlockIndent(w))
                {
                    foreach(var e in events)
                    {
                        w.WriteLine($"case NativeApi.{declaringTypeName}Event.{e.Name}:");

                        var dataType = MemberProvider.TryGetNativEventDataType(e);
                        if (dataType != null)
                        {
                            w.WriteLine($"{e.Name}?.Invoke(this, new NativeEventArgs<{dataType.Name}>(MarshalEx.PtrToStructure<{dataType.Name}>(parameter))); return IntPtr.Zero;");
                        }
                        else
                        {
                            var attribute = MemberProvider.GetEventAttribute(e);
                            if (attribute.Cancellable)
                            {
                                using (new BlockIndent(w))
                                {
                                    w.WriteLine($"var cea = new CancelEventArgs();");
                                    w.WriteLine($"{e.Name}?.Invoke(this, cea);");
                                    w.WriteLine($"return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;");
                                }
                            }
                            else
                                w.WriteLine($"{e.Name}?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;");
                        }
                    }

                    w.WriteLine($"default: throw new Exception(\"Unexpected {declaringTypeName}Event value: \" + e);");
                }
            }

            w.WriteLine();

            foreach (var e in events)
            {
                string? argsType;
                var dataType = MemberProvider.TryGetNativEventDataType(e);
                if (dataType != null)
                    argsType = "NativeEventHandler<" + dataType.Name + ">";
                else
                {
                    var attribute = MemberProvider.GetEventAttribute(e);
                    argsType = attribute.Cancellable ? "EventHandler<CancelEventArgs>" : "EventHandler";
                }
                w.WriteLine($"public event {argsType}? {e.Name};");
            }
        }

        static string GetManagedToNativeArgument(ContextualType type, string name)
        {
            if (TypeProvider.IsComplexType(type))
            {
                if (type.Nullability == Nullability.Nullable)
                    return name + "?.NativePointer ?? IntPtr.Zero";

                return name + ".NativePointer";
            }

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