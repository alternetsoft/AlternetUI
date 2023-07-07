using System;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal class NativeListBoxEx : ListBox
    {
        public NativeListBoxEx()
        {
            SetNativePointer(NativeApi.ListBox_CreateEx_(1));
        }

        public NativeListBoxEx(IntPtr nativePointer)
            : base(nativePointer)
        {
        }
    }
}