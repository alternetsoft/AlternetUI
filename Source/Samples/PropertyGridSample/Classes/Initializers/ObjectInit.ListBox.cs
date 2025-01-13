﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void AddDefaultOwnerDrawItems(
            Control control,
            Action<ListControlItem> addAction,
            bool addLong = true)
        {
            string ResPrefix2 = $"{UrlResPrefix}ToolBarPng.Large.";
            string CalendarUrl = $"{ResPrefix2}Calendar32.png";
            string PencilUrl = $"{ResPrefix2}Pencil32.png";
            string PhotoUrl = $"{ResPrefix2}Photo32.png";

            var imageSize = 24; /* image sizes are always in pixels */

            ListControlItem item = new();
            item.Alignment = HVAlignment.Center;
            item.DisplayText = string.Empty;
            item.Text = "This is some text";
            item.Image = new Bitmap(PhotoUrl);
            addAction(item);

            item = new();
            item.Text = "Bold item CenterRight";
            item.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
            item.FontStyle = FontStyle.Bold;
            item.MinHeight = control.PixelToDip(imageSize);
            item.SvgImage = KnownSvgImages.ImgBold;
            item.SvgImageSize = imageSize;
/*
            item.Image = KnownSvgImages.ImgBold.AsNormalImage(imageSize, control.IsDarkBackground);
            item.SelectedImage =
                KnownSvgImages.ImgBold.AsImageSet(
                    imageSize,
                    KnownSvgColor.HighlightText,
                    control.IsDarkBackground)?.AsImage();
            item.DisabledImage =
                KnownSvgImages.ImgBold.AsDisabledImage(imageSize, control.IsDarkBackground);
*/
            addAction(item);

            item = new();
            item.Alignment = HVAlignment.Center;
            item.Image = new Bitmap(CalendarUrl);
            item.CheckState = CheckState.Indeterminate;
            item.DisabledImage = item.Image?.ToGrayScale();
            item.ForegroundColor = Color.Green;
            item.BackgroundColor = Color.Lavender;
            item.Text = "Green <b>item</b> at center";
            item.LabelFlags = DrawLabelFlags.TextHasBold;
            addAction(item);

            item = new();
            item.Text = "Height=60/BottomCenter";
            item.CheckBoxVisible = false;
            item.MinHeight = 60;
            item.Alignment = (HorizontalAlignment.Center, VerticalAlignment.Bottom);
            item.Image = new Bitmap(PencilUrl);
            item.DisabledImage = item!.Image.ToGrayScale();
            item.ForegroundColor = Color.Indigo;
            item.BackgroundColor = Color.LightSkyBlue;
            addAction(item);

            item = new();
            item.FontStyle = FontStyle.Underline;
            item.CheckState = CheckState.Checked;
            item.Text = "Underlined item";
            addAction(item);

            item = new();
            item.Font = Control.DefaultFont.Scaled(1.5);
            item.Text = "Custom Font";
            addAction(item);

            item = new();
            item.Text = "Custom border";
            item.Alignment = HVAlignment.Center;
            item.CheckBoxVisible = false;
            item.Border = new();
            item.Border.Color = Color.Red;
            item.Border.UniformCornerRadius = 25;
            item.Border.UniformRadiusIsPercent = true;
            addAction(item);

            if (addLong)
            {
                item = new();
                item.Text = LoremIpsum.Replace(StringUtils.OneNewLine, StringUtils.OneSpace);
                addAction(item);
            }

            for (int i = 0; i < 150; i++)
            {
                ListControlItem newItem = new($"Item {i}");

                if (i == 128)
                    newItem.DisplayText = newItem.Text + ": dd";

                addAction(newItem);
            }
        }

        public static void AddManyItems(VirtualListBox listBox)
        {
            listBox.DoInsideUpdate(() =>
            {
                for(int i = 0; i < 5000; i++)
                    listBox.Items.Add(new($"Item #{LogUtils.GenNewId()}"));

                App.Log("Added 5000 items");
            });

            listBox.SelectLastItemAndScroll();
        }

        public static void InitVListBox(object control)
        {
            if (control is not VirtualListBox listBox)
                return;

            AddDefaultOwnerDrawItems(listBox, (s) =>
            {
                listBox.Add(s);
            });

            listBox.HorizontalScrollbar = true;
            listBox.Count = 200;
            listBox.CustomItemText += ListBox_CustomItemText;

            static void ListBox_CustomItemText(object? sender, GetItemTextEventArgs e)
            {
                if(string.IsNullOrEmpty(e.Result))
                {
                    e.Result = "Virtual item " + e.ItemIndex.ToString();
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

        public static void InitFontComboBox(object control)
        {
            if (control is not FontComboBox comboBox)
                return;
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.SuggestedWidth = 200;
            comboBox.Value = Control.DefaultFont.Name;
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