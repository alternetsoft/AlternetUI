using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public class SkiaFontFactoryHandler : DisposableObject, IFontFactoryHandler
    {
        private static double defaultFontSize = 12;
        private static string? defaultFontName;
        private static string? defaultMonoFontName;

        static SkiaFontFactoryHandler()
        {
        }

        public static double DefaultFontSize
        {
            get => defaultFontSize;

            set
            {
                defaultFontSize = value;
            }
        }

        public static string DefaultFontName
        {
            get => defaultFontName ?? SKTypeface.Default.FamilyName;
            set => defaultFontName = value;
        }

        public static string DefaultMonoFontName
        {
            get => defaultMonoFontName ?? SKTypeface.Default.FamilyName;
            set => defaultMonoFontName = value;
        }

        public virtual FontEncoding DefaultFontEncoding
        {
            get;
            set;
        } = FontEncoding.Default;

        public virtual bool AllowNullFontName
        {
            get;
            set;
        }

        public virtual IFontHandler CreateDefaultFontHandler()
        {
            return new PlessFontHandler(DefaultFontName, DefaultFontSize);
        }

        public virtual IFontHandler CreateDefaultMonoFontHandler()
        {
            return new PlessFontHandler(DefaultMonoFontName, DefaultFontSize);
        }

        public virtual IFontHandler CreateFontHandler()
        {
            return new PlessFontHandler();
        }

        public virtual Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            IFontHandler handler;

            switch (systemFont)
            {
                case SystemSettingsFont.OemFixed:
                case SystemSettingsFont.AnsiFixed:
                    handler = CreateDefaultMonoFontHandler();
                    break;
                default:
                    handler = CreateDefaultFontHandler();
                    break;
            }

            return new Font(handler);
        }

        public virtual string[] GetFontFamiliesNames()
        {
            return SKFontManager.Default.GetFontFamilies();
        }

        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            string name;

            switch (genericFamily)
            {
                case GenericFontFamily.Monospace:
                    name = DefaultMonoFontName;
                    break;
                default:
                    name = DefaultFontName;
                    break;
            }

            return name;
        }

        public virtual void SetDefaultFont(Font value)
        {
            defaultFontSize = value?.SizeInPoints ?? 12;
            defaultFontName = value?.Name;
        }
    }
}