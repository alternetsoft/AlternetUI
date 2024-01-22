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
            if (panel.LeftTreeView.SelectedItem is not ControlListBoxItem item)
                return default;
            if (item.Instance is T control)
                return control;
            return default;
        }

        void PictureBoxTest()
        {
            var control = GetSelectedControl<PictureBox>();
            if (control is null)
                return;
            control.Image = KnownSvgImages.GetForSize(Color.Red, 64).ImgAddChild.AsImage(64);
            /*(control?.Background as SolidBrush)?.SetColor(Color.Red);
            control?.Refresh();*/
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
                bool addLimitedControls = true;

                ControlListBoxItem item = new(typeof(WelcomePage))
                {
                    Text = "Welcome Page",
                };
                panel.LeftTreeView.Add(item);

                Type[] limitedTypes =
                [
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
                    typeof(CardPanelHeader),
                    typeof(CardPanel),
                    typeof(SpeedButton),
                    typeof(ComboBoxAndLabel),
                    typeof(TextBoxAndLabel),
                    typeof(SpeedTextButton),
                    typeof(GenericToolBarSet),
                ];

                Type[] badParentTypes =
                [
                  typeof(ControlAndLabel),
                  typeof(PopupWindow),
                  typeof(PanelAuiManager),
                ];

                Type[] badTypes =
                [
                  typeof(PopupWindow),
                  typeof(ContextMenu), // added using other style
                  typeof(NonVisualControl), // has no sense to add
                  typeof(StatusBarPanel), // part of other control
                  typeof(MenuItem), // part of other control
                  typeof(TabPage), // part of other control
                  typeof(ToolbarItem), // part of other control

                  typeof(Toolbar), // can create some modal window? or add child window 
                  typeof(WebBrowser),
                  //typeof(Popup), // button with popup like ContextMenu
                  typeof(AuiNotebook),
                  typeof(AuiToolbar),
                  typeof(AnimationPlayer),
                  typeof(PanelAuiManager),
                  typeof(PanelAuiManagerBase),
                  typeof(DialogWindow),
                  typeof(LogListBox),
                  typeof(SplitterPanel),// know how
                  typeof(Grid),// know how
                  typeof(ScrollViewer),
                  typeof(LayoutPanel),// know how
                  typeof(UserPaintControl),// know how
                  typeof(MainMenu),// can create some modal window? or add child window 
                  typeof(StatusBar),// can create some modal window? or add child window 
                  typeof(PropertyGrid),
                  typeof(CheckListBox), // as empty items error
                  typeof(PanelWebBrowser),
                  typeof(TabControl), // pages are not shown. Why?
                  typeof(Window),                
                ];

                IEnumerable<Type> result = addLimitedControls ? limitedTypes : AssemblyUtils.GetTypeDescendants(typeof(Control));
                
                foreach (Type type in result)
                {
                    /*if (type.Assembly != typeof(Control).Assembly)
                        continue;*/
                    if (Array.IndexOf(badTypes, type) >= 0)
                        continue;
                    /*if (AssemblyUtils.TypeIsDescendant(type, badParentTypes))
                        continue;*/
                    item = new(type)
                    {
                        HasTicks = true,
                        HasMargins = true,
                    };

                    panel.LeftTreeView.Add(item);
                    if (logAddedControls)
                        Application.Log($"typeof({type.Name}),");
                }

                item = new(typeof(SettingsControl))
                {
                    PropInstance = PropertyGridSettings.Default,
                    EventInstance = new object(),
                    Text = "Demo Options",
                };
                panel.LeftTreeView.Add(item);

                AddDialog<ColorDialog>();
                AddDialog<OpenFileDialog>();
                AddDialog<SaveFileDialog>();
                AddDialog<SelectDirectoryDialog>();
                AddDialog<FontDialog>();
                /*AddContextMenu<ContextMenu>();*/
            }

            panel.LeftTreeView.DoInsideUpdate(() => { Fn(); });
        }

        internal void AddMainWindow()
        {
            panel.LeftTreeView.Add(new ControlListBoxItem(typeof(Window), this.ParentWindow));
        }

        private void AddDialog<T>()
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
            panel.LeftTreeView.Add(item);
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
            panel.LeftTreeView.Add(item);
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