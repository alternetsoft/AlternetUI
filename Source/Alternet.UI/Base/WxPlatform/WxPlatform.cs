using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class WxPlatform : NativePlatform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            WxDrawing.Initialize();
            NativePlatform.Default = new WxPlatform();
            initialized = true;
        }
    }
}
