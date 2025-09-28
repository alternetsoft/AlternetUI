using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains <see cref="Color"/> related static methods.
    /// </summary>
    public static class ColorUtils
    {
        private static EnumArray<KnownColor, IKnownColorInfo> knownColorItems;
        private static bool knownColorsRegistered = false;

        static ColorUtils()
        {
            knownColorItems = new();
        }

        /// <summary>
        /// Gets default border color.
        /// </summary>
        /// <param name="isDarkBackground">Whether control has dark background.</param>
        /// <returns></returns>
        public static Color GetDefaultBorderColor(bool isDarkBackground)
        {
            return BorderSettings.DefaultColor
                ?? ColorUtils.GetTabControlInteriorBorderColor(isDarkBackground);
        }

        /// <summary>
        /// Converts <see cref="Color"/> to known color if its possible.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>If <see cref="Color.IsKnownColor"/> for <paramref name="color"/> returns
        /// <c>true</c>, color is returned as is;
        /// otherwise list of known colors is searched for <paramref name="color"/> ARGB and
        /// and if it is found, <paramref name="color"/> is modified to become a known color.</returns>
        public static Color? FindKnownColor(Color? color)
        {
            if (color is null)
                return null;
            if (color.IsKnownColor || !color.IsOpaque)
                return color;
            var result = KnownColorTable.ArgbToKnownColor(color.AsUInt());
            return result;
        }

        /// <summary>
        /// Gets or sets whether colors from the specified category are visible for the end user.
        /// </summary>
        /// <param name="category">Color category.</param>
        /// <param name="visible">Show or hide colors.</param>
        public static void SetVisibleColors(KnownColorCategory category, bool visible)
        {
            var colors = GetColorInfoItems();

            foreach (var color in colors)
            {
                if (color.Category == category)
                    color.Visible = visible;
            }
        }

        /// <summary>
        /// Gets default border color if no colors are specified for the border.
        /// </summary>
        /// <param name="control">Control which background color is checked to get whether
        /// it is dark.</param>
        /// <returns></returns>
        public static Color GetDefaultBorderColor(AbstractControl? control)
        {
            if (control is null)
                return BorderSettings.DefaultCommonBorderColor;
            var isDark = control.IsDarkBackground;
            return BorderSettings.DefaultColor
                ?? ColorUtils.GetTabControlInteriorBorderColor(isDark);
        }

        /// <summary>
        /// Gets localized color name for <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        public static string GetLabelLocalized(KnownColor color)
        {
            return GetColorInfo(color).LabelLocalized;
        }

        /// <summary>
        /// Sets localized label registered for the <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        /// <param name="label">Localized label.</param>
        public static void SetLabelLocalized(KnownColor color, string label)
        {
            GetColorInfo(color).LabelLocalized = label;
        }

        /// <summary>
        /// Gets all registered <see cref="IKnownColorInfo"/> items.
        /// </summary>
        public static IEnumerable<IKnownColorInfo> GetColorInfoItems()
        {
            RequireKnownColorItems();

            var collection = knownColorItems.GetValues();

            if (collection is null)
                yield break;

            foreach (var item in collection)
            {
                if (item is null)
                    continue;
                yield return item;
            }
        }

        /// <summary>
        /// Gets <see cref="IKnownColorInfo"/> for <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        public static IKnownColorInfo GetColorInfo(KnownColor color)
        {
            RequireKnownColorItems();

            var result = knownColorItems[color];

            if (result is null)
            {
                result = new KnownColorInfo(color);
                knownColorItems[color] = result;
            }

            return result;
        }

        /// <summary>
        /// Gets default interior border color of the tab control.
        /// </summary>
        /// <returns></returns>
        public static Color GetTabControlInteriorBorderColor(bool isDark)
        {
            return DefaultColors.GetBorderColor(isDark);
        }

        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> instance for the specified
        /// <paramref name="method"/>.
        /// </summary>
        /// <param name="method">Type of the colors to get.</param>
        /// <param name="control">Control instance. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributes"/>.</param>
        /// <param name="renderSize">Rendering size. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributesTextBox"/> or other similar values.
        /// You can skip this parameter as on most os it is ignored.</param>
        /// <returns></returns>
        public static IReadOnlyFontAndColor GetResetColors(
            ResetColorType method,
            AbstractControl? control = null,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            switch (method)
            {
                case ResetColorType.Auto:
                    return FontAndColor.Null;
                case ResetColorType.NullColor:
                    return FontAndColor.Null;
                case ResetColorType.EmptyColor:
                    return FontAndColor.Empty;
                case ResetColorType.DefaultAttributes:
                    return control?.GetDefaultFontAndColor() ?? FontAndColor.Null;
                case ResetColorType.DefaultAttributesTextBox:
                    return AbstractControl.GetStaticDefaultFontAndColor(ControlTypeId.TextBox, renderSize);
                case ResetColorType.DefaultAttributesListBox:
                    return AbstractControl.GetStaticDefaultFontAndColor(ControlTypeId.ListBox, renderSize);
                case ResetColorType.DefaultAttributesButton:
                    return AbstractControl.GetStaticDefaultFontAndColor(ControlTypeId.Button, renderSize);
                case ResetColorType.ColorMenu:
                    return FontAndColor.SystemColorMenu;
                case ResetColorType.ColorActiveCaption:
                    return FontAndColor.SystemColorActiveCaption;
                case ResetColorType.ColorInactiveCaption:
                    return FontAndColor.SystemColorInactiveCaption;
                case ResetColorType.ColorInfo:
                    return FontAndColor.SystemColorInfo;
                case ResetColorType.ColorWindow:
                    return FontAndColor.SystemColorWindow;
                case ResetColorType.ColorHighlight:
                    return FontAndColor.SystemColorHighlight;
                case ResetColorType.ColorButtonFace:
                    return FontAndColor.SystemColorButtonFace;
                default:
                    return FontAndColor.Null;
            }
        }

        private static void RequireKnownColorItems()
        {
            if (knownColorsRegistered)
                return;
            knownColorsRegistered = true;

            static void RegisterKnownColor(KnownColor color, KnownColorCategory cat)
            {
                var item = new KnownColorInfo(color)
                {
                    Category = cat,
                };

                knownColorItems[color] = item;
            }

            RegisterKnownColor(KnownColor.ActiveBorder, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ActiveCaption, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ActiveCaptionText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.AppWorkspace, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Control, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ControlDark, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ControlDarkDark, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ControlLight, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ControlLightLight, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ControlText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Desktop, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.GrayText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Highlight, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.HighlightText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.HotTrack, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.InactiveBorder, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.InactiveCaption, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.InactiveCaptionText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Info, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.InfoText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Menu, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.MenuText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ScrollBar, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Window, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.WindowFrame, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.WindowText, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.Transparent, KnownColorCategory.System);

            RegisterKnownColor(KnownColor.AliceBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.AntiqueWhite, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Aqua, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.Aquamarine, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Azure, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Beige, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Bisque, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Black, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.BlanchedAlmond, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Blue, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.BlueViolet, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Brown, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.BurlyWood, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.CadetBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Chartreuse, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Chocolate, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Coral, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.CornflowerBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Cornsilk, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Crimson, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Cyan, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkCyan, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkGoldenrod, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkKhaki, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkMagenta, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkOliveGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkOrange, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkOrchid, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkRed, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkSalmon, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkSeaGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkSlateBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkSlateGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkTurquoise, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DarkViolet, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DeepPink, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DeepSkyBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DimGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.DodgerBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Firebrick, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.FloralWhite, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.ForestGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Fuchsia, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.Gainsboro, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.GhostWhite, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Gold, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Goldenrod, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Gray, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.Green, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.GreenYellow, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Honeydew, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.HotPink, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.IndianRed, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Indigo, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Ivory, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Khaki, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Lavender, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LavenderBlush, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LawnGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LemonChiffon, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightCoral, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightCyan, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightGoldenrodYellow, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightPink, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightSalmon, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightSeaGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightSkyBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightSlateGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightSteelBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.LightYellow, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Lime, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.LimeGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Linen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Magenta, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Maroon, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.MediumAquamarine, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumOrchid, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumPurple, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumSeaGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumSlateBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumSpringGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumTurquoise, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MediumVioletRed, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MidnightBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MintCream, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.MistyRose, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Moccasin, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.NavajoWhite, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Navy, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.OldLace, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Olive, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.OliveDrab, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Orange, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.OrangeRed, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Orchid, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PaleGoldenrod, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PaleGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PaleTurquoise, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PaleVioletRed, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PapayaWhip, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PeachPuff, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Peru, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Pink, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Plum, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.PowderBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Purple, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.Red, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.RosyBrown, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.RoyalBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SaddleBrown, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Salmon, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SandyBrown, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SeaGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SeaShell, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Sienna, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Silver, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.SkyBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SlateBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SlateGray, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Snow, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SpringGreen, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.SteelBlue, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Tan, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Teal, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.Thistle, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Tomato, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Turquoise, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Violet, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Wheat, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.White, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.WhiteSmoke, KnownColorCategory.Web);
            RegisterKnownColor(KnownColor.Yellow, KnownColorCategory.Standard);
            RegisterKnownColor(KnownColor.YellowGreen, KnownColorCategory.Web);

            RegisterKnownColor(KnownColor.ButtonFace, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ButtonHighlight, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.ButtonShadow, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.GradientActiveCaption, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.GradientInactiveCaption, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.MenuBar, KnownColorCategory.System);
            RegisterKnownColor(KnownColor.MenuHighlight, KnownColorCategory.System);

            RegisterKnownColor(KnownColor.RebeccaPurple, KnownColorCategory.Web);

            SetVisibleColors(KnownColorCategory.System, false);
        }
    }
}