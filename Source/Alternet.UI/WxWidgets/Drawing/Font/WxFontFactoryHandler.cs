using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class WxFontFactoryHandler : IFontFactoryHandler
    {
        public static readonly IFontFactoryHandler Default = new WxFontFactoryHandler();

        public FontEncoding DefaultFontEncoding
        {
            get => (FontEncoding)UI.Native.Font.GetDefaultEncoding();
            set => UI.Native.Font.SetDefaultEncoding((int)value);
        }

        public IFontHandler CreateDefaultFont()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultFont();
            return result;
        }

        public IFontHandler CreateDefaultMonoFont()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultMonoFont();
            return result;
        }

        public IFontHandler CreateFont()
        {
            var result = new UI.Native.Font();
            return result;
        }

        public IFontHandler CreateFont(Font font)
        {
            var result = new UI.Native.Font();
            result.InitializeFromFont((UI.Native.Font)font.Handler);
            return result;
        }

        public Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            return SystemSettings.GetFont(systemFont);
        }

        public string[] GetFontFamiliesNames()
        {
            return UI.Native.Font.Families;
        }

        public string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            return UI.Native.Font.GetGenericFamilyName(genericFamily);
        }

        public bool IsFontFamilyValid(string name)
        {
            return UI.Native.Font.IsFamilyValid(name);
        }

        public void SetDefaultFont(Font value)
        {
            UI.Native.Window.SetParkingWindowFont((UI.Native.Font?)value?.Handler);
        }
    }
}
