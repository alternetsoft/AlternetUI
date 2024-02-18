using System;
using System.Threading;
using System.Threading.Tasks;
using Alternet.UI;

namespace ThreadingSample
{
    public partial class MainWindow : Window
    {
        private static int globalCounter;
        private int counter;
        private Thread? thread1;
        private Thread? thread2;
        private CancellationTokenSource tokenSource;

        public MainWindow()
        {
            tokenSource = new CancellationTokenSource();

            counter = ++globalCounter;
            Icon = Application.DefaultIcon;

            Disposed += MainWindow_Disposed;
            Closing += MainWindow_Closing;
            Closed += MainWindow_Closed;

            InitializeComponent();

            SetSizeToContent();

            StartCounterThreads();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Application.IdleLog($"ThreadingSample.MainWindow '{counter}' Closed");
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            Application.IdleLog($"ThreadingSample.MainWindow '{counter}' Closing");
            EndCounterThread(ref thread1);
            EndCounterThread(ref thread2);
            tokenSource.Cancel();
        }

        private void MainWindow_Disposed(object? sender, EventArgs e)
        {
            Application.IdleLog($"ThreadingSample.MainWindow '{counter}' Disposed");
        }

        private void EndCounterThread(ref Thread? thread)
        {
            thread?.Interrupt();
            thread = null;
        }

        private void ThreadAction1()
        {
            void Fn()
            {
                for (int i = 0; ; i++)
                {
                    BeginInvoke(() =>
                    {
                        if (IsDisposed)
                        {
                            Application.IdleLog($"Thread {counter}.1: Form is already diposed");
                        }
                        else
                            beginInvokeCounterLabel.Text = i.ToString();
                    });
                    Thread.Sleep(1000);
                }
            }

            CallThreadAction(Fn);
        }

        private void ThreadAction2()
        {
            void Fn()
            {
                for (int i = 0; ; i++)
                {
                    Invoke(() =>
                    {
                        if (IsDisposed)
                        {
                            Application.IdleLog($"Thread {counter}.2: Form is already diposed");
                        }
                        else
                            invokeCounterLabel.Text = i.ToString(); 
                    });
                    Thread.Sleep(1000);
                }
            }

            CallThreadAction(Fn);
        }

        private void CallThreadAction(Action action)
        {
            try
            {
                action();
            }
            catch (ThreadInterruptedException)
            {
                Application.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' awoken.");
            }
            catch (ThreadAbortException)
            {
                Application.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' aborted.");
            }
            finally
            {
                Application.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' executing finally block.");
            }
        }

        private void StartCounterThread1()
        {
            thread1 = new Thread(ThreadAction1)
            {
                Name = "1",
                IsBackground = true,
            };

            thread1.Start();
        }

        private void StartCounterThread2()
        {
            thread2 = new Thread(ThreadAction2)
            {
                Name = "2",
                IsBackground = true,
            };

            thread2.Start();
        }

        private void StartCounterThreads()
        {
            StartCounterThread1();
            StartCounterThread2();
        }

        private async void StartLongOperationButton_Click(object? sender, EventArgs e)
        {
            startLongOperationButton.Enabled = false;
            CancellationToken ct = tokenSource.Token;

            var task = Task.Run(async () =>
            {
                const int Maximum = 150;
                Invoke(() =>
                {
                    if (IsDisposed)
                    {
                        Application.IdleLog($"Form {counter} is already diposed");
                    }
                    else
                        longOperationProgressBar.Maximum = Maximum;
                });

                for (int i = 0; i < Maximum; i++)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    await Task.Delay(100, ct);
                    BeginInvoke(() =>
                    {
                        if (IsDisposed)
                        {
                            Application.IdleLog($"Form {counter} is already diposed");
                        }
                        else
                            longOperationProgressBar.Value = i;
                    });
                }
            }, ct);

            try
            {
                await task;
            }
            catch (OperationCanceledException ex)
            {
                Application.IdleLog(
                    $"Long operation canceled with message: {ex.Message}");
            }

            startLongOperationButton.Enabled = true;
            longOperationProgressBar.Value = 0;
        }
    }
}