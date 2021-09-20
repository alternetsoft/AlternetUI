
namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the different patterns available for <see cref="HatchBrush"/> objects.
    /// </summary>
    public enum BrushHatchStyle
    {
        /// <summary>
        /// A pattern of lines on a diagonal from upper right to lower left.
        /// </summary>
        BackwardDiagonal,

        /// <summary>
        /// A pattern of lines on a diagonal from upper left to lower right.
        /// </summary>
        ForwardDiagonal,

        /// <summary>
        /// A pattern of crisscross diagonal lines.
        /// </summary>
        DiagonalCross,

        /// <summary>
        ///	Specifies horizontal and vertical lines that cross.
        /// </summary>
        Cross,

        /// <summary>
        /// A pattern of horizontal lines.
        /// </summary>
        Horizontal,

        /// <summary>
        /// A pattern of vertical lines.
        /// </summary>
        Vertical
    }
}