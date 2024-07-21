using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to create sound player
    /// and to play system sounds.
    /// </summary>
    public interface ISoundFactoryHandler : IDisposable
    {
        /// <summary>
        /// Stops sound playing.
        /// </summary>
        void StopSound();

        /// <summary>
        /// Plays system sound.
        /// </summary>
        /// <param name="soundType"></param>
        void MessageBeep(SystemSoundType soundType);

        /// <summary>
        /// Plays bell.
        /// </summary>
        void Bell();

        /// <summary>
        /// Creates <see cref="ISoundPlayerHandler"/> interface provider.
        /// </summary>
        /// <param name="fileName">Path to file with sound.</param>
        /// <returns></returns>
        ISoundPlayerHandler CreateSoundPlayerHandler(string fileName);
    }
}
