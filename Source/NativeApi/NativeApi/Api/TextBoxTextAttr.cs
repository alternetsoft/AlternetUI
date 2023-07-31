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
        
        /*public static void Copy(IntPtr toAttr,IntPtr fromAttr) => 
            throw new Exception();*/

        public static IntPtr CreateTextAttr() => throw new Exception();

        public static void SetTextColor(IntPtr attr,Color colText) => 
            throw new Exception();
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

        // returns false if we have any attributes set, true otherwise
        public static bool IsDefault(IntPtr attr) => throw new Exception();

    }
}



/*
    bool HasTextColour() const { return m_colText.IsOk() && HasFlag(wxTEXT_ATTR_TEXT_COLOUR) ; }
    bool HasBackgroundColour() const { return m_colBack.IsOk() && HasFlag(wxTEXT_ATTR_BACKGROUND_COLOUR) ; }
    bool HasAlignment() const { return (m_textAlignment != wxTEXT_ALIGNMENT_DEFAULT) && HasFlag(wxTEXT_ATTR_ALIGNMENT) ; }
    bool HasTabs() const { return HasFlag(wxTEXT_ATTR_TABS) ; }
    bool HasLeftIndent() const { return HasFlag(wxTEXT_ATTR_LEFT_INDENT); }
    bool HasRightIndent() const { return HasFlag(wxTEXT_ATTR_RIGHT_INDENT); }
    bool HasFontWeight() const { return HasFlag(wxTEXT_ATTR_FONT_WEIGHT); }
    bool HasFontSize() const { return HasFlag(wxTEXT_ATTR_FONT_SIZE); }
    bool HasFontPointSize() const { return HasFlag(wxTEXT_ATTR_FONT_POINT_SIZE); }
    bool HasFontPixelSize() const { return HasFlag(wxTEXT_ATTR_FONT_PIXEL_SIZE); }
    bool HasFontItalic() const { return HasFlag(wxTEXT_ATTR_FONT_ITALIC); }
    bool HasFontUnderlined() const { return HasFlag(wxTEXT_ATTR_FONT_UNDERLINE); }
    bool HasFontStrikethrough() const { return HasFlag(wxTEXT_ATTR_FONT_STRIKETHROUGH); }
    bool HasFontFaceName() const { return HasFlag(wxTEXT_ATTR_FONT_FACE); }
    bool HasFontEncoding() const { return HasFlag(wxTEXT_ATTR_FONT_ENCODING); }
    bool HasFontFamily() const { return HasFlag(wxTEXT_ATTR_FONT_FAMILY); }
    bool HasFont() const { return HasFlag(wxTEXT_ATTR_FONT); }

    bool HasParagraphSpacingAfter() const { return HasFlag(wxTEXT_ATTR_PARA_SPACING_AFTER); }
    bool HasParagraphSpacingBefore() const { return HasFlag(wxTEXT_ATTR_PARA_SPACING_BEFORE); }
    bool HasLineSpacing() const { return HasFlag(wxTEXT_ATTR_LINE_SPACING); }
    bool HasCharacterStyleName() const { return HasFlag(wxTEXT_ATTR_CHARACTER_STYLE_NAME) && !m_characterStyleName.IsEmpty(); }
    bool HasParagraphStyleName() const { return HasFlag(wxTEXT_ATTR_PARAGRAPH_STYLE_NAME) && !m_paragraphStyleName.IsEmpty(); }
    bool HasListStyleName() const { return HasFlag(wxTEXT_ATTR_LIST_STYLE_NAME) || !m_listStyleName.IsEmpty(); }
    bool HasBulletStyle() const { return HasFlag(wxTEXT_ATTR_BULLET_STYLE); }
    bool HasBulletNumber() const { return HasFlag(wxTEXT_ATTR_BULLET_NUMBER); }
    bool HasBulletText() const { return HasFlag(wxTEXT_ATTR_BULLET_TEXT); }
    bool HasBulletName() const { return HasFlag(wxTEXT_ATTR_BULLET_NAME); }
    bool HasURL() const { return HasFlag(wxTEXT_ATTR_URL); }
    bool HasPageBreak() const { return HasFlag(wxTEXT_ATTR_PAGE_BREAK); }
    bool HasTextEffects() const { return HasFlag(wxTEXT_ATTR_EFFECTS); }
    bool HasTextEffect(int effect) const { return HasFlag(wxTEXT_ATTR_EFFECTS) && ((GetTextEffectFlags() & effect) != 0); }
    bool HasOutlineLevel() const { return HasFlag(wxTEXT_ATTR_OUTLINE_LEVEL); }

    bool HasFlag(long flag) const { return (m_flags & flag) != 0; }
    void RemoveFlag(long flag) { m_flags &= ~flag; }
    void AddFlag(long flag) { m_flags |= flag; } 
 */