using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class RezipCommandSettings
    {
        public BaseCollection<RezipCommandSetting> Items { get; } = new();

        public string? SettingsFolder { get; set; }

        public static void TestSave()
        {
            RezipCommandSettings settings = new();

            RezipCommandSetting setting = new();

            setting.Ignore = true;
            setting.RootFolder = @"C:\Path\To\RootFolder";
            setting.PathToZip = @"C:\Path\To\ZipFile.zip";
            setting.PathToResult = @"C:\Path\To\ResultFile.zip";
            setting.FileMasks = "*.txt";
            setting.CommandToRun = "echo";
            setting.CommandArgs = "Hello, World!";
            setting.RecreateZip = true;

            settings.Items.Add(setting);

            XmlUtils.SerializeToFile(@"e:\RezipCommandSettings.xml", settings);
        }
    }
}
