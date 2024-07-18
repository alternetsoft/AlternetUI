using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties which allow to work with timer.
    /// </summary>
    public interface ITimerHandler : IDisposable
    {
        /// <inheritdoc cref="Timer.Enabled"/>
        bool Enabled { get; set; }

        /// <inheritdoc cref="Timer.Interval"/>
        int Interval { get; set; }

        /// <summary>
        /// Gets or sets action which is called on timer tick.
        /// </summary>
        Action? Tick { get; set; }
    }
}
