using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ScrollBarHandler
        : NativeControlHandler<ScrollBar, Native.ScrollBar>, IScrollBarHandler
    {
        public ScrollBarHandler()
        {
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

        /// <summary>
        /// Logs scroll info.
        /// </summary>
        public void Log()
        {
            Application.Log(ToString());
            var position = $"Position: {NativeControl.ThumbPosition}";
            var thumbSize = $"ThumbSize: {NativeControl.ThumbSize}";
            var range = $"Range: {NativeControl.Range}";
            var pageSize = $"PageSize: {NativeControl.PageSize}";
            Application.Log($"Native ScrollBar: {position}, {thumbSize}, {range}, {pageSize}");
        }

        internal override Native.Control CreateNativeControl()
        {
            var result = new Native.ScrollBar();
            return result;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.Scroll = null;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.UpdateScrollInfo();
            NativeControl.Scroll = NativeControl_Scroll;
        }

        private void NativeControl_Scroll()
        {
            Control.RaiseScroll();
        }
    }
}
