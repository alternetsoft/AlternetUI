using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags for <see cref="IWrapSizer"/>.
    /// </summary>
    [Flags]
    internal enum WrapSizerFlag
    {
        /// <summary>
        /// Causes the last item on each line to use any remaining space on that line.
        /// </summary>
        ExtendLastOnEachLine = 1,

        /// <summary>
        /// Removes any spacer elements from the beginning of a row.
        /// </summary>
        RemoveLeadingSpaces = 2,

        /// <summary>
        /// Default value.
        /// </summary>
        Default = ExtendLastOnEachLine | RemoveLeadingSpaces,
    }
}