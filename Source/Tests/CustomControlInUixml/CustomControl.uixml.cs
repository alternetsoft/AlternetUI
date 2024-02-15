using System;
using Alternet.UI;

namespace CustomControlInUixml
{
    public partial class CustomControl : Control
    {
        private int times;

        public CustomControl()
        {
            InitializeComponent();
        }

        public string SomeProp
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        private void Button1_Click(object? sender, EventArgs e)
        {
            times++;
            button1.Text = $"Clicked {times}";
        }
    }
}