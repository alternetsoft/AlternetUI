///<reference path="clr.d.ts" />
///<reference path="ExternalAssembly.d.ts" />

function UseExternal()
{
	var customClass = new ExternalAssembly.CustomClass();
	customClass.TestMethod(1, true);
}

UseExternal();