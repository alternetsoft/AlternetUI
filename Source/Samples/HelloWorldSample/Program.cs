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

            var mainPanel = new StackPanel { Orientation = StackPanelOrientation.Horizontal };
            window.Controls.Add(mainPanel);

            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Width = 100
            };

            mainPanel.Controls.Add(leftPanel);

            var redButton = new Button();
            redButton.Text = "Red";
            leftPanel.Controls.Add(redButton);

            var greenButton = new Button();
            greenButton.Text = "Green";
            leftPanel.Controls.Add(greenButton);

            var blueButton = new Button();
            blueButton.Text = "Blue";
            leftPanel.Controls.Add(blueButton);

            var rightPanel = new StackPanel { Orientation = StackPanelOrientation.Vertical };
            mainPanel.Controls.Add(rightPanel);

            var b = new Button();
            b.Text = "BBB";
            rightPanel.Controls.Add(b);


            app.Run(window);
        }
    }
}