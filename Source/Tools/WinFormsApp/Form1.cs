using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly ListBox listBox = new();
        private readonly Label labelForm = new();

        public Form1()
        {
            InitializeComponent();

            listBox.Dock = DockStyle.Fill;
            this.Controls.Add(listBox);

            var panelLeft = new Panel();
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Width = 35;
            this.Controls.Add(panelLeft);

            labelForm.Text = "label";
            labelForm.Dock = DockStyle.Top;
            this.Controls.Add(labelForm);

            var panelTop = new Panel();
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 30;
            this.Controls.Add(panelTop);

            KeyDown += Form1_KeyDown;
            listBox.MouseMove += ListBox_MouseMove;
            KeyPreview = true;
            listBox.Items.Add($"DoubleClickTime: {SystemInformation.DoubleClickTime}");
            listBox.Items.Add($"MouseWheelScrollLines: {SystemInformation.MouseWheelScrollLines}");

            MouseMove += Form1_MouseMove;

            StatusStrip bar = new();
        }

        private void Form1_MouseMove(object? sender, MouseEventArgs e)
        {
            labelForm.Text = $"{e.X}:{e.Y}, {e.Location}";
            labelForm.Refresh();
        }

        private void ListBox_MouseMove(object? sender, MouseEventArgs e)
        {
            this.Text = $"{e.X}:{e.Y}, {e.Location}";
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            listBox.Items.Add(e.KeyData.ToString());
        }
    }
}
