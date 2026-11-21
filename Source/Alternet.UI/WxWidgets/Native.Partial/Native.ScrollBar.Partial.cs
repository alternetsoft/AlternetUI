using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class ScrollBar
    {
        public void OnPlatformEventScroll()
        {
            (UIControl as UI.ScrollBar)
                ?.RaiseHandlerScroll((ScrollEventType)EventTypeID, EventNewPos);
        }
    }
}