using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// A simple class which stores hue, saturation and value as doubles in the range 0.0-1.0.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HSVValue
    {
        /// <summary>
        /// Hue component of a color.
        /// </summary>
        public double Hue;

        /// <summary>
        /// Saturation component of a color.
        /// </summary>
        public double Saturation;

        /// <summary>
        /// Value component of a color.
        /// </summary>
        public double Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="HSVValue"/>.
        /// </summary>
        public HSVValue(double h = 0.0, double s = 0.0, double v = 0.0)
        {
            this.Hue = h;
            this.Saturation = s;
            this.Value = v;
        }
    }
}