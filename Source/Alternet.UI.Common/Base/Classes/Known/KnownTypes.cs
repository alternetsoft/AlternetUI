using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Contains references to some of the known types.
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// Provides a lazily initialized reference to the SkiaSharp.SkiaApi type, if available.
        /// </summary>
        /// <remarks>This property attempts to retrieve the type <c>SkiaSharp.SkiaApi</c> from the
        /// assembly containing the <see cref="SKCanvas"/> class.
        /// The value will be <see langword="null"/> if the type
        /// cannot be found.</remarks>
        public static readonly LazyStruct<Type?> SkiaSharpSkiaApi = new (() =>
        {
            var result = typeof(SKCanvas).Assembly.GetType("SkiaSharp.SkiaApi");
            return result;
        });

        /// <summary>
        /// Gets 'Alternet.UI.MauiUtils' type.
        /// </summary>
        public static readonly LazyStruct<Type?> MauiUtils = new(() =>
        {
            var result = KnownAssemblies.LibraryMaui.Value?.GetType("Alternet.UI.MauiUtils");
            return result;
        });

        /// <summary>
        /// Gets 'System.Runtime.InteropServices.NativeLibrary' type.
        /// </summary>
        public static readonly LazyStruct<Type?> InteropServicesNativeLibrary = new(() =>
        {
            var asm = KnownAssemblies.LibraryInteropServices.Value;
            var result = asm?.GetType("System.Runtime.InteropServices.NativeLibrary");
            return result;
        });

        /// <summary>
        /// Gets 'System.Runtime.InteropServices.DllImportResolver' type.
        /// </summary>
        public static readonly LazyStruct<Type?> InteropServicesDllImportResolver = new(() =>
        {
            var asm = KnownAssemblies.LibraryInteropServices.Value;
            var result = asm?.GetType("System.Runtime.InteropServices.DllImportResolver");
            return result;
        });
    }
}