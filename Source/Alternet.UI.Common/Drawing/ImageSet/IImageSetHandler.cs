using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IImageSetHandler : IImageContainer
    {
        SizeI DefaultSize { get; }

        SizeI GetPreferredBitmapSizeFor(IControl control);

        SizeI GetPreferredBitmapSizeAtScale(Coord scale);

        bool LoadFromStream(Stream stream);
    }
}
