#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;

namespace ObjectReference
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private IScriptEdit edit;

        public MainForm()
        {
            InitializeComponent();
        }

        private IScriptEdit CreateEditor(string fileName, Control parent)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName);

            return edit;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Control parent)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, parent);
        }

        private void RegisterScriptCodeForEditor()
        {
            edit.RegisterCode("global.cs", scriptRun.ScriptHost.GlobalCode);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateEditor(pnEdit);

            cbLanguages.SelectedIndex = 0;
        }

        private void AddScriptItem()
        {
            ScriptGlobalItem item = new ScriptGlobalItem("RunButton", typeof(System.Windows.Forms.Button), btNETFromScript);
            ScriptGlobalItem item2 = new ScriptGlobalItem("MainForm", typeof(System.Windows.Forms.Form), this);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            scriptRun.GlobalItems.Add(item2);
            RegisterScriptCodeForEditor();
        }

        private void StartScript()
        {
            scriptRun.ScriptSource.FromScriptCode(edit.Text);

            scriptRun.ScriptSource.SearchPaths.Add(Application.StartupPath);

            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.ScriptSource.References.Add("netstandard");
            scriptRun.ScriptSource.References.Add("Microsoft.Bcl.AsyncInterfaces");
            scriptRun.ScriptSource.References.Add("Autofac");
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }
            }

            scriptRun.Run();
        }

        private void NETFromScriptButton_Click(object sender, EventArgs e)
        {
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
                StartScript();
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "AutoFac.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReference.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"..\..\..\..\Resources\Scripter\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
            AddScriptItem();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }
    }
}
