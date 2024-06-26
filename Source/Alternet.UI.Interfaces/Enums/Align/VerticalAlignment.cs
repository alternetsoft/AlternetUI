namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how a control is positioned or stretched
    /// vertically within a parent's layout slot.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Align control towards the top of a parent's layout slot.
        /// </summary>
        Top = 0,

        /// <summary>
        /// Center control vertically.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align control towards the bottom of a parent's layout slot.
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// Stretch control vertically within a parent's layout slot.
        /// </summary>
        Stretch = 3,

        /// <summary>
        /// Stretch control vertically within a remaining empty space
        /// which is not occupied by other sibling controls.
        /// </summary>
        Fill = 4,
    }
}