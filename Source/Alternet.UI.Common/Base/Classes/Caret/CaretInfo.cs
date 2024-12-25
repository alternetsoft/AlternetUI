using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which help to work with internally painted caret.
    /// </summary>
    public partial class CaretInfo : BaseObject
    {
        private readonly List<RectI> region = new();
        private SizeI size;
        private PointI position;
        private bool visible;
        private bool focused;
        private bool topOverlayVisible;
        private bool bottomOverlayVisible;
        private LightDarkColor? overlayColor;
        private LightDarkColor? color;
        private int overlaySize;
        private SvgImage? overlayImage;

        /// <summary>
        /// Gets disposed state.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        /// Gets caret rectangle.
        /// </summary>
        public RectI Rect
        {
            get
            {
                return new(Position, Size);
            }
        }

        /// <summary>
        /// Get or sets overlay image color. If <c>null</c>,
        /// <see cref="PlessCaretHandler.DefaultOverlayColor"/> will be used.
        /// </summary>
        public virtual LightDarkColor? OverlayColor
        {
            get
            {
                return overlayColor;
            }

            set
            {
                if (overlayColor == value)
                    return;
                overlayColor = value;
                OverlayChanged();
            }
        }

        /// <summary>
        /// Gets or sets caret color. If <c>null</c>, <see cref="PlessCaretHandler.CaretColor"/>
        /// will be used.
        /// </summary>
        public virtual LightDarkColor? Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color == value)
                    return;
                color = value;
                AddToUpdateRegion(Rect);
            }
        }

        /// <summary>
        /// Gets or sets overlay image size.
        /// </summary>
        public virtual int OverlaySize
        {
            get
            {
                return overlaySize;
            }

            set
            {
                if (overlaySize == value)
                    return;
                overlaySize = value;
                OverlayChanged();
            }
        }

        /// <summary>
        /// Gets bottom overlay rectangle.
        /// </summary>
        public virtual RectI BottomOverlayRect
        {
            get
            {
                var centerPoint = Rect.BottomLineCenter;
                return GetOverlayRect(centerPoint);
            }
        }

        /// <summary>
        /// Gets top overlay rectangle.
        /// </summary>
        public virtual RectI TopOverlayRect
        {
            get
            {
                var centerPoint = Rect.TopLineCenter;
                return GetOverlayRect(centerPoint);
            }
        }

        /// <summary>
        /// Gets whether top or bottom overlay image is visible.
        /// </summary>
        public bool TopOrBottomOverlayVisible => BottomOverlayVisible || TopOverlayVisible;

        /// <summary>
        /// Gets or sets whether top and bottom overlay images are visible.
        /// </summary>
        public bool TopAndBottomOverlayVisible
        {
            get
            {
                return TopOverlayVisible && BottomOverlayVisible;
            }

            set
            {
                if (TopAndBottomOverlayVisible == value)
                    return;
                topOverlayVisible = value;
                bottomOverlayVisible = value;
                OverlayChanged(false);
            }
        }

        /// <summary>
        /// Gets or sets whether top overlay image is painted.
        /// </summary>
        public virtual bool TopOverlayVisible
        {
            get
            {
                return topOverlayVisible;
            }

            set
            {
                if (topOverlayVisible == value)
                    return;
                topOverlayVisible = value;
                AddToUpdateRegion(TopOverlayRect);
            }
        }

        /// <summary>
        /// Gets or sets overlay image. If not specified,
        /// <see cref="PlessCaretHandler.DefaultOverlayImage"/> is used.
        /// </summary>
        public virtual SvgImage? OverlayImage
        {
            get
            {
                return overlayImage;
            }

            set
            {
                if (overlayImage == value)
                    return;
                overlayImage = value;
                OverlayChanged();
            }
        }

        /// <summary>
        /// Gets overlay image which is actually used. This is calculated property.
        /// </summary>
        public virtual SvgImage SafeOverlayImage
        {
            get
            {
                return OverlayImage ?? PlessCaretHandler.DefaultOverlayImage
                    ?? KnownSvgImages.ImgCircleFilled;
            }
        }

        /// <summary>
        /// Gets or sets whether bottom overlay image is painted.
        /// </summary>
        public virtual bool BottomOverlayVisible
        {
            get
            {
                return bottomOverlayVisible;
            }

            set
            {
                if (bottomOverlayVisible == value)
                    return;
                bottomOverlayVisible = value;
                AddToUpdateRegion(BottomOverlayRect);
            }
        }

        /// <summary>
        /// Gets caret size.
        /// </summary>
        public virtual SizeI Size
        {
            get
            {
                return size;
            }

            internal set
            {
                if (size == value)
                    return;
                var oldRect = Rect;
                size = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(oldRect, Rect);
            }
        }

        /// <summary>
        /// Gets caret position.
        /// </summary>
        public virtual PointI Position
        {
            get
            {
                return position;
            }

            internal set
            {
                if (position == value)
                    return;
                var oldRect = Rect;
                position = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(oldRect, Rect);
            }
        }

        /// <summary>
        /// Gets whether caret is visible.
        /// </summary>
        public virtual bool Visible
        {
            get
            {
                return visible;
            }

            internal set
            {
                if (visible == value)
                    return;
                visible = value;

                if (!visible)
                {
                }

                AddToUpdateRegion(Rect);
            }
        }

        /// <summary>
        /// Gets whether caret container is focused.
        /// This doesn't change container's focused state.
        /// </summary>
        public virtual bool ContainerFocused
        {
            get
            {
                return focused;
            }

            internal set
            {
                if (focused == value)
                    return;
                focused = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(Rect);
            }
        }

        internal List<RectI> Region => region;

        /// <summary>
        /// Adds two rectangles to the caret update region.
        /// </summary>
        /// <param name="rect1">First rectangle.</param>
        /// <param name="rect2">Second rectangle.</param>
        public virtual void AddToUpdateRegion(RectI rect1, RectI rect2)
        {
            AddToUpdateRegion(rect1);
            AddToUpdateRegion(rect2);
        }

        /// <summary>
        /// Gets overlay rectangle for the specified center point.
        /// </summary>
        /// <param name="center">Center point of the rectangle.</param>
        /// <returns></returns>
        public virtual RectI GetOverlayRect(PointI center)
        {
            var realSize = overlaySize;
            if (realSize <= 0)
                realSize = Size.MaxWidthHeight / 2;
            RectI result = (0, 0, realSize, realSize);
            result.Center = center;
            return result;
        }

        /// <summary>
        /// Adds rectangle to the caret update region.
        /// </summary>
        /// <param name="rect"></param>
        public virtual void AddToUpdateRegion(RectI rect)
        {
            if (rect.SizeIsEmpty)
                return;
            region.Add(rect);
        }

        /// <summary>
        /// Resets the caret update region.
        /// </summary>
        public virtual void ResetUpdateRegion()
        {
            region.Clear();
        }

        /// <summary>
        /// Called when overlay image related properties are changed.
        /// </summary>
        public virtual void OverlayChanged(bool checkVisible = true)
        {
            if (!Visible)
                return;
            if (BottomOverlayVisible || !checkVisible)
                AddToUpdateRegion(BottomOverlayRect);
            if (TopOverlayVisible || !checkVisible)
                AddToUpdateRegion(TopOverlayRect);
        }

        /// <summary>
        /// Paints caret.
        /// </summary>
        /// <param name="sender">Control where caret is painted.</param>
        /// <param name="e">Paint arguments.</param>
        public virtual void Paint(AbstractControl sender, PaintEventArgs e)
        {
            var dc = e.Graphics;

            var ldColor = Color ?? PlessCaretHandler.CaretColor;

            Color caretColor
                = ldColor.LightOrDark(sender.IsDarkBackground);

            dc.FillRectangle(caretColor.AsBrush, sender.PixelToDip(Rect));

            if (TopOrBottomOverlayVisible)
            {
                var overlayColor = OverlayColor ?? PlessCaretHandler.DefaultOverlayColor
                    ?? PlessCaretHandler.CaretColor;
                var overlaySvg = SafeOverlayImage;
                var overlayImage = overlaySvg.ImageWithColor(BottomOverlayRect.Width, overlayColor);

                if (BottomOverlayVisible)
                {
                    DrawOverlayImage(BottomOverlayRect);
                }

                if (TopOverlayVisible)
                {
                    DrawOverlayImage(TopOverlayRect);
                }

                void DrawOverlayImage(RectI rect)
                {
                    if (overlayImage is not null)
                        dc.DrawImage(overlayImage, sender.PixelToDip(rect.Location));
                }
            }
        }
    }
}
