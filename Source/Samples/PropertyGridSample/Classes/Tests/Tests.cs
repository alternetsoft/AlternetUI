﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        private static bool alreadyRegistered;

        public Collection<string> ItemsString { get; set; } = NewCollection<string>();
        public Collection<object> ItemsObject { get; set; } = NewCollection<object>();
        public Brush BrushValue { get; set; } = Brush.Default;
        public Pen PenValue { get; set; } = Pen.Default;

        internal void RunTests()
        {
        }

        void AddControlAction<T>(string title, Action<T> action)
            where T : AbstractControl
        {
            PropertyGrid.AddSimpleAction<T>(title, () =>
            {
                var selectedControl = GetSelectedControl<T>();
                if (selectedControl is null)
                    return;
                action(selectedControl);
            });
        }

        [Conditional("DEBUG")]
        void InitTestsAll()
        {
            if (alreadyRegistered)
                return;
            alreadyRegistered = true;

            InitTestsToolBar();
            InitTestsTextBoxAndButton();
            InitTestsTextBox();
            InitTestsPictureBox();
            InitTestsListBox();
            InitTestsTreeView();
            InitTestsListView();
            InitTestsButton();
            InitTestsSpeedButton();
            InitTestsTabControl();

            void InitTestsTabControl()
            {
                AddControlAction<TabControl>("Enum loaded pages", (c) =>
                {
                    LogUtils.LogRange(c.LoadedPages.Select(x => x.GetType()));
                });
            }

            void InitTestsButton()
            {
                AddControlAction<Button>("Set Command", (c) =>
                {
                    c.Command = NamedCommands.CommandAppLog;
                    c.CommandParameter = "Button.Command executed";
                });
            }

            void InitTestsSpeedButton()
            {
                AddControlAction<SpeedButton>("Set Command", (c) =>
                {
                    c.Command = NamedCommands.CommandAppLog;
                    c.CommandParameter = "SpeedButton.Command executed";
                });
            }

            void InitTestsListBox()
            {
                AddControlAction<VirtualListBox>("Add 5000 items", (c) =>
                {
                    ObjectInit.AddManyItems(c);
                });

                AddControlAction<VirtualListBox>("Set dark theme", (c) =>
                {
                    c.SetColorThemeToDark();
                });

                AddControlAction<VirtualListBox>("Reset theme", (c) =>
                {
                    c.SetColorThemeToDefault();
                });
            }

            PropertyGrid.AddSimpleAction<PanelOkCancelButtons>("Reorder buttons", ReorderButtonsTest);
        }

        void TestMemoFindReplace(bool replace)
        {
            var control = GetSelectedControl<MultilineTextBox>();
            if (control is null)
                return;
            var pair = FindReplaceControl.CreateInsideDialog(replace);
            pair.Dialog.Show();
        }

        internal void LogPropGridColors()
        {
            var color = panel.PropGrid.GetCurrentColors();
            (color as PropertyGridColors)?.LogToFile();
        }

        internal static IDataObject GetDataObject()
        {
            var result = new DataObject();
            result.SetData(DataFormats.Text, "Test data string.");
            return result;
        }

        private void ControlPanel_DragStart(object? sender, DragStartEventArgs e)
        {
/*

            if (e.DistanceIsLess)
                return;

            // if (e.TimeIsGreater)
            // {
            //     e.Cancel = true;
            //     return;
            // }

            e.DragStarted = true;
            var distance = MathUtils.GetDistance(e.MouseDownLocation, e.MouseClientLocation);
            Application.Log($"DragStart {e.MouseDownLocation} {e.MouseClientLocation} {!e.DistanceIsLess} {distance}");
            DoDragDrop(GetDataObject(), DragDropEffects.Copy | DragDropEffects.Move);
*/
        }

        private void ControlPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender == ControlParent && e.RightButton == MouseButtonState.Pressed)
                UpdatePropertyGrid(ControlParent);
        }

        private void LogEvent(string name, bool logAlways = false)
        {
            IPropertyGridItem? item = PropGrid.EventProperty;

            if(item is null
                || item.Instance is null || item.PropInfo is null)
            {
                var propValue = PropGrid.EventPropValue;
                if (propValue is Color color)
                    propValue = color.ToDebugString();
                propValue ??= "NULL";
                string propName = PropGrid.EventPropName;
                string s = $"Event: [{name}] {propName} = {propValue}";

                if (logAlways)
                    App.Log(s);
                else
                    App.LogReplace(s, s);
            }
            else
            {
                var value = item.PropInfo.GetValue(item.Instance);
                var type = item.Instance.GetType().Name;
                string s = $"Event: [{name}] {type}.{item.PropInfo.Name} = {value}";

                if (logAlways)
                    App.Log(s);
                else
                    App.LogReplace(s, s);
            }
        }

        private void PGPropertySelected(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertySelected)
                LogEvent("PropertySelected");
        }

        private void PGPropertyChanged(object? sender, EventArgs e)
        {
            ControlParent.Invalidate();
            if (PropertyGridSettings.Default!.LogPropertyChanged)
                LogEvent("PropertyChanged");
        }

        private void PGPropertyChanging(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyChanging)
                LogEvent("PropertyChanging");
            if (PropGrid.EventPropName == "Error")
                e.Cancel = true;
        }

        private void PGPropertyHighlighted(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyHighlighted)
                LogEvent("PropertyHighlighted");
        }

        private void PGPropertyRightClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyRightClick)
                LogEvent("PropertyRightClick");
        }

        private void PGPropertyDoubleClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyDoubleClick)
                LogEvent("PropertyDoubleClick");
        }

        private void PGItemCollapsed(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogItemCollapsed)
                LogEvent("ItemCollapsed");
        }

        private void PGItemExpanded(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogItemExpanded)
                LogEvent("ItemExpanded");
        }

        private void PGLabelEditBegin(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogLabelEditBegin)
                LogEvent("LabelEditBegin");
        }

        private void PGLabelEditEnding(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogLabelEditEnding)
                LogEvent("LabelEditEnding");
        }

        private void PGColBeginDrag(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColBeginDrag)
                LogEvent("ColBeginDrag");
        }

        private void PGColDragging(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColDragging)
                LogEvent("ColDragging");
        }

        private void PGColEndDrag(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColEndDrag)
                LogEvent("ColEndDrag");
        }

        private void PropertyGrid_ButtonClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogButtonClick)
                LogEvent("ButtonClick", true);
        }

        internal static Collection<T> NewCollection<T>()
            where T : class
        {
            var result = new Collection<T>();
            result.AddRange(ObjectInit.GetTenItems().Cast<T>());
            return result;
        }

        internal static void TestColorVariant()
        {
            static Color Fn(Color color)
            {
                static void DebugWriteColor(string label, Color c)
                {
                    Debug.WriteLine(label+" " + c.NameAndARGBValue);
                }

                var variant = PropertyGrid.CreateVariant();
                variant.AsColor = color;
                var result = variant.AsColor;

                Debug.WriteLine("====");
                DebugWriteColor("Color", color);
                Debug.WriteLine("Variant ValueType: " + variant.ValueType);
                DebugWriteColor("Result", result);
                Debug.WriteLine("====");

                return result;
            }

            Fn(Color.Red);
            Fn(SystemColors.ButtonFace);
            Fn(Color.FloralWhite);            
        }

        internal static void TestIsNullableClass()
        {
#pragma warning disable
            var fontInfo = AssemblyUtils.GetPropInfo(WelcomeProps.Default, "AsFont")!.PropertyType;
            var fontNullableInfo =
                AssemblyUtils.GetPropInfo(NullableProps.Default, "AsFontN")!.PropertyType;
#pragma warning restore
        }

        internal void TestLong()
        {
            IPropertyGridVariant variant = PropertyGrid.CreateVariant();

            long minLong = long.MinValue;
            long maxLong = long.MaxValue;
            ulong minULong = ulong.MinValue;
            ulong maxULong = ulong.MaxValue;

            variant.AsLong = minLong;
            long minLong2 = variant.AsLong;

            variant.AsLong = maxLong;
            long maxLong2 = variant.AsLong;

            variant.AsULong = minULong;
            ulong minULong2 = variant.AsULong;

            variant.AsULong = maxULong;
            ulong maxULong2 = variant.AsULong;

            App.Log($"{minLong} - {minLong2}");
            App.Log($"{maxLong} - {maxLong2}");
            App.Log($"{minULong} - {minULong2}");
            App.Log($"{maxULong} - {maxULong2}");

            variant.AsBool = true;

            variant.AsLong = 150;

            variant.AsDateTime = DateTime.Now;

            variant.AsDouble = 18;

            variant.AsString = "hello";
        }
    }
}
