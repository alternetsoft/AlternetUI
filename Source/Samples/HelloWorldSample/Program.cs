using Alternet.UI;
using System;

namespace HelloWorldSample
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();
            var window = new Window();
            window.Title = "Alternet UI";

            var button1 = new Button();
            button1.Text = "Button 1";
            button1.Click += Button_Click;
            window.AddControl(button1);

            var button2 = new Button();
            button2.Text = "Button 2";
            button2.Click += Button_Click;
            window.AddControl(button2);

            window.AddControl(new StackLayoutPanel());

            app.Run(window);
        }

        private static void Button_Click(object sender, EventArgs e) =>
            MessageBox.Show(((Button)sender).Text + " was pressed", "Alternet UI");
    }
}