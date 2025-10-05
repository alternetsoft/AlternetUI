using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Used for the layout purposes to occupy some space.
    /// Doesn't perform any painting by default.
    /// </summary>
    public partial class Spacer : GenericControl
    {
        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        private bool hasBackground = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spacer"/> class.
        /// </summary>
        public Spacer()
        {
        }

        /// <summary>
        /// Gets or sets whether background of the control is actually painted.
        /// Default is False.
        /// </summary>
        public virtual bool HasBackground
        {
            get
            {
                return hasBackground;
            }

            set
            {
                if (hasBackground == value)
                    return;
                hasBackground = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (HasBackground)
            {
                DrawDefaultBackground(e);
                DefaultPaintDebug(e);
            }
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClientRectangle);
        }
    }
}
