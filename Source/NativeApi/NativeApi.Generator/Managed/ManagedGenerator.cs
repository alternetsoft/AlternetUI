using ApiGenerator.Api;
using System;
using System.Collections.Generic;
using System.IO;

namespace ApiGenerator.Managed
{
    internal static class ManagedGenerator
    {
        public static void WriteAllTextSmart(string path, string contents)
        {
            if (File.Exists(path))
            {
                string s = File.ReadAllText(path);
                if (s == contents)
                    return;
            }
            File.WriteAllText(path, contents);
        }
        public static void GenerateClasses(Paths paths, IEnumerable<Type> types)
        {
            Console.WriteLine("Generating managed client code...");

            var apiClassGenerator = new ManagedApiClassGenerator();
            foreach (var type in types)
            {
                var apiClass = apiClassGenerator.Generate(
                    ApiTypeFactory.Create(type, ApiTypeCreationMode.ManagedApiClass),
                    ApiTypeFactory.Create(type, ApiTypeCreationMode.ManagedPInvokeClass));
                WriteAllTextSmart(Path.Combine(paths.ManagedApiSourcePath, type.Name + ".cs"), apiClass);
            }
        }

        public static void GenerateManagedServerClasses(Paths paths, IEnumerable<Type> types)
        {
            Console.WriteLine("Generating managed server code...");

            var apiClassGenerator = new ManagedApiServerClassGenerator();
            foreach (var type in types)
            {
                var apiClass = apiClassGenerator.Generate(
                    ApiTypeFactory.Create(type, ApiTypeCreationMode.ManagedApiClass),
                    ApiTypeFactory.Create(type, ApiTypeCreationMode.ManagedPInvokeClass));
                WriteAllTextSmart(Path.Combine(paths.ManagedServerApiSourcePath, type.Name + ".Generated.cs"), apiClass);
            }
        }

        public static void GenerateNativeEventDataTypes(Paths paths, IEnumerable<Type> types)
        {
            WriteAllTextSmart(Path.Combine(paths.ManagedApiSourcePath, "NativeEventDataTypes.cs"), ManagedNativeEventDataTypesGenerator.Generate(types));
        }

        public static void GenerateEnums(Paths paths, IEnumerable<Type> types)
        {
            WriteAllTextSmart(Path.Combine(paths.ManagedApiSourcePath, "Enums.cs"), ManagedEnumsGenerator.Generate(types));
        }
    }
}