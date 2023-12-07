using Alternet.UI;

namespace MinMaster
{
    public partial class MainWindow : Window
    {
        private Alternet.UI.Timer? multipleTimer;
        private Alternet.UI.Timer? singleTimer;

        public MainWindow()
        {
            var control = new LogListBox
            {
                Parent = this,
            };

            control.BindApplicationLog();
            Application.Log($"The application started at {DateTime.Now:HH:mm:ss.fff}");

            SetTimer();
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            multipleTimer = new(2000);

            // Hook up the Elapsed event for the timer. 
            multipleTimer.Elapsed += OnMultipleTimedEvent;
            multipleTimer.AutoReset = true;
            multipleTimer.Enabled = true;
            multipleTimer.Disposed += MultipleTimer_Disposed;

            singleTimer = new(2000);
            singleTimer.Elapsed += OnSingleTimedEvent;
            singleTimer.AutoReset = false;
            singleTimer.Enabled = true;
            singleTimer.Disposed += SingleTimer_Disposed;
        }

        private void SingleTimer_Disposed(object? sender, EventArgs e)
        {
        }

        private void MultipleTimer_Disposed(object? sender, EventArgs e)
        {
        }

        private void OnSingleTimedEvent(Object source, ElapsedEventArgs e)
        {
            Application.Log($"Single timer: Elapsed event raised at {e.SignalTime:HH:mm:ss.fff}");
        }

        private void OnMultipleTimedEvent(Object source, ElapsedEventArgs e)
        {
            Application.Log($"Multiple timer: Elapsed event raised at {e.SignalTime:HH:mm:ss.fff}");
        }
    }
}
