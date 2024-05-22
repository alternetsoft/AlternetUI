using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a value that determines the Input Method Editor (IME) status of an
    /// object when the object is selected.
    /// </summary>
    public enum ImeMode
    {
        /// <summary>
        /// Inherits the IME mode of the parent control.
        /// </summary>
        Inherit = -1,

        /// <summary>
        /// None (Default).
        /// </summary>
        NoControl,

        /// <summary>
        /// The IME is on. This value indicates that the IME is on and characters
        /// specific to Chinese or Japanese can be entered. This setting is valid
        /// for Japanese, Simplified Chinese, and Traditional Chinese IME only.
        /// </summary>
        On,

        /// <summary>
        /// The IME is off. This mode indicates that the IME is off, meaning that the
        /// object behaves the same as English entry mode. This setting is valid
        /// for Japanese, Simplified Chinese, and Traditional Chinese IME only.
        /// </summary>
        Off,

        /// <summary>
        /// The IME is disabled. With this setting, the users cannot turn
        /// the IME on from the keyboard, and the IME floating window is hidden.
        /// </summary>
        Disable,

        /// <summary>
        /// Hiragana DBC. This setting is valid for the Japanese IME only.
        /// </summary>
        Hiragana,

        /// <summary>
        /// Katakana DBC. This setting is valid for the Japanese IME only.
        /// </summary>
        Katakana,

        /// <summary>
        /// Katakana SBC. This setting is valid for the Japanese IME only.
        /// </summary>
        KatakanaHalf,

        /// <summary>
        /// Alphanumeric double-byte characters. This setting is valid
        /// for Korean and Japanese IME only.
        /// </summary>
        AlphaFull,

        /// <summary>
        /// Alphanumeric single-byte characters(SBC). This setting is valid
        /// for Korean and Japanese IME only.
        /// </summary>
        Alpha,

        /// <summary>
        /// Hangul DBC. This setting is valid for the Korean IME only.
        /// </summary>
        HangulFull,

        /// <summary>
        /// Hangul SBC. This setting is valid for the Korean IME only.
        /// </summary>
        Hangul,

        /// <summary>
        /// IME closed. This setting is valid for Chinese IME only.
        /// </summary>
        Close,

        /// <summary>
        /// IME on HalfShape. This setting is valid for Chinese IME only.
        /// </summary>
        OnHalf,
    }
}
