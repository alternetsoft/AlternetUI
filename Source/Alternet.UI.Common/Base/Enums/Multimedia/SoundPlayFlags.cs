using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines flags for the sound play methods.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum SoundPlayFlags : uint
    {
        /// <summary>
        /// Play will block and wait until the sound is replayed.
        /// </summary>
        Synchronous = 0,

        /// <summary>
        /// Sound is played asynchronously, 'Play' method returns immediately.
        /// </summary>
        Asynchronous = 1,

        /// <summary>
        /// Sound is played asynchronously and loops until another sound is played,
        /// 'Stop' is called or the program terminates.
        /// </summary>
        AsynchronousLoop = 2 | Asynchronous,
    }
}
