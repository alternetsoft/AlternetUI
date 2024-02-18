using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class DrawInterior
    {
        public static void DrawTabsInterior(
            CardPanelHeader control,
            Graphics dc,
            Brush brush,
            TabAlignment tabAlignment)
        {
            var tabCount = control.Tabs.Count;
            if (tabCount == 0)
                return;
            RectD rect = control.ClientRectangle;
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
        }

        public static void DrawTabsInterior(
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

            var rectLeft = RectD.FromLTRB(
                rect.TopLeft,
                (tabRect.Left-1, rect.Bottom));
            var rectRight = RectD.FromLTRB(
                (tabRect.Right + 1, rect.Top),
                rect.BottomRight);
            var tabOuterRect = RectD.FromLTRB(
                (tabRect.Left-1, rect.Top),
                (tabRect.Right + 1, rect.Bottom));

            var rects = new RectD[] { rectLeft, tabOuterRect, rectRight};

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
                DrawingUtils.FillRectanglesBorder(
                            dc,
                            brush,
                            rects,
                            borders);
            }

            void DrawTopStart()
            {
                Thickness cBorder = (1, 1, 0, 0);
                Thickness rBorder = (1, 0, 0, 1);
                Thickness[] borders = { Thickness.Empty, cBorder, rBorder };
                DrawBorders(borders);
            }

            void DrawTopOther()
            {
                Thickness lBorder = (0, 0, 1, 1);
                Thickness cBorder = (0, 1, 0, 0);
                Thickness rBorder = (1, 0, 0, 1);
                Thickness[] borders = { lBorder, cBorder, rBorder };
                DrawBorders(borders);
            }

            void DrawTopEnd()
            {
                Thickness lBorder = (0, 0, 1, 1);
                Thickness cBorder = (0, 1, 1, 0);
                Thickness[] borders = { lBorder, cBorder, Thickness.Empty };
                DrawBorders(borders);
            }

            void DrawBottomStart()
            {

            }

            void DrawBottomOther()
            {
                Thickness lBorder = (0, 1, 1, 0);
                Thickness cBorder = (0, 0, 0, 1);
                Thickness rBorder = (1, 1, 0, 0);
                Thickness[] borders = { lBorder, cBorder, rBorder };
                DrawBorders(borders);
            }

            void DrawBottomEnd()
            {

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

            }
            
            void DrawRightOther()
            {

            }

            void DrawRightEnd()
            {

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

            }
            
            void DrawLeftOther()
            {

            }

            void DrawLeftEnd()
            {

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