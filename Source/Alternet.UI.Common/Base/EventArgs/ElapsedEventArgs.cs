using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="Timer.Elapsed" /> event of
    /// a <see cref="Timer" />.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="ElapsedEventArgs" /> object that
    /// contains the event data.</param>
    public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="Timer.Elapsed" /> event.
    /// </summary>
    public class ElapsedEventArgs : BaseEventArgs
    {
        private readonly DateTime signalTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedEventArgs"/> class.
        /// </summary>
        public ElapsedEventArgs()
        {
            signalTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the date/time when the <see cref="Timer.Elapsed" /> event was raised.</summary>
        /// <returns>The time the <see cref="Timer.Elapsed" /> event was raised.</returns>
        public DateTime SignalTime => signalTime;
    }
}
