using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class SystemTimerHandler : DisposableObject, ITimerHandler
    {
        public bool Enabled { get; set; }

        public int Interval { get; set; }

        public Action? Tick { get; set; }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}
