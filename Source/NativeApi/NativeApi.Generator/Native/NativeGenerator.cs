using ApiGenerator.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApiGenerator.Native
{
    internal static class NativeGenerator
    {
        public static void GenerateClasses(Paths paths, IEnumerable<Type> types)
        {
            var cApiIndexBuilder = new StringBuilder();
            foreach (var type in types)
            {
                GenerateCppScaffoldsIfNeeded(paths, type);
                GenerateCppApi(paths, type);
                GenerateCApi(paths, type, cApiIndexBuilder);
            }

            File.WriteAllText(Path.Combine(paths.NativeSourcePath, "Api.cpp"), cApiIndexBuilder.ToString());
        }

        public static void GenerateEnums(Paths paths, IEnumerable<Type> types)
        {
            File.WriteAllText(Path.Combine(paths.NativeApiSourcePath, "Enums.h"), CppEnumsGenerator.Generate(types));
        }

        private static void GenerateCppApi(Paths paths, Type type)
        {
            var code = CppApiGenerator.Generate(ApiTypeFactory.Create(type, ApiTypeCreationMode.NativeCppApi));
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
            var code = CApiGenerator.Generate(ApiTypeFactory.Create(type, ApiTypeCreationMode.NativeCApi));
            var fileName = TypeProvider.GetNativeName(type) + ".Api.h";
            File.WriteAllText(Path.Combine(paths.NativeApiSourcePath, fileName), code);
            indexBuilder.AppendLine($"#include \"Api/{fileName}\"");
        }
    }
}