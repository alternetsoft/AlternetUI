using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using static Alternet.UI.ControlsSampleUtils;
using System.IO;
using System.Diagnostics;

namespace ControlsSample
{
    internal partial class AllSamplesPage : Control
    {
        private IPageSite? site;

        public AllSamplesPage()
        {
            InitializeComponent();

            view.MakeAsListBox();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
                AddDefaultItems();
            }
        }

        private void AddDefaultItems()
        {
            string folder1 = PathUtils.GetAppFolder()+@"../../../../";
            string folder2 = folder1+ @"../";
            folder1 = Path.GetFullPath(folder1);
            folder2 = Path.GetFullPath(folder2);

            List<string> csproj = new();

            static IEnumerable<string> EnumProjects(string path)
            {
                var result = Directory.EnumerateFiles(
                    path, 
                    "*.csproj", 
                    SearchOption.AllDirectories);
                return result;                
            }

            var files1 = EnumProjects(folder1);
            var files2 = EnumProjects(folder2);

            csproj.AddRange(files1);
            csproj.AddRange(files2);

            for (int i = csproj.Count-1; i >=0 ; i--)
            {
                if (csproj[i].EndsWith("/ControlsSample.csproj") ||
                    csproj[i].EndsWith(@"\ControlsSample.csproj"))
                {
                    csproj.RemoveAt(i);
                    continue;
                }
#if DEBUG
#else
                if(csproj[i].EndsWith("Sample.csproj"))
                    continue;
                csproj.RemoveAt(i);
#endif
            }

            var csproj2 = csproj.Distinct();
            int index = 1;
            foreach (string s in csproj2)
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

        private void RunButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not CsProjItem item)
                return;
            string path = item.CsProjPath;
            site?.LogEvent("Run sample: " + path);

            string runext = Application.IsWindowsOS ? "bat" : "sh";
            string runutil = PathUtils.GetAppFolder() + "runsample." + runext;
            path = Path.GetDirectoryName(path)!;

            RunSample(runutil,$"\"{path}\"");

        }

        private static bool RunSample(
            string filePath, 
            string args, 
            string? folder=null)
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

        private void BuildButton_Click(object? sender, EventArgs e)
        {
        }

        private void RunAllButton_Click(object? sender, EventArgs e)
        {
        }

        private void BuildAllButton_Click(object? sender, EventArgs e)
        {
        }
    }
}