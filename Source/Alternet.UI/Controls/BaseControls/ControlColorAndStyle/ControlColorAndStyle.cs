using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines color and style settings for dark and light themse.
    /// </summary>
    public class ControlColorAndStyle
    {
        private ControlStateSettings? dark;
        private ControlStateSettings? light;

        /// <summary>
        /// Gets color and style settings for the dark theme.
        /// </summary>
        public virtual ControlStateSettings Dark
        {
            get
            {
                return dark ??= new();
            }

            set
            {
                dark = value;
            }
        }

        /// <summary>
        /// Gets color and style settings for the light theme.
        /// </summary>
        public virtual ControlStateSettings Light
        {
            get
            {
                return light ??= new();
            }

            set
            {
                light = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Dark"/> if <paramref name="isDark"/> is <c>true</c>
        /// and dark theme settings were defined;
        /// <see cref="Light"/> otherwise.
        /// </summary>
        /// <param name="isDark">Whether to return <see cref="Dark"/> or
        /// <see cref="Light"/>.</param>
        /// <returns></returns>
        public virtual ControlStateSettings DarkOrLight(bool isDark)
        {
            if (isDark && dark is not null)
                return dark;
            else
                return Light;
        }
    }
}
