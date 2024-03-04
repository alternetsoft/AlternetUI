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
        /// Initializes a new instance of the <see cref="NineRects"/> struct.
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
        /// Gets all 9 rectangles.
        /// </summary>
        public readonly RectD[] Rects
        {
            get
            {
                return new RectD[]
                {
                        TopLeft, TopCenter, TopRight,
                        CenterLeft, Center, CenterRight,
                        BottomLeft, BottomCenter, BottomRight,
                };
            }
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
        public class PreviewControl : UserControl
        {
            private VerticalAlignment selectedVert = VerticalAlignment.Top;
            private HorizontalAlignment selectedHorz = HorizontalAlignment.Left;

            /// <summary>
            /// Initializes a new instance of the <see cref="PreviewControl"/> class.
            /// </summary>
            public PreviewControl()
            {
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
                NineRects rects = new(e.ClipRectangle.ToRect(), PatchRect);
                DrawingUtils.FillRectanglesBorder(e.Graphics, Color.Red, rects.Rects);
                DrawingUtils.FillRectangleBorder(e.Graphics, Color.Navy, PatchRect);
                DrawingUtils.FillRectangleBorder(
                    e.Graphics,
                    Color.Green,
                    rects.GetRect(SelectedHorz, SelectedVert));
            }
        }
    }
}
