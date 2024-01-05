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
    internal partial class ObjectInitializers
    {
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        private static ImageLists? imageLists;

        public static ImageLists LoadImageLists()
        {
            imageLists ??= LoadImageListsCore();

            return imageLists;
        }

        public static readonly Dictionary<Type, Action<Object>> Actions = [];

        private const string ResPrefix =
            "embres:PropertyGridSample.Resources.";
        private const string ResPrefixImage = $"{ResPrefix}logo-128x128.png";

        private static readonly Image DefaultImage = Image.FromUrl(ResPrefixImage);
        private static int newItemIndex = 0;

        static void SetBackgrounds(Control control)
        {
            if (SpeedButton.Defaults is null && false)
            {
                /* Sample of changing default speed button settings */

                var border = BorderSettings.Default.Clone();
                border.UniformRadiusIsPercent = true;
                border.UniformCornerRadius = 25;
                border.Color = Color.Red;

                var defaultButton = new SpeedButton();
                defaultButton.Borders!.SetObject(border, GenericControlState.Hovered);
                defaultButton.Borders!.SetObject(border, GenericControlState.Pressed);
                defaultButton.Backgrounds ??= new();
                defaultButton.Backgrounds.SetObject(SystemColors.GrayText.AsBrush, GenericControlState.Hovered);
                defaultButton.Backgrounds.SetObject(SystemColors.GrayText.AsBrush, GenericControlState.Pressed);
                SpeedButton.Defaults = defaultButton;
            }

            control.Backgrounds = new()
            {
                Normal = Color.PaleTurquoise.AsBrush,
                Hovered = Color.IndianRed.AsBrush,
                Disabled = Color.DarkGray.AsBrush,
                Pressed = Color.Cornsilk.AsBrush,
                Focused = Color.DarkOrange.AsBrush,
            };
        }

        static ObjectInitializers()
        {
            Actions.Add(typeof(ContextMenu), InitContextMenu);
            Actions.Add(typeof(SplittedPanel), InitSplittedPanel);
            Actions.Add(typeof(ScrollViewer), InitScrollViewer);
            Actions.Add(typeof(HorizontalStackPanel), InitStackPanel);
            Actions.Add(typeof(VerticalStackPanel), InitStackPanel);
            Actions.Add(typeof(StackPanel), InitStackPanel);
            Actions.Add(typeof(ScrollBar), InitScrollBar);
            Actions.Add(typeof(SpeedButton), InitSpeedButton);
            Actions.Add(typeof(PictureBox), InitPictureBox);
            Actions.Add(typeof(GenericToolBar), InitGenericToolBar);            

            const int defaultListHeight = 250;
            SizeD defaultListSize = new(defaultListHeight, defaultListHeight);


            Actions.Add(typeof(Slider), (c) =>
            {
                Slider control = (c as Slider)!;
                control.SuggestedSize = 200;
            });

            Actions.Add(typeof(NumericUpDown), (c) =>
            {
                NumericUpDown control = (c as NumericUpDown)!;
                control.SuggestedWidth = 200;
            });

            Actions.Add(typeof(DateTimePicker), (c) =>
            {
                DateTimePicker control = (c as DateTimePicker)!;
                control.SuggestedWidth = 200;
            });

            Actions.Add(typeof(Border), (c) =>
            {
                var border = (c as Border)!;
                border.SuggestedSize = defaultListSize;
                SetBackgrounds(border);

                border.CurrentStateChanged += Border_CurrentStateChanged;

                static void Border_CurrentStateChanged(object? sender, EventArgs e)
                {
                    Application.LogNameValue("Border.CurrentState", (sender as Border)?.CurrentState);
                }
            });

            Actions.Add(typeof(Label), (c) =>
            { 
                (c as Label)!.Text = "Label";
                (c as Label)!.HorizontalAlignment = HorizontalAlignment.Left;
            });

            Actions.Add(typeof(StatusBar), (c) =>
            {
                (c as StatusBar)!.Panels.Add(new("text1"));
                (c as StatusBar)!.Panels.Add(new("text2"));
            });

            Actions.Add(typeof(CardPanel), (c) =>
            {
                var control = (c as CardPanel)!;
                control.SuggestedSize = defaultListSize;
                var panel = CreatePanelWithButtons("Card 1");
                panel.AddLabel("Use SelectedCardIndex").Margin = 5;
                panel.AddLabel("to change active card").Margin = 5;
                control.Add("card 1", panel);
                control.Add("card 2", CreatePanelWithButtons("Card 2"));
                control.SelectCard(0);
            });

            Actions.Add(typeof(CardPanelHeader), (c) =>
            {
                var control = (c as CardPanelHeader)!;
                control.SuggestedSize = defaultListSize;
                control.UseTabBackgroundColor = true;
                control.Add("tab 1");
                control.Add("tab 2");
                control.SelectFirstTab();
            });

            Actions.Add(typeof(RichTextBox), (c) =>
            {
                var control = (c as RichTextBox)!;
                control.SuggestedSize = defaultListSize;
            });

            Actions.Add(typeof(MultilineTextBox), (c) =>
            {
                var control = (c as MultilineTextBox)!;
                control.SuggestedSize = defaultListSize;
                control.Text = LoremIpsum;
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
                treeView.SuggestedSize = defaultListSize;
                InitTreeView(treeView);
            });

            Actions.Add(typeof(ListView), (c) =>
            {
                ListView listView = (c as ListView)!;
                listView.SuggestedSize = defaultListSize;
                InitListView(listView);
            });

            Actions.Add(typeof(ListBox), (c) =>
            {
                ListBox listBox = (c as ListBox)!;
                listBox.SuggestedSize = defaultListSize;
                AddTenItems(listBox.Items);
            });

            Actions.Add(typeof(ComboBox), (c) =>
            {
                ComboBox comboBox = (c as ComboBox)!;
                AddTenItems(comboBox.Items);
                comboBox.HorizontalAlignment = HorizontalAlignment.Left;
                comboBox.SuggestedWidth = 200;
            });

            Actions.Add(typeof(CheckListBox), (c) =>
            {
                CheckListBox checkListBox = (c as CheckListBox)!;
                checkListBox.SuggestedSize = defaultListHeight;
                AddTenItems(checkListBox.Items);
            });

            Actions.Add(typeof(LinkLabel), (c) =>
            {
                LinkLabel linkLabel = (c as LinkLabel)!;
                var s = "https://www.google.com";
                linkLabel.Text = "LinkLabel";
                linkLabel.Url = s;
            });

            Actions.Add(typeof(GroupBox), (c) =>
            {
                GroupBox groupBox = (c as GroupBox)!;
                groupBox.Title = "GroupBox";
                groupBox.SuggestedSize = 150;
            });

            Actions.Add(typeof(Panel), InitPanel);

            Actions.Add(typeof(Control), (c) =>
            {
                Control control = (c as Control)!;
                control.SuggestedSize = defaultListHeight;
            });

            Actions.Add(typeof(TabControl), (c) =>
            {
                TabControl control = (c as TabControl)!;
                control.SuggestedSize = defaultListHeight;
                InsertPage(control, null);
                InsertPage(control, null);
                InsertPage(control, null);
            });

            Actions.Add(typeof(AuiNotebook), (c) =>
            {
                AuiNotebook control = (c as AuiNotebook)!;
                control.SuggestedHeight = defaultListHeight;
                InsertPage(control);
                InsertPage(control);
                InsertPage(control);
            });

            Actions.Add(typeof(ProgressBar), (c) =>
            {
                ProgressBar control = (c as ProgressBar)!;
                control.Value = 50;
                control.SuggestedWidth = 200;
            });

            Actions.Add(typeof(TextBox), (c) =>
            {
                TextBox control = (c as TextBox)!;
                control.Text = "some text";
                control.SuggestedWidth = 200;
            });

            Actions.Add(typeof(ColorPicker), (c) =>
            {
                ColorPicker control = (c as ColorPicker)!;
                control.Value = Color.Red;
            });            

            Actions.Add(typeof(PanelOkCancelButtons), (c) =>
            {
                PanelOkCancelButtons control = (c as PanelOkCancelButtons)!;
                //control.BackgroundColor = Color.Cornsilk;
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

            var normal = LoadImage("Normal");
            var disabled = normal.ToGrayScale();

            return new ControlStateImages
            {
                Normal = normal,
                Hovered = LoadImage("Hovered"),
                Pressed = LoadImage("Pressed"),
                Disabled = disabled,
                Focused = LoadImage("Focused"),
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
            //parent.BackgroundColor = Color.Cornsilk;

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
            var largeImageList = new ImageList() { ImageSize = new SizeD(32, 32) };

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
