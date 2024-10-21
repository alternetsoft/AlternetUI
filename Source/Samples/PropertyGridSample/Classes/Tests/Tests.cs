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

        void LoadPngFromResource(TreeView? control)
        {
            if (control is null)
                return;

            const string highDpiSuffix = "_HighDpi";

            string[] resNames =
            [
                "ClassAlpha",
                "ConstantAlpha",
                "DelegateAlpha",
                "EventAlpha",
                "FieldAlpha",
                "GenericParameterAlpha",
                "InterfaceAlpha",
                "KeywordAlpha",
                "LocalOrParameterAlpha",
                "MethodAlpha",
                "NamespaceAlpha",
                "PropertyAlpha",
                "StructAlpha",
            ];

            string pathPrefix = "Resources.CodeComletionSymbols.";

            int size = control.HasScaleFactor ? 32 : 16;

            ImageList imgList = new();
            imgList.ImageSize = size;

            control.RemoveAll();
            control.ImageList = imgList;
            int index = 0;

            foreach (var s in resNames)
            {
                var resNameHighDpi = $"{pathPrefix}{s}{highDpiSuffix}.png";
                var resName = $"{pathPrefix}{s}.png";
                var selectedName = HasScaleFactor ? resNameHighDpi : resName;

                if (!imgList.AddFromAssemblyUrl(typeof(ObjectInit).Assembly, selectedName))
                    continue;
                
                TreeViewItem item = new(Path.GetFileName(selectedName), index);
                control.Add(item);
                index++;
            }
        }

        void LoadAllPngInFolder(TreeView? control)
        {
            if (control is null)
                return;

            var dialog = SelectDirectoryDialog.Default;

            dialog.ShowAsync(() =>
            {
                int size = 16;
                if (control.HasScaleFactor)
                    size = 32;

                ImageList imgList = new();
                imgList.ImageSize = size;

                control.RemoveAll();
                control.ImageList = imgList;
                int index = 0;

                var folder = dialog.DirectoryName;
                var files = Directory.GetFiles(folder, "*.png");

                Array.Sort(files);

                foreach (var file in files)
                {
                    var image = new Bitmap(file);
                    if (image.Size != (size, size))
                        continue;
                    if (!imgList.Add(image))
                        continue;

                    TreeViewItem item = new(Path.GetFileName(file), index);
                    control.Add(item);
                    index++;
                }
            });
        }

        void LoadAllSvgInFolder(TreeView? control)
        {
            if (control is null)
                return;

            var dialog = SelectDirectoryDialog.Default;

            dialog.ShowAsync(() =>
            {
                int size = 32;
                ImageList imgList = new();
                imgList.ImageSize = size;

                control.RemoveAll();
                control.ImageList = imgList;
                int index = 0;

                var folder = dialog.DirectoryName;
                var files = Directory.GetFiles(folder, "*.svg");

                Array.Sort(files);

                foreach (var file in files)
                {
                    var svg = new MonoSvgImage(file);
                    imgList.AddSvg(svg, control.IsDarkBackground);

                    TreeViewItem item = new(Path.GetFileName(file), index);
                    control.Add(item);
                    index++;
                }
            });
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

        void InitToolBarActions()
        {
            PropertyGrid.AddSimpleAction<ToolBar>("Test Visible", TestGenericToolBarVisible);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Enabled", TestGenericToolBarEnabled);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Delete", TestGenericToolBarDelete);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Sticky", TestGenericToolBarSticky);
            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Foreground Color",
                TestGenericToolBarForegroundColor);
            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Background Color",
                TestGenericToolBarBackgroundColor);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Font", TestGenericToolBarFont);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Background", TestGenericToolBarBackground);
            PropertyGrid.AddSimpleAction<ToolBar>("Reset Background", TestGenericToolBarResetBackground);
            PropertyGrid.AddSimpleAction<ToolBar>("Clear", TestGenericToolBarClear);
            PropertyGrid.AddSimpleAction<ToolBar>("Add OK button", TestGenericToolBarAddOk);
            PropertyGrid.AddSimpleAction<ToolBar>("Add Cancel button", TestGenericToolBarAddCancel);
            PropertyGrid.AddSimpleAction<ToolBar>("ReInit", TestGenericToolBarReInit);
        }

        void InitTextBoxActions()
        {
            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("Change SelectedText", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;

                TextFromUserParams prm = new();
                prm.OnApply = (s) =>
                {
                    control.SelectedText = s ?? string.Empty;
                };

                DialogFactory.GetTextFromUserAsync(prm);
            });
        }

        void SetToolTipImage(PictureBox control)
        {
            var template = new TemplateControls.RichToolTip<GenericLabel>();

            template.DoInsideLayout(() =>
            {
                template.Parent = control;

                try
                {
                    template.BackgroundColor = RichToolTip.DefaultToolTipBackgroundColor;
                    template.ForegroundColor = RichToolTip.DefaultToolTipForegroundColor;

                    template.TitleLabel.Text = "This is title";
                    template.TitleLabel.ParentForeColor = false;
                    template.TitleLabel.ParentFont = false;
                    template.TitleLabel.Font = template.RealFont.Scaled(1.5);
                    template.TitleLabel.ForegroundColor = RichToolTip.DefaultToolTipTitleForegroundColor;

                    template.MessageLabel.Text = "This is message text";

                    var sizeInPixels
                    = GraphicsFactory.PixelFromDip(RichToolTip.DefaultMinImageSize, control.ScaleFactor);

                    template.PictureBox
                    .SetIcon(MessageBoxIcon.Warning, sizeInPixels);

                    var image = TemplateUtils.GetTemplateAsImage(template);
                    control.ImageSet = null;
                    control.Image = image;

                    control.SuggestedSize = image.Size.PixelToDip(control.ScaleFactor);
                }
                finally
                {
                    template.Parent = null;
                }
            });

        }

        void SetMessageBoxIconError(PictureBox control)
        {
            control.SetIcon(MessageBoxIcon.Error, 64);
        }

        [Conditional("DEBUG")]
        void InitSimpleTestActions()
        {
            if (alreadyRegistered)
                return;
            alreadyRegistered = true;

            InitToolBarActions();
            InitTextBoxActions();

            AddControlAction<PictureBox>("Set MessageBoxIcon.Error", SetMessageBoxIconError);
            AddControlAction<PictureBox>("Set ToolTip image", SetToolTipImage);

            AddControlAction<TreeView>("Load png from resources", LoadPngFromResource);
            AddControlAction<TreeView>("Load all small *.png in folder...", LoadAllPngInFolder);
            AddControlAction<TreeView>("Load all *.svg in folder...", LoadAllSvgInFolder);

            AddControlAction<VirtualListBox>("Set dark theme", (c) =>
            {
                c.SetColorThemeToDark();
            });

            AddControlAction<VirtualListBox>("Reset theme", (c) =>
            {
                c.SetColorThemeToDefault();
            });

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

        internal void TestMemoFind()
        {
            TestMemoFindReplace(false);
        }

        internal void TestMemoReplace()
        {
            TestMemoFindReplace(true);
        }

        void TestRichFindReplace(bool replace)
        {
            var control = GetSelectedControl<RichTextBox>();
            if (control is null)
                return;

        }

        internal void TestRichFind()
        {
            TestRichFindReplace(false);
        }

        internal void TestRichReplace()
        {
            TestRichFindReplace(true);
        }

        void TestGenericToolBarFont()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Font = AbstractControl.DefaultFont.Scaled(2);
        }

        void TestGenericToolBarForegroundColor()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.ForegroundColor = Color.Red;
        }

        void TestGenericToolBarBackgroundColor()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = Color.DarkOliveGreen;
        }

        void TestGenericToolBarBackground()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = Color.RebeccaPurple.AsBrush;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarClear()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.DeleteAll();
        }

        void TestGenericToolBarAddOk()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.OK);
        }

        void TestGenericToolBarAddCancel()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.Cancel);
        }

        void TestGenericToolBarReInit()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.DeleteAll(false);
            ObjectInit.InitGenericToolBar(control);
        }


        void TestGenericToolBarResetBackground()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarVisible()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolVisible(childId);
            value = !value;
            control.SetToolVisible(childId, value);
        }

        void TestGenericToolBarEnabled()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var enabled = control.GetToolEnabled(childId);
            enabled = !enabled;
            control.SetToolEnabled(childId, enabled);
        }

        void TestGenericToolBarSticky()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolSticky(childId);
            value = !value;
            control.SetToolSticky(childId, value);
        }

        void TestGenericToolBarDelete()
        {
            var control = GetSelectedControl<ToolBar>();
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
            if (sender == parentParent && e.RightButton == MouseButtonState.Pressed)
                UpdatePropertyGrid(controlPanelBorder);
        }

        private void LogEvent(string name, bool logAlways = false)
        {
            var propValue = PropGrid.EventPropValue;
            if (propValue is Color color)
                propValue = color.ToDebugString();
            propValue ??= "NULL";
            string propName = PropGrid.EventPropName;
            string s = $"Event: {name}. {propName} = {propValue}";

            if (logAlways)
                App.Log(s);
            else
                App.LogReplace(s, s);
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
