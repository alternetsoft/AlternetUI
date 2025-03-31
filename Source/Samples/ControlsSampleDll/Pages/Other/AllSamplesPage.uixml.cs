using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using System.IO;
using System.Diagnostics;

namespace ControlsSample
{
    public partial class AllSamplesPage : Panel
    {
        public AllSamplesPage()
        {
            InitializeComponent();

            try
            {
                AddDefaultItems(null, EnumerateSamples());
                AddDefaultItems("Editor", EnumerateEditorSamples());
            }
            catch
            {
            }

            RunWhenIdle(view.SelectFirstItem);
            Group(runButton, buildButton, buildUIButton, buildPalButton).SuggestedWidthToMax();
            deleteBinCheckBox.BindBoolProp(this, nameof(DeleteBin));
        }

        public bool DeleteBin { get; set; }

        internal static IEnumerable<string> EnumProjectsOld(string path)
        {
            var result = Directory.EnumerateFiles(
                path,
                "*.csproj",
                SearchOption.AllDirectories);
            return result;
        }

        protected virtual string? GetSamplesFolder()
        {
            return CommonUtils.GetSamplesFolder();
        }

        private IEnumerable<string> EnumerateEditorSamples()
        {
            var editorSamplesFolder = Path.Combine(
                CommonUtils.GetSamplesFolder() ?? string.Empty,
                "../../../AlternetStudio/Demo/Editor.AlternetUI");

            editorSamplesFolder = Path.GetFullPath(editorSamplesFolder);

            if (Directory.Exists(editorSamplesFolder))
            {
                var f2 = EnumerateSamples(editorSamplesFolder, false, SearchOption.AllDirectories);
                return f2;
            }

            return Array.Empty<string>();
        }

        private IEnumerable<string> EnumerateSamples()
        {
            return EnumerateSamples(GetSamplesFolder());
        }

        private IEnumerable<string> EnumerateSamples(
            string? samplesFolder,
            bool removeMisc = true,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (samplesFolder is null)
                return Array.Empty<string>();

            List<string> csproj = new();

            IEnumerable<string> EnumProjectsFast(string path)
            {
                var dirs = Directory.EnumerateDirectories(
                    path,
                    "*",
                    searchOption);

                List<string> result = new();

                foreach (var dir in dirs)
                {
                    var name = Path.GetFileName(dir);
                    var csproj = PathUtils.AddDirectorySeparatorChar(dir) + name + ".csproj";

                    if(File.Exists(csproj))
                        result.Add(csproj);
                }

                return result;
            }

            var files1 = EnumProjectsFast(samplesFolder);

            csproj.AddRange(files1);

            if(removeMisc)
            {
                for (int i = csproj.Count - 1; i >= 0; i--)
                {
                    if (csproj[i].EndsWith("/ControlsSample.csproj") ||
                        csproj[i].EndsWith(@"\ControlsSample.csproj"))
                    {
                        csproj.RemoveAt(i);
                        continue;
                    }
                    if (csproj[i].EndsWith("Sample.csproj") || csproj[i].EndsWith("Test.csproj"))
                        continue;
                    csproj.RemoveAt(i);
                }
            }

            var csproj2 = csproj.Distinct();
            return csproj2;
        }

        private void AddDefaultItems(string? section, IEnumerable<string> samples)
        {
            int index = 1;
            foreach (string s in samples)
            {
                CsProjItem item = new(section, s, 0, index);
                index++;
                view.Items.Add(item);
            }
        }

        private class CsProjItem : ListControlItem
        {
            public CsProjItem(string? section, string path, int imageIndex, int index)
            {
                CsProjPath = path;

                if (section is not null)
                    section += ".";

                Text = section + Path.GetFileNameWithoutExtension(path);
            }

            public string CsProjPath { get; set; }
        }

        private void RunCsProjInFolder(string folder)
        {
            RunDotNetOnCsProjInFolder(folder, "run");
        }

        private void RunDotNetOnCsProjInFolder(string folder, string dotnetCmd = "run")
        {
            var targetFramework = AppUtils.GetMyTargetFrameworkName();
            var cmdRun =
                $"dotnet {dotnetCmd} --property WarningLevel=0 --framework {targetFramework}";
            AppUtils.ExecuteTerminalCommand(cmdRun, folder);
        }

        private void BuildCsProjInFolder(string folder)
        {
            RunDotNetOnCsProjInFolder(folder, "build");
        }

        private void RunButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not CsProjItem item)
                return;
            string path = item.CsProjPath;
            path = Path.GetDirectoryName(path)!;
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(path);
            App.Log("Run sample: " + path);
            RunCsProjInFolder(path);
        }

        private void BuildButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not CsProjItem item)
                return;
            string path = item.CsProjPath;
            App.Log("Build sample: " + path);
            path = Path.GetDirectoryName(path)!;
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(path);
            BuildCsProjInFolder(path);
        }

        private void BuildUIButton_Click(object? sender, EventArgs e)
        {
            var s = CommonUtils.GetUIFolder();
            if (s is null)
            {
                buildUIButton.Enabled = false;
                return;
            }
            App.Log("Build Alternet.UI.dll");
            App.Log("Path: " + s);
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(s);
            BuildCsProjInFolder(s);
        }

        private void BuildPalButton_Click(object? sender, EventArgs e)
        {
            App.Log("Build Alternet.UI.Pal");
            var s = CommonUtils.GetPalFolder();
            if (s is null)
            {
                buildPalButton.Enabled = false;
                return;
            }
            App.Log("Path: " + s);
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(s);
        }

        /*internal static bool RunSample(
            string filePath,
            string args,
            string? folder = null)
        {
            Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = args;
            if (folder != null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }*/

        private void RunAllButton_Click(object? sender, EventArgs e)
        {
        }

        private void BuildAllButton_Click(object? sender, EventArgs e)
        {
        }
    }
}