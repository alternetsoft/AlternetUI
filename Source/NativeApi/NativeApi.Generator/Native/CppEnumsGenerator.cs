using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                        var names = Enum.GetNames(type);
                        var values = (Enum.GetValues(type) ?? throw new Exception()).Cast<object>().ToArray();
                        for (int i = 0; i < names.Length; i++)
                        {
                            string? name = names[i];
                            object underlyingValue = Convert.ChangeType(values[i], Enum.GetUnderlyingType(values[i].GetType()));
                            w.WriteLine(name + " = " + underlyingValue + ",");
                        }
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