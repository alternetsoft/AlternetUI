using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains platform specific settings.
    /// </summary>
    public class DefaultPropsPlatform
    {
        static DefaultPropsPlatform()
        {
        }

        /// <summary>
        /// Returns default property values for all controls in the library.
        /// </summary>
        public DefaultPropsControls Controls { get; } = new();
    }
}