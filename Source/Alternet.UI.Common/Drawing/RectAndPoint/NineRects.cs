using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a structure that divides a container rectangle into nine distinct rectangular regions based on a
    /// specified patch rectangle. This structure is commonly used to facilitate scalable rendering of user interface
    /// elements by enabling separate manipulation of each region.
    /// </summary>
    /// <remarks>The NineRects structure provides properties and methods to access individual rectangles
    /// within the nine-part grid, including both the patch area and the surrounding regions. It supports conversion
    /// between device-independent pixels (DIPs) and pixels using a scale factor, and offers utility methods for
    /// retrieving rectangles by alignment or containment. This structure is particularly useful for scenarios such as
    /// nine-slice scaling, where different regions of a UI element require separate rendering or layout logic.</remarks>
    public struct NineRects
    {
        private RectI container;
        private RectI patch;
        private float scaleFactor = 1;

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
            this.scaleFactor = scaleFactor;
            this.container = container.PixelFromDip(scaleFactor);
            this.patch = patch.PixelFromDip(scaleFactor);
        }

        /// <summary>
        /// Defines parts of the <see cref="NineRects"/> structure.
        /// </summary>
        [Flags]
        public enum NineRectsParts
        {
            /// <summary>
            /// The top-left corner rectangle.
            /// </summary>
            TopLeft = 1 << 0,

            /// <summary>
            /// The top-center rectangle.
            /// </summary>
            TopCenter = 1 << 1,

            /// <summary>
            /// The top-right corner rectangle.
            /// </summary>
            TopRight = 1 << 2,

            /// <summary>
            /// The center-left rectangle.
            /// </summary>
            CenterLeft = 1 << 3,

            /// <summary>
            /// The center rectangle (patch area).
            /// </summary>
            Center = 1 << 4,

            /// <summary>
            /// The center-right rectangle.
            /// </summary>
            CenterRight = 1 << 5,

            /// <summary>
            /// The bottom-left corner rectangle.
            /// </summary>
            BottomLeft = 1 << 6,

            /// <summary>
            /// The bottom-center rectangle.
            /// </summary>
            BottomCenter = 1 << 7,

            /// <summary>
            /// The bottom-right corner rectangle.
            /// </summary>
            BottomRight = 1 << 8,

            /// <summary>
            /// All nine rectangles.
            /// </summary>
            All = TopLeft | TopCenter | TopRight | CenterLeft | Center | CenterRight | BottomLeft | BottomCenter | BottomRight,

            /// <summary>
            /// All rectangles except the center rectangle.
            /// </summary>
            Outer = TopLeft | TopCenter | TopRight | CenterLeft | CenterRight | BottomLeft | BottomCenter | BottomRight,
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
        /// Gets the top-left corner rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="TopLeft"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD TopLeftScaled => TopLeft.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the top-center rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="TopCenter"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD TopCenterScaled => TopCenter.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the top-right corner rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="TopRight"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD TopRightScaled => TopRight.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the center-left rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="CenterLeft"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD CenterLeftScaled => CenterLeft.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the center rectangle (patch area) in device-independent pixels (DIPs).
        /// Returns <see cref="Center"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD CenterScaled => Center.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the center-right rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="CenterRight"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD CenterRightScaled => CenterRight.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the bottom-left corner rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="BottomLeft"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD BottomLeftScaled => BottomLeft.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the bottom-center rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="BottomCenter"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD BottomCenterScaled => BottomCenter.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the bottom-right corner rectangle in device-independent pixels (DIPs).
        /// Returns <see cref="BottomRight"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD BottomRightScaled => BottomRight.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the top rectangular region in device-independent pixels (DIPs).
        /// Returns <see cref="TopRect"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD TopRectScaled => TopRect.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets the bottom rectangular region in device-independent pixels (DIPs).
        /// Returns <see cref="BottomRect"/> converted from pixels to DIPs using the specified scale factor.
        /// </summary>
        public readonly RectD BottomRectScaled => BottomRect.PixelToDip(scaleFactor);

        /// <summary>
        /// Gets or sets the scale factor used for converting pixels to DIPs.
        /// </summary>
        public float ScaleFactor
        {
            readonly get
            {
                return scaleFactor;
            }

            set
            {
                scaleFactor = value;
            }
        }

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
        /// Gets the rectangle that starts at the top-left corner of the container and spans its full height.
        /// Width is determined by the horizontal position of the patch, creating a vertical strip along the left edge of the container.
        /// </summary>
        /// <remarks>Use this property to obtain a rectangular area aligned with the left edge of the
        /// container, which is useful for layout calculations or positioning elements along the left side.</remarks>
        public readonly RectI LeftRect
        {
            get
            {
                return TopLeft.WithHeight(container.Height);
            }
        }

        /// <summary>
        /// Gets the <see cref="LeftRect"/> scaled from pixels to device-independent units using the current scale factor.
        /// </summary>
        /// <remarks>Use this property to obtain the left rectangle's dimensions adjusted for high-DPI or
        /// variable-resolution displays. The scaling ensures consistent rendering across different display
        /// environments.</remarks>
        public readonly RectD LeftRectScaled
        {
            get
            {
                return LeftRect.PixelToDip(scaleFactor);
            }
        }

        /// <summary>
        /// Gets the rectangle that represents the right area of the container, adjusted to match the container's current height.
        /// Width is determined by the horizontal position of the patch, creating a vertical strip along the right edge of the container.
        /// </summary>
        /// <remarks>Use this property to obtain a rectangular region suitable for layout or rendering
        /// operations that require alignment with the top-right section of the container. The returned rectangle's
        /// height is always synchronized with the container's height, ensuring accurate positioning and
        /// sizing.</remarks>
        public readonly RectI RightRect
        {
            get
            {
                return TopRight.WithHeight(container.Height);
            }
        }

        /// <summary>
        /// Gets the <see cref="RightRect"/> scaled from pixels to device-independent units using the current scale factor.
        /// </summary>
        /// <remarks>Use this property to obtain the right rectangle's dimensions adjusted for high-DPI or
        /// variable-resolution displays. The scaling ensures consistent rendering across different display
        /// environments.</remarks>
        public readonly RectD RightRectScaled
        {
            get
            {
                return RightRect.PixelToDip(scaleFactor);
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
        /// Gets all nine rectangles.
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
        /// Gets an array of all nine rectangles. This property returns the rectangles converted
        /// from pixels to device-independent pixels (DIPs) using the specified scale factor.
        /// </summary>
        /// <remarks>Each rectangle in the returned array corresponds to a specific corner or center
        /// position, scaled according to the current transformation. The array is newly created on
        /// each access and reflects the latest scaled positions.</remarks>
        public readonly RectD[] RectsScaled
        {
            get
            {
                return GraphicsFactory.PixelToDip(Rects, scaleFactor);
            }
        }

        /// <summary>
        /// Gets the outer rectangles (all rectangles except the center rectangle)
        /// after scaling from pixels to device-independent pixels (DIPs) using the specified scale factor.
        /// </summary>
        /// <remarks>Scaling ensures that graphical elements are rendered consistently across displays
        /// with different resolutions. The rectangles are converted to DIPs, which are independent of physical pixel
        /// density and provide uniform sizing on various devices.</remarks>
        public readonly RectD[] OuterRectsScaled
        {
            get
            {
                return GraphicsFactory.PixelToDip(OuterRects, scaleFactor);
            }
        }

        /// <summary>
        /// Gets all rectangles except the center rectangle.
        /// </summary>
        public readonly RectI[] OuterRects
        {
            get
            {
                return new RectI[]
                {
                        TopLeft, TopCenter, TopRight,
                        CenterLeft, CenterRight,
                        BottomLeft, BottomCenter, BottomRight,
                };
            }
        }

        /// <summary>
        /// Returns an array of rectangular regions corresponding to the specified parts of the nine-part grid.
        /// This method allows you to retrieve specific rectangles based on the provided flags, which can represent any combination of the defined parts.
        /// This method returns scaled rectangles, converting them from pixels to device-independent pixels (DIPs) using the specified scale factor.
        /// The returned rectangles reflect the current scaling transformation, ensuring that they are
        /// appropriately sized for rendering on different display densities.
        /// </summary>
        /// <remarks>The order of rectangles in the returned array matches the order of the
        /// <see cref="NineRectsParts"/> flags as defined in the grid: top-left, top-center, top-right, center-left, center,
        /// center-right, bottom-left, bottom-center, and bottom-right. Duplicate flags are ignored.</remarks>
        /// <param name="part">A bitwise combination of <see cref="NineRectsParts"/> values
        /// that specifies which parts of the grid to include.</param>
        /// <returns>An array of <see cref="RectI"/> objects representing the requested parts. The array is empty if no matching
        /// parts are specified.</returns>
        public readonly RectD[] GetPartsScaled(NineRectsParts part)
        {
            return GraphicsFactory.PixelToDip(GetParts(part), scaleFactor);
        }

        /// <summary>
        /// Returns an array of rectangular regions corresponding to the specified parts of the nine-part grid.
        /// </summary>
        /// <remarks>The order of rectangles in the returned array matches the order of the
        /// <see cref="NineRectsParts"/> flags as defined in the grid: top-left, top-center, top-right, center-left, center,
        /// center-right, bottom-left, bottom-center, and bottom-right. Duplicate flags are ignored.</remarks>
        /// <param name="part">A bitwise combination of <see cref="NineRectsParts"/> values
        /// that specifies which parts of the grid to include.</param>
        /// <returns>An array of <see cref="RectI"/> objects representing the requested parts. The array is empty if no matching
        /// parts are specified.</returns>
        public readonly RectI[] GetParts(NineRectsParts part)
        {
            var rects = new List<RectI>();
            if (part.HasFlag(NineRectsParts.TopLeft))
                rects.Add(TopLeft);
            if (part.HasFlag(NineRectsParts.TopCenter))
                rects.Add(TopCenter);
            if (part.HasFlag(NineRectsParts.TopRight))
                rects.Add(TopRight);
            if (part.HasFlag(NineRectsParts.CenterLeft))
                rects.Add(CenterLeft);
            if (part.HasFlag(NineRectsParts.Center))
                rects.Add(Center);
            if (part.HasFlag(NineRectsParts.CenterRight))
                rects.Add(CenterRight);
            if (part.HasFlag(NineRectsParts.BottomLeft))
                rects.Add(BottomLeft);
            if (part.HasFlag(NineRectsParts.BottomCenter))
                rects.Add(BottomCenter);
            if (part.HasFlag(NineRectsParts.BottomRight))
                rects.Add(BottomRight);
            return rects.ToArray();
        }

        /// <summary>
        /// Gets the scaled rectangle that corresponds to the specified part of the NineRects layout.
        /// </summary>
        /// <remarks>The scaling is performed using the current scale factor to convert pixel values to
        /// device-independent units. This is useful for rendering layouts consistently across different display
        /// resolutions.</remarks>
        /// <param name="part">The part of the NineRects layout to retrieve, specified as a value of the NineRectsParts enumeration.</param>
        /// <returns>A RectD structure representing the dimensions of the specified part, scaled to device-independent pixels.</returns>
        public readonly RectD GetPartScaled(NineRectsParts part)
        {
            return GraphicsFactory.PixelToDip(GetPart(part), scaleFactor);
        }

        /// <summary>
        /// Retrieves the rectangle corresponding to the specified part of the nine-part grid.
        /// </summary>
        /// <param name="part">The part of the nine-part grid for which to retrieve the rectangle.</param>
        /// <returns>The rectangle associated with the specified part. Returns <see cref="RectI.Empty"/> if the part is not
        /// recognized.</returns>
        public readonly RectI GetPart(NineRectsParts part)
        {
            return part switch
            {
                NineRectsParts.TopLeft => TopLeft,
                NineRectsParts.TopCenter => TopCenter,
                NineRectsParts.TopRight => TopRight,
                NineRectsParts.CenterLeft => CenterLeft,
                NineRectsParts.Center => Center,
                NineRectsParts.CenterRight => CenterRight,
                NineRectsParts.BottomLeft => BottomLeft,
                NineRectsParts.BottomCenter => BottomCenter,
                NineRectsParts.BottomRight => BottomRight,
                _ => RectI.Empty,
            };
        }

        /// <summary>
        /// Returns an array of outer rectangles that are fully contained within the specified container rectangle.
        /// </summary>
        /// <param name="container">The rectangle that defines the container area.
        /// Only outer rectangles entirely within this area are included
        /// in the result.</param>
        /// <param name="parts">A bitwise combination of values that specifies which outer parts to consider.
        /// Defaults to <see cref="NineRectsParts.Outer"/>.</param>
        /// <returns>An array of RectI values representing the outer rectangles that are fully contained within the container.
        /// The array is empty if no such rectangles are found.</returns>
        public readonly RectI[] OuterRectsInsideContainer(
            RectI container,
            NineRectsParts parts = NineRectsParts.Outer)
        {
            var rects = GetParts(parts);

            var foundRects = rects.Select(r =>
            {
                return container.Contains(r) ? r : RectI.Empty;
            }).Where(r => !r.IsEmpty).ToArray();

            return foundRects;
        }

        /// <summary>
        /// Returns the first outer rectangle that is fully contained within the specified container rectangle.
        /// </summary>
        /// <param name="container">The rectangle that defines the container area.</param>
        /// <param name="parts">The parts of the nine-part grid to consider for containment checking.
        /// Optional. Default is <see cref="NineRectsParts.All"/>.</param>
        /// <returns>The first rectangle that is fully contained within the container, or null if no such rectangle is found.</returns>
        public readonly RectI? OuterRectInsideContainer(
            RectI container,
            NineRectsParts parts = NineRectsParts.Outer)
        {
            var rects = GetParts(parts);

            var foundRect = Array.Find(rects, r =>
            {
                return container.Contains(r);
            });

            if (foundRect.IsEmpty)
                return null;
            else
                return foundRect;
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
        public static VerticalAlignment SuggestVertAlignmentForToolTip(RectD containerRect, RectD itemRect)
        {
            NineRects rects = new(containerRect, itemRect, scaleFactor: 1);
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