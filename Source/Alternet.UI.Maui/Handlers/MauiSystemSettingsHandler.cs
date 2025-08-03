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
    /// <summary>
    /// Provides platform-specific implementations for handling system settings
    /// in a .NET MAUI application.
    /// </summary>
    /// <remarks>This class extends <see cref="PlessSystemSettingsHandler"/> and implements
    /// <see cref="ISystemSettingsHandler"/>  to provide functionality specific to the MAUI platform,
    /// such as retrieving metrics, determining appearance settings,
    /// and creating display factory handlers.</remarks>
    public partial class MauiSystemSettingsHandler
        : PlessSystemSettingsHandler, ISystemSettingsHandler
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool IsUsingDarkBackground()
        {
            return GetAppearanceIsDark();
        }

        /// <inheritdoc/>
        public override bool GetAppearanceIsDark()
        {
            var currentTheme = Microsoft.Maui.Controls.Application.Current?.RequestedTheme;

            if (currentTheme == AppTheme.Dark)
                return true;
            return false;
        }

        /// <inheritdoc/>
        public override IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new MauiDisplayFactoryHandler();
        }

        /// <inheritdoc/>
        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.Maui;
        }
    }
}
