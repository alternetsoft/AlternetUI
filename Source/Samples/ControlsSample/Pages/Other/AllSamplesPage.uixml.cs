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
            view.MakeAsListBox();
            AddDefaultItems();
            view.SelectFirstItem();

            ControlSet.New([runButton, buildButton, buildUIButton, buildPalButton]).SuggestedWidthToMax();
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

        private string GetSamplesFolder()
        {
            string folder1 = PathUtils.GetAppFolder() + @"../../../../";
            string folder2 = folder1 + @"../";
            folder1 = Path.GetFullPath(folder1);
            folder2 = Path.GetFullPath(folder2);

            var trimmed = PathUtils.TrimEndDirectorySeparator(folder2);
            if (trimmed is not null && trimmed.EndsWith("Samples"))
                folder1 = folder2;
            return folder1;
        }

        private string GetPalFolder()
        {
            string samplesFolder = GetSamplesFolder();
            string relativePath = Path.Combine(samplesFolder, "..", "Alternet.UI.Pal");
            string result = Path.GetFullPath(relativePath);
            return result;
        }

        private string GetUIFolder()
        {
            string samplesFolder = GetSamplesFolder();
            string relativePath = Path.Combine(samplesFolder, "..", "Alternet.UI");
            string result = Path.GetFullPath(relativePath);
            return result;
        }

        private IEnumerable<string> EnumerateSamples()
        {
            var samplesFolder = GetSamplesFolder();

            List<string> csproj = [];

            static IEnumerable<string> EnumProjectsFast(string path)
            {
                var dirs = Directory.EnumerateDirectories(
                    path,
                    "*",
                    SearchOption.TopDirectoryOnly);

                List<string> result = [];

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

        private class CsProjItem : TreeViewItem
        {
            public CsProjItem(string path, int imageIndex, int index)
            {
                CsProjPath = path;
                Text = index + ". " + Path.GetFileNameWithoutExtension(path);
                ImageIndex = imageIndex;
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
            var cmdRunWindows =
              $"dotnet {dotnetCmd} /p:Platform=x64 --nologo --arch x64 --property WarningLevel=0 --framework {targetFramework}";
            var cmdRunOther =
                $"dotnet {dotnetCmd} --property --nologo WarningLevel=0 --framework {targetFramework}";
            var cmd = Application.IsWindowsOS ? cmdRunWindows : cmdRunOther;
            ExecuteTerminalCommand(cmd, folder);
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
                PathUtils.DeleteBinObjFiles(path);
            Application.Log("Run sample: " + path);
            RunCsProjInFolder(path);
        }

        private void BuildButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not CsProjItem item)
                return;
            string path = item.CsProjPath;
            Application.Log("Build sample: " + path);
            path = Path.GetDirectoryName(path)!;
            if (DeleteBin)
                PathUtils.DeleteBinObjFiles(path);
            BuildCsProjInFolder(path);
        }

        private void BuildUIButton_Click(object? sender, EventArgs e)
        {
            Application.Log("Build Alternet.UI.dll");
            var s = GetUIFolder();
            Application.Log("Path: " + s);
            if (DeleteBin)
                PathUtils.DeleteBinObjFiles(s);
            BuildCsProjInFolder(s);
        }

        private void BuildPalButton_Click(object? sender, EventArgs e)
        {
            Application.Log("Build Alternet.UI.Pal");
            var s = GetPalFolder();
            Application.Log("Path: " + s);
            if (DeleteBin)
                PathUtils.DeleteBinObjFiles(s);
        }

        public void ExecuteTerminalCommand(string command, string? folder = null)
        {
            ProcessStartInfo processInfo;
            var cmdName = Application.IsWindowsOS ? "cmd.exe" : "/bin/bash";
            var cmdPrefix = Application.IsWindowsOS ? "/c " : string.Empty;
            Application.Log("Run: "+cmdName+" "+ cmdPrefix + command);
            processInfo = new(cmdName, cmdPrefix + command)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
            };
            if (folder != null)
                processInfo.WorkingDirectory = folder;
            Process.Start(processInfo);
        }

        private static bool RunSample(
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
        }

        private void RunAllButton_Click(object? sender, EventArgs e)
        {
        }

        private void BuildAllButton_Click(object? sender, EventArgs e)
        {
        }
    }
}