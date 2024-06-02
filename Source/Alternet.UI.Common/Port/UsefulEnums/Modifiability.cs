#pragma warning disable
namespace Alternet.UI
{
    public enum Modifiability
    {
        /// <summary>
        /// Targeted value is not modifiable by localizers.
        /// </summary>
        Unmodifiable = 0,

        /// <summary>
        /// Targeted value is modifiable by localizers.
        /// </summary>
        Modifiable = 1,

        /// <summary>
        /// Targeted value's modifiability inherits from the the parent nodes.
        /// </summary>
        Inherit = 2,
    }
}