using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public static partial class LogUtils
    {
        internal static void LogCheckConstraints(Type control)
        {
            if (!AssemblyUtils.HasConstructorNoParams(control))
            {
                App.Log($"Control '{control}' has no constructor without params");
            }
        }

        internal static void LogCheckConstraints()
        {
            var controls = AssemblyUtils.AllControlDescendants.Values;

            foreach(var control in controls)
            {
                LogCheckConstraints(control);
            }
        }
    }
}
