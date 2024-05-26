using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IImageListHandler : IDisposable
    {
        SizeI PixelImageSize { get; set; }

        SizeD ImageSize { get; set; }

        void Add(Image item);
    }
}
