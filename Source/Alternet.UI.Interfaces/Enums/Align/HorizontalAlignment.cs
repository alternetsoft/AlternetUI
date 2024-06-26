namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how a control is positioned or stretched
    /// horizontally within a parent's layout slot.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Align control towards the left of a parent's layout slot.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Center control horizontally.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align control towards the right of a parent's layout slot.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Stretch control horizontally within a parent's layout slot.
        /// </summary>
        Stretch = 3,

        /// <summary>
        /// Stretch control horizontally within a remaining empty space
        /// which is not occupied by other sibling controls.
        /// </summary>
        Fill = 4,
    }
}