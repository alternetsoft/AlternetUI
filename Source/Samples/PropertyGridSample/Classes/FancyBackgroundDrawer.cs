using System;

using Alternet.UI;
using Alternet.Drawing;

public static class FancyBackgroundDrawer
{
    /// <summary>
    /// Draws a fancy, randomly-generated background in the specified rectangle on the given WinForms Graphics.
    /// </summary>
    /// <param name="g">The Graphics to draw on.</param>
    /// <param name="rect">The target rectangle area.</param>
    /// <param name="bgColor">Optional background color (default: random pastel).</param>
    /// <param name="seed">Optional seed for repeatable random backgrounds.</param>
    public static void DrawFancyBackground(Graphics g, RectD bounds, Color? bgColor = null, int? seed = null)
    {
        var rect = bounds.ToRect();

        var rand = seed.HasValue ? new Random(seed.Value) : new Random();

        // Choose soft pastel background color if not specified
        Color fill = bgColor ?? Color.FromArgb(255,
                 rand.Next(180, 256),
                 rand.Next(180, 256),
                 rand.Next(180, 256));
        g.FillRectangle(fill.AsBrush, rect);

        // Draw random circles
        int circleCount = rand.Next(7, 15);
        for (int i = 0; i < circleCount; i++)
        {
            var cx = rand.Next(rect.Left, rect.Right);
            var cy = rand.Next(rect.Top, rect.Bottom);
            int radius = rand.Next(18, 48);
            Color c = Color.FromArgb(rand.Next(60, 130), rand.Next(120, 240), rand.Next(120, 240), rand.Next(120, 240));
            g.FillEllipse(c.AsBrush, (cx - radius, cy - radius, radius * 2, radius * 2));
        }

        // Draw random polygons (show how to extend: triangles, quadrilaterals)
        int polyCount = rand.Next(2, 6);
        for (int i = 0; i < polyCount; i++)
        {
            int points = rand.Next(3, 6); // triangles/quads/pentagons
            PointD[] polyPts = new PointD[points];
            for (int j = 0; j < points; j++)
            {
                polyPts[j] = new PointD(
                    rand.Next(rect.Left, rect.Right),
                    rand.Next(rect.Top, rect.Bottom)
                );
            }
            Color c = Color.FromArgb(rand.Next(60, 100), rand.Next(32, 220), rand.Next(32, 220), rand.Next(32, 220));            
            g.FillPolygon(c.AsBrush, polyPts);
        }
    }
}