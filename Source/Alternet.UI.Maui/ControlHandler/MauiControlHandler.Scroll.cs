using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiControlHandler
    {
        private ScrollBarInfo vertPositionInfo = new();
        private ScrollBarInfo horzPositionInfo = new();

        public ScrollBarInfo VertScrollBarInfo
        {
            get => vertPositionInfo;
            set => vertPositionInfo = value;
        }

        public ScrollEventType ScrollBarEvtKind { get; set; }

        public int ScrollBarEvtPosition { get; set; }

        public ScrollBarInfo HorzScrollBarInfo
        {
            get => horzPositionInfo;
            set => horzPositionInfo = value;
        }

        public virtual bool BindScrollEvents
        {
            get => true;

            set
            {
            }
        }

        public Action? VerticalScrollBarValueChanged { get; set; }

        public Action? HorizontalScrollBarValueChanged { get; set; }

        public virtual bool IsScrollable
        {
            get;
            set;
        }

        public virtual ScrollEventType GetScrollBarEvtKind()
        {
            return ScrollBarEvtKind;
        }

        public virtual int GetScrollBarEvtPosition()
        {
            return ScrollBarEvtPosition;
        }
    }
}
