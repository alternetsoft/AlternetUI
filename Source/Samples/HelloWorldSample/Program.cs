using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        class MyCustomControl : Control
        {
            public MyCustomControl()
            {
                UserPaint = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.DrawingContext.FillRectangle(e.Bounds, Color.LightYellow);
                e.DrawingContext.DrawRectangle(e.Bounds, Color.DarkRed);
                e.DrawingContext.DrawText("Hello!", new PointF(10, 10), Color.Blue);
            }
        }

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

            var customControl = new MyCustomControl
            {
                Width = 100,
                Height = 100,
            };

            rightPanel.Controls.Add(customControl);

            app.Run(window);
        }
    }
}