using System;
using System.Threading;
using System.Threading.Tasks;
using Alternet.UI;

namespace ThreadingSample
{
    public partial class ThreadingMainWindow : Window
    {
        private static int globalCounter;

        private readonly int counter;
        private readonly CancellationTokenSource tokenSource;

        private Thread? thread1;
        private Thread? thread2;

        public ThreadingMainWindow()
        {
            tokenSource = new CancellationTokenSource();

            counter = ++globalCounter;
            Icon = App.DefaultIcon;

            Disposed += MainWindow_Disposed;
            Closing += MainWindow_Closing;
            Closed += MainWindow_Closed;

            InitializeComponent();

            SetSizeToContent();

            StartCounterThreads();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            App.IdleLog($"ThreadingSample.MainWindow '{counter}' Closed");
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            App.IdleLog($"ThreadingSample.MainWindow '{counter}' Closing");
            EndCounterThread(ref thread1);
            EndCounterThread(ref thread2);
            tokenSource.Cancel();
        }

        private void MainWindow_Disposed(object? sender, EventArgs e)
        {
            App.IdleLog($"ThreadingSample.MainWindow '{counter}' Disposed");
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
                            App.IdleLog($"Thread {counter}.1: Form is already diposed");
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
                            App.IdleLog($"Thread {counter}.2: Form is already diposed");
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
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' awoken.");
            }
            catch (ThreadAbortException)
            {
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' aborted.");
            }
            finally
            {
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' executing finally block.");
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
                        App.IdleLog($"Form {counter} is already diposed");
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
                            App.IdleLog($"Form {counter} is already diposed");
                        }
                        else
                        {
                            longOperationProgressBar.Value = i;
                            logListBox.Log($"Performing operation {i} of {Maximum}");
                        }
                    });
                }
            }, ct);

            try
            {
                await task;
            }
            catch (OperationCanceledException ex)
            {
                App.IdleLog(
                    $"Long operation canceled with message: {ex.Message}");
            }

            startLongOperationButton.Enabled = true;
            longOperationProgressBar.Value = 0;
        }
    }
}