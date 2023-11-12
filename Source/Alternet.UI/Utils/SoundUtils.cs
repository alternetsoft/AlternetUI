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
        /// Ring the system bell.
        /// </summary>
        /// <remarks>
        /// This function is categorized as a GUI one and so is not thread-safe.
        /// </remarks>
        public static void Bell() => Native.WxOtherFactory.Bell();
    }
}
