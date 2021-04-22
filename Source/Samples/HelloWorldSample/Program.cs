using Alternet.UI;
using System;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        private static CustomDrawnControl? customDrawnControl;
        private static CustomCompositeControl? customCompositeControl;
        private static TextBox? textBox;

        private static Application? application;

        public static void Main1(string[] args)
        {
            application = new Application
            {
                VisualTheme = StockVisualThemes.GenericLight
            };

            var window = new Window();
            window.Title = "Alternet UI";

            var systemNativeButton = new Button
            {
                Text = "System Native",
                Margin = new Thickness(0, 0, 0, 5)
            };
            systemNativeButton.Click += (o, e) => SetVisualTheme(StockVisualThemes.Native);
            window.Children.Add(systemNativeButton);

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }

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

            CreateLeftPanel(mainPanel);
            CreateCenterPanel(mainPanel);
            CreateRightPanel(mainPanel);

            window.ResumeLayout();

            textBox!.Text = "Hello";

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }

        private static void CreateLeftPanel(StackPanel parent)
        {
            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Width = 100
            };

            parent.Children.Add(leftPanel);

            leftPanel.SuspendLayout();

            CreateColorButtons(leftPanel);

            leftPanel.ResumeLayout();
        }

        private static void CreateCenterPanel(StackPanel parent)
        {
            var centerPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10, 0, 0, 0)
            };

            parent.Children.Add(centerPanel);

            centerPanel.SuspendLayout();

            textBox = new TextBox();
            textBox.TextChanged += (o, e) => customDrawnControl?.SetText(textBox!.Text);
            centerPanel.Children.Add(textBox);

            customDrawnControl = new CustomDrawnControl
            {
                Width = 140,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 10)
            };

            centerPanel.Children.Add(customDrawnControl);

            CreateThemeButtons(centerPanel);

            centerPanel.ResumeLayout();
        }

        private static void CreateRightPanel(StackPanel parent)
        {
            var rightPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(10, 0, 0, 0)
            };

            parent.Children.Add(rightPanel);

            rightPanel.SuspendLayout();

            customCompositeControl = new CustomCompositeControl
            {
                Width = 140,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 10)
            };

            rightPanel.Children.Add(customCompositeControl);

            rightPanel.ResumeLayout();
        }

        private static void CreateColorButtons(Control parent)
        {
            var redButton = new Button
            {
                Text = "Red",
                Margin = new Thickness(0, 0, 0, 5)
            };
            redButton.Click += (o, e) => customDrawnControl?.SetColor(Color.Pink);
            parent.Children.Add(redButton);

            var greenButton = new Button
            {
                Text = "Green",
                Margin = new Thickness(0, 0, 0, 5)
            };
            greenButton.Click += (o, e) => customDrawnControl?.SetColor(Color.LightGreen);
            parent.Children.Add(greenButton);

            var blueButton = new Button
            {
                Text = "Blue",
                Margin = new Thickness(0, 0, 0, 5)
            };
            blueButton.Click += (o, e) => customDrawnControl?.SetColor(Color.LightBlue);
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
    }
}