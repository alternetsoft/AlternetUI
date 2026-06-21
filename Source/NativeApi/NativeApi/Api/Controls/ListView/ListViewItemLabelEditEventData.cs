#nullable disable

using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListViewItemLabelEditEventData : NativeEventData
    {
        public long itemIndex;
        public NativeStringSpan label;
        public bool editCancelled;
    }
}