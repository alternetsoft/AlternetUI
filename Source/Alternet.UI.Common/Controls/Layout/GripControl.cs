using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a grip control that allows the user to resize or move the target control 
    /// (or the top-level parent form if no target is specified) by dragging the grip.
    /// The grip can be configured to allow resizing in both directions, or only horizontally or vertically.
    /// The minimum size delta for resizing can also be customized.
    /// The control is typically placed in the bottom-right corner of a status bar, but can be used in other contexts as well.
    /// </summary>
    public class GripControl : HiddenBorder
    {
        /// <summary>
        /// The default minimum size delta for resizing. This value is used if the <see cref="MinSizeDelta"/> property is not set.
        /// </summary>
        public static float DefaultMinSizeDelta = 10;

        /// <summary>
        /// The default suggested size for the grip control. This value is used to set the initial size
        /// of the control and can be overridden by setting the <see cref="AbstractControl.SuggestedSize"/> property.
        /// </summary>
        public static float DefaultSuggestedSize = 16;

        private readonly ImageDrawable primitive = new();

        private float? minSizeDelta;
        private RectD origTargetBounds;
        private bool resizing = false;
        private PointD mouseDownPos;
        private GripImageKind imageKind = GripImageKind.SizingGripRight;

        /// <summary>
        /// Initializes a new instance of the <see cref="GripControl"/> class.
        /// </summary>
        public GripControl()
        {
            primitive.CenterHorz = false;
            primitive.CenterVert = false;

            var styles = ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint;

            ParentBackColor = true;
            ParentForeColor = true;

            this.SetStyle(styles, true);
            this.SuggestedSize = new (DefaultSuggestedSize, DefaultSuggestedSize);
            this.Cursor = Cursors.SizeNWSE;
            Alignment = HVAlignment.BottomRight;
            this.TabStop = false;
        }

        /// <summary>
        /// Defines the kind of grip image to display.
        /// </summary>
        public enum GripImageKind
        {
            /// <summary>
            /// No image is displayed. The grip will still function as a resizing handle,
            /// but it will not have a visual representation.
            /// </summary>
            None,

            /// <summary>
            /// The grip image looks like the standard Windows status bar grip,
            /// which is displayed on the right side of the status bar.
            /// </summary>
            SizingGripRight,

            /// <summary>
            /// The grip image looks like the standard Windows status bar grip,
            /// which is displayed on the left side of the status bar.
            /// </summary>
            SizingGripLeft,

            /// <summary>
            /// A custom grip image is used. The image can be set using the <see cref="SvgImage"/> property.
            /// </summary>
            Custom,
        }

        /// <summary>
        /// Defines the resizing behavior of the grip when dragged by the user.
        /// </summary>
        public enum GripSizeAction
        {
            /// <summary>
            /// Dragging the grip does not resize the target control.
            /// The grip can still be used for moving if the <see cref="GripMoveAction"/> is set to allow moving.
            /// </summary>
            None,

            /// <summary>
            /// Dragging the grip resizes the target control in horizontal direction,
            /// in addition to moving if allowed by the <see cref="GripMoveAction"/>.
            /// </summary>
            ChangeWidth,

            /// <summary>
            /// Dragging the grip resizes the target control in vertical direction,
            /// in addition to moving if allowed by the <see cref="GripMoveAction"/>.
            /// </summary>
            ChangeHeight,

            /// <summary>
            /// Dragging the grip resizes the target control in both horizontal and vertical directions,
            /// in addition to moving if allowed by the <see cref="GripMoveAction"/>.
            /// </summary>
            ChangeWidthAndHeight,
        }

        /// <summary>
        /// Defines the moving behavior of the grip when dragged by the user.
        /// This determines whether dragging the grip will move the target control, and in which directions.
        /// </summary>
        public enum GripMoveAction
        {
            /// <summary>
            /// Dragging the grip does not move the target control.
            /// The grip can still be used for resizing if the <see cref="GripSizeAction"/> is set to allow resizing.
            /// </summary>
            None,

            /// <summary>
            /// Dragging the grip moves the target control in both horizontal and vertical directions,
            /// in addition to resizing if allowed by the <see cref="GripSizeAction"/>.
            /// </summary>
            ChangeLocation,

            /// <summary>
            /// Dragging the grip moves the target control horizontally by changing its left position,
            /// in addition to resizing if allowed by the <see cref="GripSizeAction"/>.
            /// </summary>
            ChangeLeft,

            /// <summary>
            /// Dragging the grip moves the target control vertically by changing its top position,
            /// in addition to resizing if allowed by the <see cref="GripSizeAction"/>.
            /// </summary>
            ChangeTop,
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
        /// Gets or sets the resizing behavior of the grip when dragged by the user.
        /// This determines whether dragging the grip will resize the target control, and in which directions.
        /// </summary>
        public virtual GripSizeAction SizeAction { get; set; } = GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets or sets the moving behavior of the grip when dragged by the user.
        /// This determines whether dragging the grip will move the target control, and in which directions.
        /// </summary>
        public virtual GripMoveAction MoveAction { get; set; } = GripMoveAction.None;

        /// <inheritdoc/>
        public override Thickness Padding
        {
            get => base.Padding;
            set
            {
                if (Padding == value)
                    return;
                base.Padding = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the kind of grip image to display. Changing this property triggers a redraw of the control.
        /// </summary>
        public virtual GripImageKind ImageKind
        {
            get => imageKind;
            set
            {
                if (imageKind == value) return;
                imageKind = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether image is centered horizontally and vertically.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsImageCentered
        {
            get
            {
                return IsImageCenteredVert && IsImageCenteredHorz;
            }

            set
            {
                if (IsImageCentered == value)
                    return;
                primitive.CenterHorz = value;
                primitive.CenterVert = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to center image vertically in the control rectangle.
        /// Default is <c>true</c>. This property is used when image is not stretched.
        /// </summary>
        public virtual bool IsImageCenteredVert
        {
            get
            {
                return primitive.CenterVert;
            }

            set
            {
                if (primitive.CenterVert == value)
                    return;
                primitive.CenterVert = value;
                if (ImageKind != GripImageKind.None)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to center image horizontally in the control rectangle.
        /// Default is <c>true</c>. This property is used when image is not stretched.
        /// </summary>
        public virtual bool IsImageCenteredHorz
        {
            get
            {
                return primitive.CenterHorz;
            }

            set
            {
                if (primitive.CenterHorz == value)
                    return;
                primitive.CenterHorz = value;
                if (ImageKind != GripImageKind.None)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the SVG image associated with this object.
        /// </summary>
        public virtual SvgImage? SvgImage
        {
            get => primitive.SvgImage;

            set
            {
                if (primitive.SvgImage == value)
                    return;
                primitive.SvgImage = value;
                if (ImageKind == GripImageKind.Custom)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw image stretched
        /// to the size of the control.
        /// </summary>
        public virtual bool IsImageStretched
        {
            get
            {
                return primitive.Stretch;
            }

            set
            {
                if (primitive.Stretch == value)
                    return;
                primitive.Stretch = value;
                if (ImageKind != GripImageKind.None)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the size of the SVG image in pixels.
        /// </summary>
        /// <remarks>Changing this property triggers a layout update and redraw of the associated image if
        /// it is visible.</remarks>
        public virtual int? SvgSize
        {
            get => primitive.SvgSize;

            set
            {
                if (primitive.SvgSize == value)
                    return;
                primitive.SvgSize = value;
                if (ImageKind != GripImageKind.None)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the SVG color.
        /// </summary>
        /// <remarks>Setting this property triggers a layout update and invalidates the control if the
        /// image is visible.</remarks>
        public virtual Color? SvgColor
        {
            get => primitive.SvgColor;

            set
            {
                if (primitive.SvgColor == value)
                    return;
                primitive.SvgColor = value;
                if (ImageKind != GripImageKind.None)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the grip can resize vertically. If false, only horizontal resizing is allowed.
        /// Default is true.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanResizeVertically
            => SizeAction == GripSizeAction.ChangeHeight || SizeAction == GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets or sets a value indicating whether the grip can resize horizontally. If false, only vertical resizing is allowed.
        /// Default is true.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanResizeHorizontally
            => SizeAction == GripSizeAction.ChangeWidth || SizeAction == GripSizeAction.ChangeWidthAndHeight;

        /// <summary>
        /// Gets a value indicating whether the grip can move vertically. If false, only horizontal moving is allowed.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMoveVertically
            => MoveAction == GripMoveAction.ChangeLocation || MoveAction == GripMoveAction.ChangeTop;

        /// <summary>
        /// Gets a value indicating whether the grip can move horizontally. If false, only vertical moving is allowed.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMoveHorizontally
            => MoveAction == GripMoveAction.ChangeLocation || MoveAction == GripMoveAction.ChangeLeft;

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
        /// Gets or sets the control that is resized or moved. If null, the top-level parent form is used.
        /// </summary>
        public virtual Control? Target { get; set; }

        internal ImageDrawable Primitive => primitive;

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
            if (ImageKind != GripImageKind.None)
            {
                DrawDefaultImage(e.Graphics, this.ClientRectangle);
            }
        }

        /// <summary>
        /// Gets the effective SVG image to be drawn based on the current <see cref="ImageKind"/> setting.
        /// </summary>
        /// <returns>The effective SVG image, or null if no image should be drawn.</returns>
        public virtual SvgImage? GetEffectiveSvgImage()
        {
            return ImageKind switch
            {
                GripImageKind.None => null,
                GripImageKind.SizingGripRight => KnownSvgImages.ImgSizingGripRight,
                GripImageKind.SizingGripLeft => KnownSvgImages.ImgSizingGripLeft,
                GripImageKind.Custom => SvgImage,
                _ => null,
            };
        }

        /// <summary>
        /// Initializes the grip control with no image and sets it up to allow moving the target control by dragging the grip.
        /// </summary>
        public virtual void ConfigureAsMovingGrip()
        {
            ImageKind = GripControl.GripImageKind.None;
            SizeAction = GripControl.GripSizeAction.None;
            MoveAction = GripControl.GripMoveAction.ChangeLocation;
            Cursor = Cursors.Default;
            MinSizeDelta = 1;
            Alignment = new(HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
        }

        /// <summary>
        /// Initializes the grip control with the standard status bar grip image aligned to the left,
        /// and sets the cursor and resizing behavior accordingly.
        /// </summary>
        public virtual void ConfigureAsSizingGripLeft()
        {
            ImageKind = GripControl.GripImageKind.SizingGripLeft;
            Cursor = Cursors.SizeNESW;
            InvertWidthDelta = true;
            Alignment = HVAlignment.BottomLeft;
            SizeAction = GripSizeAction.ChangeWidthAndHeight;
            MoveAction = GripMoveAction.ChangeLeft;
            InvertLeftDelta = true;
        }

        /// <summary>
        /// Paints image in the default style.
        /// </summary>
        public virtual void DrawDefaultImage(Graphics dc, RectD rect)
        {
            primitive.Bounds =
                (rect.Location + Padding.LeftTop, rect.Size - Padding.Size);
            primitive.SvgImage = GetEffectiveSvgImage();
            primitive.Draw(this, dc);
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
                    origTargetBounds = target?.Bounds ?? RectD.Empty;
                    this.CaptureMouse();
                }
            }
        }

        ///<summary>
        /// Gets the effective minimum size delta for resizing, considering the user-defined value and the default value.
        /// </summary>
        protected virtual float GetEffectiveMinSizeDelta()
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
        /// <param name="applyNegativeDelta">Indicates whether to apply negative delta.</param>
        /// <param name="applyMinMax">Indicates whether to apply minimum and maximum size constraints.</param>
        /// <returns>The new size for the target control.</returns>
        protected virtual float GetNewSize(
            bool isVert,
            PointD mouseNowPos,
            bool applyNegativeDelta,
            bool applyMinMax)
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

            var minDelta = GetEffectiveMinSizeDelta();

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

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (resizing)
            {
                var target = GetTarget();
                if (target == null) return;
                PointD mouseNowPos = ClientToScreen(e.Location);

                float newWidth = GetNewSize(isVert: false, mouseNowPos, InvertWidthDelta, CanResizeHorizontally);
                float newHeight = GetNewSize(isVert: true, mouseNowPos, InvertHeightDelta, CanResizeVertically);

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

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {            
            base.OnSizeChanged(e);
            Invalidate();
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