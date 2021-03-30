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
            var types = TypeProvider.GetTypes();

            //ManagedGenerator.Generate(paths, types);
            NativeGenerator.Generate(paths, types);
            Console.WriteLine("All done.");
        }
    }
}