using System;
using System.Threading;
using Alternet.UI;

namespace ThreadingSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StartCounterThreads();
        }

        private void StartCounterThreads()
        {
            new Thread(() =>
            {
                for (int i = 0; ; i++)
                {
                    BeginInvoke(() => beginInvokeCounterLabel.Text = i.ToString());
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true }.Start();

            new Thread(() =>
            {
                for (int i = 0; ; i++)
                {
                    Invoke(() => invokeCounterLabel.Text = i.ToString());
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true }.Start();
        }
    }
}