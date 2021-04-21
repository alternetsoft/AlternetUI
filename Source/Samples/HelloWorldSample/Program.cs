using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        private static MyCustomControl? customControl;
        private static TextBox? textBox;

        static Application? application;

        [STAThread]
        public static void Main(string[] args)
        {
            application = new Application
            {
                VisualTheme = StockVisualThemes.GenericLight
            };

            var window = new Window();
            window.Title = "Alternet UI";

            var mainPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Horizontal,
                Margin = new Thickness(10)
            };

            window.SuspendLayout();
            window.Children.Add(mainPanel);

            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Width = 100
            };

            mainPanel.Children.Add(leftPanel);

            leftPanel.SuspendLayout();

            CreateColorButtons(leftPanel);

            leftPanel.ResumeLayout();

            var rightPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10, 0, 0, 0)
            };

            mainPanel.Children.Add(rightPanel);

            rightPanel.SuspendLayout();

            textBox = new TextBox();
            textBox.TextChanged += (o, e) => customControl?.SetText(textBox!.Text);
            rightPanel.Children.Add(textBox);

            customControl = new MyCustomControl
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 10)
            };

            rightPanel.Children.Add(customControl);

            CreateThemeButtons(rightPanel);

            rightPanel.ResumeLayout();

            window.ResumeLayout();

            textBox.Text = "Hello";

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }

        private static void CreateColorButtons(Control parent)
        {
            var redButton = new Button
            {
                Text = "Red",
                Margin = new Thickness(0, 0, 0, 5)
            };
            redButton.Click += (o, e) => customControl?.SetColor(Color.Pink);
            parent.Children.Add(redButton);

            var greenButton = new Button
            {
                Text = "Green",
                Margin = new Thickness(0, 0, 0, 5)
            };
            greenButton.Click += (o, e) => customControl?.SetColor(Color.LightGreen);
            parent.Children.Add(greenButton);

            var blueButton = new Button
            {
                Text = "Blue",
                Margin = new Thickness(0, 0, 0, 5)
            };
            blueButton.Click += (o, e) => customControl?.SetColor(Color.LightBlue);
            parent.Children.Add(blueButton);
        }

        private static void CreateThemeButtons(Control parent)
        {
            var systemNativeButton = new Button
            {
                Text = "System Native",
                Margin = new Thickness(0, 0, 0, 5)
            };
            systemNativeButton.Click += (o, e) => SetVisualTheme(StockVisualThemes.Native);
            parent.Children.Add(systemNativeButton);

            var genericLightButton = new Button
            {
                Text = "Generic Light",
                Margin = new Thickness(0, 0, 0, 5)
            };
            genericLightButton.Click += (o, e) => SetVisualTheme(StockVisualThemes.GenericLight);
            parent.Children.Add(genericLightButton);
        }

        private static void SetVisualTheme(VisualTheme theme)
        {
            if (application == null)
                throw new Exception();
            application.VisualTheme = theme;
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