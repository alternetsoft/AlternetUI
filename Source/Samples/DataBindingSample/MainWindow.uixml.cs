using System;
using System.ComponentModel;
using Alternet.UI;

namespace HelloWorldSample
{
    public partial class MainWindow : Window
    {
        private MyData myDataObject;

        public MainWindow()
        {
            InitializeComponent();

            myDataObject = new MyData(DateTime.Now);
            Binding myBinding = new Binding("MyDataProperty") { Mode = BindingMode.TwoWay };
            myBinding.Source = myDataObject;
            BindingOperations.SetBinding(myDataTextBox, TextBox.TextProperty, myBinding);
        }

        private void ShowDataButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("MyDataProperty: " + myDataObject.MyDataProperty);
        }

        public class MyData : INotifyPropertyChanged
        {
            private string myDataProperty;

            public MyData(DateTime dateTime)
            {
                myDataProperty = "Last bound time was " + dateTime.ToLongTimeString();
            }

            public String MyDataProperty
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