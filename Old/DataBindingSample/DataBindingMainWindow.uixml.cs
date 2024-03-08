using System;
using System.ComponentModel;
using Alternet.UI;

namespace DataBindingSample
{
    public partial class DataBindingWindow : Window
    {
        public DataBindingWindow()
        {
            Icon = Application.DefaultIcon;

            InitializeComponent();

            //Binding myBinding = new Binding("MyDataProperty") { Mode = BindingMode.TwoWay };
            //BindingOperations.SetBinding(myDataTextBox, TextBox.TextProperty, myBinding);

            DataContext = new MyData(System.DateTime.Now);

            SetSizeToContent();

            LogControl.BindApplicationLog();
            LogControl.ContextMenu.Required();
        }

        private void ShowDataButton_Click(object? sender, EventArgs e)
        {
            Application.Log("MyDataProperty: " + ((MyData)DataContext!).MyDataProperty);
        }

        private void UpdateDataButton_Click(object? sender, EventArgs e)
        {
            ((MyData)DataContext!).MyDataProperty =
                "Last bound time was " + DateTime.Now.ToLongTimeString(); ;
        }

        public class MyData : INotifyPropertyChanged
        {
            private string myDataProperty;

            public MyData(System.DateTime dateTime)
            {
                myDataProperty = "Last bound time was " + dateTime.ToLongTimeString();
            }

            public string MyDataProperty
            {
                get => myDataProperty;
                set
                {
                    myDataProperty = value;
                    OnPropertyChanged(nameof(MyDataProperty));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}