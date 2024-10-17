using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MultiDocs
{
    public partial class Main
    {
        public void Test()
        {
            TestString = "Hello World";
            Test1();
            SampleClass sample = new SampleClass();
            sample.TestInt = 11;
        }
    }
}
