namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly ListBox listBox = new();
        
        public Form1()
        {
            InitializeComponent();

            
            listBox.Dock = DockStyle.Fill;
            this.Controls.Add(listBox);

            KeyDown += Form1_KeyDown;
            listBox.MouseMove += Form1_MouseMove;
            KeyPreview = true;
            listBox.Items.Add($"DoubleClickTime: {SystemInformation.DoubleClickTime}");
            listBox.Items.Add($"MouseWheelScrollLines: {SystemInformation.MouseWheelScrollLines}");
        }

        private void Form1_MouseMove(object? sender, MouseEventArgs e)
        {
            this.Text = $"{e.X}:{e.Y}";
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            listBox.Items.Add(e.KeyData.ToString());
        }
    }
}
