using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IImageSetHandler : IDisposable
    {
        SizeI DefaultSize { get; }

        bool IsOk { get; }

        bool IsReadOnly { get; }

        SizeI GetPreferredBitmapSizeFor(IControl control);

        SizeI GetPreferredBitmapSizeAtScale(Coord scale);

        void Add(Image item);

        void LoadFromStream(Stream stream);
    }
}
