// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if FEATURE_WINDOWS_SYSTEM_COLORS

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class User32
    {
        // The returned value is a COLORREF. The docs don't say that explicitly, but
        // they do document the same macros (GetRValue, etc.). [0x00BBGGRR]
        //
        // This API sets last error, but we never check it and as such we won't take
        // the overhead in the P/Invoke. It will only fail if we try and grab a color
        // index that doesn't exist.

        // [SuppressGCTransition] // available only in .NET 5
        [DllImport("User32.dll", ExactSpelling = true)]
        internal static extern uint GetSysColor(int nIndex);
    }
}

#endif