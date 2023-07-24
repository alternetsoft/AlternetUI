#pragma warning disable
using ApiCommon;
using NativeApi.Api;
using System;

namespace NativeApi.Api
{
    public class SplitterPanel : Control
    {
        public static IntPtr CreateEx(long styles) => throw new Exception();

        public long Styles { get; set; }
        public int MinimumPaneSize { get; set; }
        public double SashGravity { get; set; }
        public int SashSize { get; set; }

        public int SplitMode { get; set; }

        public bool SashVisible { get; set; }

        public bool IsSplit { get; }

        public bool CanDoubleClick { get; set; }
        public bool CanMoveSplitter { get; set; }

        public int SashPosition { get; set; }
        public bool RedrawOnSashPosition { get; set; }

        public int DefaultSashSize { get; }
        public Control Control1 { get; }
        public Control Control2 { get; }
        public void Initialize (Control window) => throw new Exception();
        public bool Replace(Control winOld, Control winNew) 
            => throw new Exception();

        public bool SplitHorizontally(Control window1, Control window2, 
            int sashPosition) => throw new Exception();

        public bool SplitVertically(Control window1, Control window2,
            int sashPosition) => throw new Exception();
        public bool DoUnsplit(Control toRemove) => throw new Exception();

        public void UpdateSize() => throw new Exception();

        [NativeEvent(cancellable: true)]
        public event NativeEventHandler<SplitterPanelEventData>? 
            SplitterSashPosChanging
            { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? SplitterSashPosResize
        { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? SplitterSashPosChanged
            { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? Unsplit
            { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? SplitterDoubleClick
            { add => throw new Exception(); remove => throw new Exception(); }
    }
}


 