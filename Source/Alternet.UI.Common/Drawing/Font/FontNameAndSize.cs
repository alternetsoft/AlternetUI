using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public struct FontNameAndSize
    {
        public string Name;

        public FontSize Size;

        public FontNameAndSize(string name, FontSize size)
        {
            Name = name;
            Size = size;
        }

        public static FontNameAndSize Default => (FontNameAndSize)Font.Default;

        public static FontNameAndSize SkiaOrDefault(FontNameAndSize value)
        {
            if (SkiaUtils.IsFamilySkia(value.Name))
                return value;
            return Default;
        }

        public static implicit operator FontInfo(FontNameAndSize value)
        {
            return new(value.Name, value.Size);
        }
    }
}
