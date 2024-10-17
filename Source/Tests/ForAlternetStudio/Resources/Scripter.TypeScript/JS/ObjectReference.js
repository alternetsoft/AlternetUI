///<reference path="clr.d.ts" />

var t;
var clickConnection;

function CatchButton() {
	RunButton.Text = "Catch me if you can";
}

function ChangeButtonLocation(obj, args) {
	var autoRand = new System.Random();
	var x = (Math.ceil(((35 * autoRand.Next(0, 10)) 
				+ 1)));
	var y = (Math.ceil(((15 * autoRand.Next(0, 10)) 
				+ 1)));
	RunButton.Location = new System.Drawing.Point(x, y);
}

function RunButtonClick() {
	t.Stop();
	RunButton.Text = "Test Button";
}

function Initialize () {
	t = new System.Windows.Forms.Timer();
	t.Interval = 1000;
	t.Tick.connect(ChangeButtonLocation);
	clickConnection = RunButton.Click.connect(function (o, e) { RunButtonClick(); });
	t.Start();
}

function Dispose() {
	clickConnection.disconnect();
	t.Dispose();
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