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
        public static void InitPictureBox(object control)
        {
            if (control is not PictureBox pictureBox)
                return;
            pictureBox.ImageStretch = false;
            SetBackgrounds(pictureBox);
            pictureBox.SuggestedSize = 150;
            pictureBox.ParentBackColor = true;

            pictureBox.Borders ??= new();
            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 15;
            border.UniformRadiusIsPercent = true;
            pictureBox.Borders.SetAll(border);

            pictureBox.Image = DefaultImage;

            pictureBox.DisabledImage = DefaultImage.ToGrayScale();

            pictureBox.VisualStateChanged += VisualStateChanged;
            pictureBox.Click += PictureBox_Click;
            pictureBox.Activated += PictureBox_Activated;
            pictureBox.Deactivated += PictureBox_Deactivated;

            static void PictureBox_Deactivated(object? sender, EventArgs e)
            {
                App.Log("PictureBox Deactivated");
            }

            static void PictureBox_Activated(object? sender, EventArgs e)
            {
                App.Log("PictureBox Activated");
            }

            static void PictureBox_Click(object? sender, EventArgs e)
            {
                App.Log("PictureBox Click");
            }

            static void VisualStateChanged(object? sender, EventArgs e)
            {
                App.LogNameValueReplace("PictureBox.VisualState", (sender as PictureBox)?.VisualState);
            }
        }
    }
}
