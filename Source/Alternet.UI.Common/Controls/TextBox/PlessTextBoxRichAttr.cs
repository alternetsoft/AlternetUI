using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class PlessTextBoxRichAttr : ITextBoxTextAttr, ITextBoxRichAttr
    {
        public static readonly ITextBoxRichAttr Empty = new PlessTextBoxRichAttr();

        public ITextBoxTextAttr AddFlag(TextBoxTextAttrFlags flag)
        {
            return this;
        }

        public void Copy(ITextBoxTextAttr fromAttr)
        {
        }

        public TextBoxTextAttrAlignment GetAlignment()
        {
            return default;
        }

        public Color GetBackgroundColor()
        {
            return Color.Empty;
        }

        public int GetBulletNumber()
        {
            return default;
        }

        public TextBoxTextAttrBulletStyle GetBulletStyle()
        {
            return default;
        }

        public string GetBulletText()
        {
            return string.Empty;
        }

        public TextBoxTextAttrFlags GetFlags()
        {
            return default;
        }

        public string GetFontFaceName()
        {
            return string.Empty;
        }

        public FontInfo GetFontInfo()
        {
            return new();
        }

        public bool GetFontItalic()
        {
            return default;
        }

        public int GetFontSize()
        {
            return default;
        }

        public bool GetFontStrikethrough()
        {
            return default;
        }

        public FontStyle GetFontStyle()
        {
            return default;
        }

        public bool GetFontUnderlined()
        {
            return default;
        }

        public FontWeight GetFontWeight()
        {
            return default;
        }

        public int GetLineSpacing()
        {
            return default;
        }

        public int GetOutlineLevel()
        {
            return default;
        }

        public int GetParagraphSpacingAfter()
        {
            return default;
        }

        public int GetParagraphSpacingBefore()
        {
            return default;
        }

        public Color GetTextColor()
        {
            return Color.Empty;
        }

        public TextBoxTextAttrEffects GetTextEffects()
        {
            return default;
        }

        public Color GetUnderlineColor()
        {
            return Color.Empty;
        }

        public TextBoxTextAttrUnderlineType GetUnderlineType()
        {
            return default;
        }

        public string GetURL()
        {
            return string.Empty;
        }

        public bool HasAlignment()
        {
            return default;
        }

        public bool HasBackgroundColor()
        {
            return default;
        }

        public bool HasBulletName()
        {
            return default;
        }

        public bool HasBulletNumber()
        {
            return default;
        }

        public bool HasBulletStyle()
        {
            return default;
        }

        public bool HasBulletText()
        {
            return default;
        }

        public bool HasCharacterStyleName()
        {
            return default;
        }

        public bool HasFlag(TextBoxTextAttrFlags flag)
        {
            return default;
        }

        public bool HasFont()
        {
            return default;
        }

        public bool HasFontEncoding()
        {
            return default;
        }

        public bool HasFontFaceName()
        {
            return default;
        }

        public bool HasFontFamily()
        {
            return default;
        }

        public bool HasFontItalic()
        {
            return default;
        }

        public bool HasFontPixelSize()
        {
            return default;
        }

        public bool HasFontPointSize()
        {
            return default;
        }

        public bool HasFontSize()
        {
            return default;
        }

        public bool HasFontStrikethrough()
        {
            return default;
        }

        public bool HasFontUnderlined()
        {
            return default;
        }

        public bool HasFontWeight()
        {
            return default;
        }

        public bool HasLeftIndent()
        {
            return default;
        }

        public bool HasLineSpacing()
        {
            return default;
        }

        public bool HasListStyleName()
        {
            return default;
        }

        public bool HasOutlineLevel()
        {
            return default;
        }

        public bool HasPageBreak()
        {
            return default;
        }

        public bool HasParagraphSpacingAfter()
        {
            return default;
        }

        public bool HasParagraphSpacingBefore()
        {
            return default;
        }

        public bool HasParagraphStyleName()
        {
            return default;
        }

        public bool HasRightIndent()
        {
            return default;
        }

        public bool HasTabs()
        {
            return default;
        }

        public bool HasTextColor()
        {
            return default;
        }

        public bool HasTextEffect(TextBoxTextAttrEffects effect)
        {
            return default;
        }

        public bool HasTextEffects()
        {
            return default;
        }

        public bool HasURL()
        {
            return default;
        }

        public bool IsCharacterStyle()
        {
            return default;
        }

        public bool IsDefault()
        {
            return default;
        }

        public bool IsParagraphStyle()
        {
            return default;
        }

        public ITextBoxTextAttr RemoveFlag(TextBoxTextAttrFlags flag)
        {
            return this;
        }

        public ITextBoxTextAttr SetAlignment(TextBoxTextAttrAlignment alignment)
        {
            return this;
        }

        public ITextBoxTextAttr SetBackgroundColor(Color colBack)
        {
            return this;
        }

        public ITextBoxTextAttr SetBulletFont(string bulletFont)
        {
            return this;
        }

        public ITextBoxTextAttr SetBulletName(string name)
        {
            return this;
        }

        public ITextBoxTextAttr SetBulletNumber(int n)
        {
            return this;
        }

        public ITextBoxTextAttr SetBulletStyle(TextBoxTextAttrBulletStyle style)
        {
            return this;
        }

        public ITextBoxTextAttr SetBulletText(string text)
        {
            return this;
        }

        public ITextBoxTextAttr SetCharacterStyleName(string name)
        {
            return this;
        }

        public ITextBoxTextAttr SetFlags(TextBoxTextAttrFlags flags)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontFaceName(string faceName)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontFamily(GenericFontFamily family)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontInfo(FontInfo value)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontItalic(bool italic = true)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontPointSize(int pointSize)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontPointSize(double pointSize)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontSlanted(bool slanted = true)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontStrikethrough(bool strikethrough = true)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontStyle(FontStyle fontStyle)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontUnderlined(bool underlined = true)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontUnderlinedEx(TextBoxTextAttrUnderlineType type, Color color)
        {
            return this;
        }

        public ITextBoxTextAttr SetFontWeight(FontWeight fontWeight)
        {
            return this;
        }

        public ITextBoxTextAttr SetLineSpacing(int spacing)
        {
            return this;
        }

        public ITextBoxTextAttr SetListStyleName(string name)
        {
            return this;
        }

        public ITextBoxTextAttr SetOutlineLevel(int level)
        {
            return this;
        }

        public ITextBoxTextAttr SetPageBreak(bool pageBreak = true)
        {
            return this;
        }

        public ITextBoxTextAttr SetParagraphSpacingAfter(int spacing)
        {
            return this;
        }

        public ITextBoxTextAttr SetParagraphSpacingBefore(int spacing)
        {
            return this;
        }

        public ITextBoxTextAttr SetParagraphStyleName(string name)
        {
            return this;
        }

        public ITextBoxTextAttr SetTextColor(Color colText)
        {
            return this;
        }

        public ITextBoxTextAttr SetTextEffects(TextBoxTextAttrEffects effects)
        {
            return this;
        }

        public ITextBoxTextAttr SetURL(string url)
        {
            return this;
        }
    }
}
