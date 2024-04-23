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
