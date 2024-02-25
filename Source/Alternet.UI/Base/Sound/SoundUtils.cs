using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods for playing sounds.
    /// </summary>
    public static class SoundUtils
    {
        /// <summary>
        /// Gets or sets whether <see cref="Bell"/> method is supressed.
        /// Default is <c>false</c>.
        /// </summary>
        public static bool SupressBell
        {
            get => SystemSounds.Beep.IsSilent;
            set => SystemSounds.Beep.IsSilent = value;
        }

        /// <summary>
        /// Stops playing sounds.
        /// </summary>
        public static void StopSound()
        {
            Native.WxOtherFactory.SoundStop();
        }

        /// <summary>
        /// Ring the system bell.
        /// </summary>
        /// <remarks>
        /// This function is categorized as a GUI one and so is not thread-safe.
        /// </remarks>
        public static void Bell()
        {
            SystemSounds.Beep.Play();
        }
    }
}
