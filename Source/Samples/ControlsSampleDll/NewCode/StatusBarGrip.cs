using System;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar grip control that allows the user to resize the target control 
    /// (or the top-level parent form if no target is specified) by dragging the grip.
    /// The grip can be configured to allow resizing in both directions, or only horizontally or vertically.
    /// The minimum size delta for resizing can also be customized.
    /// The control is typically placed in the bottom-right corner of a status bar, but can be used in other contexts as well.
    /// </summary>
    public class StatusBarGrip : UserControl
    {
        public static float DefaultMinSizeDelta = 10;
        
        public static float DefaultSuggestedSize = 16;

        private float? minSizeDelta;
        private PointD previousMousePos;
        private RectD origTargetBounds;
        private bool resizing = false;
        private PointD mouseDownPos;

        /// <summary>
        /// Gets or sets the control that is resized. If null, the top-level parent form is used.
        /// </summary>
        public Control? Target { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBarGrip"/> class.
        /// </summary>
        public StatusBarGrip()
        {
            var styles = ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint;

            this.SetStyle(styles, true);
            this.SuggestedSize = new (DefaultSuggestedSize, DefaultSuggestedSize);
            this.Cursor = Cursors.SizeNWSE;
            Alignment = HVAlignment.BottomRight;
            this.TabStop = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the grip can resize vertically. If false, only horizontal resizing is allowed.
        /// Default is true.
        /// </summary>
        public virtual bool CanResizeVertically { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the grip can resize horizontally. If false, only vertical resizing is allowed.
        /// Default is true.
        /// </summary>
        public virtual bool CanResizeHorizontally { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum size delta for resizing. If null, the default value is used.
        /// </summary>
        public virtual float? MinSizeDelta
        {
            get => minSizeDelta;
            set
            {
                if (value <= 0) value = null;
                minSizeDelta = value;
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(Color.Red.AsBrush, this.ClientRectangle);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                var target = GetTarget();

                if (target != null)
                {
                    resizing = true;
                    mouseDownPos = ClientToScreen(e.Location);
                    previousMousePos = mouseDownPos;
                    origTargetBounds = target?.Bounds ?? RectD.Empty;
                    this.CaptureMouse();
                }
            }
        }

        ///<summary>
        /// Gets the effective minimum size delta for resizing, considering the user-defined value and the default value.
        /// </summary>
        protected virtual float GetEffectiveMinDelta()
        {
            return MinSizeDelta ?? DefaultMinSizeDelta;
        }

        /// <summary>
        /// Calculates the new size for the target control based on the current mouse position
        /// and the original size, while respecting the minimum size delta and the target's
        /// minimum and maximum size constraints.
        /// </summary>
        /// <param name="isVert">Indicates whether the resizing is vertical.</param>
        /// <param name="mouseNowPos">The current mouse position.</param>
        /// <returns>The new size for the target control.</returns>
        protected virtual float GetNewSize(bool isVert, PointD mouseNowPos)
        {
            var oldSize = origTargetBounds.GetSize(isVert);
            var target = GetTarget();
            if (target == null) return oldSize;

            var nowPos = mouseNowPos.GetLocation(isVert);
            var prevPos = mouseDownPos.GetLocation(isVert);

            var delta = MathF.Round(nowPos - prevPos);

            var absDelta = MathF.Abs(delta);

            var minDelta = GetEffectiveMinDelta();

            if (absDelta < minDelta)
            {
                return oldSize;
            }

            var newSize = oldSize + delta;

            newSize = MathF.Floor(newSize / minDelta) * minDelta;

            var maxSize = target.MaximumSize.GetSize(isVert);
            var minSize = target.MinimumSize.GetSize(isVert);

            newSize = Math.Max(minSize, newSize);
            if (maxSize > 0) newSize = Math.Min(newSize, maxSize);

            return newSize;
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (resizing)
            {
                var target = GetTarget();
                if (target == null) return;
                PointD mouseNowPos = ClientToScreen(e.Location);

                float newW;
                float newH;

                if (CanResizeHorizontally)
                {
                    newW = GetNewSize(isVert: false, mouseNowPos);
                }
                else
                {
                    newW = target.Size.Width;
                }

                if (CanResizeVertically)
                {
                    newH = GetNewSize(isVert: true, mouseNowPos);
                }
                else
                {
                    newH = target.Size.Height;
                }

                previousMousePos = mouseNowPos;

                var newSize = new SizeD(newW, newH);

                if (newSize == target.Size) return;

                target.Size = newSize;
                target.Refresh();
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseCaptureLost(EventArgs e)
        {
            base.OnMouseCaptureLost(e);

            if (resizing)
            {
                resizing = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (resizing && e.Button == MouseButtons.Left)
            {
                resizing = false;
                this.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Gets the target control to be resized. If the Target property is set and valid, it returns that.
        /// </summary>
        /// <returns>The target control to be resized, or the top-level parent if the Target property is not set or invalid.</returns>
        protected virtual AbstractControl? GetTarget()
        {
            if (Target != null && !Target.DisposingOrDisposed)
                return Target;
            else
                return FindTopLevelParent(this);
        }

        /// <summary>
        /// Finds the top-level parent of the specified control in case the Target property is not set or invalid.
        /// This method traverses up the control hierarchy to find the top-level parent control,
        /// which is typically a Form, UserControl, or ContainerControl.
        /// </summary>
        /// <param name="c">The control for which to find the top-level parent.</param>
        /// <returns>The top-level parent control, or null if not found.</returns>
        protected virtual AbstractControl? FindTopLevelParent(AbstractControl c)
        {
            while (c.Parent != null) c = c.Parent;
            return c is Form || c is UserControl || c is ContainerControl ? c : null;
        }
    }
}