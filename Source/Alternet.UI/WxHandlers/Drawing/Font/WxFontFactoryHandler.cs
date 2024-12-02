using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class WxFontFactoryHandler : DisposableObject, IFontFactoryHandler
    {
        public FontEncoding DefaultFontEncoding
        {
            get => (FontEncoding)UI.Native.Font.GetDefaultEncoding();
            set => UI.Native.Font.SetDefaultEncoding((int)value);
        }

        public bool AllowNullFontName
        {
            get => false;
        }

        public IFontHandler CreateDefaultFontHandler()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultFont();
            return result;
        }

        public IFontHandler CreateFontHandler()
        {
            var result = new UI.Native.Font();
            return result;
        }

        public IFontHandler CreateFontHandler(Font font)
        {
            var result = new UI.Native.Font();
            result.InitializeFromFont((UI.Native.Font)font.Handler);
            return result;
        }

        public Font? CreateSystemFont(SystemSettingsFont systemFont)
        {
            var fnt = UI.Native.WxOtherFactory.SystemSettingsGetFont((int)systemFont);
            return new Font(fnt);
        }

        public IEnumerable<string> GetFontFamiliesNames()
        {
            return UI.Native.Font.Families;
        }

        public string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if(App.IsMacOS)
                return SkiaUtils.GetFontFamilyName(genericFamily);
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
