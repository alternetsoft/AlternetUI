using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for primitive painters.
    /// </summary>
    public class PrimitivePainter : BaseObject
    {
        /// <summary>
        /// Gets or sets destination point where to draw the primitive.
        /// </summary>
        public PointD DestPoint;

        /// <summary>
        /// Gets or sets destination size of the primitive.
        /// </summary>
        public SizeD? Size;

        /// <summary>
        /// Gets or sets whether this object is visible.
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Gets whether or not to stretch this object. Default is <c>true</c>.
        /// </summary>
        public bool Stretch = true;

        /// <summary>
        /// Gets whether or not to center this object vertically. Default is <c>true</c>.
        /// </summary>
        public bool CenterVert = true;

        /// <summary>
        /// Gets whether or not to center this object horizontally. Default is <c>true</c>.
        /// </summary>
        public bool CenterHorz = true;

        /// <summary>
        /// Gets or sets destination rectangle where to draw the image.
        /// </summary>
        public virtual RectD DestRect
        {
            get
            {
                if (Size is null)
                {
                    return new RectD(DestPoint, Drawing.SizeD.Empty);
                }
                else
                    return new RectD(DestPoint, Size.Value);
            }

            set
            {
                DestPoint = value.TopLeft;
                Size = value.Size;
            }
        }

        /// <summary>
        /// Gets whether or not center this object horizontally or vertically.
        /// </summary>
        public virtual bool CenterHorzOrVert => CenterHorz || CenterVert;

        /// <summary>
        /// Draws primitive on the canvas.
        /// </summary>
        /// <param name="control">Control in which primitive is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void Draw(Control control, Graphics dc)
        {
        }
    }
}
