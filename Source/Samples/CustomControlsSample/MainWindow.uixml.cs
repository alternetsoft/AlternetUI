using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomControlsSample
{
    internal partial class MainWindow : Window
    {
        private readonly Data data = new();
        private readonly CustomColorPicker colorPicker;
        private readonly TicTacToeControl ticTacToe;
        private readonly KnobControl knobControl;
        private readonly GaugeControl gaugeControl;
        private readonly ListBox logListBox;

        public MainWindow()
        {
            Name = "MainWindow";

            Icon = new("embres:CustomControlsSample.Sample.ico");

            DataContext = data;

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

            logListBox = new()
            {
                Margin = 5,
                SuggestedWidth = 150,
                SuggestedHeight = 100,
                Visible = false,
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
            data.PropertyChanged += Data_PropertyChanged;

            SuspendLayout();
            colorPickerStackPanel.Children.Add(colorPicker);
            ticTacToeStackPanel.Children.Add(ticTacToe);
            slidersStackPanel.Children.Add(knobControl);
            slidersStackPanel.Children.Add(gaugeControl);

            logListBox.Parent = ticTacToeStackPanel;

            ResumeLayout(true);
            this.SetSizeToContent();

            Application.Current.LogMessage += Current_LogMessage;            
        }

        private void Data_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            intValueLabel.Text = data.IntValue.ToString();
        }

        private void Current_LogMessage(object? sender, LogMessageEventArgs e)
        {
            logListBox.Add(e.Message!);
            logListBox.SelectLastItem();
        }

        public static bool DisableCustomColorPopup { get; set; } = false;

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