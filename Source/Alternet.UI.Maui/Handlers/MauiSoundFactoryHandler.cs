using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="ISoundFactoryHandler"/> interface for MAUI platform.
    /// </summary>
    public partial class MauiSoundFactoryHandler : DisposableObject, ISoundFactoryHandler
    {
        /// <summary>
        /// Gets or sets a function that creates an instance of <see cref="MauiSoundFactoryHandler"/>.
        /// </summary>
        public static Func<MauiSoundFactoryHandler> CreateSoundFactoryHandler { get; set; } = () => new MauiSoundFactoryHandler();

        /// <inheritdoc/>
        public virtual void Bell()
        {
            MessageBeep(SystemSoundType.Beep);
        }

        /// <inheritdoc/>
        public virtual ISoundPlayerHandler CreateSoundPlayerHandler(string fileName)
        {
            return MauiSoundPlayerHandler.CreateSoundPlayerHandler(fileName);
        }

        /// <inheritdoc/>
        public virtual void MessageBeep(SystemSoundType soundType)
        {
        }

        /// <inheritdoc/>
        public virtual void StopSound()
        {
        }
    }
}
