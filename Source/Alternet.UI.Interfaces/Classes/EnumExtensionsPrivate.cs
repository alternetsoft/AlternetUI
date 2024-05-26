using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI.Extensions
{
    public static class EnumExtensionsPrivate
    {
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

        public static bool HasKind(this LogItemKindFlags flags, LogItemKind kind)
        {
            var flag = kind.ToFlags();
            return flags.HasFlag(flag);
        }
    }
}
