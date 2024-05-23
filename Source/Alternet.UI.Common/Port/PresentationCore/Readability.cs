#pragma warning disable
namespace Alternet.UI
{
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
        /// Targeted value's readability inherites from parent nodes.
        /// </summary>
        Inherit = 2,
    }
}