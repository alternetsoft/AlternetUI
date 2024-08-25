using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for the drawable objects.
    /// </summary>
    public class BaseDrawable : BaseObject, IBaseDrawable
    {
        private RectD bounds;

        /// <inheritdoc/>
        public PointD Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new(value, Bounds.Size);
            }
        }

        /// <inheritdoc/>
        public SizeD Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Bounds = new(Bounds.Location, value);
            }
        }

        /// <inheritdoc/>
        public bool Visible { get; set; } = true;

        /// <inheritdoc/>
        public virtual RectD Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }

        /// <inheritdoc/>
        public virtual void Draw(Control control, Graphics dc)
        {
        }
    }
}
