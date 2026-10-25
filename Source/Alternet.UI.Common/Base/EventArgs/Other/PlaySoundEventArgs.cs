using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the play sound events.
    /// </summary>
    public class PlaySoundEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaySoundEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">The name of the sound file to play.</param>
        /// <param name="flags">The flags that specify how the sound should be played.</param>
        public PlaySoundEventArgs(string fileName, SoundPlayFlags flags)
        {
            FileName = fileName;
            Flags = flags;
        }

        /// <summary>
        /// Gets or sets the name of the sound file to play.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the flags that specify how the sound should be played.
        /// </summary>
        public SoundPlayFlags Flags { get; set; }
    }
}
