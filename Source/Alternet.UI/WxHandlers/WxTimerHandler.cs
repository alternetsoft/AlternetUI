using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class Timer : ITimerHandler
    {
        Action? ITimerHandler.Tick
        {
            get => Tick;
            set => Tick = value;
        }
    }
}