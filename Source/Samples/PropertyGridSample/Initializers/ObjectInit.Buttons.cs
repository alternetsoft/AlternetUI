﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitButton(object control)
        {
            if (control is not Button button)
                return;
            button.Text = "Button";
            button.StateImages = ButtonImages;
            button.SuggestedHeight = 100;
            button.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitSpeedTextButton(object control)
        {
            if (control is not SpeedTextButton button)
                return;
            button.Text = "Sample Text";
        }

        public static void InitSpeedColorButton(object control)
        {
            if (control is not SpeedColorButton button)
                return;
            button.SuggestedSize = 64;
            button.ColorImageSize = 56;
            button.Value = Color.Red;
        }

        public static void InitSpeedButton(object control)
        {
            if (control is not SpeedButton button)
                return;
            button.Text = "speedButton";

            var images = KnownSvgImages.GetForSize(
                button.GetSvgColor(KnownSvgColor.Normal),
                32);
            var imagesDisabled = KnownSvgImages.GetForSize(
                button.GetSvgColor(KnownSvgColor.Disabled),
                32);

            button.ImageSet = images.ImgOk;
            button.DisabledImageSet = imagesDisabled.ImgOk;
        }
    }
}