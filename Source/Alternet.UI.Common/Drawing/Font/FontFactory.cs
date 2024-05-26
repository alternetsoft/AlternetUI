using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class FontFactory
    {
        private static IFontFactoryHandler? handler;

        public static IFontFactoryHandler Handler
        {
            get => handler ??= BaseApplication.Handler.CreateFontFactoryHandler();
        }
    }
}
