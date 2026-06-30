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
        private IFontHandler? defaultFontHandler;
        private IFontHandler? defaultMonoFontHandler;

        public FontEncoding DefaultFontEncoding
        {
            get => (FontEncoding)UI.Native.Font.GetDefaultEncoding();
            set => UI.Native.Font.SetDefaultEncoding((int)value);
        }

        public bool AllowNullFontName
        {
            get => false;
        }

        public float GetDefaultFontSize()
        {
            defaultFontHandler ??= CreateDefaultFontHandler();
            return defaultFontHandler.SizeInPoints;
        }

        public string GetDefaultMonoFontName()
        {
            defaultMonoFontHandler ??= CreateDefaultMonoFontHandler();
            return defaultMonoFontHandler.GetName();
        }

        public IFontHandler CreateDefaultMonoFontHandler()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultMonoFont();
            return result;
        }

        public string GetDefaultFontName()
        {
            defaultFontHandler ??= CreateDefaultFontHandler();
            return defaultFontHandler.GetName();
        }

        public Font CreateDefaultFont()
        {
            defaultFontHandler ??= CreateDefaultFontHandler();
            return new Font(defaultFontHandler.GetName(), defaultFontHandler.SizeInPoints);
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
            var result = NativeUtils.ToStringArray(UI.Native.Font.GetFamilies());
            return result;
        }

        public string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if(App.IsMacOS)
                return SkiaUtils.GetFontFamilyName(genericFamily);
            return UI.Native.Font.GetGenericFamilyName(genericFamily).ToString();
        }

        public bool IsFontFamilyValid(string name)
        {
            return NativeStringSpan.InvokeWithResult(name, span =>
            {
                return UI.Native.Font.IsFamilyValid(span);
            });
        }

        public void SetDefaultFont(Font value)
        {
            UI.Native.Window.SetParkingWindowFont((UI.Native.Font?)value?.Handler);
        }
    }
}
