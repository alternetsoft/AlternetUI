using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ITextBoxTextAttr
    {
        void SetTextColor(Color colText);

        void SetBackgroundColor(Color colBack);

        void SetFontPointSize(int pointSize);

        void SetFontFaceName(string faceName);

        void SetFontUnderlined(bool underlined = true);

        void SetFontItalic(bool italic = true);

        void SetFontSlanted(bool slanted = true);

        void SetFontStrikethrough(bool strikethrough = true);

        void SetBulletNumber(int n);

        void SetBulletText(string text);

        void SetPageBreak(bool pageBreak = true);

        int GetOutlineLevel();

        bool IsCharacterStyle();

        bool IsParagraphStyle();

        bool GetFontUnderlined();

        Color GetUnderlineColor();

        bool GetFontStrikethrough();

        string GetFontFaceName();

        int GetParagraphSpacingAfter();

        int GetParagraphSpacingBefore();

        int GetLineSpacing();

        int GetBulletNumber();

        string GetBulletText();

        string GetURL();

        Color GetTextColor();

        Color GetBackgroundColor();

        void SetURL(string url);

        void SetParagraphSpacingAfter(int spacing);

        void SetParagraphSpacingBefore(int spacing);

        void SetLineSpacing(int spacing);

        void SetCharacterStyleName(string name);

        void SetParagraphStyleName(string name);

        void SetListStyleName(string name);

        void SetBulletFont(string bulletFont);

        void SetBulletName(string name);

        void SetOutlineLevel(int level);

        int GetFontSize();

        /// <summary>
        /// Gets whether any text attributes were set.
        /// </summary>
        /// <returns>Returns false if we have any attributes set, true
        /// otherwise</returns>
        bool IsDefault();

        bool HasTextColor();

        bool HasBackgroundColor();

        bool HasAlignment();

        bool HasTabs();

        bool HasLeftIndent();

        bool HasRightIndent();

        bool HasFontWeight();

        bool HasFontSize();

        bool HasFontPointSize();

        bool HasFontPixelSize();

        bool HasFontItalic();

        bool HasFontUnderlined();

        bool HasFontStrikethrough();

        bool HasFontFaceName();

        bool HasFontEncoding();

        bool HasFontFamily();

        bool HasFont();

        bool HasParagraphSpacingAfter();

        bool HasParagraphSpacingBefore();

        bool HasLineSpacing();

        bool HasCharacterStyleName();

        bool HasParagraphStyleName();

        bool HasListStyleName();

        bool HasBulletStyle();

        bool HasBulletNumber();

        bool HasBulletText();

        bool HasBulletName();

        bool HasURL();

        bool HasPageBreak();

        bool HasTextEffects();

        bool HasTextEffect(TextBoxTextAttrEffects effect);

        bool HasOutlineLevel();

        void SetFontUnderlinedEx(TextBoxTextAttrUnderlineType type, Color color);

        TextBoxTextAttrUnderlineType GetUnderlineType();

        void SetFontWeight(FontWeight fontWeight);

        FontWeight GetFontWeight();

        void SetTextEffects(TextBoxTextAttrEffects effects);

        TextBoxTextAttrEffects GetTextEffects();

        void SetAlignment(TextBoxTextAttrAlignment alignment);

        TextBoxTextAttrAlignment GetAlignment();

        void SetBulletStyle(TextBoxTextAttrBulletStyle style);

        TextBoxTextAttrBulletStyle GetBulletStyle();

        bool HasFlag(TextBoxTextAttrFlags flag);

        void RemoveFlag(TextBoxTextAttrFlags flag);

        void AddFlag(TextBoxTextAttrFlags flag);

        TextBoxTextAttrFlags GetFlags();

        void SetFlags(TextBoxTextAttrFlags flags);

        void Copy(ITextBoxTextAttr fromAttr);

        void SetFontFamily(GenericFontFamily family);
    }
}