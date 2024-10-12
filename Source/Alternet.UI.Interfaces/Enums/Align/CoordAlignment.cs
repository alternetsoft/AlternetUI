namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how an element is positioned or stretched
    /// within a parent's layout slot.
    /// </summary>
    public enum CoordAlignment
    {
        /// <summary>
        /// Align element towards the beginning of a parent's layout slot.
        /// </summary>
        Near = 0,

        /// <summary>
        /// Center element.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align element towards the end of a parent's layout slot.
        /// </summary>
        Far = 2,

        /// <summary>
        /// Stretch element within a parent's layout slot.
        /// </summary>
        Stretch = 3,

        /// <summary>
        /// Stretch element within a remaining empty space
        /// which is not occupied by other sibling elements.
        /// </summary>
        Fill = 4,
    }
}