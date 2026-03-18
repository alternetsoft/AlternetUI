using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static members for working with known object attributes in application contexts.
    /// </summary>
    /// <remarks>This class contains constants and utility methods to facilitate the storage and retrieval of
    /// well-known attributes associated with objects. It is intended for use in scenarios where consistent
    /// identification and management of such attributes are required across the application.</remarks>
    public static class KnownObjectAttributes
    {
        /// <summary>
        /// Provides the key used to identify rich tooltip parameters in application contexts.
        /// </summary>
        /// <remarks>This key can be used when storing or retrieving rich tooltip-related data, such as in
        /// property bags or metadata collections. The value is globally unique to prevent conflicts with other
        /// keys.</remarks>
        public static string RichToolTipParams = "RichToolTipParams-D1AA1013-B4BB-43DB-BA61-C3B0D45C5D87";

        /// <summary>
        /// Gets the existing rich tooltip parameters associated with the specified object, or creates and adds them if
        /// they do not exist.
        /// </summary>
        /// <remarks>This method ensures that each object has at most one associated set of rich tooltip
        /// parameters. If the parameters do not exist, the provided factory is used to create and add them.</remarks>
        /// <param name="obj">The object for which to retrieve or add rich tooltip parameters. Cannot be null.</param>
        /// <param name="factory">A function that creates a new instance of rich tooltip parameters if none exist for the object. Cannot be
        /// null.</param>
        /// <returns>The existing or newly created rich tooltip parameters associated with the specified object.</returns>
        public static RichToolTipParams GetOrAddRichToolTipParams(IBaseObjectWithAttr obj, Func<RichToolTipParams> factory)
        {
            var result = obj.CustomAttr.GetAttributeOrAdd<RichToolTipParams>(RichToolTipParams, factory);
            return result;
        }
    }
}
