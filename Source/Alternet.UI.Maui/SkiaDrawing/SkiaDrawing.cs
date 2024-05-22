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
   }
}