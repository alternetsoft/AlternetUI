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

            MyData myDataObject = new MyData(DateTime.Now);
            Binding myBinding = new Binding("MyDataProperty");
            myBinding.Source = myDataObject;
            BindingOperations.SetBinding(myDataTextBox, TextBox.TextProperty, myBinding);
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
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