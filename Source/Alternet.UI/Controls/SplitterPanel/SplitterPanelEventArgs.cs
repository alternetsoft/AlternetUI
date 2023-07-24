using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Provides data for the <see cref="SplitterPanel"/> events.
    /// </summary>
    public class SplitterPanelEventArgs : CancelEventArgs
    {
        internal SplitterPanelEventArgs(
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SashPosition = e.Data.SashPosition;
            OldSize = e.Data.OldSize;
            NewSize = e.Data.NewSize;
            X = e.Data.X;
            Y = e.Data.Y;
        }

        /// <summary>
        /// Gets or sets the new sash position.
        /// </summary>
        /// <remarks>
        /// May only be called while processing
        /// <see cref="SplitterPanel.SplitterMoving"/> and
        /// <see cref="SplitterPanel.SplitterMoved"/> events.
        /// </remarks>
        /// <remarks>
        /// In the case of <see cref="SplitterPanel.SplitterMoved"/> events,
        /// sets the new sash position.
        /// </remarks>
        /// <remarks>
        /// In the case of <see cref="SplitterPanel.SplitterMoving"/> events,
        /// sets the new tracking bar position so visual feedback during
        /// dragging will represent that change that will actually
        /// take place. Set to -1 from the event handler code to
        /// prevent repositioning.
        /// </remarks>
        public int SashPosition { get; set; }

        /// <summary>
        /// Returns the x coordinate of the double-click point.
        /// </summary>
        /// <remarks>
        /// May only be called while processing
        /// <see cref="SplitterPanel.SplitterDoubleClick"/> events.
        /// </remarks>
        public int X { get; }

        /// <summary>
        /// Returns the y coordinate of the double-click point.
        /// </summary>
        /// <remarks>
        /// May only be called while processing
        /// <see cref="SplitterPanel.SplitterDoubleClick"/> events.
        /// </remarks>
        public int Y { get; }

        internal int OldSize { get; set; }

        internal int NewSize { get; set; }

        internal IntPtr CancelAsIntPtr()
        {
            return Cancel ? (IntPtr)1 : IntPtr.Zero;
        }
    }
}