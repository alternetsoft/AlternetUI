#pragma warning disable
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_font.html
    public class Font
    {
        public SizeI GetPixelSize() => default;
        public bool IsUsingSizeInPixels() => default;
        public int GetNumericWeight() => default;
        public bool GetUnderlined() => default;
        public bool GetItalic() => default;
        public bool GetStrikethrough() => default;
        public int GetEncoding() => default;
        public bool IsFixedWidth() => default;
        public static int GetDefaultEncoding() => default;
        public static void SetDefaultEncoding(int encoding) { }
        public int GetWeight() => default;

        public void Initialize(GenericFontFamily genericFamily, NativeStringSpan familyName,
            float emSizeInPoints, FontStyle style) => throw new Exception();
        public void InitializeWithDefaultFont() => throw new Exception();
        public void InitializeWithDefaultMonoFont() => throw new Exception();
        public void InitializeFromFont(Font font) { }

        public static bool IsFamilyValid(NativeStringSpan fontFamily) => throw new Exception();
        public static NativeStringSpan GetGenericFamilyName(GenericFontFamily genericFamily) => throw new Exception();

        public static NativeStringSpan GetFamilies() => throw new Exception();

        public NativeStringSpan GetName() => throw new Exception();
        public float SizeInPoints { get => throw new Exception(); }
        public FontStyle Style { get => throw new Exception(); }

        public NativeStringSpan GetDescription() => throw new Exception();
        public bool IsEqualTo(Font other) => throw new Exception();
        public NativeStringSpan Serialize() => throw new Exception();
    }
}