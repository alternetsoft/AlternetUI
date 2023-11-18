using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumeration flags with supported operating systems.
    /// </summary>
    [Flags]
    public enum OperatingSystems
    {
        /// <summary>
        /// No operating systems are specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Operating system is Windows.
        /// </summary>
        Windows = 1,

        /// <summary>
        /// Operating system is Linux.
        /// </summary>
        Linux = 2,

        /// <summary>
        /// Operating system is MacOs.
        /// </summary>
        MacOs = 4,

        /// <summary>
        /// Operating system is Android.
        /// </summary>
        Android = 8,

        /// <summary>
        /// Operating system is IOS.
        /// </summary>
        IOS = 16,

        /// <summary>
        /// Any operating system.
        /// </summary>
        Any = MacOs | Linux | Windows | Android | IOS,

        /// <summary>
        /// Operating system is Windows or Linux.
        /// </summary>
        WindowsOrLinux = Linux | Windows,
    }
}
