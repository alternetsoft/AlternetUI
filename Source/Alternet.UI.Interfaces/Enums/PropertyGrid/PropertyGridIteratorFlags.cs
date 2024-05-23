using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags for the <see cref="PropertyGrid"/> iterator functions.
    /// </summary>
    /// <remarks>
    /// NOTES: At lower 16-bits, there are flags to check if item will be included.
    /// At higher 16-bits, there are same flags, but to instead check if children
    /// will be included.
    /// </remarks>
    public enum PropertyGridIteratorFlags
    {
        /// <summary>
        /// Iterate through 'normal' property items (does not include children of
        /// aggregate or hidden items by default).
        /// </summary>
        IterateProperties = PropertyGridItemFlags.Property |
                                  PropertyGridItemFlags.MiscParent |
                                  PropertyGridItemFlags.Aggregate |
                                  PropertyGridItemFlags.Collapsed |
                                  (PropertyGridItemFlags.MiscParent << 16) |
                                  (PropertyGridItemFlags.Category << 16),

        /// <summary>
        /// Iterate children of collapsed parents, and individual items that are hidden.
        /// </summary>
        IterateHidden = PropertyGridItemFlags.Hidden | (PropertyGridItemFlags.Collapsed << 16),

        /// <summary>
        /// Iterate children of parent that is an aggregate property (ie has fixed
        /// children).
        /// </summary>
        IterateFixedChildren = (PropertyGridItemFlags.Aggregate << 16) | IterateProperties,

        /// <summary>
        /// Iterate categories. Note that even without this flag, children of categories
        /// are still iterated through.
        /// </summary>
        IterateCategories = PropertyGridItemFlags.Category |
                                  (PropertyGridItemFlags.Category << 16) |
                                  PropertyGridItemFlags.Collapsed,

        /// <summary>
        /// Iterate all items that have children (not recursively).
        /// </summary>
        IterateAllParents = PropertyGridItemFlags.MiscParent |
                                   PropertyGridItemFlags.Aggregate |
                                   PropertyGridItemFlags.Category,

        /// <summary>
        /// Iterate all items that have children (recursively).
        /// </summary>
        IterateAllParentsRecursively = IterateAllParents | (IterateAllParents << 16),

        /// <summary>
        /// All iterator flags.
        /// </summary>
        IteratorFlagsAll = PropertyGridItemFlags.Property |
                                  PropertyGridItemFlags.MiscParent |
                                  PropertyGridItemFlags.Aggregate |
                                  PropertyGridItemFlags.Hidden |
                                  PropertyGridItemFlags.Category |
                                  PropertyGridItemFlags.Collapsed,

        /// <summary>
        /// Combines all flags needed to iterate through visible properties
        /// (ie. hidden properties and children of collapsed parents are skipped).
        /// </summary>
        IterateVisible = IterateProperties | PropertyGridItemFlags.Category |
                               (PropertyGridItemFlags.Aggregate << 16),

        /// <summary>
        /// Iterate all items.
        /// </summary>
        IterateAll = IterateVisible | IterateHidden,

        /// <summary>
        /// Iterate through individual properties (ie categories and children of
        /// aggregate properties are skipped).
        /// </summary>
        IterateNormal = IterateProperties | IterateHidden,

        /// <summary>
        /// Default iterator flags.
        /// </summary>
        IterateDefault = IterateNormal,
    }
}
