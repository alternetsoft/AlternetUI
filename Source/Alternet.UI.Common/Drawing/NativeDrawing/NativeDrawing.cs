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
    public abstract partial class NativeDrawing : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativeDrawing Default = new NotImplementedDrawing();

        /// <summary>
        /// Creates native font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateFont();

        /// <summary>
        /// Creates default native font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateDefaultFont();

        /// <summary>
        /// Creates system font specified with <see cref="SystemSettingsFont"/>.
        /// </summary>
        /// <param name="systemFont">System font identifier.</param>
        /// <returns></returns>
        public abstract Font CreateSystemFont(SystemSettingsFont systemFont);

        /// <summary>
        /// Creates native font using other font properties.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateFont(object font);

        /// <summary>
        /// Updates native transform matrix properties.
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public abstract void UpdateTransformMatrix(
            object matrix,
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy);

        public abstract double GetTransformMatrixM11(object matrix);

        public abstract double GetTransformMatrixM12(object matrix);

        public abstract double GetTransformMatrixM21(object matrix);

        public abstract double GetTransformMatrixM22(object matrix);

        public abstract double GetTransformMatrixDX(object matrix);

        public abstract double GetTransformMatrixDY(object matrix);

        public abstract bool GetTransformMatrixIsIdentity(object matrix);

        public abstract void ResetTransformMatrix(object matrix);

        public abstract void MultiplyTransformMatrix(object matrix1, object matrix2);

        public abstract void TranslateTransformMatrix(object matrix, double offsetX, double offsetY);

        public abstract void ScaleTransformMatrix(object matrix, double scaleX, double scaleY);

        public abstract void RotateTransformMatrix(object matrix, double angle);

        public abstract void InvertTransformMatrix(object matrix);

        public abstract void SetTransformMatrixM11(object matrix, double value);

        public abstract void SetTransformMatrixM12(object matrix, double value);

        public abstract void SetTransformMatrixM21(object matrix, double value);

        public abstract void SetTransformMatrixM22(object matrix, double value);

        public abstract void SetTransformMatrixDX(object matrix, double value);

        public abstract void SetTransformMatrixDY(object matrix, double value);

        public abstract PointD TransformMatrixOnPoint(object matrix, PointD point);

        public abstract SizeD TransformMatrixOnSize(object matrix, SizeD size);

        /// <summary>
        /// Indicates whether native transform matrix is equal to another native transform matrix.
        /// </summary>
        /// <returns></returns>
        public abstract bool TransformMatrixEquals(object matrix1, object matrix2);

        public abstract int TransformMatrixGetHashCode(object matrix);

        /// <summary>
        /// Creates native transform matrix.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTransformMatrix();

        /// <summary>
        /// Creates default native mono font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateDefaultMonoFont();

        /// <summary>
        /// Creates native pen.
        /// </summary>
        /// <returns></returns>
        public abstract object CreatePen();

        /// <summary>
        /// Creates native transparent brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTransparentBrush();

        /// <summary>
        /// Creates native hatch brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateHatchBrush();

        /// <summary>
        /// Creates native linear gradient brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateLinearGradientBrush();

        /// <summary>
        /// Creates native radial gradient brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateRadialGradientBrush();

        /// <summary>
        /// Creates native solid brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateSolidBrush();

        /// <summary>
        /// Creates native texture brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTextureBrush();

        /// <summary>
        /// Updates native pen properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdatePen(Pen pen);

        /// <summary>
        /// Updates native solid brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateSolidBrush(SolidBrush brush);

        /// <summary>
        /// Updates native font properties.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <param name="prm">Font properties.</param>
        /// <returns></returns>
        public abstract void UpdateFont(object font, FontParams prm);

        /// <summary>
        /// Updates native hatch brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateHatchBrush(HatchBrush brush);

        /// <summary>
        /// Updates native linear gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateLinearGradientBrush(LinearGradientBrush brush);

        /// <summary>
        /// Updates native radial gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateRadialGradientBrush(RadialGradientBrush brush);

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public abstract Color GetColor(SystemSettingsColor index);

        /// <summary>
        /// Returns a string array that contains all font families names
        /// currently available in the system.
        /// </summary>
        public abstract string[] GetFontFamiliesNames();

        /// <summary>
        /// Gets whether font family is installed on this computer.
        /// </summary>
        public abstract bool IsFontFamilyValid(string name);

        /// <summary>
        /// Gets the name of the font family specified using <see cref="GenericFontFamily"/>.
        /// </summary>
        public abstract string GetFontFamilyName(GenericFontFamily genericFamily);

        /// <summary>
        /// Gets default font encoding.
        /// </summary>
        /// <returns></returns>
        public abstract int GetDefaultFontEncoding();

        /// <summary>
        /// Sets default font encoding.
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetDefaultFontEncoding(int value);

        /// <summary>
        /// Gets native font name.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract string GetFontName(object font);

        /// <summary>
        /// Gets native font encoding.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract int GetFontEncoding(object font);

        /// <summary>
        /// Gets native font size in pixels.
        /// </summary>
        public abstract SizeI GetFontSizeInPixels(object font);

        /// <summary>
        /// Gets whether native font is using size in pixels.
        /// </summary>
        public abstract bool GetFontIsUsingSizeInPixels(object font);

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public abstract int GetFontNumericWeight(object font);

        /// <summary>
        /// Gets whether native font is a fixed width (or monospaced) font.
        /// </summary>
        public abstract bool GetFontIsFixedWidth(object font);

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <returns></returns>
        public abstract FontWeight GetFontWeight(object font);

        /// <summary>
        /// Gets style information for the native font.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract FontStyle GetFontStyle(object font);

        /// <summary>
        /// Gets a value that indicates whether native font is strikethrough.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetFontStrikethrough(object font);

        /// <summary>
        /// Gets a value that indicates whether native font is underlined.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetFontUnderlined(object font);

        /// <summary>
        /// Gets the em-size, in points, of the native font.
        /// </summary>
        /// <returns></returns>
        public abstract double GetFontSizeInPoints(object font);

        /// <summary>
        /// Gets native font description string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetFontInfoDesc(object font);

        /// <summary>
        /// Indicates whether native font is equal to another native font.
        /// </summary>
        /// <returns></returns>
        public abstract bool FontEquals(object font1, object font2);

        /// <summary>
        /// Returns a string that represents native font.
        /// </summary>
        /// <returns></returns>
        public abstract string FontToString(object font);

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
