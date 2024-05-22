using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines flags used in <see cref="PropertyGrid"/> methods.
    /// </summary>
    [Flags]
    public enum PropertyGridItemValueFlags
    {
        /// <summary>
        /// Flag for SetProperty* functions, HideProperty(), etc.
        /// Apply changes only for the property in question.
        /// </summary>
        DontRecurse = 0x00000000,

        /// <summary>
        /// Use this flag to retain category structure; each sub-category
        /// will be its own VariantList of Variant.
        /// </summary>
        // Flag for GetPropertyValues().
        KeepStructure = 0x00000010,

        /// <summary>
        /// Flag for SetProperty* functions, HideProperty(), etc.
        /// Apply changes recursively for the property and all its children.
        /// </summary>
        Recurse = 0x00000020,

        /// <summary>
        /// Flag for GetPropertyValues().
        /// Use this flag to include property attributes as well.
        /// </summary>
        IncAttributes = 0x00000040,

        /// <summary>
        /// Used when first starting recursion.
        /// </summary>
        RecurseStarts = 0x00000080,

        /// <summary>
        /// Force value change.
        /// </summary>
        Force = 0x00000100,

        /// <summary>
        /// Only sort categories and their immediate children.
        /// Sorting done by AutoSort option uses this.
        /// </summary>
        SortTopLevelOnly = 0x00000200,
    }
}
