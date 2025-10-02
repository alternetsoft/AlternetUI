using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a cached collection of <see cref="System.Drawing.Color"/>
    /// instances corresponding to all <see cref="System.Drawing.KnownColor"/> values.
    /// </summary>
    /// <remarks>This class improves performance by precomputing and caching all
    /// <see cref="System.Drawing.Color"/> instances derived
    /// from <see cref="System.Drawing.KnownColor"/> values. Use the
    /// <see cref="Get"/>  method to retrieve
    /// a cached <see cref="System.Drawing.Color"/> instance.</remarks>
    public static class SystemDrawingColorCache
    {
        private static readonly System.Drawing.Color[] colors;

        static SystemDrawingColorCache()
        {
            try
            {
                var values = Enum.GetValues(typeof(System.Drawing.KnownColor));
                colors = new System.Drawing.Color[values.Length + 1];

                foreach (System.Drawing.KnownColor kc in values)
                {
                    colors[(int)kc] = System.Drawing.Color.FromKnownColor(kc);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Error] {ex}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the <see cref="System.Drawing.Color"/> associated with
        /// the specified <see cref="System.Drawing.KnownColor"/>.
        /// This method uses a cached collection of colors for improved performance.
        /// </summary>
        /// <param name="kc">A value from the <see cref="System.Drawing.KnownColor"/>
        /// enumeration representing the color to retrieve.</param>
        /// <returns>The <see cref="System.Drawing.Color"/> corresponding
        /// to the specified <paramref name="kc"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Color Get(System.Drawing.KnownColor kc)
        {
            return colors[(int)kc];
        }
    }
}