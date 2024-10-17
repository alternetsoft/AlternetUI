///<reference path="clr.d.ts" />

var x: number = 10;

function test(i: number): string
{
    return "x" + i;
}

interface MyInterface {
	myMethod();
}

enum MyEnum {
	One,
	Two
}

class Capitalizer {
    capitalize = ( i : string ) => i.toUpperCase();
	myMethod1(i : number, s : string) : number {
		return 0;
	}
	myMethod2() {
	}
}

class Class2 {
	myMethod() {
	}
}

function debuggerTest()
{
    secondFunction(10);
	var alphas;
	alphas = ["1", "2", "3", "4"];
	System.Console.WriteLine(alphas[0]);
	
    System.Console.WriteLine("Hello from JS 1");
    System.Console.WriteLine("Hello from JS 2");
    System.Console.WriteLine("Hello from JS 3");

    var o = new TestLibrary.MyClass();
    var h = TestLibrary.MyClass.MyProperty;

    System.Console.WriteLine(h);

    var c = System.Console;
    c.WriteLine(test(10));

    
    var keys = System.Windows.Forms.Control.ModifierKeys;
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
