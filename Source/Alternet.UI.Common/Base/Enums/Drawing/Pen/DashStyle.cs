namespace Alternet.Drawing
{
    /// <summary>
    /// Gets or sets the style used for dashed lines drawn with the pen.
    /// </summary>
    public enum DashStyle
    {
        /// <summary>
        /// Specifies a solid line.
        /// </summary>
        Solid,

        /// <summary>
        /// Specifies a line consisting of dashes.
        /// </summary>
        Dash,

        /// <summary>
        /// Specifies a line consisting of dots.
        /// </summary>
        Dot,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot.
        /// </summary>
        DashDot,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot-dot.
        /// </summary>
        DashDotDot,

        /// <summary>
        /// Specifies a user-defined custom dash style.
        /// </summary>
        Custom,
    }
}