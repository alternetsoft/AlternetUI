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
        public override void SetDefaultFont(Font value)
        {
            UI.Native.Window.SetParkingWindowFont((UI.Native.Font?)value?.NativeObject);
        }

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
        public override FontStyle GetFontStyle(Font fnt)
        {
            return (FontStyle)((UI.Native.Font)fnt.NativeObject).Style;
        }

        /// <inheritdoc/>
        public override bool GetFontStrikethrough(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).GetStrikethrough();
        }

        /// <inheritdoc/>
        public override bool GetFontUnderlined(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).GetUnderlined();
        }

        /// <inheritdoc/>
        public override string FontToString(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).Description;
        }

        /// <inheritdoc/>
        public override double GetFontSizeInPoints(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).SizeInPoints;
        }

        /// <inheritdoc/>
        public override bool FontEquals(Font fnt1, Font fnt2)
        {
            return ((UI.Native.Font)fnt1.NativeObject).IsEqualTo((UI.Native.Font)fnt2.NativeObject);
        }

        /// <inheritdoc/>
        public override string GetFontInfoDesc(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).Serialize();
        }

        /// <inheritdoc/>
        public override object CreateFont(Font font)
        {
            var result = CreateFont();
            ((UI.Native.Font)result).InitializeFromFont((UI.Native.Font)font.NativeObject);
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
        public override void UpdateFont(Font fnt, FontParams prm)
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

            ((UI.Native.Font)fnt.NativeObject).Initialize(
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
        public override SizeI GetFontSizeInPixels(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).GetPixelSize();
        }

        /// <inheritdoc/>
        public override bool GetFontIsUsingSizeInPixels(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).IsUsingSizeInPixels();
        }

        /// <inheritdoc/>
        public override int GetFontNumericWeight(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).GetNumericWeight();
        }

        /// <inheritdoc/>
        public override bool GetFontIsFixedWidth(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).IsFixedWidth();
        }

        /// <inheritdoc/>
        public override FontWeight GetFontWeight(Font fnt)
        {
            return (FontWeight)((UI.Native.Font)fnt.NativeObject).GetWeight();
        }

        /// <inheritdoc/>
        public override string GetFontName(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).Name;
        }

        /// <inheritdoc/>
        public override int GetFontEncoding(Font fnt)
        {
            return ((UI.Native.Font)fnt.NativeObject).GetEncoding();
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
