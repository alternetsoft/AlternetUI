using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements slicing of the rectangle into 9 parts.
    /// </summary>
    /// <remarks>
    /// Issue with details is here:
    /// <see href="https://github.com/alternetsoft/AlternetUI/issues/115"/>.
    /// </remarks>
    public struct NineRects
    {
        private RectI container;
        private RectI patch;

        /// <summary>
        /// Initializes a new instance of the <see cref="NineRects"/> class with the
        /// specified container and patch rectangles.
        /// </summary>
        /// <param name="container">Rectangle to slice.</param>
        /// <param name="patch">Rectangle which defines sliced parts.</param>
        /// <remarks>
        /// <paramref name="patch"/> coordinates are not counted from top-left corner of
        /// the <paramref name="container"/>, these rectangles are assumed to be siblings.
        /// </remarks>
        public NineRects(RectI container, RectI patch)
        {
            this.container = container;
            this.patch = patch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineRects"/> class with the
        /// specified container and patch rectangles.
        /// </summary>
        /// <param name="container">The outer rectangle that defines the container area.</param>
        /// <param name="patch">The inner rectangle that defines the patch area
        /// within the container.</param>
        /// <param name="scaleFactor">The scale factor to convert from device-independent
        /// pixels (DIPs) to pixels.</param>
        public NineRects(RectD container, RectD patch, Coord scaleFactor)
        {
            this.container = container.PixelFromDip(scaleFactor);
            this.patch = patch.PixelFromDip(scaleFactor);
        }

        /// <summary>
        /// Rectangle which is sliced.
        /// </summary>
        public readonly RectI Container => container;

        /// <summary>
        /// Rectangle which defines sliced parts.
        /// </summary>
        public readonly RectI Patch => patch;

        /// <summary>
        /// Top-left corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI TopLeft
        {
            get
            {
                return RectI.FromLTRB(
                    container.Location,
                    patch.Location);
            }
        }

        /// <summary>
        /// Top-center corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI TopCenter
        {
            get
            {
                return RectI.FromLTRB(
                    (patch.X, container.Y),
                    (patch.Right, patch.Y));
            }
        }

        /// <summary>
        /// Top-right corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI TopRight
        {
            get
            {
                return RectI.FromLTRB(
                    (patch.Right, container.Y),
                    (container.Right, patch.Y));
            }
        }

        /// <summary>
        /// Center-left corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI CenterLeft
        {
            get
            {
                return RectI.FromLTRB(
                    (container.X, patch.Y),
                    (patch.X, patch.Bottom));
            }
        }

        /// <summary>
        /// Same as <see cref="Patch"/>.
        /// </summary>
        public readonly RectI Center => patch;

        /// <summary>
        /// Center-right corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI CenterRight
        {
            get
            {
                return RectI.FromLTRB(
                    (patch.Right, patch.Y),
                    (container.Right, patch.Bottom));
            }
        }

        /// <summary>
        /// Bottom-left corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI BottomLeft
        {
            get
            {
                return RectI.FromLTRB(
                    (container.X, patch.Bottom),
                    (patch.X, container.Bottom));
            }
        }

        /// <summary>
        /// Bottom-center corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI BottomCenter
        {
            get
            {
                return RectI.FromLTRB(
                    (patch.X, patch.Bottom),
                    (patch.Right, container.Bottom));
            }
        }

        /// <summary>
        /// Bottom-right corner of the <see cref="Container"/>.
        /// </summary>
        public readonly RectI BottomRight
        {
            get
            {
                return RectI.FromLTRB(
                    (patch.Right, patch.Bottom),
                    (container.Right, container.Bottom));
            }
        }

        /// <summary>
        /// Gets the top rectangular region defined by the container's position
        /// and the patch's dimensions. This rectangle returns the area which is
        /// above the patch.
        /// </summary>
        public readonly RectI TopRect
        {
            get
            {
                return TopLeft.WithWidth(container.Width);
            }
        }

        /// <summary>
        /// Gets the bottom rectangular region defined by the container's position
        /// and the patch's dimensions. This rectangle returns the area which is
        /// below the patch.
        /// </summary>
        public readonly RectI BottomRect
        {
            get
            {
                return BottomLeft.WithWidth(container.Width);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the height of the top rectangle is greater
        /// than the height of the bottom rectangle.
        /// </summary>
        public readonly bool IsTopRectLarger
        {
            get
            {
                return TopRect.Height > BottomRect.Height;
            }
        }

        /// <summary>
        /// Gets all 9 rectangles.
        /// </summary>
        public readonly RectI[] Rects
        {
            get
            {
                return new RectI[]
                {
                        TopLeft, TopCenter, TopRight,
                        CenterLeft, Center, CenterRight,
                        BottomLeft, BottomCenter, BottomRight,
                };
            }
        }

        /// <summary>
        /// Suggests the vertical alignment for a tooltip based on the relative sizes
        /// and positions of a container rectangle and an item rectangle.
        /// </summary>
        /// <param name="containerRect">The rectangle representing the container area.</param>
        /// <param name="itemRect">The rectangle representing the item for which
        /// the tooltip is being positioned.</param>
        /// <returns>A <see cref="VerticalAlignment"/> value indicating
        /// the suggested vertical alignment. Returns <see cref="VerticalAlignment.Top"/>
        /// if the top area is larger; otherwise, returns <see cref="VerticalAlignment.Bottom"/>.
        /// </returns>
        public static VerticalAlignment SuggestVertAlignmentForToolTip(
            RectD containerRect,
            RectD itemRect)
        {
            NineRects rects = new(containerRect, itemRect, 1);
            var result = rects.IsTopRectLarger ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            return result;
        }

        /// <summary>
        /// Gets rectangle specified by <paramref name="horz"/> and
        /// <paramref name="vert"/> params. Only left, center, right, top, bottom
        /// values are supported.
        /// </summary>
        public readonly RectI GetRect(HorizontalAlignment horz, VerticalAlignment vert)
        {
            switch (horz)
            {
                case HorizontalAlignment.Left:
                    switch (vert)
                    {
                        case VerticalAlignment.Top:
                            return TopLeft;
                        case VerticalAlignment.Center:
                            return CenterLeft;
                        case VerticalAlignment.Bottom:
                            return BottomLeft;
                        default:
                            return RectI.Empty;
                    }

                case HorizontalAlignment.Center:
                    switch (vert)
                    {
                        case VerticalAlignment.Top:
                            return TopCenter;
                        case VerticalAlignment.Center:
                            return Center;
                        case VerticalAlignment.Bottom:
                            return BottomCenter;
                        default:
                            return RectI.Empty;
                    }

                case HorizontalAlignment.Right:
                    switch (vert)
                    {
                        case VerticalAlignment.Top:
                            return TopRight;
                        case VerticalAlignment.Center:
                            return CenterRight;
                        case VerticalAlignment.Bottom:
                            return BottomRight;
                        default:
                            return RectI.Empty;
                    }

                default:
                    return RectI.Empty;
            }
        }

        /// <summary>
        /// Sample preview control for the <see cref="NineRects"/> structure.
        /// </summary>
        public class PreviewControl : HiddenBorder
        {
            private VerticalAlignment selectedVert = VerticalAlignment.Top;
            private HorizontalAlignment selectedHorz = HorizontalAlignment.Left;

            /// <summary>
            /// Initializes a new instance of the <see cref="PreviewControl"/> class.
            /// </summary>
            public PreviewControl()
            {
                UserPaint = true;
                SuggestedSize = 300;
            }

            /// <summary>
            /// Gets or sets <see cref="NineRects.Patch"/> value.
            /// </summary>
            public RectI PatchRect { get; set; } = (50, 25, 75, 80);

            /// <summary>
            /// Gets or sets which rectangle is visually selected (vertical).
            /// </summary>
            public VerticalAlignment SelectedVert
            {
                get => selectedVert;
                set
                {
                    selectedVert = value;
                    Invalidate();
                }
            }

            /// <summary>
            /// Gets or sets which rectangle is visually selected (horizontal).
            /// </summary>
            public HorizontalAlignment SelectedHorz
            {
                get => selectedHorz;
                set
                {
                    selectedHorz = value;
                    Invalidate();
                }
            }

            /// <inheritdoc/>
            protected override void OnPaint(PaintEventArgs e)
            {
                var scaleFactor = ScaleFactor;
                NineRects rects = new(e.ClientRectangle.ToRect(), PatchRect);
                DrawingUtils.DrawBordersWithBrush(
                    e.Graphics,
                    Color.Red.AsBrush,
                    GraphicsFactory.PixelToDip(rects.Rects, scaleFactor));
                DrawingUtils.DrawBorderWithBrush(
                    e.Graphics,
                    Color.Navy.AsBrush,
                    PatchRect.PixelToDip(scaleFactor));
                var borderRect = rects.GetRect(SelectedHorz, SelectedVert);
                DrawingUtils.DrawBorderWithBrush(
                    e.Graphics,
                    Color.Green.AsBrush,
                    borderRect.PixelToDip(scaleFactor));
            }
        }
    }
}
