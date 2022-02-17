using System;
using System.ComponentModel;
using Alternet.UI;

namespace HelloWorldSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Binding myBinding = new Binding("MyDataProperty") { Mode = BindingMode.TwoWay };
            //BindingOperations.SetBinding(myDataTextBox, TextBox.TextProperty, myBinding);

            DataContext = new MyData(DateTime.Now);
        }

        private void ShowDataButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("MyDataProperty: " + ((MyData)DataContext).MyDataProperty);
        }

        public class MyData : INotifyPropertyChanged
        {
            private string myDataProperty;

            public MyData(DateTime dateTime)
            {
                myDataProperty = "Last bound time was " + dateTime.ToLongTimeString();
            }

            public string MyDataProperty
            {
                get => myDataProperty;
                set
                {
                    myDataProperty = value;
                    OnPropertyChanged("MyDataProperty");
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}