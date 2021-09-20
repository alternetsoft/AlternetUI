
namespace Alternet.Drawing
{
    /// <summary>
    /// Gets or sets the style used for dashed lines drawn with this <see cref="Pen"/>.
    /// </summary>
    public enum PenDashStyle
    {
        /// <summary>
        /// Specifies a solid line.
        /// </summary>
        Solid,

        /// <summary>
        /// Specifies a line consisting of dots.
        /// </summary>
        Dot,

        /// <summary>
        /// Specifies a line consisting of dashes.
        /// </summary>
        Dash,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot.
        /// </summary>
        DashDot,

        /// <summary>
        /// Specifies a user-defined custom dash style.
        /// </summary>
        Custom
    }
}