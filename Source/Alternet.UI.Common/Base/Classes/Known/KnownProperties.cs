using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains property information for some of the properties.
    /// </summary>
    public static class KnownProperties
    {
        /// <summary>
        /// Contains property information for some of the <see cref="AbstractControl"/> properties.
        /// </summary>
        public static class AbstractControlProperties
        {
            private static PropertyInfo? enabled;
            private static PropertyInfo? visible;

            /// <summary>
            /// Gets property information for the 'Visible' property of the control.
            /// </summary>
            public static PropertyInfo? Visible
            {
                get
                {
                    return visible ??= GetControlProperty("IsVisible");
                }
            }

            /// <summary>
            /// Gets property information for the 'Enabled' property of the control.
            /// </summary>
            public static PropertyInfo? Enabled
            {
                get
                {
                    return enabled ??= GetControlProperty("IsEnabled");
                }
            }

            private static PropertyInfo? GetControlProperty(string propName)
            {
                return AssemblyUtils.GetPropertySafe(
                                        typeof(AbstractControl),
                                        propName,
                                        BindingFlags.Public | BindingFlags.Instance);
            }
        }
    }
}
