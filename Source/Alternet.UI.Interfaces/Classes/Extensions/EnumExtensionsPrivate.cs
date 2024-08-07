using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Contains extension methods for the enums.
    /// </summary>
    public static class EnumExtensionsPrivate
    {
        /// <summary>
        /// Converts <see cref="LogItemKind"/> to <see cref="LogItemKindFlags"/>.
        /// </summary>
        /// <param name="kind">Value to convert.</param>
        /// <returns></returns>
        public static LogItemKindFlags ToFlags(this LogItemKind kind)
        {
            switch (kind)
            {
                case LogItemKind.Information:
                    return LogItemKindFlags.Information;
                case LogItemKind.Error:
                    return LogItemKindFlags.Error;
                case LogItemKind.Warning:
                    return LogItemKindFlags.Warning;
                case LogItemKind.Other:
                    return LogItemKindFlags.Other;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets whether <see cref="LogItemKindFlags"/> has <see cref="LogItemKind"/> flag.
        /// </summary>
        /// <param name="flags">Value to check.</param>
        /// <param name="kind">Flag to test.</param>
        /// <returns></returns>
        public static bool HasKind(this LogItemKindFlags flags, LogItemKind kind)
        {
            var flag = kind.ToFlags();
            return flags.HasFlag(flag);
        }
    }
}
