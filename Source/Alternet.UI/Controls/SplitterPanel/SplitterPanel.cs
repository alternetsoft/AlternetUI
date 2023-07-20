using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public class SplitterPanel : Control
    {
        private Control? control1;
        private Control? control2;

        internal long Styles
        {
            get
            {
                return Handler.Styles;
            }

            set
            {
                Handler.Styles = value;
            }
        }

        public int MinimumPaneSize
        {
            get
            {
                return Handler.MinimumPaneSize;
            }

            set
            {
                Handler.MinimumPaneSize = value;
            }
        }

        public double SashGravity
        {
            get
            {
                return Handler.SashGravity;
            }

            set
            {
                Handler.SashGravity = value;
            }
        }

        public int SashSize
        {
            get
            {
                return Handler.SashSize;
            }

            set
            {
                Handler.SashSize = value;
            }
        }

        public bool SashVisible
        {
            get
            {
                return Handler.SashVisible;
            }

            set
            {
                Handler.SashVisible = value;
            }
        }

        public bool IsSplit
        {
            get
            {
                return Handler.IsSplit;
            }

        }

        public int SashPosition
        {
            get
            {
                return Handler.SashPosition;
            }

            set
            {
                Handler.SashPosition = value;
            }
        }

        public bool RedrawOnSashPosition
        {
            get
            {
                return Handler.RedrawOnSashPosition;
            }

            set
            {
                Handler.RedrawOnSashPosition = value;
            }
        }

        public int DefaultSashSize
        {
            get
            {
                return Handler.DefaultSashSize;
            }

        }

        public void UpdateSize()
        {
            Handler.UpdateSize();
        }

        internal new NativeSplitterPanelHandler Handler =>
            (NativeSplitterPanelHandler)base.Handler;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnChildInserted(int childIndex, Control childControl)
        {
            base.OnChildInserted(childIndex, childControl);
            int count = Children.Count;
            if (count > 2)
            {
                throw new Exception(
                    "More than two child controls are not allowed");
            }

            Control newControl1 = GetChildOrNull(0);
            Control newControl2 = GetChildOrNull(1);
        }

        public void Initialize(Control window)
        {
            Handler.NativeControl.Initialize(window.Handler.NativeControl);
        }

        public bool Replace(Control winOld, Control winNew)
        {
            return Handler.NativeControl.Replace(
                winOld.Handler.NativeControl,
                winNew.Handler.NativeControl);
        }

        /*
            The initial position of the sash. If this value is positive,
            it specifies the size of the upper pane. If it is negative,
            its absolute value gives the size of the lower pane.
            Finally, specify 0 (default) to choose the default position
            (half of the total window height).
         */
        public bool SplitHorizontally(int sashPosition = 0)
        {
            Control? window1 = GetChildOrNull(0);
            Control? window2 = GetChildOrNull(1);

            if(window1 == null || window2 == null)
                return false;

            Native.Control? nc1 = window1.Handler.NativeControl;
            Native.Control? nc2 = window2.Handler.NativeControl;

            if (nc1 == null || nc2 == null)
                return false;

            return Handler.NativeControl.SplitHorizontally(
                nc1,
                nc2,
                sashPosition);
        }

        public bool SplitVertically(int sashPosition = 0)
        {
            Control window1 = GetChildOrNull(0);
            Control window2 = GetChildOrNull(1);

            if (window1 == null || window2 == null)
                return false;

            return Handler.NativeControl.SplitVertically(
                window1.Handler.NativeControl,
                window2.Handler.NativeControl,
                sashPosition);
        }

        public bool DoUnsplit(Control toRemove)
        {
            return Handler.NativeControl.DoUnsplit(toRemove.Handler.NativeControl);
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(int childIndex, Control childControl)
        {
            base.OnChildRemoved(childIndex, childControl);
        }

        /// <inheritdoc />
        protected override ControlHandler CreateHandler()
        {
            return new NativeSplitterPanelHandler();
        }
    }
}
