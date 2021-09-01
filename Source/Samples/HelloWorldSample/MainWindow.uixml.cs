using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using Brush = Alternet.UI.Brush;
using Brushes = Alternet.UI.Brushes;

namespace HelloWorldSample
{
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //textBox.TextChanged += TextBox_TextChanged;
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

        protected override void OnClosing(WindowClosingEventArgs e)
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
            SetBrush(Brushes.Pink);
        }

        private void GreenButton_Click(object? sender, EventArgs e)
        {
            SetBrush(Brushes.LightGreen);
        }

        private void RedButton_Click(object? sender, EventArgs e)
        {
            SetBrush(Brushes.LightBlue);
        }

        private void SetBrush(Brush b) => customDrawnControl!.Brush = customCompositeControl!.Brush = b;

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