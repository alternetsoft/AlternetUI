using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class MauiPlatform : NativePlatform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            SkiaDrawing.Initialize();
            NativePlatform.Default = new MauiPlatform();
            initialized = true;
        }

        public override bool SystemSettingsAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public override string SystemSettingsAppearanceName()
        {
            throw new NotImplementedException();
        }

        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            throw new NotImplementedException();
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }
    }
}
