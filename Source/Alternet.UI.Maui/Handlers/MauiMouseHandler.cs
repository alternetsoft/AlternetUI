using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS
using Windows.Devices.Input;
#endif
#if ANDROID
#endif
#if IOS || MACCATALYST
#endif

namespace Alternet.UI
{
    internal partial class MauiMouseHandler : PlessMouseHandler
    {
#if WINDOWS
        private static MouseCapabilities? mouseCapabilities;

        public static MouseCapabilities MouseCapabilities =>
            mouseCapabilities ??= new ();
#endif

#if ANDROID
#endif

#if IOS || MACCATALYST
#endif
        public override bool? MousePresent
        {
            get
            {
#if WINDOWS
                return MouseCapabilities.MousePresent != 0;
#endif
#if ANDROID
                return base.MousePresent;
#endif
#if IOS || MACCATALYST
                return base.MousePresent;
#endif
            }
        }
    }
}