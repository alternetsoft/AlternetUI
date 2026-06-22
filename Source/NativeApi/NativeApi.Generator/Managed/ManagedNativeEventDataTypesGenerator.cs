using ApiGenerator.Api;
using Namotion.Reflection;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ApiGenerator.Managed
{
    internal static class ManagedNativeEventDataTypesGenerator
    {
        public static string Generate(IEnumerable<Type> dataTypes)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            var types = new PInvokeTypes();

            w.WriteLine(GeneratorUtils.HeaderText);

            w.WriteLine("using System.Runtime.InteropServices;");
            w.WriteLine();
            w.WriteLine("namespace Alternet.UI.Native");
            using (new BlockIndent(w, noNewlineAtEnd: true))
            {
                foreach (var type in dataTypes)
                {
                    w.WriteLine($"[StructLayout(LayoutKind.Sequential, Pack = 1)]");
                    var typeName = TypeProvider.GetNativeName(type);
                    w.WriteLine($"class {typeName}");
                    using (new BlockIndent(w))
                    {
                        foreach (var field in type.GetFields())
                            WriteField(w, types, field);
                    }
                    w.WriteLine();
                }
            }

            return codeWriter.ToString();
        }

        private static void WriteField(IndentedTextWriter w, Types types, FieldInfo field)
        {
            var st = field.FieldType.ToContextualType();
            var s = types.GetTypeName(st);

            if (s == "NativeApiTypes.NativeStringSpan")
            {
                s = "NativeStringSpan";
            }

            w.WriteLine("public " + s + " " + field.Name + ";");
        }
    }
}