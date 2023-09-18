using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods for log handling.
    /// </summary>
    public static class LogUtils
    {
        private static int id;

        /// <summary>
        /// Gets unique id for debug purposes.
        /// </summary>
        public static int GenNewId()
        {
            return id++;
        }
    }
}
