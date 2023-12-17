#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Panel : Control
    {
        public bool WantChars { get; set; }
        public bool ShowVertScrollBar { get; set; }
        public bool ShowHorzScrollBar { get; set; }
        public bool ScrollBarAlwaysVisible { get; set; }
    }
}