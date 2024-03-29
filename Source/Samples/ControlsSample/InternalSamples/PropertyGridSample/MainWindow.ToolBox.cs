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
            InitSimpleTestActions();

            void Fn()
            {
                bool logAddedControls = false;

                ControlListBoxItem item = new(typeof(WelcomePage))
                {
                    Text = "Welcome Page",
                };
                ToolBox.Add(item);

                Type[] limitedTypes =
                {
                    typeof(Border),
                    typeof(PictureBox),
                    typeof(Button),
                    typeof(GenericToolBar),
                    typeof(SplittedPanel),
                    typeof(Calendar),
                    typeof(CheckBox),
                    typeof(ColorPicker),
                    typeof(ComboBox),
                    typeof(DateTimePicker),
                    typeof(GroupBox),
                    typeof(HorizontalStackPanel),
                    typeof(Label),
                    typeof(GenericLabel),
                    typeof(TabControl),
                    typeof(LinkLabel),
                    typeof(ListBox),
                    typeof(ListView),
                    typeof(MultilineTextBox),
                    typeof(NumericUpDown),
                    typeof(Panel),
                    typeof(PanelOkCancelButtons),
                    typeof(ProgressBar),
                    typeof(RadioButton),
                    typeof(RichTextBox),
                    typeof(FindReplaceControl),
                    typeof(Slider),
                    typeof(ScrollBar),
                    typeof(StackPanel),
                    typeof(TextBox),
                    typeof(TreeView),
                    typeof(VerticalStackPanel),
                    typeof(CardPanel),
                    typeof(SpeedButton),
                    typeof(ComboBoxAndLabel),
                    typeof(TextBoxAndLabel),
                    typeof(SpeedTextButton),
                    typeof(SpeedColorButton),
                    typeof(ToolBarSet),
                    typeof(SideBarPanel),
                    typeof(ControlPainterPreview),
                    typeof(ColorComboBox),
                    typeof(ColorListBox),
                    typeof(VListBox),
                    typeof(UserControl),
                };

                List<ControlListBoxItem> items = new();
                
                foreach (Type type in limitedTypes)
                {
                    item = new(type)
                    {
                        HasTicks = true,
                        HasMargins = true,
                    };

                    if (logAddedControls)
                        Application.Log($"typeof({type.FullName}),");
                    items.Add(item);
                }

                items.Add(CreateDialogItem<ColorDialog>());
                items.Add(CreateDialogItem<OpenFileDialog>());
                items.Add(CreateDialogItem<SaveFileDialog>());
                items.Add(CreateDialogItem<SelectDirectoryDialog>());
                items.Add(CreateDialogItem<FontDialog>());

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
                }
            }
        }
    }
}