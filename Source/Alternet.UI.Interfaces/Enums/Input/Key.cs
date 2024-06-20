using System.ComponentModel;

using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    ///     An enumeration of all of the possible key values on a keyboard.
    /// </summary>
    [TypeConverter("Alternet.UI.KeyConverter")]
    [ValueSerializer("Alternet.UI.KeyValueSerializer")]
    public enum Key
    {
        /// <summary>
        ///  No key pressed.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The "Backspace" key.
        /// </summary>
        Backspace = 1,

        /// <summary>
        ///  The "Tab" key.
        /// </summary>
        Tab = 2,

        /// <summary>
        ///  The "Enter" key.
        /// </summary>
        Enter = 3,

        /// <summary>
        ///  The "Pause" key.
        /// </summary>
        Pause = 4,

        /// <summary>
        ///  The "Caps Lock" key.
        /// </summary>
        CapsLock = 5,

        /// <summary>
        ///  The "Esc" key.
        /// </summary>
        Escape = 6,

        /// <summary>
        ///  The "Space Bar" key.
        /// </summary>
        Space = 7,

        /// <summary>
        ///  The "Page Up" key.
        /// </summary>
        PageUp = 8,

        /// <summary>
        ///  The "Page Down" key.
        /// </summary>
        PageDown = 9,

        /// <summary>
        ///  The "End" key.
        /// </summary>
        End = 10,

        /// <summary>
        ///  The "Home" key.
        /// </summary>
        Home = 11,

        /// <summary>
        ///  The "Left Arrow" key.
        /// </summary>
        LeftArrow = 12,

        /// <summary>
        ///  The "Up Arrow" key.
        /// </summary>
        UpArrow = 13,

        /// <summary>
        ///  The "Right Arrow" key.
        /// </summary>
        RightArrow = 14,

        /// <summary>
        ///  The "Down Arrow" key.
        /// </summary>
        DownArrow = 15,

        /// <summary>
        ///  The "Print Screen" key.
        /// </summary>
        PrintScreen = 16,

        /// <summary>
        ///  The "Insert" key.
        /// </summary>
        Insert = 17,

        /// <summary>
        ///  The "Delete" key.
        /// </summary>
        Delete = 18,

        /// <summary>
        ///  The "0" key.
        /// </summary>
        D0 = 19,

        /// <summary>
        ///  The "1" key.
        /// </summary>
        D1 = 20,

        /// <summary>
        ///  The "2" key.
        /// </summary>
        D2 = 21,

        /// <summary>
        ///  The "3" key.
        /// </summary>
        D3 = 22,

        /// <summary>
        ///  The "4" key.
        /// </summary>
        D4 = 23,

        /// <summary>
        ///  The "5" key.
        /// </summary>
        D5 = 24,

        /// <summary>
        ///  The "6" key.
        /// </summary>
        D6 = 25,

        /// <summary>
        ///  The "7" key.
        /// </summary>
        D7 = 26,

        /// <summary>
        ///  The "8" key.
        /// </summary>
        D8 = 27,

        /// <summary>
        ///  The "9" key.
        /// </summary>
        D9 = 28,

        /// <summary>
        ///  The "A" key.
        /// </summary>
        A = 29,

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
        /// The Microsoft "Windows Logo" key on Windows or "Command"
        /// key on macOS or "Meta" key on Linux.
        /// </summary>
        Windows,

        /// <summary>
        ///  The Microsoft "Menu" key.
        /// </summary>
        Menu = 133,

        /// <summary>
        ///  The '!' (33, 0x21) key.
        /// </summary>
        ExclamationMark = 134,

        /// <summary>
        ///  The '"' (34, 0x220) key.
        /// </summary>
        QuotationMark = 135,

        /// <summary>
        ///  The '#' (35, 0x23) key.
        /// </summary>
        NumberSign = 136,

        /// <summary>
        ///  The '$' (36, 0x24) key.
        /// </summary>
        DollarSign = 137,

        /// <summary>
        ///  The '%' (37, 0x25) key.
        /// </summary>
        PercentSign = 138,

        /// <summary>
        ///  The ampersand (38, 0x26) key.
        /// </summary>
        Ampersand = 139,

        /// <summary>
        ///  The '(' (40, 0x28) key.
        /// </summary>
        LeftParenthesis = 140,

        /// <summary>
        ///  The ')' (41, 0x29) key.
        /// </summary>
        RightParenthesis = 141,

        /// <summary>
        ///  The '*' (42, 0x2A) key.
        /// </summary>
        Asterisk = 142,

        /// <summary>
        ///  The '+' (43, 0x2B) key.
        /// </summary>
        PlusSign = 143,

        /// <summary>
        ///  The ':' (58, 0x3A) key.
        /// </summary>
        Colon = 144,

        /// <summary>
        ///  The less than sign (60, 0x3C) key.
        /// </summary>
        LessThanSign = 145,

        /// <summary>
        ///  The greater than sign (62, 0x3E) key.
        /// </summary>
        GreaterThanSign = 146,

        /// <summary>
        ///  The '?' (63, 0x3F) key.
        /// </summary>
        QuestionMark = 147,

        /// <summary>
        ///  The '@' (64, 0x40) key.
        /// </summary>
        CommercialAt = 148,

        /// <summary>
        ///  The '^' (94, 0x5E) key.
        /// </summary>
        CircumflexAccent = 149,

        /// <summary>
        ///  The '_' (95, 0x5F) key.
        /// </summary>
        LowLine = 150,

        /// <summary>
        ///  The '{', 123, 0x7B) key.
        /// </summary>
        LeftCurlyBracket = 151,

        /// <summary>
        ///  The '|', 124, 0x7C) key.
        /// </summary>
        VerticalLine = 152,

        /// <summary>
        ///  The '}', 125, 0x7D) key.
        /// </summary>
        RightCurlyBracket = 153,

        /// <summary>
        ///  The '~', 126, 0x7E) key.
        /// </summary>
        Tilde = 154,

        /* These are legacy names of keys */

        /// <summary>
        /// Same as <see cref="RightArrow"/>.
        /// </summary>
        Right = RightArrow,

        /// <summary>
        /// Same as <see cref="UpArrow"/>.
        /// </summary>
        Up = UpArrow,

        /// <summary>
        /// Same as <see cref="DownArrow"/>.
        /// </summary>
        Down = DownArrow,

        /// <summary>
        /// Same as <see cref="LeftArrow"/>.
        /// </summary>
        Left = LeftArrow,

        /// <summary>
        /// Same as <see cref="Period"/>.
        /// </summary>
        OemPeriod = Period,

        /// <summary>
        /// Same as <see cref="Backspace"/>.
        /// </summary>
        Back = Backspace,

        /// <summary>
        /// Same as <see cref="Enter"/>.
        /// </summary>
        Return = Enter,

        /// <summary>
        /// Same as <see cref="PageUp"/>.
        /// </summary>
        Prior = PageUp,

        /// <summary>
        /// Same as <see cref="PageDown"/>.
        /// </summary>
        Next = PageDown,

        /// <summary>
        /// Same as <see cref="CloseBracket"/>.
        /// </summary>
        OemCloseBrackets = CloseBracket,

        /// <summary>
        /// Same as <see cref="OpenBracket"/>.
        /// </summary>
        OemOpenBrackets = OpenBracket,

        /// <summary>
        /// The OEM pipe key on a US standard keyboard.
        /// The pipe symbol (|) resembles a vertical line.
        /// What is it exactly? Currently mapped as VerticalLine.
        /// </summary>
        OemPipe = VerticalLine,

        /// <summary>
        ///     The gamepad A button.
        /// </summary>
        GamepadA = 155,

        /// <summary>
        ///     The gamepad B button.
        /// </summary>
        GamepadB = 156,

        /// <summary>
        ///     The gamepad X button.
        /// </summary>
        GamepadX = 157,

        /// <summary>
        ///     The gamepad Y button.
        /// </summary>
        GamepadY = 158,

        /// <summary>
        ///     The gamepad right shoulder.
        /// </summary>
        GamepadRightShoulder = 159,

        /// <summary>
        ///     The gamepad left shoulder.
        /// </summary>
        GamepadLeftShoulder = 160,

        /// <summary>
        ///     The gamepad left trigger.
        /// </summary>
        GamepadLeftTrigger = 161,

        /// <summary>
        ///     The gamepad right trigger.
        /// </summary>
        GamepadRightTrigger = 162,

        /// <summary>
        ///     The gamepad d-pad up.
        /// </summary>
        GamepadDPadUp = 163,

        /// <summary>
        ///     The gamepad d-pad down.
        /// </summary>
        GamepadDPadDown = 164,

        /// <summary>
        ///     The gamepad d-pad left.
        /// </summary>
        GamepadDPadLeft = 165,

        /// <summary>
        ///     The gamepad d-pad right.
        /// </summary>
        GamepadDPadRight = 166,

        /// <summary>
        ///     The gamepad menu button.
        /// </summary>
        GamepadMenu = 167,

        /// <summary>
        ///     The gamepad view button.
        /// </summary>
        GamepadView = 168,

        /// <summary>
        ///     The gamepad left thumbstick button.
        /// </summary>
        GamepadLeftThumbstickButton = 169,

        /// <summary>
        ///     The gamepad right thumbstick button.
        /// </summary>
        GamepadRightThumbstickButton = 170,

        /// <summary>
        ///     The gamepad left thumbstick up.
        /// </summary>
        GamepadLeftThumbstickUp = 171,

        /// <summary>
        ///     The gamepad left thumbstick down.
        /// </summary>
        GamepadLeftThumbstickDown = 172,

        /// <summary>
        ///     The gamepad left thumbstick right.
        /// </summary>
        GamepadLeftThumbstickRight = 173,

        /// <summary>
        ///     The gamepad left thumbstick left.
        /// </summary>
        GamepadLeftThumbstickLeft = 174,

        /// <summary>
        ///     The gamepad right thumbstick up.
        /// </summary>
        GamepadRightThumbstickUp = 175,

        /// <summary>
        ///     The gamepad right thumbstick down.
        /// </summary>
        GamepadRightThumbstickDown = 176,

        /// <summary>
        ///     The gamepad right thumbstick right.
        /// </summary>
        GamepadRightThumbstickRight = 177,

        /// <summary>
        ///     The gamepad right thumbstick left.
        /// </summary>
        GamepadRightThumbstickLeft = 178,

        /// <summary>
        ///     The Kana symbol key-shift button
        /// </summary>
        Kana = 179,

        /// <summary>
        ///   The Hangul symbol key-shift button.
        /// </summary>
        Hangul = Kana,

        /// <summary>
        ///     The Junja symbol key-shift button.
        /// </summary>
        Junja = 180,

        /// <summary>
        ///     The Final symbol key-shift button.
        /// </summary>
        Final = 181,

        /// <summary>
        ///     The Hanja symbol key shift button.
        /// </summary>
        Hanja = 182,

        /// <summary>
        ///     The Kanji symbol key-shift button.
        /// </summary>
        Kanji = Hanja,

        /// <summary>
        ///     The IME convert button or key.
        /// </summary>
        Convert = 183,

        /// <summary>
        ///     The IME nonconvert button or key.
        /// </summary>
        NonConvert = 184,

        /// <summary>
        ///     The IME accept button or key.
        /// </summary>
        Accept = 185,

        /// <summary>
        ///     The IME mode change key.
        /// </summary>
        ModeChange = 186,

        /// <summary>
        ///     The Select key or button.
        /// </summary>
        Select = 187,

        /// <summary>
        ///     The execute key or button.
        /// </summary>
        Execute = 188,

        /// <summary>
        ///     The snapshot key or button.
        /// </summary>
        Snapshot = 189,

        /// <summary>
        ///     The sleep key or button.
        /// </summary>
        Sleep = 190,

        /// <summary>
        ///     The navigation up button.
        /// </summary>
        NavigationView = 191,

        /// <summary>
        ///     The navigation menu button.
        /// </summary>
        NavigationMenu = 192,

        /// <summary>
        ///     The navigation up button.
        /// </summary>
        NavigationUp = 193,

        /// <summary>
        ///     The navigation down button.
        /// </summary>
        NavigationDown = 194,

        /// <summary>
        ///     The navigation left button.
        /// </summary>
        NavigationLeft = 195,

        /// <summary>
        ///     The navigation right button.
        /// </summary>
        NavigationRight = 196,

        /// <summary>
        ///     The navigation accept button.
        /// </summary>
        NavigationAccept = 197,

        /// <summary>
        ///     The navigation cancel button.
        /// </summary>
        NavigationCancel = 198,

        /* max values */

        /// <summary>
        /// Max supported enum value when library is running on WxWidgets platform.
        /// </summary>
        MaxWxWidgets = Tilde,

        /// <summary>
        /// Max supported enum value when library is running on Maui platform.
        /// </summary>
        MaxMaui = NavigationCancel,

        /// <summary>
        /// Max enum value.
        /// </summary>
        Max = MaxMaui,
    }
}
