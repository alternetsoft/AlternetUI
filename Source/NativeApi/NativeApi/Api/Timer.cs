using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Timer
    {
        public event EventHandler? Tick { add => throw new Exception(); remove => throw new Exception(); }

        public bool Enabled { get; set; }

        public int Interval { get; set; }
    }
}