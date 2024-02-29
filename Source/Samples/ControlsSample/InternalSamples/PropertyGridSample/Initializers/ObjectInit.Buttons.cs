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
        public static void LogClick(object? sender, EventArgs e)
        {
            Application.Log($"{sender?.GetType()} Click");
        }

        public static void InitButton(object control)
        {
            if (control is not Button button)
                return;
            button.Text = "Butt&on";
            button.Click += LogClick;
            button.StateImages = ButtonImages;
            button.SuggestedHeight = 100;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.MouseWheel += Button_MouseWheel;
        }

        private static void Button_MouseWheel(object sender, MouseEventArgs e)
        {
            Application.Log($"Button MouseWheel: {e.Delta}, {e.Timestamp}");
        }

        public static void InitSpeedTextButton(object control)
        {
            if (control is not SpeedTextButton button)
                return;
            button.Text = "Sample Text";
            button.Click += LogClick;
        }

        public static void InitSpeedColorButton(object control)
        {
            if (control is not SpeedColorButton button)
                return;
            button.ColorImageSize = 56;
            button.Value = Color.Red;
            button.Click += LogClick;
        }

        public static void InitSpeedButton(object control)
        {
            if (control is not SpeedButton button)
                return;
            button.Text = "speedButton";
            button.TextVisible = true;
            button.Click += LogClick;
            button.LoadSvg(KnownSvgUrls.UrlImageOk, 32);
        }
    }
}