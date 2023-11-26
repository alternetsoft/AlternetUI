using System;

namespace NativeApi.Api
{
    public class GroupBox : Control
    {
        public string? Title { get; set; }

        public int GetTopBorderForSizer() => default;
        public int GetOtherBorderForSizer() => default;
    }
}