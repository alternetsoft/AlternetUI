using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements light and dark color pair.
    /// </summary>
    public class LightDarkColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
        {
            Light = light;
            Dark = dark;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="color">Light and dark color.</param>
        public LightDarkColor(Color color)
        {
            Light = color;
            Dark = color;
        }

        /// <summary>
        /// Gets light color.
        /// </summary>
        public virtual Color Light { get; set; }

        /// <summary>
        /// Gets dark color.
        /// </summary>
        public virtual Color Dark { get; set; }

        /// <summary>
        /// Gets dark or light color depending on the <paramref name="isDark"/>
        /// parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light color.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Get(bool isDark) => isDark ? Dark : Light;
    }
}
