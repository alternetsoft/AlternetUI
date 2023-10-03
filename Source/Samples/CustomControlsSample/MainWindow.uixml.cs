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
        private readonly KnobControl knobControl;
        private readonly GaugeControl gaugeControl;

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:CustomControlsSample.Sample.ico");

            DataContext = new Data();

            InitializeComponent();

            colorPicker = new()
            {
                Value = Color.Red,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 5)
            };

            ticTacToe = new()
            {
                Margin = new Thickness(10, 10, 10, 10)
            };

            knobControl = new()
            {
                Minimum = 0,
                Maximum = 100,
                Margin = new Thickness(0, 0, 5, 0),
            };

            gaugeControl = new()
            {
                Minimum = 0,
                Maximum = 100,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5,0,0,0),
            };

            Binding myBinding = new ("IntValue") 
            { 
                Mode = BindingMode.TwoWay,
            };            

            BindingOperations.SetBinding(
                gaugeControl, 
                GaugeControl.ValueProperty, 
                myBinding);
            BindingOperations.SetBinding(
                knobControl,
                KnobControl.ValueProperty,
                myBinding);

            SuspendLayout();
            colorPickerStackPanel.Children.Add(colorPicker);
            ticTacToeStackPanel.Children.Add(ticTacToe);
            slidersStackPanel.Children.Add(knobControl);
            slidersStackPanel.Children.Add(gaugeControl);
            ResumeLayout(true);
            this.SetSizeToContent();
            this.MinimumSize = this.Size;
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