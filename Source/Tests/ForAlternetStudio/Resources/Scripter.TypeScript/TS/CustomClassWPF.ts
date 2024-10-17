///<reference path="clr.d.ts" />

namespace ExternalAssembly
{
	class CustomClass
	{
		TestMethod(firstParam: number, secondParam: boolean)
		{
			System.Windows.MessageBox.Show(System.String.Format("first param is {0}, second param is {1}", firstParam, secondParam));
		}

		TestProperty: number;
	}
}
