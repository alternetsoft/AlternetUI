#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Timer
    {
        public event EventHandler? Tick;

        public bool Enabled { get; set; }

        public int Interval { get; set; }

        public void Restart() { }

        public bool AutoReset { get; set; }
    }
}