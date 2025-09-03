using System.ComponentModel;

using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    ///     An enumeration of all of the possible key values on a keyboard.
    /// </summary>
    [TypeConverter("Alternet.UI.KeyConverter")]
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
        ///  The ` key.
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
        ///  The '!' (33 0x21) key.
        /// </summary>
        ExclamationMark = 134,

        /// <summary>
        ///  The '"' (34 0x220) key.
        /// </summary>
        QuotationMark = 135,

        /// <summary>
        ///  The '#' (35 0x23) key.
        /// </summary>
        NumberSign = 136,

        /// <summary>
        ///  The '$' (36 0x24) key.
        /// </summary>
        DollarSign = 137,

        /// <summary>
        ///  The '%' (37 0x25) key.
        /// </summary>
        PercentSign = 138,

        /// <summary>
        ///  The ampersand (38 0x26) key.
        /// </summary>
        Ampersand = 139,

        /// <summary>
        ///  The '(' (40 0x28) key.
        /// </summary>
        LeftParenthesis = 140,

        /// <summary>
        ///  The ')' (41 0x29) key.
        /// </summary>
        RightParenthesis = 141,

        /// <summary>
        ///  The '*' (42 0x2A) key.
        /// </summary>
        Asterisk = 142,

        /// <summary>
        ///  The '+' (43 0x2B) key.
        /// </summary>
        PlusSign = 143,

        /// <summary>
        ///  The ':' (58 0x3A) key.
        /// </summary>
        Colon = 144,

        /// <summary>
        ///  The less than sign (60 0x3C) key.
        /// </summary>
        LessThanSign = 145,

        /// <summary>
        ///  The greater than sign (62 0x3E) key.
        /// </summary>
        GreaterThanSign = 146,

        /// <summary>
        ///  The '?' (63 0x3F) key.
        /// </summary>
        QuestionMark = 147,

        /// <summary>
        ///  The '@' (64 0x40) key.
        /// </summary>
        CommercialAt = 148,

        /// <summary>
        ///  The '^' (94 0x5E) key.
        /// </summary>
        CircumflexAccent = 149,

        /// <summary>
        ///  The '_' (95 0x5F) key.
        /// </summary>
        LowLine = 150,

        /// <summary>
        ///  The '{' 123 0x7B) key.
        /// </summary>
        LeftCurlyBracket = 151,

        /// <summary>
        ///  The '|' 124 0x7C) key.
        /// </summary>
        VerticalLine = 152,

        /// <summary>
        ///  The '}' 125 0x7D) key.
        /// </summary>
        RightCurlyBracket = 153,

        /// <summary>
        ///  The '~' 126 0x7E) key.
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
        ///     The IME non-convert button or key.
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

        /* MacOs */

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        AlternateErase = 199,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        SysReqOrAttention = 200,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        ClearOrAgain = 201,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        CrSelOrProps = 202,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        ExSel = 203,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// An alias for the LANG3 key on Japanese language keyboards.
        /// </summary>
        Katakana = 204,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// An alias for the LANG4 key on Japanese language keyboards.
        /// </summary>
        Hiragana = 205,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        LockingCapsLock = 206,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        LockingNumLock = 207,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        LockingScrollLock = 208,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Undo = 209,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Cut = 210,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Copy = 211,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Paste = 212,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Find = 213,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        NumPadEqualSignAS400 = 214,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Help = 215,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Power = 216,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        NonUSBackslash = 217,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International1 = 218,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International2 = 219,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International3 = 220,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International4 = 221,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International5 = 222,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International6 = 223,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International7 = 224,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International8 = 225,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        International9 = 226,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang1 = 227,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang2 = 228,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang3 = 229,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang4 = 230,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang5 = 231,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang6 = 232,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang7 = 233,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang8 = 234,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Lang9 = 235,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// An alias for the LANG2 key on Japanese language keyboards from Apple.
        /// </summary>
        AlphanumericSwitch = 236,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Separator = 237,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Out = 238,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Oper = 239,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        NonUSPound = 240,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Application = 241,

        /// <summary>
        /// Key mapped from UIKeyboardHidUsage enum value.
        /// </summary>
        Again = 242,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        LineFeed = 243,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        Oem8 = 244,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        ProcessKey = 245,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        Packet = 246,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        Attn = 247,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        EraseEof = 248,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        Zoom = 249,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        NoName = 250,

        /// <summary>
        /// Mapped from the corresponding <see cref="Keys"/> value.
        /// </summary>
        Pa1 = 251,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaAudioTrack,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Wakeup,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Pairing,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaTopMenu,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        K11,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        K12,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        LastChannel,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvDataService,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VoiceAssist,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvRadioService,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvTeletext,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvNumberEntry,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvTerrestrialAnalog,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvTerrestrialDigital,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvSatellite,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvSatelliteBs,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvSatelliteCs,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvSatelliteService,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvNetwork,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvAntennaCable,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputHdmi1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputHdmi2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputHdmi3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputHdmi4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputComposite1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputComposite2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputComponent1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputComponent2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvInputVga1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvAudioDescription,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvAudioDescriptionMixUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvAudioDescriptionMixDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvZoomMode,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvContentsMenu,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvMediaContextMenu,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        TvTimerProgramming,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        NavigatePrevious,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        NavigateNext,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        NavigateIn,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        NavigateOut,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        StemPrimary,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Stem1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Stem2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Stem3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DpadUpLeft,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DpadDownLeft,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DpadUpRight,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DpadDownRight,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaSkipForward,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaSkipBackward,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaStepForward,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaStepBackward,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        SoftSleep,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        SystemNavigationUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        SystemNavigationDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        SystemNavigationLeft,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        SystemNavigationRight,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        AllApps,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        ThumbsUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        ThumbsDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        ProfileSwitch,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp5,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp6,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp7,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        VideoApp8,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        FeaturedApp1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        FeaturedApp2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        FeaturedApp3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        FeaturedApp4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DemoApp1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DemoApp2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DemoApp3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        DemoApp4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        KeyboardBacklightDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        KeyboardBacklightUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        KeyboardBacklightToggle,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        StylusButtonPrimary,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        StylusButtonSecondary,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        StylusButtonTertiary,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        StylusButtonTail,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        RecentApps,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Macro1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Macro2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Macro3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Macro4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes toggles picture-in-picture mode
        /// or other windowing functions.
        /// </summary>
        TVWindow,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes shows a programming guide.
        /// </summary>
        TVGuide,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On some TV remotes switches to a DVR mode for recorded shows.
        /// </summary>
        TvDvr,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On some TV remotes bookmarks content or web pages.
        /// </summary>
        TVBookmark,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Toggle captions. Switches the mode for closed-captioning
        /// text for example during television shows.
        /// </summary>
        TVCaptions,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Starts the system settings activity.
        /// </summary>
        Settings,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes switches the input on a television screen.
        /// </summary>
        TVPower,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes switches the input on a television screen.
        /// </summary>
        TVInput,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes toggles the power on an external Set-top-box.
        /// </summary>
        TVStbPower,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes switches the input mode on an external Set-top-box.
        /// </summary>
        TVStbInput,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes toggles the power on an external A/V Receiver.
        /// </summary>
        TVAvrPower,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes switches the input mode on an external A/V Receiver.
        /// </summary>
        TVAvrInput,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Red "programmable". On TV remotes acts as a contextual/programmable.
        /// </summary>
        TVProgRed,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Green "programmable". On TV remotes acts as a contextual/programmable.
        /// </summary>
        TVProgGreen,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Yellow "programmable". On TV remotes acts as a contextual/programmable.
        /// </summary>
        TVProgYellow,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Blue "programmable". On TV remotes acts as a contextual/programmable.
        /// </summary>
        TVProgBlue,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        // App switch  Should bring up the application switcher dialog.
        AppSwitch,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 1.
        /// </summary>
        GamepadButton1,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton2,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton3,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton4,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton5,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton6,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton7,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton8,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton9,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton10,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton11,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton12,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton13,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton14,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton15,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Generic Game Pad Button 2.
        /// </summary>
        GamepadButton16,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Toggles the current input language such
        /// as switching between English and Japanese on a QWERTY keyboard.
        /// </summary>
        LanguageSwitch,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Toggles silent or vibrate mode on and off
        /// to make the device behave more politely in certain settings such as on a crowded train.
        /// </summary>
        MannerMode,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Toggles the display between 2D and 3D mode.
        /// </summary>
        ThreeDMode,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch an address book application.
        /// </summary>
        Contacts,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch a calendar application.
        /// </summary>
        Calendar,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch a music player application.
        /// </summary>
        Music,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch a calculator application.
        /// </summary>
        Calculator,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Japanese full-width / half-width
        /// </summary>
        JapaneseZenkakuHankaku,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Japanese alphanumeric
        /// </summary>
        JapaneseEisu,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Japanese non-conversion
        /// </summary>
        JapaneseMuhenkan,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Japanese conversion
        /// </summary>
        JapaneseHenkan,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        JapaneseYen,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        JapaneseRo,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Assist  Launches the global assist activity.
        /// </summary>
        Assist,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Adjusts the screen brightness down.
        /// </summary>
        BrightnessDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Adjusts the screen brightness up.
        /// </summary>
        BrightnessUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Numeric keypad '('
        /// </summary>
        NumPadLeftParen,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Numeric keypad ')'
        /// </summary>
        NumPadRightParen,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On a game controller the C button should be
        /// either the button labeled C or the third button on the bottom row of controller buttons.
        /// </summary>
        GamepadC,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On a game controller the Z button should be
        /// either the button labeled Z or the third button on the upper row of controller buttons.
        /// </summary>
        GamepadZ,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Function modifier.
        /// </summary>
        Function,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Common on TV remotes to show additional information
        /// related to what is currently being viewed.
        /// </summary>
        TVInfo,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes increments the television channel.
        /// </summary>
        TVChannelUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes decrements the television channel.
        /// </summary>
        TVChannelDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaRewind,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaFastForward,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Symbol modifier  Used to enter alternate symbols.
        /// </summary>
        Sym,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch a browser application.
        /// </summary>
        LaunchBrowser,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Mutes the microphone unlike Android.Views.Keycode.VolumeMute.
        /// </summary>
        Mute,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Picture Symbols modifier. Used to switch symbol sets (Emoji Kao-moji).
        /// </summary>
        Pictsymbols,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Switch Charset modifier. Used to switch character sets (Kanji Katakana).
        /// </summary>
        SwitchCharset,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Num,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to hang up calls and stop media.
        /// </summary>
        Headsethook,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to focus the camera.
        /// </summary>
        Focus,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Notification,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Call,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        Endcall,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Directional Pad Up. May also be synthesized from trackball motions.
        /// </summary>
        DpadUp,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Directional Pad Down. May also be synthesized from trackball motions.
        /// </summary>
        DpadDown,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Directional Pad Left. May also be synthesized from trackball motions.
        /// </summary>
        DpadLeft,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Directional Pad Right. May also be synthesized from trackball motions.
        /// </summary>
        DpadRight,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Directional Pad Center. May also be synthesized from trackball motions.
        /// </summary>
        DpadCenter,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Used to launch a camera application or take pictures.
        /// </summary>
        Camera,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// On TV remotes switches to viewing live TV.
        /// </summary>
        TvViewLive,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaPlay,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaPause,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// May be used to close a CD tray for example.
        /// </summary>
        MediaClose,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// Eject media. May be used to eject a CD tray for example.
        /// </summary>
        MediaEject,

        /// <summary>
        /// Mapped from the corresponding "Android.Views.Keycode" value.
        /// </summary>
        MediaRecord,

        /* max values */

        /// <summary>
        /// Max supported enum value when library is running on WxWidgets platform.
        /// </summary>
        MaxWxWidgets = Tilde,

        /// <summary>
        /// Max enum value.
        /// </summary>
        Max = MediaRecord,
    }
}

#pragma warning disable
namespace Alternet.UI.Markup
{
}
#pragma warning restore