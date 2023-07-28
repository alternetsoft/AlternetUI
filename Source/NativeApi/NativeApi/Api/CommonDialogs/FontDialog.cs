#pragma warning disable
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class FontDialog
    {
        public bool AllowSymbols { get; set; }
        public bool ShowHelp { get; set; }
        public bool EnableEffects { get; set; }

        public int RestrictSelection { get; set; }

        public Color Color { get; set; }

        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public void SetRange(int minRange, int maxRange) { }

        public Font Font { get; set; }

        public string? Title { get ; set; }
    }
}