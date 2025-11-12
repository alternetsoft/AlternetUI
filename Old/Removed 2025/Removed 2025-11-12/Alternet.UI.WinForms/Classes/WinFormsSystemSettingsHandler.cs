using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI.WinForms
{
    /// <summary>
    /// Provides platform-specific implementations for handling system settings
    /// in a .NET MAUI application.
    /// </summary>
    /// <remarks>This class extends <see cref="PlessSystemSettingsHandler"/> and implements
    /// <see cref="ISystemSettingsHandler"/>  to provide functionality specific to the MAUI platform,
    /// such as retrieving metrics, determining appearance settings,
    /// and creating display factory handlers.</remarks>
    public partial class WinFormsSystemSettingsHandler
        : PlessSystemSettingsHandler, ISystemSettingsHandler
    {
        /// <inheritdoc/>
        public override int GetMetric(SystemSettingsMetric index)
        {
            var result = base.GetMetric(index);
            if (IsMetricScaled(index))
            {
                var scaleFactor = WinFormsDisplayHandler.GetDefaultScaleFactor();

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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new WinFormsDisplayFactoryHandler();
        }

        /// <inheritdoc/>
        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.WinForms;
        }
    }
}
