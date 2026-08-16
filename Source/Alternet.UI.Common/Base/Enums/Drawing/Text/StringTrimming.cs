namespace Alternet.Drawing;

/// <summary>
/// Specifies how text is trimmed when it exceeds the layout region.
/// </summary>
public enum StringTrimming
{
    /// <summary>
    /// No trimming is done. Text is displayed entirety, even if it overflows the layout region.
    /// </summary>
    None = 0,

    /// <summary>
    /// The text is broken at the boundary of the last character that is inside the layout region.
    /// </summary>
    Character = 1,

    /// <summary>
    /// The text is broken at the boundary of the last word that is inside the layout region.
    /// </summary>
    Word = 2,

    /// <summary>
    /// The text is broken at the last character that is inside
    /// the layout region and an ellipsis (...) is inserted after the character.
    /// </summary>
    EllipsisCharacter = 3,

    /// <summary>
    /// The text is broken at the last word that is inside the
    /// layout region and an ellipsis (...) is inserted after the word.
    /// </summary>
    EllipsisWord = 4,

    /// <summary>
    /// The center is removed from the text and replaced by an ellipsis characters.
    /// </summary>
    EllipsisPath = 5,
}
