using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class TabControl
    {
        internal static void DrawTabControlInterior(
            Graphics dc,
            RectD bounds,
            RectD header,
            Brush brush,
            TabAlignment tabAlignment)
        {
            switch (tabAlignment)
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
                            dc,
                            brush,
                            rect,
                            border);
            }

            void DrawInteriorTop()
            {
                RectD rect =
                    (bounds.Left, header.Bottom, bounds.Width, bounds.Height - header.Height);
                DrawBorder(rect, (1, 0, 1, 1));
            }

            void DrawInteriorBottom()
            {
                RectD rect =
                    (bounds.Left, bounds.Top, bounds.Width, bounds.Height - header.Height);
                DrawBorder(rect, (1, 1, 1, 0));
            }

            void DrawInteriorLeft()
            {
                RectD rect =
                    (header.Right, bounds.Top, bounds.Width - header.Width, bounds.Height);
                DrawBorder(rect, (0, 1, 1, 1));
            }

            void DrawInteriorRight()
            {
                RectD rect =
                    (bounds.Left, bounds.Top, bounds.Width - header.Width, bounds.Height);
                DrawBorder(rect, (1, 1, 0, 1));
            }
        }

        internal static void DrawTabsInterior(
            CardPanelHeader control,
            Graphics dc,
            RectD rect,
            Brush brush,
            TabAlignment tabAlignment)
        {
            var tabCount = control.Tabs.Count;
            if (tabCount == 0)
                return;
            var tabIndex = control.SelectedTabIndex ?? 0;
            var tab = control.Tabs[tabIndex];
            var tabRect = tab.HeaderButton.Bounds;

            DrawTabsInterior(
                        dc,
                        rect,
                        tabRect,
                        brush,
                        tabAlignment,
                        tabIndex,
                        tabCount);

            if(tabAlignment == TabAlignment.Top || tabAlignment == TabAlignment.Bottom)
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

        internal static void DrawTabsInterior(
            Graphics dc,
            RectD rect,
            RectD tabRect,
            Brush brush,
            TabAlignment tabAlignment,
            int tabIndex,
            int tabCount)
        {
            const int PositionStart = 0;
            const int PositionEnd = 1;
            const int PositionOther = 2;

            int position;

            if (tabIndex == 0)
                position = PositionStart;
            else
            if (tabIndex >= tabCount - 1)
                position = PositionEnd;
            else
                position = PositionOther;

            RectD rectStart;
            RectD rectEnd;
            RectD rectOther;

            bool isTopOrBottom = tabAlignment == TabAlignment.Top
                || tabAlignment == TabAlignment.Bottom;

            if(isTopOrBottom)
            {
                rectStart = RectD.FromLTRB(
                    rect.TopLeft,
                    (tabRect.Left - 1, rect.Bottom));
                rectOther = RectD.FromLTRB(
                    (tabRect.Left - 1, rect.Top),
                    (tabRect.Right + 1, rect.Bottom));
                rectEnd = RectD.FromLTRB(
                    (tabRect.Right + 1, rect.Top),
                    rect.BottomRight);
            }
            else
            {
                rectStart = RectD.FromLTRB(
                    rect.TopLeft,
                    (rect.Right, tabRect.Top - 1));
                rectOther = RectD.FromLTRB(
                    (rect.Left, tabRect.Top - 1),
                    (rect.Right, tabRect.Bottom + 1));
                rectEnd = RectD.FromLTRB(
                    (rect.Left, tabRect.Bottom + 1),
                    rect.BottomRight);
            }

            var rects = new RectD[] { rectStart, rectOther, rectEnd };

            switch (tabAlignment)
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

            void DrawTopStart()
            {
                Thickness otherBorder = (1, 1, 0, 0);
                Thickness endBorder = (1, 0, 0, 1);
                Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawTopOther()
            {
                Thickness startBorder = (0, 0, 1, 1);
                Thickness otherBorder = (0, 1, 1, 0);
                Thickness endBorder = (0, 0, 0, 1);
                Thickness[] borders = { startBorder, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawTopEnd()
            {
                /*Thickness startBorder = (0, 0, 1, 1);
                Thickness otherBorder = (0, 1, 1, 0);
                Thickness[] borders = { startBorder, otherBorder, Thickness.Empty };
                DrawBorders(borders);*/
                DrawTopOther();
            }

            void DrawBottomStart()
            {
                Thickness otherBorder = (1, 0, 1, 1);
                Thickness endBorder = (0, 1, 0, 0);
                Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawBottomOther()
            {
                Thickness startBorder = (0, 1, 1, 0);
                Thickness otherBorder = (0, 0, 1, 1);
                Thickness endBorder = (0, 1, 0, 0);
                Thickness[] borders = { startBorder, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawBottomEnd()
            {
                /*Thickness startBorder = (0, 1, 1, 0);
                Thickness otherBorder = (0, 0, 1, 1);
                Thickness[] borders = { startBorder, otherBorder, Thickness.Empty };
                DrawBorders(borders);*/
                DrawBottomOther();
            }

            void DrawOnTop()
            {
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

            void DrawRightStart()
            {
                Thickness otherBorder = (0, 1, 1, 1);
                Thickness endBorder = (1, 0, 0, 0);
                Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawRightOther()
            {
                Thickness startBorder = (1, 0, 0, 1);
                Thickness otherBorder = (0, 0, 1, 1);
                Thickness endBorder = (1, 0, 0, 0);
                Thickness[] borders = { startBorder, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawRightEnd()
            {
                /*Thickness startBorder = (1, 0, 0, 1);
                Thickness otherBorder = (0, 0, 1, 1);
                Thickness[] borders = { startBorder, otherBorder, Thickness.Empty };
                DrawBorders(borders);*/
                DrawRightOther();
            }

            void DrawOnRight()
            {
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

            void DrawLeftStart()
            {
                Thickness otherBorder = (1, 1, 0, 1);
                Thickness endBorder = (0, 0, 1, 0);
                Thickness[] borders = { Thickness.Empty, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawLeftOther()
            {
                Thickness startBorder = (0, 0, 1, 1);
                Thickness otherBorder = (1, 0, 0, 1);
                Thickness endBorder = (0, 0, 1, 0);
                Thickness[] borders = { startBorder, otherBorder, endBorder };
                DrawBorders(borders);
            }

            void DrawLeftEnd()
            {
                /*Thickness startBorder = (0, 0, 1, 1);
                Thickness otherBorder = (1, 0, 0, 1);
                Thickness[] borders = { startBorder, otherBorder, Thickness.Empty };
                DrawBorders(borders);*/
                DrawLeftOther();
            }

            void DrawOnLeft()
            {
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
    }
}