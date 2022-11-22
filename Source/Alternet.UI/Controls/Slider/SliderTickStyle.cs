
namespace Alternet.UI
{
    /// <summary>
    /// Specifies the location of tick marks in a <see cref="Slider"/> control.
    /// </summary>
    /// <remarks>
    /// Use the members of this enumeration to set the value of the <see cref="Slider.TickStyle"/> property of the <see cref="Slider"/> control.
    /// </remarks>
    public enum SliderTickStyle
    {
        /// <summary>
        /// No tick marks appear in the control.
        /// </summary>
        None,

        /// <summary>
        /// Tick marks are located on the top of a horizontal control or on the left of a vertical control.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Tick marks are located on the bottom of a horizontal control or on the right side of a vertical control.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Tick marks are located on both sides of the control.
        /// </summary>
        Both
    }
}