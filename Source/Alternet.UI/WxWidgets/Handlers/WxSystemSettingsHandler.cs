using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxSystemSettingsHandler : ISystemSettingsHandler
    {
        public Color GetColor(KnownSystemColor index)
        {
            SystemSettingsColor systemSettingsColor = WxColorUtils.Convert(index);
            return UI.Native.WxOtherFactory.SystemSettingsGetColor((int)systemSettingsColor);
        }

        public int GetMetric(SystemSettingsMetric index, IControl? control)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric(
                (int)index,
                WxPlatform.WxWidget(control));
        }

        public int GetMetric(SystemSettingsMetric index)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, default);
        }

        public string GetAppearanceName()
        {
            return Native.WxOtherFactory.SystemAppearanceGetName();
        }

        public bool GetAppearanceIsDark()
        {
            return Native.WxOtherFactory.SystemAppearanceIsDark();
        }

        public bool IsUsingDarkBackground()
        {
            return Native.WxOtherFactory.SystemAppearanceIsUsingDarkBackground();
        }

        public bool HasFeature(SystemSettingsFeature index)
        {
            return Native.WxOtherFactory.SystemSettingsHasFeature((int)index);
        }

        public Font GetFont(SystemSettingsFont systemFont)
        {
            var fnt = Native.WxOtherFactory.SystemSettingsGetFont((int)systemFont);
            return new Font(fnt);
        }
    }
}
