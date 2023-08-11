namespace Alternet.UI
{
    /// <summary>
    /// Used to describe how a control is positioned or stretched horizontally within a parent's layout slot.
    /// </summary>
    /// <remarks>
    /// See <see cref="Control.HorizontalAlignment"/> for more details.
    /// </remarks>
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
    }

}