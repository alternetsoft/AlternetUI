using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        /// <inheritdoc/>
        public override object CreateFont() => new UI.Native.Font();

        /// <inheritdoc/>
        public override object CreateDefaultFont()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultFont();
            return result;
        }

        /// <inheritdoc/>
        public override Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            return SystemSettings.GetFont(systemFont);
        }

        /// <inheritdoc/>
        public override string[] GetFontFamiliesNames()
        {
            return UI.Native.Font.Families;
        }

        /// <inheritdoc/>
        public override bool IsFontFamilyValid(string name)
        {
            return UI.Native.Font.IsFamilyValid(name);
        }

        /// <inheritdoc/>
        public override string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            return UI.Native.Font.GetGenericFamilyName((UI.Native.GenericFontFamily)genericFamily);
        }

        /// <inheritdoc/>
        public override FontStyle GetFontStyle(object font)
        {
            return (FontStyle)((UI.Native.Font)font).Style;
        }

        /// <inheritdoc/>
        public override bool GetFontStrikethrough(object font)
        {
            return ((UI.Native.Font)font).GetStrikethrough();
        }

        /// <inheritdoc/>
        public override bool GetFontUnderlined(object font)
        {
            return ((UI.Native.Font)font).GetUnderlined();
        }

        /// <inheritdoc/>
        public override string FontToString(object font)
        {
            return ((UI.Native.Font)font).Description;
        }

        /// <inheritdoc/>
        public override double GetFontSizeInPoints(object font)
        {
            return ((UI.Native.Font)font).SizeInPoints;
        }

        /// <inheritdoc/>
        public override bool FontEquals(object font1, object font2)
        {
            return ((UI.Native.Font)font1).IsEqualTo((UI.Native.Font)font2);
        }

        /// <inheritdoc/>
        public override string GetFontInfoDesc(object font)
        {
            return ((UI.Native.Font)font).Serialize();
        }

        /// <inheritdoc/>
        public override object CreateFont(object font)
        {
            var result = CreateFont();
            ((UI.Native.Font)result).InitializeFromFont((UI.Native.Font)font);
            return result;
        }

        /// <inheritdoc/>
        public override object CreateDefaultMonoFont()
        {
            var result = new UI.Native.Font();
            result.InitializeWithDefaultMonoFont();
            return result;
        }

        /// <inheritdoc/>
        public override void UpdateFont(object font, FontParams prm)
        {
            if (prm.Unit != GraphicsUnit.Point)
            {
                prm.Size = GraphicsUnitConverter.Convert(
                    prm.Unit,
                    GraphicsUnit.Point,
                    Display.Primary.DPI.Height,
                    prm.Size);
            }

            if (prm.GenericFamily == null && prm.FamilyName == null)
            {
                Application.LogError("Font name and family are null, using default font.");
                prm.GenericFamily = GenericFontFamily.Default;
            }

            prm.Size = Font.CheckSize(prm.Size);

            ((UI.Native.Font)font).Initialize(
               ToNativeGenericFamily(prm.GenericFamily),
               prm.FamilyName,
               prm.Size,
               (UI.Native.FontStyle)prm.Style);

            static UI.Native.GenericFontFamily ToNativeGenericFamily(
                GenericFontFamily? value)
            {
                return value == null ?
                    UI.Native.GenericFontFamily.None :
                    (UI.Native.GenericFontFamily)value;
            }
        }

        /// <inheritdoc/>
        public override SizeI GetFontSizeInPixels(object font)
        {
            return ((UI.Native.Font)font).GetPixelSize();
        }

        /// <inheritdoc/>
        public override bool GetFontIsUsingSizeInPixels(object font)
        {
            return ((UI.Native.Font)font).IsUsingSizeInPixels();
        }

        /// <inheritdoc/>
        public override int GetFontNumericWeight(object font)
        {
            return ((UI.Native.Font)font).GetNumericWeight();
        }

        /// <inheritdoc/>
        public override bool GetFontIsFixedWidth(object font)
        {
            return ((UI.Native.Font)font).IsFixedWidth();
        }

        /// <inheritdoc/>
        public override FontWeight GetFontWeight(object font)
        {
            return (FontWeight)((UI.Native.Font)font).GetWeight();
        }

        /// <inheritdoc/>
        public override string GetFontName(object font)
        {
            return ((UI.Native.Font)font).Name;
        }

        /// <inheritdoc/>
        public override int GetFontEncoding(object font)
        {
            return ((UI.Native.Font)font).GetEncoding();
        }

        /// <inheritdoc/>
        public override int GetDefaultFontEncoding()
        {
            return UI.Native.Font.GetDefaultEncoding();
        }

        /// <inheritdoc/>
        public override void SetDefaultFontEncoding(int value)
        {
            UI.Native.Font.SetDefaultEncoding(value);
        }
    }
}
