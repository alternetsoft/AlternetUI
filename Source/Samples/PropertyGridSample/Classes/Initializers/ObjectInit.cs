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

        public static string LoremIpsumSmall =
"Beneath a sky stitched with teacup clouds, the girl tiptoed across checkerboard moss. " +
"Each step made a peculiar sound—like libraries whispering to mushrooms. " +
"Trees bent inward to eavesdrop, their leaves rustling riddles only crickets could decipher.";

        public static string LoremIpsumVerySmall = "I see a sky with clouds.";

        public static string LoremIpsum = LoremIpsumSmall +
Environment.NewLine + Environment.NewLine +
"The map she carried was drawn entirely in nonsense, but somehow it felt correct. " +
"It pulsed faintly in her hands, humming with ink made from stolen dreams and marmalade." +
Environment.NewLine + Environment.NewLine +
"“Left is usually right,” said the rabbit-shaped shadow, bowing courteously. " +
"“Unless, of course, you're upside-down.”" +
Environment.NewLine + Environment.NewLine +
"And so, with a smile too wide for logic, she stepped forward—into a world where clocks " +
"melted politely and hats outgrew heads.";

        private static ImageLists? imageLists;
        
        public static SizeD DefaultListSize = new(defaultListHeight, defaultListHeight);

        static ObjectInit()
        {
            AddAction<ShapeControl>(InitShapeControl);
            AddAction<GenericItemControl>(InitGenericListItemControl);
            AddAction<CardPanelHeader>(InitCardPanelHeader);
            AddAction<RichToolTip>(InitRichToolTip);
            AddAction<DateTimePicker>(InitDateTimePicker);
            AddAction<DatePicker>(InitDatePicker);
            AddAction<TimePicker>(InitTimePicker);
            AddAction<ListPicker>(InitListPicker);
            AddAction<EditableListPicker>(InitListPicker);            
            AddAction<EnumPicker>(InitEnumPicker);
            AddAction<ColorPicker>(InitColorPicker);
            AddAction<FontNamePicker>(InitFontNamePicker);
            AddAction<TextBoxWithListPopup>(InitTextBoxWithListPopup);
            AddAction<XSlider>(InitGenericSlider);
            AddAction<XCheckBox>(InitStdCheckBox);
            AddAction<XRadioButton>(InitStdRadioButton);

            Actions.Add(typeof(PageSetupDialog), InitPageSetupDialog);
            Actions.Add(typeof(PrintPreviewDialog), InitPrintPreviewDialog);
            Actions.Add(typeof(PrintDialog), InitPrintDialog);
            Actions.Add(typeof(ContextMenu), InitContextMenu);
            Actions.Add(typeof(SplittedPanel), InitSplittedPanel);
            Actions.Add(typeof(ScrollViewer), InitScrollViewer);
            Actions.Add(typeof(ScrollablePanelSettings), InitScrollablePanelSettings);            
            Actions.Add(typeof(HorizontalStackPanel), InitStackPanel);
            Actions.Add(typeof(VerticalStackPanel), InitStackPanel);
            Actions.Add(typeof(StackPanel), InitStackPanel);
            Actions.Add(typeof(XScrollBar), InitXScrollBar);
            Actions.Add(typeof(SpeedButton), InitSpeedButton);
            Actions.Add(typeof(PictureBox), InitPictureBox);
            Actions.Add(typeof(ToolBar), InitGenericToolBar);
            Actions.Add(typeof(FindReplaceControl), InitFindReplaceControl);
            Actions.Add(typeof(ToolBarSet), InitGenericToolBarSet);
            Actions.Add(typeof(CardPanel), InitCardPanel);
            Actions.Add(typeof(TextBox), InitTextBox);
            Actions.Add(typeof(TextBoxAndLabel), InitTextBoxAndLabel);
            Actions.Add(typeof(TextBoxAndButton), InitTextBoxAndButton);
            Actions.Add(typeof(MultilineTextBox), InitMultilineTextBox);
            Actions.Add(typeof(Label), InitGenericLabel);
            Actions.Add(typeof(LabelAndButton), InitLabelAndButton);
            Actions.Add(typeof(LinkLabel), InitLinkLabel);
            Actions.Add(typeof(XButton), InitStdButton);
            Actions.Add(typeof(SpeedTextButton), InitSpeedTextButton);
            Actions.Add(typeof(SpeedColorButton), InitSpeedColorButton);
            Actions.Add(typeof(SideBarPanel), InitSideBarPanel);
            Actions.Add(typeof(TabControl), InitGenericTabControl);
            Actions.Add(typeof(VirtualListBox), InitListBoxItems);
            Actions.Add(typeof(FileListBox), InitFileListBox);
            Actions.Add(typeof(XListBox), InitStdListBox);
            Actions.Add(typeof(XComboBox), InitStdComboBox);
            Actions.Add(typeof(XCheckListBox), InitCheckListBox);

            Actions.Add(typeof(UserControl), (c) =>
            {
                var control = (c as UserControl)!;
                control.HasBorder = true;
                control.SuggestedSize = 200;
                control.ParentBackColor = true;
                control.Paint += (sender, e) =>
                {
                    e.Graphics.FillRectangle(control.RealBackgroundColor.AsBrush, e.ClientRectangle);
                    (sender as UserControl)?.DrawDefaultBackground(e);
                };
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

            Actions.Add(typeof(NumericUpDown), (c) =>
            {
                NumericUpDown control = (c as NumericUpDown)!;
                control.SuggestedWidth = 200;
            });

            Actions.Add(typeof(Border), (c) =>
            {
                var border = (c as Border)!;
                border.ParentBackColor = false;
                border.ParentForeColor = false;
                border.SuggestedSize = DefaultListSize;
                SetBackgrounds(border);

                border.Layout = LayoutStyle.Vertical;
                XButton button = new();
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

            Actions.Add(typeof(XTreeView), (c) =>
            {
                XTreeView treeView = (c as XTreeView)!;
                treeView.SuggestedSize = DefaultListSize;
                InitVirtualTreeControl(treeView);
            });

            Actions.Add(typeof(Panel), InitPanel);
            Actions.Add(typeof(PanelSettings), InitPanelSettings);

            Actions.Add(typeof(ResizableWindowBorder), (c) =>
            {
                ResizableWindowBorder control = (c as ResizableWindowBorder)!;
                control.IgnoreLayout = true;
                control.Size = 300;
                control.Location = (10, 10);
                control.Title = "This is title";

                control.MinimizeEnabled = true;
                control.MaximizeEnabled = true;
                control.HasSystemMenu = true;
                control.MinimizeButtonClick += (s, e) => App.Log("Minimize button clicked");
                control.MaximizeButtonClick += (s, e) => App.Log("Maximize button clicked");
                control.CloseButtonClick += (s, e) => App.Log("Close button clicked");
                control.IconClick += (s, e) => App.Log("Icon clicked");
            });

            Actions.Add(typeof(ResizableBorder), (c) =>
            {
                ResizableBorder control = (c as ResizableBorder)!;

                control.HasBorder = true;
                control.IgnoreLayout = true;
                control.Size = 300;
                control.Location = (10, 10);
            });

            Actions.Add(typeof(AbstractControl), (c) =>
            {
                AbstractControl control = (c as AbstractControl)!;
                control.SuggestedSize = defaultListHeight;
            });

            Actions.Add(typeof(XProgressBar), (c) =>
            {
                XProgressBar control = (c as XProgressBar)!;
                control.OrientationChanged += OrientationChanged;
                control.Value = 50;
                control.SuggestedWidth = 250;

                static void OrientationChanged(object? sender, EventArgs e)
                {
                    if (sender is not XProgressBar control)
                        return;
                    if (control.IsVertical)
                        control.SuggestedSize = (float.NaN, 250);
                    else
                        control.SuggestedSize = (250, float.NaN);
                }

            });

            Actions.Add(typeof(PanelOkCancelButtons), (c) =>
            {
                PanelOkCancelButtons control = (c as PanelOkCancelButtons)!;
                control.HasBorder = true;
            });
        }

        public static void InitStdCheckBox(XCheckBox control)
        {
            control.Text = "XCheckBox";
        }

        public static void InitStdRadioButton(XRadioButton control)
        {
            control.Text = "XRadioButton";
        }

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

        public static Image DefaultImage { get; } = Image.FromUrl(ResPrefixImage);

        public static ImageSet DefaultImageSet { get; } = ImageSet.FromUrl(ResPrefixImage);

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

        public static void InitDateTimePicker(DateTimePicker control)
        {
            control.Kind = DateTimePickerKind.DateTime;

            control.ValueChanged += (s, e) =>
            {
                App.LogReplace($"DateTimePicker: {control.Value}", "DateTimePicker:");
            };
        }

        public static void InitDatePicker(DatePicker control)
        {
        }

        public static void InitTimePicker(TimePicker control)
        {
        }

        public static void InitListPicker(ListPicker control)
        {
            control.Add("Item 1");
            control.Add("Item 2");
            control.Add("Item 3");
            control.Add("Item 4");
            control.Add("Item 5");
            control.Add("Item 6");
            control.Add("Item 7");
            control.Add("Item 8");

            control.Value = "Item 4";
        }

        public static void InitEnumPicker(EnumPicker control)
        {
            control.EnumType = typeof(HorizontalAlignment);
            control.Value = HorizontalAlignment.Center;
        }

        public static void InitGenericSlider(XSlider control)
        {
            control.Value = 4;
            control.SuggestedWidth = 250;
            control.OrientationChanged += OrientationChanged;

            static void OrientationChanged(object? sender, EventArgs e)
            {
                if (sender is not XSlider control)
                    return;
                if (control.IsVertical)
                    control.SuggestedSize = (float.NaN, 250);
                else
                    control.SuggestedSize = (250, float.NaN);
                control.PerformLayout();
            }

            control.ValueChanged += (s,e) =>
            {
                App.Invoke(() =>
                {
                    App.LogReplace(
                        $"GenericSlider: V: {control.Value}, LTS: {control.LeftTopSpacerSize}, W:{control.MaxLeftTopSpacerSize}",
                        "GenericSlider:");
                });
            };
        }

        public static void InitTextBoxWithListPopup(TextBoxWithListPopup control)
        {
            control.Text = "some text";

            var btn = control.ButtonCombo;

            btn.Add("Item 1");
            btn.Add("Item 2");
            btn.Add("Item 3");
            btn.Add("Item 4");
            btn.Add("Item 5");
            btn.Add("Item 6");
            btn.Add("Item 7");
            btn.Add("Item 8");

            control.SyncTextAndComboButton();
        }

        public static void InitFontNamePicker(FontNamePicker control)
        {
        }

        public static void InitColorPicker(ColorPicker control)
        {
        }

        public static void InitRichToolTip(RichToolTip control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.MaxWidth = 450;
            control.MinHeight = 300;

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
                var button = new XButton()
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
                App.Log($"Button '{(sender as XButton)?.Text}' Click");
            }
        }

        public static IEnumerable<object> GetTenItems()
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
            Label label = new();

            StringBuilder sb = new();

            for(int i = 1; i <= 20; i++)
            {
                sb.AppendLine(LoremIpsum);
                sb.AppendLine(Environment.NewLine);
            }

            label.Text = sb.ToString();
            label.MaxWidth = 200;
            label.WordWrap = true;

            label.Parent = sv.Content;
        }

        public static void InitScrollablePanelSettings(object control)
        {
            if(control is not ScrollablePanelSettings sv)
                return;
            sv.SuggestedHeight = 300;
            InitPanelSettings(sv.Panel);
        }

        public static void InitStackPanel(object control)
        {
            if (control is not Control panel)
                return;

            panel.SuggestedHeight = 250;
            panel.HasBorder = true;

#pragma warning disable
            XButton OkButton = new()
            {
                Text = "1",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsDefault = true,
                Parent = panel,
            };

            XButton CancelButton = new()
            {
                Text = "2",
                Margin = PanelOkCancelButtons.DefaultButtonMargin,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsCancel = true,
                Parent = panel,
            };

            XButton ApplyButton = new()
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

        public static void InitVirtualTreeControl(XTreeView control)
        {
            if (App.SafeWindow.UseSmallImages)
                control.ImageList = LoadImageLists().Small;
            else
                control.ImageList = LoadImageLists().Large;

            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            AddItems(control, 10);
        }

        public static void InitTreeView(XTreeView control)
        {
            if (App.SafeWindow.UseSmallImages)
                control.ImageList = LoadImageLists().Small;
            else
                control.ImageList = LoadImageLists().Large;

            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            AddItems(control, 10);
        }

        public static int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        public static void AddItems(XTreeView treeView, int count)
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
