using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Winforms
{
    internal class SystemSettingsHandlerWinforms : PlessSystemSettingsHandler, ISystemSettingsHandler
    {
        /// <inheritdoc/>
        public override int GetMetric(SystemSettingsMetric index)
        {
            var result = base.GetMetric(index);
            if (IsMetricScaled(index))
            {
                var scaleFactor = WinformsUtils.GetScaleFactor(WinformsUtils.MainForm);

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
#if NET10_0_OR_GREATER
            return System.Windows.Forms.Application.IsDarkModeEnabled;
#else
            return false;
#endif
        }

        /// <inheritdoc/>
        public override ColorStruct? GetColor(Alternet.Drawing.KnownSystemColor index)
        {
            var drawingColor = WinformsUtils.GetColor(index);
            if (drawingColor.HasValue)
            {
                return WinformsUtils.CreateColorStruct(drawingColor.Value);
            }

            return null;
        }

        /// <inheritdoc/>
        public override IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.WinForms;
        }
    }
}
