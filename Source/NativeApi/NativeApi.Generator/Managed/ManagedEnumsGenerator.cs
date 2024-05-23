﻿using ApiGenerator.Api;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace ApiGenerator.Managed
{
    internal static class ManagedEnumsGenerator
    {
        public static string Generate(IEnumerable<Type> types)
        {
            var codeWriter = new StringWriter();
            var w = new IndentedTextWriter(codeWriter);

            w.WriteLine(GeneratorUtils.HeaderText);

            w.WriteLine();
            w.WriteLine("namespace Alternet.UI.Native");
            using (new BlockIndent(w, noNewlineAtEnd: true))
            {
                foreach (var type in types)
                {
                    var typeName = TypeProvider.GetNativeName(type);
                    w.WriteLine($"enum {typeName}");
                    using (new BlockIndent(w))
                    {
                        foreach (var (Name, Value) in MemberProvider.GetEnumNamesAndValues(type))
                            w.WriteLine($"{Name} = {Value},");
                    }
                    w.WriteLine();
                }
            }

            return codeWriter.ToString();
        }
    }
}