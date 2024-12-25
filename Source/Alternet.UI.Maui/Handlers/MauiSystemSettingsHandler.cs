using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Alternet.UI
{
    internal partial class MauiSystemSettingsHandler
        : PlessSystemSettingsHandler, ISystemSettingsHandler
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
            var currentTheme = Application.Current?.RequestedTheme;

            if (currentTheme == AppTheme.Dark)
                return true;
            return false;
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
