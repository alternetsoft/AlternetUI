namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible modifiability kinds for the targeted value.
    /// </summary>
    public enum Modifiability
    {
        /// <summary>
        /// Targeted value is not modifiable.
        /// </summary>
        Unmodifiable = 0,

        /// <summary>
        /// Targeted value is modifiable.
        /// </summary>
        Modifiable = 1,

        /// <summary>
        /// Targeted value's modifiability inherits from the the parent.
        /// </summary>
        Inherit = 2,
    }
}