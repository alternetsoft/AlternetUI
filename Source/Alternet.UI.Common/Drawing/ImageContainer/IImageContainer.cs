using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IImageContainer : IDisposable
    {
        bool IsDummy { get; }

        bool IsOk { get; }

        bool IsReadOnly { get; }

        bool Add(Image image);

        bool Remove(int imageIndex);

        bool Clear();
    }
}
