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
    internal partial class AllSamplesPage : Control
    {
        public AllSamplesPage()
        {
            InitializeComponent();
            AddDefaultItems();
            view.SelectFirstItem();

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

        private IEnumerable<string> EnumerateSamples()
        {
            var samplesFolder = CommonUtils.GetSamplesFolder();
            if (samplesFolder is null)
                return Array.Empty<string>();

            List<string> csproj = new();

            static IEnumerable<string> EnumProjectsFast(string path)
            {
                var dirs = Directory.EnumerateDirectories(
                    path,
                    "*",
                    SearchOption.TopDirectoryOnly);

                List<string> result = new();

                foreach (var dir in dirs)
                {
                    var name = Path.GetFileName(dir);
                    var csproj = PathUtils.AddDirectorySeparatorChar(dir) + name + ".csproj";
                    result.Add(csproj);
                }

                return result;
            }

            var files1 = EnumProjectsFast(samplesFolder);

            csproj.AddRange(files1);

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

            var csproj2 = csproj.Distinct();
            return csproj2;
        }

        private void AddDefaultItems()
        {
            var samples = EnumerateSamples();

            int index = 1;
            foreach (string s in samples)
            {
                CsProjItem item = new(s, 0, index);
                index++;
                view.Items.Add(item);
            }
        }

        private class CsProjItem : ListControlItem
        {
            public CsProjItem(string path, int imageIndex, int index)
            {
                CsProjPath = path;
                Text = index + ". " + Path.GetFileNameWithoutExtension(path);
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
            var arch = App.Is64BitOS ? "x64" : "x86";
            var cmdRunWindows =
              $"dotnet {dotnetCmd} /p:Platform={arch} --arch {arch} --property WarningLevel=0 --framework {targetFramework}";
            var cmdRunOther =
                $"dotnet {dotnetCmd} --property WarningLevel=0 --framework {targetFramework}";
            var cmd = App.IsWindowsOS ? cmdRunWindows : cmdRunOther;
            AppUtils.ExecuteTerminalCommand(cmd, folder);
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