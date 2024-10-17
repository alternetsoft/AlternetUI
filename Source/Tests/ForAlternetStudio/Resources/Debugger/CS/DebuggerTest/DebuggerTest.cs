using System;
using System.Linq;
using System.Collections.Generic;

public class C
{
	public static int M() => 1;
	public static int M(int x) => x;

	public static int M1(int x) => x;

	public static int M2(int x, string s) => x + s.Length;
	
    public static int fi = 12;
    private static int fi_p = 11;

    public static double fd = 12;
    protected static double fd_pr = 12;
    public static float ff = 12;
    public static Version fv = new Version();
    public static string fs = "hello!";
    public static char fc = 'h';
	
	public static int pfi {get; set; } = 12;
    private static int pfi_p {get; set; } = 11;

    public static double pfd {get; set; } = 12;
    protected static double pfd_pr {get; set; } = 12;
    public static float pff {get; set; } = 12;
    public static Version pfv {get; set; } = new Version();
    public static string pfs {get; set; } = "hello!";
    public static char pfc {get; set; } = 'h';

}

namespace DebuggerTest
{
    class TestBase
    {
        int _hi = 88;

        string _baseS = null;
        DateTime BaseProp { get { return DateTime.Now; } }
    }

    class TestInstance : TestBase
    {
        int _hello = 77;

        public void g1<T>(T s, int a, Version v)
        {
        }

        int InstProp { get { return 11; } }

        int MyFunc()
        {

            return 1;
        }

        public string InstanceMethod(int xx)
        {
            int bb = xx + MyFunc();
            return bb.ToString();
        }

        public string InstanceMethod(int xx, int y)
        {
            int bb = xx + MyFunc() + y;
            return bb.ToString();
        }
    }

    public struct mystruct
    {
        public int i;
        public float f;

        public int IProp { get; set; }
    }

    public class mystruct1
    {
        public int i;
        public float f;
        public mystruct ms1;
    }

    class S
    {
        public static string M()
        {
            return "hello";
        }
    }

    class C1
    {
        public static int f = 23;
        
        public C1(int a, int b, string c)
        {
        }
    }

    public class Program
    {
        static Version v = new Version();

        static string MyFunc(string s)
        {
            return s;
        }

        static void f1(string s, int a, Version v, int k)
        {
        }

        static void f1(string s, int a, Version v)
        {
        }

        /// <summary>
        /// F1 hello
        /// </summary>
        static int f1(string s)
        {
            return 0;
        }

        static void g1<T>(T s, int a, Version v)
        {
        }

        static event EventHandler MyEvent;
        static void LambdaEventHandlerTest()
        {
            MyEvent += (o, e) =>
            {
                Console.WriteLine("xxx");
                if (MyEvent != null)
                    MyEvent(null, EventArgs.Empty);
            };
        }

        static void VariableCaptureTest(string arg)
        {
            var records = new List<string>();
            // This line is the problem. If you comment out this line, the variable gets updated
            if (!records.Any(x => x == arg))
            {
                // Set a breakpoint on this assignment
                // This will not change the value in Watch or Locals
                arg = "Bob";
            }
        }

        static IEnumerable<string> E()
        {
            yield return "e one";
            yield return "e two";
            yield return "e three";
        }

        static void CustomEvaluatorsTest()
        {
            var l = new List<string>(new[] { "one", "two", "three" });
            var e = E();
            var a = new[] { "a one", "a two", "a three" };
        }

        public class ChildrenVisibilityTestClassBase
        {
            public int BasePublicIntField = 1;
            private int basePrivateIntField = 33;
            protected string BaseProtectedStringProperty { get; set; }

            protected static double BaseProtectedStaticDoubleProperty { get; set; }
            public static double BasePublicStaticDoubleProperty { get; set; }
        }

        public class ChildrenVisibilityTestClass : ChildrenVisibilityTestClassBase
        {
            public int PublicIntField = 1;
            public int PublicIntProperty { get; set; }

            private int privateIntField = 33;
            protected string ProtectedStringProperty { get; set; }

            protected static double ProtectedStaticDoubleProperty { get; set; }
            public static double PublicStaticDoubleProperty { get; set; }
        }
        
        [STAThread]
        public static void Main(string[] args)
        {
            C.fi = 14;
            
            ChildrenVisibilityTestClassBase childrenVisibilityTestClass = new ChildrenVisibilityTestClass();

            CustomEvaluatorsTest();

            VariableCaptureTest("hi");
            LambdaEventHandlerTest();
            //            return;

            var list = new System.Collections.Generic.List<mystruct>();
            list.Add(new mystruct());
            list.Add(new mystruct());
            list.Add(new mystruct());

            if ((args != null) && (args.Length > 0))
                foreach (string str in args)
                    Console.WriteLine(str);

            var st = v.Major.ToString();
            st = MyFunc(v.Major.ToString());

            string mys = "hello";

            char cc = 't';

            int[] aaa = new int[] { 1, 2, 3 };
            mystruct ms = new mystruct();
            mystruct1 ms1 = new mystruct1();
            ms.i = 10;
            ms.IProp = 333;
            ms1.i = 11;
            ms1.ms1.i = 123;
            int i = 0;
            for (int j = 0; j < 10; j++)
            {
                i++;
            }

            while (true || i < 100)
            {
                Console.Write(System.IO.File.Exists("ddsd"));

                var tt = new TestInstance();
                tt.InstanceMethod(34);
                tt.InstanceMethod(34, 2);


                i++;
                X(i);

                var ttddd = System.Diagnostics.Debug.IndentSize;
                if (i % 100 == 0)
                    System.Diagnostics.Debug.WriteLine("> " + i);

                int fff = SecondClass.Func();
            }
        }

        static int MyProp { get { return 165; } }

        static int _sf = 77;

        static void X(int i)
        {

            int ii = 5;

            Y(10);
        }

        static void Y(int i)
        {
            int ff = 0;
        }

        public static int Test()
        {
            int gg = 0;

            return 63;
        }
    }
}
