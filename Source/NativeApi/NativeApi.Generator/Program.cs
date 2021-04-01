using System;
using System.IO;
using ApiGenerator.Api;
using ApiGenerator.Native;
using ApiGenerator.Managed;

namespace ApiGenerator
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var paths = new Paths(Path.GetFullPath(@"..\..\..\..\..\..\"));

            var classTypes = TypeProvider.GetClassTypes();
            ManagedGenerator.GenerateClasses(paths, classTypes);
            NativeGenerator.GenerateClasses(paths, classTypes);

            var enumTypes = TypeProvider.GetEnumTypes();
            NativeGenerator.GenerateEnums(paths, enumTypes);
            ManagedGenerator.GenerateEnums(paths, enumTypes);

            Console.WriteLine("All done.");
        }
    }
}