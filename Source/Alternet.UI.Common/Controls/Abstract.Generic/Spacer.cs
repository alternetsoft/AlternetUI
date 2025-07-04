using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Used for the layout purposes to occupy some space.
    /// Doesn't perform any painting.
    /// </summary>
    public partial class Spacer : GenericControl
    {
        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spacer"/> class.
        /// </summary>
        public Spacer()
        {
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            DrawDefaultBackground(e);
            DefaultPaintDebug(e);
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClipRectangle);
        }
    }
}
