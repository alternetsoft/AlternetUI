using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing : NativeDrawing
    {
        public static SKColor NullColor = new();

        public static SKColor Convert(Color color)
        {
            if (color is null || !color.IsOk)
                return NullColor;

            if (color.NativeObject is not null)
                return (SKColor)color.NativeObject;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var skColor = new SKColor(r, g, b, a);
            color.NativeObject = skColor;
            return skColor;
        }

        /// <inheritdoc/>
        public override Graphics CreateGraphicsFromScreen()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Graphics CreateGraphicsFromImage(Image image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreatePen()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override void UpdatePen(Pen pen)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override Color GetColor(SystemSettingsColor index)
        {
            return NotImplemented<Color>();
        }
   }
}