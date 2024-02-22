﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the help request events.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="hlpevent">A <see cref="HelpEventArgs" /> that contains the event data.</param>
    public delegate void HelpEventHandler(object? sender, HelpEventArgs hlpevent);

    /// <summary>
    /// Provides data for the help request events.
    /// </summary>
    public class HelpEventArgs : HandledEventArgs
    {
        private readonly PointD mousePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpEventArgs" /> class.</summary>
        /// <param name="mousePos">The coordinates of the mouse pointer.</param>
        public HelpEventArgs(PointD mousePos = default)
        {
            this.mousePos = mousePos;
        }

        /// <summary>
        /// Gets the screen coordinates of the mouse pointer.
        /// </summary>
        /// <returns>A <see cref="PointD" /> representing the screen coordinates
        /// of the mouse pointer.
        /// </returns>
        public PointD MousePos => mousePos;
    }
}
