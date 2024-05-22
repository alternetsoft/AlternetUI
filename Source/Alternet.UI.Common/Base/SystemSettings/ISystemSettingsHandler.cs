using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ISystemSettingsHandler
    {
        string GetAppearanceName();

        bool GetAppearanceIsDark();

        bool IsUsingDarkBackground();

        int GetMetric(SystemSettingsMetric index);

        bool HasFeature(SystemSettingsFeature index);

        Font GetFont(SystemSettingsFont systemFont);

        Color GetColor(KnownSystemColor index);

        int GetMetric(SystemSettingsMetric index, IControl? control);
    }
}
