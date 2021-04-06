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

            var panel = new StackPanel();
            window.Controls.Add(panel);

            var button1 = new Button();
            button1.Text = "Button 1";
            button1.Click += Button_Click;
            panel.Controls.Add(button1);

            var button2 = new Button();
            button2.Text = "Button 2";
            button2.Click += Button_Click;
            panel.Controls.Add(button2);

            app.Run(window);
        }

        private static void Button_Click(object sender, EventArgs e) =>
            MessageBox.Show(((Button)sender).Text + " was pressed", "Alternet UI");
    }
}