///<reference path="clr.d.ts" />

class Catcher
{
    public CatchButton() {
        RunButton.Text = "Catch me if you can";
    }
    
    public ChangeButtonLocation(obj: System.Object, args: System.EventArgs) {
        let autoRand: System.Random = new System.Random();
        let x: number = (<number>(((35 * autoRand.Next(0, 10)) 
                    + 1)));
        let y: number = (<number>(((15 * autoRand.Next(0, 10)) 
                    + 1)));
        RunButton.Location = new System.Drawing.Point(x, y);
    }
    
    public RunButtonClick() {
        this.t.Stop();
        RunButton.Text = "Test Button";
    }
    
    private t: System.Windows.Forms.Timer;
    private clickConnection : any;
	
    public constructor () {
        this.t = new System.Windows.Forms.Timer();
        this.t.Interval = 1000;
        this.t.Tick.connect(this.ChangeButtonLocation);
        this.clickConnection = RunButton.Click.connect((o, e) => this.RunButtonClick());
        this.t.Start();
    }
    
    public Dispose() {
		this.clickConnection.disconnect();
		this.t.Dispose();
    }
}

function RunMe()
{
	let f: Catcher = new Catcher();
	f.CatchButton();
	return f;
}