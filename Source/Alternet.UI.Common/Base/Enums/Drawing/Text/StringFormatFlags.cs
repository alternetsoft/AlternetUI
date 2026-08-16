using System;

namespace Alternet.Drawing;

/// <summary>
/// Specifies formatting information, display manipulations, and font related features for text.
/// </summary>
[Flags]
public enum StringFormatFlags
{
    /// <summary>
    /// Specifies that text is displayed from right to left.
    /// For example, this flag is used for displaying Arabic and Hebrew text.
    /// </summary>
    DirectionRightToLeft = 0x00000001,

    /// <summary>
    /// Specifies that text is displayed vertically.
    /// For example, this flag is used for displaying East Asian text.
    /// </summary>
    DirectionVertical = 0x00000002,

    /// <summary>
    /// Specifies that no part of glyphs overhang the bounding rectangle. By default some glyphs
    /// overhang the rectangle slightly where necessary to appear at the edge visually.
    /// This flag ensures no painting outside the rectangle but causes the aligned edges
    /// of adjacent lines of text to appear uneven.
    /// </summary>
    FitBlackBox = 0x00000004,

    /// <summary>
    /// Control characters (for example, the left-to-right mark) to be shown in the output with a special glyphs.
    /// </summary>
    DisplayFormatControl = 0x00000020,

    /// <summary>
    /// Disables fallback to alternate fonts for characters not supported in the specified font.
    /// Missing characters will be displayed with the fonts missing glyph (an open square).
    /// </summary>
    NoFontFallback = 0x00000400,

    /// <summary>
    /// The space at the end of each line is included in a text measurement.
    /// </summary>
    MeasureTrailingSpaces = 0x00000800,

    /// <summary>
    /// The wrapping of text to the next line is disabled. NoWrap is used when a layout point
    /// is used instead of a rectangle. When text is drawn within a rectangle, by default, text is broken at
    /// the last word bounds that is inside the rectangle's bounds and wrapped to the next line.
    /// </summary>
    NoWrap = 0x00001000,

    /// <summary>
    /// Only entire lines are laid out in the layout rectangle. By default, layout
    /// continues until the end of the text or until clipped lines are not visible.
    /// The default setting allows the last line to be partially visible.
    /// To ensure that only whole lines are shown, set this flag and provide an appropriate layout
    /// rectangle with height to fit at least one line.
    /// </summary>
    LineLimit = 0x00002000,

    /// <summary>
    /// Specifies that characters and lines painted outside the layout rectangle are allowed to show.
    /// By default, all text that extends outside
    /// the layout rectangle is clipped. This flag will has an
    /// effect on a string measurement if trailing spaces are included in the measurement.
    /// If clipping is enabled, trailing spaces that are outside the layout rectangle will not be included
    /// in the measurement. If clipping is disabled, all trailing spaces are included in the measurement.
    /// </summary>
    NoClip = 0x00004000,
}