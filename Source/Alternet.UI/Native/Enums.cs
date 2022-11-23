// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

namespace Alternet.UI.Native
{
    enum FileDialogMode
    {
        Open = 0,
        Save = 1,
    }
    
    enum DragAction
    {
        Continue = 0,
        Drop = 1,
        Cancel = 2,
    }
    
    enum DragDropEffects
    {
        None = 0,
        Copy = 1,
        Move = 2,
        Link = 4,
    }
    
    enum BrushHatchStyle
    {
        BackwardDiagonal = 0,
        ForwardDiagonal = 1,
        DiagonalCross = 2,
        Cross = 3,
        Horizontal = 4,
        Vertical = 5,
    }
    
    enum FillMode
    {
        Alternate = 0,
        Winding = 1,
    }
    
    enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underlined = 4,
        Strikethrough = 8,
    }
    
    enum GenericFontFamily
    {
        None = 0,
        SansSerif = 1,
        Serif = 2,
        Monospace = 3,
    }
    
    enum InterpolationMode
    {
        None = 0,
        LowQuality = 1,
        MediumQuality = 2,
        HighQuality = 3,
    }
    
    enum LineCap
    {
        Flat = 0,
        Round = 1,
    }
    
    enum LineJoin
    {
        Miter = 0,
        Bevel = 1,
        Round = 2,
    }
    
    enum PenDashStyle
    {
        Solid = 0,
        Dot = 1,
        Dash = 2,
        DashDot = 3,
        Custom = 4,
    }
    
    enum Duplex
    {
        Simplex = 0,
        Vertical = 1,
        Horizontal = 2,
    }
    
    enum PaperKind
    {
        Letter = 0,
        Legal = 1,
        A4 = 2,
        C = 3,
        D = 4,
        E = 5,
        LetterSmall = 6,
        Tabloid = 7,
        Ledger = 8,
        Statement = 9,
        Executive = 10,
        A3 = 11,
        A4mall = 12,
        A5 = 13,
        B4 = 14,
        B5 = 15,
        Folio = 16,
        Quarto = 17,
        Sheet10X14 = 18,
        Sheet11X17 = 19,
        Note = 20,
        Envelope9 = 21,
        Envelope10 = 22,
        Envelope11 = 23,
        Envelope12 = 24,
        Envelope14 = 25,
        EnvelopeDl = 26,
        EnvelopeC5 = 27,
        EnvelopeC3 = 28,
        EnvelopeC4 = 29,
        EnvelopeC6 = 30,
        EnvelopeC65 = 31,
        EnvelopeB4 = 32,
        EnvelopeB5 = 33,
        EnvelopeB6 = 34,
        EnvelopeItaly = 35,
        EnvelopeMonarch = 36,
        EnvelopePersonal = 37,
        FanfoldUs = 38,
        FanfoldStanardGerman = 39,
        FanfoldLegalGerman = 40,
        IsoB4 = 41,
        JapanesePostcard = 42,
        Sheet9X11 = 43,
        Sheet10X11 = 44,
        Sheet15X11 = 45,
        EnvelopeInvite = 46,
        LetterExtra = 47,
        LegalExtra = 48,
        TabloidExtra = 49,
        A4Extra = 50,
        LetterTransverse = 51,
        A4Transverse = 52,
        LetterExtraTransverse = 53,
        APlus = 54,
        BPlus = 55,
        LetterPlus = 56,
        A4Plus = 57,
        A5Transverse = 58,
        B5Transverse = 59,
        A3Extra = 60,
        A5Extra = 61,
        B5Extra = 62,
        A2 = 63,
        A3Transverse = 64,
        A3ExtraTransverse = 65,
        DblJapanesePostcard = 66,
        A6 = 67,
        JapaneseEnvelopeKaku2 = 68,
        JapaneseEnvelopeKaku3 = 69,
        JapaneseEnvelopeChou3 = 70,
        JapaneseEnvelopeChou4 = 71,
        LetterRotated = 72,
        A3Rotated = 73,
        A4Rotated = 74,
        A5Rotated = 75,
        B4JisRotated = 76,
        B5JisRotated = 77,
        JapanesePostcardRotated = 78,
        DblJapanesePostcardRotated = 79,
        A6Rotated = 80,
        JapaneseEnvelopeKaku2Rotated = 81,
        JapaneseEnvelopeKaku3Rotated = 82,
        JapaneseEnvelopeChou3Rotated = 83,
        JapaneseEnvelopeChou4Rotated = 84,
        B6Jis = 85,
        B6JisRotated = 86,
        Sheet12X11 = 87,
        JapaneseEnvelopeYou4 = 88,
        JapaneseEnvelopeYou4Rotated = 89,
        P16k = 90,
        P32k = 91,
        P32kbig = 92,
        PrcEnvelope1 = 93,
        PrcEnvelope2 = 94,
        PrcEnvelope3 = 95,
        PrcEnvelope4 = 96,
        PrcEnvelope5 = 97,
        PrcEnvelope6 = 98,
        PrcEnvelope7 = 99,
        PrcEnvelope8 = 100,
        PrcEnvelope9 = 101,
        PrcEnvelope10 = 102,
        P16kRotated = 103,
        P32kRotated = 104,
        P32kbigRotated = 105,
        PrcEnvelope1Rotated = 106,
        PrcEnvelope2Rotated = 107,
        PrcEnvelope3Rotated = 108,
        PrcEnvelope4Rotated = 109,
        PrcEnvelope5Rotated = 110,
        PrcEnvelope6Rotated = 111,
        PrcEnvelope7Rotated = 112,
        PrcEnvelope8Rotated = 113,
        PrcEnvelope9Rotated = 114,
        PrcEnvelope10Rotated = 115,
        A0 = 116,
        A1 = 117,
    }
    
    enum PrinterResolutionKind
    {
        Draft = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }
    
    enum PrintRange
    {
        AllPages = 0,
        Selection = 1,
        SomePages = 2,
    }
    
    enum TextHorizontalAlignment
    {
        Left = 0,
        Center = 1,
        Right = 2,
    }
    
    enum TextTrimming
    {
        None = 0,
        Pixel = 1,
        Character = 2,
    }
    
    enum TextVerticalAlignment
    {
        Top = 0,
        Center = 1,
        Bottom = 2,
    }
    
    enum TextWrapping
    {
        None = 0,
        Character = 1,
        Word = 2,
    }
    
    enum Key
    {
        None = 0,
        Backspace = 1,
        Tab = 2,
        Enter = 3,
        Pause = 4,
        CapsLock = 5,
        Escape = 6,
        Space = 7,
        PageUp = 8,
        PageDown = 9,
        End = 10,
        Home = 11,
        LeftArrow = 12,
        UpArrow = 13,
        RightArrow = 14,
        DownArrow = 15,
        PrintScreen = 16,
        Insert = 17,
        Delete = 18,
        D0 = 19,
        D1 = 20,
        D2 = 21,
        D3 = 22,
        D4 = 23,
        D5 = 24,
        D6 = 25,
        D7 = 26,
        D8 = 27,
        D9 = 28,
        A = 29,
        B = 30,
        C = 31,
        D = 32,
        E = 33,
        F = 34,
        G = 35,
        H = 36,
        I = 37,
        J = 38,
        K = 39,
        L = 40,
        M = 41,
        N = 42,
        O = 43,
        P = 44,
        Q = 45,
        R = 46,
        S = 47,
        T = 48,
        U = 49,
        V = 50,
        W = 51,
        X = 52,
        Y = 53,
        Z = 54,
        NumPad0 = 55,
        NumPad1 = 56,
        NumPad2 = 57,
        NumPad3 = 58,
        NumPad4 = 59,
        NumPad5 = 60,
        NumPad6 = 61,
        NumPad7 = 62,
        NumPad8 = 63,
        NumPad9 = 64,
        NumPadStar = 65,
        NumPadPlus = 66,
        NumPadMinus = 67,
        NumPadDot = 68,
        NumPadSlash = 69,
        F1 = 70,
        F2 = 71,
        F3 = 72,
        F4 = 73,
        F5 = 74,
        F6 = 75,
        F7 = 76,
        F8 = 77,
        F9 = 78,
        F10 = 79,
        F11 = 80,
        F12 = 81,
        F13 = 82,
        F14 = 83,
        F15 = 84,
        F16 = 85,
        F17 = 86,
        F18 = 87,
        F19 = 88,
        F20 = 89,
        F21 = 90,
        F22 = 91,
        F23 = 92,
        F24 = 93,
        NumLock = 94,
        ScrollLock = 95,
        BrowserBack = 96,
        BrowserForward = 97,
        BrowserRefresh = 98,
        BrowserStop = 99,
        BrowserSearch = 100,
        BrowserFavorites = 101,
        BrowserHome = 102,
        VolumeMute = 103,
        VolumeDown = 104,
        VolumeUp = 105,
        MediaNextTrack = 106,
        MediaPreviousTrack = 107,
        MediaStop = 108,
        MediaPlayPause = 109,
        LaunchMail = 110,
        SelectMedia = 111,
        LaunchApplication1 = 112,
        LaunchApplication2 = 113,
        Semicolon = 114,
        Equals = 115,
        Comma = 116,
        Minus = 117,
        Period = 118,
        Slash = 119,
        OpenBracket = 120,
        CloseBracket = 121,
        Quote = 122,
        Backslash = 123,
        Clear = 124,
        Backtick = 125,
        Shift = 126,
        Control = 127,
        Alt = 128,
        MacCommand = 129,
        MacOption = 130,
        MacControl = 131,
        Windows = 132,
        Menu = 133,
    }
    
    enum KeyStates
    {
        None = 0,
        Down = 1,
        Toggled = 2,
    }
    
    enum ModifierKeys
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
    }
    
    enum ListBoxSelectionMode
    {
        Single = 0,
        Multiple = 1,
    }
    
    enum ListViewColumnWidthMode
    {
        Fixed = 0,
        AutoSize = 1,
        AutoSizeHeader = 2,
    }
    
    enum ListViewGridLinesDisplayMode
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        VerticalAndHorizontal = 3,
    }
    
    enum ListViewHitTestLocations
    {
        None = 2,
        AboveClientArea = 4,
        BelowClientArea = 8,
        LeftOfClientArea = 16,
        RightOfClientArea = 32,
        ItemImage = 64,
        ItemLabel = 128,
        RightOfItem = 256,
    }
    
    enum ListViewItemBoundsPortion
    {
        EntireItem = 0,
        Icon = 1,
        Label = 2,
    }
    
    enum ListViewSelectionMode
    {
        Single = 0,
        Multiple = 1,
    }
    
    enum ListViewSortMode
    {
        None = 0,
        Ascending = 1,
        Descending = 2,
        Custom = 3,
    }
    
    enum ListViewView
    {
        List = 0,
        Details = 1,
        SmallIcon = 2,
        LargeIcon = 3,
    }
    
    enum MessageBoxResult
    {
        OK = 0,
        Cancel = 1,
        Yes = 2,
        No = 3,
    }
    
    enum MessageBoxDefaultButton
    {
        OK = 0,
        Cancel = 1,
        Yes = 2,
        No = 3,
    }
    
    enum MessageBoxButtons
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 2,
        YesNo = 3,
    }
    
    enum MessageBoxIcon
    {
        None = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
    }
    
    enum MouseButton
    {
        Left = 0,
        Middle = 1,
        Right = 2,
        XButton1 = 3,
        XButton2 = 4,
    }
    
    enum MouseButtonState
    {
        Released = 0,
        Pressed = 1,
    }
    
    enum ProgressBarOrientation
    {
        Horizontal = 0,
        Vertical = 1,
    }
    
    enum SliderOrientation
    {
        Horizontal = 0,
        Vertical = 1,
    }
    
    enum SliderTickStyle
    {
        None = 0,
        TopLeft = 1,
        BottomRight = 2,
        Both = 3,
    }
    
    enum TabAlignment
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3,
    }
    
    enum TreeViewHitTestLocations
    {
        None = 2,
        AboveClientArea = 4,
        BelowClientArea = 8,
        LeftOfClientArea = 16,
        RightOfClientArea = 32,
        ItemExpandButton = 64,
        ItemImage = 128,
        ItemIndent = 256,
        ItemLabel = 512,
        RightOfItemLabel = 1024,
        ItemStateImage = 2048,
        ItemUpperPart = 4096,
        ItemLowerPart = 8192,
    }
    
    enum TreeViewSelectionMode
    {
        Single = 0,
        Multiple = 1,
    }
    
    enum ModalResult
    {
        None = 0,
        Canceled = 1,
        Accepted = 2,
    }
    
    enum WindowStartLocation
    {
        Default = 0,
        Manual = 1,
        CenterScreen = 2,
        CenterOwner = 3,
    }
    
    enum WindowState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2,
    }
    
}