using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.IO;

namespace ApiGenerator.Native
{
    internal static class CppScaffoldGenerator
    {
        public static string GenerateHeader(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            w.WriteLine("#pragma once");
            w.WriteLine("#include \"Common.h\"");
            w.WriteLine("#include \"ApiTypes.h\"");

            w.WriteLine();
            w.WriteLine("namespace Alternet::UI");
            using (new BlockIndent(w))
            {
                var typeName = TypeProvider.GetNativeName(type);
                w.WriteLine($"class {typeName}");
                w.WriteLine("{");
                w.Indent--;
                w.WriteLine($"#include \"Api/{typeName}.inc\"");
                w.Indent++;
                w.WriteLine("public:");
                w.WriteLine();
                w.WriteLine("private:");
                w.WriteLine();
                w.WriteLine("};");
            }

            return codeWriter.ToString();
        }

        public static string GenerateSource(Type type)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);
            var typeName = TypeProvider.GetNativeName(type);

            w.WriteLine($"#include \"{typeName}.h\"");

            w.WriteLine();
            w.WriteLine("namespace Alternet::UI");
            using (new BlockIndent(w))
            {
            }

            return codeWriter.ToString();
        }
    }
}