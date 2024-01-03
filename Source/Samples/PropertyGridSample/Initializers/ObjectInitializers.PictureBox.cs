﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInitializers
    {
        public static void InitPictureBox(object control)
        {
            if (control is not PictureBox pictureBox)
                return;
            pictureBox.ImageStretch = false;
            SetBackgrounds(pictureBox);
            pictureBox.SuggestedSize = 150;

            pictureBox.Borders ??= new();
            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 15;
            border.UniformRadiusIsPercent = true;
            pictureBox.Borders.SetAll(border);

            pictureBox.Image = DefaultImage;

            pictureBox.DisabledImage = DefaultImage.ToGrayScale();

            pictureBox.CurrentStateChanged += CurrentStateChanged;
            pictureBox.Click += PictureBox_Click;

            static void PictureBox_Click(object? sender, EventArgs e)
            {
                Application.Log("PictureBox Click");
            }

            static void CurrentStateChanged(object? sender, EventArgs e)
            {
                Application.LogNameValue("PictureBox.CurrentState", (sender as PictureBox)?.CurrentState);
            }
        }
    }
}
