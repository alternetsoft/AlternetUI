namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies how to join consecutive line or curve segments.
    /// </summary>
    public enum LineJoin
    {
        /// <summary>
        /// Specifies a mitered join. This produces a sharp corner.
        /// </summary>
        Miter,

        /// <summary>
        /// Specifies a beveled join. This produces a diagonal corner.
        /// </summary>
        Bevel,

        /// <summary>
        /// Specifies a circular join. This produces a smooth, circular arc between the lines.
        /// </summary>
        Round,
    }
}