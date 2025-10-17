using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Skia;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements <see cref="IFontFactoryHandler"/> interface provider for SkiaSharp fonts.
    /// </summary>
    public class SkiaFontFactoryHandler : DisposableObject, IFontFactoryHandler
    {
        /// <summary>
        /// Gets default <see cref="IFontFactoryHandler"/> provider for SkiaSharp fonts.
        /// </summary>
        public static readonly SkiaFontFactoryHandler Default = new();

        static SkiaFontFactoryHandler()
        {
        }

        /// <summary>
        /// Gets or sets default font encoding.
        /// </summary>
        public virtual FontEncoding DefaultFontEncoding { get; set; } = FontEncoding.Default;

        /// <summary>
        /// Gets or sets whether to allow an empty font name.
        /// </summary>
        public virtual bool AllowNullFontName
        {
            get;
            set;
        }

        /// <summary>
        /// Creates <see cref="IFontHandler"/> interface provider for the default font.
        /// </summary>
        /// <returns></returns>
        public virtual IFontHandler CreateDefaultFontHandler()
        {
            return new SkiaFontHandler(SkiaHelper.DefaultFontName, SkiaHelper.DefaultFontSize);
        }

        /// <summary>
        /// Creates <see cref="IFontHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        public virtual IFontHandler CreateFontHandler()
        {
            return new SkiaFontHandler();
        }

        /// <summary>
        /// Creates system font.
        /// </summary>
        /// <returns></returns>
        public virtual Font? CreateSystemFont(SystemSettingsFont systemFont)
        {
            return null;
        }

        /// <summary>
        /// Gets font families names.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetFontFamiliesNames() => SkiaHelper.GetFontFamiliesNames();

        /// <summary>
        /// Gets generic font family name.
        /// </summary>
        /// <param name="genericFamily">Generic font family id.</param>
        /// <returns></returns>
        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
            => SkiaUtils.GetFontFamilyName(genericFamily);

        /// <summary>
        /// Sets default font for use with SkiaSharp.
        /// </summary>
        /// <param name="value">Font.</param>
        public virtual void SetDefaultFont(Font value)
        {
            SkiaHelper.DefaultFontSize = value?.SizeInPoints ?? SkiaHelper.DefaultFontSize;
            SkiaHelper.DefaultFontName = value?.Name ?? SKTypeface.Default.FamilyName;
        }
    }
}