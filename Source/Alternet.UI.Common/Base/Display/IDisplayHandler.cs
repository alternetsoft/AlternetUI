using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IDisplayHandler : IDisposable
    {
        public bool IsOk { get; }

        public string GetName();

        public double GetScaleFactor();

        public bool IsPrimary();

        public RectI GetClientArea();

        public RectI GetGeometry();
    }
}
