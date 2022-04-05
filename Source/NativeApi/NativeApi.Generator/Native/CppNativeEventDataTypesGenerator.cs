using ApiGenerator.Api;
using Namotion.Reflection;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ApiGenerator.Native
{
    internal static class CppNativeEventDataTypesGenerator
    {
        public static string Generate(IEnumerable<Type> dataTypes)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);
            var types = new CTypes();

            w.WriteLine(GeneratorUtils.HeaderText);
            w.WriteLine("#pragma once");

            w.WriteLine("#include \"ApiUtils.h\"");

            w.WriteLine();
            w.WriteLine("namespace Alternet::UI");
            using (new BlockIndent(w, noNewlineAtEnd: true))
            {
                foreach (var type in dataTypes)
                {
                    w.WriteLine($"#pragma pack(push, 1)");
                    var typeName = TypeProvider.GetNativeName(type);
                    w.WriteLine($"struct {typeName}");
                    using (new BlockIndent(w, noNewlineAtEnd: true))
                    {
                        foreach (var field in type.GetFields())
                            WriteField(w, types, field);
                    }
                    w.WriteLine(";");
                    w.WriteLine($"#pragma pack(pop)");
                    w.WriteLine();
                }
            }

            return codeWriter.ToString();
        }

        private static void WriteField(IndentedTextWriter w, Types types, FieldInfo field)
        {
            w.WriteLine(types.GetTypeName(field.FieldType.ToContextualType(), TypeUsage.Static) + " " + field.Name + ";");
        }

    }
}