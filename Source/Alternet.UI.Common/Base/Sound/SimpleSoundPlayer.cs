using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Controls playback of a sound from a audio file. This player supports only wav files.
    /// On Linux requires package osspd.
    /// </summary>
    public class SimpleSoundPlayer : HandledObject<ISoundPlayerHandler>
    {
        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSoundPlayer"/> class.
        /// </summary>
        /// <param name="fileName">Path to audio file.</param>
        public SimpleSoundPlayer(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Gets filename of the audio file.
        /// </summary>
        public virtual string FileName => fileName;

        /// <summary>
        /// Returns <c>true</c> if the object contains a successfully loaded file or
        /// resource, <c>false</c> otherwise.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                return Handler.IsOk;
            }
        }

        /// <summary>
        /// Stops playing sounds.
        /// </summary>
        public virtual void Stop()
        {
            SoundUtils.Handler.StopSound();
        }

        /// <summary>
        /// Plays the sound file.
        /// </summary>
        /// <param name="flags">Sound play flags.</param>
        /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If another sound is playing, it will be interrupted.
        /// Note that in general it
        /// is possible to delete the object which is being asynchronously played
        /// any time after calling this function and the sound would continue playing,
        /// however this currently doesn't work under Windows for sound objects loaded
        /// from memory data.
        /// </remarks>
        public virtual bool Play(SoundPlayFlags flags = SoundPlayFlags.Asynchronous)
        {
            return Handler.Play(flags);
        }

        /// <inheritdoc/>
        protected override ISoundPlayerHandler CreateHandler()
        {
            return SoundUtils.Handler.CreateSoundPlayerHandler(fileName);
        }
    }
}
