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
        private SoundType soundType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSound"/> class.
        /// </summary>
        public SystemSound()
        {
        }

        public SystemSound(SoundType soundType)
        {
            this.soundType = soundType;
        }

        /// <summary>
        /// Occurs when <see cref="Play"/> method is called.
        /// </summary>
        public event EventHandler<HandledEventArgs>? PlaySound;

        /// <summary>
        /// Enumerates known system sounds.
        /// </summary>
        public enum SoundType
        {
            /// <summary>
            /// 
            /// </summary>
            Beep = 0,

            Hand = 16,

            Question = 32,

            Exclamation = 48,

            Asterisk = 64,
        }

        /// <summary>
        /// Gets or sets whether sound is actually played.
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
