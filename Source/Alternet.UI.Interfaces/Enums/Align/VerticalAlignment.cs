namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how an element is positioned or stretched
    /// vertically within a parent's layout slot.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Align element towards the top of a parent's layout slot.
        /// </summary>
        Top = 0,

        /// <summary>
        /// Center element vertically.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align element towards the bottom of a parent's layout slot.
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// Stretch element vertically within a parent's layout slot.
        /// </summary>
        Stretch = 3,

        /// <summary>
        /// Stretch element vertically within a remaining empty space
        /// which is not occupied by other sibling elements.
        /// </summary>
        Fill = 4,
    }
}