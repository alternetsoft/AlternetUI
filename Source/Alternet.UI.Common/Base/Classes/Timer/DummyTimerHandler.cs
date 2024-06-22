using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implementation of the <see cref="ITimerHandler"/> which does nothing.
    /// </summary>
    public class DummyTimerHandler : DisposableObject, ITimerHandler
    {
        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <inheritdoc/>
        public int Interval { get; set; }

        /// <inheritdoc/>
        public Action? Tick { get; set; }
    }
}
