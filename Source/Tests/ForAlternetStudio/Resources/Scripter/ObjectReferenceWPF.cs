using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

public class Catcher : DependencyObject, IDisposable
{
    public void CatchButton()
    {
        ScriptGlobalClass.RunButton.Content = "Catch me if you can";
    }

    public void ChangeButtonLocation(object obj, EventArgs args)
    {
        Random autoRand = new Random();
        int x = (int)(30 * autoRand.Next(0, 10) + 1);
        int y = (int)(11 * autoRand.Next(0, 10) + 1);
        ScriptGlobalClass.RunButton.Margin = new Thickness(x, y, 0, 0);
    }


    public void RunButtonClick(object sender, RoutedEventArgs es)
    {
        t.Stop();
        ScriptGlobalClass.RunButton.Content = "Test Button";
    }

    public static Catcher Main()
    {
        Catcher f = new Catcher();
        f.CatchButton();
        return f;
    }

    private DispatcherTimer t;

    public Catcher()
    {
        t = new DispatcherTimer();
        t.Interval = TimeSpan.FromMilliseconds(1000);
        t.Tick += new EventHandler(ChangeButtonLocation);
        ScriptGlobalClass.RunButton.Click += RunButtonClick;
        t.Start();
    }

    public void Dispose()
    {
        t.Stop();
        ScriptGlobalClass.RunButton.Click -= RunButtonClick;
    }
}
