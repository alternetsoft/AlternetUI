#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


#if !ALTERNET_UI_INTEGRATION_REMOTING
using Alternet.UI.Markup;
#endif

using System.ComponentModel;

#if ALTERNET_UI_INTEGRATION_REMOTING
namespace Alternet.UI.Integration.Remoting
#else
namespace Alternet.UI
#endif
{
    /// <summary>
    ///     An enumeration of all of the possible key values on a keyboard.
    /// </summary>
#if !ALTERNET_UI_INTEGRATION_REMOTING
    [TypeConverter(typeof(KeyConverter))]
    [ValueSerializer(typeof(KeyValueSerializer))]
#endif
    public enum Key
    {
        /// <summary>
        ///  No key pressed.
        /// </summary>
        None,

        /// <summary>
        ///  The "Backspace" key.
        /// </summary>
        Backspace,

        /// <summary>
        ///  The "Tab" key.
        /// </summary>
        Tab,

        /// <summary>
        ///  The "Enter" key.
        /// </summary>
        Enter,

        /// <summary>
        ///  The "Pause" key.
        /// </summary>
        Pause,

        /// <summary>
        ///  The "Caps Lock" key.
        /// </summary>
        CapsLock,

        /// <summary>
        ///  The "Esc" key.
        /// </summary>
        Escape,

        /// <summary>
        ///  The "Space Bar" key.
        /// </summary>
        Space,

        /// <summary>
        ///  The "Page Up" key.
        /// </summary>
        PageUp,

        /// <summary>
        ///  The "Page Down" key.
        /// </summary>
        PageDown,

        /// <summary>
        ///  The "End" key.
        /// </summary>
        End,

        /// <summary>
        ///  The "Home" key.
        /// </summary>
        Home,

        /// <summary>
        ///  The "Left Arrow" key.
        /// </summary>
        LeftArrow,

        /// <summary>
        ///  The "Up Arrow" key.
        /// </summary>
        UpArrow,

        /// <summary>
        ///  The "Right Arrow" key.
        /// </summary>
        RightArrow,

        /// <summary>
        ///  The "Down Arrow" key.
        /// </summary>
        DownArrow,

        /// <summary>
        ///  The "Print Screen" key.
        /// </summary>
        PrintScreen,

        /// <summary>
        ///  The "Insert" key.
        /// </summary>
        Insert,

        /// <summary>
        ///  The "Delete" key.
        /// </summary>
        Delete,

        /// <summary>
        ///  The "0" key.
        /// </summary>
        D0,

        /// <summary>
        ///  The "1" key.
        /// </summary>
        D1,

        /// <summary>
        ///  The "2" key.
        /// </summary>
        D2,

        /// <summary>
        ///  The "3" key.
        /// </summary>
        D3,

        /// <summary>
        ///  The "4" key.
        /// </summary>
        D4,

        /// <summary>
        ///  The "5" key.
        /// </summary>
        D5,

        /// <summary>
        ///  The "6" key.
        /// </summary>
        D6,

        /// <summary>
        ///  The "7" key.
        /// </summary>
        D7,

        /// <summary>
        ///  The "8" key.
        /// </summary>
        D8,

        /// <summary>
        ///  The "9" key.
        /// </summary>
        D9,

        /// <summary>
        ///  The "A" key.
        /// </summary>
        A,

        /// <summary>
        ///  The "B" key.
        /// </summary>
        B,

        /// <summary>
        ///  The "C" key.
        /// </summary>
        C,

        /// <summary>
        ///  The "D" key.
        /// </summary>
        D,

        /// <summary>
        ///  The "E" key.
        /// </summary>
        E,

        /// <summary>
        ///  The "F" key.
        /// </summary>
        F,

        /// <summary>
        ///  The "G" key.
        /// </summary>
        G,

        /// <summary>
        ///  The "H" key.
        /// </summary>
        H,

        /// <summary>
        ///  The "I" key.
        /// </summary>
        I,

        /// <summary>
        ///  The "J" key.
        /// </summary>
        J,

        /// <summary>
        ///  The "K" key.
        /// </summary>
        K,

        /// <summary>
        ///  The "L" key.
        /// </summary>
        L,

        /// <summary>
        ///  The "M" key.
        /// </summary>
        M,

        /// <summary>
        ///  The "N" key.
        /// </summary>
        N,

        /// <summary>
        ///  The "O" key.
        /// </summary>
        O,

        /// <summary>
        ///  The "P" key.
        /// </summary>
        P,

        /// <summary>
        ///  The "Q" key.
        /// </summary>
        Q,

        /// <summary>
        ///  The "R" key.
        /// </summary>
        R,

        /// <summary>
        ///  The "S" key.
        /// </summary>
        S,

        /// <summary>
        ///  The "T" key.
        /// </summary>
        T,

        /// <summary>
        ///  The "U" key.
        /// </summary>
        U,

        /// <summary>
        ///  The "V" key.
        /// </summary>
        V,

        /// <summary>
        ///  The "W" key.
        /// </summary>
        W,

        /// <summary>
        ///  The "X" key.
        /// </summary>
        X,

        /// <summary>
        ///  The "Y" key.
        /// </summary>
        Y,

        /// <summary>
        ///  The "Z" key.
        /// </summary>
        Z,

        /// <summary>
        ///  The "0" key on the numeric keypad.
        /// </summary>
        NumPad0,

        /// <summary>
        ///  The "1" key on the numeric keypad.
        /// </summary>
        NumPad1,

        /// <summary>
        ///  The "2" key on the numeric keypad.
        /// </summary>
        NumPad2,

        /// <summary>
        ///  The "3" key on the numeric keypad.
        /// </summary>
        NumPad3,

        /// <summary>
        ///  The "4" key on the numeric keypad.
        /// </summary>
        NumPad4,

        /// <summary>
        ///  The "5" key on the numeric keypad.
        /// </summary>
        NumPad5,

        /// <summary>
        ///  The "6" key on the numeric keypad.
        /// </summary>
        NumPad6,

        /// <summary>
        ///  The "7" key on the numeric keypad.
        /// </summary>
        NumPad7,

        /// <summary>
        ///  The "8" key on the numeric keypad.
        /// </summary>
        NumPad8,

        /// <summary>
        ///  The "9" key on the numeric keypad.
        /// </summary>
        NumPad9,

        /// <summary>
        ///  The "*" key on the numeric keypad.
        /// </summary>
        NumPadStar,

        /// <summary>
        ///  The "+" key on the numeric keypad.
        /// </summary>
        NumPadPlus,

        /// <summary>
        ///  The "-" key on the numeric keypad.
        /// </summary>
        NumPadMinus,

        /// <summary>
        ///  The "." key on the numeric keypad.
        /// </summary>
        NumPadDot,

        /// <summary>
        ///  The "/" on the numeric keypad.
        /// </summary>
        NumPadSlash,

        /// <summary>
        ///  The "F1" key.
        /// </summary>
        F1,

        /// <summary>
        ///  The "F2" key.
        /// </summary>
        F2,

        /// <summary>
        ///  The "F3" key.
        /// </summary>
        F3,

        /// <summary>
        ///  The "F4" key.
        /// </summary>
        F4,

        /// <summary>
        ///  The "F5" key.
        /// </summary>
        F5,

        /// <summary>
        ///  The "F6" key.
        /// </summary>
        F6,

        /// <summary>
        ///  The "F7" key.
        /// </summary>
        F7,

        /// <summary>
        ///  The "F8" key.
        /// </summary>
        F8,

        /// <summary>
        ///  The "F9" key.
        /// </summary>
        F9,

        /// <summary>
        ///  The "F10" key.
        /// </summary>
        F10,

        /// <summary>
        ///  The "F11" key.
        /// </summary>
        F11,

        /// <summary>
        ///  The "F12" key.
        /// </summary>
        F12,

        /// <summary>
        ///  The "F13" key.
        /// </summary>
        F13,

        /// <summary>
        ///  The "F14" key.
        /// </summary>
        F14,

        /// <summary>
        ///  The "F15" key.
        /// </summary>
        F15,

        /// <summary>
        ///  The "F16" key.
        /// </summary>
        F16,

        /// <summary>
        ///  The "F17" key.
        /// </summary>
        F17,

        /// <summary>
        ///  The "F18" key.
        /// </summary>
        F18,

        /// <summary>
        ///  The "F19" key.
        /// </summary>
        F19,

        /// <summary>
        ///  The "F20" key.
        /// </summary>
        F20,

        /// <summary>
        ///  The "F21" key.
        /// </summary>
        F21,

        /// <summary>
        ///  The "F22" key.
        /// </summary>
        F22,

        /// <summary>
        ///  The "F23" key.
        /// </summary>
        F23,

        /// <summary>
        ///  The "F24" key.
        /// </summary>
        F24,

        /// <summary>
        ///  The "Num Lock" key.
        /// </summary>
        NumLock,

        /// <summary>
        ///  The "Scroll Lock" key.
        /// </summary>
        ScrollLock,

        /// <summary>
        ///  The "Browser Back" key.
        /// </summary>
        BrowserBack,

        /// <summary>
        ///  The "Browser Forward" key.
        /// </summary>
        BrowserForward,

        /// <summary>
        ///  The "Browser Refresh" key.
        /// </summary>
        BrowserRefresh,

        /// <summary>
        ///  The "Browser Stop" key.
        /// </summary>
        BrowserStop,

        /// <summary>
        ///  The "Browser Search" key.
        /// </summary>
        BrowserSearch,

        /// <summary>
        ///  The "Browser Favorites" key.
        /// </summary>
        BrowserFavorites,

        /// <summary>
        ///  The "Browser Home" key.
        /// </summary>
        BrowserHome,

        /// <summary>
        ///  The "Volume Mute" key.
        /// </summary>
        VolumeMute,

        /// <summary>
        ///  The "Volume Down" key.
        /// </summary>
        VolumeDown,

        /// <summary>
        ///  The "Volume Up" key.
        /// </summary>
        VolumeUp,

        /// <summary>
        ///  The "Media Next Track" key.
        /// </summary>
        MediaNextTrack,

        /// <summary>
        ///  The "Media Previous Track" key.
        /// </summary>
        MediaPreviousTrack,

        /// <summary>
        ///  The "Media Stop" key.
        /// </summary>
        MediaStop,

        /// <summary>
        ///  The "Media Play Pause" key.
        /// </summary>
        MediaPlayPause,

        /// <summary>
        ///  The "Launch Mail" key.
        /// </summary>
        LaunchMail,

        /// <summary>
        ///  The "Select Media" key.
        /// </summary>
        SelectMedia,

        /// <summary>
        ///  The "Launch Application1" key.
        /// </summary>
        LaunchApplication1,

        /// <summary>
        ///  The "Launch Application2" key.
        /// </summary>
        LaunchApplication2,

        /// <summary>
        ///  The ";" key.
        /// </summary>
        Semicolon,

        /// <summary>
        ///  The "=" key.
        /// </summary>
        Equals,

        /// <summary>
        ///  The "," key.
        /// </summary>
        Comma,

        /// <summary>
        ///  The "-" key.
        /// </summary>
        Minus,

        /// <summary>
        ///  The "." key.
        /// </summary>
        Period,

        /// <summary>
        ///  The "/" key.
        /// </summary>
        Slash,

        /// <summary>
        ///  The "[" key.
        /// </summary>
        OpenBracket,

        /// <summary>
        ///  The "]" key.
        /// </summary>
        CloseBracket,

        /// <summary>
        ///  The "'" key.
        /// </summary>
        Quote,

        /// <summary>
        ///  The "\" key.
        /// </summary>
        Backslash,

        /// <summary>
        ///  The "Clear" key.
        /// </summary>
        Clear,

        /// <summary>
        ///  The "`" key.
        /// </summary>
        Backtick,

        /// <summary>
        ///  The "Shift" key.
        /// </summary>
        Shift,

        /// <summary>
        ///  The "Control" key.
        /// </summary>
        Control,

        /// <summary>
        ///  The "Alt" key on Windows and Linux or "Option" key on macOS.
        /// </summary>
        Alt,

        /// <summary>
        ///  The "Command" key on Apple keyboard.
        /// </summary>
        MacCommand,

        /// <summary>
        /// The "Option" key on Apple keyboard.
        /// </summary>
        MacOption,

        /// <summary>
        /// The "Control" key on Apple keyboard.
        /// </summary>
        MacControl,

        /// <summary>
        /// The Microsoft "Windows Logo" key on Windows or "Command" key on macOS or "Meta" key on Linux.
        /// </summary>
        Windows,

        /// <summary>
        ///  The Microsoft "Menu" key.
        /// </summary>
        Menu,
    }
}
