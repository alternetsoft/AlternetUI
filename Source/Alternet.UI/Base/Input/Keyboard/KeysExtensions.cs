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
        /// Array with <see cref="Key"/> to <see cref="Keys"/> conversion data.
        /// </summary>
        public static Keys[] KeyToKeysConversion = new[]
        {
            //  No key pressed.
            Keys.None,

            //  The "Backspace" key.
            Keys.Back,
        
            //  The "Tab" key.
            Keys.Tab,
        
            //  The "Enter" key.
            Keys.Enter,
        
            //  The "Pause" key.
            Keys.Pause,
        
            //  The "Caps Lock" key.
            Keys.CapsLock,

            //  The "Esc" key.
            Keys.Escape,
        
            //  The "Space Bar" key.
            Keys.Space,
        
            //  The "Page Up" key.
            Keys.PageUp,
        
            //  The "Page Down" key.
            Keys.PageDown,
        
            //  The "End" key.
            Keys.End,
        
            //  The "Home" key.
            Keys.Home,
        
            //  The "Left Arrow" key.
            Keys.Left,
        
            //  The "Up Arrow" key.
            Keys.Up,
        
            //  The "Right Arrow" key.
            Keys.Right,
        
            //  The "Down Arrow" key.
            Keys.Down,
        
            //  The "Print Screen" key.
            Keys.PrintScreen,
        
            //  The "Insert" key.
            Keys.Insert,

            //  The "Delete" key.
            Keys.Delete,
        
            //  The "0" key.
            Keys.D0,
        
            //  The "1" key.
            Keys.D1,

            //  The "2" key.
            Keys.D2,
        
            //  The "3" key.
            Keys.D3,
        
            //  The "4" key.
            Keys.D4,
        
            //  The "5" key.
            Keys.D5,
        
            //  The "6" key.
            Keys.D6,
        
            //  The "7" key.
            Keys.D7,
        
            //  The "8" key.
            Keys.D8,
        
            //  The "9" key.
            Keys.D9,
        
            //  The "A" key.
            Keys.A,
        
            //  The "B" key.
            Keys.B,
        
            //  The "C" key.
            Keys.C,
        
            //  The "D" key.
            Keys.D,
        
            //  The "E" key.
            Keys.E,

            //  The "F" key.
            Keys.F,
        
            //  The "G" key.
            Keys.G,
        
            //  The "H" key.
            Keys.H,
        
            //  The "I" key.
            Keys.I,

            //  The "J" key.
            Keys.J,
        
            //  The "K" key.
            Keys.K,

            //  The "L" key.
            Keys.L,
        
            //  The "M" key.
            Keys.M,
        
            //  The "N" key.
            Keys.N,
        
            //  The "O" key.
            Keys.O,
        
            //  The "P" key.
            Keys.P,

            //  The "Q" key.
            Keys.Q,
        
            //  The "R" key.
            Keys.R,
        
            //  The "S" key.
            Keys.S,
        
            //  The "T" key.
            Keys.T,
        
            //  The "U" key.
            Keys.U,
        
            //  The "V" key.
            Keys.V,

            //  The "W" key.
            Keys.W,
        
            //  The "X" key.
            Keys.X,
        
            //  The "Y" key.
            Keys.Y,
        
            //  The "Z" key.
            Keys.Z,
        
            //  The "0" key on the numeric keypad.
            Keys.NumPad0,
        
            //  The "1" key on the numeric keypad.
            Keys.NumPad1,

            //  The "2" key on the numeric keypad.
            Keys.NumPad2,
        
            //  The "3" key on the numeric keypad.
            Keys.NumPad3,
        
            //  The "4" key on the numeric keypad.
            Keys.NumPad4,
        
            //  The "5" key on the numeric keypad.
            Keys.NumPad5,
        
            //  The "6" key on the numeric keypad.
            Keys.NumPad6,
        
            //  The "7" key on the numeric keypad.
            Keys.NumPad7,
        
            //  The "8" key on the numeric keypad.
            Keys.NumPad8,
        
            //  The "9" key on the numeric keypad.
            Keys.NumPad9,
        
            //  The "*" key on the numeric keypad.
            Keys.Multiply,
        
            //  The "+" key on the numeric keypad.
            Keys.Add,
        
            //  The "-" key on the numeric keypad.
            Keys.Subtract,
        
            //  The "." key on the numeric keypad.
            Keys.Decimal,
        
            //  The "/" on the numeric keypad.
            Keys.Divide,
        
            //  The "F1" key.
            Keys.F1,
        
            //  The "F2" key.
            Keys.F2,
        
            //  The "F3" key.
            Keys.F3,
        
            //  The "F4" key.
            Keys.F4,
        
            //  The "F5" key.
            Keys.F5,
        
            //  The "F6" key.
            Keys.F6,
        
            //  The "F7" key.
            Keys.F7,
        
            //  The "F8" key.
            Keys.F8,
        
            //  The "F9" key.
            Keys.F9,
        
            //  The "F10" key.
            Keys.F10,
        
            //  The "F11" key.
            Keys.F11,
        
            //  The "F12" key.
            Keys.F12,
        
            //  The "F13" key.
            Keys.F13,
        
            //  The "F14" key.
            Keys.F14,
        
            //  The "F15" key.
            Keys.F15,

            //  The "F16" key.
            Keys.F16,
        
            //  The "F17" key.
            Keys.F17,
        
            //  The "F18" key.
            Keys.F18,
        
            //  The "F19" key.
            Keys.F19,
        
            //  The "F20" key.
            Keys.F20,
        
            //  The "F21" key.
            Keys.F21,
        
            //  The "F22" key.
            Keys.F22,
        
            //  The "F23" key.
            Keys.F23,
        
            //  The "F24" key.
            Keys.F24,
        
            //  The "Num Lock" key.
            Keys.NumLock,
        
            //  The "Scroll Lock" key.
            Keys.Scroll,
        
            //  The "Browser Back" key.
            Keys.BrowserBack,
        
            //  The "Browser Forward" key.
            Keys.BrowserForward,
        
            //  The "Browser Refresh" key.
            Keys.BrowserRefresh,
        
            //  The "Browser Stop" key.
            Keys.BrowserStop,
        
            //  The "Browser Search" key.
            Keys.BrowserSearch,
        
            //  The "Browser Favorites" key.
            Keys.BrowserFavorites,
        
            //  The "Browser Home" key.
            Keys.BrowserHome,
        
            //  The "Volume Mute" key.
            Keys.VolumeMute,
        
            //  The "Volume Down" key.
            Keys.VolumeDown,
        
            //  The "Volume Up" key.
            Keys.VolumeUp,
        
            //  The "Media Next Track" key.
            Keys.MediaNextTrack,

            //  The "Media Previous Track" key.
            Keys.MediaPreviousTrack,
        
            //  The "Media Stop" key.
            Keys.MediaStop,
        
            //  The "Media Play Pause" key.
            Keys.MediaPlayPause,
        
            //  The "Launch Mail" key.
            Keys.LaunchMail,
        
            //  The "Select Media" key.
            Keys.SelectMedia,
        
            //  The "Launch Application1" key.
            Keys.LaunchApplication1,

            //  The "Launch Application2" key.
            Keys.LaunchApplication2,

            //  The ";" key.
            Keys.OemSemicolon,
        
            //  The "=" key.
            Keys.Oemplus,//Equals,
        
            //  The "," key.
            Keys.Oemcomma,

            //  The "-" key.
            Keys.OemMinus,
        
            //  The "." key.
            Keys.OemPeriod,
        
            //  The "/" key.
            Keys.Oem2,
        
            //  The "[" key.
            Keys.Oem4,
        
            //  The "]" key.
            Keys.Oem6,
        
            //  The "'" key.
            Keys.Oem7,

            //  The "\" key.
            Keys.OemPipe,
        
            //  The "Clear" key.
            Keys.Clear,
        
            //  The "`" key.
            Keys.Oem3,
        
            //  The "Shift" key.
            Keys.Shift,
        
            //  The "Control" key.
            Keys.Control,

            //  The "Alt" key on Windows and Linux or "Option" key on macOS.
            Keys.Alt,
        
            //  The "Command" key on Apple keyboard.
            Keys.None/*MacCommand*/,
        
            // The "Option" key on Apple keyboard.
            Keys.None/*MacOption*/,
        
            // The "Control" key on Apple keyboard.
            Keys.None/*MacControl*/,
        
            // The Microsoft "Windows Logo" key on Windows or "Command" key on macOS or "Meta" key on Linux.
            Keys.LWin,
        
            //  The Microsoft "Menu" key.
            Keys.Apps,
        
            //  The '!' (33, 0x21) key.
            Keys.None, //Keys.ExclamationMark,
        
            //  The '"' (34, 0x220) key.
            Keys.None, //Keys.QuotationMark,
        
            //  The '#' (35, 0x23) key.
            Keys.None, //Keys.NumberSign,
        
            //  The '$' (36, 0x24) key.
            Keys.None, //Keys.DollarSign,
        
            //  The '%' (37, 0x25) key.
            Keys.None, //Keys.PercentSign,

            //  The ampersand (38, 0x26) key.
            Keys.None, //Keys.Ampersand,
        
            //  The '(' (40, 0x28) key.
            Keys.None, //Keys.LeftParenthesis,
        
            //  The ')' (41, 0x29) key.
            Keys.None, //Keys.RightParenthesis,
        
            //  The '*' (42, 0x2A) key.
            Keys.Multiply,

            //  The '+' (43, 0x2B) key.
            Keys.Add,
        
            //  The ':' (58, 0x3A) key.
            Keys.OemSemicolon, //Key.Colon,
        
            //  The less than sign (60, 0x3C) key.
            Keys.Oemcomma, //Key.LessThanSign,
        
            //  The greater than sign (62, 0x3E) key.
            Keys.OemPeriod, //Key.GreaterThanSign,
        
            //  The '?' (63, 0x3F) key.
            Keys.None, //Keys.QuestionMark,
        
            //  The '@' (64, 0x40) key.
            Keys.D2, //Keys.CommercialAt,
        
            //  The '^' (94, 0x5E) key.
            Keys.D6, //Keys.CircumflexAccent,

            //  The '_' (95, 0x5F) key.
            Keys.OemMinus, //Keys.LowLine, 
        
            //  The '{', 123, 0x7B) key.
            Keys.Oem4,//LeftCurlyBracket,
        
            //  The '|', 124, 0x7C) key.
            Keys.OemPipe,//VerticalLine,
        
            //  The '}', 125, 0x7D) key.
            Keys.Oem6, //RightCurlyBracket,

            //  The '~', 126, 0x7E) key.
            Keys.Oem3,//Tilde,
        };

        /// <summary>
        /// Converts <see cref="Keys"/> to <see cref="Key"/>.
        /// </summary>
        /// <param name="keys">Key information in <see cref="Keys"/> enum.</param>
        /// <returns></returns>
        public static Key ToKey(this Keys keys)
        {
            var ikey = GetKeyValue(keys);
            if (ikey < 0 || ikey > Keys.OemClear)
                return Key.None;
            switch (ikey)
            {
                case Keys.None:
                    return Key.None;
                case Keys.LButton:
                    return Key.None;
                case Keys.RButton:
                    return Key.None;
                case Keys.Cancel:
                    return Key.None;
                case Keys.MButton:
                    return Key.None;
                case Keys.XButton1:
                    return Key.None;
                case Keys.XButton2:
                    return Key.None;
                case Keys.Back:
                    return Key.Back;
                case Keys.Tab:
                    return Key.Tab;
                case Keys.LineFeed:
                    return Key.None;
                case Keys.Clear:
                    return Key.Clear;
                case Keys.Return:
                    return Key.Return;
                case Keys.ShiftKey:
                    return Key.Shift;
                case Keys.ControlKey:
                    return Key.Control;
                case Keys.Menu:
                    return Key.Menu;
                case Keys.Pause:
                    return Key.Pause;
                case Keys.CapsLock:
                    return Key.CapsLock;
                case Keys.KanaMode:
                    return Key.None;
                case Keys.JunjaMode:
                    return Key.None;
                case Keys.FinalMode:
                    return Key.None;
                case Keys.HanjaMode:
                    return Key.None;
                case Keys.Escape:
                    return Key.Escape;
                case Keys.IMEConvert:
                    return Key.None;
                case Keys.IMENonconvert:
                    return Key.None;
                case Keys.IMEAccept:
                    return Key.None;
                case Keys.IMEModeChange:
                    return Key.None;
                case Keys.Space:
                    return Key.Space;
                case Keys.PageUp:
                    return Key.PageUp;
                case Keys.PageDown:
                    return Key.PageDown;
                case Keys.End:
                    return Key.End;
                case Keys.Home:
                    return Key.Home;
                case Keys.Left:
                    return Key.Left;
                case Keys.Up:
                    return Key.Up;
                case Keys.Right:
                    return Key.Right;
                case Keys.Down:
                    return Key.Down;
                case Keys.Select:
                    return Key.None;
                case Keys.Print:
                    return Key.PrintScreen;
                case Keys.Execute:
                    return Key.None;
                case Keys.PrintScreen:
                    return Key.PrintScreen;
                case Keys.Insert:
                    return Key.Insert;
                case Keys.Delete:
                    return Key.Delete;
                case Keys.Help:
                    return Key.None;
                case Keys.D0:
                    return Key.D0;
                case Keys.D1:
                    return Key.D1;
                case Keys.D2:
                    return Key.D2;
                case Keys.D3:
                    return Key.D3;
                case Keys.D4:
                    return Key.D4;
                case Keys.D5:
                    return Key.D5;
                case Keys.D6:
                    return Key.D6;
                case Keys.D7:
                    return Key.D7;
                case Keys.D8:
                    return Key.D8;
                case Keys.D9:
                    return Key.D9;
                case Keys.A:
                    return Key.A;
                case Keys.B:
                    return Key.B;
                case Keys.C:
                    return Key.C;
                case Keys.D:
                    return Key.D;
                case Keys.E:
                    return Key.E;
                case Keys.F:
                    return Key.F;
                case Keys.G:
                    return Key.G;
                case Keys.H:
                    return Key.H;
                case Keys.I:
                    return Key.I;
                case Keys.J:
                    return Key.J;
                case Keys.K:
                    return Key.K;
                case Keys.L:
                    return Key.L;
                case Keys.M:
                    return Key.M;
                case Keys.N:
                    return Key.N;
                case Keys.O:
                    return Key.O;
                case Keys.P:
                    return Key.P;
                case Keys.Q:
                    return Key.Q;
                case Keys.R:
                    return Key.R;
                case Keys.S:
                    return Key.S;
                case Keys.T:
                    return Key.T;
                case Keys.U:
                    return Key.U;
                case Keys.V:
                    return Key.V;
                case Keys.W:
                    return Key.W;
                case Keys.X:
                    return Key.X;
                case Keys.Y:
                    return Key.Y;
                case Keys.Z:
                    return Key.Z;
                case Keys.LWin:
                    return Key.Windows;
                case Keys.RWin:
                    return Key.Windows;
                case Keys.Apps:
                    return Key.None;
                case Keys.Sleep:
                    return Key.None;
                case Keys.NumPad0:
                    return Key.NumPad0;
                case Keys.NumPad1:
                    return Key.NumPad1;
                case Keys.NumPad2:
                    return Key.NumPad2;
                case Keys.NumPad3:
                    return Key.NumPad3;
                case Keys.NumPad4:
                    return Key.NumPad4;
                case Keys.NumPad5:
                    return Key.NumPad5;
                case Keys.NumPad6:
                    return Key.NumPad6;
                case Keys.NumPad7:
                    return Key.NumPad7;
                case Keys.NumPad8:
                    return Key.NumPad8;
                case Keys.NumPad9:
                    return Key.NumPad9;
                case Keys.Multiply:
                    return Key.Asterisk;
                case Keys.Add:
                    return Key.PlusSign;
                case Keys.Separator:
                    return Key.None;
                case Keys.Subtract:
                    return Key.Minus;
                case Keys.Decimal:
                    return Key.NumPadDot;
                case Keys.Divide:
                    return Key.Slash;
                case Keys.F1:
                    return Key.F1;
                case Keys.F2:
                    return Key.F2;
                case Keys.F3:
                    return Key.F3;
                case Keys.F4:
                    return Key.F4;
                case Keys.F5:
                    return Key.F5;
                case Keys.F6:
                    return Key.F6;
                case Keys.F7:
                    return Key.F7;
                case Keys.F8:
                    return Key.F8;
                case Keys.F9:
                    return Key.F9;
                case Keys.F10:
                    return Key.F10;
                case Keys.F11:
                    return Key.F11;
                case Keys.F12:
                    return Key.F12;
                case Keys.F13:
                    return Key.F13;
                case Keys.F14:
                    return Key.F14;
                case Keys.F15:
                    return Key.F15;
                case Keys.F16:
                    return Key.F16;
                case Keys.F17:
                    return Key.F17;
                case Keys.F18:
                    return Key.F18;
                case Keys.F19:
                    return Key.F19;
                case Keys.F20:
                    return Key.F20;
                case Keys.F21:
                    return Key.F21;
                case Keys.F22:
                    return Key.F22;
                case Keys.F23:
                    return Key.F23;
                case Keys.F24:
                    return Key.F24;
                case Keys.NumLock:
                    return Key.NumLock;
                case Keys.Scroll:
                    return Key.ScrollLock;
                case Keys.LShiftKey:
                    return Key.Shift;
                case Keys.RShiftKey:
                    return Key.Shift;
                case Keys.LControlKey:
                    return Key.Control;
                case Keys.RControlKey:
                    return Key.Control;
                case Keys.LMenu:
                    return Key.Menu;
                case Keys.RMenu:
                    return Key.Menu;
                case Keys.BrowserBack:
                    return Key.BrowserBack;
                case Keys.BrowserForward:
                    return Key.BrowserForward;
                case Keys.BrowserRefresh:
                    return Key.BrowserRefresh;
                case Keys.BrowserStop:
                    return Key.BrowserStop;
                case Keys.BrowserSearch:
                    return Key.BrowserSearch;
                case Keys.BrowserFavorites:
                    return Key.BrowserFavorites;
                case Keys.BrowserHome:
                    return Key.BrowserHome;
                case Keys.VolumeMute:
                    return Key.VolumeMute;
                case Keys.VolumeDown:
                    return Key.VolumeDown;
                case Keys.VolumeUp:
                    return Key.VolumeUp;
                case Keys.MediaNextTrack:
                    return Key.MediaNextTrack;
                case Keys.MediaPreviousTrack:
                    return Key.MediaPreviousTrack;
                case Keys.MediaStop:
                    return Key.MediaStop;
                case Keys.MediaPlayPause:
                    return Key.MediaPlayPause;
                case Keys.LaunchMail:
                    return Key.LaunchMail;
                case Keys.SelectMedia:
                    return Key.SelectMedia;
                case Keys.LaunchApplication1:
                    return Key.LaunchApplication1;
                case Keys.LaunchApplication2:
                    return Key.LaunchApplication2;
                case Keys.OemSemicolon:
                    return Key.Semicolon;
                case Keys.Oemplus:
                    return Key.PlusSign;
                case Keys.Oemcomma:
                    return Key.Comma;
                case Keys.OemMinus:
                    return Key.Minus;
                case Keys.OemPeriod:
                    return Key.OemPeriod;
                case Keys.OemQuestion:
                    return Key.QuestionMark;
                case Keys.Oemtilde:
                    return Key.Tilde;
                case Keys.OemOpenBrackets:
                    return Key.OpenBracket;
                case Keys.OemPipe:
                    return Key.OemPipe;
                case Keys.OemCloseBrackets:
                    return Key.CloseBracket;
                case Keys.OemQuotes:
                    return Key.Quote;
                case Keys.Oem8:
                    return Key.None;
                case Keys.OemBackslash:
                    return Key.Backslash;
                case Keys.ProcessKey:
                    return Key.None;
                case Keys.Packet:
                    return Key.None;
                case Keys.Attn:
                    return Key.None;
                case Keys.Crsel:
                    return Key.None;
                case Keys.Exsel:
                    return Key.None;
                case Keys.EraseEof:
                    return Key.None;
                case Keys.Play:
                    return Key.None;
                case Keys.Zoom:
                    return Key.None;
                case Keys.NoName:
                    return Key.None;
                case Keys.Pa1:
                    return Key.None;
                case Keys.OemClear:
                    return Key.None;
                default:
                    return Key.None;
            }
        }

        /// <summary>
        /// Converts <see cref="Key"/> to <see cref="Keys"/>.
        /// </summary>
        public static Keys ToKeys(this Key key, ModifierKeys modifiers)
        {
            var result = KeyToKeysConversion[(int)key];
            return result | modifiers.ToKeys();
        }

        /// <summary>
        /// Converts <see cref="ModifierKeys"/> to <see cref="Keys"/>.
        /// </summary>
        public static Keys ToKeys(this ModifierKeys modifiers)
        {
            Keys result = 0;
            if (modifiers.HasFlag(ModifierKeys.Shift))
                result |= Keys.Shift;
            if (modifiers.HasFlag(ModifierKeys.Control))
                result |= Keys.Control;
            if (modifiers.HasFlag(ModifierKeys.Alt))
                result |= Keys.Alt;
            return result;
        }

        /// <summary>
        /// Converts <see cref="Keys"/> to <see cref="ModifierKeys"/>.
        /// </summary>
        public static ModifierKeys ToModifiers(this Keys keys)
        {
            ModifierKeys result = 0;
            if ((keys & Keys.Control) == Keys.Control)
                result |= ModifierKeys.Control;
            if ((keys & Keys.Shift) == Keys.Shift)
                result |= ModifierKeys.Shift;
            if ((keys & Keys.Alt) == Keys.Alt)
                result |= ModifierKeys.Alt;
            return result;
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
        public static Keys GetKeyValue(Keys keys) => (keys & Keys.KeyCode);
    }
}