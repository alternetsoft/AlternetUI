#nullable disable

using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListViewItemLabelEditEventData : NativeEventData
    {
        public long itemIndex;
        public string label;
        public bool editCancelled;
    }
}