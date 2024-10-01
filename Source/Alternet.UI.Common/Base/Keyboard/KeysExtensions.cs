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
        private static PlatformKeyMapping<Keys>? keyAndKeysMapping;

        /// <summary>
        /// Gets or sets <see cref="Keys"/> to/from <see cref="Key"/> enum mapping.
        /// </summary>
        public static PlatformKeyMapping<Keys> KeyAndKeysMapping
        {
            get
            {
                if (keyAndKeysMapping is null)
                {
                    keyAndKeysMapping = new PlatformKeyMapping<Keys>(Keys.OemClear, Key.Max);
                    RegisterKeyMappings(keyAndKeysMapping);
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
        public static void RegisterKeyMappings(PlatformKeyMapping<Keys> mapping)
        {
            mapping.Add(Keys.Back, Key.Back);
            mapping.Add(Keys.Tab, Key.Tab);
            mapping.Add(Keys.Clear, Key.Clear);
            mapping.Add(Keys.Return, Key.Return);
            mapping.Add(Keys.ShiftKey, Key.Shift);
            mapping.Add(Keys.ControlKey, Key.Control);
            mapping.Add(Keys.Menu, Key.Menu);
            mapping.Add(Keys.Pause, Key.Pause);
            mapping.Add(Keys.CapsLock, Key.CapsLock);
            mapping.Add(Keys.KanaMode, Key.Kana);
            mapping.Add(Keys.JunjaMode, Key.Junja);
            mapping.Add(Keys.FinalMode, Key.Final);
            mapping.Add(Keys.HanjaMode, Key.Hanja);
            mapping.Add(Keys.Escape, Key.Escape);
            mapping.Add(Keys.IMEConvert, Key.Convert);
            mapping.Add(Keys.IMENonconvert, Key.NonConvert);
            mapping.Add(Keys.IMEAccept, Key.Accept);
            mapping.Add(Keys.IMEModeChange, Key.ModeChange);
            mapping.Add(Keys.Space, Key.Space);
            mapping.Add(Keys.PageUp, Key.PageUp);
            mapping.Add(Keys.PageDown, Key.PageDown);
            mapping.Add(Keys.End, Key.End);
            mapping.Add(Keys.Home, Key.Home);
            mapping.Add(Keys.Left, Key.Left);
            mapping.Add(Keys.Up, Key.Up);
            mapping.Add(Keys.Right, Key.Right);
            mapping.Add(Keys.Down, Key.Down);
            mapping.Add(Keys.Select, Key.Select);
            mapping.Add(Keys.Print, Key.PrintScreen);
            mapping.Add(Keys.Execute, Key.Execute);
            mapping.Add(Keys.PrintScreen, Key.PrintScreen);
            mapping.Add(Keys.Insert, Key.Insert);
            mapping.Add(Keys.Delete, Key.Delete);
            mapping.Add(Keys.Help, Key.F1);
            mapping.Add(Keys.D0, Key.D0);
            mapping.Add(Keys.D1, Key.D1);
            mapping.Add(Keys.D2, Key.D2);
            mapping.Add(Keys.D3, Key.D3);
            mapping.Add(Keys.D4, Key.D4);
            mapping.Add(Keys.D5, Key.D5);
            mapping.Add(Keys.D6, Key.D6);
            mapping.Add(Keys.D7, Key.D7);
            mapping.Add(Keys.D8, Key.D8);
            mapping.Add(Keys.D9, Key.D9);
            mapping.Add(Keys.A, Key.A);
            mapping.Add(Keys.B, Key.B);
            mapping.Add(Keys.C, Key.C);
            mapping.Add(Keys.D, Key.D);
            mapping.Add(Keys.E, Key.E);
            mapping.Add(Keys.F, Key.F);
            mapping.Add(Keys.G, Key.G);
            mapping.Add(Keys.H, Key.H);
            mapping.Add(Keys.I, Key.I);
            mapping.Add(Keys.J, Key.J);
            mapping.Add(Keys.K, Key.K);
            mapping.Add(Keys.L, Key.L);
            mapping.Add(Keys.M, Key.M);
            mapping.Add(Keys.N, Key.N);
            mapping.Add(Keys.O, Key.O);
            mapping.Add(Keys.P, Key.P);
            mapping.Add(Keys.Q, Key.Q);
            mapping.Add(Keys.R, Key.R);
            mapping.Add(Keys.S, Key.S);
            mapping.Add(Keys.T, Key.T);
            mapping.Add(Keys.U, Key.U);
            mapping.Add(Keys.V, Key.V);
            mapping.Add(Keys.W, Key.W);
            mapping.Add(Keys.X, Key.X);
            mapping.Add(Keys.Y, Key.Y);
            mapping.Add(Keys.Z, Key.Z);
            mapping.Add(Keys.Apps, Key.Menu);
            mapping.Add(Keys.Sleep, Key.Sleep);
            mapping.Add(Keys.NumPad0, Key.NumPad0);
            mapping.Add(Keys.NumPad1, Key.NumPad1);
            mapping.Add(Keys.NumPad2, Key.NumPad2);
            mapping.Add(Keys.NumPad3, Key.NumPad3);
            mapping.Add(Keys.NumPad4, Key.NumPad4);
            mapping.Add(Keys.NumPad5, Key.NumPad5);
            mapping.Add(Keys.NumPad6, Key.NumPad6);
            mapping.Add(Keys.NumPad7, Key.NumPad7);
            mapping.Add(Keys.NumPad8, Key.NumPad8);
            mapping.Add(Keys.NumPad9, Key.NumPad9);
            mapping.Add(Keys.Multiply, Key.Asterisk);
            mapping.Add(Keys.Add, Key.PlusSign);
            mapping.Add(Keys.Separator, Key.Comma);
            mapping.Add(Keys.Subtract, Key.Minus);
            mapping.Add(Keys.Decimal, Key.NumPadDot);
            mapping.Add(Keys.Divide, Key.Slash);
            mapping.Add(Keys.F1, Key.F1);
            mapping.Add(Keys.F2, Key.F2);
            mapping.Add(Keys.F3, Key.F3);
            mapping.Add(Keys.F4, Key.F4);
            mapping.Add(Keys.F5, Key.F5);
            mapping.Add(Keys.F6, Key.F6);
            mapping.Add(Keys.F7, Key.F7);
            mapping.Add(Keys.F8, Key.F8);
            mapping.Add(Keys.F9, Key.F9);
            mapping.Add(Keys.F10, Key.F10);
            mapping.Add(Keys.F11, Key.F11);
            mapping.Add(Keys.F12, Key.F12);
            mapping.Add(Keys.F13, Key.F13);
            mapping.Add(Keys.F14, Key.F14);
            mapping.Add(Keys.F15, Key.F15);
            mapping.Add(Keys.F16, Key.F16);
            mapping.Add(Keys.F17, Key.F17);
            mapping.Add(Keys.F18, Key.F18);
            mapping.Add(Keys.F19, Key.F19);
            mapping.Add(Keys.F20, Key.F20);
            mapping.Add(Keys.F21, Key.F21);
            mapping.Add(Keys.F22, Key.F22);
            mapping.Add(Keys.F23, Key.F23);
            mapping.Add(Keys.F24, Key.F24);
            mapping.Add(Keys.NumLock, Key.NumLock);
            mapping.Add(Keys.Scroll, Key.ScrollLock);

            mapping.Add(Keys.BrowserBack, Key.BrowserBack);
            mapping.Add(Keys.BrowserForward, Key.BrowserForward);
            mapping.Add(Keys.BrowserRefresh, Key.BrowserRefresh);
            mapping.Add(Keys.BrowserStop, Key.BrowserStop);
            mapping.Add(Keys.BrowserSearch, Key.BrowserSearch);
            mapping.Add(Keys.BrowserFavorites, Key.BrowserFavorites);
            mapping.Add(Keys.BrowserHome, Key.BrowserHome);
            mapping.Add(Keys.VolumeMute, Key.VolumeMute);
            mapping.Add(Keys.VolumeDown, Key.VolumeDown);
            mapping.Add(Keys.VolumeUp, Key.VolumeUp);
            mapping.Add(Keys.MediaNextTrack, Key.MediaNextTrack);
            mapping.Add(Keys.MediaPreviousTrack, Key.MediaPreviousTrack);
            mapping.Add(Keys.MediaStop, Key.MediaStop);
            mapping.Add(Keys.MediaPlayPause, Key.MediaPlayPause);
            mapping.Add(Keys.LaunchMail, Key.LaunchMail);
            mapping.Add(Keys.SelectMedia, Key.SelectMedia);
            mapping.Add(Keys.LaunchApplication1, Key.LaunchApplication1);
            mapping.Add(Keys.LaunchApplication2, Key.LaunchApplication2);
            mapping.Add(Keys.OemSemicolon, Key.Semicolon);
            mapping.Add(Keys.Oemplus, Key.PlusSign);
            mapping.Add(Keys.Oemcomma, Key.Comma);
            mapping.Add(Keys.OemMinus, Key.Minus);
            mapping.Add(Keys.OemPeriod, Key.OemPeriod);
            mapping.Add(Keys.OemQuestion, Key.QuestionMark);
            mapping.Add(Keys.Oemtilde, Key.Tilde);
            mapping.Add(Keys.OemOpenBrackets, Key.OpenBracket);
            mapping.Add(Keys.OemPipe, Key.OemPipe);
            mapping.Add(Keys.OemCloseBrackets, Key.CloseBracket);
            mapping.Add(Keys.OemQuotes, Key.Quote);
            mapping.Add(Keys.OemBackslash, Key.Backslash);
            mapping.Add(Keys.OemClear, Key.Clear);
            mapping.Add(Keys.Exsel, Key.ExSel);
            mapping.Add(Keys.Play, Key.MediaPlayPause);
            mapping.Add(Keys.Cancel, Key.NavigationCancel);
            mapping.Add(Keys.Crsel, Key.CrSelOrProps);

            mapping.Add(Keys.LineFeed, null);
            mapping.Add(Keys.Oem8, null);
            mapping.Add(Keys.ProcessKey, null);
            mapping.Add(Keys.Packet, null);
            mapping.Add(Keys.Attn, null);
            mapping.Add(Keys.EraseEof, null);
            mapping.Add(Keys.Zoom, null);
            mapping.Add(Keys.NoName, null);
            mapping.Add(Keys.Pa1, null);

            mapping.Add(Keys.LButton, null);
            mapping.Add(Keys.RButton, null);
            mapping.Add(Keys.MButton, null);
            mapping.Add(Keys.XButton1, null);
            mapping.Add(Keys.XButton2, null);

            mapping.Add(Keys.LShiftKey, null);
            mapping.Add(Keys.RShiftKey, null);
            mapping.Add(Keys.LControlKey, null);
            mapping.Add(Keys.RControlKey, null);
            mapping.Add(Keys.LMenu, null);
            mapping.Add(Keys.RMenu, null);
            mapping.Add(Keys.LWin, null);
            mapping.Add(Keys.RWin, null);
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