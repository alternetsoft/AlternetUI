using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly ListBox listBox = new();

        public Form1()
        {
            InitializeComponent();

            listBox.Dock = DockStyle.Bottom;
            listBox.Height = 200;
            Controls.Add(listBox);

            KeyDown += Form1_KeyDown;
            tabControl1.MouseMove += ListBox_MouseMove;
            KeyPreview = true;
            listBox.Items.Add($"DoubleClickTime: {SystemInformation.DoubleClickTime}");
            listBox.Items.Add($"MouseWheelScrollLines: {SystemInformation.MouseWheelScrollLines}");
            listBox.Items.Add($"Screen.Bounds: {Screen.AllScreens[0].Bounds}");

            StatusStrip bar = new();
        }

        private void ListBox_MouseMove(object? sender, MouseEventArgs e)
        {
            this.Text = $"{e.X}:{e.Y}, {e.Location}";
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            listBox.Items.Add(e.KeyData.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox.Items.Add(tabControl1.DisplayRectangle.ToString());
        }
    }
}
