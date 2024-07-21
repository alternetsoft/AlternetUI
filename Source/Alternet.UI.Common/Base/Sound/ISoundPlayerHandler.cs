using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to control sound player.
    /// </summary>
    public interface ISoundPlayerHandler : IDisposable
    {
        /// <summary>
        /// Gets whether this object is ok.
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// Stops playing sound.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts playing sound using the specified flags.
        /// </summary>
        /// <param name="flags">Flags.</param>
        /// <returns></returns>
        bool Play(SoundPlayFlags flags = SoundPlayFlags.Asynchronous);
    }
}
