using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines the type of shape to be drawn.
    /// </summary>
    public enum DrawingShapeType
    {
        /// <summary>
        /// Indicates that no shape should be drawn.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that a rectangle shape should be drawn.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Indicates that an ellipse shape should be drawn.
        /// </summary>
        Ellipse,

        /// <summary>
        /// Indicates that a rounded rectangle shape should be drawn.
        /// </summary>
        RoundedRectangle,

        /// <summary>
        /// Indicates that a pie shape should be drawn.
        /// </summary>
        Pie,

        /// <summary>
        /// Indicates that a circle shape should be drawn.
        /// </summary>
        Circle,

        /// <summary>
        /// Indicates that an arc shape should be drawn.
        /// </summary>
        Arc,
    }
}
