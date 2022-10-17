namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies how to wrap characters from a string that does not completely fit into a layout shape.
    /// </summary>
    public enum TextWrapping
    {
        /// <summary>
        /// Specifies no wrapping.
        /// </summary>
        None,

        /// <summary>
        /// Specifies that text is wrapped by the character boundary.
        /// </summary>
        Character,

        /// <summary>
        /// Specifies that text is wrapped by the word boundary.
        /// </summary>
        Word,
    }
}