﻿using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Button : Control
    {
        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool IsDefault { get; set; }

        public bool HasBorder { get; set; }

        public bool IsCancel { get; set; }

        public Image? NormalImage { get; set; }

        public Image? HoveredImage { get; set; }

        public Image? PressedImage { get; set; }

        public Image? DisabledImage { get; set; }
    }
}