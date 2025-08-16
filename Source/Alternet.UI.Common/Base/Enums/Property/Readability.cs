namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible readability kinds for the targeted value.
    /// </summary>
    public enum Readability
    {
        /// <summary>
        /// Targeted value is not readable.
        /// </summary>
        Unreadable = 0,

        /// <summary>
        /// Targeted value is readable text.
        /// </summary>
        Readable = 1,

        /// <summary>
        /// Targeted value's readability inherits from parent.
        /// </summary>
        Inherit = 2,
    }
}