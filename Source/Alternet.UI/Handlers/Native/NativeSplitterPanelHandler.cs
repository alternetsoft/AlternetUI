using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    internal class NativeSplitterPanelHandler : ControlHandler
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

        public int SashSize
        {
            get
            {
                return NativeControl.SashSize;
            }

            set
            {
                NativeControl.SashSize = value;
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

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public new SplitterPanel Control => (SplitterPanel)(base.Control);

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

            NativeControl.SplitterSashPosChanging +=
                NativeControl_SplitterSashPosChanging;
            NativeControl.SplitterSashPosChanged +=
                NativeControl_SplitterSashPosChanged;
            NativeControl.Unsplit += NativeControl_Unsplit;
            NativeControl.SplitterDoubleClick += NativeControl_SplitterDoubleClick;
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
        }

        private void NativeControl_SplitterDoubleClick(object sender, EventArgs e)
        {
            Control.RaiseSplitterDoubleClick(e);
        }

        private void NativeControl_Unsplit(object sender, EventArgs e)
        {
            Control.RaiseUnsplit(e);
        }

        private void NativeControl_SplitterSashPosChanged(object sender, EventArgs e)
        {
            Control.RaiseSplitterMoved(e);
        }

        private void NativeControl_SplitterSashPosChanging(
            object sender,
            EventArgs e)
        {
            Control.RaiseSplitterMoving(e);
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