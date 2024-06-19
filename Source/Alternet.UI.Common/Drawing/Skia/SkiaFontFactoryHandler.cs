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
        public static readonly SkiaFontFactoryHandler Default = new();

        static SkiaFontFactoryHandler()
        {
        }

        public virtual FontEncoding DefaultFontEncoding { get; set; } = FontEncoding.Default;

        public virtual bool AllowNullFontName
        {
            get;
            set;
        }

        public virtual IFontHandler CreateDefaultFontHandler()
        {
            return new PlessFontHandler(SkiaUtils.DefaultFontName, SkiaUtils.DefaultFontSize);
        }

        public virtual IFontHandler CreateFontHandler()
        {
            return new PlessFontHandler();
        }

        public virtual Font? CreateSystemFont(SystemSettingsFont systemFont)
        {
            return null;
        }

        public virtual IEnumerable<string> GetFontFamiliesNames() => SkiaUtils.GetFontFamiliesNames();

        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
            => SkiaUtils.GetFontFamilyName(genericFamily);

        public virtual void SetDefaultFont(Font value)
        {
            SkiaUtils.DefaultFontSize = value?.SizeInPoints ?? SkiaUtils.DefaultFontSize;
            SkiaUtils.DefaultFontName = value?.Name ?? SKTypeface.Default.FamilyName;
        }
    }
}