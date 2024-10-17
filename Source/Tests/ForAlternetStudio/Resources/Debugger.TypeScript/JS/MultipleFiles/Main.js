///<reference path="clr.d.ts" />
///<reference path="testlibrary.d.ts" />

var x = 10;

function test(i)
{
    return "x" + i;
}

class Capitalizer {
    capitalize = ( i ) => i.toUpperCase();
}

function debuggerTest()
{
	secondFunction(10);

	System.Console.WriteLine("Hello from JS 1");
	System.Console.WriteLine("Hello from JS 2");
	System.Console.WriteLine("Hello from JS 3");

	var o = new TestLibrary.MyClass();
	var h = TestLibrary.MyClass.MyProperty;

	System.Console.WriteLine(h);

	var c = System.Console;
	c.WriteLine(test(10));

	var bb = System.Drawing.Brushes.AntiqueWhite;

	c.WriteLine(new Capitalizer().capitalize("hello"));

	var i = 0;
	while (true) {
		i++;
		i += 2;
		x++;
		if (i > 1000)
			break;
	}
}

debuggerTest();
