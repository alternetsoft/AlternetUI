using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomControlsSample
{
    internal partial class MainWindow : Window
    {
        private readonly CustomColorPicker colorPicker;
        private readonly TicTacToeControl ticTacToe;

        public MainWindow()
        {
            InitializeComponent();

            colorPicker = new()
            {
                Value = Color.Red,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 5)
            };
            colorPickerStackPanel.Children.Add(colorPicker);

            ticTacToe = new()
            {
                Margin = new Thickness(10, 10, 10, 10)
            };
            ticTacToeStackPanel.Children.Add(ticTacToe);

            DataContext = new Data();

        }

        class Data : INotifyPropertyChanged
        {
            private int intValue = 60;

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
    }
}