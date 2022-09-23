using System;
using System.Threading;
using System.Threading.Tasks;
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

        private async void StartLongOperationButton_Click(object sender, System.EventArgs e)
        {
            startLongOperationButton.Enabled = false;
            await Task.Run(async () =>
            {
                const int Maximum = 50;
                Invoke(() => longOperationProgressBar.Maximum = Maximum);

                for (int i = 0; i < Maximum; i++)
                {
                    await Task.Delay(100);
                    BeginInvoke(() => longOperationProgressBar.Value = i);
                }
            });
            
            startLongOperationButton.Enabled = true;
            longOperationProgressBar.Value = 0;
        }
    }
}