using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alternet.UI.Native
{
    internal static class MarshalEx
    {
        [return: NotNull]
        public static T PtrToStructure<T>(IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T))!;

        public static T GetDelegateForFunctionPointer<T>(IntPtr ptr)
            where T : Delegate
            =>
            (T)Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
    }
}