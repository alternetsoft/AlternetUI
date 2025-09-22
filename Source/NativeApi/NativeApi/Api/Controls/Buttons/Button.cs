#pragma warning disable
using ApiCommon;
using System;


namespace NativeApi.Api
{
    public class Button : Control
    {
        public static bool ImagesEnabled { get; set; }

        public event EventHandler? Click;

        public bool ExactFit { get; set; }

        public bool HasBorder { get; set; }

        public Image? NormalImage { get; set; }

        public Image? HoveredImage { get; set; }

        public Image? PressedImage { get; set; }

        public Image? DisabledImage { get; set; }

        public Image? FocusedImage { get; set; }

        public bool TextVisible { get; set; }
        public int TextAlign { get; set; }
        public void SetImagePosition(int dir) {}
        public void SetImageMargins(float x, float y) {}
    }
}