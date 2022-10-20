using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal sealed class ShapesPage : DrawingPage
    {
        private Shapes shapes;

        public ShapesPage()
        {
            shapes = new Shapes(this);
        }

        public override string Name => "Shapes";

        public Pen StrokePen { get; set; } = Pens.Blue;

        public Brush FillBrush { get; set; } = Brushes.LightGreen;

        public Brush BackgroundBrush { get; set; } = Brushes.White;

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            var s = shapes;

            var actions = new[]
            {
                new Cell(nameof(s.DrawLine), s.DrawLine),
                new Cell(nameof(s.DrawLines), s.DrawLines),
                new Cell(nameof(s.DrawPolygon), s.DrawPolygon),
                new Cell(nameof(s.FillPolygon), s.FillPolygon),
                new Cell(nameof(s.DrawRectangle), s.DrawRectangle),
                new Cell(nameof(s.FillRectangle), s.FillRectangle),
                new Cell(nameof(s.DrawRectangles), s.DrawRectangles),
                new Cell(nameof(s.FillRectangles), s.FillRectangles),
                new Cell(nameof(s.DrawRoundedRectangle), s.DrawRoundedRectangle),
                new Cell(nameof(s.FillRoundedRectangle), s.FillRoundedRectangle),
                new Cell(nameof(s.DrawCircle), s.DrawCircle),
                new Cell(nameof(s.FillCircle), s.FillCircle),
                new Cell(nameof(s.DrawEllipse), s.DrawEllipse),
                new Cell(nameof(s.FillEllipse), s.FillEllipse),
                new Cell(nameof(s.DrawBezier), s.DrawBezier),
                new Cell(nameof(s.DrawBeziers), s.DrawBeziers),
                new Cell(nameof(s.DrawArc), s.DrawArc),
                new Cell(nameof(s.FillPie), s.FillPie),
                new Cell(nameof(s.DrawPie), s.DrawPie),
            };

            DrawShapesGrid(dc, bounds, actions);
        }

        protected override Control CreateSettingsControl()
        {
            var control = new ShapesPageSettings();
            control.Initialize(this);
            return control;
        }

        private void DrawShapesGrid(DrawingContext dc, Rect bounds, Cell[] cells)
        {
            const int ColumnCount = 5;
            const double CellMargin = 10;
            var cellSize = (Math.Min(bounds.Width, bounds.Height) / ColumnCount) - CellMargin - (CellMargin / ColumnCount);

            double x = CellMargin, y = CellMargin;

            var cellBackgroundBrush = Brushes.LightGray;
            var cellBorderPen = Pens.Black;

            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];

                var cellRect = new Rect(x, y, cellSize, cellSize);
                if (cellRect.Width <= 0 || cellRect.Height <= 0)
                    continue;

                var cellNameRect = cellRect.InflatedBy(-2, -2);
                cellNameRect.Height -= 4;

                var nameTextSize = dc.MeasureText(cell.Name, Control.DefaultFont);
                bool nameVisible = nameTextSize.Width <= cellNameRect.Width;

                var cellContentFrameRect = cellNameRect.InflatedBy(-5, -5);
                if (nameVisible)
                    cellContentFrameRect.Height -= dc.MeasureText(cell.Name, Control.DefaultFont).Height;

                if (cellContentFrameRect.Width <= 0 || cellContentFrameRect.Height <= 0)
                    continue;

                var cellContentRect = cellContentFrameRect.InflatedBy(-10, -10);
                if (cellContentRect.Width <= 0 || cellContentRect.Height <= 0)
                    continue;

                dc.FillRectangle(cellBackgroundBrush, cellRect);
                dc.DrawRectangle(cellBorderPen, cellRect);

                dc.FillRectangle(BackgroundBrush, cellContentFrameRect);
                dc.DrawRectangle(cellBorderPen, cellContentFrameRect);

                // todo: set clip

                cell.DrawAction(dc, cellContentRect);

                if (nameVisible)
                {
                    dc.DrawText(
                        cell.Name,
                        Control.DefaultFont,
                        Brushes.Black,
                        cellNameRect,
                        new TextFormat
                        {
                            HorizontalAlignment = TextHorizontalAlignment.Center,
                            VerticalAlignment = TextVerticalAlignment.Bottom,
                            Wrapping = TextWrapping.None,
                            Trimming = TextTrimming.Pixel
                        });
                }

                if ((i + 1) % ColumnCount == 0)
                {
                    x = CellMargin;
                    y += cellSize + CellMargin;
                }
                else
                {
                    x += cellSize + CellMargin;
                }
            }
        }

        private class Cell
        {
            public Cell(string name, Action<DrawingContext, Rect> drawAction)
            {
                Name = name;
                DrawAction = drawAction;
            }

            public string Name { get; }

            public Action<DrawingContext, Rect> DrawAction { get; }
        }
    }
}