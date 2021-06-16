using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace HelloWorldSample
{
    internal class MainWindow : Window
    {
        private CheckBox allowCloseWindowCheckBox;
        private RadioButton option1RadioButton;
        private TextBox textBox;
        private CustomDrawnControl customDrawnControl;
        private CustomCompositeControl customCompositeControl;
        private Button redButton;
        private Button greenButton;
        private Button blueButton;
        private Button systemNativeButton;
        private Button genericLightButton;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("HelloWorldSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            allowCloseWindowCheckBox = (CheckBox)FindControl("allowCloseWindowCheckBox");
            option1RadioButton = (RadioButton)FindControl("option1RadioButton");
            textBox = (TextBox)FindControl("textBox");
            customDrawnControl = (CustomDrawnControl)FindControl("customDrawnControl");
            customCompositeControl = (CustomCompositeControl)FindControl("customCompositeControl");
            redButton = (Button)FindControl("redButton");
            greenButton = (Button)FindControl("greenButton");
            blueButton = (Button)FindControl("blueButton");
            systemNativeButton = (Button)FindControl("systemNativeButton");
            genericLightButton = (Button)FindControl("genericLightButton");

            textBox.TextChanged += TextBox_TextChanged;
            redButton.Click += RedButton_Click;
            greenButton.Click += GreenButton_Click;
            blueButton.Click += BlueButton_Click;
            systemNativeButton.Click += SystemNativeButton_Click;
            genericLightButton.Click += GenericLightButton_Click;
            option1RadioButton.CheckedChanged += Option1RadioButton_CheckedChanged;

            UpdateText();
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!allowCloseWindowCheckBox.IsChecked)
            {
                MessageBox.Show("Closing the window is not allowed. Set the check box to allow.", "Closing Not Allowed");
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void GenericLightButton_Click(object? sender, EventArgs e)
        {
            Application.Current.VisualTheme = StockVisualThemes.GenericLight;
        }

        private void SystemNativeButton_Click(object? sender, EventArgs e)
        {
            Application.Current.VisualTheme = StockVisualThemes.Native;
        }

        private void BlueButton_Click(object? sender, EventArgs e)
        {
            SetColor(Color.Pink);
        }

        private void GreenButton_Click(object? sender, EventArgs e)
        {
            SetColor(Color.LightGreen);
        }

        private void RedButton_Click(object? sender, EventArgs e)
        {
            SetColor(Color.LightBlue);
        }

        private void SetColor(Color c) => customDrawnControl!.Color = customCompositeControl!.Color = c;

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            customDrawnControl!.Text = customCompositeControl!.Text = textBox!.Text;
        }
    }
}