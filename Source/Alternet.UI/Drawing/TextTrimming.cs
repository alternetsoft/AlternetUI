namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies how to trim characters from a string that does not completely fit into a layout shape.
    /// </summary>
    public enum TextTrimming
    {
        /// <summary>
        /// Specifies no trimming.
        /// </summary>
        None,

        /// <summary>
        /// Specifies that the text is trimmed to the nearest character.
        /// </summary>
        Character,

        /// <summary>
        /// Specifies that text is trimmed to the nearest word.
        /// </summary>
        Word,

        /// <summary>
        /// Specifies that the text is trimmed to the nearest character, and an ellipsis is inserted at the end of a trimmed line.
        /// </summary>
        EllipsisCharacter,

        /// <summary>
        /// Specifies that text is trimmed to the nearest word, and an ellipsis is inserted at the end of a trimmed line.
        /// </summary>
        EllipsisWord
    }
}