using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    internal static class MessageBox
    {
        public static void Show(string text, string? caption = null)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            NativeApi.MessageBox_Show(text, caption);
        }

        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi()
            {
                Initialize();
            }

            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MessageBox_Show(string text, string? caption);
        }
    }
}