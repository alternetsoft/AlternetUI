using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitVListBox(object control)
        {
            string ResPrefix2 = $"{UrlResPrefix}ToolBarPng.Large.";
            string CalendarUrl = $"{ResPrefix2}Calendar32.png";
            string PencilUrl = $"{ResPrefix2}Pencil32.png";
            string PhotoUrl = $"{ResPrefix2}Photo32.png";

            if (control is not VirtualListBox listBox)
                return;

            listBox.HScrollBarVisible = true;

            for (int i = 0; i < 150; i++)
            {
                ListControlItem newItem = new($"Item {i}");
                listBox.Add(newItem);
            }

            var firstIndex = 2;

            var item = listBox.RequiredItem(0);
            item.Alignment = GenericAlignment.Center;
            item.Text = string.Empty;
            item.Image = new Bitmap(PhotoUrl);

            var imageSize = 24; /* image sizes are always in pixels */
            item = listBox.RequiredItem(firstIndex);
            item.Text = "Bold item at right";
            item.Alignment = GenericAlignment.CenterRight;
            item.FontStyle = FontStyle.Bold;
            item.MinHeight = listBox.PixelToDip(imageSize);
            item.Image = KnownSvgImages.ImgBold.AsNormalImage(imageSize, listBox.IsDarkBackground);
            item.SelectedImage =
                KnownSvgImages.ImgBold.AsImageSet(imageSize, KnownSvgColor.HighlightText, listBox.IsDarkBackground)?.AsImage();
            item.DisabledImage =
                KnownSvgImages.ImgBold.AsDisabledImage(imageSize, listBox.IsDarkBackground);

            item = listBox.RequiredItem(firstIndex + 1);
            item.Alignment = GenericAlignment.Center;
            item.Image = new Bitmap(CalendarUrl);
            item.CheckState = CheckState.Indeterminate;
            item.DisabledImage = item!.Image.ToGrayScale();
            item.ForegroundColor = Color.Green;
            item.BackgroundColor = Color.Lavender;
            item.Text = "Green item at center";

            item = listBox.RequiredItem(firstIndex + 2);
            item.Text = "Custom height/align";
            item.CheckBoxVisible = false;
            item.MinHeight = 60;
            item.Alignment = GenericAlignment.Bottom | GenericAlignment.CenterHorizontal;
            item.Image = new Bitmap(PencilUrl);
            item.DisabledImage = item!.Image.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;

            item = listBox.RequiredItem(firstIndex + 3);
            item.FontStyle = FontStyle.Underline;
            item.CheckState = CheckState.Checked;
            item.Text = "Underlined item";

            item = listBox.RequiredItem(firstIndex + 5);
            item.Text = "Custom border";
            item.Alignment = GenericAlignment.Center;
            item.CheckBoxVisible = false;
            item.Border = new();
            item.Border.Color = Color.Red;
            item.Border.UniformCornerRadius = 50;
            item.Border.UniformRadiusIsPercent = true;

            item = listBox.RequiredItem(firstIndex + 6);
            item.Text = LoremIpsum.Replace(StringUtils.OneNewLine, StringUtils.OneSpace);

            listBox.Count = 5000;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if (sender is not VirtualListBox listBox)
                    return;
                if (e.ItemIndex >= listBox.Items.Count)
                {
                    e.Result = "Custom item " + e.ItemIndex.ToString();
                    e.Handled = true;
                }
            }
        }

        public static void InitListBox(object control)
        {
            if (control is not ListBox listBox)
                return;
            listBox.SuggestedSize = defaultListSize;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitCheckListBox(object control)
        {
            if (control is not CheckListBox listBox)
                return;
            listBox.SuggestedSize = defaultListHeight;
            listBox.Items.AddRange(GetTenItems());
        }

        public static void InitComboBox(object control)
        {
            if (control is not ComboBox comboBox)
                return;
            comboBox.Items.AddRange(GetTenItems());
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
        }

        public static void InitColorComboBox(object control)
        {
            if (control is not ColorComboBox comboBox)
                return;
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
            comboBox.Value = Color.Red;
        }
    }
}