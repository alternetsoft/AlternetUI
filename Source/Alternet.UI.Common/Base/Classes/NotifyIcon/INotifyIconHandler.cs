using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface INotifyIconHandler : IDisposable
    {
        string? Text { get; set; }

        Action? Click { get; set; }

        Action? DoubleClick { get; set; }

        Image? Icon { set; }

        ContextMenu? Menu { set; }

        bool Visible { get; set; }

        bool IsIconInstalled { get; }

        bool IsOk { get; }
    }
}