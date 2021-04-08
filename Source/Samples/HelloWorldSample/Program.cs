using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        private static MyCustomControl? customControl;
        private static TextBox? textBox;

        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();
            var window = new Window();
            window.Title = "Alternet UI";

            var mainPanel = new StackPanel { Orientation = StackPanelOrientation.Horizontal };
            window.SuspendLayout();
            window.Controls.Add(mainPanel);

            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Width = 100
            };

            mainPanel.Controls.Add(leftPanel);

            leftPanel.SuspendLayout();

            var redButton = new Button();
            redButton.Text = "Red";
            redButton.Click += (o, e) => customControl?.SetColor(Color.Pink);
            leftPanel.Controls.Add(redButton);

            var greenButton = new Button();
            greenButton.Text = "Green";
            greenButton.Click += (o, e) => customControl?.SetColor(Color.LightGreen);
            leftPanel.Controls.Add(greenButton);

            var blueButton = new Button();
            blueButton.Text = "Blue";
            blueButton.Click += (o, e) => customControl?.SetColor(Color.LightBlue);
            leftPanel.Controls.Add(blueButton);

            leftPanel.ResumeLayout();

            var rightPanel = new StackPanel { Orientation = StackPanelOrientation.Vertical };
            mainPanel.Controls.Add(rightPanel);

            rightPanel.SuspendLayout();

            textBox = new TextBox();
            textBox.TextChanged += (o, e) => customControl?.SetText(textBox!.Text);
            rightPanel.Controls.Add(textBox);

            customControl = new MyCustomControl
            {
                Width = 100,
                Height = 100,
            };

            rightPanel.Controls.Add(customControl);
            rightPanel.ResumeLayout();

            window.ResumeLayout();

            textBox.Text = "Hello";

            app.Run(window);
        }

        private class MyCustomControl : Control
        {
            private string text = "";
            private Color color = Color.LightGreen;

            public MyCustomControl()
            {
                UserPaint = true;
            }

            public void SetText(string value)
            {
                text = value;
                Update();
            }

            public void SetColor(Color value)
            {
                color = value;
                Update();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.DrawingContext.FillRectangle(e.Bounds, color);
                e.DrawingContext.DrawRectangle(e.Bounds, Color.Gray);
                e.DrawingContext.DrawText(text, new PointF(10, 10), Color.Black);
            }
        }
    }
}