using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a grip component that allows the user to resize or move the target control 
    /// (or the top-level parent form if no target is specified) by dragging the grip.
    /// The grip can be configured to allow resizing in both directions, or only horizontally or vertically.
    /// The minimum size delta for resizing can also be customized.
    /// The control is typically placed in the bottom-right corner of a status bar,
    /// but can be used in other contexts as well.
    /// </summary>
    public class GripComponent : FrameworkElement
    {
        private float? minSizeDelta;
        private float? minPositionDelta;
        private RectD origTargetBounds;
        private bool resizing = false;
        private PointD mouseDownPos;
        private Control? interactionControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GripComponent"/> class.
        /// </summary>
        public GripComponent()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether to invert the width delta when resizing horizontally.
        /// </summary>
        [Browsable(false)]
        public bool InvertWidthDelta { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to invert the height delta when resizing vertically.
        /// </summary>
        [Browsable(false)]
        public bool InvertHeightDelta { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to invert the left position delta when moving horizontally.
        /// </summary>
        [Browsable(false)]
        public bool InvertLeftDelta { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to invert the top position delta when moving vertically.
        /// </summary>
        [Browsable(false)]
        public bool InvertTopDelta { get; set; } = false;

        /// <summary>
        /// Gets or sets the control whch events are being handled by the grip.
        /// </summary>
        public virtual Control? InteractionControl
        {
            get => interactionControl;

            set
            {
                if (interactionControl == value) return;

                if (interactionControl != null)
                {
                    interactionControl.MouseDown -= OnMouseDown;
                    interactionControl.MouseMove -= OnMouseMove;
                    interactionControl.MouseUp -= OnMouseUp;
                    interactionControl.MouseCaptureLost -= OnMouseCaptureLost;
                    interactionControl.Disposed -= OnControlDisposed;
                }

                interactionControl = value;

                if (interactionControl != null)
                {
                    interactionControl.MouseDown += OnMouseDown;
                    interactionControl.MouseMove += OnMouseMove;
                    interactionControl.MouseUp += OnMouseUp;
                    interactionControl.MouseCaptureLost += OnMouseCaptureLost;
                    interactionControl.Disposed += OnControlDisposed;
                }
            }
        }

        /// <summary>
        /// Gets or sets the resizing behavior of the grip when dragged by the user.
        /// This determines whether dragging the grip will resize the target control, and in which directions.
        /// </summary>
        public virtual GripControl.GripSizeAction SizeAction { get; set; }
            = GripControl.GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets or sets the moving behavior of the grip when dragged by the user.
        /// This determines whether dragging the grip will move the target control, and in which directions.
        /// </summary>
        public virtual GripControl.GripMoveAction MoveAction { get; set; } = GripControl.GripMoveAction.None;

        /// <summary>
        /// Gets a value indicating whether the grip can resize vertically.
        /// If false, only horizontal resizing is allowed.
        /// Default is true.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanResizeVertically
            => SizeAction == GripControl.GripSizeAction.ChangeHeight
            || SizeAction == GripControl.GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets or sets a value indicating whether the grip can resize horizontally.
        /// If false, only vertical resizing is allowed.
        /// Default is true.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanResizeHorizontally
            => SizeAction == GripControl.GripSizeAction.ChangeWidth
            || SizeAction == GripControl.GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets a value indicating whether the grip can resize.
        /// If false, the grip will not resize the target control when dragged.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanResize => CanResizeVertically || CanResizeHorizontally;

        /// <summary>
        /// Gets a value indicating whether the grip can move vertically. If false, only horizontal moving is allowed.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMoveVertically
            => MoveAction == GripControl.GripMoveAction.ChangeLocation
            || MoveAction == GripControl.GripMoveAction.ChangeTop;

        /// <summary>
        /// Gets a value indicating whether the grip can move horizontally. If false, only vertical moving is allowed.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMoveHorizontally
            => MoveAction == GripControl.GripMoveAction.ChangeLocation
            || MoveAction == GripControl.GripMoveAction.ChangeLeft;

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

        /// <summary>
        /// Gets or sets the minimum position delta for moving. If null, the default value is used.
        /// </summary>
        public virtual float? MinPositionDelta
        {
            get => minPositionDelta;
            set
            {
                if (value <= 0) value = null;
                minPositionDelta = value;
            }
        }

        /// <summary>
        /// Gets or sets the control that is resized or moved. If null, the top-level parent form is used.
        /// </summary>
        [Browsable(false)]
        public virtual IGripControlTarget? Target { get; set; }

        /// <summary>
        /// Gets or sets the function that returns the target control.
        /// If null, the <see cref="Target"/> property is used to determine the target control.
        /// </summary>
        [Browsable(false)]
        public virtual Func<IGripControlTarget?>? TargetProvider { get; set; }

        /// <summary>
        /// Initializes the grip control with no image and sets it up
        /// to allow moving the target control by dragging the grip.
        /// </summary>
        public virtual GripComponent ConfigureAsMovingGrip()
        {
            SizeAction = GripControl.GripSizeAction.None;
            MoveAction = GripControl.GripMoveAction.ChangeLocation;
            return this;
        }

        /// <summary>
        /// Initializes the grip control with the standard status bar grip image aligned to the left,
        /// and sets the cursor and resizing behavior accordingly.
        /// </summary>
        public virtual GripComponent ConfigureAsSizingGripLeft()
        {
            InvertWidthDelta = true;
            SizeAction = GripControl.GripSizeAction.ChangeWidthAndHeight;
            MoveAction = GripControl.GripMoveAction.ChangeLeft;
            InvertLeftDelta = true;
            return this;
        }

        /// <summary>
        /// Sets the target control for the grip control.
        /// </summary>
        /// <param name="target">The target control to be set.</param>
        /// <returns>The current instance of <see cref="GripControl"/>.</returns>
        public GripComponent SetTarget(AbstractControl? target)
        {
            Target = target;
            return this;
        }

        ///<summary>
        /// Gets the effective minimum position delta for moving, considering
        /// the user-defined value and the default value.
        /// </summary>
        protected virtual float GetEffectiveMinPositionDelta()
        {
            return MinPositionDelta ?? GripControl.DefaultMinPositionDelta;
        }

        ///<summary>
        /// Gets the effective minimum size delta for resizing,
        /// considering the user-defined value and the default value.
        /// </summary>
        protected virtual float GetEffectiveMinSizeDelta()
        {
            return MinSizeDelta ?? GripControl.DefaultMinSizeDelta;
        }

        /// <summary>
        /// Calculates the new size for the target control based on the current mouse position
        /// and the original size, while respecting the minimum size delta and the target's
        /// minimum and maximum size constraints.
        /// </summary>
        /// <param name="isVert">Indicates whether the resizing is vertical.</param>
        /// <param name="mouseNowPos">The current mouse position.</param>
        /// <param name="applyNegativeDelta">Indicates whether to apply negative delta.</param>
        /// <param name="applyMinMax">Indicates whether to apply minimum and maximum size constraints.</param>
        /// <param name="minSizeDelta">The minimum size delta to be applied. If not specified,
        /// the effective minimum size delta will be used.</param>
        /// <returns>The new size for the target control.</returns>
        protected virtual float GetNewSize(
            bool isVert,
            PointD mouseNowPos,
            bool applyNegativeDelta,
            bool applyMinMax,
            float? minSizeDelta = null)
        {
            var oldSize = origTargetBounds.GetSize(isVert);
            var target = GetTarget();
            if (target == null) return oldSize;

            var nowPos = mouseNowPos.GetLocation(isVert);
            var prevPos = mouseDownPos.GetLocation(isVert);

            var delta = nowPos - prevPos;

            if (applyNegativeDelta)
            {
                delta = -delta;
            }

            var absDelta = MathF.Abs(delta);

            var minDelta = minSizeDelta ?? GetEffectiveMinSizeDelta();

            var newSize = oldSize + delta;

            if (minDelta > 1)
            {
                if (absDelta < minDelta)
                {
                    return oldSize;
                }

                newSize = MathF.Floor(newSize / minDelta) * minDelta;
            }

            if (applyMinMax)
            {
                var maxSize = target.MaximumSize.GetSize(isVert);
                var minSize = target.MinimumSize.GetSize(isVert);

                newSize = Math.Max(minSize, newSize);
                if (maxSize > 0) newSize = Math.Min(newSize, maxSize);
            }

            return newSize;
        }

        /// <summary>
        /// Handles the MouseMove event for the interaction control.
        /// This method is called when the mouse is moved.
        /// </summary>
        /// <param name="sender">The control that received the mouse move event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if(sender is not Control control) return;

            if (resizing)
            {
                var target = GetTarget();
                if (target == null) return;
                PointD mouseNowPos = control.PointToScreen(e.Location);

                float szDelta = GetEffectiveMinSizeDelta();

                if (!CanResize)
                {
                    szDelta = GetEffectiveMinPositionDelta();
                }

                float newWidth = GetNewSize(
                    isVert: false,
                    mouseNowPos,
                    InvertWidthDelta,
                    CanResizeHorizontally,
                    szDelta);

                float newHeight = GetNewSize(
                    isVert: true,
                    mouseNowPos,
                    InvertHeightDelta,
                    CanResizeVertically,
                    szDelta);

                var bounds = origTargetBounds;

                float deltaX = newWidth - bounds.Width;
                float deltaY = newHeight - bounds.Height;

                if (CanResizeHorizontally)
                {
                    bounds.Width = newWidth;
                }

                if (CanResizeVertically)
                {
                    bounds.Height = newHeight;
                }

                if (!CanResize)
                {
                    var minPositionDelta = GetEffectiveMinPositionDelta();
                    if (deltaX > 0)
                    {
                        deltaX = Math.Max(deltaX, minPositionDelta);
                    }
                    else if (deltaX < 0)
                    {
                        deltaX = Math.Min(deltaX, -minPositionDelta);
                    }
                    if (deltaY > 0)
                    {
                        deltaY = Math.Max(deltaY, minPositionDelta);
                    }
                    else if (deltaY < 0)
                    {
                        deltaY = Math.Min(deltaY, -minPositionDelta);
                    }
                }

                if (CanMoveHorizontally)
                {
                    if (InvertLeftDelta)
                        bounds.X -= deltaX;
                    else 
                        bounds.X += deltaX;
                }

                if (CanMoveVertically)
                {
                    if (InvertTopDelta)
                        bounds.Y -= deltaY;
                    else
                        bounds.Y += deltaY;
                }

                if (bounds == target.Bounds) return;

                target.Bounds = bounds;
            }
        }

        /// <summary>
        /// Handles the MouseCaptureLost event for the interaction control.
        /// This method is called when the mouse capture is lost.
        /// </summary>
        /// <param name="sender">The control that lost mouse capture.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseCaptureLost(object? sender, EventArgs e)
        {
            if (resizing)
            {
                resizing = false;
            }
        }

        /// <summary>
        /// Handles the MouseUp event for the interaction control.
        /// This method is called when a mouse button is released.
        /// </summary>
        /// <param name="sender">The control that received the mouse up event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (sender is not Control control) return;

            if (resizing && e.Button == MouseButtons.Left)
            {
                resizing = false;
                control.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Handles the MouseDown event for the interaction control.
        /// This method is called when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The control that received the mouse down event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (sender is not Control control) return;

            if (e.Button == MouseButtons.Left)
            {
                var target = GetTarget();

                if (target != null)
                {
                    resizing = true;
                    mouseDownPos = control.PointToScreen(e.Location);
                    origTargetBounds = target?.Bounds ?? RectD.Empty;
                    control.CaptureMouse();
                }
            }
        }

        /// <summary>
        /// Called when the interaction control is disposed.
        /// Override this method to perform any necessary cleanup.
        /// </summary>
        /// <param name="sender">The control that is being disposed.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnControlDisposed(object? sender, EventArgs e)
        {
            if(sender == interactionControl)
            {
                InteractionControl = null;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            InteractionControl = null;
            base.DisposeManaged();
        }

        /// <summary>
        /// Gets the target control to be resized. If the Target property is set and valid, it returns that.
        /// </summary>
        /// <returns>The target control to be resized, or null
        /// if the Target property is not set or invalid.</returns>
        protected virtual IGripControlTarget? GetTarget()
        {
            if (TargetProvider != null)
            {
                var target = TargetProvider();
                if (target != null && !target.DisposingOrDisposed)
                    return target;
            }

            if (Target != null && !Target.DisposingOrDisposed)
                return Target;
            else
                return null;
        }
    }
}