using System;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal class NativeToolbarEx : Toolbar
    {
        public NativeToolbarEx(bool mainToolbar)
        {
            SetNativePointer(NativeApi.Toolbar_CreateEx_(mainToolbar));
        }

        public NativeToolbarEx(IntPtr nativePointer)
            : base(nativePointer)
        {
        }
    }
}