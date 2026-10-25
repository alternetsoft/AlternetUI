using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="ISoundPlayerHandler"/> interface for MAUI platform.
    /// </summary>
    public partial class MauiSoundPlayerHandler : DisposableObject, ISoundPlayerHandler
    {
        /// <summary>
        /// Gets or sets a function that creates an instance of <see cref="MauiSoundPlayerHandler"/>.
        /// </summary>
        public static Func<string, MauiSoundPlayerHandler> CreateSoundPlayerHandler { get; set; }
            = (fileName) => new MauiSoundPlayerHandler(fileName);

        /// <summary>
        /// Occurs when a sound is played.
        /// </summary>
        public static event EventHandler<PlaySoundEventArgs>? PlaySound;

        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiSoundPlayerHandler"/> class.
        /// </summary>
        /// <param name="fileName">The name of the sound file to play.</param>
        public MauiSoundPlayerHandler(string fileName)
        {
            this.fileName = fileName;
        }

        /// <inheritdoc/>
        public virtual bool IsOk => true;

        /// <inheritdoc/>
        public virtual bool Play(SoundPlayFlags flags = SoundPlayFlags.Asynchronous)
        {
            PlaySound?.Invoke(this, new PlaySoundEventArgs(fileName, flags));
            return true;
        }

        /// <inheritdoc/>
        public virtual void Stop()
        {
        }
    }
}
