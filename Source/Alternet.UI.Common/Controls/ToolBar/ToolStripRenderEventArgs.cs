using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>Represents the method that will handle the ToolStgrip rendering.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="ToolStripRenderEventArgs" /> that
    /// contains the event data.</param>
    public delegate void ToolStripRenderEventHandler(object? sender, ToolStripRenderEventArgs e);

    /// <summary>
    /// Provides data for the ToolStrip rendering methods and events.
    /// </summary>
    public class ToolStripRenderEventArgs : EventArgs
    {
        private readonly AbstractControl toolStrip;
        private readonly Graphics graphics;
        private RectD affectedBounds = RectD.Empty;
        private Color backColor = Color.Empty;

        /// <summary>Initializes a new instance of the
        /// <see cref="ToolStripRenderEventArgs"/> class for
        /// the specified control and using the specified <see cref="Graphics" />.</summary>
        /// <param name="g">The <see cref="Graphics" /> to use for painting.</param>
        /// <param name="toolStrip">The control to paint.</param>
        public ToolStripRenderEventArgs(Graphics g, AbstractControl toolStrip)
        {
            this.toolStrip = toolStrip;
            graphics = g;
            affectedBounds = new RectD(PointD.Empty, toolStrip.Size);
        }

        /// <summary>Initializes a new instance of the
        /// <see cref="ToolStripRenderEventArgs"/> class using the specified parameters.</summary>
        /// <param name="g">The <see cref="Graphics" /> to use for painting.</param>
        /// <param name="toolStrip">The control to paint.</param>
        /// <param name="affectedBounds">The <see cref="RectD" /> representing
        /// the bounds of the area to be painted.</param>
        /// <param name="backColor">The <see cref="Color" /> that the background
        /// of the control is painted with.</param>
        public ToolStripRenderEventArgs(
            Graphics g,
            Control toolStrip,
            RectD affectedBounds,
            Color backColor)
        {
            this.toolStrip = toolStrip;
            this.affectedBounds = affectedBounds;
            graphics = g;
            this.backColor = backColor;
        }

        /// <summary>Gets the <see cref="RectD" /> representing the bounds of the area
        /// to be painted.</summary>
        /// <returns>The <see cref="RectD" /> representing the bounds of the area
        /// to be painted.</returns>
        public RectD AffectedBounds => affectedBounds;

        /// <summary>Gets the <see cref="Color" /> that the background is painted with.</summary>
        /// <returns>The <see cref="Color" /> that the background  is painted with.</returns>
        public Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
            }
        }

        /// <summary>Gets the <see cref="Graphics" /> used to paint.</summary>
        /// <returns>The <see cref="Graphics" /> used to paint.</returns>
        public Graphics Graphics => graphics;

        /// <summary>Gets the control to be painted.</summary>
        /// <returns>The control to be painted.</returns>
        public AbstractControl ToolStrip => toolStrip;

        /// <summary>Gets the <see cref="RectD" /> representing
        /// the overlap area between a drop-down and its owner item/>.</summary>
        public RectD ConnectedArea
        {
            get;
            set;
        }
    }
}
