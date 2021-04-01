using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiGenerator.Managed
{
    internal static class ManagedGenerator
    {
        public static void GenerateClasses(Paths paths, IEnumerable<Type> types)
        {
            Console.WriteLine("Generating managed code...");

            var apiClassGenerator = new ManagedApiClassGenerator();
            foreach (var type in types)
            {
                var apiClass = apiClassGenerator.Generate(type);
                File.WriteAllText(Path.Combine(paths.ManagedApiSourcePath, type.Name + ".cs"), apiClass);
            }
        }

        public static void GenerateEnums(Paths paths, IEnumerable<Type> types)
        {
            File.WriteAllText(Path.Combine(paths.ManagedApiSourcePath, "Enums.cs"), ManagedEnumsGenerator.Generate(types));
        }

    }
}