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

            var mainPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Horizontal,
                Margin = new Thickness(10)
            };

            window.SuspendLayout();
            window.Controls.Add(mainPanel);

            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Width = 100
            };

            mainPanel.Controls.Add(leftPanel);

            leftPanel.SuspendLayout();

            CreateButtons(leftPanel);

            leftPanel.ResumeLayout();

            var rightPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10, 0, 0, 0)
            };

            mainPanel.Controls.Add(rightPanel);

            rightPanel.SuspendLayout();

            textBox = new TextBox();
            textBox.TextChanged += (o, e) => customControl?.SetText(textBox!.Text);
            rightPanel.Controls.Add(textBox);

            customControl = new MyCustomControl
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0)
            };

            rightPanel.Controls.Add(customControl);
            rightPanel.ResumeLayout();

            window.ResumeLayout();

            textBox.Text = "Hello";

            app.Run(window);
        }

        private static void CreateButtons(Control parent)
        {
            var redButton = new Button
            {
                Text = "Red",
                Margin = new Thickness(0, 0, 0, 5)
            };
            redButton.Click += (o, e) => customControl?.SetColor(Color.Pink);
            parent.Controls.Add(redButton);

            var greenButton = new Button
            {
                Text = "Green",
                Margin = new Thickness(0, 0, 0, 5)
            };
            greenButton.Click += (o, e) => customControl?.SetColor(Color.LightGreen);
            parent.Controls.Add(greenButton);

            var blueButton = new Button
            {
                Text = "Blue",
                Margin = new Thickness(0, 0, 0, 5)
            };
            blueButton.Click += (o, e) => customControl?.SetColor(Color.LightBlue);
            parent.Controls.Add(blueButton);
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