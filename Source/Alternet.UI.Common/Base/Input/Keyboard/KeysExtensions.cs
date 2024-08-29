using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the keyboard input handling,
    /// implements extension methods for input related enums and classes.
    /// </summary>
    public static class KeysExtensions
    {
        private static AbstractTwoWayEnumMapping<Keys, Key>? keyAndKeysMapping;

        /// <summary>
        /// Gets or sets <see cref="Keys"/> to/from <see cref="Key"/> enum mapping.
        /// </summary>
        public static AbstractTwoWayEnumMapping<Keys, Key> KeyAndKeysMapping
        {
            get
            {
                if (keyAndKeysMapping is null)
                {
                    keyAndKeysMapping = new TwoWayEnumMapping<Keys, Key>(Keys.OemClear, Key.Max);
                    RegisterKeyMappings();
                }

                return keyAndKeysMapping;
            }

            set
            {
                keyAndKeysMapping = value;
            }
        }

        /// <summary>
        /// Registers default <see cref="Keys"/> to/from <see cref="Key"/> enum mappings.
        /// </summary>
        public static void RegisterKeyMappings()
        {
            Fn(Keys.Back, Key.Back);
            Fn(Keys.Tab, Key.Tab);
            Fn(Keys.Clear, Key.Clear);
            Fn(Keys.Return, Key.Return);
            Fn(Keys.ShiftKey, Key.Shift);
            Fn(Keys.ControlKey, Key.Control);
            Fn(Keys.Menu, Key.Menu);
            Fn(Keys.Pause, Key.Pause);
            Fn(Keys.CapsLock, Key.CapsLock);
            Fn(Keys.KanaMode, Key.Kana);
            Fn(Keys.JunjaMode, Key.Junja);
            Fn(Keys.FinalMode, Key.Final);
            Fn(Keys.HanjaMode, Key.Hanja);
            Fn(Keys.Escape, Key.Escape);
            Fn(Keys.IMEConvert, Key.Convert);
            Fn(Keys.IMENonconvert, Key.NonConvert);
            Fn(Keys.IMEAccept, Key.Accept);
            Fn(Keys.IMEModeChange, Key.ModeChange);
            Fn(Keys.Space, Key.Space);
            Fn(Keys.PageUp, Key.PageUp);
            Fn(Keys.PageDown, Key.PageDown);
            Fn(Keys.End, Key.End);
            Fn(Keys.Home, Key.Home);
            Fn(Keys.Left, Key.Left);
            Fn(Keys.Up, Key.Up);
            Fn(Keys.Right, Key.Right);
            Fn(Keys.Down, Key.Down);
            Fn(Keys.Select, Key.Select);
            Fn(Keys.Print, Key.PrintScreen);
            Fn(Keys.Execute, Key.Execute);
            Fn(Keys.PrintScreen, Key.PrintScreen);
            Fn(Keys.Insert, Key.Insert);
            Fn(Keys.Delete, Key.Delete);
            Fn(Keys.Help, Key.F1);
            Fn(Keys.D0, Key.D0);
            Fn(Keys.D1, Key.D1);
            Fn(Keys.D2, Key.D2);
            Fn(Keys.D3, Key.D3);
            Fn(Keys.D4, Key.D4);
            Fn(Keys.D5, Key.D5);
            Fn(Keys.D6, Key.D6);
            Fn(Keys.D7, Key.D7);
            Fn(Keys.D8, Key.D8);
            Fn(Keys.D9, Key.D9);
            Fn(Keys.A, Key.A);
            Fn(Keys.B, Key.B);
            Fn(Keys.C, Key.C);
            Fn(Keys.D, Key.D);
            Fn(Keys.E, Key.E);
            Fn(Keys.F, Key.F);
            Fn(Keys.G, Key.G);
            Fn(Keys.H, Key.H);
            Fn(Keys.I, Key.I);
            Fn(Keys.J, Key.J);
            Fn(Keys.K, Key.K);
            Fn(Keys.L, Key.L);
            Fn(Keys.M, Key.M);
            Fn(Keys.N, Key.N);
            Fn(Keys.O, Key.O);
            Fn(Keys.P, Key.P);
            Fn(Keys.Q, Key.Q);
            Fn(Keys.R, Key.R);
            Fn(Keys.S, Key.S);
            Fn(Keys.T, Key.T);
            Fn(Keys.U, Key.U);
            Fn(Keys.V, Key.V);
            Fn(Keys.W, Key.W);
            Fn(Keys.X, Key.X);
            Fn(Keys.Y, Key.Y);
            Fn(Keys.Z, Key.Z);
            Fn(Keys.Apps, Key.Menu);
            Fn(Keys.Sleep, Key.Sleep);
            Fn(Keys.NumPad0, Key.NumPad0);
            Fn(Keys.NumPad1, Key.NumPad1);
            Fn(Keys.NumPad2, Key.NumPad2);
            Fn(Keys.NumPad3, Key.NumPad3);
            Fn(Keys.NumPad4, Key.NumPad4);
            Fn(Keys.NumPad5, Key.NumPad5);
            Fn(Keys.NumPad6, Key.NumPad6);
            Fn(Keys.NumPad7, Key.NumPad7);
            Fn(Keys.NumPad8, Key.NumPad8);
            Fn(Keys.NumPad9, Key.NumPad9);
            Fn(Keys.Multiply, Key.Asterisk);
            Fn(Keys.Add, Key.PlusSign);
            Fn(Keys.Separator, Key.Comma);
            Fn(Keys.Subtract, Key.Minus);
            Fn(Keys.Decimal, Key.NumPadDot);
            Fn(Keys.Divide, Key.Slash);
            Fn(Keys.F1, Key.F1);
            Fn(Keys.F2, Key.F2);
            Fn(Keys.F3, Key.F3);
            Fn(Keys.F4, Key.F4);
            Fn(Keys.F5, Key.F5);
            Fn(Keys.F6, Key.F6);
            Fn(Keys.F7, Key.F7);
            Fn(Keys.F8, Key.F8);
            Fn(Keys.F9, Key.F9);
            Fn(Keys.F10, Key.F10);
            Fn(Keys.F11, Key.F11);
            Fn(Keys.F12, Key.F12);
            Fn(Keys.F13, Key.F13);
            Fn(Keys.F14, Key.F14);
            Fn(Keys.F15, Key.F15);
            Fn(Keys.F16, Key.F16);
            Fn(Keys.F17, Key.F17);
            Fn(Keys.F18, Key.F18);
            Fn(Keys.F19, Key.F19);
            Fn(Keys.F20, Key.F20);
            Fn(Keys.F21, Key.F21);
            Fn(Keys.F22, Key.F22);
            Fn(Keys.F23, Key.F23);
            Fn(Keys.F24, Key.F24);
            Fn(Keys.NumLock, Key.NumLock);
            Fn(Keys.Scroll, Key.ScrollLock);

            Fn(Keys.BrowserBack, Key.BrowserBack);
            Fn(Keys.BrowserForward, Key.BrowserForward);
            Fn(Keys.BrowserRefresh, Key.BrowserRefresh);
            Fn(Keys.BrowserStop, Key.BrowserStop);
            Fn(Keys.BrowserSearch, Key.BrowserSearch);
            Fn(Keys.BrowserFavorites, Key.BrowserFavorites);
            Fn(Keys.BrowserHome, Key.BrowserHome);
            Fn(Keys.VolumeMute, Key.VolumeMute);
            Fn(Keys.VolumeDown, Key.VolumeDown);
            Fn(Keys.VolumeUp, Key.VolumeUp);
            Fn(Keys.MediaNextTrack, Key.MediaNextTrack);
            Fn(Keys.MediaPreviousTrack, Key.MediaPreviousTrack);
            Fn(Keys.MediaStop, Key.MediaStop);
            Fn(Keys.MediaPlayPause, Key.MediaPlayPause);
            Fn(Keys.LaunchMail, Key.LaunchMail);
            Fn(Keys.SelectMedia, Key.SelectMedia);
            Fn(Keys.LaunchApplication1, Key.LaunchApplication1);
            Fn(Keys.LaunchApplication2, Key.LaunchApplication2);
            Fn(Keys.OemSemicolon, Key.Semicolon);
            Fn(Keys.Oemplus, Key.PlusSign);
            Fn(Keys.Oemcomma, Key.Comma);
            Fn(Keys.OemMinus, Key.Minus);
            Fn(Keys.OemPeriod, Key.OemPeriod);
            Fn(Keys.OemQuestion, Key.QuestionMark);
            Fn(Keys.Oemtilde, Key.Tilde);
            Fn(Keys.OemOpenBrackets, Key.OpenBracket);
            Fn(Keys.OemPipe, Key.OemPipe);
            Fn(Keys.OemCloseBrackets, Key.CloseBracket);
            Fn(Keys.OemQuotes, Key.Quote);
            Fn(Keys.OemBackslash, Key.Backslash);
            Fn(Keys.OemClear, Key.Clear);

            // Fn(Keys.LButton, Key.None);
            // Fn(Keys.RButton, Key.None);
            // Fn(Keys.Cancel, Key.None);
            // Fn(Keys.MButton, Key.None);
            // Fn(Keys.XButton1, Key.None);
            // Fn(Keys.XButton2, Key.None);
            // Fn(Keys.LineFeed, Key.None);
            // Fn(Keys.Oem8, Key.None);
            // Fn(Keys.ProcessKey, Key.None);
            // Fn(Keys.Packet, Key.None);
            // Fn(Keys.Attn, Key.None);
            // Fn(Keys.Crsel, Key.None);
            // Fn(Keys.Exsel, Key.None);
            // Fn(Keys.EraseEof, Key.None);
            // Fn(Keys.Play, Key.None);
            // Fn(Keys.Zoom, Key.None);
            // Fn(Keys.NoName, Key.None);
            // Fn(Keys.Pa1, Key.None);
            // Fn(Keys.LShiftKey, Key.Shift);
            // Fn(Keys.RShiftKey, Key.Shift);
            // Fn(Keys.LControlKey, Key.Control);
            // Fn(Keys.RControlKey, Key.Control);
            // Fn(Keys.LMenu, Key.Menu);
            // Fn(Keys.RMenu, Key.Menu);
            // Fn(Keys.LWin, Key.Windows);
            // Fn(Keys.RWin, Key.Windows);
            void Fn(Keys keys, Key key)
            {
                KeyAndKeysMapping.Add(keys, key);
            }
        }

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

            return KeyAndKeysMapping.SourceToDest.Convert(ikey);
        }

        /// <summary>
        /// Converts <see cref="Key"/> to <see cref="Keys"/>.
        /// </summary>
        public static Keys ToKeys(this Key key, ModifierKeys modifiers)
        {
            var result = KeyAndKeysMapping.DestToSource.Convert(key);
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

        /// <summary>
        /// Gets whether the specified <see cref="ModifierKeys"/> value
        /// has <see cref="ModifierKeys.Alt"/> flag.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAlt(this ModifierKeys modifiers)
        {
            return modifiers.HasFlag(ModifierKeys.Alt);
        }

        /// <summary>
        /// Gets whether the specified <see cref="ModifierKeys"/> value
        /// has <see cref="ModifierKeys.Control"/> flag.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasControl(this ModifierKeys modifiers)
        {
            return modifiers.HasFlag(ModifierKeys.Control);
        }

        /// <summary>
        /// Gets whether the specified <see cref="ModifierKeys"/> value
        /// has <see cref="ModifierKeys.Shift"/> flag.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasShift(this ModifierKeys modifiers)
        {
            return modifiers.HasFlag(ModifierKeys.Shift);
        }

        /// <summary>
        /// Gets whether the specified <see cref="ModifierKeys"/> value
        /// has <see cref="ModifierKeys.Windows"/> flag.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasWindows(this ModifierKeys modifiers)
        {
            return modifiers.HasFlag(ModifierKeys.Windows);
        }

        /// <summary>
        /// Converts the specified <see cref="ModifierKeys"/> value
        /// to <see cref="RawModifierKeys"/> value.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns></returns>
        public static RawModifierKeys ToRawModifierKeys(this ModifierKeys modifiers)
        {
            RawModifierKeys result = RawModifierKeys.None;

            if (modifiers.HasAlt())
                result |= RawModifierKeys.Alt;
            if (modifiers.HasControl())
                result |= RawModifierKeys.Control;
            if (modifiers.HasShift())
                result |= RawModifierKeys.Shift;
            if (modifiers.HasWindows())
                result |= RawModifierKeys.Windows;
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
            if (result < 0 || result > Keys.OemClear)
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
        public static Keys GetKeyValue(Keys keys) => keys & Keys.KeyCode;
    }
}