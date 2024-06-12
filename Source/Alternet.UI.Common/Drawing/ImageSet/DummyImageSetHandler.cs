using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class DummyImageSetHandler : DummyImageContainer, IImageSetHandler
    {
        public static IImageSetHandler Default = new DummyImageSetHandler();

        public SizeI DefaultSize { get; }

        public SizeI GetPreferredBitmapSizeAtScale(double scale)
        {
            return new((int)(DefaultSize.Width * scale), (int)(DefaultSize.Height * scale));
        }

        public SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return GetPreferredBitmapSizeAtScale(control.ScaleFactor);
        }
    }
}
