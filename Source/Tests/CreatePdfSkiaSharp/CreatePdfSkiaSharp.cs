using System.IO;
using SkiaSharp;

public class SkiaPdfDemo
{
    public static void CreateSamplePdf(string outputPath)
    {
        // Create the PDF document
        using (var stream = File.OpenWrite(outputPath))
        using (var document = SKDocument.CreatePdf(stream))
        {
            // Start a new page
            SKCanvas canvas = document.BeginPage(595, 842); // A4 size in points (72 DPI)
            // Draw something
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Black;
                paint.TextSize = 32;
                canvas.DrawText("Hello, SkiaSharp PDF!", 100, 100, paint);

                paint.Color = SKColors.Red;
                paint.TextSize = 24;
                canvas.DrawRect(new SKRect(100, 120, 300, 200), paint);

                paint.Color = SKColors.Blue;
                paint.TextSize = 20;
                canvas.DrawText("This is a rectangle.", 100, 240, paint);
            }
            // Finish the page
            document.EndPage();
            // Optionally add more pages...

            // Finish the document
            document.Close();
        }
    }
}