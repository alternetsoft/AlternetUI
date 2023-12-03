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

            Group([runButton, buildButton, buildUIButton, buildPalButton]).SuggestedWidthToMax();
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
                return [];

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
              $"dotnet {dotnetCmd} /p:Platform=x64 --arch x64 --property WarningLevel=0 --framework {targetFramework}";
            var cmdRunOther =
                $"dotnet {dotnetCmd} --property WarningLevel=0 --framework {targetFramework}";
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
                FileUtils.DeleteBinObjFiles(path);
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
            Application.Log("Build Alternet.UI.dll");
            Application.Log("Path: " + s);
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(s);
            BuildCsProjInFolder(s);
        }

        private void BuildPalButton_Click(object? sender, EventArgs e)
        {
            Application.Log("Build Alternet.UI.Pal");
            var s = CommonUtils.GetPalFolder();
            if (s is null)
            {
                buildPalButton.Enabled = false;
                return;
            }
            Application.Log("Path: " + s);
            if (DeleteBin)
                FileUtils.DeleteBinObjFiles(s);
        }

        public void ExecuteTerminalCommand(string command, string? folder = null)
        {
            void ExecuteOnWindows()
            {
                ProcessStartInfo processInfo;
                var cmdName = "cmd.exe";
                var cmdPrefix = "/c ";
                Application.Log("Run: " + cmdName + " " + cmdPrefix + command);
                processInfo = new(cmdName, cmdPrefix + command)
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                };
                if (folder != null)
                    processInfo.WorkingDirectory = folder;
                Process.Start(processInfo);
            }

            void ExecuteOnOther()
            {
                Process proc = new();
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                if (folder != null)
                    proc.StartInfo.WorkingDirectory = folder;
                proc.Start();
            }

            if (Application.IsWindowsOS)
                ExecuteOnWindows();
            else
                ExecuteOnOther();

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