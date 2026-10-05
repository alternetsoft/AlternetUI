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
    internal class SystemSettingsHandlerWinforms : Alternet.UI.DisposableObject, Alternet.UI.ISystemSettingsHandler
    {
        public virtual string AppName { get; set; } = string.Empty;
        
        public virtual string AppDisplayName { get; set; } = string.Empty;
        
        public virtual string AppClassName { get; set; } = string.Empty;
        
        public virtual string VendorName { get; set; } = string.Empty;
        
        public virtual string VendorDisplayName { get; set; } = string.Empty;
        
        public virtual bool UseBestVisual { get; set; } = true;

        public virtual IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            throw new NotImplementedException();
        }

        public virtual bool GetAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public virtual ColorStruct? GetColor(KnownSystemColor index)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }

        public virtual LangDirection GetLangDirection()
        {
            throw new NotImplementedException();
        }

        public virtual int GetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public virtual int GetMetric(SystemSettingsMetric index, AbstractControl? control)
        {
            throw new NotImplementedException();
        }

        public virtual UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.WinForms;
        }

        public virtual string? GetUIVersion()
        {
            Assembly thisAssembly = typeof(App).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName?.Version;
            return ver?.ToString();
        }

        public virtual string GetLibraryVersionString()
        {
            return GetUIVersion() ?? string.Empty;
        }

        public virtual string GetAppearanceName()
        {
            return "Default";
        }

        public virtual bool HasFeature(SystemSettingsFeature index)
        {
            return false;
        }

        public virtual bool SetNativeTheme(string theme)
        {
            return false;
        }

        public virtual void SetSystemOption(string name, int value)
        {
        }

        public virtual void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
        }
    }
}
