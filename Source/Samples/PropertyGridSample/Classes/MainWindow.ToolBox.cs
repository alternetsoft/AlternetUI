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
                bool logNotAddedControls = false;

                ToolBoxAdd<Border>();
                ToolBoxAdd<PictureBox>();
                ToolBoxAdd<Button>();
                ToolBoxAdd<ToolBar>();
                ToolBoxAdd<SplittedPanel>();
                ToolBoxAdd<Calendar>();
                ToolBoxAdd<CheckBox>();
                ToolBoxAdd<ComboBox>();
                ToolBoxAdd<StdComboBox>();
                ToolBoxAdd<GroupBox>();
                ToolBoxAdd<HorizontalStackPanel>();
                ToolBoxAdd<TabControl>();
                ToolBoxAdd<LinkLabel>();
                ToolBoxAdd<StdListBox>();
                ToolBoxAdd<ListView>();
                ToolBoxAdd<MultilineTextBox>();
                ToolBoxAdd<NumericUpDown>();
                ToolBoxAdd<Panel>();
                ToolBoxAdd<PanelOkCancelButtons>();
                ToolBoxAdd<ProgressBar>();
                ToolBoxAdd<RadioButton>();
                ToolBoxAdd<RichTextBox>();
                ToolBoxAdd<FindReplaceControl>();
                ToolBoxAdd<StdSlider>();
                ToolBoxAdd<ScrollBar>();
                ToolBoxAdd<StackPanel>();
                ToolBoxAdd<TextBox>();
                ToolBoxAdd<VerticalStackPanel>();
                ToolBoxAdd<CardPanel>();
                ToolBoxAdd<SpeedButton>();
                ToolBoxAdd<TextBoxAndLabel>();
                ToolBoxAdd<SpeedTextButton>();
                ToolBoxAdd<SpeedColorButton>();
                ToolBoxAdd<ToolBarSet>();
                ToolBoxAdd<SideBarPanel>();
                ToolBoxAdd<ColorListBox>();
                ToolBoxAdd<VirtualListBox>();
                ToolBoxAdd<UserControl>();
                ToolBoxAdd<RichToolTip>();
                ToolBoxAdd<TextBoxAndButton>();
                ToolBoxAdd<FontNamePicker>();
                ToolBoxAdd<Calculator>();
                ToolBoxAdd<LabelAndButton>();
                ToolBoxAdd<IntPicker>();
                ToolBoxAdd<DatePicker>();
                ToolBoxAdd<DateTimePicker>();
                ToolBoxAdd<TimePicker>();
                ToolBoxAdd<ListPicker>();
                ToolBoxAdd<EnumPicker>();
                ToolBoxAdd<ColorPicker>();
                ToolBoxAdd<TextBoxWithListPopup>();
                ToolBoxAdd<CardPanelHeader>();
                ToolBoxAdd<TreeView>();
                ToolBoxAdd<StdTreeView>();
                ToolBoxAdd<ListBox>();
                ToolBoxAdd<Label>();
                ToolBoxAdd<CheckedListBox>();
                ToolBoxAdd<FileListBox>();
                ToolBoxAdd<FontListBox>();

                void ToolBoxAdd<T>()
                {
                    LimitedTypes.Add(typeof(T));
                }

                /*
                This is commented out because Slider control doesn't work property on Windows when dark mode is enabled.
                ToolBoxAdd<Alternet.UI.Slider>();
                */

                /*
                ToolBoxAdd<StdCheckListBox>();
                ToolBoxAdd<Alternet.UI.HiddenBorder>();
                ToolBoxAdd<Alternet.UI.VerticalLine>();
                ToolBoxAdd<Alternet.UI.ContainerControl>();
                ToolBoxAdd<Alternet.UI.GraphicControl>();
                ToolBoxAdd<Alternet.UI.PaintActionsControl>();
                ToolBoxAdd<Alternet.UI.ScrollableUserControl>();
                ToolBoxAdd<Alternet.UI.Control>();
                ToolBoxAdd<Alternet.UI.ColorPickerAndButton>();
                ToolBoxAdd<Alternet.UI.ComboBoxAndButton>();
                ToolBoxAdd<Alternet.UI.EnumPickerAndButton>();
                ToolBoxAdd<Alternet.UI.ComboBoxAndLabel>();
                ToolBoxAdd<Alternet.UI.ValueEditorDouble>();
                ToolBoxAdd<Alternet.UI.ValueEditorSingle>();
                ToolBoxAdd<Alternet.UI.ValueEditorUDouble>();
                ToolBoxAdd<Alternet.UI.ValueEditorUSingle>();
                ToolBoxAdd<Alternet.UI.ValueEditorInt16>();
                ToolBoxAdd<Alternet.UI.ValueEditorInt32>();
                ToolBoxAdd<Alternet.UI.ValueEditorInt64>();
                ToolBoxAdd<Alternet.UI.ValueEditorSByte>();
                ToolBoxAdd<Alternet.UI.ValueEditorEMail>();
                ToolBoxAdd<Alternet.UI.ValueEditorString>();
                ToolBoxAdd<Alternet.UI.ValueEditorUrl>();
                ToolBoxAdd<Alternet.UI.ValueEditorByte>();
                ToolBoxAdd<Alternet.UI.ValueEditorUInt16>();
                ToolBoxAdd<Alternet.UI.ValueEditorUInt32>();
                ToolBoxAdd<Alternet.UI.ValueEditorUInt64>();
                ToolBoxAdd<Alternet.UI.HeaderLabel>();
                ToolBoxAdd<Alternet.UI.Grid>();
                ToolBoxAdd<Alternet.UI.LayoutPanel>();
                ToolBoxAdd<Alternet.UI.SplittedTreeAndCards>();
                ToolBoxAdd<Alternet.UI.ScrollViewer>();
                ToolBoxAdd<Alternet.UI.PanelListBoxAndCards>();
                ToolBoxAdd<Alternet.UI.PanelTreeAndCards>();
                ToolBoxAdd<Alternet.UI.SplittedControlsPanel>();
                ToolBoxAdd<Alternet.UI.Splitter>();
                ToolBoxAdd<Alternet.UI.ColorComboBox>();
                ToolBoxAdd<Alternet.UI.FontComboBox>();
                ToolBoxAdd<Alternet.UI.ListBoxHeader>();
                ToolBoxAdd<Alternet.UI.ActionsListBox>();
                ToolBoxAdd<Alternet.UI.LogListBox>();
                ToolBoxAdd<Alternet.UI.VirtualCheckListBox>();
                ToolBoxAdd<Alternet.UI.AnimationPlayer>();
                ToolBoxAdd<Alternet.UI.InnerPopupToolBar>();
                ToolBoxAdd<Alternet.UI.PopupControl>();
                ToolBoxAdd<Alternet.UI.PreviewFile>();
                ToolBoxAdd<Alternet.UI.PreviewFileSplitted>();
                ToolBoxAdd<Alternet.UI.PreviewInBrowser>();
                ToolBoxAdd<Alternet.UI.PreviewTextFile>();
                ToolBoxAdd<Alternet.UI.PreviewUixml>();
                ToolBoxAdd<Alternet.UI.PreviewUixmlSplitted>();
                ToolBoxAdd<Alternet.UI.PropertyGrid>();
                ToolBoxAdd<Alternet.UI.HScrollBar>();
                ToolBoxAdd<Alternet.UI.VScrollBar>();
                ToolBoxAdd<Alternet.UI.FontSizePicker>();
                ToolBoxAdd<Alternet.UI.SpeedButtonWithListPopup>();
                ToolBoxAdd<Alternet.UI.SpeedDateButton>();
                ToolBoxAdd<Alternet.UI.SpeedEnumButton>();
                ToolBoxAdd<Alternet.UI.WebBrowser>();
                ToolBoxAdd<Alternet.UI.PanelFormSelector>();
                ToolBoxAdd<Alternet.UI.PanelMultilineTextBox>();
                ToolBoxAdd<Alternet.UI.PanelRichTextBox>();
                ToolBoxAdd<Alternet.UI.PanelSettings>();
                ToolBoxAdd<Alternet.UI.PanelWebBrowser>();
                ToolBoxAdd<Alternet.UI.PanelWithToolBar>();
                */

                if (DebugUtils.IsDebugDefined)
                {
                    ToolBoxAdd<GenericWrappedTextControl>();
                }

                LimitedTypes.AddRange(LimitedTypesStatic);

                var otherCat = new ControlCategoryAttribute("Other");

                if (logNotAddedControls)
                {
                    logNotAddedControls = false;
                    LogNotShownTypes();
                }

                void LogNotShownTypes()
                {
                    var exportedTypes = AssemblyUtils.GetExportedTypesSafe(KnownAssemblies.LibraryCommon);
                    foreach (var type in exportedTypes)
                    {
                        if (LimitedTypes.Contains(type))
                            continue;
                        if (!typeof(Control).IsAssignableFrom(type))
                            continue;
                        if (type.IsAbstract || type.IsInterface)
                            continue;

                        var categoryAttr = AssemblyUtils.GetControlCategory(type) ?? otherCat;

                        if (categoryAttr.IsHidden || categoryAttr.IsInternal)
                            continue;

                        Debug.WriteLine($"Not added to ToolBox: {type.FullName}");
                    }
                }

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