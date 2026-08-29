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
    public partial class MainControl
    {
        public static readonly List<Type> LimitedTypesStatic = new();

        readonly List<Type> LimitedTypes = new();

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

        void ToolBoxAdd<T>()
        {
            LimitedTypes.Add(typeof(T));
        }

        void ToolBoxAdd<T>(Action<T> action)
        {
            LimitedTypes.Add(typeof(T));
            ObjectInit.AddAction<T>(action);
        }

        private void InitToolBox()
        {
            InitTestsAll();

            void Fn()
            {
                List<Type> noTicks = new();

                noTicks.Add(typeof(ResizableBorder));
                noTicks.Add(typeof(ResizableWindowBorder));

                bool logAddedControls = false;
                bool logNotAddedControls = false;

                ToolBoxAdd<Border>(ObjectInit.InitBorder);
                ToolBoxAdd<Calculator>();
                ToolBoxAdd<Calendar>();
                ToolBoxAdd<CardPanel>(ObjectInit.InitCardPanel);
                ToolBoxAdd<CardPanelHeader>(ObjectInit.InitCardPanelHeader);
                ToolBoxAdd<ColorListBox>();
                ToolBoxAdd<ColorPicker>(ObjectInit.InitColorPicker);
                ToolBoxAdd<ContextMenu>(ObjectInit.InitContextMenu);
                ToolBoxAdd<DatePicker>(ObjectInit.InitDatePicker);
                ToolBoxAdd<DateTimePicker>(ObjectInit.InitDateTimePicker);
                ToolBoxAdd<DayOfWeekPicker>();
                ToolBoxAdd<EditableListPicker>(ObjectInit.InitListPicker);
                ToolBoxAdd<EnumPicker>(ObjectInit.InitEnumPicker);
                ToolBoxAdd<FileListBox>(ObjectInit.InitFileListBox);
                ToolBoxAdd<FindReplaceControl>(ObjectInit.InitFindReplaceControl);
                ToolBoxAdd<FontListBox>();
                ToolBoxAdd<FontNamePicker>(ObjectInit.InitFontNamePicker);
                ToolBoxAdd<GenericItemControl>(ObjectInit.InitGenericListItemControl);
                ToolBoxAdd<HorizontalStackPanel>(ObjectInit.InitStackPanel);
                ToolBoxAdd<IntPicker>();
                ToolBoxAdd<Label>(ObjectInit.InitGenericLabel);
                ToolBoxAdd<LabelAndButton>(ObjectInit.InitLabelAndButton);
                ToolBoxAdd<LinkLabel>(ObjectInit.InitLinkLabel);
                ToolBoxAdd<ListPicker>(ObjectInit.InitListPicker);
                ToolBoxAdd<MonthPicker>();
                ToolBoxAdd<MultilineTextBox>(ObjectInit.InitMultilineTextBox);
                ToolBoxAdd<NumericUpDown>(ObjectInit.InitNumericUpDown);
                ToolBoxAdd<Panel>(ObjectInit.InitPanel);
                ToolBoxAdd<PanelOkCancelButtons>(ObjectInit.InitPanelOkCancelButtons);
                ToolBoxAdd<PanelSettings>(ObjectInit.InitPanelSettings);
                ToolBoxAdd<PictureBox>(ObjectInit.InitPictureBox);
                ToolBoxAdd<PinCodePicker>();
                ToolBoxAdd<RelativeWeekdayOfMonthPicker>();
                ToolBoxAdd<RelativeWeekdayPicker>();
                ToolBoxAdd<RepeatPatternPicker>(ObjectInit.InitRepeatPatternPicker);
                ToolBoxAdd<ResizableBorder>(ObjectInit.InitResizableBorder);
                ToolBoxAdd<ResizableWindowBorder>(ObjectInit.InitResizableWindowBorder);
                ToolBoxAdd<RichToolTip>(ObjectInit.InitRichToolTip);
                ToolBoxAdd<ScrollablePanelSettings>(ObjectInit.InitScrollablePanelSettings);
                ToolBoxAdd<ScrollViewer>(ObjectInit.InitScrollViewer);
                ToolBoxAdd<ShapeControl>(ObjectInit.InitShapeControl);
                ToolBoxAdd<SideBarPanel>(ObjectInit.InitSideBarPanel);
                ToolBoxAdd<SpeedButton>(ObjectInit.InitSpeedButton);
                ToolBoxAdd<SpeedColorButton>(ObjectInit.InitSpeedColorButton);
                ToolBoxAdd<SpeedTextButton>(ObjectInit.InitSpeedTextButton);
                ToolBoxAdd<SplittedPanel>(ObjectInit.InitSplittedPanel);
                ToolBoxAdd<StackPanel>(ObjectInit.InitStackPanel);
                ToolBoxAdd<StatusBar>(ObjectInit.InitStatusBar);
                ToolBoxAdd<TabControl>(ObjectInit.InitGenericTabControl);
                ToolBoxAdd<TextBox>(ObjectInit.InitTextBox);
                ToolBoxAdd<TextBoxAndButton>(ObjectInit.InitTextBoxAndButton);
                ToolBoxAdd<TextBoxAndLabel>(ObjectInit.InitTextBoxAndLabel);
                ToolBoxAdd<TextBoxWithListPopup>(ObjectInit.InitTextBoxWithListPopup);
                ToolBoxAdd<TextPicker>();
                ToolBoxAdd<TimePicker>(ObjectInit.InitTimePicker);
                ToolBoxAdd<ToolBar>(ObjectInit.InitGenericToolBar);
                ToolBoxAdd<ToolBarSet>(ObjectInit.InitGenericToolBarSet);
                ToolBoxAdd<UserControl>(ObjectInit.InitUserControl);
                ToolBoxAdd<VerticalStackPanel>(ObjectInit.InitStackPanel);
                ToolBoxAdd<VirtualListBox>(DemoUtils.InitListBoxItems);
                ToolBoxAdd<XButton>(ObjectInit.InitStdButton);
                ToolBoxAdd<XCheckBox>(ObjectInit.InitXCheckBox);
                ToolBoxAdd<XCheckListBox>(ObjectInit.InitCheckListBox);
                ToolBoxAdd<XComboBox>(ObjectInit.InitStdComboBox);
                ToolBoxAdd<XIntPicker>();
                ToolBoxAdd<XListBox>(ObjectInit.InitStdListBox);
                ToolBoxAdd<XProgressBar>(ObjectInit.InitXProgressBar);
                ToolBoxAdd<XRadioButton>(ObjectInit.InitXRadioButton);
                ToolBoxAdd<XScrollBar>(ObjectInit.InitXScrollBar);
                ToolBoxAdd<XSlider>(ObjectInit.InitXSlider);
                ToolBoxAdd<XTreeView>(ObjectInit.InitXTreeView);

                /*
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
                ToolBoxAdd<Alternet.UI.SplittedTreeAndCards>();
                ToolBoxAdd<Alternet.UI.SplittedControlsPanel>();
                ToolBoxAdd<Alternet.UI.Splitter>();
                ToolBoxAdd<Alternet.UI.ColorComboBox>();
                ToolBoxAdd<Alternet.UI.FontComboBox>();
                ToolBoxAdd<Alternet.UI.ListBoxHeader>();
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
                ToolBoxAdd<Alternet.UI.FontSizePicker>();
                ToolBoxAdd<Alternet.UI.SpeedButtonWithListPopup>();
                ToolBoxAdd<Alternet.UI.SpeedDateButton>();
                ToolBoxAdd<Alternet.UI.SpeedEnumButton>();
                ToolBoxAdd<Alternet.UI.WebBrowser>();
                */

                if (DebugUtils.IsDebugDefined)
                {
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
                items.Add(CreateDialogItem<OpenFileDialog>(ObjectInit.InitOpenFileDialog));
                items.Add(CreateDialogItem<SaveFileDialog>(ObjectInit.InitSaveFileDialog));
                items.Add(CreateDialogItem<SelectDirectoryDialog>());
                items.Add(CreateDialogItem<FontDialog>());
                items.Add(CreateDialogItem<PageSetupDialog>(ObjectInit.InitPageSetupDialog));
                items.Add(CreateDialogItem<PrintPreviewDialog>(ObjectInit.InitPrintPreviewDialog));
                items.Add(CreateDialogItem<PrintDialog>(ObjectInit.InitPrintDialog));
                
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

                ControlListBoxItem item = new(typeof(SettingsControl))
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

        private ControlListBoxItem CreateDialogItem<T>(Action<T>? action = null)
            where T : CommonDialog
        {
            if (action != null)
                ObjectInit.AddAction<T>(action);

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