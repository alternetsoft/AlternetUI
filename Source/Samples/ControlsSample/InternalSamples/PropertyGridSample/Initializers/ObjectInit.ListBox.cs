using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitVListBox(object control)
        {
            if (control is not VListBox listBox)
                return;

            for (int i = 0; i < 150; i++)
            {
                ListControlItem item = new($"Item {i}");
                listBox.Add(item);
            }

            var imageSize = 24; /* image sizes are always in pixels */
            var item5 = listBox.SafeItem(5);
            item5!.Text = "Bold item at right";
            item5!.Alignment = GenericAlignment.CenterRight;
            item5!.FontStyle = FontStyle.Bold;
            item5!.MinHeight = listBox.PixelToDip(imageSize);
            item5!.Image = KnownSvgImages.GetForSize(listBox.GetSvgColor(), imageSize).ImgBold.AsImage();
            item5!.SelectedImage =
                KnownSvgImages.GetForSize(SystemColors.HighlightText, imageSize).ImgBold.AsImage();
            item5!.DisabledImage =
                KnownSvgImages.GetForSize(listBox.GetSvgColor(KnownSvgColor.Disabled), imageSize).ImgBold.AsImage();

            const string ResPrefix = "embres:ControlsSample.Resources.ToolBarPng.Large.";
            string CalendarUrl = $"{ResPrefix}Calendar32.png";
            string PencilUrl = $"{ResPrefix}Pencil32.png";
            string PhotoUrl = $"{ResPrefix}Photo32.png";

            var item6 = listBox.SafeItem(6);
            item6!.Alignment = GenericAlignment.Center;
            item6!.Image = new Bitmap(CalendarUrl);
            item6!.DisabledImage = item6!.Image.ToGrayScale();
            item6!.ForegroundColor = Color.Green;
            item6!.BackgroundColor = Color.Lavender;
            item6!.Text = "Green item at center";

            var item8 = listBox.SafeItem(8);
            item8!.Text = "Custom height/align";
            item8!.MinHeight = 60;
            item8!.Alignment = GenericAlignment.Bottom | GenericAlignment.CenterHorizontal;
            item8!.Image = new Bitmap(PencilUrl);
            item8!.DisabledImage = item8!.Image.ToGrayScale();
            item8!.ForegroundColor = Color.Indigo;
            item8!.BackgroundColor = Color.LightSkyBlue;

            var item9 = listBox.SafeItem(9);
            item9!.FontStyle = FontStyle.Underline;
            item9!.Text = "Underlined item";

            listBox.Count = 5000;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if (sender is not VListBox listBox)
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