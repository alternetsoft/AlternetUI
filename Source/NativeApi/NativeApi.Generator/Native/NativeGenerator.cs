﻿using ApiGenerator.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApiGenerator.Native
{
    internal static class NativeGenerator
    {
        public static void Generate(Paths paths, IEnumerable<Type> types)
        {
            Console.WriteLine("Generating native code...");

            var cApiIndexBuilder = new StringBuilder();
            foreach (var type in types)
            {
                GenerateCppScaffoldsIfNeeded(paths, type);
                GenerateCppApi(paths, type);
                GenerateCApi(paths, type, cApiIndexBuilder);
            }

            File.WriteAllText(Path.Combine(paths.NativeSourcePath, "Api.cpp"), cApiIndexBuilder.ToString());
        }

        private static void GenerateCppApi(Paths paths, Type type)
        {
            var code = CppApiGenerator.Generate(type);
            var fileName = TypeProvider.GetNativeName(type) + ".inc";
            File.WriteAllText(Path.Combine(paths.NativeApiSourcePath, fileName), code);
        }

        private static void GenerateCppScaffoldsIfNeeded(Paths paths, Type type)
        {
            var typeName = TypeProvider.GetNativeName(type);
            var headerFileName = Path.Combine(paths.NativeSourcePath, typeName + ".h");
            if (!File.Exists(headerFileName))
                File.WriteAllText(headerFileName, CppScaffoldGenerator.GenerateHeader(type));

            var sourceFileName = Path.Combine(paths.NativeSourcePath, typeName + ".cpp");
            if (!File.Exists(sourceFileName))
                File.WriteAllText(sourceFileName, CppScaffoldGenerator.GenerateSource(type));
        }

        private static void GenerateCApi(Paths paths, Type type, StringBuilder indexBuilder)
        {
            var code = CApiGenerator.Generate(type);
            var fileName = TypeProvider.GetNativeName(type) + ".Api.h";
            File.WriteAllText(Path.Combine(paths.NativeApiSourcePath, fileName), code);
            indexBuilder.AppendLine($"#include \"Api/{fileName}\"");
        }
    }
}