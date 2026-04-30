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

        public void SetColor(Color color) => throw new Exception();

        public byte GetColorR() => throw new Exception();
        public byte GetColorG() => throw new Exception();
        public byte GetColorB() => throw new Exception();
        public byte GetColorA() => throw new Exception();
        public byte GetColorState() => throw new Exception();

        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public void SetRange(int minRange, int maxRange) { }

        public void SetInitialFont(
            GenericFontFamily genericFamily,
            string? familyName,
            float emSizeInPoints,
            FontStyle style) => throw new Exception();

        public string ResultFontName { get; }
        public float ResultFontSizeInPoints { get; }
        public FontStyle ResultFontStyle { get; }

        public string? Title { get ; set; }
    }
}