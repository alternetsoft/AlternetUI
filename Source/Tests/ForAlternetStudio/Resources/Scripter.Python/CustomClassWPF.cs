using System.Windows;

namespace ExternalAssembly
{
    public class CustomClass
    {
        public void TestMethod(int firstParam, bool secondParam)
        {
            MessageBox.Show(string.Format("first param is {0}, second param is {1}", firstParam, secondParam));
        }

        public int TestProperty { get; set; }
    }
}
