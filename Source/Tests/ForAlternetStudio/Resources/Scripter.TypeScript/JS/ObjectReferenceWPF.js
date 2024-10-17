///<reference path="clr.d.ts" />

var t;
var clickConnection;

function CatchButton() {
    RunButton.Content = host.toObject("Catch me if you can");
}

function ChangeButtonLocation(obj, args) {
	var autoRand = new System.Random();
	var x = (Math.ceil(((35 * autoRand.Next(0, 10)) 
				+ 1)));
	var y = (Math.ceil(((15 * autoRand.Next(0, 10)) 
				+ 1)));
	RunButton.Margin = new System.Windows.Thickness(x, y, 0, 0);
}

function RunButtonClick() {
	t.Stop();
    RunButton.Content = host.toObject("Test Button");
}

function Initialize () {
    t = new System.Windows.Threading.DispatcherTimer();
    t.Interval = System.TimeSpan.FromMilliseconds(1000);
	t.Tick.connect(ChangeButtonLocation);
	clickConnection = RunButton.Click.connect(function (o, e) { RunButtonClick(); });
	t.Start();
}

function Dispose() {
	clickConnection.disconnect();
	t.Stop();
}

function RunMe()
{
	var f = {
		"Dispose": Dispose
	};
	CatchButton();
	Initialize();
	return f;
}