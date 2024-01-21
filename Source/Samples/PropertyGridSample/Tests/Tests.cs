using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        public Collection<string> ItemsString { get; set; } = NewCollection<string>();
        public Collection<object> ItemsObject { get; set; } = NewCollection<object>();
        public Brush BrushValue { get; set; } = Brush.Default;
        public Pen PenValue { get; set; } = Pen.Default;

#pragma warning disable
        internal void RunTests()
#pragma warning restore
        {
        }

        void InitSimpleTestActions()
        {
#if DEBUG
            PropertyGrid.AddSimpleAction<PictureBox>("Test", PictureBoxTest);
            PropertyGrid.AddSimpleAction<PanelOkCancelButtons>("Reorder buttons", ReorderButtonsTest);

            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Visible", TestGenericToolBarVisible);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Enabled", TestGenericToolBarEnabled);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Delete", TestGenericToolBarDelete);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Sticky", TestGenericToolBarSticky);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Foreground Color", TestGenericToolBarForegroundColor);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Background Color", TestGenericToolBarBackgroundColor);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Font", TestGenericToolBarFont);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Test Background", TestGenericToolBarBackground);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Reset Background", TestGenericToolBarResetBackground);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Clear", TestGenericToolBarClear);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Add OK button", TestGenericToolBarAddOk);
            PropertyGrid.AddSimpleAction<GenericToolBar>("Add Cancel button", TestGenericToolBarAddCancel);
            PropertyGrid.AddSimpleAction<GenericToolBar>("ReInit", TestGenericToolBarReInit);

            PropertyGrid.AddSimpleAction<RichTextBox>("Find", TestRichFind);
            PropertyGrid.AddSimpleAction<RichTextBox>("Replace", TestRichReplace);
            PropertyGrid.AddSimpleAction<MultilineTextBox>("Find", TestMemoFind);
            PropertyGrid.AddSimpleAction<MultilineTextBox>("Replace", TestMemoReplace);
#endif
        }

        void TestMemoFindReplace(bool replace)
        {
            /*->Replace(3, 8, "ABC");*/

            var control = GetSelectedControl<MultilineTextBox>();
            if (control is null)
                return;

            var parentWindow = new DialogWindow();
            parentWindow.Disposed += ParentWindow_Disposed;
            parentWindow.MinimizeEnabled = false;
            parentWindow.MaximizeEnabled = false;
            parentWindow.HasSystemMenu = false;
            parentWindow.Title = "Search and Replace";

            FindReplaceControl findReplace = new();
            findReplace.Manager = findReplace.CreateLogger();
            findReplace.CloseButtonVisible = false;
            findReplace.ReplaceVisible = replace;
            findReplace.Parent = parentWindow;
            
            parentWindow.SetSizeToContent(WindowSizeToContentMode.Height);
            parentWindow.ShowModal();

            void ParentWindow_Disposed(object? sender, EventArgs e)
            {
                Application.Log("FindReplace window disposed");
            }
        }

        void TestMemoFind()
        {
            TestMemoFindReplace(false);
        }

        void TestMemoReplace()
        {
            TestMemoFindReplace(true);
        }

        void TestRichFindReplace(bool replace)
        {
            var control = GetSelectedControl<RichTextBox>();
            if (control is null)
                return;

        }

        void TestRichFind()
        {
            TestRichFindReplace(false);
        }

        void TestRichReplace()
        {
            TestRichFindReplace(true);
        }

        void TestGenericToolBarFont()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.Font = Control.DefaultFont.Scaled(2);
        }

        void TestGenericToolBarForegroundColor()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.ForegroundColor = Color.Red;
        }

        void TestGenericToolBarBackgroundColor()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = Color.DarkOliveGreen;
        }

        void TestGenericToolBarBackground()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.Background = Color.RebeccaPurple.AsBrush;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarClear()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.DeleteAll();
        }

        void TestGenericToolBarAddOk()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.OK);
        }

        void TestGenericToolBarAddCancel()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.Cancel);
        }

        void TestGenericToolBarReInit()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.DeleteAll();
            ObjectInit.InitGenericToolBar(control);
        }


        void TestGenericToolBarResetBackground()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarVisible()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolVisible(childId);
            value = !value;
            control.SetToolVisible(childId, value);
        }

        void TestGenericToolBarEnabled()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var enabled = control.GetToolEnabled(childId);
            enabled = !enabled;
            control.SetToolEnabled(childId, enabled);
        }

        void TestGenericToolBarSticky()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolSticky(childId);
            value = !value;
            control.SetToolSticky(childId, value);
        }

        void TestGenericToolBarDelete()
        {
            var control = GetSelectedControl<GenericToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            control.DeleteTool(childId);
        }

        internal void LogPropGridColors()
        {
            var color = panel.PropGrid.GetCurrentColors();
            (color as PropertyGridColors)?.LogToFile();
        }

        private static IDataObject GetDataObject()
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

        private void ControlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Source == parentParent && e.RightButton == MouseButtonState.Pressed)
                UpdatePropertyGrid(controlPanelBorder);
        }

        private void LogEvent(string name, bool logAlways = false)
        {
            var propValue = PropGrid.EventPropValue;
            if (propValue is Color color)
                propValue = color.ToDebugString();
            propValue ??= "NULL";
            string propName = PropGrid.EventPropName;
            string s = $"Event: {name}. PropName: <{propName}>. Value: <{propValue}>";

            if (logAlways)
                Application.Log(s);
            else
                Application.LogReplace(s, s);
        }

        private void PGPropertySelected(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertySelected)
                LogEvent("PropertySelected");
        }

        private void PGPropertyChanged(object? sender, EventArgs e)
        {
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
        {
            var result = new Collection<T>();
            ObjectInit.AddTenItems(result);

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

            Application.Log($"{minLong} - {minLong2}");
            Application.Log($"{maxLong} - {maxLong2}");
            Application.Log($"{minULong} - {minULong2}");
            Application.Log($"{maxULong} - {maxULong2}");

            variant.AsBool = true;

            variant.AsLong = 150;

            variant.AsDateTime = DateTime.Now;

            variant.AsDouble = 18;

            variant.AsString = "hello";
        }
    }
}
