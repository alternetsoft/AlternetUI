using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines abstract methods related to native drawing.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="Default"/> property until native drawing
    /// is initialized.
    /// </remarks>
    public class NativeDrawing : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativeDrawing Default = new();

        /// <summary>
        /// Creates native font.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateFont() => NotImplemented();

        /// <summary>
        /// Creates default native font.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateDefaultFont() => NotImplemented();

        /// <summary>
        /// Creates system font specified with <see cref="SystemSettingsFont"/>.
        /// </summary>
        /// <param name="systemFont">System font identifier.</param>
        /// <returns></returns>
        public virtual Font CreateSystemFont(SystemSettingsFont systemFont) => NotImplemented<Font>();

        /// <summary>
        /// Creates native font using other font properties.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateFont(object font) => NotImplemented();

        /// <summary>
        /// Creates default native mono font.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateDefaultMonoFont() => NotImplemented();

        /// <summary>
        /// Creates native pen.
        /// </summary>
        /// <returns></returns>
        public virtual object CreatePen() => NotImplemented();

        /// <summary>
        /// Creates native transparent brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateTransparentBrush() => NotImplemented();

        /// <summary>
        /// Creates native hatch brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateHatchBrush() => NotImplemented();

        /// <summary>
        /// Creates native linear gradient brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateLinearGradientBrush() => NotImplemented();

        /// <summary>
        /// Creates native radial gradient brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateRadialGradientBrush() => NotImplemented();

        /// <summary>
        /// Creates native solid brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateSolidBrush() => NotImplemented();

        /// <summary>
        /// Creates native texture brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateTextureBrush() => NotImplemented();

        /// <summary>
        /// Updates native pen properties.
        /// </summary>
        /// <returns></returns>
        public virtual void UpdatePen(Pen pen) => NotImplemented();

        /// <summary>
        /// Updates native solid brush properties.
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateSolidBrush(SolidBrush brush) => NotImplemented();

        /// <summary>
        /// Updates native font properties.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <param name="prm">Font properties.</param>
        /// <returns></returns>
        public virtual void UpdateFont(object font, FontParams prm) => NotImplemented();

        /// <summary>
        /// Updates native hatch brush properties.
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateHatchBrush(HatchBrush brush) => NotImplemented();

        /// <summary>
        /// Updates native linear gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateLinearGradientBrush(LinearGradientBrush brush) => NotImplemented();

        /// <summary>
        /// Updates native radial gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateRadialGradientBrush(RadialGradientBrush brush) => NotImplemented();

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public virtual Color GetColor(SystemSettingsColor index) => NotImplemented<Color>();

        /// <summary>
        /// Returns a string array that contains all font families names
        /// currently available in the system.
        /// </summary>
        public virtual string[] GetFontFamiliesNames() => NotImplemented<string[]>();

        /// <summary>
        /// Gets whether font family is installed on this computer.
        /// </summary>
        public virtual bool IsFontFamilyValid(string name) => NotImplemented<bool>();

        /// <summary>
        /// Gets the name of the font family specified using <see cref="GenericFontFamily"/>.
        /// </summary>
        public virtual string GetFontFamilyName(GenericFontFamily genericFamily)
             => NotImplemented<string>();

        /// <summary>
        /// Gets default font encoding.
        /// </summary>
        /// <returns></returns>
        public virtual int GetDefaultFontEncoding() => NotImplemented<int>();

        /// <summary>
        /// Sets default font encoding.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetDefaultFontEncoding(int value) => NotImplemented();

        /// <summary>
        /// Gets native font name.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public virtual string GetFontName(object font) => NotImplemented<string>();

        /// <summary>
        /// Gets native font encoding.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public virtual int GetFontEncoding(object font) => NotImplemented<int>();

        /// <summary>
        /// Gets native font size in pixels.
        /// </summary>
        public virtual SizeI GetFontSizeInPixels(object font) => NotImplemented<SizeI>();

        /// <summary>
        /// Gets whether native font is using size in pixels.
        /// </summary>
        public virtual bool GetFontIsUsingSizeInPixels(object font) => NotImplemented<bool>();

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public virtual int GetFontNumericWeight(object font) => NotImplemented<int>();

        /// <summary>
        /// Gets whether native font is a fixed width (or monospaced) font.
        /// </summary>
        public virtual bool GetFontIsFixedWidth(object font) => NotImplemented<bool>();

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <returns></returns>
        public virtual FontWeight GetFontWeight(object font) => NotImplemented<FontWeight>();

        /// <summary>
        /// Gets style information for the native font.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public virtual FontStyle GetFontStyle(object font) => NotImplemented<FontStyle>();

        /// <summary>
        /// Gets a value that indicates whether native font is strikethrough.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetFontStrikethrough(object font) => NotImplemented<bool>();

        /// <summary>
        /// Gets a value that indicates whether native font is underlined.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetFontUnderlined(object font) => NotImplemented<bool>();

        /// <summary>
        /// Gets the em-size, in points, of the native font.
        /// </summary>
        /// <returns></returns>
        public virtual double GetFontSizeInPoints(object font) => NotImplemented<double>();

        /// <summary>
        /// Gets native font description string.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFontInfoDesc(object font) => NotImplemented<string>();

        /// <summary>
        /// Indicates whether native font is equal to another native font.
        /// </summary>
        /// <returns></returns>
        public virtual bool FontEquals(object font1, object font2) => NotImplemented<bool>();

        /// <summary>
        /// Returns a string that represents native font.
        /// </summary>
        /// <returns></returns>
        public virtual string FontToString(object font) => NotImplemented<string>();

        public struct FontParams
        {
            public GenericFontFamily? GenericFamily;
            public string? FamilyName;
            public double Size;
            public FontStyle Style = FontStyle.Regular;
            public GraphicsUnit Unit = GraphicsUnit.Point;
            public byte GdiCharSet = 1;

            public FontParams()
            {
            }
        }
    }
}