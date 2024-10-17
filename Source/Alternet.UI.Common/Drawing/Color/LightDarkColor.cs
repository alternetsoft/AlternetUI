using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Etxtends <see cref="Color"/> with additional features.
    /// Implements light and dark color pair, one of them
    /// is used when argb of the <see cref="LightDarkColor"/> is requested. Returned argb
    /// depends on the value specified in <see cref="DefaultIsDark"/> and a result
    /// of <see cref="SystemSettings.AppearanceIsDark"/>.
    /// </summary>
    public class LightDarkColor : Color
    {
        /// <summary>
        /// Gets or sets whether dark or light color is returned
        /// when argb of the color is requested. When this is null (default),
        /// <see cref="SystemSettings.AppearanceIsDark"/> is used in order to determine
        /// which argb to return.
        /// </summary>
        public static bool? DefaultIsDark;

        private readonly Color light;
        private readonly Color dark;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
        {
            this.light = light;
            this.dark = dark;
        }

        /// <summary>
        /// Gets dark color.
        /// </summary>
        public Color Dark => dark;

        /// <summary>
        /// Gets light color.
        /// </summary>
        public Color Light => light;

        /// <inheritdoc/>
        protected override void RequireArgb(ref ColorStruct val)
        {
            var appearanceIsDark = DefaultIsDark ?? SystemSettings.AppearanceIsDark;

            if (appearanceIsDark)
                val = dark.AsStruct;
            else
                val = light.AsStruct;
        }
    }
}
