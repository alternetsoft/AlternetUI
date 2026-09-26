using Alternet.Winforms;

namespace WinformsSample;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        PictureBox pictureBox = new();
        PictureBox pictureBox2 = new();

        var svg1 = SkiaWinforms.CreateSvgBitmaps(Alternet.UI.KnownSvgImages.ImgAngleUp);
        var bitmap1 = svg1.ToNormalBitmap(32, 32, isDark: false);

        var svg2 = SkiaWinforms.CreateSvgBitmaps(Alternet.UI.KnownSvgImages.ImgAngleLeft);
        var bitmap2 = svg2.ToDisabledBitmap(32, 32, isDark: false);

        pictureBox.Image = bitmap1;
        pictureBox.Size = new Size(32, 32);
        this.Controls.Add(pictureBox);

        pictureBox2.Image = bitmap2;
        pictureBox2.Size = new Size(32, 32);
        pictureBox2.Location = new Point(128, 128);
        this.Controls.Add(pictureBox2);
    }
}
