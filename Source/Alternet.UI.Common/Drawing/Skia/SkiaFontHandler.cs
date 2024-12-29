using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements <see cref="IFontHandler"/> for the SkiaSharp font.
    /// </summary>
    internal class SkiaFontHandler : PlessFontHandler
    {
        private bool? isFixedFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontHandler"/> class.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="sizeInPoints">Font size in points.</param>
        public SkiaFontHandler(string name, Coord sizeInPoints)
        {
            this.Name = name;
            this.SizeInPoints = sizeInPoints;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontHandler"/> class.
        /// </summary>
        public SkiaFontHandler()
        {
        }

        /// <inheritdoc/>
        public override bool IsFixedWidth(Font font)
        {
            return isFixedFont ??= font.SkiaFont.Typeface.IsFixedPitch;
        }

        public override void Changed()
        {
            isFixedFont = null;
            base.Changed();
        }
    }
}
