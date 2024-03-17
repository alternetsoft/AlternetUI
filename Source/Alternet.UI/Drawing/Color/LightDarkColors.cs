using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static members related to <see cref="LightDarkColor"/>.
    /// </summary>
    public static class LightDarkColors
    {
        private static LightDarkColor? red;
        private static LightDarkColor? green;
        private static LightDarkColor? blue;

        /// <summary>
        /// Gets default red colors pair.
        /// </summary>
        public static LightDarkColor Red
        {
            get
            {
                return red ??= new(light: (192, 10, 22), dark: (244, 75, 86));
            }

            set => red = value;
        }

        /// <summary>
        /// Gets default green colors pair.
        /// </summary>
        public static LightDarkColor Green
        {
            get
            {
                return green ??= new(light: (30, 124, 30), dark: (138, 226, 138));
            }

            set => green = value;
        }

        /// <summary>
        /// Gets default blue colors pair.
        /// </summary>
        public static LightDarkColor Blue
        {
            get
            {
                return blue ??= new(light: (0, 90, 181), dark: (85, 170, 255));
            }

            set => blue = value;
        }
    }
}