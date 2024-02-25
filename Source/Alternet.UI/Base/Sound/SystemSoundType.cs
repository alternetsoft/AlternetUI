using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known system sounds.
    /// </summary>
    public enum SystemSoundType
    {
        /// <summary>
        /// The sound specified as the 'Beep' sound.
        /// </summary>
        Beep = 0,

        /// <summary>
        /// The sound specified as the 'Hand' sound.
        /// </summary>
        Hand = 16,

        /// <summary>
        /// The sound specified as the 'Question' sound.
        /// </summary>
        Question = 32,

        /// <summary>
        /// The sound specified as the 'Exclamation' sound.
        /// </summary>
        Exclamation = 48,

        /// <summary>
        /// The sound specified as the 'Asterisk' sound.
        /// </summary>
        Asterisk = 64,
    }
}
