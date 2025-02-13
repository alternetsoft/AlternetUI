using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloWorld.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindowCS.xaml
    /// </summary>
    public partial class MainWindowCS : Window
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                new Application().Run(new MainWindowCS());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
