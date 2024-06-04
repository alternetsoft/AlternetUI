using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides access to the font factory.
    /// </summary>
    public static class FontFactory
    {
        private static IFontFactoryHandler? handler;

        public static bool OnlySkiaFonts { get; set; } = true;

        public static IFontFactoryHandler Handler
        {
            get => handler ??= GraphicsFactory.Handler.CreateFontFactoryHandler();

            set => handler = value;
        }
    }
}
