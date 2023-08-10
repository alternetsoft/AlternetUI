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

        public DefaultPropsControls Controls { get; } = new();
    }
}