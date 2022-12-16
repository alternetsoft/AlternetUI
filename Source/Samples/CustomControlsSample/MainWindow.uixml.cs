using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomControlsSample
{
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new Data();

            //UpdateText();
        }

        class Data : INotifyPropertyChanged
        {
            private int intValue;

            public event PropertyChangedEventHandler? PropertyChanged;

            public int IntValue
            {
                get => intValue;
                set
                {
                    intValue = value;
                    OnPropertyChanged();
                }
            }

            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            //if (allowCloseWindowCheckBox == null)
            //    return;

            //if (!allowCloseWindowCheckBox.IsChecked)
            //{
            //    MessageBox.Show("Closing the window is not allowed. Set the check box to allow.", "Closing Not Allowed");
            //    e.Cancel = true;
            //}

            base.OnClosing(e);
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
//            MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }

        private void BlueButton_Click(object? sender, EventArgs e)
        {
//            SetBrush(Brushes.LightBlue);
        }

        private void GreenButton_Click(object? sender, EventArgs e)
        {
  //          SetBrush(Brushes.LightGreen);
        }

        private void RedButton_Click(object? sender, EventArgs e)
        {
      //      SetBrush(Brushes.Pink);
        }

        //private void SetBrush(Brush b) => customDrawnControl!.Brush = /*customCompositeControl!.Brush =*/ b;

        private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            //UpdateText();
        }

        //private void UpdateText()
        //{
        //    if (customDrawnControl == null)
        //        return;

        //    customDrawnControl!.Text = /*customCompositeControl!.Text =*/ textBox!.Text;
        //}

        private void ColorPicker_ValueChanged(object sender, System.EventArgs e)
        {
            //MessageBox.Show("New Color Value: " + colorPicker.Value);
        }
    }
}