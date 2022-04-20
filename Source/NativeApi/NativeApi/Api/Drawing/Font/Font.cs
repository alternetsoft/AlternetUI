using System;

namespace NativeApi.Api
{
    public class Font
    {
        public void Initialize(GenericFontFamily genericFamily, string? familyName, double emSizeInPoints, FontStyle style) => throw new Exception();
        public void InitializeWithDefaultFont() => throw new Exception();

        public static bool IsFamilyValid(string fontFamily) => throw new Exception();
        public static string GetGenericFamilyName(GenericFontFamily genericFamily) => throw new Exception();

        public static string[] Families { get => throw new Exception(); }

        public string Name { get => throw new Exception(); }
        public double SizeInPoints { get => throw new Exception(); }
        public FontStyle Style { get => throw new Exception(); }

        public string Description { get => throw new Exception(); }
        public bool IsEqualTo(Font other) => throw new Exception();
        public string Serialize() => throw new Exception();
    }
}