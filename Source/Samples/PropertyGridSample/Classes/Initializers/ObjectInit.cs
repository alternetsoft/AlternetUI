using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        private const int defaultListHeight = 250;
        public const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        private static ImageLists? imageLists;
        private static SizeD defaultListSize = new(defaultListHeight, defaultListHeight);

        public static ImageLists LoadImageLists()
        {
            imageLists ??= LoadImageListsCore();

            return imageLists;
        }

        public static readonly Dictionary<Type, Action<Object>> Actions = new();

        internal static string AsmResPrefix
            = AssemblyUtils.GetAssemblyResPrefix(typeof(ObjectInit).Assembly)+"Resources.";
        internal static string UrlResPrefix
            = AssemblyUtils.GetImageUrlInAssembly(typeof(ObjectInit).Assembly, "Resources.");
        internal static string ResPrefixImage = $"{UrlResPrefix}logo128x128.png";

        internal static readonly Image DefaultImage = Image.FromUrl(ResPrefixImage);

        private static int newItemIndex = 0;

        public static void SetBackgrounds(AbstractControl control)
        {
            if(control.IsDarkBackground)
            {
                control.Backgrounds = new()
                {
                    Normal = Color.PaleTurquoise.Darker().AsBrush,
                    Hovered = Color.IndianRed.Darker().AsBrush,
                    Disabled = Color.DarkGray.Darker().AsBrush,
                    Pressed = Color.Cornsilk.Darker().AsBrush,
                    Focused = Color.DarkOrange.Darker().AsBrush,
                };                
            }
            else
            {
                control.Backgrounds = new()
                {
                    Normal = Color.PaleTurquoise.AsBrush,
                    Hovered = Color.IndianRed.AsBrush,
                    Disabled = Color.DarkGray.AsBrush,
                    Pressed = Color.Cornsilk.AsBrush,
                    Focused = Color.DarkOrange.AsBrush,
                };
            }            
        }

        public static void InitPageSetupDialog(object control)
        {
            if (control is not PageSetupDialog dialog)
                return;
            dialog.Document = CreatePrintDocument();
        }

        public static void InitPrintPreviewDialog(object control)
        {
            if (control is not PrintPreviewDialog dialog)
                return;
            dialog.Document = CreatePrintDocument();
        }

        public static void InitPrintDialog(object control)
        {
            if (control is not PrintDialog dialog)
                return;
            dialog.Document = CreatePrintDocument();
        }

        public static PrintDocument CreatePrintDocument()
        {
            var document = new PrintDocument
            {
                OriginAtMargins = false,
                DocumentName = "Sample document",
            };

            document.PrinterSettings.FromPage = 1;
            document.PrinterSettings.MinimumPage = 1;

            var maxPage = 3 + 1;
            document.PrinterSettings.MaximumPage = maxPage;
            document.PrinterSettings.ToPage = maxPage;

            document.PageSettings.Color = true;
            document.PageSettings.Margins = 20;

            document.PrintPage += Document_PrintPage;

            return document;

            void Document_PrintPage(object? sender, PrintPageEventArgs e)
            {
                int pageNumber = e.PageNumber;

                var bounds = new RectD(new PointD(), e.PrintablePageBounds.Size);

                if (pageNumber == 1)
                {
                    PrintingSample.PrintingMainWindow.DrawFirstPage(
                        e.DrawingContext,
                        bounds);
                }
                else
                {
                    PrintingSample.PrintingMainWindow.DrawAdditionalPage(
                        e.DrawingContext,
                        pageNumber,
                        bounds);
                }

                var v = 3;

                e.HasMorePages = pageNumber - 1 < v;
            }
        }

        public static void InitRichToolTip(RichToolTip control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.MaxWidth = 450;

            control.ShowToolTip(
                "This is title",
                LoremIpsum,
                MessageBoxIcon.Information,
                0);
        }

        public static void AddAction<T>(Action<T> action)
        {
            Actions.Add(typeof(T), (o) =>
            {
                if (o is T tObject)
                    action(tObject);
            });
        }

        static ObjectInit()
        {
            AddAction<RichToolTip>(InitRichToolTip);

            Actions.Add(typeof(PageSetupDialog), InitPageSetupDialog);
            Actions.Add(typeof(PrintPreviewDialog), InitPrintPreviewDialog);
            Actions.Add(typeof(PrintDialog), InitPrintDialog);
            Actions.Add(typeof(ContextMenu), InitContextMenu);
            Actions.Add(typeof(SplittedPanel), InitSplittedPanel);
            Actions.Add(typeof(ScrollViewer), InitScrollViewer);
            Actions.Add(typeof(HorizontalStackPanel), InitStackPanel);
            Actions.Add(typeof(VerticalStackPanel), InitStackPanel);
            Actions.Add(typeof(StackPanel), InitStackPanel);
            Actions.Add(typeof(ScrollBar), InitScrollBar);
            Actions.Add(typeof(SpeedButton), InitSpeedButton);
            Actions.Add(typeof(PictureBox), InitPictureBox);
            Actions.Add(typeof(ToolBar), InitGenericToolBar);
            Actions.Add(typeof(FindReplaceControl), InitFindReplaceControl);
            Actions.Add(typeof(ToolBarSet), InitGenericToolBarSet);
            Actions.Add(typeof(CardPanel), InitCardPanel);
            Actions.Add(typeof(TextBox), InitTextBox);
            Actions.Add(typeof(TextBoxAndLabel), InitTextBoxAndLabel);
            Actions.Add(typeof(TextBoxAndButton), InitTextBoxAndButton);
            Actions.Add(typeof(RichTextBox), InitRichTextBox);
            Actions.Add(typeof(ComboBoxAndLabel), InitComboBoxAndLabel);
            Actions.Add(typeof(MultilineTextBox), InitMultilineTextBox);
            Actions.Add(typeof(GenericLabel), InitGenericLabel);
            Actions.Add(typeof(Label), InitLabel);
            Actions.Add(typeof(GenericTextControl), InitGenericTextControl);
            Actions.Add(typeof(LinkLabel), InitLinkLabel);
            Actions.Add(typeof(Button), InitButton);
            Actions.Add(typeof(SpeedTextButton), InitSpeedTextButton);
            Actions.Add(typeof(SpeedColorButton), InitSpeedColorButton);
            Actions.Add(typeof(SideBarPanel), InitSideBarPanel);
            Actions.Add(typeof(TabControl), InitGenericTabControl);
            Actions.Add(typeof(VirtualListBox), InitVListBox);
            Actions.Add(typeof(ListBox), InitListBox);
            Actions.Add(typeof(ComboBox), InitComboBox);
            Actions.Add(typeof(ColorComboBox), InitColorComboBox);
            Actions.Add(typeof(FontComboBox), InitFontComboBox);
            Actions.Add(typeof(CheckListBox), InitCheckListBox);

            Actions.Add(typeof(UserControl), (c) =>
            {
                var control = (c as UserControl)!;
                control.HasBorder = true;
                control.SuggestedSize = 200;
            });

            Actions.Add(typeof(SaveFileDialog), (c) =>
            {
                var control = (c as SaveFileDialog)!;
                control.Title = "Some title";
            });

            Actions.Add(typeof(OpenFileDialog), (c) =>
            {
                var control = (c as OpenFileDialog)!;
                control.Title = "Some title";
            });

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
                border.ParentBackColor = false;
                border.ParentForeColor = false;
                border.SuggestedSize = defaultListSize;
                SetBackgrounds(border);

                border.Layout = LayoutStyle.Vertical;
                Button button = new();
                button.Text = "Click me";
                button.Parent = border;
                button.Click += Button_Click;

                border.VisualStateChanged += Border_VisualStateChanged;

                static void Button_Click(object? sender, EventArgs e)
                {
                    App.Log("Button in Border clicked.");
                }

                static void Border_VisualStateChanged(object? sender, EventArgs e)
                {
                    App.LogNameValue("Border.VisualState", (sender as Border)?.VisualState);
                }
            });

            Actions.Add(typeof(StatusBar), (c) =>
            {
                (c as StatusBar)!.Panels.Add(new("text1"));
                (c as StatusBar)!.Panels.Add(new("text2"));
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

            Actions.Add(typeof(GroupBox), (c) =>
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

            Actions.Add(typeof(Panel), InitPanel);

            Actions.Add(typeof(AbstractControl), (c) =>
            {
                AbstractControl control = (c as AbstractControl)!;
                control.SuggestedSize = defaultListHeight;
            });

            Actions.Add(typeof(ProgressBar), (c) =>
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
                        control.SuggestedSize = (Double.NaN, 250);
                    else
                        control.SuggestedSize = (250, Double.NaN);
                    control.PerformLayout();
                }

            });

            Actions.Add(typeof(PanelOkCancelButtons), (c) =>
            {
                PanelOkCancelButtons control = (c as PanelOkCancelButtons)!;
                control.HasBorder = true;
            });
        }

        private static ControlStateImages? buttonImages;

        public static ControlStateImages GetButtonImages(AbstractControl control) =>
            buttonImages ??= LoadButtonImages();

        private static ControlStateImages LoadButtonImages()
        {
            static Image LoadImage(string stateName)
            {
                var s = $"{UrlResPrefix}ButtonImages.ButtonImage{stateName}.png";
                return new Bitmap(s);
            }

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

        private static StackPanel CreatePanelWithButtons(string s)
        {
            VerticalStackPanel panel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = 5,
                ParentBackColor = true,
                ParentForeColor = true,
            };

            for (int i = 1; i < 4; i++)
            {
                var button = new Button()
                {
                    Text = s + " Button " + i.ToString(),
                    Margin = 5,
                };
                panel.Children.Add(button);
                button.Click += Button_Click;
            }

            return panel;

            static void Button_Click(object? sender, EventArgs e)
            {
                App.Log($"Button '{(sender as Button)?.Text}' Click");
            }
        }

        internal static IEnumerable<object> GetTenItems()
        {
            var items = new List<string>();

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

            return items;
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
            if (control is not Control panel)
                return;

            panel.SuggestedHeight = 250;
            panel.HasBorder = true;

#pragma warning disable
            Button OkButton = new()
            {
                Text = "1",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsDefault = true,
                Parent = panel,
            };

            Button CancelButton = new()
            {
                Text = "2",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsCancel = true,
                Parent = panel,
            };

            Button ApplyButton = new()
            {
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                Text = "3",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Parent = panel,
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
                listView.View = ListViewView.Details;
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
            var largeImageList = new ImageList() { ImageSize = new(32, 32) };

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames =
                allResourceNames.Where(x => x.StartsWith(AsmResPrefix + "ImageListIcons."));
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
