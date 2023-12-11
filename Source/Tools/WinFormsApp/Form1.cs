namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private ListBox listBox = new();
        
        public Form1()
        {
            InitializeComponent();

            
            listBox.Dock = DockStyle.Fill;
            this.Controls.Add(listBox);

            KeyDown += Form1_KeyDown;
            KeyPreview = true;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            listBox.Items.Add(e.KeyData.ToString());
        }
    }
}
