using Alternet.UI;

internal class CustomDrawnControl : Control
{
    protected override void OnPaint(PaintEventArgs e)
    {
        var context = e.DrawingContext;
        context.FillRectangle(brush, Handler.ClientRectangle);
        context.DrawRectangle(Pens.Gray, Handler.ClientRectangle);
        context.DrawText(text, DefaultFont, Brushes.Black, new PointF(10, 10));
        context.DrawImage(image, new PointF(0, 10 +
                context.MeasureText(text, DefaultFont).Height));
    }
}