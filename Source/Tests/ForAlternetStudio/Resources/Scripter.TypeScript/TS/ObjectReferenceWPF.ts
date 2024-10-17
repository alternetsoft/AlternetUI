///<reference path="clr.d.ts" />

class Catcher
{
    public CatchButton() {
        RunButton.Content = host.toObject("Catch me if you can");
    }
    
    public ChangeButtonLocation(obj: System.Object, args: System.EventArgs) {
        let autoRand: System.Random = new System.Random();
        let x: number = (<number>(((35 * autoRand.Next(0, 10)) 
                    + 1)));
        let y: number = (<number>(((15 * autoRand.Next(0, 10)) 
                    + 1)));
        RunButton.Margin = new System.Windows.Thickness(x, y, 0, 0);
    }
    
    public RunButtonClick(sender: System.Object, args: System.EventArgs) {
        this.t.Stop();
        RunButton.Content = host.toObject("Test Button");
    }
    
    private t: System.Windows.Threading.DispatcherTimer;
    private clickConnection : any;
	
    public constructor () {
        this.t = new System.Windows.Threading.DispatcherTimer();
        this.t.Interval = System.TimeSpan.FromMilliseconds(1000);
        this.t.Tick.connect(this.ChangeButtonLocation);
        this.clickConnection = RunButton.Click.connect((o, e) => this.RunButtonClick(o,e));
        this.t.Start();
    }
    
    public Dispose() {
		this.clickConnection.disconnect();
		this.t.Stop();
    }
}

function RunMe()
{
	let f: Catcher = new Catcher();
	f.CatchButton();
	return f;
}