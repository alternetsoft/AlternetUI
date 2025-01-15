﻿using System;
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
                bool logAddedControls = false;

                ControlListBoxItem item = new(typeof(WelcomePage))
                {
                    Text = "Welcome Page",
                };
                ToolBox.Add(item);

                LimitedTypes.Add(typeof(Border));
                LimitedTypes.Add(typeof(PictureBox));
                LimitedTypes.Add(typeof(Button));
                LimitedTypes.Add(typeof(ToolBar));
                LimitedTypes.Add(typeof(SplittedPanel));
                LimitedTypes.Add(typeof(Calendar));
                LimitedTypes.Add(typeof(CheckBox));
                LimitedTypes.Add(typeof(ComboBox));
                LimitedTypes.Add(typeof(DateTimePicker));
                LimitedTypes.Add(typeof(GroupBox));
                LimitedTypes.Add(typeof(HorizontalStackPanel));
                LimitedTypes.Add(typeof(Label));
                LimitedTypes.Add(typeof(GenericLabel));
                LimitedTypes.Add(typeof(TabControl));
                LimitedTypes.Add(typeof(LinkLabel));
                LimitedTypes.Add(typeof(ListBox));
                LimitedTypes.Add(typeof(ListView));
                LimitedTypes.Add(typeof(MultilineTextBox));
                LimitedTypes.Add(typeof(NumericUpDown));
                LimitedTypes.Add(typeof(Panel));
                LimitedTypes.Add(typeof(PanelOkCancelButtons));
                LimitedTypes.Add(typeof(ProgressBar));
                LimitedTypes.Add(typeof(RadioButton));
                LimitedTypes.Add(typeof(RichTextBox));
                LimitedTypes.Add(typeof(FindReplaceControl));
                LimitedTypes.Add(typeof(Slider));
                LimitedTypes.Add(typeof(ScrollBar));
                LimitedTypes.Add(typeof(StackPanel));
                LimitedTypes.Add(typeof(TextBox));
                LimitedTypes.Add(typeof(TreeView));
                LimitedTypes.Add(typeof(VerticalStackPanel));
                LimitedTypes.Add(typeof(CardPanel));
                LimitedTypes.Add(typeof(SpeedButton));
                LimitedTypes.Add(typeof(ComboBoxAndLabel));
                LimitedTypes.Add(typeof(TextBoxAndLabel));
                LimitedTypes.Add(typeof(SpeedTextButton));
                LimitedTypes.Add(typeof(SpeedColorButton));
                LimitedTypes.Add(typeof(ToolBarSet));
                LimitedTypes.Add(typeof(SideBarPanel));
                LimitedTypes.Add(typeof(ColorComboBox));
                LimitedTypes.Add(typeof(ColorListBox));
                LimitedTypes.Add(typeof(VirtualListBox));
                LimitedTypes.Add(typeof(UserControl));
                LimitedTypes.Add(typeof(RichToolTip));
                LimitedTypes.Add(typeof(TextBoxAndButton));
                LimitedTypes.Add(typeof(FontComboBox));

                if (DebugUtils.IsDebugDefined)
                {
                    LimitedTypes.Add(typeof(GenericTextControl));
                }

                LimitedTypes.AddRange(LimitedTypesStatic);

                List<ControlListBoxItem> items = new();
                
                foreach (Type type in LimitedTypes)
                {
                    item = new(type)
                    {
                        HasTicks = true,
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

                foreach (var elem in items)
                    ToolBox.Add(elem);

                item = new(typeof(SettingsControl))
                {
                    PropInstance = PropertyGridSettings.Default,
                    EventInstance = new object(),
                    Text = "Demo Options",
                };
                ToolBox.Add(item);
            }

            ToolBox.DoInsideUpdate(() => { Fn(); });
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

        private void ApplicationIdle(object? sender, EventArgs e)
        {
            if (updatePropertyGrid)
            {
                updatePropertyGrid = false;
                try
                {
                    UpdatePropertyGrid();
                }
                catch
                {
                    if (DebugUtils.IsDebugDefined)
                        throw;
                }
            }
        }
    }
}