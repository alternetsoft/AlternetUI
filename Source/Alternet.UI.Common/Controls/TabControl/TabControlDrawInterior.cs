using System;

using Alternet.Drawing;

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
        public virtual void DrawTabControlInterior(TabControlInteriorDrawParams prm)
        {
            switch (prm.TabAlignment)
            {
                case TabAlignment.Top:
                    DrawInteriorTop();
                    break;
                case TabAlignment.Bottom:
                    DrawInteriorBottom();
                    break;
                case TabAlignment.Left:
                    DrawInteriorLeft();
                    break;
                case TabAlignment.Right:
                    DrawInteriorRight();
                    break;
            }

            void DrawBorder(RectD rect, Thickness border)
            {
                DrawingUtils.DrawBorderWithBrush(
                            prm.Graphics,
                            prm.Brush,
                            rect,
                            border);
            }

            void DrawInteriorTop()
            {
                RectD rect = (
                    prm.Bounds.Left,
                    prm.HeaderBounds.Bottom,
                    prm.Bounds.Width,
                    prm.Bounds.Height - prm.HeaderBounds.Height);
                DrawBorder(rect, (1, 0, 1, 1));
            }

            void DrawInteriorBottom()
            {
                RectD rect = (
                    prm.Bounds.Left,
                    prm.Bounds.Top,
                    prm.Bounds.Width,
                    prm.Bounds.Height - prm.HeaderBounds.Height);
                DrawBorder(rect, (1, 1, 1, 0));
            }

            void DrawInteriorLeft()
            {
                RectD rect = (
                    prm.HeaderBounds.Right,
                    prm.Bounds.Top,
                    prm.Bounds.Width - prm.HeaderBounds.Width,
                    prm.Bounds.Height);
                DrawBorder(rect, (0, 1, 1, 1));
            }

            void DrawInteriorRight()
            {
                RectD rect = (
                    prm.Bounds.Left,
                    prm.Bounds.Top,
                    prm.Bounds.Width - prm.HeaderBounds.Width,
                    prm.Bounds.Height);
                DrawBorder(rect, (1, 1, 0, 1));
            }
        }

        /// <summary>
        /// Draws tab header interior.
        /// </summary>
        public virtual void DrawTabHeaderInterior(TabHeaderInteriorDrawParams prm)
        {
            var tabCount = prm.Control.Tabs.Count;
            if (tabCount == 0)
                return;
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

            DrawTabHeaderItemsInterior(prmItem);

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

                    var tab = prm.Control.Tabs[i];
                    var rect = tab.HeaderButton.Bounds;
                    PointD startPoint = (rect.Right + 1, rect.Top);
                    var height = Math.Min(rect.Height - 4, 12);
                    SizeD size = (1, height);
                    RectD drawRect = (startPoint, size);
                    var centeredRect = drawRect.CenterIn(rect, false, true);
                    DrawingUtils.DrawVertLine(prm.Graphics, prm.Brush, centeredRect.Location, height, 1);
                }
            }
        }

        /// <summary>
        /// Draws tab header items interior.
        /// </summary>
        public virtual void DrawTabHeaderItemsInterior(TabHeaderItemsInteriorDrawParams prm)
        {
            const int PositionStart = 0;
            const int PositionEnd = 1;
            const int PositionOther = 2;

            int position;

            if (prm.TabIndex == 0)
                position = PositionStart;
            else
                if (prm.TabIndex >= prm.TabCount - 1)
                    position = PositionEnd;
                else
                    position = PositionOther;

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
                            prm.Graphics,
                            prm.Brush,
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
                    case PositionStart:
                        DrawTopStart();
                        break;
                    case PositionOther:
                        DrawTopOther();
                        break;
                    case PositionEnd:
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
                    case PositionStart:
                        DrawRightStart();
                        break;
                    case PositionOther:
                        DrawRightOther();
                        break;
                    case PositionEnd:
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
                    case PositionStart:
                        DrawLeftStart();
                        break;
                    case PositionOther:
                        DrawLeftOther();
                        break;
                    case PositionEnd:
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
                    case PositionStart:
                        DrawBottomStart();
                        break;
                    case PositionOther:
                        DrawBottomOther();
                        break;
                    case PositionEnd:
                        DrawBottomEnd();
                        break;
                }
            }
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