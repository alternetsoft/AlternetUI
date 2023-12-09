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

                dc.FillRectangle(Brushes.White, (0, 0, image.Width, image.Height));
                dc.DrawText("Hello text", font, Color.Black.AsBrush, (5, 5));
            }
        }
    }
}
