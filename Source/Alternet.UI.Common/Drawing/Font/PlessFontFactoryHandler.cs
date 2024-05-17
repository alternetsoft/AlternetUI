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
    public class PlessFontFactoryHandler : IPlessFontFactoryHandler
    {
        private static IFontHandler? defaultFont;
        private static IFontHandler? defaultMonoFont;
        private static FontEncoding defaultFontEncoding = FontEncoding.Default;

        public virtual FontEncoding DefaultFontEncoding
        {
            get => defaultFontEncoding;
            set => defaultFontEncoding = value;
        }

        public virtual IFontHandler CreateDefaultFont()
        {
            defaultFont ??= new PlessFontHandler();
            return defaultFont;
        }

        public virtual IFontHandler CreateDefaultMonoFont()
        {
            defaultMonoFont ??= new PlessFontHandler();
            return defaultMonoFont;
        }

        public virtual IFontHandler CreateFont()
        {
            return new PlessFontHandler();
        }

        public virtual IFontHandler CreateFont(Font font)
        {
            var result = CreateFont();
            IFontHandler.FontParams prm = new(font);
            result.Update(prm);
            return result;
        }

        public virtual Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public virtual string[] GetFontFamiliesNames()
        {
            throw new NotImplementedException();
        }

        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsFontFamilyValid(string name)
        {
            throw new NotImplementedException();
        }

        public virtual void SetDefaultFont(Font value)
        {
            defaultFont = value.Handler;
        }
    }
}
