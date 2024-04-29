using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class MauiPlatform : NativePlatform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            SkiaDrawing.Initialize();
            NativePlatform.Default = new MauiPlatform();
            initialized = true;
        }
    }
}
