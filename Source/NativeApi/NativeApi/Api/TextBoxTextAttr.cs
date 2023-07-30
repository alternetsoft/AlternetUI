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
        public static void Delete(IntPtr attr) => throw new Exception();
        
        public void Copy(IntPtr attr) => throw new Exception();

        public static IntPtr CreateTextAttr() => throw new Exception();

        public void SetTextColor(IntPtr attr,Color colText) => throw new Exception();
        public void SetBackgroundColor(IntPtr attr, Color colBack) => 
            throw new Exception();
        public void SetAlignment(IntPtr attr, int alignment) => throw new Exception();

        public void SetFontPointSize(IntPtr attr, int pointSize) => 
            throw new Exception();
        public void SetFontStyle(IntPtr attr, int fontStyle) => throw new Exception();
        public void SetFontWeight(IntPtr attr, int fontWeight) => 
            throw new Exception();
        public void SetFontFaceName(IntPtr attr, string faceName) => 
            throw new Exception();
        public void SetFontUnderlined(IntPtr attr, bool underlined) => 
            throw new Exception();
        public void SetFontUnderlinedEx(IntPtr attr, int type, Color colour) => 
            throw new Exception();
        public void SetFontStrikethrough(IntPtr attr, bool strikethrough) => 
            throw new Exception();
        public void SetFontFamily(IntPtr attr, int family) => throw new Exception();

        public Color GetTextColor(IntPtr attr) => throw new Exception();
        public Color GetBackgroundColor(IntPtr attr) => throw new Exception();
        public int GetAlignment(IntPtr attr) => throw new Exception();
        public void SetURL(IntPtr attr, string url) => throw new Exception();
        public void SetFlags(IntPtr attr, long flags) => throw new Exception();
        public void SetParagraphSpacingAfter(IntPtr attr, int spacing) =>
            throw new Exception();
        public void SetParagraphSpacingBefore(IntPtr attr, int spacing) =>
            throw new Exception();
        public void SetLineSpacing(IntPtr attr, int spacing) => throw new Exception();
        public void SetBulletStyle(IntPtr attr, int style) => throw new Exception();
        public void SetBulletNumber(IntPtr attr, int n) => throw new Exception();
        public void SetBulletText(IntPtr attr, string text) => throw new Exception();
        public void SetPageBreak(IntPtr attr, bool pageBreak) => throw new Exception();
        public void SetCharacterStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public void SetParagraphStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public void SetListStyleName(IntPtr attr, string name) =>
            throw new Exception();
        public void SetBulletFont(IntPtr attr, string bulletFont) =>
            throw new Exception();
        public void SetBulletName(IntPtr attr, string name) => throw new Exception();
        public void SetTextEffects(IntPtr attr, int effects) => throw new Exception();
        public void SetTextEffectFlags(IntPtr attr, int effects) =>
            throw new Exception();
        public void SetOutlineLevel(IntPtr attr, int level) => throw new Exception();
        public long GetFlags(IntPtr attr) => throw new Exception();
        public int GetFontSize(IntPtr attr) => throw new Exception();
        public int GetFontStyle(IntPtr attr) => throw new Exception();
        public int GetFontWeight(IntPtr attr) => throw new Exception();
        public bool GetFontUnderlined(IntPtr attr) => throw new Exception();
        public int GetUnderlineType(IntPtr attr) => throw new Exception();
        public Color GetUnderlineColor(IntPtr attr) => throw new Exception();
        public bool GetFontStrikethrough(IntPtr attr) => throw new Exception();
        public string GetFontFaceName(IntPtr attr) => throw new Exception();
        public int GetFontFamily(IntPtr attr) => throw new Exception();
        public int GetParagraphSpacingAfter(IntPtr attr) => throw new Exception();
        public int GetParagraphSpacingBefore(IntPtr attr) => throw new Exception();
        public int GetLineSpacing(IntPtr attr) => throw new Exception();
        public int GetBulletStyle(IntPtr attr) => throw new Exception();
        public int GetBulletNumber(IntPtr attr) => throw new Exception();
        public string GetBulletText(IntPtr attr) => throw new Exception();
        public string GetURL(IntPtr attr) => throw new Exception();
        public int GetTextEffects(IntPtr attr) => throw new Exception();
        public int GetTextEffectFlags(IntPtr attr) => throw new Exception();
        public int GetOutlineLevel(IntPtr attr) => throw new Exception();
        public bool IsCharacterStyle(IntPtr attr) => throw new Exception();
        public bool IsParagraphStyle(IntPtr attr) => throw new Exception();

        // returns false if we have any attributes set, true otherwise
        public bool IsDefault(IntPtr attr) => throw new Exception();

    }
}