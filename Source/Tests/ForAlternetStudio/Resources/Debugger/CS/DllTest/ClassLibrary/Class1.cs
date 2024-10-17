using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class HelloWorld
    {
        public void Display(string text)
        {
            System.Windows.Forms.MessageBox.Show("Hello World! " + text);
        }
    }
}
