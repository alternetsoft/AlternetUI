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
    /// Extends <see cref="Color"/> with additional features.
    /// Implements light and dark color pair, one of them
    /// is used when argb of the <see cref="LightDarkColor"/> is requested. Returned argb
    /// depends on the value specified in <see cref="IsDarkOverride"/> and a result
    /// of <see cref="SystemSettings.IsUsingDarkBackground"/>.
    /// </summary>
    public class LightDarkColor : Color
    {
        /// <summary>
        /// Gets or sets whether dark or light color is returned
        /// when argb of the color is requested. When this is null (default),
        /// <see cref="SystemSettings.AppearanceIsDark"/> is used in order to determine
        /// which argb to return.
        /// </summary>
        public static bool? IsDarkOverride;

        private readonly Color light;
        private readonly Color dark;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
            : base(0, StateFlags.ValueValid)
        {
            this.light = light;
            this.dark = dark;
        }

        /// <summary>
        /// Gets whether dark or light color is returned.
        /// </summary>
        public static bool IsUsingDarkColor
        {
            get
            {
                return IsDarkOverride ?? SystemSettings.IsUsingDarkBackground;
            }
        }

        /// <summary>
        /// Gets dark color.
        /// </summary>
        public Color Dark => dark;

        /// <summary>
        /// Gets light color.
        /// </summary>
        public Color Light => light;

        /// <summary>
        /// Calls the specified action inside the block which temporary changes
        /// value of the <see cref="IsDarkOverride"/> property.
        /// </summary>
        /// <param name="tempIsDarkOverride">Temporary value for the
        /// <see cref="IsDarkOverride"/> property.</param>
        /// <param name="action">Action to call.</param>
        public static void DoInsideTempIsDarkOverride(bool? tempIsDarkOverride, Action? action)
        {
            var savedOverride = IsDarkOverride;
            try
            {
                IsDarkOverride = tempIsDarkOverride;
                action?.Invoke();
            }
            finally
            {
                IsDarkOverride = savedOverride;
            }
        }

        /// <inheritdoc/>
        protected override void RequireArgb(ref ColorStruct val)
        {
            if (IsUsingDarkColor)
                val = dark.AsStruct;
            else
                val = light.AsStruct;
        }
    }
}
