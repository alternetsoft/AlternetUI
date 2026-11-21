using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="ITextBoxRichAttr"/> interface provider.
    /// </summary>
    public class PlessTextBoxRichAttr : ITextBoxTextAttr, ITextBoxRichAttr
    {
        /// <summary>
        /// Gets dummy <see cref="ITextBoxRichAttr"/> interface provider.
        /// </summary>
        public static readonly ITextBoxRichAttr Empty = new PlessTextBoxRichAttr();

        /// <inheritdoc/>
        public ITextBoxTextAttr AddFlag(TextBoxTextAttrFlags flag)
        {
            return this;
        }

        /// <inheritdoc/>
        public void Copy(ITextBoxTextAttr fromAttr)
        {
        }

        /// <inheritdoc/>
        public TextBoxTextAttrAlignment GetAlignment()
        {
            return default;
        }

        /// <inheritdoc/>
        public Color GetBackgroundColor()
        {
            return Color.Empty;
        }

        /// <inheritdoc/>
        public int GetBulletNumber()
        {
            return default;
        }

        /// <inheritdoc/>
        public TextBoxTextAttrBulletStyle GetBulletStyle()
        {
            return default;
        }

        /// <inheritdoc/>
        public string GetBulletText()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public TextBoxTextAttrFlags GetFlags()
        {
            return default;
        }

        /// <inheritdoc/>
        public string GetFontFaceName()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public FontInfo GetFontInfo()
        {
            return new();
        }

        /// <inheritdoc/>
        public bool GetFontItalic()
        {
            return default;
        }

        /// <inheritdoc/>
        public int GetFontSize()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool GetFontStrikethrough()
        {
            return default;
        }

        /// <inheritdoc/>
        public FontStyle GetFontStyle()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool GetFontUnderlined()
        {
            return default;
        }

        /// <inheritdoc/>
        public FontWeight GetFontWeight()
        {
            return default;
        }

        /// <inheritdoc/>
        public int GetLineSpacing()
        {
            return default;
        }

        /// <inheritdoc/>
        public int GetOutlineLevel()
        {
            return default;
        }

        /// <inheritdoc/>
        public int GetParagraphSpacingAfter()
        {
            return default;
        }

        /// <inheritdoc/>
        public int GetParagraphSpacingBefore()
        {
            return default;
        }

        /// <inheritdoc/>
        public Color GetTextColor()
        {
            return Color.Empty;
        }

        /// <inheritdoc/>
        public TextBoxTextAttrEffects GetTextEffects()
        {
            return default;
        }

        /// <inheritdoc/>
        public Color GetUnderlineColor()
        {
            return Color.Empty;
        }

        /// <inheritdoc/>
        public TextBoxTextAttrUnderlineType GetUnderlineType()
        {
            return default;
        }

        /// <inheritdoc/>
        public string GetURL()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public bool HasAlignment()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasBackgroundColor()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasBulletName()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasBulletNumber()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasBulletStyle()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasBulletText()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasCharacterStyleName()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFlag(TextBoxTextAttrFlags flag)
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFont()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontEncoding()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontFaceName()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontFamily()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontItalic()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontPixelSize()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontPointSize()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontSize()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontStrikethrough()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontUnderlined()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasFontWeight()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasLeftIndent()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasLineSpacing()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasListStyleName()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasOutlineLevel()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasPageBreak()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasParagraphSpacingAfter()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasParagraphSpacingBefore()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasParagraphStyleName()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasRightIndent()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasTabs()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasTextColor()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasTextEffect(TextBoxTextAttrEffects effect)
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasTextEffects()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool HasURL()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool IsCharacterStyle()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool IsDefault()
        {
            return default;
        }

        /// <inheritdoc/>
        public bool IsParagraphStyle()
        {
            return default;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr RemoveFlag(TextBoxTextAttrFlags flag)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetAlignment(TextBoxTextAttrAlignment alignment)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBackgroundColor(Color colBack)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBulletFont(string bulletFont)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBulletName(string name)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBulletNumber(int n)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBulletStyle(TextBoxTextAttrBulletStyle style)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetBulletText(string text)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetCharacterStyleName(string name)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFlags(TextBoxTextAttrFlags flags)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontFaceName(string faceName)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontFamily(GenericFontFamily family)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontInfo(FontInfo value)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontItalic(bool italic = true)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontPointSize(int pointSize)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontPointSize(Coord pointSize)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontSlanted(bool slanted = true)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontStrikethrough(bool strikethrough = true)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontStyle(FontStyle fontStyle)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontUnderlined(bool underlined = true)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontUnderlinedEx(TextBoxTextAttrUnderlineType type, Color color)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetFontWeight(FontWeight fontWeight)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetLineSpacing(int spacing)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetListStyleName(string name)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetOutlineLevel(int level)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetPageBreak(bool pageBreak = true)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetParagraphSpacingAfter(int spacing)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetParagraphSpacingBefore(int spacing)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetParagraphStyleName(string name)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetTextColor(Color colText)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetTextEffects(TextBoxTextAttrEffects effects)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITextBoxTextAttr SetURL(string url)
        {
            return this;
        }
    }
}
