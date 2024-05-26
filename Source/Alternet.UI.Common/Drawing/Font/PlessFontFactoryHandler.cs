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

        private bool allowNullFontName;

        public virtual bool AllowNullFontName
        {
            get => allowNullFontName;
            set => allowNullFontName = value;
        }

        public virtual FontEncoding DefaultFontEncoding
        {
            get => defaultFontEncoding;
            set => defaultFontEncoding = value;
        }

        public virtual IFontHandler CreateDefaultFontHandler()
        {
            defaultFont ??= new PlessFontHandler();
            return defaultFont;
        }

        public virtual IFontHandler CreateDefaultMonoFontHandler()
        {
            defaultMonoFont ??= new PlessFontHandler();
            return defaultMonoFont;
        }

        public virtual IFontHandler CreateFontHandler()
        {
            return new PlessFontHandler();
        }

        public virtual IFontHandler CreateFontHandler(Font font)
        {
            var result = CreateFontHandler();
            IFontHandler.FontParams prm = new(font);
            result.Update(prm);
            return result;
        }

        public virtual Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            return new Font(CreateFontHandler());
        }

        public virtual string[] GetFontFamiliesNames()
        {
            return new string[] { Font.Default.Name };
        }

        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            return Font.Default.Name;
        }

        public virtual bool IsFontFamilyValid(string name)
        {
            return Font.Default.Name == name;
        }

        public virtual void SetDefaultFont(Font value)
        {
            defaultFont = value.Handler;
        }
    }
}
