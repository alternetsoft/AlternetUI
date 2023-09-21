using System;
using System.IO;
using ApiGenerator.Api;
using ApiGenerator.Native;
using ApiGenerator.Managed;

namespace ApiGenerator
{
    internal static class Program
    {
#pragma warning disable
        public static void Main(string[] args)
#pragma warning restore
        {
            bool UseNativeGenerator = true;

            var paths = new Paths(Path.GetFullPath(@"..\..\..\..\..\..\"));

            var classTypes = TypeProvider.GetClassTypes();
            ManagedGenerator.GenerateClasses(paths, classTypes);
            if(UseNativeGenerator)
                NativeGenerator.GenerateClasses(paths, classTypes);

            var enumTypes = TypeProvider.GetEnumTypes();
            if (UseNativeGenerator)
                NativeGenerator.GenerateEnums(paths, enumTypes);
            ManagedGenerator.GenerateEnums(paths, enumTypes);

            var nativeEventDataTypes = TypeProvider.GetNativeEventDataTypes();
            if (UseNativeGenerator)
                NativeGenerator.GenerateNativeEventDataTypes(paths, nativeEventDataTypes);
            ManagedGenerator.GenerateNativeEventDataTypes(paths, nativeEventDataTypes);

            var managedServerClassTypes = TypeProvider.GetManagedServerClassTypes();
            ManagedGenerator.GenerateManagedServerClasses(paths, managedServerClassTypes);
            if (UseNativeGenerator)
            {
                NativeGenerator.GenerateManagedServerClasses(
                    paths,
                    managedServerClassTypes);
            }

            Console.WriteLine("All done.");
        }
    }
}