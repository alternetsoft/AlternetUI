using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements extension methods for <see cref="Key"/> and <see cref="Keys"/>.
    /// </summary>
    public static class KeysExtensions
    {
        /// <summary>
        /// Array with <see cref="Keys"/> to <see cref="Key"/> conversion data.
        /// </summary>
        public static Key[] KeysToKeyConversion = new[]
        {
            Key.None,
            Key.None/*Keys.LButton*/,
            Key.None/*Keys.RButton*/,
            Key.None/*Keys.Cancel*/,
            Key.None/*Keys.MButton*/,
            Key.None/*Keys.XButton1*/,
            Key.None/*Keys.XButton2*/,
            Key.Back,
            Key.Tab,
            Key.None/*Keys.LineFeed*/,
            Key.Clear,
            Key.Return,
            Key.Enter,
            Key.Shift,
            Key.Control,
            Key.Menu,
            Key.Pause,
            Key.None/*Keys.Capital*/,
            // The CAPS LOCK key.
            Key.CapsLock,
            // The IME Kana mode key.
            Key.None/*Keys.KanaMode*/,
            // The IME Hanguel mode key. (maintained for compatibility; use <see langword="HangulMode" />)
            Key.None/*Keys.HanguelMode*/,
            // The IME Hangul mode key.
            Key.None/*Keys.HangulMode*/,
            // The IME Junja mode key.
            Key.None/*Keys.JunjaMode*/,
            // The IME final mode key.
            Key.None/*Keys.FinalMode*/,
            // The IME Hanja mode key.
            Key.None/*Keys.HanjaMode*/,
            // The IME Kanji mode key.
            Key.None/*Keys.KanjiMode*/,
            // The ESC key.
            Key.Escape,
            // The IME convert key.
            Key.None/*Keys.IMEConvert*/,
            // The IME nonconvert key.
            Key.None/*Keys.IMENonconvert*/,
            // The IME accept key, replaces <see cref="System.Windows.Forms.Keys.IMEAceept" />.
            Key.None/*Keys.IMEAccept*/,
            // The IME accept key. Obsolete, use <see cref="F:System.Windows.Forms.Keys.IMEAccept" /> instead.
            Key.None/*Keys.IMEAceept*/,
            // The IME mode change key.
            Key.None/*Keys.IMEModeChange*/,
            // The SPACEBAR key.
            Key.Space,
            // The PAGE UP key.
            Key.Prior,
            // The PAGE UP key.
            Key.PageUp,
            // The PAGE DOWN key.
            Key.Next,
            // The PAGE DOWN key.
            Key.PageDown,
            // The END key.
            Key.End,
            // The HOME key.
            Key.Home,
            // The LEFT ARROW key.
            Key.Left,
            // The UP ARROW key.
            Key.Up,
            // The RIGHT ARROW key.
            Key.Right,
            // The DOWN ARROW key.
            Key.Down,
            // The SELECT key.
            Key.None/*Keys.Select*/,
            // The PRINT key.
            Key.None/*Keys.Print*/,
            // The EXECUTE key.
            Key.None/*Keys.Execute*/,
            // The PRINT SCREEN key.
            Key.PrintScreen/*Keys.Snapshot*/,
            // The PRINT SCREEN key.
            Key.PrintScreen,
            // The INS key.
            Key.Insert,
            // The DEL key.
            Key.Delete,
            // The HELP key.
            Key.None/*Keys.Help*/,
            // The 0 key.
            Key.D0,
            // The 1 key.
            Key.D1,
            // The 2 key.
            Key.D2,
            // The 3 key.
            Key.D3,
            // The 4 key.
            Key.D4,
            // The 5 key.
            Key.D5,
            // The 6 key.
            Key.D6,
            // The 7 key.
            Key.D7,
            // The 8 key.
            Key.D8,
            // The 9 key.
            Key.D9,
            // The A key.
            Key.A,
            // The B key.
            Key.B,
            // The C key.
            Key.C,
            // The D key.
            Key.D,
            // The E key.
            Key.E,
            // The F key.
            Key.F,
            // The G key.
            Key.G,
            // The H key.
            Key.H,
            // The I key.
            Key.I,
            // The J key.
            Key.J,
            // The K key.
            Key.K,
            // The L key.
            Key.L,
            // The M key.
            Key.M,
            // The N key.
            Key.N,
            // The O key.
            Key.O,
            // The P key.
            Key.P,
            // The Q key.
            Key.Q,
            // The R key.
            Key.R,
            // The S key.
            Key.S,
            // The T key.
            Key.T,
            // The U key.
            Key.U,
            // The V key.
            Key.V,
            // The W key.
            Key.W,
            // The X key.
            Key.X,
            // The Y key.
            Key.Y,
            // The Z key.
            Key.Z,
            // The left Windows logo key (Microsoft Natural Keyboard).
            Key.Windows/*Keys.LWin*/,
            // The right Windows logo key (Microsoft Natural Keyboard).
            Key.Windows/*Key.RWin*/,
            // The application key (Microsoft Natural Keyboard).
            Key.None/*Keys.Apps*/,
            // The computer sleep key.
            Key.None/*Keys.Sleep*/,
            // The 0 key on the numeric keypad.
            Key.NumPad0,
            // The 1 key on the numeric keypad.
            Key.NumPad1,
            // The 2 key on the numeric keypad.
            Key.NumPad2,
            // The 3 key on the numeric keypad.
            Key.NumPad3,
            // The 4 key on the numeric keypad.
            Key.NumPad4,
            // The 5 key on the numeric keypad.
            Key.NumPad5,
            // The 6 key on the numeric keypad.
            Key.NumPad6,
            // The 7 key on the numeric keypad.
            Key.NumPad7,
            // The 8 key on the numeric keypad.
            Key.NumPad8,
            // The 9 key on the numeric keypad.
            Key.NumPad9,
            // The multiply key.
            Key.Asterisk/*Keys.Multiply*/,
            // The add key.
            Key.PlusSign/*Keys.Add*/,
            // The separator key.
            Key.None/*Keys.Separator*/,
            // The subtract key.
            Key.Minus/*Keys.Subtract*/,
            // The decimal key.
            Key.None/*Keys.Decimal*/,
            // The divide key.
            Key.Slash/*Key.Divide*/,
            // The F1 key.
            Key.F1,
            // The F2 key.
            Key.F2,
            // The F3 key.
            Key.F3,
            // The F4 key.
            Key.F4,
            // The F5 key.
            Key.F5,
            // The F6 key.
            Key.F6,
            // The F7 key.
            Key.F7,
            // The F8 key.
            Key.F8,
            // The F9 key.
            Key.F9,
            // The F10 key.
            Key.F10,
            // The F11 key.
            Key.F11,
            // The F12 key.
            Key.F12,
            // The F13 key.
            Key.F13,
            // The F14 key.
            Key.F14,
            // The F15 key.
            Key.F15,
            // The F16 key.
            Key.F16,
            // The F17 key.
            Key.F17,
            // The F18 key.
            Key.F18,
            // The F19 key.
            Key.F19,
            // The F20 key.
            Key.F20,
            // The F21 key.
            Key.F21,
            // The F22 key.
            Key.F22,
            // The F23 key.
            Key.F23,
            // The F24 key.
            Key.F24,
            // The NUM LOCK key.
            Key.NumLock,
            // The SCROLL LOCK key.
            Key.ScrollLock,
            // The left SHIFT key.
            Key.Shift/*Keys.LShiftKey*/,
            // The right SHIFT key.
            Key.Shift/*RShiftKey*/,
            // The left CTRL key.
            Key.Control/*LControlKey*/,
            // The right CTRL key.
            Key.Control/*RControlKey*/,
            // The left ALT key.
            Key.Menu/*LMenu*/,
            // The right ALT key.
            Key.Menu/*RMenu*/,
            // The browser back key (Windows 2000 or later).
            Key.BrowserBack,
            // The browser forward key (Windows 2000 or later).
            Key.BrowserForward,
            // The browser refresh key (Windows 2000 or later).
            Key.BrowserRefresh,
            // The browser stop key (Windows 2000 or later).
            Key.BrowserStop,
            // The browser search key (Windows 2000 or later).
            Key.BrowserSearch,
            // The browser favorites key (Windows 2000 or later).
            Key.BrowserFavorites,
            // The browser home key (Windows 2000 or later).
            Key.BrowserHome,
            // The volume mute key (Windows 2000 or later).
            Key.VolumeMute,
            // The volume down key (Windows 2000 or later).
            Key.VolumeDown,
            // The volume up key (Windows 2000 or later).
            Key.VolumeUp,
            // The media next track key (Windows 2000 or later).
            Key.MediaNextTrack,
            // The media previous track key (Windows 2000 or later).
            Key.MediaPreviousTrack,
            // The media Stop key (Windows 2000 or later).
            Key.MediaStop,
            // The media play pause key (Windows 2000 or later).
            Key.MediaPlayPause,
            // The launch mail key (Windows 2000 or later).
            Key.LaunchMail,
            // The select media key (Windows 2000 or later).
            Key.SelectMedia,
            // The start application one key (Windows 2000 or later).
            Key.LaunchApplication1,
            // The start application two key (Windows 2000 or later).
            Key.LaunchApplication2,
            // The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
            Key.Semicolon/*Keys.OemSemicolon*/,
            // The OEM 1 key.
            Key.None/*Keys.Oem1*/,
            // The OEM plus key on any country/region keyboard (Windows 2000 or later).
            Key.PlusSign/*Key.Oemplus*/,
            // The OEM comma key on any country/region keyboard (Windows 2000 or later).
            Key.Comma/*Key.Oemcomma*/,
            // The OEM minus key on any country/region keyboard (Windows 2000 or later).
            Key.Minus/*Key.OemMinus*/,
            // The OEM period key on any country/region keyboard (Windows 2000 or later).
            Key.OemPeriod,
            // The OEM question mark key on a US standard keyboard (Windows 2000 or later).
            Key.QuestionMark/*Key.OemQuestion*/,
            // The OEM 2 key.
            Key.None/*Key.Oem2*/,
            // The OEM tilde key on a US standard keyboard (Windows 2000 or later).
            Key.Tilde/*Key.Oemtilde*/,
            // The OEM 3 key.
            Key.None/*Key.Oem3*/,
            // The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
            Key.OpenBracket/*Keys.OemOpenBrackets*/,
            // The OEM 4 key.
            Key.None/*Key.Oem4*/,
            // The OEM pipe key on a US standard keyboard (Windows 2000 or later).
            Key.OemPipe,
            // The OEM 5 key.
            Key.None/*Key.Oem5*/,
            // The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
            Key.CloseBracket/*Key.OemCloseBrackets*/,
            // The OEM 6 key.
            Key.None/*Key.Oem6*/,
            // The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
            Key.Quote/*Keys.OemQuotes*/,
            // The OEM 7 key.
            Key.None/*Key.Oem7*/,
            // The OEM 8 key.
            Key.None/*Key.Oem8*/,
            // The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000 or later).
            Key.Backslash/*Key.OemBackslash*/,
            // The OEM 102 key.
            Key.None/*Keys.Oem102*/,
            // The PROCESS KEY key.
            Key.None/*Key.ProcessKey*/,
            // Used to pass Unicode characters as if they were keystrokes. The Packet key value is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
            Key.None/*Key.Packet*/,
            // The ATTN key.
            Key.None/*Key.Attn*/,
            // The CRSEL key.
            Key.None/*Key.Crsel*/,
            // The EXSEL key.
            Key.None/*Key.Exsel*/,
            // The ERASE EOF key.
            Key.None/*Key.EraseEof*/,
            // The PLAY key.
            Key.None/*Key.Play*/,
            // The ZOOM key.
            Key.None/*Key.Zoom*/,
            // A constant reserved for future use.
            Key.None/*Key.NoName*/,
            // The PA1 key.
            Key.None/*Key.Pa1*/,
            // The CLEAR key.
            Key.None/*Key.OemClear*/,
        };

        /// <summary>
        /// Converts <see cref="Keys"/> to <see cref="Key"/>.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Key ToKey(this Keys keys)
        {
            var ikey = GetKeyValue(keys);
            if (ikey >= 0 && ikey <= (int)Keys.OemClear)
                return KeysToKeyConversion[ikey];
            else
                return Key.None;
        }

        /// <summary>Gets a value indicating whether the ALT key was pressed.</summary>
        /// <returns>
        /// <see langword="true" /> if the ALT key was pressed;
		/// otherwise, <see langword="false" />.</returns>
        public static bool IsAlt(Keys keys) => (keys & Keys.Alt) == Keys.Alt;

        /// <summary>Gets a value indicating whether the CTRL key was pressed.</summary>
        /// <returns>
        /// <see langword="true" /> if the CTRL key was pressed;
		/// otherwise, <see langword="false" />.</returns>
        public static bool IsControl(Keys keys) => (keys & Keys.Control) == Keys.Control;

        /// <summary>Gets a value indicating whether the SHIFT key was pressed.</summary>
        /// <returns>
        /// <see langword="true" /> if the SHIFT key was pressed;
        /// otherwise, <see langword="false" />.</returns>
        public static bool IsShift(Keys keys) => (keys & Keys.Shift) == Keys.Shift;

        /// <summary>Gets the keyboard code from <see cref="Keys"/>.</summary>
        /// <returns>A <see cref="Keys" /> value that is the key code for the event.</returns>
        /// <remarks>
        /// This function removes key modifiers (CTRL, SHIFT, and ALT) from the <see cref="Keys"/>
        /// and returns only key codes without modifiers.
        /// </remarks>
        public static Keys GetKeyCode(Keys keys)
        {
            Keys result = keys & Keys.KeyCode;
            if (!Enum.IsDefined(typeof(Keys), (int)result))
                return Keys.None;
            return result;
        }

        /// <summary>Gets the modifier flags from <see cref="Keys"/>. The flags indicate
        /// which combination
        /// of CTRL, SHIFT, and ALT keys was pressed.</summary>
        /// <returns>A <see cref="Keys" /> value representing one or more modifier flags.</returns>
        public static Keys GetModifiers(Keys keys) => keys & Keys.Modifiers;

        /// <summary>Gets the keyboard value from <see cref="Keys"/>.</summary>
        /// <returns>The integer representation of the keyboard key without modifiers.</returns>
        public static int GetKeyValue(Keys keys) => (int)(keys & Keys.KeyCode);
    }
}