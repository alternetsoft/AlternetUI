#pragma warning disable
using ApiCommon;
using System;


namespace NativeApi.Api
{
    public class Button : Control
    {
        public static bool ImagesEnabled { get; set; }

        public event EventHandler? Click;

        public string Text { get; set; }

        public bool ExactFit { get; set; }

        public bool IsDefault { get; set; }

        public bool HasBorder { get; set; }

        public bool IsCancel { get; set; }

        public Image? NormalImage { get; set; }

        public Image? HoveredImage { get; set; }

        public Image? PressedImage { get; set; }

        public Image? DisabledImage { get; set; }

        public Image? FocusedImage { get; set; }

        public bool AcceptsFocus { get;set;}
        public bool AcceptsFocusFromKeyboard { get;set;}
        public bool AcceptsFocusRecursively { get;set;}

        public bool TextVisible { get; set; }
        public int TextAlign { get; set; }
        public void SetImagePosition(int dir) {}
        public void SetImageMargins(double x, double y) {}
    }
}