using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        public static readonly List<Type> LimitedTypesStatic = new();

        private readonly List<Type> LimitedTypes = new();

        private T? GetSelectedControl<T>()
        {
            if (ToolBox.SelectedItem is not ControlListBoxItem item)
                return default;
            if (item.Instance is T control)
                return control;
            return default;
        }

        void ReorderButtonsTest()
        {
            var control = GetSelectedControl<PanelOkCancelButtons>();
            if (control is null)
                return;
            control.SetChildIndex(control.CancelButton, 0);
            control.SetChildIndex(control.OkButton, -1);
        }

        private void InitToolBox()
        {
            InitTestsAll();

            void Fn()
            {
                List<Type> noTicks = new();

                if (App.IsMacOS)
                {
                    noTicks.Add(typeof(CheckBox));
                }

                bool logAddedControls = false;

                LimitedTypes.Add(typeof(Border));
                LimitedTypes.Add(typeof(PictureBox));
                LimitedTypes.Add(typeof(Button));
                LimitedTypes.Add(typeof(ToolBar));
                LimitedTypes.Add(typeof(SplittedPanel));
                LimitedTypes.Add(typeof(Calendar));
                LimitedTypes.Add(typeof(CheckBox));
                LimitedTypes.Add(typeof(ComboBox));
                LimitedTypes.Add(typeof(StdComboBox));
                LimitedTypes.Add(typeof(GroupBox));
                LimitedTypes.Add(typeof(HorizontalStackPanel));
                LimitedTypes.Add(typeof(TabControl));
                LimitedTypes.Add(typeof(LinkLabel));
                LimitedTypes.Add(typeof(StdListBox));
                LimitedTypes.Add(typeof(ListView));
                LimitedTypes.Add(typeof(MultilineTextBox));
                LimitedTypes.Add(typeof(NumericUpDown));
                LimitedTypes.Add(typeof(Panel));
                LimitedTypes.Add(typeof(PanelOkCancelButtons));
                LimitedTypes.Add(typeof(ProgressBar));
                LimitedTypes.Add(typeof(RadioButton));
                LimitedTypes.Add(typeof(RichTextBox));
                LimitedTypes.Add(typeof(FindReplaceControl));
                LimitedTypes.Add(typeof(StdSlider));
                LimitedTypes.Add(typeof(ScrollBar));
                LimitedTypes.Add(typeof(StackPanel));
                LimitedTypes.Add(typeof(TextBox));
                LimitedTypes.Add(typeof(VerticalStackPanel));
                LimitedTypes.Add(typeof(CardPanel));
                LimitedTypes.Add(typeof(SpeedButton));
                LimitedTypes.Add(typeof(TextBoxAndLabel));
                LimitedTypes.Add(typeof(SpeedTextButton));
                LimitedTypes.Add(typeof(SpeedColorButton));
                LimitedTypes.Add(typeof(ToolBarSet));
                LimitedTypes.Add(typeof(SideBarPanel));
                LimitedTypes.Add(typeof(ColorListBox));
                LimitedTypes.Add(typeof(VirtualListBox));
                LimitedTypes.Add(typeof(UserControl));
                LimitedTypes.Add(typeof(RichToolTip));
                LimitedTypes.Add(typeof(TextBoxAndButton));
                LimitedTypes.Add(typeof(FontNamePicker));
                LimitedTypes.Add(typeof(Calculator));
                LimitedTypes.Add(typeof(LabelAndButton));
                LimitedTypes.Add(typeof(IntPicker));
                LimitedTypes.Add(typeof(DatePicker));
                LimitedTypes.Add(typeof(DateTimePicker));
                LimitedTypes.Add(typeof(TimePicker));
                LimitedTypes.Add(typeof(ListPicker));
                LimitedTypes.Add(typeof(EnumPicker));
                LimitedTypes.Add(typeof(ColorPicker));
                LimitedTypes.Add(typeof(TextBoxWithListPopup));
                LimitedTypes.Add(typeof(CardPanelHeader));
                LimitedTypes.Add(typeof(TreeView));
                LimitedTypes.Add(typeof(StdTreeView));

                LimitedTypes.Add(typeof(Label));

                if (DebugUtils.IsDebugDefined)
                {
                    LimitedTypes.Add(typeof(GenericWrappedTextControl));
                }

                LimitedTypes.AddRange(LimitedTypesStatic);

                List<ControlListBoxItem> items = new();

                ControlListBoxItem item;

                foreach (Type type in LimitedTypes)
                {
                    item = new(type)
                    {
                        HasTicks = noTicks.IndexOf(type) < 0,
                        HasMargins = true,
                    };

                    if (logAddedControls)
                        App.Log($"typeof({type.FullName}),");
                    items.Add(item);
                }

                items.Add(CreateDialogItem<ColorDialog>());
                items.Add(CreateDialogItem<OpenFileDialog>());
                items.Add(CreateDialogItem<SaveFileDialog>());
                items.Add(CreateDialogItem<SelectDirectoryDialog>());
                items.Add(CreateDialogItem<FontDialog>());

                items.Add(CreateDialogItem<PageSetupDialog>());
                items.Add(CreateDialogItem<PrintPreviewDialog>());
                items.Add(CreateDialogItem<PrintDialog>());

                items.Sort();

                BaseDictionary<string, TreeViewItem> categories = new();

                var otherCat = new ControlCategoryAttribute("Other");

                foreach (var elem in items)
                {
                    var type = elem.InstanceType;
                    var categoryAttr = AssemblyUtils.GetControlCategory(type) ?? otherCat;

                    if(categoryAttr.IsHidden || categoryAttr.IsInternal)
                        continue;

                    var categoryTitle = categoryAttr.CategoryTitle;

                    if (!categories.TryGetValue(categoryTitle, out var categoryItem))
                    {
                        categoryItem = new TreeViewItem(categoryTitle);
                        categoryItem.HideSelection = true;
                        categoryItem.ExpandOnClick = true;
                        categoryItem.AutoCollapseSiblings = true;
                        categories[categoryTitle] = categoryItem;
                        ToolBox.Add(categoryItem);
                    }

                    categoryItem.Add(elem);

                }
            }

            ToolBox.DoInsideUpdate(() =>
            {
                Fn();

                ToolBox.RootItem.Sort();

                ControlListBoxItem item = new(typeof(WelcomePage))
                {
                    Text = "Welcome Page",
                };
                ToolBox.RootItem.Insert(0, item);

                item = new(typeof(SettingsControl))
                {
                    PropInstance = PropertyGridSettings.Default,
                    EventInstance = new object(),
                    Text = "Options",
                };
                ToolBox.Add(item);
            });
        }

        internal void AddMainWindow()
        {
            ToolBox.Add(new ControlListBoxItem(typeof(Window), this.ParentWindow));
        }

        private ControlListBoxItem CreateDialogItem<T>()
            where T : CommonDialog
        {
            var dialog = (T)ControlListBoxItem.CreateInstance(typeof(T))!;
            var button = new ShowDialogButton
            {
                Dialog = dialog,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = dialog,
                EventInstance = new object(),
                HasMargins = true,
            };
            return item;
        }

        internal void AddContextMenu<T>()
            where T : ContextMenu
        {
            var menu = (T)ControlListBoxItem.CreateInstance(typeof(T))!;

            var button = new ShowContextMenuButton
            {
                Menu = menu,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = menu,
                EventInstance = new object(),
            };
            ToolBox.Add(item);
        }
    }
}