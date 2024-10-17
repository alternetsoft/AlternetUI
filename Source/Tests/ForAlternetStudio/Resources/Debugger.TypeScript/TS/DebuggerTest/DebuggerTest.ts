///<reference path="clr.d.ts" />

var x: number = 10;
 
function test(i: number): string
{
	return "x" + i;
}

class Capitalizer {
	capitalize = ( i : string ) => i.toUpperCase();
}

function debuggerTest()
{
	System.Console.WriteLine("Hello from JS 1");
	System.Console.WriteLine("Hello from JS 2");
	System.Console.WriteLine("Hello from JS 3");
	//xx.Microsoft.Win32.

	var c = System.Console;
	c.WriteLine(test(10));

	var bb = System.Drawing.Brushes.AntiqueWhite;

	c.WriteLine(new Capitalizer().capitalize("hello"));

	var i : number = 0;
	while (true) {
		i++;
		i += 2;
		x++;
              if (i > 1000)
                break; 
	}
}

debuggerTest();
