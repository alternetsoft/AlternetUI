namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the available cap styles with which a pen object can end a line.
    /// </summary>
    public enum LineCap
    {
        /// <summary>
        /// Specifies a flat (butt) line cap (line ends should be square and should not project).
        /// Begin/end contours with no extension.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// Specifies a square line cap (line ends with square projection).
        /// Begin/end contours with a half square extension.
        /// </summary>
        Square = 1,

        /// <summary>
        /// Specifies a round line cap (rounded line ends).
        /// Begin/end contours with a semi-circle extension.
        /// </summary>
        Round = 2,
    }
}