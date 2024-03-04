#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace NativeApi.Api
{
    public class TextBoxTextAttr
    {
        public static void Delete(IntPtr attr) { }

        public static void DeleteRichTextAttr(IntPtr attr) { }

        public static void Copy(IntPtr toAttr, IntPtr fromAttr) {}

        public static IntPtr CreateTextAttr() => throw new Exception();

        public static IntPtr CreateRichTextAttr() => default;

        public static IntPtr RichGetTextBoxAttr(IntPtr attr) => default;

        public static void SetTextColor(IntPtr attr, Color colText) {}
        public static void SetBackgroundColor(IntPtr attr, Color colBack) => 
            throw new Exception();
        public static void SetAlignment(IntPtr attr, int alignment) => 
            throw new Exception();

        public static void SetFontPointSize(IntPtr attr, int pointSize) => 
            throw new Exception();
        public static void SetFontStyle(IntPtr attr, int fontStyle) => 
            throw new Exception();
        public static void SetFontWeight(IntPtr attr, int fontWeight) => 
            throw new Exception();
        public static void SetFontFaceName(IntPtr attr, string faceName) => 
            throw new Exception();
        public static void SetFontUnderlined(IntPtr attr, bool underlined) => 
            throw new Exception();
        public static void SetFontUnderlinedEx(IntPtr attr, int type, Color colour) => 
            throw new Exception();
        public static void SetFontStrikethrough(IntPtr attr, bool strikethrough) => 
            throw new Exception();
        public static void SetFontFamily(IntPtr attr, int family) => 
            throw new Exception();

        public static Color GetTextColor(IntPtr attr) => throw new Exception();
        public static Color GetBackgroundColor(IntPtr attr) => throw new Exception();
        public static int GetAlignment(IntPtr attr) => throw new Exception();
        public static void SetURL(IntPtr attr, string url) => throw new Exception();
        public static void SetFlags(IntPtr attr, long flags) => throw new Exception();
        public static void SetParagraphSpacingAfter(IntPtr attr, int spacing) =>
            throw new Exception();
        public static void SetParagraphSpacingBefore(IntPtr attr, int spacing) =>
            throw new Exception();
        public static void SetLineSpacing(IntPtr attr, int spacing) =>
            throw new Exception();
        public static void SetBulletStyle(IntPtr attr, int style) =>
            throw new Exception();
        public static void SetBulletNumber(IntPtr attr, int n) =>
            throw new Exception();
        public static void SetBulletText(IntPtr attr, string text) =>
            throw new Exception();
        public static void SetPageBreak(IntPtr attr, bool pageBreak) =>
            throw new Exception();
        public static void SetCharacterStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public static void SetParagraphStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public static void SetListStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public static void SetBulletFont(IntPtr attr, string bulletFont) =>
            throw new Exception();
        public static void SetBulletName(IntPtr attr, string name) =>
            throw new Exception();
        public static void SetTextEffects(IntPtr attr, int effects) =>
            throw new Exception();
        public static void SetTextEffectFlags(IntPtr attr, int effects) =>
            throw new Exception();
        public static void SetOutlineLevel(IntPtr attr, int level) =>
            throw new Exception();
        public static long GetFlags(IntPtr attr) => throw new Exception();
        public static int GetFontSize(IntPtr attr) => throw new Exception();
        public static int GetFontStyle(IntPtr attr) => throw new Exception();
        public static int GetFontWeight(IntPtr attr) => throw new Exception();
        public static bool GetFontUnderlined(IntPtr attr) => throw new Exception();
        public static int GetUnderlineType(IntPtr attr) => throw new Exception();
        public static Color GetUnderlineColor(IntPtr attr) => throw new Exception();
        public static bool GetFontStrikethrough(IntPtr attr) => throw new Exception();
        public static string GetFontFaceName(IntPtr attr) => throw new Exception();
        public static int GetFontFamily(IntPtr attr) => throw new Exception();
        public static int GetParagraphSpacingAfter(IntPtr attr) =>
            throw new Exception();
        public static int GetParagraphSpacingBefore(IntPtr attr) =>
            throw new Exception();
        public static int GetLineSpacing(IntPtr attr) => throw new Exception();
        public static int GetBulletStyle(IntPtr attr) => throw new Exception();
        public static int GetBulletNumber(IntPtr attr) => throw new Exception();
        public static string GetBulletText(IntPtr attr) => throw new Exception();
        public static string GetURL(IntPtr attr) => throw new Exception();
        public static int GetTextEffects(IntPtr attr) => throw new Exception();
        public static int GetTextEffectFlags(IntPtr attr) => throw new Exception();
        public static int GetOutlineLevel(IntPtr attr) => throw new Exception();
        public static bool IsCharacterStyle(IntPtr attr) => throw new Exception();
        public static bool IsParagraphStyle(IntPtr attr) => throw new Exception();

        public static bool IsDefault(IntPtr attr) => throw new Exception();
        public static bool HasTextColor(IntPtr attr) => false;
        public static bool HasBackgroundColor(IntPtr attr) => false;
        public static bool HasAlignment(IntPtr attr) => false;
        public static bool HasTabs(IntPtr attr) => false;
        public static bool HasLeftIndent(IntPtr attr) => false;
        public static bool HasRightIndent(IntPtr attr) => false;
        public static bool HasFontWeight(IntPtr attr) => false;
        public static bool HasFontSize(IntPtr attr) => false;
        public static bool HasFontPointSize(IntPtr attr) => false;
        public static bool HasFontPixelSize(IntPtr attr) => false;
        public static bool HasFontItalic(IntPtr attr) => false;
        public static bool HasFontUnderlined(IntPtr attr) => false;
        public static bool HasFontStrikethrough(IntPtr attr) => false;
        public static bool HasFontFaceName(IntPtr attr) => false;
        public static bool HasFontEncoding(IntPtr attr) => false;
        public static bool HasFontFamily(IntPtr attr) => false;
        public static bool HasFont(IntPtr attr) => false;
        public static bool HasParagraphSpacingAfter(IntPtr attr) => false;
        public static bool HasParagraphSpacingBefore(IntPtr attr) => false;
        public static bool HasLineSpacing(IntPtr attr) => false;
        public static bool HasCharacterStyleName(IntPtr attr) => false;
        public static bool HasParagraphStyleName(IntPtr attr) => false;
        public static bool HasListStyleName(IntPtr attr) => false;
        public static bool HasBulletStyle(IntPtr attr) => false;
        public static bool HasBulletNumber(IntPtr attr) => false;
        public static bool HasBulletText(IntPtr attr) => false;
        public static bool HasBulletName(IntPtr attr) => false;
        public static bool HasURL(IntPtr attr) => false;
        public static bool HasPageBreak(IntPtr attr) => false;
        public static bool HasTextEffects(IntPtr attr) => false;
        public static bool HasTextEffect(IntPtr attr, int effect) => false;
        public static bool HasOutlineLevel(IntPtr attr) => false;
        public static bool HasFlag(IntPtr attr, long flag) => false;
        public static void RemoveFlag(IntPtr attr, long flag) { }
        public static void AddFlag(IntPtr attr, long flag) { }

    }
}