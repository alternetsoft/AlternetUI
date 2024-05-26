using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ICaretHandler : IDisposable
    {
        int BlinkTime { get; set; }

        SizeI Size { get; set; }

        PointI Position { get; set; }

        bool IsOk { get; }

        bool Visible { get; set; }

        Control? Control { get; }
    }
}
