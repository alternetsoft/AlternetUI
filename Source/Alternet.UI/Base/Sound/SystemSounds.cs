﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Retrieves sounds associated with a set of operating system sound-event types.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class SystemSounds
    {
        private static volatile SystemSound? asterisk;
        private static volatile SystemSound? beep;
        private static volatile SystemSound? exclamation;
        private static volatile SystemSound? hand;
        private static volatile SystemSound? question;

        private SystemSounds()
        {
        }

        /// <summary>
        /// Gets the sound associated with the <see langword="Asterisk" />
        /// program event in the current sound scheme.</summary>
        /// <returns>A <see cref="SystemSound" /> associated with
        /// the <see langword="Asterisk" /> program event in the current sound scheme.</returns>
        public static SystemSound Asterisk
        {
            get
            {
                asterisk ??= new SystemSound(SystemSound.SoundType.Asterisk);
                return asterisk;
            }

            set => asterisk = value;
        }

        /// <summary>
        /// Gets the sound associated with the <see langword="Beep" /> program
        /// event in the current sound scheme.</summary>
        /// <returns>A <see cref="SystemSound" /> associated with
        /// the <see langword="Beep" /> program event in the current sound scheme.</returns>
        public static SystemSound Beep
        {
            get
            {
                beep ??= new SystemSound(SystemSound.SoundType.Beep);
                return beep;
            }

            set => beep = value;
        }

        /// <summary>
        /// Gets the sound associated with the <see langword="Exclamation" /> program
        /// event in the current sound scheme.</summary>
        /// <returns>A <see cref="SystemSound" /> associated with
        /// the <see langword="Exclamation" /> program event in the current sound scheme.</returns>
        public static SystemSound Exclamation
        {
            get
            {
                exclamation ??= new SystemSound(SystemSound.SoundType.Exclamation);
                return exclamation;
            }

            set => exclamation = value;
        }

        /// <summary>Gets the sound associated with the <see langword="Hand" />
        /// program event in the current Windows sound scheme.</summary>
        /// <returns>A <see cref="SystemSound" /> associated with the
        /// <see langword="Hand" /> program event in the current Windows sound scheme.</returns>
        public static SystemSound Hand
        {
            get
            {
                hand ??= new SystemSound(SystemSound.SoundType.Hand);
                return hand;
            }
        }

        /// <summary>
        /// Gets the sound associated with the <see langword="Question" /> program
        /// event in the current sound scheme.</summary>
        /// <returns>A <see cref="SystemSound" /> associated with the
        /// <see langword="Question" /> program event in the current sound scheme.</returns>
        public static SystemSound Question
        {
            get
            {
                question ??= new SystemSound(SystemSound.SoundType.Question);
                return question;
            }

            set => question = value;
        }
     }
}