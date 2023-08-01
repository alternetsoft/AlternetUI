using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags to indicate which attributes are being applied.
    /// </summary>
    [Flags]
    public enum TextBoxTextAttrFlags
    {
        None = 0,

        TextColor = 0x00000001,

        BackgroundColor = 0x00000002,

        FontFace = 0x00000004,

        FontPointSize = 0x00000008,

        FontPixelSize = 0x10000000,

        FontWeight = 0x00000010,

        FontItalic = 0x00000020,

        FontUnderline = 0x00000040,

        FontStrikethrough = 0x08000000,

        FontEncoding = 0x02000000,

        FontFamily = 0x04000000,

        FontSize = FontPointSize | FontPixelSize,

        Font = FontFace | FontSize | FontWeight |
            FontItalic | FontUnderline | FontStrikethrough |
            FontEncoding | FontFamily,

        Alignment = 0x00000080,

        LeftIndent = 0x00000100,

        RightIndent = 0x00000200,

        Tabs = 0x00000400,

        ParaSpacingAfter = 0x00000800,

        ParaSpacingBefore = 0x00001000,

        LineSpacing = 0x00002000,

        CharacterStyleName = 0x00004000,

        ParagraphStyleName = 0x00008000,

        ListStyleName = 0x00010000,

        BulletStyle = 0x00020000,

        BulletNumber = 0x00040000,

        BulletText = 0x00080000,

        BulletName = 0x00100000,

        Bullet = BulletStyle | BulletNumber | BulletText | BulletName,

        Url = 0x00200000,

        PageBreak = 0x00400000,

        Effects = 0x00800000,

        OutlineLevel = 0x01000000,

        AvoidPageBreakBefore = 0x20000000,

        AvoidPageBreakAfter = 0x40000000,

        Character = Font | Effects | BackgroundColor | TextColor
            | CharacterStyleName | Url,

        Paragraph = Alignment | LeftIndent | RightIndent | Tabs |
            ParaSpacingBefore | ParaSpacingAfter | LineSpacing |
            Bullet | ParagraphStyleName | ListStyleName | OutlineLevel |
            PageBreak | AvoidPageBreakBefore | AvoidPageBreakAfter,

        All = Character | Paragraph,
    }
}
