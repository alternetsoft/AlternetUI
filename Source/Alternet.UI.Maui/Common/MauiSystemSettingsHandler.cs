using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class MauiSystemSettingsHandler : DisposableObject, ISystemSettingsHandler
    {
        public string AppName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string AppDisplayName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string AppClassName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string VendorName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string VendorDisplayName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public bool UseBestVisual
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new MauiDisplayFactoryHandler();
        }

        public bool GetAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public string GetAppearanceName()
        {
            throw new NotImplementedException();
        }

        public Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public ColorStruct? GetColor(KnownSystemColor index)
        {
            return null;
        }

        public LangDirection GetLangDirection()
        {
            throw new NotImplementedException();
        }

        public string GetLibraryVersionString()
        {
            throw new NotImplementedException();
        }

        public int GetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public int GetMetric(SystemSettingsMetric index, IControl? control)
        {
            throw new NotImplementedException();
        }

        public UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.Maui;
        }

        public string? GetUIVersion()
        {
            throw new NotImplementedException();
        }

        public bool HasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        public bool IsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }

        public bool SetNativeTheme(string theme)
        {
            throw new NotImplementedException();
        }

        public void SetSystemOption(string name, int value)
        {
            throw new NotImplementedException();
        }

        public void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
            throw new NotImplementedException();
        }

        public void SuppressBellOnError(bool value)
        {
            throw new NotImplementedException();
        }
    }
}
