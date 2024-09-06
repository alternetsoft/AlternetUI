using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    public class GenerateBindableSettings
    {
        public Collection<GenerateBindableSetting> Items { get; } = new();

        public Collection<TypeNameAlias> TypeAliases { get; } = new();

        public void AddTypeAlias(string? typeName, string changeTo)
        {
            if (typeName == null)
                return;

            var item = new TypeNameAlias();
            item.TypeName = typeName;
            item.ChangeTo = changeTo;
            TypeAliases.Add(item);
        }

        public static void TestSave()
        {
            GenerateBindableSettings settings = new();

            GenerateBindableSetting setting = new();

            setting.RootFolder = @"..\..\..\..\..\..\..\AlternetStudio\Source";
            setting.PathToDll = @"$(RootFolder)\Editor\bin\Debug\net8.0\Alternet.Editor.AlterNetUI.v10.dll";
            setting.TypeName = "Alternet.Editor.SyntaxEdit";
            setting.PathToResult = @"$(RootFolder)\Editor.MAUI\Classes\SyntaxEditMaui.Generated.cs";
            setting.ResultTypeName = "Alternet.Editor.SyntaxEditMaui";

            settings.Items.Add(setting);

            settings.AddTypeAlias(typeof(byte).FullName, "byte");
            settings.AddTypeAlias(typeof(sbyte).FullName, "sbyte");
            settings.AddTypeAlias(typeof(short).FullName, "short");
            settings.AddTypeAlias(typeof(ushort).FullName, "ushort");
            settings.AddTypeAlias(typeof(int).FullName, "int");
            settings.AddTypeAlias(typeof(uint).FullName, "uint");
            settings.AddTypeAlias(typeof(long).FullName, "long");
            settings.AddTypeAlias(typeof(ulong).FullName, "ulong");
            settings.AddTypeAlias(typeof(string).FullName, "string");
            settings.AddTypeAlias(typeof(bool).FullName, "bool");
            settings.AddTypeAlias(typeof(double).FullName, "double");
            settings.AddTypeAlias(typeof(float).FullName, "float");
            settings.AddTypeAlias(typeof(char).FullName, "char");
            settings.AddTypeAlias(typeof(decimal).FullName, "decimal");
            settings.AddTypeAlias(typeof(nint).FullName, "nint");
            settings.AddTypeAlias(typeof(nuint).FullName, "nuint");

            XmlUtils.SerializeToFile(@"e:\GenerateBindableSettings.xml", settings);
        }
    }
}




