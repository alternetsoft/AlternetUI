///<reference path="clr.d.ts" />

function debuggerTest( x, y, z )
{
	System.Console.WriteLine( "Hello from JS 1" );
	System.Console.WriteLine( "Hello from JS 2" );
	System.Console.WriteLine( "Hello from JS 3" );

	var i = 0;
	while ( true )
	{
		i++;
		System.Console.WriteLine( "{0}", i );
	}
}

debuggerTest( 1, 2 );