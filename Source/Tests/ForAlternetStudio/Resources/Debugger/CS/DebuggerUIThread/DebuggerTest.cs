using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using static ScriptGlobalClass;

namespace DebuggerTest
{
    public class Program
    {
        public static void CalculatePI()
        {
            int iterations, k, s;
            resultLabel.Text = "Calculating...";
            Application.DoEvents();
            k = 1;
            s = 0;
            iterations = 10000;

            for (int i = 0; i < iterations; i++)
            {
                progressBar.Value = (int)(i / iterations * 100) + 1;
                Application.DoEvents();

                if (i % 2 == 0)
                {
                    s += 4 / k;
                }
                else
                {
                    s -= 4 / k;
                }

                k += 2;
            }

            resultLabel.Text = string.Format("PI approx. value: {0}", s);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            CalculatePI();
        }
    }
}
