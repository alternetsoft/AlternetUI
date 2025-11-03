using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to macOs operating system.
    /// </summary>
    public static class MacOsUtils
    {
        /// <summary>
        /// Retrieves a list of loaded native libraries on macOS.
        /// </summary>
        /// <returns>An array of strings containing the paths of the loaded libraries.</returns>
        internal static string[] GetLoadedLibraries()
        {
            List<string> libraries = new();
            int count = NativeMethods._dyld_image_count();

            for (int i = 0; i < count; i++)
            {
                IntPtr namePtr = NativeMethods._dyld_get_image_name(i);
                var name = Marshal.PtrToStringAnsi(namePtr);
                if (!string.IsNullOrEmpty(name))
                    libraries.Add(name);
            }

            return libraries.ToArray();
        }

        /// <summary>
        /// Contains native methods.
        /// </summary>
        public static class NativeMethods
        {
            [DllImport("libSystem.dylib")]
            internal static extern int _dyld_image_count();

            [DllImport("libSystem.dylib")]
            internal static extern IntPtr _dyld_get_image_name(int index);
        }
    }
}
