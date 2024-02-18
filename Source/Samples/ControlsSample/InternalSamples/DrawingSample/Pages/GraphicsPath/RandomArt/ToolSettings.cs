using System;

namespace DrawingSample.RandomArt
{
    internal class ToolSettings
    {
        public Random Random { get; } = new Random(0);

        public double PartLength => 20;

        public double Jitter => 20;
    }
}