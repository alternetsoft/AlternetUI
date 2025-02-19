using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains refrences to some of the known types.
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// Gets 'Alternet.UI.MauiUtils' type.
        /// </summary>
        public static LazyStruct<Type?> MauiUtils = new(() =>
        {
            var result = KnownAssemblies.LibraryMaui.Value?.GetType("Alternet.UI.MauiUtils");
            return result;
        });

        /// <summary>
        /// Gets 'System.Runtime.InteropServices.NativeLibrary' type.
        /// </summary>
        public static LazyStruct<Type?> InteropServicesNativeLibrary = new(() =>
        {
            var asm = KnownAssemblies.LibraryInteropServices.Value;
            var result = asm?.GetType("System.Runtime.InteropServices.NativeLibrary");
            return result;
        });

        /// <summary>
        /// Gets 'System.Runtime.InteropServices.DllImportResolver' type.
        /// </summary>
        public static LazyStruct<Type?> InteropServicesDllImportResolver = new(() =>
        {
            var asm = KnownAssemblies.LibraryInteropServices.Value;
            var result = asm?.GetType("System.Runtime.InteropServices.DllImportResolver");
            return result;
        });
    }
}