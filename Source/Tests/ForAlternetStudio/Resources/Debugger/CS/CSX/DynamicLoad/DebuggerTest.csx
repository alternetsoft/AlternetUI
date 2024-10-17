#load "ScriptToInclude.csx"

using System; 

class TestBase
{
	int _hi = 88; 

	string _baseS = null;
	DateTime BaseProp { get { return DateTime.Now; }}
}

class TestInstance : TestBase
{
	int _hello = 77; 
  
	
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
}

class S
{
	public static string M()
	{
		return "hello";
	}
}

public class Program
{
	static Version v = new Version();

	static string MyFunc(string s)
	{
		return s;
	}
	
	public static void Main() 
	{
		var st = 
		v.Major.ToString();
		st = MyFunc(v.Major.ToString());

	 	System.Diagnostics.Debug.WriteLine("version  " + st);
		string mys = "hello";

		char cc = 't';
		
		int[] aaa = new int [] {1, 2, 3};
		mystruct ms = new mystruct();
		mystruct1 ms1 = new mystruct1();
		ms.i=10;
		ms.IProp = 333;
		ms1.i = 11;
		int i = 0;
		for (int j = 0; j < 10; j++)
		{
			i++;
		}
		
		while(i < 100)
		{
			Console.Write(System.IO.File.Exists("ddsd"));
			
			var tt = new TestInstance();
			tt.InstanceMethod(34);
			
			//throw new System.Exception();
	        i++;
			X(i);
							
			int fff = ClassToInclude.GetNextInt();
		}
		System.Diagnostics.Debug.WriteLine("done");

	}
	
	static int MyProp { get { return 165; }}
	
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

Program.Main();

