using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless <see cref="IFontFactoryHandler"/> implementation.
    /// </summary>
    public class PlessFontFactoryHandler : IFontFactoryHandler
    {
        private static IFontHandler? defaultFont;
        private static IFontHandler? defaultMonoFont;
        private static int defaultFontEncoding;

        public int DefaultFontEncoding
        {
            get => defaultFontEncoding;
            set => defaultFontEncoding = value;
        }

        public IFontHandler CreateDefaultFont()
        {
            defaultFont ??= new PlessFontHandler();
            return defaultFont;
        }

        public IFontHandler CreateDefaultMonoFont()
        {
            defaultMonoFont ??= new PlessFontHandler();
            return defaultMonoFont;
        }

        public IFontHandler CreateFont()
        {
            return new PlessFontHandler();
        }

        public IFontHandler CreateFont(Font font)
        {
            throw new NotImplementedException();
        }

        public Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public string[] GetFontFamiliesNames()
        {
            throw new NotImplementedException();
        }

        public string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            throw new NotImplementedException();
        }

        public bool IsFontFamilyValid(string name)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultFont(Font value)
        {
            defaultFont = value.Handler;
        }
    }
}
