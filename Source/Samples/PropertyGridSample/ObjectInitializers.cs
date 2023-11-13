using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace PropertyGridSample
{
    internal class ObjectInitializers
    {
        private static ImageLists? imageLists;

        public static ImageLists LoadImageLists()
        {
            imageLists ??= LoadImageListsCore();

            return imageLists;
        }

        public static readonly Dictionary<Type, Action<Object>> Actions = new();

        private const string ResPrefix =
            "embres:PropertyGridSample.Resources.";
        private const string ResPrefixImage = $"{ResPrefix}logo-128x128.png";

        private static readonly Image DefaultImage = Image.FromUrl(ResPrefixImage);
        private static int newItemIndex = 0;

        static ObjectInitializers()
        {
            const int defaultListHeight = 300;

            Actions.Add(typeof(ContextMenu), InitContextMenu);

            Actions.Add(typeof(Label), (c) =>
            { 
                (c as Label)!.Text = "Label";
                (c as Label)!.HorizontalAlignment = HorizontalAlignment.Left;
            });

            Actions.Add(typeof(ScrollViewer), InitScrollViewer);
            Actions.Add(typeof(HorizontalStackPanel), InitStackPanel);
            Actions.Add(typeof(VerticalStackPanel), InitStackPanel);
            Actions.Add(typeof(StackPanel), InitStackPanel);

            Actions.Add(typeof(StatusBar), (c) =>
            {
                (c as StatusBar)!.Panels.Add(new("text1"));
                (c as StatusBar)!.Panels.Add(new("text2"));
            });

            Actions.Add(typeof(Border), (c) =>
            {
                var border = (c as Border)!;
                border.SuggestedHeight = 150;
                border.BorderColor = Color.Indigo;
                border.BackgroundColor = Color.BurlyWood;
            });

            Actions.Add(typeof(RichTextBox), (c) =>
            {
                var control = (c as RichTextBox)!;
                control.SuggestedHeight = 200;
            });

            Actions.Add(typeof(MultilineTextBox), (c) =>
            {
                var control = (c as MultilineTextBox)!;
                control.SuggestedHeight = 200;
            });

            Actions.Add(typeof(Button), (c) =>
            {
                var button = (c as Button)!;
                button.Text = "Button";
                button.StateImages = ButtonImages;
                button.SuggestedHeight = 100;
                button.HorizontalAlignment = HorizontalAlignment.Left;
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
                treeView.SuggestedHeight = defaultListHeight;
                InitTreeView(treeView);
            });

            Actions.Add(typeof(ListView), (c) =>
            {
                ListView listView = (c as ListView)!;
                listView.SuggestedHeight = defaultListHeight;
                InitListView(listView);
            });

            Actions.Add(typeof(ListBox), (c) =>
            {
                ListBox listBox = (c as ListBox)!;
                listBox.SuggestedHeight = defaultListHeight;
                AddTenItems(listBox.Items);
            });

            Actions.Add(typeof(ComboBox), (c) =>
            {
                ComboBox comboBox = (c as ComboBox)!;
                AddTenItems(comboBox.Items);
                comboBox.HorizontalAlignment = HorizontalAlignment.Left;
                comboBox.SuggestedWidth = 150;
            });

            Actions.Add(typeof(CheckListBox), (c) =>
            {
                CheckListBox checkListBox = (c as CheckListBox)!;
                checkListBox.SuggestedHeight = defaultListHeight;
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
                groupBox.SuggestedHeight = 150;
            });

            Actions.Add(typeof(PictureBox), (c) =>
            {
                PictureBox pictureBox = (c as PictureBox)!;
                pictureBox.Image = DefaultImage;
            });

            Actions.Add(typeof(Panel), (c) =>
            {
                Panel panel = (c as Panel)!;
                panel.SuggestedHeight = 150;
                panel.BackgroundColor = Color.BurlyWood;
            });


            Actions.Add(typeof(Control), (c) =>
            {
                Control control = (c as Control)!;
                control.SuggestedHeight = 150;
            });

            Actions.Add(typeof(TabControl), (c) =>
            {
                TabControl control = (c as TabControl)!;
                control.SuggestedHeight = 300;
                InsertPage(control, null);
                InsertPage(control, null);
                InsertPage(control, null);
            });

            Actions.Add(typeof(AuiNotebook), (c) =>
            {
                AuiNotebook control = (c as AuiNotebook)!;
                control.SuggestedHeight = 300;
                InsertPage(control);
                InsertPage(control);
                InsertPage(control);
            });

            Actions.Add(typeof(ProgressBar), (c) =>
            {
                ProgressBar control = (c as ProgressBar)!;
                control.Value = 50;
            });

            Actions.Add(typeof(TextBox), (c) =>
            {
                TextBox control = (c as TextBox)!;
                control.Text = "some text";
            });

            Actions.Add(typeof(PanelOkCancelButtons), (c) =>
            {
                PanelOkCancelButtons control = (c as PanelOkCancelButtons)!;
                control.BackgroundColor = Color.Cornsilk;
            });
        }

        private static ControlStateImages? buttonImages;

        public static ControlStateImages ButtonImages => buttonImages ??= LoadButtonImages();

        private static ControlStateImages LoadButtonImages()
        {
            static Image LoadImage(string stateName) =>
                new Bitmap(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        $"PropertyGridSample.Resources.ButtonImages.ButtonImage{stateName}.png")
                    ?? throw new Exception());

            return new ControlStateImages
            {
                NormalImage = LoadImage("Normal"),
                HoveredImage = LoadImage("Hovered"),
                PressedImage = LoadImage("Pressed"),
                DisabledImage = LoadImage("Disabled"),
                FocusedImage = LoadImage("Focused"),
            };
        }

        private static void InsertPage(TabControl tabControl, int? index = null)
        {
            var s = "Page " + LogUtils.GenNewId();
            var page = new TabPage(s)
            {
                Padding = 5,
            };

            var panel = CreatePanelWithButtons(s);
            page.Children.Add(panel);

            if (index == null)
                tabControl.Pages.Add(page);
            else
                tabControl.Pages.Insert(index.Value, page);
        }

        private static void InsertPage(AuiNotebook tabControl)
        {
            var s = "Page " + LogUtils.GenNewId();
            var page = new Control()
            {
                Padding = 5,
            };

            var panel = CreatePanelWithButtons(s);
            page.Children.Add(panel);
            tabControl.AddPage(page, s, true);
        }

        private static StackPanel CreatePanelWithButtons(string s)
        {
            VerticalStackPanel panel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = 5,
            };

            for (int i = 1; i < 4; i++)
            {
                var button = new Button()
                {
                    Text = s + " Button " + i.ToString(),
                    Margin = 5,
                };
                panel.Children.Add(button);
            }

            return panel;
        }

        internal static void AddTenItems(IList items)
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

        public static void InitScrollViewer(object control)
        {
            ScrollViewer? sv = control as ScrollViewer;
            sv!.SuggestedHeight = 250;
            StackPanel panel = new();
            InitStackPanel(panel);
            panel.Parent = sv;
        }

        public static void InitStackPanel(object control)
        {
            var parent = control as Control;
            parent!.SuggestedHeight = 250;
            parent.BackgroundColor = Color.Cornsilk;

#pragma warning disable
            Button OkButton = new()
            {
                Text = "1",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsDefault = true,
                Parent = parent,
            };

            Button CancelButton = new()
            {
                Text = "2",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsCancel = true,
                Parent = parent,
            };

            Button ApplyButton = new()
            {
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                Text = "3",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Parent = parent,
            };
#pragma warning restore

        }

        public static void InitContextMenu(object control)
        {
            var contextMenu = (control as ContextMenu)!;

            MenuItem menuItem1 = new()
            {
                Text = "Open...",
            };
            menuItem1.Click += (sender, e) => {  };

            MenuItem menuItem2 = new()
            {
                Text = "Save...",
            };
            menuItem2.Click += (sender, e) => { };

            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);
        }

        public static void InitListView(ListView listView)
        {
            var imageLists = LoadImageLists();
            listView.HorizontalAlignment = HorizontalAlignment.Stretch;
            listView.SmallImageList = imageLists.Small;
            listView.LargeImageList = imageLists.Large;

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
            control.ImageList = LoadImageLists().Small;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
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

        private static ImageLists LoadImageListsCore()
        {
            var smallImageList = new ImageList();
            var largeImageList = new ImageList() { ImageSize = new Size(32, 32) };

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames =
                allResourceNames.Where(x => x.StartsWith("PropertyGridSample.Resources.ImageListIcons."));
            var smallImageResourceNames =
                allImageResourceNames.Where(x => x.Contains(".Small.")).ToArray();
            var largeImageResourceNames =
                allImageResourceNames.Where(x => x.Contains(".Large.")).ToArray();
            if (smallImageResourceNames.Length != largeImageResourceNames.Length)
                throw new Exception();

            Image LoadImage(string name) =>
                new Bitmap(assembly.GetManifestResourceStream(name) ?? throw new Exception());

            for (int i = 0; i < smallImageResourceNames.Length; i++)
            {
                smallImageList.Images.Add(LoadImage(smallImageResourceNames[i]));
                largeImageList.Images.Add(LoadImage(largeImageResourceNames[i]));
            }

            return new ImageLists(smallImageList, largeImageList);
        }

        public class ImageLists
        {
            public ImageLists(ImageList small, ImageList large)
            {
                Small = small;
                Large = large;
            }

            public ImageList Small { get; }

            public ImageList Large { get; }
        }
    }
}
