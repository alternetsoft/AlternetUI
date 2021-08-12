using System;

namespace NativeApi.Api
{
    public class Font
    {
        public void Initialize(GenericFontFamily genericFamily, string? familyName, float emSize) => throw new Exception();
        public void InitializeWithDefaultFont() => throw new Exception();

        public static bool IsFamilyValid(string fontFamily) => throw new Exception();
        public static string GetGenericFamilyName(GenericFontFamily genericFamily) => throw new Exception();

        public static string[] Families { get => throw new Exception(); }

        public string Name { get => throw new Exception(); }
        public float Size { get => throw new Exception(); }

        public string ToString_() => throw new Exception();
        public bool IsEqualTo(Font other) => throw new Exception();
        public int GetHashCode_() => throw new Exception();
    }
}