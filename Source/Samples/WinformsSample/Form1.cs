using Alternet.Winforms;

namespace WinformsSample;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        PictureBox pictureBox = new();
        PictureBox pictureBox2 = new();

        var svg = Alternet.UI.KnownSvgImages.ImgAngleUp;
        var bitmap = svg.ToNormalBitmap(32, 32, isDark: false);

        var svg2 = Alternet.UI.KnownSvgImages.ImgAngleDown;
        var bitmap2 = svg2.ToDisabledBitmap(32, 32, isDark: false);

        pictureBox.Image = bitmap;
        this.Controls.Add(pictureBox);

        pictureBox2.Image = bitmap2;
        this.Controls.Add(pictureBox2);
    }
}
