using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class WxPlatform : NativePlatform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            WxDrawing.Initialize();
            NativePlatform.Default = new WxPlatform();
            initialized = true;
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, default);
        }

        public override string SystemSettingsAppearanceName()
        {
            return Native.WxOtherFactory.SystemAppearanceGetName();
        }

        public override bool SystemSettingsAppearanceIsDark()
        {
            return Native.WxOtherFactory.SystemAppearanceIsDark();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            return Native.WxOtherFactory.SystemAppearanceIsUsingDarkBackground();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            return Native.WxOtherFactory.SystemSettingsHasFeature((int)index);
        }

        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            return Native.WxOtherFactory.SystemSettingsGetColor((int)index);
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            var fnt = Native.WxOtherFactory.SystemSettingsGetFont((int)systemFont);
            return new Font(fnt);
        }
    }
}
