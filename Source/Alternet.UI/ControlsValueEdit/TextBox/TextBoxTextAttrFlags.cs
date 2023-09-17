using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags to indicate which attributes are being applied.
    /// Used in <see cref="ITextBoxTextAttr.SetFlags"/>,
    /// <see cref="ITextBoxTextAttr.AddFlag"/>,
    /// <see cref="ITextBoxTextAttr.RemoveFlag"/>.
    /// </summary>
    [Flags]
    public enum TextBoxTextAttrFlags
    {
        /// <summary>
        /// No attrributes are applied.
        /// </summary>
        None = 0,

        /// <summary>
        /// Apply "TextColor" attribute.
        /// </summary>
        TextColor = 0x00000001,

        /// <summary>
        /// Apply "BackgroundColor" attribute.
        /// </summary>
        BackgroundColor = 0x00000002,

        /// <summary>
        /// Apply "FontFace" attribute.
        /// </summary>
        FontFace = 0x00000004,

        /// <summary>
        /// Apply "FontPointSize" attribute.
        /// </summary>
        FontPointSize = 0x00000008,

        /// <summary>
        /// Apply "FontPixelSize" attribute.
        /// </summary>
        FontPixelSize = 0x10000000,

        /// <summary>
        /// Apply "FontWeight" attribute.
        /// </summary>
        FontWeight = 0x00000010,

        /// <summary>
        /// Apply "FontItalic" attribute.
        /// </summary>
        FontItalic = 0x00000020,

        /// <summary>
        /// Apply "FontUnderline" attribute.
        /// </summary>
        FontUnderline = 0x00000040,

        /// <summary>
        /// Apply "FontStrikethrough" attribute.
        /// </summary>
        FontStrikethrough = 0x08000000,

        /// <summary>
        /// Apply "FontEncoding" attribute.
        /// </summary>
        FontEncoding = 0x02000000,

        /// <summary>
        /// Apply "FontFamily" attribute.
        /// </summary>
        FontFamily = 0x04000000,

        /// <summary>
        /// Apply font size attributes.
        /// </summary>
        FontSize = FontPointSize | FontPixelSize,

        /// <summary>
        /// Apply font attributes attribute.
        /// </summary>
        Font = FontFace | FontSize | FontWeight |
            FontItalic | FontUnderline | FontStrikethrough |
            FontEncoding | FontFamily,

        /// <summary>
        /// Apply "Alignment" attribute.
        /// </summary>
        Alignment = 0x00000080,

        /// <summary>
        /// Apply "LeftIndent" attribute.
        /// </summary>
        LeftIndent = 0x00000100,

        /// <summary>
        /// Apply "RightIndent" attribute.
        /// </summary>
        RightIndent = 0x00000200,

        /// <summary>
        /// Apply "Tabs" attribute.
        /// </summary>
        Tabs = 0x00000400,

        /// <summary>
        /// Apply "ParaSpacingAfter" attribute.
        /// </summary>
        ParaSpacingAfter = 0x00000800,

        /// <summary>
        /// Apply "ParaSpacingBefore" attribute.
        /// </summary>
        ParaSpacingBefore = 0x00001000,

        /// <summary>
        /// Apply "LineSpacing" attribute.
        /// </summary>
        LineSpacing = 0x00002000,

        /// <summary>
        /// Apply "CharacterStyleName" attribute.
        /// </summary>
        CharacterStyleName = 0x00004000,

        /// <summary>
        /// Apply "ParagraphStyleName" attribute.
        /// </summary>
        ParagraphStyleName = 0x00008000,

        /// <summary>
        /// Apply "ListStyleName" attribute.
        /// </summary>
        ListStyleName = 0x00010000,

        /// <summary>
        /// Apply "BulletStyle" attribute.
        /// </summary>
        BulletStyle = 0x00020000,

        /// <summary>
        /// Apply "BulletNumber" attribute.
        /// </summary>
        BulletNumber = 0x00040000,

        /// <summary>
        /// Apply "BulletText" attribute.
        /// </summary>
        BulletText = 0x00080000,

        /// <summary>
        /// Apply "BulletName" attribute.
        /// </summary>
        BulletName = 0x00100000,

        /// <summary>
        /// Apply bullet attribute.
        /// </summary>
        Bullet = BulletStyle | BulletNumber | BulletText | BulletName,

        /// <summary>
        /// Apply "Url" attribute.
        /// </summary>
        Url = 0x00200000,

        /// <summary>
        /// Apply "PageBreak" attribute.
        /// </summary>
        PageBreak = 0x00400000,

        /// <summary>
        /// Apply "Effects" attribute.
        /// </summary>
        Effects = 0x00800000,

        /// <summary>
        /// Apply "OutlineLevel" attribute.
        /// </summary>
        OutlineLevel = 0x01000000,

        /// <summary>
        /// Apply "AvoidPageBreakBefore" attribute.
        /// </summary>
        AvoidPageBreakBefore = 0x20000000,

        /// <summary>
        /// Apply "AvoidPageBreakAfter" attribute.
        /// </summary>
        AvoidPageBreakAfter = 0x40000000,

        /// <summary>
        /// Apply character attributes.
        /// </summary>
        Character = Font | Effects | BackgroundColor | TextColor
            | CharacterStyleName | Url,

        /// <summary>
        /// Apply paragraph attributes.
        /// </summary>
        Paragraph = Alignment | LeftIndent | RightIndent | Tabs |
            ParaSpacingBefore | ParaSpacingAfter | LineSpacing |
            Bullet | ParagraphStyleName | ListStyleName | OutlineLevel |
            PageBreak | AvoidPageBreakBefore | AvoidPageBreakAfter,

        /// <summary>
        /// All attributes flag.
        /// </summary>
        All = Character | Paragraph,
    }
}
