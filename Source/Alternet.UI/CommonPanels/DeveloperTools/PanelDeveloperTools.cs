﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class PanelDeveloperTools : PanelAuiManager
    {
        private readonly IAuiNotebookPage? mainLogPage;

        private readonly LogListBox mainLogListBox = new()
        {
            HasBorder = false,
        };

        private ListBox? typesListBox;
        private IAuiNotebookPage? typesPage;
        private ListBox? controlsListBox;
        private IAuiNotebookPage? controlsPage;

        public PanelDeveloperTools()
            : base()
        {
            DefaultRightPaneBestSize = new(350, 200);
            DefaultRightPaneMinSize = new(350, 200);

            mainLogListBox.Parent = CenterNotebook;
            mainLogListBox.ContextMenu.Required();
            mainLogListBox.MenuItemShowDevTools?.SetEnabled(false);
            mainLogListBox.BindApplicationLog();
            mainLogPage = CenterNotebook.AddPage(
                mainLogListBox,
                CommonStrings.Default.NotebookTabTitleOutput);
            ActionsControl.Required();
            PropGrid.Required();

            PropGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            PropGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            PropGrid.ProcessException += PropertyGrid_ProcessException;
            PropGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;

            RightNotebook.ChangeSelection(0);
            ControlsListBox.Required();
            InitActions();
            DebugUtils.HookExceptionEvents();
            TypesListBox.SelectionChanged += TypesListBox_SelectionChanged;
            CenterNotebook.ChangeSelection(0);
            PropGrid.SuggestedInitDefaults();
        }

        internal IAuiNotebookPage? MainLogPage => mainLogPage;

        internal IAuiNotebookPage? TypesPage => typesPage;

        internal IAuiNotebookPage? ControlsPage => controlsPage;

        internal object? LastFocusedControl { get; set; }

        [Browsable(false)]
        internal ListBox TypesListBox
        {
            get
            {
                if (typesListBox == null)
                {
                    typesListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));

                    void AddControl(Type type)
                    {
                        typesListBox.Add(type.Name, type);
                    }

                    AddControl(typeof(Control));

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(Control).Assembly)
                            continue;
                        if (!AssemblyUtils.HasOwnEvents(type))
                            continue;
                        AddControl(type);
                    }

                    typesPage = CenterNotebook.AddPage(
                        typesListBox,
                        "Types");
                }

                return typesListBox;
            }
        }

        [Browsable(false)]
        internal ListBox ControlsListBox
        {
            get
            {
                if (controlsListBox == null)
                {
                    controlsListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    controlsListBox.Add("FirstWindow (no auto refresh)", ControlsActionMainForm);
                    controlsListBox.Add("GetFocusedControl (no auto refresh)", ControlsActionFocusedControl);
                    controlsListBox.Add("Clear", ControlsActionEmpty);
                    controlsListBox.SelectionChanged += ControlsListBox_SelectionChanged;

                    controlsPage = CenterNotebook.AddPage(controlsListBox, "Controls");
                }

                return controlsListBox;
            }
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            controlsListBox?.SelectedAction?.Invoke();
        }

        private void ControlsActionEmpty()
        {
            PropGridSetProps(null);
        }

        private void PropertyGrid_ProcessException(object? sender, PropertyGridExceptionEventArgs e)
        {
            Application.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void PropGridSetProps(object? instance)
        {
            RightNotebook.ChangeSelection(PropGrid);
            PropGrid.DoInsideUpdate(() =>
            {
                PropGrid.Clear();
                if (instance is null)
                    return;
                PropGrid.AddConstItem("(type)", null, instance.GetType().Name);
                PropGrid.AddProps(instance, null, true);
            });
        }

        private void ControlsActionMainForm()
        {
            PropGridSetProps(Application.FirstWindow());
        }

        private void ControlsActionFocusedControl()
        {
            PropGridSetProps(LastFocusedControl);
        }

        private static void LogOSInformation()
        {
            var os = Environment.OSVersion;
            Application.Log("Current OS Information:\n");
            Application.Log($"Platform: {os.Platform:G}");
            Application.Log($"Version String: {os.VersionString}");
            Application.Log($"Major version: {os.Version.Major}");
            Application.Log($"Minor version: {os.Version.Minor}");
            Application.Log($"Service Pack: '{os.ServicePack}'");
        }

        private void TypesListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = TypesListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            EventLogManager.UpdateEventsPropertyGrid(PropGrid, type);
        }

        private void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        private void InitActions()
        {
            AddAction("Log system settings", SystemSettings.Log);
            AddAction("Log font families", LogUtils.LogFontFamilies);
            AddAction("Log system fonts", SystemSettings.LogSystemFonts);
            AddAction("Log display info", Display.Log);
            AddAction("HookExceptionEvents()", DebugUtils.HookExceptionEvents);
            AddAction("Log useful defines", LogUsefulDefines);
            AddAction("Log OS information", LogOSInformation);
            AddAction("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); });
        }
    }
}