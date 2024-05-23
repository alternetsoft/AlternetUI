using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ITimerHandler : IDisposable
    {
        bool Enabled { get; set; }

        int Interval { get; set; }

        Action? Tick { get; set; }
    }
}
