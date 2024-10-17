using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class Catcher : System.ComponentModel.Component
{
    public void CatchButton()
    {
        ScriptGlobalClass.RunButton.Text = "Catch me if you can";
    }

    public void ChangeButtonLocation(object obj, EventArgs args)
    {
        Random autoRand = new Random();
        int x = (int)(35 * autoRand.Next(0, 10) + 1);
        int y = (int)(15 * autoRand.Next(0, 10) + 1);
        ScriptGlobalClass.RunButton.Location = new Point(x, y);
    }


    public void RunButtonClick(object obj, EventArgs args)
    {
        t.Stop();
        ScriptGlobalClass.RunButton.Text = "Test Button";
    }

    public static Catcher Main()
    {
        Catcher f = new Catcher();
        f.CatchButton();
        return f;
    }

    private Timer t;

    public Catcher()
    {
        t = new Timer();
        t.Interval = 1000;
        t.Tick += new EventHandler(ChangeButtonLocation);
        ScriptGlobalClass.RunButton.Click += new EventHandler(RunButtonClick);
        t.Start();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ScriptGlobalClass.RunButton.Click -= new EventHandler(RunButtonClick);
            t.Stop();
            t.Dispose();
        }
        base.Dispose(disposing);
    }

}