using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

using PropertyGridSample;

namespace ControlsSample
{
    public static class NativeControlsRegistration
    {
        public static void ToolBoxAdd<T>()
        {
            PropertyGridSample.MainControl.LimitedTypesStatic.Add(typeof(T));
        }

        public static void InitActions()
        {
            ToolBoxAdd<Button>();
            ToolBoxAdd<ComboBox>();
            ToolBoxAdd<GroupBox>();
            ToolBoxAdd<CheckBox>();
            ToolBoxAdd<RadioButton>();
            ToolBoxAdd<ProgressBar>();
            ToolBoxAdd<ListBox>();
            ToolBoxAdd<CheckedListBox>();
            ToolBoxAdd<TreeView>();

            /*
            This is commented out because Slider control doesn't work property on Windows when dark mode is enabled.
            ToolBoxAdd<Alternet.UI.Slider>();
            */

            ObjectInit.Actions.Add(typeof(ComboBoxAndLabel), InitComboBoxAndLabel);
            ObjectInit.Actions.Add(typeof(ComboBox), InitComboBox);
            ObjectInit.Actions.Add(typeof(Button), InitButton);
            ObjectInit.Actions.Add(typeof(ListBox), InitListBox);
            ObjectInit.Actions.Add(typeof(CheckedListBox), InitCheckedListBox);

            ObjectInit.Actions.Add(typeof(CheckBox), (c) =>
            {
                (c as CheckBox)!.Text = "CheckBox";
            });

            ObjectInit.Actions.Add(typeof(RadioButton), (c) =>
            {
                (c as RadioButton)!.Text = "RadioButton";
            });

            ObjectInit.Actions.Add(typeof(GroupBox), (c) =>
            {
                GroupBox groupBox = (c as GroupBox)!;
                groupBox.Title = "GroupBox";
                groupBox.SuggestedSize = 150;
                groupBox.MinChildMargin = 10;

                groupBox.Layout = LayoutStyle.Vertical;

                Label label = new("Label 1");
                label.Parent = groupBox;

                CheckBox checkBox = new("CheckBox 1");
                checkBox.Parent = groupBox;
            });

            ObjectInit.Actions.Add(typeof(TreeView), (c) =>
            {
                TreeView treeView = (c as TreeView)!;
                treeView.SuggestedSize = ObjectInit.DefaultListSize;
                InitVirtualTreeControl(treeView);
            });

            ObjectInit.Actions.Add(typeof(ProgressBar), (c) =>
            {
                ProgressBar control = (c as ProgressBar)!;
                control.OrientationChanged += OrientationChanged;
                control.Value = 50;
                control.SuggestedWidth = 200;

                static void OrientationChanged(object? sender, EventArgs e)
                {
                    if (sender is not ProgressBar control)
                        return;
                    if (control.Orientation == ProgressBarOrientation.Vertical)
                        control.SuggestedSize = (float.NaN, 250);
                    else
                        control.SuggestedSize = (250, float.NaN);
                    control.PerformLayout();
                }

            });
        }

        public static void InitButton(object control)
        {
            if (control is not Button button)
                return;
            button.Text = "Butt&on";
            button.Click += ObjectInit.LogClick;
            button.StateImages = ObjectInit.GetButtonImages(button);
            button.SetImageMargins(5);
            button.SuggestedHeight = 100;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.MouseWheel += ObjectInit.Button_MouseWheel;
        }

        public static void InitComboBoxAndLabel(object control)
        {
            if (control is not ComboBoxAndLabel textBox)
                return;
            textBox.Text = "item 1";
            textBox.Label.Text = "Label";
            textBox.ComboBox.SuggestedWidth = 200;
            textBox.ComboBox.Add("item 1");
            textBox.ComboBox.Add("item 2");
            textBox.ComboBox.Add("item 3");
            textBox.ComboBox.Add("item 4");
            textBox.ComboBox.Add("item 5");
        }

        public static void AddManyItems(ListBox listBox, int numItems)
        {
            listBox.DoInsideUpdate(() =>
            {
                for (int i = 0; i < numItems; i++)
                    listBox.Items.Add($"Item #{LogUtils.GenNewId()}");

                App.Log($"Added {numItems} items");
            });

            listBox.SelectLastItem();
        }

        public static void InitCheckedListBox(object control)
        {
            InitListBox(control);
            if (control is not CheckedListBox listBox)
                return;
            listBox.CheckedItemsChanged += (s, e) =>
            {
                ObjectInit.LogItems("ListBox CheckedItemsChanged", listBox.CheckedItems);
            };
        }
        public static void InitListBox(object control)
        {
            if (control is not ListBox listBox)
                return;
            listBox.SuggestedSize = ObjectInit.DefaultListSize;

            foreach (var item in ObjectInit.GetTenItems())
                listBox.Items.Add(item);

            listBox.SelectedIndexChanged += (s, e) => ObjectInit.LogItems("ListBox SelectedIndexChanged", listBox.SelectedItems);
        }

        public static void InitVirtualTreeControl(TreeView control)
        {
            if (App.SafeWindow.UseSmallImages)
                control.ImageList = ObjectInit.LoadImageLists().Small;
            else
                control.ImageList = ObjectInit.LoadImageLists().Large;

            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            AddItems(control, 10);
        }

        public static void AddItems(TreeView treeView, int count)
        {
            treeView.BeginUpdate();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    int imageIndex = i % 4;
                    var item = new TreeViewItem(
                        "Item " + ObjectInit.GenItemIndex(),
                        imageIndex);
                    for (int j = 0; j < 3; j++)
                    {
                        var childItem = new TreeViewItem(
                            item.Text + "." + j,
                            imageIndex);
                        item.Add(childItem);

                        if (i < 5)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                childItem.Add(
                                    new TreeViewItem(
                                        item.Text + "." + k,
                                        imageIndex));
                            }
                        }
                    }

                    treeView.Add(item);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        public static void InitComboBox(object control)
        {
            if (control is not ComboBox comboBox)
                return;
            comboBox.Items.AddRange(ObjectInit.GetTenItems());
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
        }
    }
}
