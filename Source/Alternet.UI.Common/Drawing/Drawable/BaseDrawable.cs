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
    public partial class BaseDrawable : BaseObject, IBaseDrawable
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
        public virtual bool Visible { get; set; } = true;

        /// <inheritdoc/>
        public bool Enabled
        {
            get
            {
                return VisualState != VisualControlState.Disabled;
            }
        }

        /// <summary>
        /// Gets or sets visual state.
        /// </summary>
        public virtual VisualControlState VisualState { get; set; } = VisualControlState.Normal;

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
        public void SetVisible(bool value) => Visible = value;

        /// <inheritdoc/>
        public virtual void Draw(AbstractControl control, Graphics dc)
        {
        }
    }
}
