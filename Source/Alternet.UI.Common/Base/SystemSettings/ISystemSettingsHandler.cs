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
        string AppName { get; set; }

        string AppDisplayName { get; set; }

        string AppClassName { get; set; }

        string VendorName { get; set; }

        string VendorDisplayName { get; set; }

        bool UseBestVisual { get; set; }

        IDisplayFactoryHandler CreateDisplayFactoryHandler();

        bool SetNativeTheme(string theme);

        void SetUseBestVisual(bool flag, bool forceTrueColour = false);

        string GetAppearanceName();

        bool GetAppearanceIsDark();

        bool IsUsingDarkBackground();

        int GetMetric(SystemSettingsMetric index);

        bool HasFeature(SystemSettingsFeature index);

        ColorStruct? GetColor(KnownSystemColor index);

        int GetMetric(SystemSettingsMetric index, Control? control);

        string GetLibraryVersionString();

        string? GetUIVersion();

        void SetSystemOption(string name, int value);

        LangDirection GetLangDirection();

        UIPlatformKind GetPlatformKind();

        void SuppressBellOnError(bool value);

        Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);
    }
}
