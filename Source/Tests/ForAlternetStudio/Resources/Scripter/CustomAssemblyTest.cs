using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExternalAssembly;

public class TestClass
{
    public void UseExternal()
    {
        CustomClass customClass = new CustomClass();
        customClass.TestMethod(1, true);
    }

    public static void Main()
    {
        TestClass F = new TestClass();
        F.UseExternal();
    }

    public TestClass()
    {
    }
}