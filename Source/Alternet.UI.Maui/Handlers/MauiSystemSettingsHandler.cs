using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class MauiSystemSettingsHandler : PlessSystemSettingsHandler, ISystemSettingsHandler
    {
        public override int GetMetric(SystemSettingsMetric index)
        {
            var result = base.GetMetric(index);
            if (IsMetricScaled(index))
            {
                var scaleFactor = MauiDisplayHandler.GetDefaultScaleFactor();

                if (scaleFactor > 1)
                    result = (int)(result * scaleFactor);
            }

            return result;
        }

        public override bool GetAppearanceIsDark()
        {
            return base.GetAppearanceIsDark();
        }

        public override IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new MauiDisplayFactoryHandler();
        }

        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.Maui;
        }
    }
}
