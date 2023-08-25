using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    internal enum PropertyGridItemValueFlags
    {
        // Flag for SetProperty* functions, HideProperty(), etc.
        // Apply changes only for the property in question.
        DontRecurse = 0x00000000,

        // Flag for GetPropertyValues().
        // Use this flag to retain category structure; each sub-category
        // will be its own VariantList of Variant.
        KeepStructure = 0x00000010,

        // Flag for SetProperty* functions, HideProperty(), etc.
        // Apply changes recursively for the property and all its children.
        Recurse = 0x00000020,

        // Flag for GetPropertyValues().
        // Use this flag to include property attributes as well.
        IncAttributes = 0x00000040,

        // Used when first starting recursion.
        RecurseStarts = 0x00000080,

        // Force value change.
        Force = 0x00000100,

        // Only sort categories and their immediate children.
        // Sorting done by AutoSort option uses this.
        SortTopLevelOnly = 0x00000200,
    }
}
