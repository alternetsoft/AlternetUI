using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Declares platform related methods.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="Default"/> property until native platform
    /// is initialized.
    /// </remarks>
    public abstract partial class NativePlatform : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativePlatform Default = new NotImplementedPlatform();

        public abstract int SystemSettingsGetMetric(SystemSettingsMetric index);

        public abstract string SystemSettingsAppearanceName();

        public abstract bool SystemSettingsAppearanceIsDark();

        public abstract bool SystemSettingsIsUsingDarkBackground();

        public abstract bool SystemSettingsHasFeature(SystemSettingsFeature index);

        public abstract Color SystemSettingsGetColor(SystemSettingsColor index);

        public abstract Font SystemSettingsGetFont(SystemSettingsFont systemFont);
    }
}
