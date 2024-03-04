#pragma warning disable
using ApiCommon;
using NativeApi.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    public class SplitterPanelEventData : NativeEventData
    {
        public int SashPosition;
        public int OldSize;
        public int NewSize;
        public int X;
        public int Y;
    }
}