using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class ObjectInitializers
    {
        public static readonly Dictionary<Type, Action<Object>> Actions = new();

        private const string ResPrefix =
            "embres:PropertyGridSample.Resources.";
        private const string ResPrefixImage = $"{ResPrefix}logo-128x128.png";

        private static readonly Image DefaultImage = Image.FromUrl(ResPrefixImage);
        private static int newItemIndex = 0;

        static ObjectInitializers()
        {
            const int defaultListHeight = 300;

            Actions.Add(typeof(Label), (c) =>
            { 
                (c as Label)!.Text = "Label"; 
            });

            Actions.Add(typeof(Border), (c) =>
            {
                (c as Border)!.Height = 150; 
            });

            Actions.Add(typeof(Button), (c) =>
            {
                (c as Button)!.Text = "Button";
            });

            Actions.Add(typeof(CheckBox), (c) =>
            {
                (c as CheckBox)!.Text = "CheckBox";
            });

            Actions.Add(typeof(RadioButton), (c) =>
            {
                (c as RadioButton)!.Text = "RadioButton";
            });

            Actions.Add(typeof(TreeView), (c) =>
            {
                TreeView treeView = (c as TreeView)!;
                treeView.Height = defaultListHeight;
                InitTreeView(treeView);
            });

            Actions.Add(typeof(ListView), (c) =>
            {
                ListView listView = (c as ListView)!;
                listView.Height = defaultListHeight;
                InitListView(listView);
            });

            Actions.Add(typeof(ListBox), (c) =>
            {
                ListBox listBox = (c as ListBox)!;
                listBox.Height = defaultListHeight;
                AddTenItems(listBox.Items);
            });

            Actions.Add(typeof(ComboBox), (c) =>
            {
                ComboBox comboBox = (c as ComboBox)!;
                AddTenItems(comboBox.Items);
            });

            Actions.Add(typeof(CheckListBox), (c) =>
            {
                CheckListBox checkListBox = (c as CheckListBox)!;
                checkListBox.Height = defaultListHeight;
                AddTenItems(checkListBox.Items);
            });

            Actions.Add(typeof(LinkLabel), (c) =>
            {
                LinkLabel linkLabel = (c as LinkLabel)!;
                linkLabel.Text = "LinkLabel";
                linkLabel.Url = "https://www.google.com/";
            });

            Actions.Add(typeof(GroupBox), (c) =>
            {
                GroupBox groupBox = (c as GroupBox)!;
                groupBox.Title = "GroupBox";
                groupBox.Height = 150;
            });

            Actions.Add(typeof(PictureBox), (c) =>
            {
                PictureBox pictureBox = (c as PictureBox)!;
                pictureBox.Image = DefaultImage;
            });

        }

        private static void AddTenItems(Collection<object> items)
        {
            items.Add("One");
            items.Add("Two");
            items.Add("Three");
            items.Add("Four");
            items.Add("Five");
            items.Add("Six");
            items.Add("Seven");
            items.Add("Eight");
            items.Add("Nine");
            items.Add("Ten");
        }

        public static void InitListView(ListView listView)
        {
            AddDefaultItems();

            void InitializeColumns()
            {
                listView?.Columns.Add(new ListViewColumn("Column 1"));
                listView?.Columns.Add(new ListViewColumn("Column 2"));
            }

            void AddDefaultItems()
            {
                InitializeColumns();
                AddItems(50);
                foreach (var column in listView!.Columns)
                    column.WidthMode = ListViewColumnWidthMode.AutoSize;
            }

            void AddItems(int count)
            {
                if (listView == null)
                    return;

                listView.BeginUpdate();
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        var ix = GenItemIndex();
                        listView.Items.Add(
                            new ListViewItem(new[] {
                            "Item " + ix,
                            "Some Info " + ix
                            }, i % 4));
                    }
                }
                finally
                {
                    listView.EndUpdate();
                }
            }


        }

        public static void InitTreeView(TreeView control)
        {
            AddItems(control, 10);
        }

        private static int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private static void AddItems(TreeView treeView, int count)
        {
            treeView.BeginUpdate();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    int imageIndex = i % 4;
                    var item = new TreeViewItem(
                        "Item " + GenItemIndex(),
                        imageIndex);
                    for (int j = 0; j < 3; j++)
                    {
                        var childItem = new TreeViewItem(
                            item.Text + "." + j,
                            imageIndex);
                        item.Items.Add(childItem);

                        if (i < 5)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                childItem.Items.Add(
                                    new TreeViewItem(
                                        item.Text + "." + k,
                                        imageIndex));
                            }
                        }
                    }

                    treeView.Items.Add(item);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }
    }
}
