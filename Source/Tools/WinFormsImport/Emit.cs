using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace WinFormsImport
{
    internal static class Emit
    {
        public static List<string> BadTypes =
        [
            "User32",
        ];

        public static string Body = string.Empty;

        private static int indentLev = 0;

        public static void Import(Type type, string path)
        {
            if (type.IsNotPublic)
                return;

            if (type.Name.StartsWith("__"))
                return;

            if (BadTypes.IndexOf(type.Name) >= 0)
                return;

            Console.WriteLine(type.Name);

            Body = string.Empty;

            StartNameSpace(type);
            StartClass(type);

            EchoProps(type);
            EchoMethods(type);
            EchoEvents(type);

            EndClass(type);
            EndNameSpace(type);

            string filePath = path + "Imported." + type.Name + ".cs";

            File.WriteAllText(filePath, Emit.Body);
        }

        public static void WriteLine(string? s = null)
        {
            Echo((s ?? string.Empty) + Environment.NewLine);
        }

        public static void Echo(string s)
        {
            string prefix = string.Empty;

            for (int i = 0; i < indentLev; i++)
                prefix += "    ";

            Body += prefix + s;
        }

        public static void BeginIndent()
        {
            indentLev++;
        }

        public static void EndIndent()
        {
            indentLev--;
        }

        public static void StartNameSpace(Type type)
        {
            var namesp = type.Namespace;

            WriteLine("#pragma warning disable");
            WriteLine($"namespace Imported.{namesp}");
            WriteLine("{");
            BeginIndent();
        }

        public static void EndNameSpace(Type type)
        {
            EndIndent();
            WriteLine("}");
        }

        public static void StartClass(Type type)
        {
            var classKind = type.IsClass ? "class" : "struct";

            WriteLine("/// <summary>");
            WriteLine("/// This is imported class.");
            WriteLine("/// </summary>");

            WriteLine($"public {classKind} {type.Name}");
            WriteLine("{");
            BeginIndent();
        }

        public static void EndClass(Type type)
        {
            EndIndent();
            WriteLine("}");
        }

        public static void ImportAssembly(Assembly assembly, string path)
        {
            var types = assembly.DefinedTypes;

            foreach (var type in types)
            {
                Emit.Import(type, path);
            }
        }

        private static void EchoProps(Type type)
        {
            WriteLine("/*");
            WriteLine("Properties:");
            WriteLine();

            WriteLine("*/");
        }

        private static void EchoMethods(Type type)
        {
            WriteLine("/*");
            WriteLine("Methods:");
            WriteLine();

            WriteLine("*/");
        }

        private static void EchoEvents(Type type)
        {
            WriteLine("/*");
            WriteLine("Events:");
            WriteLine();

            var events = AssemblyUtils.EnumEvents(
                type,
                true,
                bindingFlags: BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var item in events)
            {
                WriteLine(item.Name);
            }

            WriteLine("*/");
        }
    }
}
