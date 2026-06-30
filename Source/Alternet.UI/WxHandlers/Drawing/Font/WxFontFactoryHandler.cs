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

        public Font CreateDefaultMonoFont()
        {
            defaultMonoFontHandler ??= CreateDefaultMonoFontHandler();
            return new Font(defaultMonoFontHandler.GetName(), defaultMonoFontHandler.SizeInPoints);
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
            var fontRef = WxControlHandler.GetFontRef(font);
            result.InitializeFromFontRef(fontRef);
            return result;
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
            var fontRef = WxControlHandler.GetFontRef(value);
            UI.Native.Window.SetParkingWindowFontRef(fontRef);
        }
    }
}
