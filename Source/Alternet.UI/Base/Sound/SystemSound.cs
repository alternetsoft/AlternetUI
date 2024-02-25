using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a system sound type.
    /// </summary>
    public class SystemSound
    {
        private readonly SystemSoundType soundType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSound"/> class.
        /// </summary>
        public SystemSound()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSound"/> class.
        /// </summary>
        /// <param name="soundType">Type of the system sound.</param>
        public SystemSound(SystemSoundType soundType)
        {
            this.soundType = soundType;
        }

        /// <summary>
        /// Occurs when <see cref="Play"/> method is called.
        /// </summary>
        public static event EventHandler<HandledEventArgs>? PlaySound;

        /// <summary>
        /// Gets type of the system sound.
        /// </summary>
        public SystemSoundType SoundType => soundType;

        /// <summary>
        /// Gets or sets whether sound is actually played. Default is <c>false</c> (sound is played).
        /// </summary>
        public bool IsSilent { get; set; }

        /// <summary>
        /// Plays the system sound type.
        /// </summary>
        public virtual void Play()
        {
            if (IsSilent)
                return;

            if(PlaySound is not null)
            {
                HandledEventArgs e = new();
                PlaySound(this, e);
                if (e.Handled)
                    return;
            }

            if(Application.IsWindowsOS)
                SafeNativeMethods.MessageBeep((int)soundType);
            else
                Native.WxOtherFactory.Bell();
        }

        private class SafeNativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool MessageBeep(int type);
        }
    }
}
