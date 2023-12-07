using Alternet.UI;
using Alternet.Drawing;

namespace MinMaster
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Bitmap image = new(700, 500);

            var control = new PictureBox
            {
                Parent = this,
                Image = image,
                ImageStretch = false,
            };

            using var dc = control.Canvas;

            if(dc is not null)
            {
                var font = Font.Default.Scaled(5);
                var measure = dc.MeasureText("Hello text", font);

                dc.FillRectangle(Brushes.White, (0, 0, image.Width, image.Height));
                dc.DrawText("Hello text", font, Color.Black.AsBrush, (5, 5));
                dc.DrawRectangle(Color.Navy.AsPen, (5, 5, measure.Width, measure.Height));
                DrawWave(dc, (5, 5 + measure.Height, 10, 10), Color.Red);
            }
        }

        /// <summary>
        /// Draws waved line in the specified rectangular area.
        /// </summary>
        /// <param name="rect">Rectangle that bounds the drawing area for the wave.</param>
        /// <param name="color">Color used to draw wave.</param>
        public virtual void DrawWave(DrawingContext dc, Rect rect, Color color)
        {
            Draw(dc, rect.ToRect(), color);

            void Draw(DrawingContext dc, Int32Rect rect, Color color)
            {
                int minSize = 4;
                int offset = 6;

                int left = rect.Left - (rect.Left % offset);
                int i = rect.Right % offset;
                int right = (i != 0) ? rect.Right + (offset - i) : rect.Right;

                int scale = 2;
                int size = (right - left) / scale;

                offset = 3;

                if (size < minSize)
                    size = minSize;
                else
                {
                    i = (int)((size - minSize) / offset);
                    if ((size - minSize) % offset != 0)
                        i++;
                    size = minSize + (i * offset);
                }

                Point[] pts = new Point[size];
                for (int index = 0; index < size; index++)
                {
                    pts[index].X = left + (index * scale);
                    pts[index].Y = rect.Bottom - 1;
                    switch (index % 3)
                    {
                        case 0:
                            {
                                pts[index].Y -= scale;
                                break;
                            }

                        case 2:
                            {
                                pts[index].Y += scale;
                                break;
                            }
                    }
                }

                dc.DrawBeziers(color.GetAsPen(1), pts);
            }
        }

    }
}
