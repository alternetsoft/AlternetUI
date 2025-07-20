using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    /// <summary>Provides an interface to expose Win32 HWND handles.</summary>
    public interface IWin32Window
    {
        /// <summary>Gets the handle to the window represented by the implementer.</summary>
        /// <returns>A handle to the window represented by the implementer.</returns>
        IntPtr Handle { get; }
    }
}
