using System;
using System.IO;
using System.Reflection;

namespace Alternet.UI.Build.Tasks.ApiInfoCollector
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Alternet.UI.Build.Tasks.ApiInfoCollector.exe <input-assembly-file-path> <output-xml-file-path>");
                return 1;
            }

            try
            {
                var asmPath = Path.GetFullPath(args[0]);
                var outPath = Path.GetFullPath(args[1]);
                var asmFileName = Path.GetFileName(asmPath);

                Assembly asm;

                if (string.Compare(asmFileName, "Alternet.UI.Common.dll") == 0)
                {
                    asmPath = Path.Combine(PathUtils.GetAppFolder(), asmFileName);
                    asm = typeof(Alternet.UI.AbstractControl).Assembly;
                }
                else
                    asm = Assembly.LoadFrom(asmPath);

                var xmlDocument = ApiInfoGenerator.Generate(asm);
                
                var outPathDirectory = Path.GetDirectoryName(outPath);
                if (!Directory.Exists(outPathDirectory))
                    Directory.CreateDirectory(outPathDirectory!);
                xmlDocument.Save(outPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception: " + e);
                return 1;
            }

            return 0;
        }
    }
}