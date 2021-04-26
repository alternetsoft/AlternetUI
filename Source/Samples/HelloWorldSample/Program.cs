using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        private static CustomDrawnControl? customDrawnControl;
        private static CustomCompositeControl? customCompositeControl;
        private static TextBox? textBox;

        private static Application? application;

        private static CheckBox? allowCloseWindowCheckBox;

        [STAThread]
        public static void Main(string[] args)
        {
            application = new Application
            {
                VisualTheme = StockVisualThemes.GenericLight
            };

            var window = new Window
            {
                Size = new SizeF(600, 400),
                Title = "Alternet UI"
            };

            window.Closing += Window_Closing;

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

        private static void Window_Closing(object? sender, CancelEventArgs? e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            if (!allowCloseWindowCheckBox!.IsChecked)
            {
                MessageBox.Show("Closing the window is not allowed. Set the check box to allow.", "Closing Not Allowed");
                e.Cancel = true;
            }
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
            textBox.TextChanged += (o, e) => customDrawnControl!.Text = customCompositeControl!.Text = textBox!.Text;
            centerPanel.Children.Add(textBox);

            customDrawnControl = new CustomDrawnControl
            {
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
                Margin = new Thickness(0, 10, 0, 10)
            };

            rightPanel.Children.Add(customCompositeControl);

            rightPanel.ResumeLayout();
        }

        private static void CreateColorButtons(Control parent)
        {
            void SetColor(Color c) => customDrawnControl!.Color = customCompositeControl!.Color = c;

            var redButton = new Button
            {
                Text = "Red",
                Margin = new Thickness(0, 0, 0, 5)
            };
            redButton.Click += (o, e) => SetColor(Color.Pink);
            parent.Children.Add(redButton);

            var greenButton = new Button
            {
                Text = "Green",
                Margin = new Thickness(0, 0, 0, 5)
            };
            greenButton.Click += (o, e) => SetColor(Color.LightGreen);
            parent.Children.Add(greenButton);

            var blueButton = new Button
            {
                Text = "Blue",
                Margin = new Thickness(0, 0, 0, 5)
            };
            blueButton.Click += (o, e) => SetColor(Color.LightBlue);
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

        private static void CreateLeftPanel(StackPanel parent)
        {
            var leftPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
            };

            parent.Children.Add(leftPanel);

            leftPanel.SuspendLayout();

            CreateColorButtons(leftPanel);

            allowCloseWindowCheckBox = new CheckBox { Text = "Allow closing the window", IsChecked = true };
            leftPanel.Children.Add(allowCloseWindowCheckBox);

            leftPanel.ResumeLayout();
        }
    }
}