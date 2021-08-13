using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace ApiGenerator.Native
{
    internal static class CppEnumsGenerator
    {
        public static string Generate(IEnumerable<Type> types)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            w.WriteLine(GeneratorUtils.HeaderText);
            w.WriteLine("#pragma once");

            w.WriteLine();
            w.WriteLine("namespace Alternet::UI");
            using (new BlockIndent(w, noNewlineAtEnd: true))
            {
                foreach (var type in types)
                {
                    var typeName = TypeProvider.GetNativeName(type);
                    w.WriteLine($"enum class {typeName}");
                    using (new BlockIndent(w, noNewlineAtEnd: true))
                    {
                        foreach (var name in Enum.GetNames(type))
                            w.WriteLine(name + ",");
                    }
                    w.WriteLine(";");
                    w.WriteLine();
                }
            }

            w.WriteLine();

            foreach (var type in types)
            {
                if (!TypeProvider.IsFlagsEnum(type))
                    continue;
                var typeName = TypeProvider.GetNativeName(type);
                w.WriteLine($"template<> struct enable_bitmask_operators<Alternet::UI::{typeName}> {{ static const bool enable = true; }};");
            }

            return codeWriter.ToString();
        }
    }
}