using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains different text attributes for use with <see cref="TextBox"/>.
    /// </summary>
    public interface ITextBoxTextAttr
    {
        /// <summary>
        /// Gets <see cref="FontStyle"/> of the text.
        /// </summary>
        public FontStyle GetFontStyle();

        /// <summary>
        /// Gets <see cref="FontInfo"/> of the text.
        /// </summary>
        public FontInfo GetFontInfo();

        /// <summary>
        /// Sets <see cref="FontInfo"/> of the text.
        /// </summary>
        ITextBoxTextAttr SetFontInfo(FontInfo value);

        /// <summary>
        /// Sets <see cref="FontStyle"/> of the text.
        /// </summary>
        ITextBoxTextAttr SetFontStyle(FontStyle fontStyle);

        /// <summary>
        /// Sets color of the text.
        /// </summary>
        /// <param name="colText">New text color.</param>
        ITextBoxTextAttr SetTextColor(Color colText);

        /// <summary>
        /// Sets background color.
        /// </summary>
        /// <param name="colBack">New background color.</param>
        ITextBoxTextAttr SetBackgroundColor(Color colBack);

        /// <summary>
        /// Sets font size in points.
        /// </summary>
        /// <param name="pointSize">New font size in points.</param>
        ITextBoxTextAttr SetFontPointSize(int pointSize);

        /// <summary>
        /// Sets font size in points.
        /// </summary>
        /// <param name="pointSize">New font size in points.</param>
        ITextBoxTextAttr SetFontPointSize(double pointSize);

        /// <summary>
        /// Sets name of the font.
        /// </summary>
        /// <param name="faceName">New font name.</param>
        ITextBoxTextAttr SetFontFaceName(string faceName);

        /// <summary>
        /// Returns boolean value indicating whether text is shown in the italic style.
        /// </summary>
        bool GetFontItalic();

        /// <summary>
        /// Sets boolean value indicating whether to underline the text.
        /// </summary>
        /// <param name="underlined">Underline text or not.</param>
        ITextBoxTextAttr SetFontUnderlined(bool underlined = true);

        /// <summary>
        /// Sets boolean value indicating whether to show text in the italic style.
        /// </summary>
        ITextBoxTextAttr SetFontItalic(bool italic = true);

        /// <summary>
        /// Sets boolean value indicating whether to show text in the slanted style
        /// (almost same as italic).
        /// </summary>
        ITextBoxTextAttr SetFontSlanted(bool slanted = true);

        /// <summary>
        /// Sets boolean value indicating whether to show text in the
        /// strikethrough style
        /// </summary>
        ITextBoxTextAttr SetFontStrikethrough(bool strikethrough = true);

        /// <summary>
        /// Sets initial value for ordered list numbers.
        /// </summary>
        ITextBoxTextAttr SetBulletNumber(int n);

        /// <summary>
        /// Sets the bullet text, which could be a symbol, or
        /// (for example) cached outline text.
        /// </summary>
        ITextBoxTextAttr SetBulletText(string text);

        /// <summary>
        /// Specifies a page break before this paragraph.
        /// </summary>
        ITextBoxTextAttr SetPageBreak(bool pageBreak = true);

        /// <summary>
        /// Returns outline level of the text.
        /// </summary>
        int GetOutlineLevel();

        /// <summary>
        /// Returns true if character style attributes are specified.
        /// </summary>
        bool IsCharacterStyle();

        /// <summary>
        /// Returns true if paragraph style attributes are specified.
        /// </summary>
        bool IsParagraphStyle();

        /// <summary>
        /// Returns true if font underlined attribute is specified.
        /// </summary>
        bool GetFontUnderlined();

        /// <summary>
        /// Gets color of the text underline.
        /// </summary>
        Color GetUnderlineColor();

        /// <summary>
        /// Returns true if font strikethrough attribute is specified.
        /// </summary>
        bool GetFontStrikethrough();

        /// <summary>
        /// Returns name of the font.
        /// </summary>
        string GetFontFaceName();

        /// <summary>
        /// Returns integer value which defines an empty spacing after the paragraph.
        /// </summary>
        /// <remarks>
        ///  Spacing is counted in tenths of a millimetre.
        /// </remarks>
        int GetParagraphSpacingAfter();

        /// <summary>
        /// Returns integer value which defines an empty spacing before the paragraph.
        /// </summary>
        /// <remarks>
        ///  Spacing is counted in tenths of a millimetre.
        /// </remarks>
        int GetParagraphSpacingBefore();

        /// <summary>
        /// Returns integer value which defines an empty spacing between the
        /// lines of text inside the paragraph.
        /// </summary>
        /// <remarks>
        /// Line spacing is a multiple, where 10 means single-spacing,
        /// 15 means 1.5 spacing, and 20 means double spacing.
        /// </remarks>
        int GetLineSpacing();

        /// <summary>
        /// Returns the bullet number.
        /// </summary>
        int GetBulletNumber();

        /// <summary>
        /// Returns the bullet text, which could be a symbol,
        /// or (for example) cached outline text.
        /// </summary>
        string GetBulletText();

        /// <summary>
        /// Returns URL attribute associated with the text.
        /// </summary>
        /// <returns></returns>
        string GetURL();

        /// <summary>
        /// Returns color of the text.
        /// </summary>
        /// <returns></returns>
        Color GetTextColor();

        /// <summary>
        /// Returns background color.
        /// </summary>
        /// <returns></returns>
        Color GetBackgroundColor();

        /// <summary>
        /// Sets value of the URL attribute for the text.
        /// </summary>
        ITextBoxTextAttr SetURL(string url);

        /// <summary>
        /// Sets integer value which defines an empty spacing after the paragraph.
        /// </summary>
        /// <remarks>
        ///  Spacing is counted in tenths of a millimetre.
        /// </remarks>
        ITextBoxTextAttr SetParagraphSpacingAfter(int spacing);

        /// <summary>
        /// Sets integer value which defines an empty spacing before the paragraph.
        /// </summary>
        /// <remarks>
        ///  Spacing is counted in tenths of a millimetre.
        /// </remarks>
        ITextBoxTextAttr SetParagraphSpacingBefore(int spacing);

        /// <summary>
        /// Sets integer value which defines an empty spacing between the
        /// lines of text inside the paragraph.
        /// </summary>
        /// <remarks>
        /// Line spacing is a multiple, where 10 means single-spacing,
        /// 15 means 1.5 spacing, and 20 means double spacing.
        /// </remarks>
        ITextBoxTextAttr SetLineSpacing(int spacing);

        /// <summary>
        /// Sets the character style name.
        /// </summary>
        ITextBoxTextAttr SetCharacterStyleName(string name);

        /// <summary>
        /// Sets the paragraph style name.
        /// </summary>
        ITextBoxTextAttr SetParagraphStyleName(string name);

        /// <summary>
        /// Sets the list style name.
        /// </summary>
        ITextBoxTextAttr SetListStyleName(string name);

        /// <summary>
        /// Sets the bullet font name.
        /// </summary>
        ITextBoxTextAttr SetBulletFont(string bulletFont);

        /// <summary>
        /// Sets the standard bullet name, applicable if the bullet
        /// style is standard.
        /// </summary>
        /// <remarks>
        /// Valid standard bullet names are: "standard/circle", "standard/square",
        /// "standard/diamond", "standard/triangle".
        /// </remarks>
        ITextBoxTextAttr SetBulletName(string name);

        /// <summary>
        /// Sets outline level of the text.
        /// </summary>
        ITextBoxTextAttr SetOutlineLevel(int level);

        /// <summary>
        /// Returns font size.
        /// </summary>
        /// <returns></returns>
        int GetFontSize();

        /// <summary>
        /// Gets whether any text attributes were set.
        /// </summary>
        /// <returns>Returns false if we have any attributes set, true
        /// otherwise</returns>
        bool IsDefault();

        /// <summary>
        /// Returns boolean value indicating whether text color attribute is specified
        /// for the text.
        /// </summary>
        bool HasTextColor();

        /// <summary>
        /// Returns boolean value indicating whether background color attribute
        /// is specified.
        /// </summary>
        bool HasBackgroundColor();

        /// <summary>
        /// Returns boolean value indicating whether alignment attribute
        /// is specified.
        /// </summary>
        bool HasAlignment();

        /// <summary>
        /// Returns boolean value indicating whether tab stops are specified.
        /// </summary>
        bool HasTabs();

        /// <summary>
        /// Returns boolean value indicating whether left indentation is specified.
        /// </summary>
        bool HasLeftIndent();

        /// <summary>
        /// Returns boolean value indicating whether right indentation is specified.
        /// </summary>
        bool HasRightIndent();

        /// <summary>
        /// Returns boolean value indicating whether font weight is specified.
        /// </summary>
        bool HasFontWeight();

        /// <summary>
        /// Returns boolean value indicating whether font size is specified.
        /// </summary>
        bool HasFontSize();

        /// <summary>
        /// Returns boolean value indicating whether font size in points is specified.
        /// </summary>
        bool HasFontPointSize();

        /// <summary>
        /// Returns boolean value indicating whether font size in pixels is specified.
        /// </summary>
        bool HasFontPixelSize();

        /// <summary>
        /// Returns boolean value indicating whether italic font is specified.
        /// </summary>
        bool HasFontItalic();

        /// <summary>
        /// Returns boolean value indicating whether underlined font is specified.
        /// </summary>
        bool HasFontUnderlined();

        /// <summary>
        /// Returns boolean value indicating whether strikethrough font is specified.
        /// </summary>
        bool HasFontStrikethrough();

        /// <summary>
        /// Returns boolean value indicating whether font name is specified.
        /// </summary>
        bool HasFontFaceName();

        /// <summary>
        /// Returns boolean value indicating whether font encoding is specified.
        /// </summary>
        bool HasFontEncoding();

        /// <summary>
        /// Returns boolean value indicating whether font family is specified.
        /// </summary>
        bool HasFontFamily();

        /// <summary>
        /// Returns boolean value indicating whether any font attribute is specified.
        /// </summary>
        bool HasFont();

        /// <summary>
        /// Returns boolean value indicating whether spacing after paragraph
        /// is specified.
        /// </summary>
        bool HasParagraphSpacingAfter();

        /// <summary>
        /// Returns boolean value indicating whether spacing before paragraph
        /// is specified.
        /// </summary>
        bool HasParagraphSpacingBefore();

        /// <summary>
        /// Returns boolean value indicating whether line spacing is specified.
        /// </summary>
        bool HasLineSpacing();

        /// <summary>
        /// Returns true if the attribute object specifies a character style name.
        /// </summary>
        bool HasCharacterStyleName();

        /// <summary>
        /// Returns true if the attribute object specifies a paragraph style name.
        /// </summary>
        bool HasParagraphStyleName();

        /// <summary>
        /// Returns true if the attribute object specifies a list style name.
        /// </summary>
        bool HasListStyleName();

        /// <summary>
        /// Returns true if the attribute object specifies a bullet style.
        /// </summary>
        bool HasBulletStyle();

        /// <summary>
        /// Returns true if bullet starting number for ordered lists is specified.
        /// </summary>
        bool HasBulletNumber();

        /// <summary>
        /// Returns true if the attribute object specifies bullet
        /// text (usually specifying a symbol).
        /// </summary>
        bool HasBulletText();

        /// <summary>
        /// Returns true if the attribute object specifies a standard bullet name.
        /// </summary>
        bool HasBulletName();

        /// <summary>
        /// Returns true if URL attribute is specified for the text.
        /// </summary>
        bool HasURL();

        /// <summary>
        /// Returns true if page break attribute is specified.
        /// </summary>
        bool HasPageBreak();

        /// <summary>
        /// Returns true if any text effect is specified.
        /// </summary>
        bool HasTextEffects();

        /// <summary>
        /// Returns true if specific text effect is specified.
        /// </summary>
        bool HasTextEffect(TextBoxTextAttrEffects effect);

        /// <summary>
        /// Returns true if outline level is specified.
        /// </summary>
        bool HasOutlineLevel();

        /// <summary>
        /// Sets advanced font underline style.
        /// </summary>
        /// <param name="type">Type of the underline.</param>
        /// <param name="color">Color of the underline.</param>
        ITextBoxTextAttr SetFontUnderlinedEx(TextBoxTextAttrUnderlineType type, Color color);

        /// <summary>
        /// Returns type of the underline.
        /// </summary>
        TextBoxTextAttrUnderlineType GetUnderlineType();

        /// <summary>
        /// Sets font weight.
        /// </summary>
        /// <param name="fontWeight">New value for the font waight attribute.</param>
        ITextBoxTextAttr SetFontWeight(FontWeight fontWeight);

        /// <summary>
        /// Returns font weight.
        /// </summary>
        FontWeight GetFontWeight();

        /// <summary>
        /// Sets text effects.
        /// </summary>
        ITextBoxTextAttr SetTextEffects(TextBoxTextAttrEffects effects);

        /// <summary>
        /// Returns text effects.
        /// </summary>
        TextBoxTextAttrEffects GetTextEffects();

        /// <summary>
        /// Sets text alignment.
        /// </summary>
        /// <param name="alignment">New alignment of the text.</param>
        ITextBoxTextAttr SetAlignment(TextBoxTextAttrAlignment alignment);

        /// <summary>
        /// Returns text alignment.
        /// </summary>
        TextBoxTextAttrAlignment GetAlignment();

        /// <summary>
        /// Sets bullet style for the lists.
        /// </summary>
        /// <param name="style">New bullet style.</param>
        ITextBoxTextAttr SetBulletStyle(TextBoxTextAttrBulletStyle style);

        /// <summary>
        /// Returns bullet style used for the lists.
        /// </summary>
        TextBoxTextAttrBulletStyle GetBulletStyle();

        /// <summary>
        /// Returns true if specific attribute is set.
        /// </summary>
        /// <param name="flag">Attribute identification flag.</param>
        /// <returns></returns>
        bool HasFlag(TextBoxTextAttrFlags flag);

        /// <summary>
        /// Mark attribute as not specified.
        /// </summary>
        /// <param name="flag">Attribute identification flag.</param>
        ITextBoxTextAttr RemoveFlag(TextBoxTextAttrFlags flag);

        /// <summary>
        /// Add flag indicating that some attribute is specified.
        /// </summary>
        /// <param name="flag">Attribute identification flag.</param>
        ITextBoxTextAttr AddFlag(TextBoxTextAttrFlags flag);

        /// <summary>
        /// Returns flags for specified attributes.
        /// </summary>
        TextBoxTextAttrFlags GetFlags();

        /// <summary>
        /// Set flags indicating that these attributes are specified in the style.
        /// </summary>
        /// <param name="flags">Attribute identification flags.</param>
        ITextBoxTextAttr SetFlags(TextBoxTextAttrFlags flags);

        /// <summary>
        /// Copy all attributes from the other style.
        /// </summary>
        /// <param name="fromAttr">Style from which attributes will copied.</param>
        void Copy(ITextBoxTextAttr fromAttr);

        /// <summary>
        /// Sets font family attribute.
        /// </summary>
        /// <param name="family">New font family value.</param>
        ITextBoxTextAttr SetFontFamily(GenericFontFamily family);
    }
}