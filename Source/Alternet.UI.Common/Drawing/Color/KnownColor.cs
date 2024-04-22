// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the known system colors.
    /// </summary>
    public enum KnownColor
    {
        // This enum is order dependent!!!
        // The value of these known colors are indexes into a color array.
        // Do not modify this enum without updating KnownColorTable.
        // 0 - reserved for "not a known color"

        // "System" colors, Part 1

        /// <summary>
        /// The system-defined color of the active window's border.
        /// </summary>
        ActiveBorder = 1,

        /// <summary>
        /// The system-defined color of the background of the active window's title bar.
        /// </summary>
        ActiveCaption = 2,

        /// <summary>
        /// The system-defined color of the text in the active window's title bar.
        /// </summary>
        ActiveCaptionText = 3,

        /// <summary>
        /// The system-defined color of the application workspace. The application workspace
        /// is the area in a multiple-document view that is not being occupied by documents.
        /// </summary>
        AppWorkspace = 4,

        /// <summary>
        /// The system-defined face color of a 3-D element.
        /// </summary>
        Control = 5,

        /// <summary>
        /// The system-defined shadow color of a 3-D element. The shadow color is applied
        /// to parts of a 3-D element that face away from the light source.
        /// </summary>
        ControlDark = 6,

        /// <summary>
        /// The system-defined color that is the dark shadow color of a 3-D element. The
        /// dark shadow color is applied to the parts of a 3-D element that are the darkest color.
        /// </summary>
        ControlDarkDark = 7,

        /// <summary>
        /// The system-defined color that is the light color of a 3-D element. The light
        /// color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ControlLight = 8,

        /// <summary>
        /// The system-defined highlight color of a 3-D element. The highlight color is
        /// applied to the parts of a 3-D element that are the lightest color.
        /// </summary>
        ControlLightLight = 9,

        /// <summary>
        /// The system-defined color of text in a 3-D element.
        /// </summary>
        ControlText = 10,

        /// <summary>
        /// The system-defined color of the desktop.
        /// </summary>
        Desktop = 11,

        /// <summary>
        /// The system-defined color of dimmed text. Items in a list that are disabled
        /// are displayed in dimmed text.
        /// </summary>
        GrayText = 12,

        /// <summary>
        /// The system-defined color of the background of selected items. This includes selected
        /// menu items as well as selected text.
        /// </summary>
        Highlight = 13,

        /// <summary>
        /// The system-defined color of the text of selected items.
        /// </summary>
        HighlightText = 14,

        /// <summary>
        /// The system-defined color used to designate a hot-tracked item.
        /// Single-clicking a hot-tracked item executes the item.
        /// </summary>
        HotTrack = 15,

        /// <summary>
        /// The system-defined color of an inactive window's border.
        /// </summary>
        InactiveBorder = 16,

        /// <summary>
        /// The system-defined color of the background of an inactive window's title bar.
        /// </summary>
        InactiveCaption = 17,

        /// <summary>
        /// The system-defined color of the text in an inactive window's title bar.
        /// </summary>
        InactiveCaptionText = 18,

        /// <summary>
        /// The system-defined color of the background of a ToolTip.
        /// </summary>
        Info = 19,

        /// <summary>
        /// The system-defined color of the text of a ToolTip.
        /// </summary>
        InfoText = 20,

        /// <summary>
        /// The system-defined color of a menu's background.
        /// </summary>
        Menu = 21,

        /// <summary>
        /// The system-defined color of a menu's text.
        /// </summary>
        MenuText = 22,

        /// <summary>
        /// The system-defined color of the background of a scroll bar.
        /// </summary>
        ScrollBar = 23,

        /// <summary>
        /// The system-defined color of the background in the client area of a window.
        /// </summary>
        Window = 24,

        /// <summary>
        /// The system-defined color of a window frame.
        /// </summary>
        WindowFrame = 25,

        /// <summary>
        /// The system-defined color of the text in the client area of a window.
        /// </summary>
        WindowText = 26,

        // "Web" Colors, Part 1

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Transparent = 27,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        AliceBlue = 28,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        AntiqueWhite = 29,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Aqua = 30,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Aquamarine = 31,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Azure = 32,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Beige = 33,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Bisque = 34,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Black = 35,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        BlanchedAlmond = 36,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Blue = 37,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        BlueViolet = 38,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Brown = 39,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        BurlyWood = 40,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        CadetBlue = 41,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Chartreuse = 42,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Chocolate = 43,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Coral = 44,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        CornflowerBlue = 45,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Cornsilk = 46,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Crimson = 47,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Cyan = 48,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkBlue = 49,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkCyan = 50,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkGoldenrod = 51,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkGray = 52,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkGreen = 53,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkKhaki = 54,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkMagenta = 55,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkOliveGreen = 56,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkOrange = 57,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkOrchid = 58,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkRed = 59,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkSalmon = 60,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkSeaGreen = 61,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkSlateBlue = 62,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkSlateGray = 63,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkTurquoise = 64,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DarkViolet = 65,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DeepPink = 66,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DeepSkyBlue = 67,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DimGray = 68,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        DodgerBlue = 69,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Firebrick = 70,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        FloralWhite = 71,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        ForestGreen = 72,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Fuchsia = 73,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Gainsboro = 74,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        GhostWhite = 75,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Gold = 76,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Goldenrod = 77,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Gray = 78,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Green = 79,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        GreenYellow = 80,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Honeydew = 81,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        HotPink = 82,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        IndianRed = 83,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Indigo = 84,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Ivory = 85,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Khaki = 86,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Lavender = 87,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LavenderBlush = 88,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LawnGreen = 89,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LemonChiffon = 90,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightBlue = 91,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightCoral = 92,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightCyan = 93,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightGoldenrodYellow = 94,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightGray = 95,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightGreen = 96,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightPink = 97,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightSalmon = 98,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightSeaGreen = 99,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightSkyBlue = 100,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightSlateGray = 101,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightSteelBlue = 102,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LightYellow = 103,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Lime = 104,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        LimeGreen = 105,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Linen = 106,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Magenta = 107,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Maroon = 108,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumAquamarine = 109,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumBlue = 110,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumOrchid = 111,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumPurple = 112,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumSeaGreen = 113,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumSlateBlue = 114,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumSpringGreen = 115,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumTurquoise = 116,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MediumVioletRed = 117,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MidnightBlue = 118,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MintCream = 119,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        MistyRose = 120,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Moccasin = 121,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        NavajoWhite = 122,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Navy = 123,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        OldLace = 124,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Olive = 125,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        OliveDrab = 126,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Orange = 127,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        OrangeRed = 128,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Orchid = 129,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PaleGoldenrod = 130,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PaleGreen = 131,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PaleTurquoise = 132,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PaleVioletRed = 133,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PapayaWhip = 134,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PeachPuff = 135,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Peru = 136,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Pink = 137,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Plum = 138,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        PowderBlue = 139,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Purple = 140,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Red = 141,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        RosyBrown = 142,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        RoyalBlue = 143,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SaddleBrown = 144,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Salmon = 145,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SandyBrown = 146,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SeaGreen = 147,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SeaShell = 148,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Sienna = 149,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Silver = 150,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SkyBlue = 151,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SlateBlue = 152,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SlateGray = 153,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Snow = 154,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SpringGreen = 155,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        SteelBlue = 156,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Tan = 157,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Teal = 158,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Thistle = 159,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Tomato = 160,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Turquoise = 161,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Violet = 162,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Wheat = 163,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        White = 164,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        WhiteSmoke = 165,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        Yellow = 166,

        /// <summary>
        /// A system-defined color.
        /// </summary>
        YellowGreen = 167,

        // "System" colors, Part 2

        /// <summary>
        /// The system-defined face color of a 3-D element.
        /// </summary>
        ButtonFace = 168,

        /// <summary>
        /// The system-defined color that is the highlight color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face the light source.
        /// </summary>
        ButtonHighlight = 169,

        /// <summary>
        /// The system-defined color that is the shadow color of a 3-D element.
        /// This color is applied to parts of a 3-D element that face away from the
        /// light source.
        /// </summary>
        ButtonShadow = 170,

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an active window's title bar.
        /// </summary>
        GradientActiveCaption = 171,

        /// <summary>
        /// The system-defined color of the lightest color in the color gradient
        /// of an inactive window's title bar.
        /// </summary>
        GradientInactiveCaption = 172,

        /// <summary>
        /// The system-defined color of the background of a menu bar.
        /// </summary>
        MenuBar = 173,

        /// <summary>
        /// The system-defined color used to highlight menu items when the menu appears
        /// as a flat menu.
        /// </summary>
        MenuHighlight = 174,

        // "Web" colors, Part 2

        /// <summary>
        /// A system-defined color.
        /// </summary>
        RebeccaPurple = 175,
    }
}
