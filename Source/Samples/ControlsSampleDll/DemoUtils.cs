using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Alternet.UI
{
    /// <summary>
    /// Contains utility methods used in demo applications.
    /// </summary>
    public static class DemoUtils
    {
        public const int DefaultListHeight = 250;

        public static SizeD DefaultListSize = new(DefaultListHeight, DefaultListHeight);

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

        public const string LoremIpsumSmallSingleLine =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.";

        public const string LoremIpsumSmallThreeLines =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n" +
            "Suspendisse tincidunt orci vitae arcu congue commodo.\n" +
            "Proin fermentum rhoncus dictum.";

        public static string UrlResPrefix
            = AssemblyUtils.GetImageUrlInAssembly(typeof(DemoUtils).Assembly, "Resources.");

        public static string ResPrefix2 = $"{UrlResPrefix}ToolBarPng.Large.";

        public static string CalendarUrl = $"{ResPrefix2}Calendar32.png";
        public static string PencilUrl = $"{ResPrefix2}Pencil32.png";
        public static string PhotoUrl = $"{ResPrefix2}Photo32.png";

        private static int newItemIndex = 0;

        public static void AddDefaultOwnerDrawItemsForTreeView(
            Control control,
            Action<TreeViewItem> addAction,
            bool addLong = true)
        {
            var svgImageSize = 24; /* image sizes are always in pixels */

            TreeViewItem item = new();
            item.DisplayText = "This is display text";
            item.Text = "This is some text";
            item.CheckBoxVisible = true;
            item.Image = Image.FromUrlCached(PhotoUrl);
            addAction(item);

            item = new();
            item.Text = "Bold item (right, vert center)";
            item.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
            item.FontStyle = FontStyle.Bold;
            item.MinHeight = control.PixelToDip(svgImageSize) * 3;
            item.SvgImage = KnownSvgImages.ImgBold;
            item.SvgImageSize = svgImageSize;
            addAction(item);

            addAction(CreateGreenBoldItem());

            item = new();
            item.Text = "H = 60 (bottom, center)";
            item.CheckBoxVisible = false;
            item.MinHeight = 60;
            item.Alignment = (HorizontalAlignment.Center, VerticalAlignment.Bottom);
            item.Image = Image.FromUrlCached(PencilUrl);
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;
            addAction(item);

            item = new();
            item.FontStyle = FontStyle.Underline;
            item.CheckState = CheckState.Checked;
            item.Text = "Underlined item";
            item.ToolTip = "Custom tooltip for item";
            item.IsToolTipVisible = true;
            addAction(item);

            item = new();
            item.Font = Control.DefaultFont.Scaled(1.5f);
            item.Text = "Custom Font";
            addAction(item);

            item = new();
            item.Text = "Custom border";
            item.Alignment = HVAlignment.Center;
            item.CheckBoxVisible = false;
            item.Border = new();
            item.Border.Color = LightDarkColors.Red;
            item.Border.UniformCornerRadius = 25;
            item.Border.UniformRadiusIsPercent = true;
            addAction(item);

            if (addLong)
            {
                item = new();
                item.Text = LoremIpsumSmall;
                addAction(item);
            }

            addAction(new TreeViewSeparatorItem());

            for (int i = 0; i < 150; i++)
            {
                TreeViewItem newItem = new($"Item {i}");

                if (i == 128)
                    newItem.DisplayText = newItem.Text + ": dd";

                addAction(newItem);
            }
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

        public static TreeViewItem CreateGreenBoldItem()
        {
            TreeViewItem item = new();
            InitGreenBoldItem(item);
            return item;
        }

        public static void InitGreenBoldItem(ListControlItem item)
        {
            item.Alignment = HVAlignment.Center;
            item.Image = Image.FromUrlCached(CalendarUrl);
            item.CheckState = CheckState.Indeterminate;
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.White;
            item.BackgroundColor = Color.ForestGreen;
            item.Text = "Green <b>item</b> at center";
            item.LabelFlags = DrawLabelFlags.TextHasBold;
        }

        public static void SetDefaultOwnerDrawItemsForListBox(
            VirtualListBox control,
            bool addLong = true)
        {
            ListSource items = new();

            AddDefaultOwnerDrawItemsForListBox(
                control,
                (item) =>
                {
                    items.Add(item);
                },
                addLong);

            control.SetItemsFast(items, VirtualListBox.SetItemsKind.ChangeField);
        }

        public static void InitListBoxItems(object control)
        {
            if (control is not VirtualListBox listBox)
                return;

            SetDefaultOwnerDrawItemsForListBox(listBox);

            listBox.HorizontalScrollbar = true;
            listBox.Count = 200;
            listBox.SuggestedSize = DefaultListSize;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if (string.IsNullOrEmpty(e.Result))
                {
                    e.Result = "Virtual item " + e.ItemIndex.ToString();
                    e.Handled = true;
                }
            }
        }

        public static void AddDefaultOwnerDrawItemsForListBox(
            Control control,
            Action<ListControlItem> addAction,
            bool addLong = true)
        {
            var svgImageSize = 24; /* image sizes are always in pixels */

            ListControlItem item = new();
            item.DisplayText = "This is display text";
            item.Text = "This is some text";
            item.CheckBoxVisible = true;
            item.Image = Image.FromUrlCached(PhotoUrl);
            addAction(item);

            item = new();
            item.Text = "Bold item (right, vert center)";
            item.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
            item.FontStyle = FontStyle.Bold;
            item.MinHeight = control.PixelToDip(svgImageSize) * 3;
            item.SvgImage = KnownSvgImages.ImgBold;
            item.SvgImageSize = svgImageSize;
            addAction(item);

            addAction(CreateGreenBoldItem());

            item = new();
            item.Text = "H = 60 (bottom, center)";
            item.CheckBoxVisible = false;
            item.MinHeight = 60;
            item.Alignment = (HorizontalAlignment.Center, VerticalAlignment.Bottom);
            item.Image = Image.FromUrlCached(PencilUrl);
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;
            addAction(item);

            item = new();
            item.FontStyle = FontStyle.Underline;
            item.CheckState = CheckState.Checked;
            item.Text = "Underlined item";
            item.ToolTip = "Custom tooltip for item";
            item.IsToolTipVisible = true;
            addAction(item);

            item = new();
            item.Font = Control.DefaultFont.Scaled(1.5f);
            item.Text = "Custom Font";
            addAction(item);

            item = new();
            item.Text = "Custom border";
            item.Alignment = HVAlignment.Center;
            item.CheckBoxVisible = false;
            item.Border = new();
            item.Border.Color = LightDarkColors.Red;
            item.Border.UniformCornerRadius = 25;
            item.Border.UniformRadiusIsPercent = true;
            addAction(item);

            if (addLong)
            {
                item = new();
                item.Text = LoremIpsumSmall;
                addAction(item);
            }

            addAction(new ListControlSeparatorItem());

            for (int i = 0; i < 150; i++)
            {
                ListControlItem newItem = new($"Item {i}");

                if (i == 128)
                    newItem.DisplayText = newItem.Text + ": dd";

                addAction(newItem);
            }
        }
    }
}
