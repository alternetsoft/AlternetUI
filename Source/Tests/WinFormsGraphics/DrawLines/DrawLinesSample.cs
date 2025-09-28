using System.Drawing;

class Program
{
    static void Main()
    {
        var bmp = new Bitmap(150, 150);
        using (var g = Graphics.FromImage(bmp))
        using (var pen = new Pen(Color.Red, 3))
        {
            // Example points (A, B, C, D)
            var points = new[]
            {
                new Point(20, 20),   // A
                new Point(120, 30),  // B
                new Point(100, 120), // C
                new Point(30, 100),  // D
            };

            g.Clear(Color.White);
            g.DrawLines(pen, points);

            // Draw points for clarity
            foreach (var pt in points)
                g.FillEllipse(Brushes.Blue, pt.X - 4, pt.Y - 4, 8, 8);
        }

        bmp.Save("DrawLinesResult.png");
    }
}