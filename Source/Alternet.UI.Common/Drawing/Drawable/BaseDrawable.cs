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
        /// <summary>
        /// Represents a delegate that is invoked before the paint operation occurs.
        /// </summary>
        /// <remarks>This delegate can be used to perform custom actions or
        /// modifications prior to the
        /// paint operation. Assign a method to this delegate
        /// to handle pre-paint logic.</remarks>
        public PaintDelegate? BeforePaint;

        /// <summary>
        /// Represents a delegate that is invoked after the painting operation is completed.
        /// </summary>
        /// <remarks>This delegate can be used to perform additional operations
        /// or customizations after
        /// the primary painting logic has been executed.
        /// The specific parameters and behavior of the delegate depend
        /// on the implementation of the painting logic.</remarks>
        public PaintDelegate? AfterPaint;

        private RectD bounds;

        /// <summary>
        /// Represents a method that handles custom painting for a drawable control.
        /// </summary>
        /// <param name="prm">The parameters for the paint operation.</param>
        public delegate void PaintDelegate(ref PaintDelegateParams prm);

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
        public void Draw(AbstractControl control, Graphics dc)
        {
            PaintDelegateParams prm = new()
            {
                Drawable = this,
                Control = control,
                Graphics = dc,
                Handled = false,
            };

            if (BeforePaint is not null)
            {
                BeforePaint(ref prm);
                if(prm.Handled)
                    return;
            }

            OnDraw(control, dc);

            AfterPaint?.Invoke(ref prm);
        }

        /// <summary>
        /// Performs custom drawing operations for the specified control.
        /// </summary>
        /// <remarks>This method is intended to be overridden in a derived class
        /// to provide custom drawing logic.
        /// The base implementation does not perform any drawing operations.</remarks>
        /// <param name="control">The control to be drawn.
        /// This parameter cannot be <see langword="null"/>.</param>
        /// <param name="dc">The graphics context used for rendering.
        /// This parameter cannot be <see langword="null"/>.</param>
        protected virtual void OnDraw(AbstractControl control, Graphics dc)
        {
        }

        /// <summary>
        /// Contains parameters for the <see cref="PaintDelegate"/> used in custom
        /// painting of a drawable object.
        /// </summary>
        public struct PaintDelegateParams
        {
            /// <summary>
            /// Gets or sets the <see cref="BaseDrawable"/> instance being painted.
            /// </summary>
            public BaseDrawable Drawable;

            /// <summary>
            /// Gets or sets the <see cref="AbstractControl"/> in which the drawable is painted.
            /// </summary>
            public AbstractControl Control;

            /// <summary>
            /// Gets or sets the <see cref="Graphics"/> context used for painting.
            /// </summary>
            public Graphics Graphics;

            /// <summary>
            /// Gets or sets a value indicating whether the paint event was handled.
            /// </summary>
            public bool Handled;
        }
    }
}
