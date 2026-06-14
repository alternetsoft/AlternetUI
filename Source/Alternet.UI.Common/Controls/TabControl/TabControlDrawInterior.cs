using System;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Implementation of the <see cref="TabControl"/> interior drawing.
    /// </summary>
    public partial class TabControlDrawInterior
    {
        /// <summary>
        /// Derfault provider for the <see cref="TabControl"/> interior drawing.
        /// </summary>
        public static TabControlDrawInterior Default = new();

        /// <summary>
        /// Draws <see cref="TabControl"/> interior.
        /// </summary>
        public virtual void DrawTabControlInterior(ref TabControlInteriorDrawParams prm)
        {
            var dc = prm.Graphics;
            var brush = prm.Brush;

            GetTabControlBorderRect(prm.Bounds, prm.HeaderBounds, prm.TabAlignment, out RectD rect, out Thickness border);

            DrawBorder(rect, border);

            void DrawBorder(RectD rect, Thickness border)
            {
                DrawingUtils.DrawBorderWithBrush(
                            dc,
                            brush,
                            rect,
                            border);
            }
        }

        /// <summary>
        /// Gets the rectangle and border thickness for drawing the <see cref="TabControl"/> interior.
        /// </summary>
        /// <param name="rect">The rectangle for the tab control interior.</param>
        /// <param name="border">The border thickness for the tab control interior.</param>
        /// <param name="bounds">The bounds of the tab control.</param>
        /// <param name="headerBounds">The bounds of the tab control header.</param>
        /// <param name="tabAlignment">The alignment of the tab control header.</param>
        public virtual void GetTabControlBorderRect(
            RectD bounds,
            RectD headerBounds,
            TabAlignment tabAlignment,
            out RectD rect,
            out Thickness border)
        {
            switch (tabAlignment)
            {
                case TabAlignment.Top:
                    GetInteriorTop(out rect, out border);
                    break;
                case TabAlignment.Bottom:
                    GetInteriorBottom(out rect, out border);
                    break;
                case TabAlignment.Left:
                    GetInteriorLeft(out rect, out border);
                    break;
                case TabAlignment.Right:
                    GetInteriorRight(out rect, out border);
                    break;
                default:
                    rect = default;
                    border = default;
                    break;
            }

            void GetInteriorTop(out RectD rect, out Thickness border)
            {
                rect = (
                    bounds.Left,
                    headerBounds.Bottom,
                    bounds.Width,
                    bounds.Height - headerBounds.Height);
                border = (1, 0, 1, 1);
            }

            void GetInteriorBottom(out RectD rect, out Thickness border)
            {
                rect = (
                    bounds.Left,
                    bounds.Top,
                    bounds.Width,
                    bounds.Height - headerBounds.Height);
                border = (1, 1, 1, 0);
            }

            void GetInteriorLeft(out RectD rect, out Thickness border)
            {
                rect = (
                    headerBounds.Right,
                    bounds.Top,
                    bounds.Width - headerBounds.Width,
                    bounds.Height);
                border = (0, 1, 1, 1);
            }

            void GetInteriorRight(out RectD rect, out Thickness border)
            {
                rect = (
                    bounds.Left,
                    bounds.Top,
                    bounds.Width - headerBounds.Width,
                    bounds.Height);
                border = (1, 1, 0, 1);
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TabHeaderItemsInteriorDrawParams"/>.
        /// </summary>
        /// <param name="prm">The parameters for drawing tab header interior.</param>
        /// <returns>A new instance of <see cref="TabHeaderItemsInteriorDrawParams"/>.</returns>
        public virtual TabHeaderItemsInteriorDrawParams CreateTabHeaderItemsInteriorDrawParams(
            ref TabHeaderInteriorDrawParams prm)
        {
            var tabCount = prm.Control.Tabs.Count;
            var tabIndex = prm.Control.SelectedTabIndex ?? 0;
            var tab = prm.Control.Tabs[tabIndex];
            var tabRect = tab.HeaderButton.Bounds;

            TabHeaderItemsInteriorDrawParams prmItem = new()
            {
                Graphics = prm.Graphics,
                Rect = prm.Bounds,
                TabRect = tabRect,
                Brush = prm.Brush,
                TabAlignment = prm.TabAlignment,
                TabIndex = tabIndex,
                TabCount = tabCount,
                RoundCorners = prm.RoundCorners,
            };

            return prmItem;
        }

        /// <summary>
        /// Draws tab header interior.
        /// </summary>
        public virtual void DrawTabHeaderInterior(ref TabHeaderInteriorDrawParams prm)
        {
            var tabCount = prm.Control.Tabs.Count;
            if (tabCount == 0)
                return;
            var tabIndex = prm.Control.SelectedTabIndex ?? 0;

            var prmItem = CreateTabHeaderItemsInteriorDrawParams(ref prm);

            DrawTabHeaderItemsInterior(ref prmItem);

            var dc = prm.Graphics;
            var brush = prm.Brush;
            var control = prm.Control;

            if (prm.TabAlignment == TabAlignment.Top || prm.TabAlignment == TabAlignment.Bottom)
                DrawLines();

            void DrawLines()
            {
                if (tabCount < 3)
                    return;

                for (int i = 0; i < tabCount - 1; i++)
                {
                    if (i == tabIndex || i == (tabIndex - 1))
                        continue;

                    var tab = control.Tabs[i];
                    var rect = tab.HeaderButton.Bounds;
                    PointD startPoint = (rect.Right + 1, rect.Top);
                    var height = Math.Min(rect.Height - 4, 12);
                    SizeD size = (1, height);
                    RectD drawRect = (startPoint, size);
                    var centeredRect = drawRect.CenterIn(rect, false, true);
                    DrawingUtils.DrawVertLine(dc, brush, centeredRect.Location, height, 1);
                }
            }
        }

        /// <summary>
        /// Gets rectangles for the <see cref="TabControl"/> interior drawing.
        /// </summary>
        /// <param name="prm">The parameters for drawing tab header items interior.</param>
        /// <returns>An array of rectangles for the tab control interior drawing.</returns>
        public virtual RectD[] GetRects(ref TabHeaderItemsInteriorDrawParams prm)
        {
            RectD rectStart;
            RectD rectEnd;
            RectD rectOther;

            bool isTopOrBottom = prm.TabAlignment == TabAlignment.Top
                || prm.TabAlignment == TabAlignment.Bottom;

            if (isTopOrBottom)
            {
                rectStart = RectD.FromLTRB(
                    prm.Rect.TopLeft,
                    (prm.TabRect.Left - 1, prm.Rect.Bottom));
                rectOther = RectD.FromLTRB(
                    (prm.TabRect.Left - 1, prm.Rect.Top),
                    (prm.TabRect.Right + 1, prm.Rect.Bottom));
                rectEnd = RectD.FromLTRB(
                    (prm.TabRect.Right + 1, prm.Rect.Top),
                    prm.Rect.BottomRight);
            }
            else
            {
                rectStart = RectD.FromLTRB(
                    prm.Rect.TopLeft,
                    (prm.Rect.Right, prm.TabRect.Top - 1));
                rectOther = RectD.FromLTRB(
                    (prm.Rect.Left, prm.TabRect.Top - 1),
                    (prm.Rect.Right, prm.TabRect.Bottom + 1));
                rectEnd = RectD.FromLTRB(
                    (prm.Rect.Left, prm.TabRect.Bottom + 1),
                    prm.Rect.BottomRight);
            }

            var rects = new RectD[] { rectStart, rectOther, rectEnd };
            return rects;
        }

        /// <summary>
        /// Draws tab header items interior.
        /// </summary>
        public virtual void DrawTabHeaderItemsInterior(ref TabHeaderItemsInteriorDrawParams prm)
        {
            var dc = prm.Graphics;
            var brush = prm.Brush;
            var position = prm.TabPosition;

            var rects = GetRects(ref prm);

            switch (prm.TabAlignment)
            {
                case TabAlignment.Top:
                    DrawOnTop();
                    break;
                case TabAlignment.Bottom:
                    DrawOnBottom();
                    break;
                case TabAlignment.Left:
                    DrawOnLeft();
                    break;
                case TabAlignment.Right:
                    DrawOnRight();
                    break;
            }

            void DrawBorders(Thickness[] borders)
            {
#pragma warning disable
                DrawingUtils.DrawBordersWithBrush(
                            dc,
                            brush,
                            rects,
                            borders);
#pragma warning restore
            }

            void DrawOnTop()
            {
                void DrawTopStart()
                {
                    Thickness otherBorder = new(left: 1, top: 1, right: 0, bottom: 0);
                    Thickness endBorder = new(left: 1, top: 0, right: 0, bottom: 1);
                    Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawTopOther()
                {
                    Thickness startBorder = new(left: 0, top: 0, right: 1, bottom: 1);
                    Thickness otherBorder = new(left: 0, top: 1, right: 1, bottom: 0);
                    Thickness endBorder = new(left: 0, top: 0, right: 0, bottom: 1);
                    Thickness[] borders = { startBorder, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawTopEnd()
                {
                    DrawTopOther();
                }

                switch (position)
                {
                    case TabHeaderItemsInteriorDrawParams.PositionStart:
                        DrawTopStart();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionMiddle:
                        DrawTopOther();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionEnd:
                        DrawTopEnd();
                        break;
                }
            }

            void DrawOnRight()
            {
                void DrawRightStart()
                {
                    Thickness otherBorder = new(left: 0, top: 1, right: 1, bottom: 1);
                    Thickness endBorder = new(left: 1, top: 0, right: 0, bottom: 0);
                    Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawRightOther()
                {
                    Thickness startBorder = new(left: 1, top: 0, right: 0, bottom: 1);
                    Thickness otherBorder = new(left: 0, top: 0, right: 1, bottom: 1);
                    Thickness endBorder = new(left: 1, top: 0, right: 0, bottom: 0);
                    Thickness[] borders = { startBorder, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawRightEnd()
                {
                    DrawRightOther();
                }

                switch (position)
                {
                    case TabHeaderItemsInteriorDrawParams.PositionStart:
                        DrawRightStart();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionMiddle:
                        DrawRightOther();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionEnd:
                        DrawRightEnd();
                        break;
                }
            }

            void DrawOnLeft()
            {
                void DrawLeftStart()
                {
                    Thickness otherBorder = new(left: 1, top: 1, right: 0, bottom: 1);
                    Thickness endBorder = new(left: 0, top: 0, right: 1, bottom: 0);
                    Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawLeftOther()
                {
                    Thickness startBorder = new(left: 0, top: 0, right: 1, bottom: 1);
                    Thickness otherBorder = new(left: 1, top: 0, right: 0, bottom: 1);
                    Thickness endBorder = new(left: 0, top: 0, right: 1, bottom: 0);
                    Thickness[] borders = { startBorder, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawLeftEnd()
                {
                    DrawLeftOther();
                }

                switch (position)
                {
                    case TabHeaderItemsInteriorDrawParams.PositionStart:
                        DrawLeftStart();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionMiddle:
                        DrawLeftOther();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionEnd:
                        DrawLeftEnd();
                        break;
                }
            }

            void DrawOnBottom()
            {
                void DrawBottomStart()
                {
                    Thickness otherBorder = new(left: 1, top: 0, right: 1, bottom: 1);
                    Thickness endBorder = new(left: 0, top: 1, right: 0, bottom: 0);
                    Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawBottomOther()
                {
                    Thickness startBorder = new(left: 0, top: 1, right: 1, bottom: 0);
                    Thickness otherBorder = new(left: 0, top: 0, right: 1, bottom: 1);
                    Thickness endBorder = new(left: 0, top: 1, right: 0, bottom: 0);
                    Thickness[] borders = { startBorder, otherBorder, endBorder };
                    DrawBorders(borders);
                }

                void DrawBottomEnd()
                {
                    DrawBottomOther();
                }

                switch (position)
                {
                    case TabHeaderItemsInteriorDrawParams.PositionStart:
                        DrawBottomStart();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionMiddle:
                        DrawBottomOther();
                        break;
                    case TabHeaderItemsInteriorDrawParams.PositionEnd:
                        DrawBottomEnd();
                        break;
                }
            }
        }

        internal static void TestNewPainting(TabControl tc, PaintEventArgs e)
        {
            var tabCount = tc.Header.Tabs.Count;

            if (tabCount > 0)
            {
                var tabIndex = tc.Header.SelectedTabIndex ?? 0;
                var tab = tc.Header.Tabs[tabIndex];

                var r1 = tc.Contents.Bounds;
                r1.Inflate(2, 2);

                var r2 = tab.HeaderButton.Bounds;
                r2.Offset(tc.Header.Location);

                e.Graphics.FillRectangle(LightDarkColors.Green.WithAlpha(50).AsBrush, r1);
                e.Graphics.FillRectangle(LightDarkColors.Red.WithAlpha(50).AsBrush, r2);

                var scaleFactor = tc.ScaleFactor;

                var canvas = SkiaUtils.CreateBitmapCanvas(
                    tc.ClientRectangle.Size,
                    scaleFactor,
                    isTransparent: true);
                canvas.UseUnscaledDrawImage = true;

                var panelLeft = r1.Left;
                var panelRight = r1.Right;
                var panelTop = r1.Top;
                var panelBottom = r1.Bottom;

                var tabLeft = r2.Left;
                var tabRight = r2.Right;
                var tabHeight = r2.Height;

                var path = CreateFullyRoundedTabPath(
                panelLeft, panelRight, panelBottom, panelTop,
                tabLeft, tabRight, tabHeight,
                5);

                canvas.Canvas.DrawPath(path, LightDarkColors.Red.AsPen);

                var skBitmap = canvas.Bitmap;

                if (skBitmap is null || skBitmap.Width == 0 || skBitmap.Height == 0)
                    return;

                var bitmap = (Image)skBitmap;
                e.Graphics.DrawImage(bitmap, tc.ClientRectangle.Location);
            }
        }

        private static SKPath CreateFullyRoundedTabPath(
                float panelLeft, float panelRight, float panelBottom, float panelTop,
                float tabLeft, float tabRight, float tabHeight,
                float radius)
        {
            SKPath path = new SKPath();
            float tabTopY = panelTop - tabHeight;

            // --- 1. START: Midpoint of the left wall going UP ---
            path.MoveTo(panelLeft, (panelTop + panelBottom) / 2f);

            // --- 2. TOP-LEFT CONTAINER CORNER ---
            // Tangent 1 is top-left corner; Tangent 2 is the tab's left flare entry
            path.ArcTo(new SKPoint(panelLeft, panelTop), new SKPoint(tabLeft - radius, panelTop), radius);

            // --- 3. BOTTOM-LEFT TAB FLARE ---
            // Turns upward into the vertical tab wall
            path.ArcTo(new SKPoint(tabLeft, panelTop), new SKPoint(tabLeft, tabTopY), radius);

            // --- 4. TOP-LEFT TAB CORNER ---
            // Turns right across the top edge of the tab
            path.ArcTo(new SKPoint(tabLeft, tabTopY), new SKPoint(tabRight, tabTopY), radius);

            // --- 5. TOP-RIGHT TAB CORNER ---
            // Turns downward toward the main panel baseline
            path.ArcTo(new SKPoint(tabRight, tabTopY), new SKPoint(tabRight, panelTop), radius);

            // --- 6. BOTTOM-RIGHT TAB FLARE ---
            // Turns right onto the remaining top baseline of the container
            path.ArcTo(new SKPoint(tabRight, panelTop), new SKPoint(panelRight, panelTop), radius);

            // --- 7. TOP-RIGHT CONTAINER CORNER ---
            // Turns down the right wall of the container
            path.ArcTo(new SKPoint(panelRight, panelTop), new SKPoint(panelRight, panelBottom), radius);

            // --- 8. BOTTOM-RIGHT CONTAINER CORNER ---
            // Turns left across the bottom floor of the container
            path.ArcTo(new SKPoint(panelRight, panelBottom), new SKPoint(panelLeft, panelBottom), radius);

            // --- 9. BOTTOM-LEFT CONTAINER CORNER ---
            // Turns back up the left wall to meet our starting point
            path.ArcTo(new SKPoint(panelLeft, panelBottom), new SKPoint(panelLeft, panelTop), radius);

            // Close the loop cleanly
            path.Close();
            return path;
        }

        /*
        panelLeft: The absolute horizontal coordinate X for the left edge of the main content window.
        panelRight: The absolute horizontal coordinate X for the right edge of the main content window.
        panelTop: The vertical coordinate Y for the top horizontal baseline of the main panel
        (the line that the tab sits on top of).
        panelBottom: The vertical coordinate Y for the very bottom edge of the main content container.
        
        tabLeft: The absolute horizontal coordinate X where the vertical left wall of the tab begins.
        tabRight: The absolute horizontal coordinate X where the vertical right wall of the tab ends.
        tabHeight: The structural height of the tab itself, measured upward from the panelTop line.        
        */
        private static SKPath CreateTabWithPanelPath(
                float panelLeft, float panelRight, float panelBottom, float panelTop,
                float tabLeft, float tabRight, float tabHeight,
                float cornerRadius)
        {
            SKPath path = new SKPath();

            float tabTopY = panelTop - tabHeight;

            // 1. Start from the bottom-left of the main content panel
            path.MoveTo(panelLeft, panelBottom);

            // 2. Up to the top-left corner of the main panel
            path.LineTo(panelLeft, panelTop);

            // 3. Move along the top panel line until we hit the tab's left flare entry point
            // We stop short by the radius size to give room for the reverse flare curve
            path.LineTo(tabLeft - cornerRadius, panelTop);

            // 4. BOTTOM-LEFT TAB FLARE (Concave curve turning UP into the tab)
            // Tangent 1: (tabLeft, panelTop), Tangent 2: (tabLeft, tabTopY)
            path.ArcTo(new SKPoint(tabLeft, panelTop), new SKPoint(tabLeft, tabTopY), cornerRadius);

            // 5. TOP-LEFT TAB CORNER (Convex curve turning RIGHT across the tab top)
            path.ArcTo(new SKPoint(tabLeft, tabTopY), new SKPoint(tabRight, tabTopY), cornerRadius);

            // 6. TOP-RIGHT TAB CORNER (Convex curve turning DOWN towards the panel)
            path.ArcTo(new SKPoint(tabRight, tabTopY), new SKPoint(tabRight, panelTop), cornerRadius);

            // 7. BOTTOM-RIGHT TAB FLARE (Concave curve turning RIGHT onto the panel top)
            path.ArcTo(new SKPoint(tabRight, panelTop), new SKPoint(panelRight, panelTop), cornerRadius);

            // 8. Continue along the rest of the top panel line to the right edge
            path.LineTo(panelRight, panelTop);

            // 9. Down the right side and across the bottom to close the panel shape
            path.LineTo(panelRight, panelBottom);
            path.LineTo(panelLeft, panelBottom);

            path.Close();
            return path;
        }

        /// <summary>
        /// Defines the parameters for drawing the borders of a tab control header.
        /// </summary>
        public struct TabHeaderInteriorDrawParams
        {
            /// <summary>
            /// Gets or sets a value indicating whether border has rounded corners.
            /// </summary>
            public bool RoundCorners;

            /// <summary>
            /// The control to which the tab page belongs.
            /// </summary>
            public CardPanelHeader Control;

            /// <summary>
            /// The graphics context used for drawing.
            /// </summary>
            public Graphics Graphics;

            /// <summary>
            /// The rectangle representing the tab control area.
            /// </summary>
            public RectD Bounds;

            /// <summary>
            /// The brush used for drawing the borders.
            /// </summary>
            public Brush Brush;

            /// <summary>
            /// The alignment of the tab page (top, bottom, left, right).
            /// </summary>
            public TabAlignment TabAlignment;
        }

        /// <summary>
        /// Defines the parameters for drawing the borders of tab header items.
        /// </summary>
        public struct TabHeaderItemsInteriorDrawParams
        {
            /// <summary>
            /// Start position identifier.
            /// </summary>
            public const int PositionStart = 0;

            /// <summary>
            /// Middle position identifier.
            /// </summary>
            public const int PositionMiddle = 1;

            /// <summary>
            /// End position identifier.
            /// </summary>
            public const int PositionEnd = 2;

            /// <summary>
            /// Gets or sets a value indicating whether border has rounded corners.
            /// </summary>
            public bool RoundCorners;

            /// <summary>
            /// The graphics context used for drawing.
            /// </summary>
            public Graphics Graphics;

            /// <summary>
            /// The rectangle representing the bounds of the tab header.
            /// </summary>
            public RectD Rect;

            /// <summary>
            /// The rectangle representing the header item area.
            /// </summary>
            public RectD TabRect;

            /// <summary>
            /// The brush used for drawing the borders.
            /// </summary>
            public Brush Brush;

            /// <summary>
            /// The alignment of the tab page (top, bottom, left, right).
            /// </summary>
            public TabAlignment TabAlignment;

            /// <summary>
            /// The index of the tab.
            /// </summary>            
            public int TabIndex;

            /// <summary>
            /// Total number of tabs.
            /// </summary>
            public int TabCount;

            /// <summary>
            /// Gets the position of the tab (start, other, end).
            /// </summary>
            public int TabPosition
            {
                get
                {
                    int position;

                    if (TabIndex == 0)
                        position = PositionStart;
                    else
                        if (TabIndex >= TabCount - 1)
                            position = PositionEnd;
                        else
                            position = PositionMiddle;
                    return position;
                }
            }
        }

        /// <summary>
        /// Defines the parameters for drawing the borders of a tab control.
        /// </summary>
        public struct TabControlInteriorDrawParams
        {
            /// <summary>
            /// Gets or sets a value indicating whether border has rounded corners.
            /// </summary>
            public bool RoundCorners;

            /// <summary>
            /// The control to which the tab page belongs.
            /// </summary>
            public CardPanelHeader Control;
            
            /// <summary>
            /// The graphics context used for drawing.
            /// </summary>
            public Graphics Graphics;

            /// <summary>
            /// The rectangle representing the tab control area.
            /// </summary>
            public RectD Bounds;

            /// <summary>
            /// The brush used for drawing the borders.
            /// </summary>
            public Brush Brush;

            /// <summary>
            /// The alignment of the tab page (top, bottom, left, right).
            /// </summary>
            public TabAlignment TabAlignment;

            /// <summary>
            /// The rectangle representing the bounds of the tab page header, used for drawing borders.
            /// </summary>
            public RectD HeaderBounds;
        }
    }
}