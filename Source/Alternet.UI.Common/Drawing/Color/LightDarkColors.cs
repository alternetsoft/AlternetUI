using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static members related to colors which have different
    /// argb in dark and light themes.
    /// </summary>
    public static partial class LightDarkColors
    {
        private static LightDarkColor? yellow;
        private static LightDarkColor? red;
        private static LightDarkColor? green;
        private static LightDarkColor? blue;
        private static LightDarkColor? blueDarker;
        private static LightDarkColor? blueLighter;

        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> that is gray text.
        /// </summary>
        public static LightDarkColor GrayText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color
        /// of the active window's border.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the
        /// active window's border.</returns>
        public static LightDarkColor ActiveBorder;

        /// <summary>Gets a <see cref="Color" /> structure that is the color
        /// of the background of the active window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the
        /// active window's title bar.</returns>
        public static LightDarkColor ActiveCaption;

        /// <summary>Gets a <see cref="Color" /> structure that is the color
        /// of the text in the active window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the text in
        /// the active window's title bar.</returns>
        public static LightDarkColor ActiveCaptionText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color
        /// of the application workspace. </summary>
        /// <returns>A <see cref="Color" /> that is the color of the
        /// application workspace.</returns>
        public static LightDarkColor AppWorkspace;

        /// <summary>Gets a <see cref="Color" /> structure that is the face
        /// color of a 3-D element.</summary>
        /// <returns>A <see cref="Color" /> that is the face color of
        /// a 3-D element.</returns>
        public static LightDarkColor ButtonFace;

        /// <summary>Gets a <see cref="Color" /> structure that is the
        /// highlight color of a 3-D element. </summary>
        /// <returns>A <see cref="Color" /> that is the highlight color
        /// of a 3-D element.</returns>
        public static LightDarkColor ButtonHighlight;

        /// <summary>Gets a <see cref="Color" /> structure that is the shadow
        /// color of a 3-D element. </summary>
        /// <returns>A <see cref="Color" /> that is the shadow color of
        /// a 3-D element.</returns>
        public static LightDarkColor ButtonShadow;

        /// <summary>Gets a <see cref="Color" /> structure that is the
        /// face color of a 3-D element.</summary>
        /// <returns>A <see cref="Color" /> that is the face color of
        /// a 3-D element.</returns>
        public static LightDarkColor Control;

        /// <summary>Gets a <see cref="Color" /> structure that is the shadow color of a 3-D
        /// element. </summary>
        /// <returns>A <see cref="Color" /> that is the shadow color of a 3-D element.</returns>
        public static LightDarkColor ControlDark;

        /// <summary>Gets a <see cref="Color" /> structure that is the dark shadow color
        /// of a 3-D element. </summary>
        /// <returns>A <see cref="Color" /> that is the dark shadow
        /// color of a 3-D element.</returns>
        public static LightDarkColor ControlDarkDark;

        /// <summary>Gets a <see cref="Color" /> structure that is the light color of a 3-D
        /// element. </summary>
        /// <returns>A <see cref="Color" /> that is the light color of a 3-D element.</returns>
        public static LightDarkColor ControlLight;

        /// <summary>Gets a <see cref="Color" /> structure that is the highlight color of
        /// a 3-D element. </summary>
        /// <returns>A <see cref="Color" /> that is the highlight color
        /// of a 3-D element.</returns>
        public static LightDarkColor ControlLightLight;

        /// <summary>Gets a <see cref="Color" /> structure that is the color
        /// of the desktop.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the desktop.</returns>
        public static LightDarkColor Desktop;

        /// <summary>Gets a <see cref="Color" /> structure that is the lightest color in the
        /// color gradient of an active window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the lightest color in the color gradient
        /// of an active window's title bar.</returns>
        public static LightDarkColor GradientActiveCaption;

        /// <summary>Gets a <see cref="Color" /> structure that is the lightest color in the
        /// color gradient of an inactive window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the lightest color in the color gradient
        /// of an inactive window's title bar.</returns>
        public static LightDarkColor GradientInactiveCaption;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the background
        /// of selected items.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background of selected
        /// items.</returns>
        public static LightDarkColor Highlight;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the text of
        /// selected items.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the text of selected
        /// items.</returns>
        public static LightDarkColor HighlightText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color used to designate
        /// a hot-tracked item. </summary>
        /// <returns>A <see cref="Color" /> that is the color used to designate a hot-tracked
        /// item.</returns>
        public static LightDarkColor HotTrack;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of an inactive
        /// window's border.</summary>
        /// <returns>A <see cref="Color" /> that is the color of an inactive window's
        /// border.</returns>
        public static LightDarkColor InactiveBorder;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the background
        /// of an inactive window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background of an
        /// inactive window's title bar.</returns>
        public static LightDarkColor InactiveCaption;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the text
        /// in an inactive window's title bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the text in an inactive
        /// window's title bar.</returns>
        public static LightDarkColor InactiveCaptionText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the background
        /// of a ToolTip.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background of a
        /// ToolTip.</returns>
        public static LightDarkColor Info;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the text
        /// of a ToolTip.</summary>
        /// <returns>A <see cref="Color" /> that is the color
        /// of the text of a ToolTip.</returns>
        public static LightDarkColor InfoText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of a menu's
        /// background.</summary>
        /// <returns>A <see cref="Color" /> that is the color of a menu's background.</returns>
        public static LightDarkColor Menu;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the
        /// background of a menu bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background of a menu
        /// bar.</returns>
        public static LightDarkColor MenuBar;

        /// <summary>Gets a <see cref="Color" /> structure that is the color used to highlight
        /// menu items when the menu appears as a flat menu.</summary>
        /// <returns>A <see cref="Color" /> that is the color used to highlight menu items
        /// when the menu appears as a flat menu.</returns>
        public static LightDarkColor MenuHighlight;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of a menu's
        /// text.</summary>
        /// <returns>A <see cref="Color" /> that is the color of a menu's text.</returns>
        public static LightDarkColor MenuText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the background
        /// of a scroll bar.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background of a scroll
        /// bar.</returns>
        public static LightDarkColor ScrollBar;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of
        /// a window frame.</summary>
        /// <returns>A <see cref="Color" /> that is the color of a window frame.</returns>
        public static LightDarkColor WindowFrame;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the background
        /// in the client area of a window.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the background in the client
        /// area of a window.</returns>
        public static LightDarkColor Window;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of the text in the
        /// client area of a window.</summary>
        /// <returns>A <see cref="Color" /> that is the color of the text in the client area
        /// of a window.</returns>
        public static LightDarkColor WindowText;

        /// <summary>Gets a <see cref="Color" /> structure that is the color of text in a
        /// 3-D element.</summary>
        /// <returns>A <see cref="Color" /> that is the color of text in
        /// a 3-D element.</returns>
        public static LightDarkColor ControlText;

        static LightDarkColors()
        {
            InitColors(SystemColorsLight.Default, SystemColorsDarkMacOs.Default);
        }

        /// <summary>
        /// Initializes colors from the specified light and dark color sources.
        /// </summary>
        /// <param name="lightColorSource">The source of light colors.</param>
        /// <param name="darkColorSource">The source of dark colors.</param>
        [MemberNotNull(nameof(GrayText), nameof(ActiveBorder), nameof(ActiveCaption), nameof(ActiveCaptionText),
            nameof(AppWorkspace), nameof(ButtonFace), nameof(ButtonHighlight), nameof(ButtonShadow), nameof(Control),
            nameof(ControlDark), nameof(ControlDarkDark), nameof(ControlLight), nameof(ControlLightLight), nameof(Desktop),
            nameof(GradientActiveCaption), nameof(GradientInactiveCaption), nameof(Highlight), nameof(HighlightText),
            nameof(HotTrack), nameof(InactiveBorder), nameof(InactiveCaption), nameof(InactiveCaptionText),
            nameof(Menu), nameof(MenuBar), nameof(MenuHighlight), nameof(MenuText), nameof(ScrollBar), nameof(WindowFrame),
            nameof(Window), nameof(WindowText), nameof(ControlText), nameof(Info), nameof(InfoText))]
        public static void InitColors(ISystemColorStructs lightColorSource, ISystemColorStructs darkColorSource)
        {
            GrayText = new LightDarkColor(lightColorSource.GrayText, darkColorSource.GrayText);
            ActiveBorder = new LightDarkColor(lightColorSource.ActiveBorder, darkColorSource.ActiveBorder);
            ActiveCaption = new LightDarkColor(lightColorSource.ActiveCaption, darkColorSource.ActiveCaption);
            ActiveCaptionText = new LightDarkColor(lightColorSource.ActiveCaptionText, darkColorSource.ActiveCaptionText);
            AppWorkspace = new LightDarkColor(lightColorSource.AppWorkspace, darkColorSource.AppWorkspace);
            ButtonFace = new LightDarkColor(lightColorSource.ButtonFace, darkColorSource.ButtonFace);
            ButtonHighlight = new LightDarkColor(lightColorSource.ButtonHighlight, darkColorSource.ButtonHighlight);
            ButtonShadow = new LightDarkColor(lightColorSource.ButtonShadow, darkColorSource.ButtonShadow);
            Control = new LightDarkColor(lightColorSource.Control, darkColorSource.Control);
            ControlDark = new LightDarkColor(lightColorSource.ControlDark, darkColorSource.ControlDark);
            ControlDarkDark = new LightDarkColor(lightColorSource.ControlDarkDark, darkColorSource.ControlDarkDark);
            ControlLight = new LightDarkColor(lightColorSource.ControlLight, darkColorSource.ControlLight);
            ControlLightLight = new LightDarkColor(lightColorSource.ControlLightLight, darkColorSource.ControlLightLight);
            Desktop = new LightDarkColor(lightColorSource.Desktop, darkColorSource.Desktop);
            GradientActiveCaption
                = new LightDarkColor(lightColorSource.GradientActiveCaption, darkColorSource.GradientActiveCaption);
            GradientInactiveCaption
                = new LightDarkColor(lightColorSource.GradientInactiveCaption, darkColorSource.GradientInactiveCaption);
            Highlight = new LightDarkColor(lightColorSource.Highlight, darkColorSource.Highlight);
            HighlightText = new LightDarkColor(lightColorSource.HighlightText, darkColorSource.HighlightText);
            HotTrack = new LightDarkColor(lightColorSource.HotTrack, darkColorSource.HotTrack);
            InactiveBorder = new LightDarkColor(lightColorSource.InactiveBorder, darkColorSource.InactiveBorder);
            InactiveCaption = new LightDarkColor(lightColorSource.InactiveCaption, darkColorSource.InactiveCaption);
            InactiveCaptionText = new LightDarkColor(lightColorSource.InactiveCaptionText, darkColorSource.InactiveCaptionText);
            Info = new LightDarkColor(lightColorSource.Info, darkColorSource.Info);
            InfoText = new LightDarkColor(lightColorSource.InfoText, darkColorSource.InfoText);
            Menu = new LightDarkColor(lightColorSource.Menu, darkColorSource.Menu);
            MenuBar = new LightDarkColor(lightColorSource.MenuBar, darkColorSource.MenuBar);
            MenuHighlight = new LightDarkColor(lightColorSource.MenuHighlight, darkColorSource.MenuHighlight);
            MenuText = new LightDarkColor(lightColorSource.MenuText, darkColorSource.MenuText);
            ScrollBar = new LightDarkColor(lightColorSource.ScrollBar, darkColorSource.ScrollBar);
            WindowFrame = new LightDarkColor(lightColorSource.WindowFrame, darkColorSource.WindowFrame);
            Window = new LightDarkColor(lightColorSource.Window, darkColorSource.Window);
            WindowText = new LightDarkColor(lightColorSource.WindowText, darkColorSource.WindowText);
            ControlText = new LightDarkColor(lightColorSource.ControlText, darkColorSource.ControlText);
        }

        /// <summary>
        /// Gets default red colors pair.
        /// </summary>
        public static LightDarkColor Red
        {
            get
            {
                return red ??= Color.LightDark(light: (192, 10, 22), dark: (244, 75, 86));
            }

            set => red = value;
        }

        /// <summary>
        /// Gets default yellow colors pair.
        /// </summary>
        public static LightDarkColor Yellow
        {
            get
            {
                return yellow ??= Color.LightDark(light: (239, 184, 57), dark: (239, 184, 57));
            }

            set => yellow = value;
        }

        /// <summary>
        /// Gets default green colors pair.
        /// </summary>
        public static LightDarkColor Green
        {
            get
            {
                return green ??= Color.LightDark(light: (30, 124, 30), dark: (138, 226, 138));
            }

            set => green = value;
        }

        /// <summary>
        /// Gets default blue colors pair.
        /// </summary>
        public static LightDarkColor Blue
        {
            get
            {
                return blue ??= Color.LightDark(light: (0, 90, 181), dark: (85, 170, 255));
            }

            set => blue = value;
        }

        /// <summary>
        /// Gets default blue darker colors pair.
        /// </summary>
        public static LightDarkColor BlueDarker
        {
            get
            {
                return blueDarker ??= new(Blue.Light.Darker(), Blue.Dark.Darker());
            }

            set => blueDarker = value;
        }

        /// <summary>
        /// Gets default blue lighter colors pair.
        /// </summary>
        public static LightDarkColor BlueLighter
        {
            get
            {
                return blueLighter ??= new(Blue.Light.Lighter(), Blue.Dark.Lighter());
            }

            set => blueLighter = value;
        }

        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> that is white.
        /// </summary>
        public static LightDarkColor White { get; } = new LightDarkColor(Color.White).SetImmutable();

        /// <summary>
        /// Contains background colors which look good with light text.
        /// </summary>
        public static class LightTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Black => Color.Black;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Blue => Color.Blue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color BlueViolet => Color.BlueViolet;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Brown => Color.Brown;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color CadetBlue => Color.CadetBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Chocolate => Color.Chocolate;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color CornflowerBlue => Color.CornflowerBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Crimson => Color.Crimson;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkBlue => Color.DarkBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkCyan => Color.DarkCyan;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkGoldenrod => Color.DarkGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkGreen => Color.DarkGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkMagenta => Color.DarkMagenta;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkOliveGreen => Color.DarkOliveGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkOrchid => Color.DarkOrchid;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkRed => Color.DarkRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkSlateBlue => Color.DarkSlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkSlateGray => Color.DarkSlateGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkViolet => Color.DarkViolet;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DimGray => Color.DimGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Firebrick => Color.Firebrick;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color ForestGreen => Color.ForestGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Green => Color.Green;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color IndianRed => Color.IndianRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Indigo => Color.Indigo;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Maroon => Color.Maroon;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumBlue => Color.MediumBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumOrchid => Color.MediumOrchid;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumPurple => Color.MediumPurple;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumSlateBlue => Color.MediumSlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumVioletRed => Color.MediumVioletRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MidnightBlue => Color.MidnightBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Olive => Color.Olive;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color OrangeRed => Color.OrangeRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color RebeccaPurple => Color.RebeccaPurple;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color RoyalBlue => Color.RoyalBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SaddleBrown => Color.SaddleBrown;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SeaGreen => Color.SeaGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Sienna => Color.Sienna;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SlateBlue => Color.SlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SlateGray => Color.SlateGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SteelBlue => Color.SteelBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Teal => Color.Teal;

            /// <summary>
            /// Gets all background colors which look good with light text.
            /// </summary>
            public static IEnumerable<Color> AllColors
            {
                get
                {
                    return AssemblyUtils.GetStaticPropertyValues<Color>(typeof(LightTextBackgrounds));
                }
            }
        }

        /// <summary>
        /// Contains background colors which look good with dark text.
        /// </summary>
        public static class DarkTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color AntiqueWhite => Color.AntiqueWhite;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Aquamarine => Color.Aquamarine;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Bisque => Color.Bisque;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Gold => Color.Gold;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Khaki => Color.Khaki;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Lavender => Color.Lavender;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightBlue => Color.LightBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightCyan => Color.LightCyan;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightGray => Color.LightGray;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightGreen => Color.LightGreen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightPink => Color.LightPink;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightSalmon => Color.LightSalmon;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightSkyBlue => Color.LightSkyBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightYellow => Color.LightYellow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Linen => Color.Linen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color MintCream => Color.MintCream;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color MistyRose => Color.MistyRose;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Moccasin => Color.Moccasin;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color NavajoWhite => Color.NavajoWhite;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color OldLace => Color.OldLace;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleGoldenrod => Color.PaleGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleGreen => Color.PaleGreen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleTurquoise => Color.PaleTurquoise;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PapayaWhip => Color.PapayaWhip;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PeachPuff => Color.PeachPuff;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Pink => Color.Pink;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PowderBlue => Color.PowderBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color SeaShell => Color.SeaShell;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color SkyBlue => Color.SkyBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Snow => Color.Snow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Wheat => Color.Wheat;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color WhiteSmoke => Color.WhiteSmoke;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Yellow => Color.Yellow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color YellowGreen => Color.YellowGreen;

            /// <summary>
            /// Gets all background colors which look good with dark text.
            /// </summary>
            public static IEnumerable<Color> AllColors
            {
                get
                {
                    return AssemblyUtils.GetStaticPropertyValues<Color>(typeof(DarkTextBackgrounds));
                }
            }
        }

        /// <summary>
        /// Contains background colors which look good with light and dark text.
        /// </summary>
        public static class LightDarkTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Chocolate => Color.Chocolate;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkCyan => Color.DarkCyan;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkGoldenrod => Color.DarkGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkGreen => Color.DarkGreen;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Olive => Color.Olive;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color RoyalBlue => Color.RoyalBlue;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color SlateBlue => Color.SlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Teal => Color.Teal;

            /// <summary>
            /// Gets all background colors which look good with light and dark text.
            /// </summary>
            public static IEnumerable<Color> AllColors
            {
                get
                {
                    return AssemblyUtils.GetStaticPropertyValues<Color>(typeof(LightDarkTextBackgrounds));
                }
            }
        }

    }
}