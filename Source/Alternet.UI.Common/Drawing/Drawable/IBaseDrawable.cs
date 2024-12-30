using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides access to methods and properties of the drawable object.
    /// </summary>
    public interface IBaseDrawable
    {
        /// <summary>
        /// Gets or sets visual state of the object.
        /// </summary>
        VisualControlState VisualState { get; set; }

        /// <summary>
        /// Gets or sets location where to draw this object.
        /// </summary>
        PointD Location { get; set; }

        /// <summary>
        /// Gets or sets size of this object.
        /// </summary>
        SizeD Size { get; set; }

        /// <summary>
        /// Gets or sets whether this object is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets whether this object is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets or sets destination rectangle where to draw the object.
        /// </summary>
        RectD Bounds { get; set; }

        /// <summary>
        /// Draws object on the canvas.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <param name="dc">Drawing context.</param>
        void Draw(AbstractControl control, Graphics dc);

        /// <summary>
        /// Sets whether this object is visible.
        /// </summary>
        void SetVisible(bool value);
    }
}
