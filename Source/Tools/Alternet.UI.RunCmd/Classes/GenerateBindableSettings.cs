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

            XmlUtils.SerializeToFile(@"e:\GenerateBindableSettings.xml", settings);
        }
    }
}




