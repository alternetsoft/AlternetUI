namespace Alternet.UI
{
    /// <summary>
    /// Specifies the location of tick marks in a slider control.
    /// </summary>
    public enum SliderTickStyle
    {
        /// <summary>
        /// No tick marks appear in the control.
        /// </summary>
        None,

        /// <summary>
        /// Tick marks are located on the top of a horizontal control or on the left of a vertical
        /// control.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Tick marks are located on the bottom of a horizontal control or on the right side of
        /// a vertical control.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Tick marks are located on both sides of the control. Windows only.
        /// </summary>
        Both,
    }
}