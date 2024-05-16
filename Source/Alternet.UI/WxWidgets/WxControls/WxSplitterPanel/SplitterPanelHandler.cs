using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Alternet.UI
{
    internal class SplitterPanelHandler : WxControlHandler
    {
        public new Native.SplitterPanel NativeControl =>
            (Native.SplitterPanel)base.NativeControl!;

        public long Styles
        {
            get
            {
                return NativeControl.Styles;
            }

            set
            {
                NativeControl.Styles = value;
            }
        }

        public int SplitMode
        {
            get
            {
                return NativeControl.SplitMode;
            }

            set
            {
                NativeControl.SplitMode = value;
            }
        }

        public int MinimumPaneSize
        {
            get
            {
                return NativeControl.MinimumPaneSize;
            }

            set
            {
                NativeControl.MinimumPaneSize = value;
            }
        }

        public double SashGravity
        {
            get
            {
                return NativeControl.SashGravity;
            }

            set
            {
                NativeControl.SashGravity = value;
            }
        }

        public bool SashVisible
        {
            get
            {
                return NativeControl.SashVisible;
            }

            set
            {
                NativeControl.SashVisible = value;
            }
        }

        public bool IsSplit
        {
            get
            {
                return NativeControl.IsSplit;
            }
        }

        public int SashPosition
        {
            get
            {
                return NativeControl.SashPosition;
            }

            set
            {
                NativeControl.SashPosition = value;
            }
        }

        public bool RedrawOnSashPosition
        {
            get
            {
                return NativeControl.RedrawOnSashPosition;
            }

            set
            {
                NativeControl.RedrawOnSashPosition = value;
            }
        }

        public int DefaultSashSize
        {
            get
            {
                return NativeControl.DefaultSashSize;
            }
        }

        public int SashSize
        {
            get
            {
                return NativeControl.SashSize;
            }
        }

        public new SplitterPanel Control => (SplitterPanel)base.Control;

        public bool CanDoubleClick
        {
            get
            {
                return NativeControl.CanDoubleClick;
            }

            set
            {
                NativeControl.CanDoubleClick = value;
            }
        }

        public bool CanMoveSplitter
        {
            get
            {
                return NativeControl.CanMoveSplitter;
            }

            set
            {
                NativeControl.CanMoveSplitter = value;
            }
        }

        public void UpdateSize()
        {
            NativeControl.UpdateSize();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeSplitterPanel(SplitterPanel.DefaultCreateStyle);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.SplitterSashPosChanging += NativeControl_SplitterSashPosChanging;
            NativeControl.SplitterSashPosChanged +=
                NativeControl_SplitterSashPosChanged;
            NativeControl.Unsplit += NativeControl_Unsplit;
            NativeControl.SplitterDoubleClick += NativeControl_SplitterDoubleClick;
            NativeControl.SplitterSashPosResize +=
                NativeControl_SplitterSashPosResize;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            NativeControl.SplitterSashPosChanging -=
               NativeControl_SplitterSashPosChanging;
            NativeControl.SplitterSashPosChanged -=
                NativeControl_SplitterSashPosChanged;
            NativeControl.Unsplit -= NativeControl_Unsplit;
            NativeControl.SplitterDoubleClick -= NativeControl_SplitterDoubleClick;
            NativeControl.SplitterSashPosResize -=
                NativeControl_SplitterSashPosResize;
        }

        private void NativeControl_SplitterSashPosResize(
            object? sender,
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SplitterPanelEventArgs ea = new(e);
            Control.RaiseSplitterResize(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        private void NativeControl_SplitterDoubleClick(
            object? sender,
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SplitterPanelEventArgs ea = new(e);
            Control.RaiseSplitterDoubleClick(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        private void NativeControl_Unsplit(
            object? sender,
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SplitterPanelEventArgs ea = new(e);
            Control.RaiseUnsplit(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        private void NativeControl_SplitterSashPosChanged(
            object? sender,
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SplitterPanelEventArgs ea = new(e);
            Control.RaiseSplitterMoved(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        private void NativeControl_SplitterSashPosChanging(
            object? sender,
            Native.NativeEventArgs<Native.SplitterPanelEventData> e)
        {
            SplitterPanelEventArgs ea = new(e);
            Control.RaiseSplitterMoving(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        public class NativeSplitterPanel : Native.SplitterPanel
        {
            public NativeSplitterPanel(SplitterPanelCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.SplitterPanel_CreateEx_((int)style));
            }
        }
    }
}