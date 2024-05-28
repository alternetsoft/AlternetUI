using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxScrollBarHandler
        : WxControlHandler<ScrollBar, Native.ScrollBar>, IScrollBarHandler
    {
        public Action? Scroll
        {
            get => NativeControl.Scroll;
            set => NativeControl.Scroll = value;
        }

        public ScrollEventType EventTypeID
        {
            get
            {
                return (ScrollEventType)NativeControl.EventTypeID;
            }
        }

        public int EventOldPos
        {
            get
            {
                return NativeControl.EventOldPos;
            }
        }

        public int EventNewPos
        {
            get
            {
                return NativeControl.EventNewPos;
            }
        }

        public bool IsVertical
        {
            get
            {
                return NativeControl.IsVertical;
            }

            set
            {
                NativeControl.IsVertical = value;
            }
        }

        public int ThumbPosition
        {
            get
            {
                return NativeControl.ThumbPosition;
            }

            set
            {
                CheckDisposed();
                NativeControl.ThumbPosition = value;
            }
        }

        public int Range
        {
            get
            {
                return NativeControl.Range;
            }
        }

        public int ThumbSize
        {
            get
            {
                return NativeControl.ThumbSize;
            }
        }

        public int PageSize
        {
            get
            {
                return NativeControl.PageSize;
            }
        }

        public void SetScrollbar(
            int position,
            int thumbSize,
            int range,
            int pageSize,
            bool refresh = true)
        {
            NativeControl.SetScrollbar(
                position,
                thumbSize,
                range,
                pageSize,
                refresh);
        }

        internal override Native.Control CreateNativeControl()
        {
            var result = new Native.ScrollBar();
            return result;
        }
    }
}
