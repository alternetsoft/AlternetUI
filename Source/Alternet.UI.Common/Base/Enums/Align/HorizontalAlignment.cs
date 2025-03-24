namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how an element is positioned or stretched
    /// horizontally within a parent's layout slot.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Align element towards the left of a parent's layout slot.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Center element horizontally.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align element towards the right of a parent's layout slot.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Stretch element horizontally within a parent's layout slot.
        /// </summary>
        Stretch = 3,

        /// <summary>
        /// Stretch element horizontally within a remaining empty space
        /// which is not occupied by other sibling elements.
        /// </summary>
        Fill = 4,
    }
}