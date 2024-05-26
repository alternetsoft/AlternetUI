using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IIconSetHandler : IDisposable
    {
        bool IsOk { get; }

        void Add(Image image);

        void Add(Stream stream);

        void Clear();
    }
}