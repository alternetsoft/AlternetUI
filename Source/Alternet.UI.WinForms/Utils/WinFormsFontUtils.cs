using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.UI.WinForms
{
    /// <summary>
    /// Provides utility methods related to <see cref="Font"/> objects.
    /// </summary>
    public static class WinFormsFontUtils
    {
        /// <summary>
        /// A list of preferred monospace font names, ordered by priority.
        /// </summary>
        /// <remarks>This array contains commonly used monospace fonts, with "Consolas" as the highest
        /// priority. It can be used to select a suitable font for displaying code or other fixed-width text.</remarks>
        public static readonly string[] PreferredMonospaceFonts = new[]
        {
            "Consolas",
            "Lucida Console",
            "Courier New",
        };

        /// <summary>
        /// Represents the normal font weight used in text rendering.
        /// Value of this field is used when <see cref="System.Drawing.Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        /// <remarks>This field corresponds to the standard "Normal" weight in font styles, typically used
        /// for regular text.</remarks>
        public static SKFontStyleWeight NormalFontWeight = SKFontStyleWeight.Normal;

        /// <summary>
        /// Gets or sets the bold font weight used in text rendering.
        /// Value of this field is used when <see cref="System.Drawing.Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>        
        /// <remarks>This field corresponds to the standard "Bold" weight in font styles, typically used
        /// for regular text.</remarks>
        public static SKFontStyleWeight BoldFontWeight = SKFontStyleWeight.Bold;

        private static Font? bestMonospaceFont;

        /// <summary>
        /// Gets or sets the best available monospace font for rendering text.
        /// </summary>
        public static Font BestMonospaceFont
        {
            get
            {
                return bestMonospaceFont ??= CreateBestMonospaceFont();
            }

            set
            {
                bestMonospaceFont = value;
            }
        }

        /// <summary>
        /// Returns a Font with the exact <paramref name="style"/> applied.
        /// If the font already has that style, the original instance is returned.
        /// </summary>
        public static Font WithStyle(this Font font, FontStyle style)
        {
            if (font is null) throw new ArgumentNullException(nameof(font));
            if (font.Style == style) return font;
            return new Font(font, style);
        }

        /// <summary>
        /// Returns a Font created by applying a modifier function to the current FontStyle.
        /// Example: f.WithStyle(s => s | FontStyle.Bold) to add bold.
        /// </summary>
        public static Font WithStyle(this Font font, Func<FontStyle, FontStyle> modifier)
        {
            if (font is null) throw new ArgumentNullException(nameof(font));
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            var newStyle = modifier(font.Style);
            if (newStyle == font.Style) return font;
            return new Font(font, newStyle);
        }

        /// <summary>
        /// Adds the given style flags to the font (e.g. AddStyle(FontStyle.Bold | FontStyle.Italic)).
        /// </summary>
        public static Font AddStyle(this Font font, FontStyle flags)
            => font.WithStyle(s => s | flags);

        /// <summary>
        /// Removes the given style flags from the font (e.g. RemoveStyle(FontStyle.Underline)).
        /// </summary>
        public static Font RemoveStyle(this Font font, FontStyle flags)
            => font.WithStyle(s => s & ~flags);

        /// <summary>
        /// Toggles the specified flags on the font style.
        /// </summary>
        public static Font ToggleStyle(this Font font, FontStyle flags)
            => font.WithStyle(s => (s & flags) == flags ? (s & ~flags) : (s | flags));

        /// <summary>
        /// Sets or clears a specific flag in the provided <see cref="FontStyle"/> value based
        /// on the specified condition.
        /// </summary>
        /// <param name="current">The current <see cref="FontStyle"/> value.</param>
        /// <param name="flag">The <see cref="FontStyle"/> flag to set or clear.</param>
        /// <param name="value">A nullable boolean indicating whether to set or clear the flag.
        /// If <see langword="true"/>, the flag is set.
        /// If <see langword="false"/>, the flag is cleared.  If <see langword="null"/>, the <paramref name="current"/>
        /// value is returned unchanged.</param>
        /// <returns>A <see cref="FontStyle"/> value with the specified flag set or cleared based on the <paramref
        /// name="value"/>.</returns>
        public static FontStyle SetFlag(FontStyle current, FontStyle flag, bool? value)
        {
            if (!value.HasValue) return current;
            return value.Value ? (current | flag) : (current & ~flag);
        }

        /// <summary>
        /// Returns a font where individual style components can be set to true/false/null (null = keep current).
        /// Example: f.WithStyleComponents(bold: true, italic: null, underline: false)
        /// </summary>
        public static Font WithStyleComponents(
            this Font font,
            bool? bold = null,
            bool? italic = null,
            bool? underline = null,
            bool? strikeout = null)
        {
            if (font is null) throw new ArgumentNullException(nameof(font));

            var s = font.Style;

            s = SetFlag(s, FontStyle.Bold, bold);
            s = SetFlag(s, FontStyle.Italic, italic);
            s = SetFlag(s, FontStyle.Underline, underline);
            s = SetFlag(s, FontStyle.Strikeout, strikeout);

            if (s == font.Style) return font;
            return new Font(font, s);
        }

        /// <summary>
        /// Retrieves the default font used for user interface elements.
        /// </summary>
        /// <remarks>This method first attempts to retrieve the default font from <see
        /// cref="System.Windows.Forms.Control.DefaultFont"/>.  If that is unavailable, it falls back to <see
        /// cref="System.Drawing.SystemFonts.DefaultFont"/>.</remarks>
        /// <returns>The default <see cref="System.Drawing.Font"/> used for user interface elements. If no default font is
        /// available, returns a fallback font.</returns>
        public static Font GetDefaultUiFont()
        {
            return System.Windows.Forms.Control.DefaultFont ?? System.Drawing.SystemFonts.DefaultFont;
        }

        /// <summary>
        /// Creates the best available monospace font from a list of preferred fonts,
        /// or falls back to a generic monospace font.
        /// </summary>
        /// <remarks>This method attempts to find and return the first installed font from a predefined
        /// list of preferred monospace fonts. If none of the preferred fonts are available, or if an error occurs, the
        /// method defaults to using the generic monospace font.</remarks>
        /// <param name="sizePt">The desired font size in points. If <see langword="null"/>,
        /// the default UI font size is used.</param>
        /// <param name="style">The font style to apply, such as <see cref="FontStyle.Regular"/>
        /// or <see cref="FontStyle.Bold"/>. The
        /// default is <see cref="FontStyle.Regular"/>.</param>
        /// <returns>A <see cref="Font"/> object representing the best available monospace font.
        /// If none of the preferred fonts
        /// are installed, a generic monospace font is returned.</returns>
        public static Font CreateBestMonospaceFont(float? sizePt = null, FontStyle style = FontStyle.Regular)
        {
            float size = sizePt ?? GetDefaultUiFont().Size; // point size

            try
            {
                using var installed = new System.Drawing.Text.InstalledFontCollection();
                var families = installed.Families;

                // pick first preferred family that is installed
                foreach (var name in PreferredMonospaceFonts)
                {
                    if (families.Any(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase)))
                        return new Font(name, size, style, GraphicsUnit.Point);
                }

                return new Font(FontFamily.GenericMonospace, size, style, GraphicsUnit.Point);
            }
            catch
            {
                return new Font(FontFamily.GenericMonospace, size, style, GraphicsUnit.Point);
            }
        }

        /// <summary>
        /// Ensures that the specified <see cref="Alternet.Drawing.Font"/> instance corresponds
        /// to the given <see cref="System.Drawing.Font"/> instance.
        /// </summary>
        /// <remarks>This method ensures that the <paramref name="uiFont"/> parameter
        /// represents the same font as <paramref name="winFont"/>. If <paramref name="uiFont"/> is
        /// already equivalent to <paramref name="winFont"/>, no changes are made.</remarks>
        /// <param name="uiFont">A reference to an <see cref="Alternet.Drawing.Font"/> instance.
        /// If <paramref name="winFont"/> is <see langword="null"/>,
        /// this will be set to <see langword="null"/>. Otherwise, it will be updated to match
        /// <paramref name="winFont"/> if they are not already equivalent.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to synchronize with.
        /// If <see langword="null"/>, <paramref name="uiFont"/>
        /// will be set to <see langword="null"/>.</param>
        public static void EnsureFont(
            ref Alternet.Drawing.Font? uiFont,
            System.Drawing.Font? winFont,
            float scale = 1)
        {
            if (winFont == null)
            {
                uiFont = null;
                return;
            }

            if (uiFont is null)
            {
                uiFont = winFont.ToAlternet(scale);
            }
            else
            {
                if (!Equals(uiFont, winFont, scale))
                    uiFont = winFont.ToAlternet(scale);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Alternet.Drawing.Font"/>
        /// and <see cref="System.Drawing.Font"/>
        /// instances represent the same font.
        /// </summary>
        /// <remarks>The comparison checks the font name, size (in points),
        /// and style to determine
        /// equality.</remarks>
        /// <param name="uiFont">The <see cref="Alternet.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both fonts are <see langword="null"/>
        /// or if they represent the same font;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool Equals(
            Alternet.Drawing.Font? uiFont,
            System.Drawing.Font? winFont,
            float scale)
        {
            if (uiFont == null && winFont == null)
                return true;
            if (uiFont == null || winFont == null)
                return false;

            var size = FontSizeToAlternet(winFont.SizeInPoints, scale);

            return uiFont.Equals(
                winFont.Name,
                size,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }

        /// <summary>
        /// Converts the font settings to a <see cref="SkiaFontInfo"/> object.
        /// </summary>
        /// <remarks>The resulting <see cref="SkiaFontInfo"/> includes the font's weight, slant, name, 
        /// and size based on the current settings.</remarks>
        /// <returns>A <see cref="SkiaFontInfo"/> object representing the font's attributes.</returns>
        public static Alternet.Drawing.SkiaFontInfo ToSkiaFontInfo(
            System.Drawing.Font font,
            float scale)
        {
            Alternet.Drawing.SkiaFontInfo result = new();

            result.Weight = font.Style.HasFlag(FontStyle.Bold) ? BoldFontWeight : NormalFontWeight;

            result.Slant = font.Style.HasFlag(FontStyle.Italic)
                ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

            result.Name = font.Name;

            var size = FontSizeToAlternet(font.SizeInPoints, scale);

            result.SizeInDips = size;
            return result;
        }

        public static float FontSizeToAlternet(float sizeInPoints, float scale)
        {
            var size = sizeInPoints * (96f / 72f);
            size *= scale;
            return size;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Font"/> object to an
        /// <see cref="Alternet.Drawing.Font"/> object.
        /// </summary>
        /// <remarks>The conversion maps the font name, size (in points),
        /// and style from the <see cref="System.Drawing.Font"/> to the corresponding
        /// properties of the <see cref="Alternet.Drawing.Font"/>.</remarks>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> to convert.
        /// Can be <see langword="null"/>.</param>
        /// <returns>An <see cref="Alternet.Drawing.Font"/> object that represents
        /// the converted font, or <see langword="null"/>
        /// if <paramref name="winFont"/> is <see langword="null"/>.</returns>
        public static Alternet.Drawing.Font? ToAlternet(this System.Drawing.Font? winFont, float scale)
        {
            if (winFont is null)
                return null;
            var size = FontSizeToAlternet(winFont.SizeInPoints, scale);
            return new Alternet.Drawing.Font(
                winFont.Name,
                size,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }
    }
}
