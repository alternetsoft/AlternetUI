using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal sealed class ShapesPage : DrawingPage
    {
        private readonly Shapes shapes;

        public ShapesPage()
        {
            shapes = new Shapes(this);

            StrokePen = CreateStrokePen();
            FillBrush = CreateFillBrush();
            BackgroundBrush = Brushes.White;
        }

        public Color BrushColor
        {
            get => brushColor;
            set
            {
                brushColor = value;
                FillBrush = CreateFillBrush();
                Canvas?.Invalidate();
            }
        }

        public Color PenColor
        {
            get => penColor;
            set
            {
                penColor = value;
                StrokePen = CreateStrokePen();
                Canvas?.Invalidate();
            }
        }

        private Pen CreateStrokePen()
        {
            return new Pen(PenColor, 1);
        }

        private Brush CreateFillBrush()
        {
            return new SolidBrush(BrushColor);
        }

        private Color brushColor = Color.LightGreen;

        private Color penColor = Color.Blue;

        public override string Name => "Shapes";

        public Pen StrokePen { get; set; } = Pens.Blue;

        public Brush FillBrush { get; private set; }

        public Brush BackgroundBrush { get; private set; }

        public override void Draw(Graphics dc, RectD bounds)
        {
            dc.FillRectangle(Color.White.AsBrush, bounds);

            var s = shapes;

            var actions = new[]
            {
                new Cell("Draw Line", s.DrawLine),
                new Cell("Draw Lines", s.DrawLines),
                new Cell("Draw Polygon", s.DrawPolygon),
                new Cell("Fill Polygon", s.FillPolygon),
                new Cell("Draw Rect", s.DrawRectangle),
                new Cell("Fill Rect", s.FillRectangle),
                new Cell("Draw Rects", s.DrawRectangles),
                new Cell("Fill Rects", s.FillRectangles),
                new Cell("Draw Rounded Rect", s.DrawRoundedRectangle),
                new Cell("Fill Rounded Rect", s.FillRoundedRectangle),
                new Cell("Draw Circle", s.DrawCircle),
                new Cell("Fill Circle", s.FillCircle),
                new Cell("Draw Ellipse", s.DrawEllipse),
                new Cell("Fill Ellipse", s.FillEllipse),
                new Cell("Draw Bezier", s.DrawBezier),
                new Cell("Draw Beziers", s.DrawBeziers),
                new Cell("Draw Arc", s.DrawArc),
                new Cell("Fill Pie", s.FillPie),
                new Cell("Draw Pie", s.DrawPie),
            };

            DrawShapesGrid(dc, bounds, actions);
        }

        protected override AbstractControl CreateSettingsControl()
        {
            var control = new ShapesPageSettings();
            control.Initialize(this);
            return control;
        }

        private void DrawShapesGrid(Graphics dc, RectD bounds, Cell[] cells)
        {
            dc.DoInsideClipped(bounds, Internal);

            void Internal()
            {
                var textFormat = new TextFormat
                {
                    HorizontalAlignment = TextHorizontalAlignment.Center,
                    VerticalAlignment = TextVerticalAlignment.Bottom,
                    Wrapping = TextWrapping.Word,
                    Trimming = TextTrimming.Character,
                };

                const int ColumnCount = 5;
                const double CellMargin = 0;
                var cellSize =
                    (Math.Max(bounds.Width, bounds.Height) / (ColumnCount)) - CellMargin
                    - (CellMargin / (ColumnCount));

                double x = CellMargin, y = CellMargin;

                var cellBackgroundBrush = Brushes.White;
                var cellBorderPen = Pens.White;

                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];

                    var cellRect = new RectD(x, y, cellSize, cellSize);
                    if (cellRect.Width <= 0 || cellRect.Height <= 0)
                        continue;

                    var cellNameRect = cellRect.InflatedBy(-2, -2);
                    cellNameRect.Height -= 4;

                    var cw = cellNameRect.Width;

                    var cellName = DrawingUtils.WrapTextToMultipleLines(
                        cell.Name,
                        cw,
                        AbstractControl.DefaultFont,
                        dc);

                    var nameTextSize = dc.MeasureText(cellName, AbstractControl.DefaultFont);
                    bool nameVisible = nameTextSize.Width <= cellNameRect.Width;

                    var cellContentFrameRect = cellNameRect.InflatedBy(-5, -5);
                    if (nameVisible)
                    {
                        cellContentFrameRect.Top += nameTextSize.Height;
                        cellContentFrameRect.Height -= nameTextSize.Height;
                    }

                    if (cellContentFrameRect.Width <= 0 || cellContentFrameRect.Height <= 0)
                        continue;

                    var cellContentRect = cellContentFrameRect.InflatedBy(-10, -10);
                    if (cellContentRect.Width <= 0 || cellContentRect.Height <= 0)
                        continue;

                    dc.FillRectangle(cellBackgroundBrush, cellRect);
                    dc.DrawRectangle(cellBorderPen, cellRect);

                    dc.FillRectangle(BackgroundBrush, cellContentFrameRect);
                    dc.DrawRectangle(cellBorderPen, cellContentFrameRect);

                    cell.DrawAction(dc, cellContentRect);

                    if (nameVisible)
                    {
                        dc.DrawLabel(
                            cellName,
                            AbstractControl.DefaultFont,
                            Color.Black,
                            Color.Empty,
                            null,
                            cellNameRect);
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
        }

        private class Cell
        {
            public Cell(string name, Action<Graphics, RectD> drawAction)
            {
                Name = name;
                DrawAction = drawAction;
            }

            public string Name { get; }

            public Action<Graphics, RectD> DrawAction { get; }
        }
    }
}