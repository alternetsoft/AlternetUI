using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates key codes for the WxWidgets platform.
    /// </summary>
    public enum WxWidgetsKeyCode
    {
        None = 0,

        ControlA = 1,
        ControlB,
        ControlC,
        ControlD,
        ControlE,
        ControlF,
        ControlG,
        ControlH,
        ControlI,
        ControlJ,
        ControlK,
        ControlL,
        ControlM,
        ControlN,
        ControlO,
        ControlP,
        ControlQ,
        ControlR,
        ControlS,
        ControlT,
        ControlU,
        ControlV,
        ControlW,
        ControlX,
        ControlY,
        ControlZ,

        Back = 8, /* backspace */
        Tab = 9,
        Return = 13,
        Escape = 27,

        /* values from 33 to 126 are reserved for the standard ASCII characters */

        Space = 32,

        ExclamationMark = 33, // ! 
        DoubleQuotes = 34, // "  ; Quotation mark ; speech marks
        NumberSign = 35, // # 
        DollarSign = 36, // $ 
        PercentSign = 37, // % 
        Ampersand = 38, // & 
        SingleQuote = 39, // ' or Apostrophe
        OpeningRoundBracket = 40, // round brackets or parentheses, opening round bracket
        ClosingRoundBracket = 41, // parentheses or round brackets, closing parentheses
        Asterisk = 42, // * 
        PlusSign = 43, // + 
        Comma = 44, // , 
        MinusSign = 45, // - Hyphen , minus sign
        Dot = 46, // . full stop
        Slash = 47, // / forward slash , fraction bar , division slash
        D0 = 48, // 0 number zero
        D1 = 49, // 1 number one
        D2 = 50, // 2 number two
        D3 = 51, // 3 number three
        D4 = 52, // 4 number four
        D5 = 53, // 5 number five
        D6 = 54, // 6 number six
        D7 = 55, // 7 number seven
        D8 = 56, // 8 number eight
        D9 = 57, // 9 number nine
        Colon = 58, // : 
        Semicolon = 59, // ;
        LessThanSign = 60, // < 
        EqualsSign = 61, //= 
        GreaterThanSign = 62, // >
        QuestionMark = 63, // ? 
        AtSign = 64, // @ 
        A = 65, // A Capital letter A
        B = 66, // B Capital letter B
        C = 67, // C Capital letter C
        D = 68, // D Capital letter D
        E = 69, // E Capital letter E
        F = 70, // F Capital letter F
        G = 71, // G Capital letter G
        H = 72, // H Capital letter H
        I = 73, // I Capital letter I
        J = 74, // J Capital letter J
        K = 75, // K Capital letter K
        L = 76, // L Capital letter L
        M = 77, // M Capital letter M
        N = 78, // N Capital letter N
        O = 79, // O Capital letter O
        P = 80, // P Capital letter P
        Q = 81, // Q Capital letter Q
        R = 82, // R Capital letter R
        S = 83, // S Capital letter S
        T = 84, // T Capital letter T
        U = 85, // U Capital letter U
        V = 86, // V Capital letter V
        W = 87, // W Capital letter W
        X = 88, // X Capital letter X
        Y = 89, // Y Capital letter Y
        Z = 90, // Z Capital letter Z
        OpeningSquareBracket = 91, // [ (square brackets or box brackets, opening bracket)
        Backslash = 92, // \ reverse slash
        ClosingSquareBracket = 93, // ] (box brackets or square brackets, closing bracket
        CircumflexAccent = 94, // ^ CircumflexAccent or Caret
        Underscore = 95, // _  understrike, underbar or low line
        GraveAccent = 96, // ` 
        LowerA = 97, // a Lowercase letter a , minuscule a
        LowerB = 98, // b Lowercase letter b , minuscule b
        LowerC = 99, // c Lowercase letter c , minuscule c
        LowerD = 100, // d Lowercase letter d , minuscule d
        LowerE = 101, // e Lowercase letter e , minuscule e
        LowerF = 102, // f Lowercase letter f , minuscule f
        LowerG = 103, // g Lowercase letter g , minuscule g
        LowerH = 104, // h Lowercase letter h , minuscule h
        LowerI = 105, // i Lowercase letter i , minuscule i
        LowerJ = 106, // j Lowercase letter j , minuscule j
        LowerK = 107, // k Lowercase letter k , minuscule k
        LowerL = 108, // l Lowercase letter l , minuscule l
        LowerM = 109, // m Lowercase letter m , minuscule m
        LowerN = 110, // n Lowercase letter n , minuscule n
        LowerO = 111, // o Lowercase letter o , minuscule o
        LowerP = 112, // p Lowercase letter p , minuscule p
        LowerQ = 113, // q Lowercase letter q , minuscule q
        LowerR = 114, // r Lowercase letter r , minuscule r
        LowerS = 115, // s Lowercase letter s , minuscule s
        LowerT = 116, // t Lowercase letter t , minuscule t
        LowerU = 117, // u Lowercase letter u , minuscule u
        LowerV = 118, // v Lowercase letter v , minuscule v
        LowerW = 119, // w Lowercase letter w , minuscule w
        LowerX = 120, // x Lowercase letter x , minuscule x
        LowerY = 121, // y Lowercase letter y , minuscule y
        LowerZ = 122, // z Lowercase letter z , minuscule z
        OpeningCurlyBracket = 123, // { braces or curly brackets, opening braces
        VerticalBar = 124, // | , vbar, vertical line or vertical slash
        ClosingCurlyBracket = 125, // } (curly brackets or braces, closing curly brackets
        Tilde = 126, // ~ swung dash

        Delete = 127,

        /* values from 128 to 255 are reserved for ASCII extended characters
           (note that there isn't a single fixed standard for the meaning
           of these values; avoid them in portable apps!) */

        /* These are not compatible with unicode characters.
           If you want to get a unicode character from a key event, use
           wxKeyEvent::GetUnicodeKey                                    */
        Start = 300,
        LButton,
        RButton,
        Cancel,
        MButton,
        Clear,
        Shift,
        Alt,
        Control,
        Menu,
        Pause,
        Capital,
        End,
        Home,
        Left,
        Up,
        Right,
        Down,
        Select,
        Print,
        Execute,
        Snapshot,
        Insert,
        Help,
        Numpad0,
        Numpad1,
        Numpad2,
        Numpad3,
        Numpad4,
        Numpad5,
        Numpad6,
        Numpad7,
        Numpad8,
        Numpad9,
        Multiply,
        Add,
        Separator,
        Subtract,
        Decimal,
        Divide,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        NumLock,
        Scroll,
        PageUp,
        PageDown,
        NumpadSpace,
        NumpadTab,
        NumpadEnter,
        NumpadF1,
        NumpadF2,
        NumpadF3,
        NumpadF4,
        NumpadHome,
        NumpadLeft,
        NumpadUp,
        NumpadRight,
        NumpadDown,
        NumpadPageUp,
        NumpadPageDown,
        NumpadEnd,
        NumpadBegin,
        NumpadInsert,
        NumpadDelete,
        NumpadEqual,
        NumpadMultiply,
        NumpadAdd,
        NumpadSeparator,
        NumpadSubtract,
        NumpadDecimal,
        NumpadDivide,

        WindowsLeft,
        WindowsRight,
        WindowsMenu,

        RawControlMacOs = WindowsMenu + 1,

        RawControl = Control,

        Command = Control,

        /* Hardware-specific buttons */
        Special1 = WindowsMenu + 2, /* Skip RAW_CONTROL if necessary */
        Special2,
        Special3,
        Special4,
        Special5,
        Special6,
        Special7,
        Special8,
        Special9,
        Special10,
        Special11,
        Special12,
        Special13,
        Special14,
        Special15,
        Special16,
        Special17,
        Special18,
        Special19,
        Special20,

        BrowserBack,
        BrowserForward,
        BrowserRefresh,
        BrowserStop,
        BrowserSearch,
        BrowserFavorites,
        BrowserHome,
        VolumeMute,
        VolumeDown,
        VolumeUp,
        MediaNextTrack,
        MediaPrevTrack,
        MediaStop,
        MediaPlayPause,
        LaunchMail,

        // Events for these keys are currently only generated by wxGTK, with the
        // exception of Launch{A,B}, see LaunchAPP{1,2} below.
        Launch0,
        Launch1,
        Launch2,
        Launch3,
        Launch4,
        Launch5,
        Launch6,
        Launch7,
        Launch8,
        Launch9,
        LaunchA,
        LaunchB,
        LaunchC,
        LaunchD,
        LaunchE,
        LaunchF,

        // These constants are the same as the corresponding GTK keys, so give them
        // the same value, but they are also generated by wxMSW.
        LaunchApp1 = LaunchA,
        LaunchApp2 = LaunchB,
    };
}