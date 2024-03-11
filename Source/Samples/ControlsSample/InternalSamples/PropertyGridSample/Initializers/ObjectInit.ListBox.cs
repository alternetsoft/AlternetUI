﻿using System;
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
                ListControlItem newItem = new($"Item {i}");
                listBox.Add(newItem);
            }

            var firstIndex = 2;

            var imageSize = 24; /* image sizes are always in pixels */
            var item = listBox.SafeItem(firstIndex);
            item!.Text = "Bold item at right";
            item!.Alignment = GenericAlignment.CenterRight;
            item!.FontStyle = FontStyle.Bold;
            item!.MinHeight = listBox.PixelToDip(imageSize);
            item!.Image = KnownSvgImages.GetForSize(listBox.GetSvgColor(), imageSize).ImgBold.AsImage();
            item!.SelectedImage =
                KnownSvgImages.GetForSize(SystemColors.HighlightText, imageSize).ImgBold.AsImage();
            item!.DisabledImage =
                KnownSvgImages.GetForSize(listBox.GetSvgColor(KnownSvgColor.Disabled), imageSize).ImgBold.AsImage();

            const string ResPrefix = "embres:ControlsSample.Resources.ToolBarPng.Large.";
            string CalendarUrl = $"{ResPrefix}Calendar32.png";
            string PencilUrl = $"{ResPrefix}Pencil32.png";
            string PhotoUrl = $"{ResPrefix}Photo32.png";

            item = listBox.RequiredItem(firstIndex + 1);
            item.Alignment = GenericAlignment.Center;
            item.Image = new Bitmap(CalendarUrl);
            item.DisabledImage = item!.Image.ToGrayScale();
            item.ForegroundColor = Color.Green;
            item.BackgroundColor = Color.Lavender;
            item.Text = "Green item at center";

            item = listBox.RequiredItem(firstIndex + 2);
            item.Text = "Custom height/align";
            item.MinHeight = 60;
            item.Alignment = GenericAlignment.Bottom | GenericAlignment.CenterHorizontal;
            item.Image = new Bitmap(PencilUrl);
            item.DisabledImage = item!.Image.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;

            item = listBox.RequiredItem(firstIndex + 3);
            item.FontStyle = FontStyle.Underline;
            item.Text = "Underlined item";

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