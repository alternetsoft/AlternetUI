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
        /// Specifies that the text is trimmed to the pixels of the specified text bounds.
        /// </summary>
        Pixel,

        /// <summary>
        /// Specifies that the text is trimmed to the nearest character.
        /// </summary>
        Character,
    }
}