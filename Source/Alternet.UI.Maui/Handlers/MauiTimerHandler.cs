using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Alternet.UI
{
    internal partial class MauiTimerHandler : DisposableObject, ITimerHandler
    {
        private readonly IDispatcherTimer timer;

        public MauiTimerHandler()
        {
            timer = Application.Current?.Dispatcher.CreateTimer()!;
            timer.Tick += Timer_Tick;

            if (timer is null)
                throw new Exception("Error CreateTimer.");
        }

        public bool Enabled
        {
            get => timer.IsRunning;

            set
            {
                if (Enabled == value)
                    return;
                if (value)
                    timer.Start();
                else
                    timer.Stop();
            }
        }

        public int Interval
        {
            get => (int)timer.Interval.TotalMilliseconds;
            set => timer.Interval = TimeSpan.FromMilliseconds(value);
        }

        public Action? Tick { get; set; }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Tick?.Invoke();
        }
    }
}
