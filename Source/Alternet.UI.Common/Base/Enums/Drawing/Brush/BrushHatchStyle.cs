namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the different patterns available for the hatch brushes.
    /// </summary>
    public enum BrushHatchStyle
    {
        /// <summary>
        /// A pattern of horizontal lines.
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// A pattern of vertical lines.
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// A pattern of lines on a diagonal from upper left to lower right.
        /// </summary>
        ForwardDiagonal = 2,

        /// <summary>
        /// A pattern of lines on a diagonal from upper right to lower left.
        /// </summary>
        BackwardDiagonal = 3,

        /// <summary>
        /// Specifies horizontal and vertical lines that cross.
        /// </summary>
        Cross = 4,

        /// <summary>
        /// A pattern of crisscross diagonal lines.
        /// </summary>
        DiagonalCross = 5,
    }
}